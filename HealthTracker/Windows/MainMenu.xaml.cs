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
using HealthTracker.Pages;

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
            if (ThemesToggleButton.IsChecked == true)
                ThemesController.SetTheme(ThemesController.ThemeTypes.Modern);
            else
                ThemesController.SetTheme(ThemesController.ThemeTypes.Classic);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
            => Close();

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void RadioButtonBMI_Click(object sender, RoutedEventArgs e)
            => frameContent.Navigate(new BMIPage());

        private void RadioButtonRecomendation_Click(object sender, RoutedEventArgs e)
            => frameContent.Navigate(new RecomendationPage());

        private void RadioButtonMeal_Click(object sender, RoutedEventArgs e)
            => frameContent.Navigate(new MealPage());

        private void RadioButtonVitalSigns_Click(object sender, RoutedEventArgs e)
            => frameContent.Navigate(new VitalSignsPage());

        private void RadioButtonSleep_Click(object sender, RoutedEventArgs e) 
            => frameContent.Navigate(new SleepPage());

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ThemesToggleButton.IsChecked = ThemesController.SelectedTheme == ThemesController.ThemeTypes.Modern;
        }

        private void InformationButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ОТдых");
        }
    }
}
