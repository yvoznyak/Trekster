// <copyright file="Add_Category.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Trekster_app
{
    using System.Windows;
    using Trekster_app.BLL;
    using Trekster_app.Presentation;

    /// <summary>
    /// Class Add Category Window.
    /// </summary>
    public partial class Add_Category : Window
    {
        private readonly MainWindow mainWindow;
        private readonly Controller controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="Add_Category"/> class.
        /// </summary>
        /// <param name="w"> Window. </param>
        public Add_Category(MainWindow w)
        {
            this.InitializeComponent();
            this.mainWindow = w;
            this.controller = new Controller();
        }

        /// <summary>
        /// Add category func.
        /// </summary>
        /// <param name="sender"> Object. </param>
        /// <param name="e"> Event. </param>
        private void Add_func(object sender, RoutedEventArgs e)
        {
            var category_name = this.NameTextBox.Text;

            if (this.NameTextBox.Text == "Введіть назву")
            {
            }
            else if (this.NameTextBox.Text == "Така назва вже існує")
            {
                this.NameTextBox.Text = "Введіть назву";
            }
            else if (this.controller.Category_name_exists(category_name))
            {
                this.NameTextBox.Text = "Така назва вже існує";
            }
            else if (this.Profit.IsChecked == false && this.Expense.IsChecked == false)
            {
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

                this.controller.Add_category(category_name, type);

                this.mainWindow.Renew();

                this.Close();
            }

            Program.Log.Info($"Page Add category closed.");
        }

        /// <summary>
        /// Cancel func.
        /// </summary>
        /// <param name="sender"> Object. </param>
        /// <param name="e"> Event. </param>
        private void Cancel_func(object sender, RoutedEventArgs e)
        {
            this.Close();

            Program.Log.Info($"Page Add category closed.");
        }
    }
}
