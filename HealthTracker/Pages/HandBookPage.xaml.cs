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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
namespace HealthTracker.Pages
{
    /// <summary>
    /// Логика взаимодействия для HandBookPage.xaml
    /// </summary>
    public partial class HandBookPage : Page
    {
        public HandBookPage()
        {
            InitializeComponent();
        }
        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = (Hyperlink)sender;
            string navigateUri = hyperlink.NavigateUri.AbsoluteUri;

            try
            {
                Process.Start(new ProcessStartInfo(navigateUri));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при открытии браузера: " + ex.Message);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(e.Uri.AbsoluteUri);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при попытке открыть ссылку: " + ex.Message);
            }
            e.Handled = true;
        }
    }
}
