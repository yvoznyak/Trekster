// <copyright file="Add_Transaction.xaml.cs" company="PlaceholderCompany">
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
    /// Class Add transaction window.
    /// </summary>
    public partial class Add_Transaction : Window
    {
        private readonly MainWindow mainWindow;
        private readonly Controller controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="Add_Transaction"/> class.
        /// </summary>
        /// <param name="w"> Window. </param>
        public Add_Transaction(MainWindow w)
        {
            this.InitializeComponent();

            this.mainWindow = w;

            this.controller = new Controller();

            this.add_transaction_account.ItemsSource = this.controller.Accounts_options();
            this.add_transaction_category.ItemsSource = this.controller.Categories_options();

            this.add_transaction_account.SelectedIndex = 0;
            this.add_transaction_category.SelectedIndex = 0;
        }

        /// <summary>
        /// PreviewText func.
        /// </summary>
        /// <param name="sender"> Object. </param>
        /// <param name="e"> Event. </param>
        public new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Fill dropdown list.
        /// </summary>
        /// <param name="sender"> Object. </param>
        /// <param name="e"> Event. </param>
        private void CbValueType_DropDownClosed(object sender, EventArgs e)
        {
            var x = this.add_transaction_account.SelectedItem.ToString();
            if (x != "Рахунок" && x is not null)
            {
                this.add_transaction_currency.ItemsSource = this.controller.Currencies_options(x);
                this.add_transaction_currency.SelectedIndex = 0;
            }
            else
            {
                this.add_transaction_currency.ItemsSource = new ObservableCollection<string>() { "Валюта" };
            }

            Program.Log.Info($"Page Add transaction closed.");
        }

        /// <summary>
        /// Save func.
        /// </summary>
        /// <param name="sender"> Object. </param>
        /// <param name="e"> Event. </param>
        private void Save_func(object sender, RoutedEventArgs e)
        {
            var account = this.add_transaction_account.Text;
            var category = this.add_transaction_category.Text;
            var sum = this.add_transaction_sum.Text;
            var currency = this.add_transaction_currency.Text;

            if (this.add_transaction_account.SelectedIndex == 0 || this.add_transaction_category.SelectedIndex == 0 || this.add_transaction_sum.Text == string.Empty || this.add_transaction_currency.SelectedIndex == 0)
            {
            }
            else
            {
                this.controller.Add_transaction(account, category, sum, currency);

                this.mainWindow.Renew();

                this.Close();
            }

            Program.Log.Info($"Page Add transaction closed.");
        }

        /// <summary>
        /// Cancel func.
        /// </summary>
        /// <param name="sender"> Object. </param>
        /// <param name="e"> Event. </param>
        private void Cancel_func(object sender, RoutedEventArgs e)
        {
            this.Close();

            Program.Log.Info($"Page Add transaction closed.");
        }
    }
}
