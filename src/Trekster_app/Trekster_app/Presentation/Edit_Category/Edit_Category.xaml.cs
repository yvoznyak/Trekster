// <copyright file="Edit_Category.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Trekster_app
{
    using System;
    using System.Windows;
    using Trekster_app.BLL;
    using Trekster_app.Presentation;

    /// <summary>
    /// Class edit category.
    /// </summary>
    public partial class Edit_Category : Window
    {
        private readonly MainWindow mainWindow;
        private readonly int id;
        private readonly Controller controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="Edit_Category"/> class.
        /// </summary>
        /// <param name="w"> Window. </param>
        /// <param name="name">Name. </param>
        public Edit_Category(MainWindow w, string name)
        {
            this.InitializeComponent();

            this.mainWindow = w;

            this.controller = new Controller();

            var id_str = name.Substring(20);

            this.id = Convert.ToInt32(id_str);
        }

        private void Save_func(object sender, RoutedEventArgs e)
        {
            var category_name = this.edit_account.Text;

            if (this.edit_account.Text == "Така назва вже існує")
            {
                this.edit_account.Text = "Введіть назву";
            }
            else if (this.controller.Category_name_exists(category_name))
            {
                this.edit_account.Text = "Така назва вже існує";
            }
            else
            {
                var type = 0;

                if (this.Profit.IsChecked == true)
                {
                    type = 1;
                }
                else
                {
                    type = -1;
                }

                this.controller.Edit_category(category_name, this.id, type);

                this.mainWindow.Renew();

                this.Close();

                Program.Log.Info($"Page Edit category closed.");
            }
        }

        private void Delete_func(object sender, RoutedEventArgs e)
        {
            this.controller.Delete_category(this.id);

            this.mainWindow.Renew();

            this.Close();

            Program.Log.Info($"Page Edit category closed.");
        }

        private void Cancel_func(object sender, RoutedEventArgs e)
        {
            this.Close();

            Program.Log.Info($"Page Edit category closed.");
        }
    }
}
