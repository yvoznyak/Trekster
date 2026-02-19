// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Trekster_app.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Xml.Linq;
    using Trekster_app.BLL;

    /// <summary>
    /// Class MainWindow.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SolidColorBrush chosen = new SolidColorBrush(Color.FromRgb(250, 186, 250));
        private readonly SolidColorBrush notChosen = new SolidColorBrush(Color.FromRgb(201, 135, 201));
        private readonly List<Border> blocks;
        private readonly List<Button> buttons;
        private readonly Controller controller;
        private readonly Duration openCloseDuration = new Duration(TimeSpan.FromSeconds(10));

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.controller = new Controller();

            this.blocks = new List<Border>()
            {
                this.Main_block,
                this.Accounts_block,
                this.History_block,
                this.Profits_block,
                this.Expences_block,
                this.Settings_block,
                this.Settings_accounts_block,
                this.Settings_categories_block,
            };
            this.buttons = new List<Button>()
            {
                this.main_button,
                this.accounts_button,
                this.history_button,
                this.expences_button,
                this.profits_button,
                this.settings_button,
            };

            this.main_button.Background = this.chosen;

            DateTime dt = DateTime.Now;

            this.month_expenses.Text = dt.ToString("MMMM");
            this.month_profits.Text = dt.ToString("MMMM");
            this.Renew();
        }

        /// <summary>
        /// Func to renew all pages.
        /// </summary>
        public void Renew()
        {
            this.Renew_accounts_stack();
            this.Renew_history_stack();
            this.Renew_total_balance();
            this.Renew_profits_stack();
            this.Renew_expenses_stack();
            this.Renew_accounts_settings_stack();
            this.Renew_categories_settings_stack();
            this.Renew_diagram();
        }

        /// <summary>
        /// Func to renew accounts stack.
        /// </summary>
        private void Renew_accounts_stack()
        {
            this.account_stack.Children.Clear();

            var data = this.controller.Get_accounts_current_balances();

            foreach (var x in data)
            {
                Border border = new Border();
                border.Background = new SolidColorBrush(Color.FromRgb(179, 219, 128));
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(4);
                border.Margin = new Thickness(25, 7, 25, 7);
                border.Height = 40;

                TextBlock txt = new TextBlock();
                txt.Foreground = Brushes.Black;
                txt.FontSize = 18;
                txt.FontWeight = FontWeights.Bold;
                txt.HorizontalAlignment = HorizontalAlignment.Left;
                txt.VerticalAlignment = VerticalAlignment.Center;
                txt.Margin = new Thickness(6, 0, 0, 0);
                txt.Text = x.Value;
                border.Child = txt;

                this.account_stack.Children.Add(border);
            }
        }

        /// <summary>
        /// Func to renew history stack.
        /// </summary>
        private void Renew_history_stack()
        {
            this.history_stack.Children.Clear();

            var data = this.controller.Get_history();

            foreach (var x in data)
            {
                var colors = this.controller.Get_transaction_color_by_id(x.Key);

                Border border = new Border();
                border.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(colors[0]), Convert.ToByte(colors[1]), Convert.ToByte(colors[2])));
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(4);
                border.Margin = new Thickness(25, 7, 25, 7);
                border.Height = 40;

                var panel = new WrapPanel();

                TextBlock txt = new TextBlock();
                txt.Foreground = Brushes.Black;
                txt.FontSize = 18;
                txt.FontWeight = FontWeights.Bold;
                txt.HorizontalAlignment = HorizontalAlignment.Left;
                txt.VerticalAlignment = VerticalAlignment.Center;
                txt.Margin = new Thickness(6, 0, 0, 0);
                txt.Text = x.Value;

                panel.Children.Add(txt);

                var edit_button = new Button();

                edit_button.Content = "Редагувати";
                edit_button.Name = "edit_button_" + x.Key;
                edit_button.Background = new SolidColorBrush(Color.FromRgb(219, 219, 219));
                edit_button.BorderBrush = Brushes.Black;
                edit_button.BorderThickness = new Thickness(3);
                edit_button.FontWeight = FontWeights.Bold;
                edit_button.FontSize = 18;
                edit_button.Width = 110;
                edit_button.Margin = new Thickness(10, 1, 0, 1);
                edit_button.Click += this.Edit_trans_func;

                panel.Children.Add(edit_button);

                border.Child = panel;

                this.history_stack.Children.Add(border);
            }
        }

        /// <summary>
        /// Func to renew total balance.
        /// </summary>
        private void Renew_total_balance()
        {
            this.total_balance.Text = this.controller.Get_current_total();
        }

        /// <summary>
        /// Func to renew accounts settings stack.
        /// </summary>
        private void Renew_accounts_settings_stack()
        {
            this.settings_accounts_stack.Children.Clear();

            var data = this.controller.Get_accounts_settings();

            foreach (var x in data)
            {
                Border border = new Border();
                border.Background = new SolidColorBrush(Color.FromRgb(179, 219, 128));
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(4);
                border.Margin = new Thickness(25, 7, 25, 7);
                border.Height = 40;

                var panel = new WrapPanel();

                TextBlock txt = new TextBlock();
                txt.Foreground = Brushes.Black;
                txt.FontSize = 18;
                txt.FontWeight = FontWeights.Bold;
                txt.HorizontalAlignment = HorizontalAlignment.Left;
                txt.VerticalAlignment = VerticalAlignment.Center;
                txt.Margin = new Thickness(6, 0, 0, 0);
                txt.Text = x.Value;

                panel.Children.Add(txt);

                var edit_button = new Button();
                edit_button.Content = "Редагувати";
                edit_button.Name = "edit_button_account_" + x.Key;
                edit_button.Background = new SolidColorBrush(Color.FromRgb(219, 219, 219));
                edit_button.BorderBrush = Brushes.Black;
                edit_button.BorderThickness = new Thickness(3);
                edit_button.FontWeight = FontWeights.Bold;
                edit_button.FontSize = 18;
                edit_button.Margin = new Thickness(5, 1, 0, 1);
                edit_button.Click += this.Edit_account_func;

                panel.Children.Add(edit_button);

                border.Child = panel;

                this.settings_accounts_stack.Children.Add(border);
            }
        }

        /// <summary>
        /// Func to renew categories settings stack.
        /// </summary>
        private void Renew_categories_settings_stack()
        {
            this.settings_categories_stack.Children.Clear();

            var data = this.controller.Get_categories_settings();

            foreach (var x in data)
            {
                var colors = this.controller.Get_category_color_by_id(x.Key);

                Border border = new Border();
                border.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(colors[0]), Convert.ToByte(colors[1]), Convert.ToByte(colors[2])));
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(4);
                border.Margin = new Thickness(25, 7, 25, 7);
                border.Height = 40;

                var panel = new WrapPanel();

                TextBlock txt = new TextBlock();
                txt.Foreground = Brushes.Black;
                txt.FontSize = 18;
                txt.FontWeight = FontWeights.Bold;
                txt.HorizontalAlignment = HorizontalAlignment.Left;
                txt.VerticalAlignment = VerticalAlignment.Center;
                txt.Margin = new Thickness(6, 0, 0, 0);
                txt.Text = x.Value;

                panel.Children.Add(txt);

                var edit_button = new Button();
                edit_button.Content = "Редагувати";
                edit_button.Name = "edit_button_account_" + x.Key;
                edit_button.Background = new SolidColorBrush(Color.FromRgb(219, 219, 219));
                edit_button.BorderBrush = Brushes.Black;
                edit_button.BorderThickness = new Thickness(3);
                edit_button.FontWeight = FontWeights.Bold;
                edit_button.FontSize = 18;
                edit_button.Margin = new Thickness(5, 1, 0, 1);
                edit_button.Click += this.Edit_category_func;

                panel.Children.Add(edit_button);

                border.Child = panel;

                this.settings_categories_stack.Children.Add(border);
            }
        }

        /// <summary>
        /// Func to renew diagram.
        /// </summary>
        private void Renew_diagram()
        {
            var k = this.controller.Get_expences_rectangle();

            if (k >= 1)
            {
                k = 0.999;
            }

            this.Expences_rectangle.Width = this.Expences_container.Width * k;
            this.Remains_rectangle.Width = this.Remains_container.Width - this.Expences_rectangle.Width;

            DoubleAnimation widthAnimation_1 = new DoubleAnimation(0, this.Expences_rectangle.Width, this.openCloseDuration);
            DoubleAnimation widthAnimation_2 = new DoubleAnimation(0, this.Remains_rectangle.Width, this.openCloseDuration);
            this.Expences_rectangle.BeginAnimation(HeightProperty, widthAnimation_1);
            this.Remains_rectangle.BeginAnimation(HeightProperty, widthAnimation_2);
        }

        /// <summary>
        /// Func to renew profits stack.
        /// </summary>
        private void Renew_profits_stack()
        {
            this.profits_stack.Children.Clear();

            var data = this.controller.Get_profits();

            foreach (var x in data)
            {
                Border border = new Border();
                border.Background = new SolidColorBrush(Color.FromRgb(179, 219, 128));
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(4);
                border.Margin = new Thickness(25, 7, 25, 7);
                border.Height = 40;

                TextBlock txt = new TextBlock();
                txt.Foreground = Brushes.Black;
                txt.FontSize = 18;
                txt.FontWeight = FontWeights.Bold;
                txt.HorizontalAlignment = HorizontalAlignment.Left;
                txt.VerticalAlignment = VerticalAlignment.Center;
                txt.Margin = new Thickness(6, 0, 0, 0);
                txt.Text = x.Value;

                border.Child = txt;

                this.profits_stack.Children.Add(border);
            }

            this.profits_summary.Text = this.controller.Get_profits_summary();
        }

        /// <summary>
        /// Func to renew expenses stack.
        /// </summary>
        private void Renew_expenses_stack()
        {
            this.expences_stack.Children.Clear();

            var data = this.controller.Get_expenses();

            foreach (var x in data)
            {
                Border border = new Border();
                border.Background = new SolidColorBrush(Color.FromRgb(179, 219, 128));
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(4);
                border.Margin = new Thickness(25, 7, 25, 7);
                border.Height = 40;

                TextBlock txt = new TextBlock();
                txt.Foreground = Brushes.Black;
                txt.FontSize = 18;
                txt.FontWeight = FontWeights.Bold;
                txt.HorizontalAlignment = HorizontalAlignment.Left;
                txt.VerticalAlignment = VerticalAlignment.Center;
                txt.Margin = new Thickness(6, 0, 0, 0);
                txt.Text = x.Value;

                border.Child = txt;

                this.expences_stack.Children.Add(border);
            }

            this.expenses_summary.Text = this.controller.Get_expenses_summary();
        }

        /// <summary>
        /// Func to change page.
        /// </summary>
        /// <param name="block"> Block. </param>
        /// <param name="button"> Button. </param>
        private void Changed_page(Border block, Button button)
        {
            this.Renew();

            foreach (var tmp in this.blocks)
            {
                if (tmp.Name != block.Name)
                {
                    tmp.Visibility = Visibility.Hidden;
                }
            }

            foreach (var tmp in this.buttons)
            {
                if (tmp.Name != button.Name)
                {
                    tmp.Background = this.notChosen;
                }
            }

            block.Visibility = Visibility.Visible;
            button.Background = this.chosen;

            Program.Log.Info($"Page {block.Name} opened.");
        }

        /// <summary>
        /// Func to open main window.
        /// </summary>
        /// <param name="sender"> Object. </param>
        /// <param name="e"> Event. </param>
        private void Main_func(object sender, RoutedEventArgs e)
        {
            this.Changed_page(this.Main_block, this.main_button);
        }

        /// <summary>
        /// Func to open accounts window.
        /// </summary>
        /// <param name="sender"> Object. </param>
        /// <param name="e"> Event. </param>
        private void Accounts_func(object sender, RoutedEventArgs e)
        {
            this.Changed_page(this.Accounts_block, this.accounts_button);
        }

        private void History_func(object sender, RoutedEventArgs e)
        {
            this.Changed_page(this.History_block, this.history_button);
        }

        private void Profits_func(object sender, RoutedEventArgs e)
        {
            this.Changed_page(this.Profits_block, this.profits_button);
        }

        private void Expences_func(object sender, RoutedEventArgs e)
        {
            this.Changed_page(this.Expences_block, this.expences_button);
        }

        private void Settings_func(object sender, RoutedEventArgs e)
        {
            this.Changed_page(this.Settings_block, this.settings_button);
        }

        private void Add_trans_func(object sender, RoutedEventArgs e)
        {
            Program.Log.Info($"Page Add transaction opened.");

            Add_Transaction transaction = new Add_Transaction(this)
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
            };
            transaction.Left = this.Left + (this.Width / 2) - (transaction.Width / 3.5);
            transaction.Top = this.Top + (this.Height / 2) - (transaction.Height / 2);
            transaction.ShowDialog();
        }

        private void Edit_trans_func(object sender, RoutedEventArgs e)
        {
            Program.Log.Info($"Page Edit transaction opened.");

            Edit_Transaction transaction = new Edit_Transaction(this, (sender as Button) !.Name)
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
            };
            transaction.Left = this.Left + (this.Width / 2) - (transaction.Width / 3.5);
            transaction.Top = this.Top + (this.Height / 2) - (transaction.Height / 2);
            transaction.ShowDialog();
        }

        private void Add_account_func(object sender, RoutedEventArgs e)
        {
            Program.Log.Info($"Page Add account opened.");

            Add_Account transaction = new Add_Account(this);
            transaction.WindowStartupLocation = WindowStartupLocation.Manual;
            transaction.Left = this.Left + (this.Width / 2) - (transaction.Width / 3.5);
            transaction.Top = this.Top + (this.Height / 2) - (transaction.Height / 2);
            transaction.ShowDialog();
        }

        private void Settings_accounts_func(object sender, RoutedEventArgs e)
        {
            this.Changed_page(this.Settings_accounts_block, this.settings_button);
        }

        private void Edit_account_func(object sender, RoutedEventArgs e)
        {
            Program.Log.Info($"Page Edit account opened.");

            Edit_Account transaction = new Edit_Account(this, (sender as Button) !.Name)
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
            };
            transaction.Left = this.Left + (this.Width / 2) - (transaction.Width / 3.5);
            transaction.Top = this.Top + (this.Height / 2) - (transaction.Height / 2);
            transaction.ShowDialog();
            this.Renew_accounts_settings_stack();
        }

        private void Settings_categories_func(object sender, RoutedEventArgs e)
        {
            this.Changed_page(this.Settings_categories_block, this.settings_button);
        }

        private void Add_category_func(object sender, RoutedEventArgs e)
        {
            Program.Log.Info($"Page Add category opened.");

            Add_Category transaction = new Add_Category(this);
            transaction.WindowStartupLocation = WindowStartupLocation.Manual;
            transaction.Left = this.Left + (this.Width / 2) - (transaction.Width / 3.5);
            transaction.Top = this.Top + (this.Height / 2) - (transaction.Height / 2);
            transaction.ShowDialog();
        }

        private void Edit_category_func(object sender, RoutedEventArgs e)
        {
            Program.Log.Info($"Page Edit category opened.");

            Edit_Category transaction = new Edit_Category(this, (sender as Button) !.Name)
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
            };
            transaction.Left = this.Left + (this.Width / 2) - (transaction.Width / 3.5);
            transaction.Top = this.Top + (this.Height / 2) - (transaction.Height / 2);
            transaction.ShowDialog();
            this.Renew_accounts_settings_stack();
        }
    }
}
