// <copyright file="Edit_Transaction.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Trekster_app
{
    using System;
    using System.Collections.ObjectModel;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Input;
    using Trekster_app.BLL;
    using Trekster_app.Presentation;

    /// <summary>
    /// Class edit transaction window.
    /// </summary>
    public partial class Edit_Transaction : Window
    {
        private readonly Controller controller;
        private readonly MainWindow mainWindow;
        private readonly int id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Edit_Transaction"/> class.
        /// </summary>
        /// <param name="w"> Window. </param>
        /// <param name="name"> Name. </param>
        public Edit_Transaction(MainWindow w, string name)
        {
            this.InitializeComponent();

            this.mainWindow = w;

            this.controller = new Controller();

            this.edit_transaction_account.ItemsSource = this.controller.Accounts_options();
            this.edit_transaction_category.ItemsSource = this.controller.Categories_options();
            this.edit_transaction_account.SelectedIndex = 0;
            this.edit_transaction_category.SelectedIndex = 0;

            var id_str = name.Substring(12);

            this.id = Convert.ToInt32(id_str);
        }

        /// <summary>
        /// Preview text func.
        /// </summary>
        /// <param name="sender"> Object. </param>
        /// <param name="e"> Event. </param>
        public new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CbValueType_DropDownClosed(object sender, EventArgs e)
        {
            var x = this.edit_transaction_account.SelectedItem.ToString();
            if (x != "Рахунок" && x is not null)
            {
                this.edit_transaction_currency.ItemsSource = this.controller.Currencies_options(x);
                this.edit_transaction_currency.SelectedIndex = 0;
            }
            else
            {
                this.edit_transaction_currency.ItemsSource = new ObservableCollection<string>()
                {
                    "Валюта",
                };
            }
        }

        private void Save_func(object sender, RoutedEventArgs e)
        {
            var account = this.edit_transaction_account.Text;
            var category = this.edit_transaction_category.Text;
            var sum = this.edit_transaction_sum.Text;
            var currency = this.edit_transaction_currency.Text;

            if (this.edit_transaction_account.SelectedIndex == 0 ||
                this.edit_transaction_category.SelectedIndex == 0 ||
                this.edit_transaction_sum.Text == string.Empty ||
                this.edit_transaction_currency.SelectedIndex == 0)
            {
            }
            else
            {
                this.controller.Edit_transaction(account, category, sum, currency, this.id);

                this.mainWindow.Renew();

                this.Close();
            }

            Program.Log.Info($"Page Edit transaction closed.");
        }

        private void Cancel_func(object sender, RoutedEventArgs e)
        {
            this.Close();

            Program.Log.Info($"Page Edit transaction closed.");
        }
    }
}
