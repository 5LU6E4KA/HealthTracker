using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthTracker.ChangeTheme
{
    public static class ThemesController
    {
        public static ThemeTypes SelectedTheme { get; set; }

        public enum ThemeTypes
        {
            Classic, Modern
        }

        private static void ChangeTheme(Uri uri)
        {
            ResourceDictionary dictionary = (ResourceDictionary)Application.LoadComponent(resourceLocator: uri);
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(item: dictionary);

        }

        public static void SetTheme(ThemeTypes theme)
        {
            Config.SelectedTheme = theme.ToString();
            SelectedTheme = theme;

            try
            {
                ChangeTheme(new Uri($"/Resources/Themes/{SelectedTheme.ToString()}Theme.xaml", UriKind.Relative));
            }
            catch { }

        }
    }
}
