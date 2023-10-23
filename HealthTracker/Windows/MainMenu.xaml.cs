using HealthTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HealthTracker.ChangeTheme;

namespace HealthTracker.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }
        private void Themes_Click(object sender, RoutedEventArgs e)
        {
            if (Themes.IsChecked == true)
                ThemesController.SetTheme(ThemesController.ThemeTypes.Modern);
            else
                ThemesController.SetTheme(ThemesController.ThemeTypes.Classic);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void rdHome_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new BMIPage());
        }

        private void rdAnalytics_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new RecomendationPage());
        }

        private void rdMessages_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new MealPage());
        }

        private void rdCollections_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new VitalSignsPage());
        }

        private void rdUsers_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new SleepPage());
        }

        private void rdNotifications_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new DescriptionPage());
        }
    }
}
