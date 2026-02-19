// <copyright file="Controller.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Trekster_app.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Class Controller.
    /// </summary>
    public class Controller
    {
        private List<Currency> currencies = new List<Currency>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Controller"/> class.
        /// </summary>
        public Controller()
        {
            var context = new TreksterDbContext();
            this.currencies = context.Currencies.ToList();
        }

        /// <summary>
        /// Get current balance.
        /// </summary>
        /// <returns> String with balances. </returns>
        public string Get_current_total()
        {
            var context = new TreksterDbContext();

            var str = "Загалом: ";

            var dct = new Dictionary<int, double>();

            var accounts = context.Accounts.ToList();
            var startbalances = context.Startbalances.ToList();
            var transactions = context.Transactions.ToList();
            var categories = context.Categories.ToList();

            foreach (var elem in this.currencies)
            {
                dct.Add(elem.Id, 0);
            }

            foreach (var account in accounts)
            {
                var account_balances = startbalances.Where(x => x.Idaccount == account.Id);
                var account_transactions = transactions.Where(x => x.Idaccount == account.Id);

                foreach (var elem in account_balances)
                {
                    var sum = elem.Sum;
                    var balance_transactions = account_transactions.Where(x => x.Idcurrency == elem.Idcurrency);
                    var currency = this.currencies.Where(x => x.Id == elem.Idcurrency).First().Id;

                    foreach (var transaction in balance_transactions)
                    {
                        var sign = categories.Where(x => x.Id == transaction.Idcategory).First().Type;
                        sum += transaction.Sum * sign;
                    }

                    dct[currency] += sum;
                }
            }

            foreach (var elem in this.currencies.Select(x => x.Id))
            {
                if (dct[elem] != 0)
                {
                    str += Math.Round(dct[elem], 3) + " " + this.currencies.Where(x => x.Id == elem).First().Name + ", ";
                }
            }

            str = str.Remove(str.Length - 1);
            str = str.Remove(str.Length - 1);

            return str;
        }

        /// <summary>
        /// Get percentage for expenses rectangle.
        /// </summary>
        /// <returns> Percentage. </returns>
        public double Get_expences_rectangle()
        {
            var context = new TreksterDbContext();

            var transactions = context.Transactions.ToList();
            var categories = context.Categories.ToList();

            var profit_category_ids = categories.Where(x => x.Type == 1).Select(x => x.Id).ToList();

            var profit = transactions.Where(x => profit_category_ids.Contains(x.Idcategory)).Select(x => x.Sum).Sum();
            var expences = transactions.Where(x => !profit_category_ids.Contains(x.Idcategory)).Select(x => x.Sum).Sum();

            if (expences == profit)
            {
                return 1;
            }
            else
            {
                return Convert.ToDouble(expences / profit);
            }
        }

        /// <summary>
        /// Add account to db.
        /// </summary>
        /// <param name="account_name"> Name of account.</param>
        /// <param name="dct"> Dictionary with start balances. </param>
        public void Add_account(string account_name, Dictionary<string, string> dct)
        {
            var context = new TreksterDbContext();

            var acc = new Account
            {
                Name = account_name,
            };

            context.Accounts.Add(acc);

            context.SaveChanges();

            Program.Log.Info($"Added new account: Name = {account_name}");

            var account_id = context.Accounts.Where(x => x.Name == account_name).First().Id;

            foreach (var elem in dct)
            {
                if (elem.Value != string.Empty)
                {
                    var start_balance = new Startbalance();

                    start_balance.Idcurrency = this.currencies.Where(x => x.Name == elem.Key).First().Id;

                    start_balance.Idaccount = account_id;

                    start_balance.Sum = float.Parse(elem.Value, CultureInfo.InvariantCulture.NumberFormat);

                    context.Startbalances.Add(start_balance);
                    context.SaveChanges();

                    Program.Log.Info($"Added new startbalance: Idcurrecy = {start_balance.Idcurrency}," +
                        $" Idaccount = {start_balance.Idaccount}," +
                        $" Sum = {start_balance.Sum}");
                }
            }
        }

        /// <summary>
        /// Get current balances for each account.
        /// </summary>
        /// <returns> String with balances. </returns>
        public Dictionary<int, string> Get_accounts_current_balances()
        {
            var context = new TreksterDbContext();

            var result = new Dictionary<int, string>();

            var accounts = context.Accounts.ToList();
            var startbalances = context.Startbalances.ToList();
            var transactions = context.Transactions.ToList();
            var categories = context.Categories.ToList();

            foreach (var account in accounts)
            {
                var str = account.Name + ": ";

                var account_balances = startbalances.Where(x => x.Idaccount == account.Id);
                var account_transactions = transactions.Where(x => x.Idaccount == account.Id);

                foreach (var elem in account_balances)
                {
                    var sum = elem.Sum;
                    var balance_transactions = account_transactions.Where(x => x.Idcurrency == elem.Idcurrency);
                    var currency = this.currencies.Where(x => x.Id == elem.Idcurrency).First().Name;

                    foreach (var transaction in balance_transactions)
                    {
                        var sign = categories.Where(x => x.Id == transaction.Idcategory).First().Type;
                        sum += transaction.Sum * sign;
                    }

                    str += Math.Round(sum, 3) + " " + currency + ", ";
                }

                str = str.Remove(str.Length - 1);
                str = str.Remove(str.Length - 1);

                result.Add(account.Id, str);
            }

            return result;
        }

        /// <summary>
        /// Func to check if account with this name exists.
        /// </summary>
        /// <param name="name"> Name to check. </param>
        /// <returns> Bool: exists or no. </returns>
        public bool Account_name_exists(string name)
        {
            var context = new TreksterDbContext();

            var accounts_names = context.Accounts.Select(x => x.Name).ToList();

            if (accounts_names.Contains(name))
            {
                Program.Log.Error($"Account {name} already exists.");
            }

            return accounts_names.Contains(name);
        }

        /// <summary>
        /// Func to check if category with this name exists.
        /// </summary>
        /// <param name="name"> Name to check. </param>
        /// <returns> Bool: exists or no. </returns>
        public bool Category_name_exists(string name)
        {
            var context = new TreksterDbContext();

            var categories_names = context.Categories.Select(x => x.Name).ToList();

            if (categories_names.Contains(name))
            {
                Program.Log.Error($"Category {name} already exists.");
            }

            return categories_names.Contains(name);
        }

        /// <summary>
        /// Func to get history of transcactions.
        /// </summary>
        /// <returns> Dictionary of transcations. </returns>
        public Dictionary<int, string> Get_history()
        {
            var context = new TreksterDbContext();

            var result = new Dictionary<int, string>();

            var accounts = context.Accounts.ToList();
            var currencies = context.Currencies.ToList();
            var transactions = context.Transactions.ToList();
            var categories = context.Categories.ToList();

            foreach (var transaction in transactions)
            {
                var transactionCurrency = currencies.Where(x => x.Id == transaction.Idcurrency).First().Name;
                var transactionCategory = categories.Where(x => x.Id == transaction.Idcategory).First().Name;
                var transactionAccount = accounts.Where(x => x.Id == transaction.Idaccount).First().Name;
                var transactionType = categories.Where(x => x.Id == transaction.Idcategory).First().Type;

                var sign = string.Empty;
                if (transactionType == 1)
                {
                    sign = "+";
                }
                else
                {
                    sign = "-";
                }

                var str = transaction.Date.ToString("MM.dd.yyyy") + ", " + transactionAccount + ", " + transactionCategory + ", " + sign + transaction.Sum + " " + transactionCurrency;

                result.Add(transaction.Id, str);
            }

            return result;
        }

        /// <summary>
        /// Func to get color of rectangle of transcation.
        /// </summary>
        /// <param name="id"> Id of transction. </param>
        /// <returns> List of RGB. </returns>
        public List<int> Get_transaction_color_by_id(int id)
        {
            var context = new TreksterDbContext();

            var result = new List<int>();

            var category_id = context.Transactions.Where(x => x.Id == id).First().Idcategory;
            var category_type = context.Categories.Where(x => x.Id == category_id).First().Type;

            if (category_type == 1)
            {
                result.Add(153);
                result.Add(217);
                result.Add(234);
            }
            else
            {
                result.Add(245);
                result.Add(133);
                result.Add(170);
            }

            return result;
        }

        /// <summary>
        /// Func to get options for account dropdown list.
        /// </summary>
        /// <returns> List of accounts names. </returns>
        public ObservableCollection<string> Accounts_options()
        {
            var context = new TreksterDbContext();

            var list = new ObservableCollection<string>();

            var accounts = context.Accounts.Select(x => x.Name).ToList();

            list.Add("Рахунок");
            foreach (var elem in accounts)
            {
                list.Add(elem);
            }

            return list;
        }

        /// <summary>
        /// Func to get options for categories dropdown list.
        /// </summary>
        /// <returns> List of categories names. </returns>
        public ObservableCollection<string> Categories_options()
        {
            var context = new TreksterDbContext();

            var list = new ObservableCollection<string>();

            var categories = context.Categories.Select(x => x.Name).ToList();

            list.Add("Категорія");
            foreach (var elem in categories)
            {
                list.Add(elem);
            }

            return list;
        }

        /// <summary>
        /// Func to get options for currencies dropdown list.
        /// </summary>
        /// <param name="account"> Name of account. </param>
        /// <returns> List of currencies names. </returns>
        public ObservableCollection<string> Currencies_options(string account)
        {
            var context = new TreksterDbContext();

            var list = new ObservableCollection<string>();

            var account_id = context.Accounts.Where(x => x.Name == account).First().Id;
            var currencies_ids = context.Startbalances.Where(x => x.Idaccount == account_id).Select(x => x.Idcurrency).ToHashSet();

            list.Add("Валюта");
            foreach (var elem in currencies_ids)
            {
                list.Add(this.currencies.Where(x => x.Id == elem).Select(x => x.Name).First());
            }

            return list;
        }

        /// <summary>
        /// Func to edit transaction in db.
        /// </summary>
        /// <param name="account"> Name of account. </param>
        /// <param name="category"> Name of category. </param>
        /// <param name="sum"> Sum of transaction. </param>
        /// <param name="currency"> Currency of transaction. </param>
        /// <param name="transaction_id"> Id of transaction. </param>
        public void Edit_transaction(string account, string category, string sum, string currency, int transaction_id)
        {
            var context = new TreksterDbContext();

            var transaction = context.Transactions.Where(x => x.Id == transaction_id).First();
            context.Transactions.Remove(transaction);

            var transaction_sum = float.Parse(sum, CultureInfo.InvariantCulture.NumberFormat);

            context.Remove(transaction);
            context.SaveChanges();

            if (sum == "0")
            {
                Program.Log.Info($"Deleted transaction: Id = {transaction_id}");

                return;
            }

            var new_transaction = new Transaction();

            var idaccount = context.Accounts.Where(x => x.Name == account).First().Id;
            var idcategory = context.Categories.Where(x => x.Name == category).First().Id;
            var idcurrency = context.Currencies.Where(x => x.Name == currency).First().Id;

            new_transaction.Idaccount = idaccount;
            new_transaction.Idcategory = idcategory;
            new_transaction.Idcurrency = idcurrency;
            new_transaction.Sum = transaction_sum;
            new_transaction.Date = DateTime.Now;

            context.Transactions.Add(new_transaction);

            Program.Log.Info($"Edited transaction: Idaccount = {idaccount}, Idcategory = {idcategory}," +
                $" Idcurrecy = {idcurrency}, Sum = {transaction_sum}, Date = {new_transaction.Date}.");

            context.SaveChanges();
        }

        /// <summary>
        /// Func to add transaction to db.
        /// </summary>
        /// <param name="account"> Name of account. </param>
        /// <param name="category"> Category of transaction. </param>
        /// <param name="sum"> Sum of transaction. </param>
        /// <param name="currency"> Currency of transaction. </param>
        public void Add_transaction(string account, string category, string sum, string currency)
        {
            var context = new TreksterDbContext();

            var transaction = new Transaction();

            transaction.Idaccount = context.Accounts.Where(x => x.Name == account).First().Id;
            transaction.Idcategory = context.Categories.Where(x => x.Name == category).First().Id;
            transaction.Idcurrency = context.Currencies.Where(x => x.Name == currency).First().Id;

            transaction.Sum = float.Parse(sum, CultureInfo.InvariantCulture.NumberFormat);
            transaction.Date = DateTime.Now;

            context.Add(transaction);
            context.SaveChanges();

            Program.Log.Info($"Added transaction: Idaccount = {transaction.Idaccount}, Idcategory = {transaction.Idcategory}," +
                $" Idcurrecy = {transaction.Idcurrency}, Sum = {transaction.Sum}, Date = {transaction.Date}.");
        }

        /// <summary>
        /// Func to get statistics of profits.
        /// </summary>
        /// <returns> Dictionary of strings. </returns>
        public Dictionary<int, string> Get_profits()
        {
            var context = new TreksterDbContext();

            var result = new Dictionary<int, string>();

            var accounts = context.Accounts.ToList();
            var transactions = context.Transactions.Where(x => x.Date.Month == DateTime.Now.Month).ToList();
            var categories = context.Categories.Where(x => x.Type == 1).ToList();

            foreach (var category in categories)
            {
                var dct = new Dictionary<int, float>();

                foreach (var currency in this.currencies)
                {
                    dct.Add(currency.Id, 0);
                }

                foreach (var transaction in transactions.Where(x => x.Idcategory == category.Id))
                {
                    dct[transaction.Idcurrency] += transaction.Sum;
                }

                var str = string.Empty;
                foreach (var elem in dct)
                {
                    if (elem.Value != 0)
                    {
                        var currency_name = this.currencies.Where(x => x.Id == elem.Key).First().Name;
                        str += elem.Value + " " + currency_name + ", ";
                    }
                }

                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Remove(str.Length - 1);
                    str = str.Remove(str.Length - 1);

                    result.Add(category.Id, category.Name + ": " + str);
                }
            }

            return result;
        }

        /// <summary>
        /// Func to get summary of profits.
        /// </summary>
        /// <returns> String. </returns>
        public string Get_profits_summary()
        {
            var context = new TreksterDbContext();

            var final_str = "Підсумок: ";

            var accounts = context.Accounts.ToList();
            var transactions = context.Transactions.Where(x => x.Date.Month == DateTime.Now.Month).ToList();
            var categories = context.Categories.Where(x => x.Type == 1).Select(x => x.Id).ToList();

            var dct = new Dictionary<int, float>();

            foreach (var currency in this.currencies)
            {
                dct.Add(currency.Id, 0);
            }

            foreach (var transaction in transactions)
            {
                if (categories.Contains(transaction.Idcategory))
                {
                    dct[transaction.Idcurrency] += transaction.Sum;
                }
            }

            foreach (var elem in dct)
            {
                if (elem.Value != 0)
                {
                    var currency_name = this.currencies.Where(x => x.Id == elem.Key).First().Name;
                    final_str += elem.Value + " " + currency_name + ", ";
                }
            }

            final_str = final_str.Remove(final_str.Length - 1);
            final_str = final_str.Remove(final_str.Length - 1);

            return final_str;
        }

        /// <summary>
        /// Func to get statistics of expenses.
        /// </summary>
        /// <returns> Dictionary of strings. </returns>
        public Dictionary<int, string> Get_expenses()
        {
            var context = new TreksterDbContext();

            var result = new Dictionary<int, string>();

            var accounts = context.Accounts.ToList();
            var transactions = context.Transactions.Where(x => x.Date.Month == DateTime.Now.Month).ToList();
            var categories = context.Categories.Where(x => x.Type == -1).ToList();

            foreach (var category in categories)
            {
                var dct = new Dictionary<int, float>();

                foreach (var currency in this.currencies)
                {
                    dct.Add(currency.Id, 0);
                }

                foreach (var transaction in transactions.Where(x => x.Idcategory == category.Id))
                {
                    dct[transaction.Idcurrency] += transaction.Sum;
                }

                var str = string.Empty;
                foreach (var elem in dct)
                {
                    if (elem.Value != 0)
                    {
                        var currency_name = this.currencies.Where(x => x.Id == elem.Key).First().Name;
                        str += elem.Value + " " + currency_name + ", ";
                    }
                }

                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Remove(str.Length - 1);
                    str = str.Remove(str.Length - 1);

                    result.Add(category.Id, category.Name + ": " + str);
                }
            }

            return result;
        }

        /// <summary>
        /// Func to get summary of expenses.
        /// </summary>
        /// <returns> String. </returns>
        public string Get_expenses_summary()
        {
            var context = new TreksterDbContext();

            var final_str = "Підсумок: ";

            var accounts = context.Accounts.ToList();
            var transactions = context.Transactions.Where(x => x.Date.Month == DateTime.Now.Month).ToList();
            var categories = context.Categories.Where(x => x.Type == -1).Select(x => x.Id).ToList();

            var dct = new Dictionary<int, float>();

            foreach (var currency in this.currencies)
            {
                dct.Add(currency.Id, 0);
            }

            foreach (var transaction in transactions)
            {
                if (categories.Contains(transaction.Idcategory))
                {
                    dct[transaction.Idcurrency] += transaction.Sum;
                }
            }

            foreach (var elem in dct)
            {
                if (elem.Value != 0)
                {
                    var currency_name = this.currencies.Where(x => x.Id == elem.Key).First().Name;
                    final_str += elem.Value + " " + currency_name + ", ";
                }
            }

            final_str = final_str.Remove(final_str.Length - 1);
            final_str = final_str.Remove(final_str.Length - 1);

            return final_str;
        }

        /// <summary>
        /// Func to get text for account block.
        /// </summary>
        /// <returns> Dictionary of strings. </returns>
        public Dictionary<int, string> Get_accounts_settings()
        {
            var context = new TreksterDbContext();

            var result = new Dictionary<int, string>();

            var accounts = context.Accounts.ToList();

            foreach (var account in accounts)
            {
                result.Add(account.Id, account.Name);
            }

            return result;
        }

        /// <summary>
        /// Func to delete account from db.
        /// </summary>
        /// <param name="id"> Id of account. </param>
        public void Delete_account(int id)
        {
            var context = new TreksterDbContext();

            var startbalances = context.Startbalances.Where(x => x.Idaccount == id).ToList();
            context.RemoveRange(startbalances);

            var transactions = context.Transactions.Where(x => x.Idaccount == id).ToList();
            context.RemoveRange(transactions);

            var account = context.Accounts.Where(x => x.Id == id).First();
            context.Remove(account);

            context.SaveChanges();

            Program.Log.Info($"Deleted account with id {id}.");
        }

        /// <summary>
        /// Func to edit account in db.
        /// </summary>
        /// <param name="name"> Name account. </param>
        /// <param name="id"> Id aof account. </param>
        /// <param name="dct"> Dct with start balances. </param>
        public void Edit_account(string name, int id, Dictionary<string, string> dct)
        {
            var context = new TreksterDbContext();

            var account = context.Accounts.Where(x => x.Id == id).First();
            var startbalances = context.Startbalances.Where(x => x.Idaccount == id).ToList();

            if (name != "Введіть назву")
            {
                account.Name = name;
            }

            foreach (var elem in dct)
            {
                if (elem.Value != string.Empty)
                {
                    var idcurrency = this.currencies.Where(x => x.Name == elem.Key).First().Id;

                    var start_balance = startbalances.Where(x => x.Idcurrency == idcurrency);

                    if (start_balance.Count() == 0)
                    {
                        var sb = new Startbalance();

                        sb.Idcurrency = idcurrency;

                        sb.Idaccount = id;

                        sb.Sum = float.Parse(elem.Value, CultureInfo.InvariantCulture.NumberFormat);

                        context.Startbalances.Add(sb);

                        context.SaveChanges();
                    }
                    else
                    {
                        start_balance.First().Sum = float.Parse(elem.Value, CultureInfo.InvariantCulture.NumberFormat);
                        context.SaveChanges();
                    }
                }
            }

            Program.Log.Info($"Edited account with id {id}.");

            context.SaveChanges();
        }

        /// <summary>
        /// Func to add to category to db.
        /// </summary>
        /// <param name="category_name"> Name of category. </param>
        /// <param name="type"> Type of category. </param>
        public void Add_category(string category_name, int type)
        {
            var context = new TreksterDbContext();

            var category = new Category
            {
                Name = category_name,
                Type = type,
            };

            context.Categories.Add(category);

            context.SaveChanges();

            Program.Log.Info($"Added category: Name = {category_name}, Type = {type}.");
        }

        /// <summary>
        /// Func to get text for setting block.
        /// </summary>
        /// <returns> Dictionary of strings. </returns>
        public Dictionary<int, string> Get_categories_settings()
        {
            var context = new TreksterDbContext();

            var result = new Dictionary<int, string>();

            var categories = context.Categories.ToList();

            foreach (var account in categories)
            {
                result.Add(account.Id, account.Name);
            }

            return result;
        }

        /// <summary>
        /// Func to get color of block for category.
        /// </summary>
        /// <param name="id"> Id of category. </param>
        /// <returns> List of RGB. </returns>
        public List<int> Get_category_color_by_id(int id)
        {
            var context = new TreksterDbContext();

            var result = new List<int>();

            var category_type = context.Categories.Where(x => x.Id == id).First().Type;

            if (category_type == 1)
            {
                result.Add(153);
                result.Add(217);
                result.Add(234);
            }
            else
            {
                result.Add(245);
                result.Add(133);
                result.Add(170);
            }

            return result;
        }

        /// <summary>
        /// Func to delete category from db.
        /// </summary>
        /// <param name="id"> Id of category. </param>
        public void Delete_category(int id)
        {
            var context = new TreksterDbContext();

            var transactions = context.Transactions.Where(x => x.Idcategory == id).ToList();
            context.RemoveRange(transactions);

            var category = context.Categories.Where(x => x.Id == id).First();
            context.Remove(category);

            context.SaveChanges();

            Program.Log.Info($"Deleted category with id {id}");
        }

        /// <summary>
        /// Func to edit category in db.
        /// </summary>
        /// <param name="name"> Name of category. </param>
        /// <param name="id"> Id of category. </param>
        /// <param name="type"> Type of category. </param>
        public void Edit_category(string name, int id, int type)
        {
            var context = new TreksterDbContext();

            var category = context.Categories.Where(x => x.Id == id).First();

            if (name != "Введіть назву")
            {
                category.Name = name;
            }

            category.Type = type;

            context.SaveChanges();

            Program.Log.Info($"Edited category: Id = {id}, Name = {name}, Type = {type}");
        }
    }
}
