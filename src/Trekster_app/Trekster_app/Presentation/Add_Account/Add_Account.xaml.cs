// <copyright file="Add_Account.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Trekster_app
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Input;
    using Trekster_app.BLL;
    using Trekster_app.Presentation;

    /// <summary>
    /// Add account class.
    /// </summary>
    public partial class Add_Account : Window
    {
        private readonly Controller controller;
        private MainWindow mainWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="Add_Account"/> class.
        /// </summary>
        /// <param name="w"> Window. </param>
        public Add_Account(MainWindow w)
        {
            this.InitializeComponent();
            this.mainWindow = w;
            this.controller = new Controller();
        }

        /// <summary>
        /// Preview text input.
        /// </summary>
        /// <param name="sender"> Object. </param>
        /// <param name="e"> Event. </param>
        public new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Add func.
        /// </summary>
        /// <param name="sender"> Object. </param>
        /// <param name="e"> Event. </param>
        private void Add_func(object sender, RoutedEventArgs e)
        {
            var account_name = this.NameTextBox.Text;

            if (this.NameTextBox.Text == "Введіть назву")
            {
            }
            else if (this.NameTextBox.Text == "Така назва вже існує")
            {
                this.NameTextBox.Text = "Введіть назву";
            }
            else if (this.controller.Account_name_exists(account_name))
            {
                this.NameTextBox.Text = "Така назва вже існує";
            }
            else if (this.uah.Text == string.Empty &&
                this.usd.Text == string.Empty &&
                this.eur.Text == string.Empty &&
                this.usdt.Text == string.Empty &&
                this.btc.Text == string.Empty)
            {
            }
            else
            {
                var dct = new Dictionary<string, string>();

                dct.Add("uah", this.uah.Text);
                dct.Add("usd", this.usd.Text);
                dct.Add("eur", this.eur.Text);
                dct.Add("usdt", this.usdt.Text);
                dct.Add("btc", this.btc.Text);

                this.controller.Add_account(account_name, dct);

                this.mainWindow.Renew();

                this.Close();
            }

            Program.Log.Info($"Page Add account closed.");
        }

        /// <summary>
        /// Cancel. func.
        /// </summary>
        /// <param name="sender"> Object. </param>
        /// <param name="e"> Event. </param>
        private void Cancel_func(object sender, RoutedEventArgs e)
        {
            this.Close();

            Program.Log.Info($"Page Add account closed.");
        }
    }
}
