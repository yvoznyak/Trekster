// <copyright file="Edit_Account.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Trekster_app
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Input;
    using Trekster_app.BLL;
    using Trekster_app.Presentation;

    /// <summary>
    /// Class edit account.
    /// </summary>
    public partial class Edit_Account : Window
    {
        private readonly MainWindow mainWindow;
        private readonly int id;
        private readonly Controller controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="Edit_Account"/> class.
        /// </summary>
        /// <param name="w"> Window. </param>
        /// <param name="name"> Name. </param>
        public Edit_Account(MainWindow w, string name)
        {
            this.InitializeComponent();

            this.mainWindow = w;

            this.controller = new Controller();

            var id_str = name.Substring(20);

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

        private void Save_func(object sender, RoutedEventArgs e)
        {
            var account = this.edit_account.Text;

            if (this.edit_account.Text == "Така назва вже існує")
            {
                this.edit_account.Text = "Введіть назву";
            }
            else if (this.controller.Account_name_exists(account))
            {
                this.edit_account.Text = "Така назва вже існує";
            }
            else
            {
                var dct = new Dictionary<string, string>();

                dct.Add("uah", this.uah.Text);
                dct.Add("usd", this.usd.Text);
                dct.Add("eur", this.eur.Text);
                dct.Add("usdt", this.usdt.Text);
                dct.Add("btc", this.btc.Text);

                this.controller.Edit_account(account, this.id, dct);

                this.mainWindow.Renew();

                this.Close();
            }

            Program.Log.Info($"Page Edit account closed.");
        }

        private void Delete_func(object sender, RoutedEventArgs e)
        {
            this.controller.Delete_account(this.id);

            this.mainWindow.Renew();

            this.Close();

            Program.Log.Info($"Page Edit account closed.");
        }

        private void Cancel_func(object sender, RoutedEventArgs e)
        {
            this.Close();

            Program.Log.Info($"Page Edit account closed.");
        }
    }
}
