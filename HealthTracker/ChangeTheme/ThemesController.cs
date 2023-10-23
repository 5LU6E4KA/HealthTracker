using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthTracker.ChangeTheme
{
    public static class ThemesController
    {
        public static ThemeTypes CurrentTheme { get; set; }

        public enum ThemeTypes
        {
            Classic, Modern
        }

        public static ResourceDictionary ThemeDictionary
        {
            get { return Application.Current.Resources.MergedDictionaries[0]; }
            set { Application.Current.Resources.MergedDictionaries[0] = value; }
        }

        private static void ChangeTheme(Uri uri)
        {
            ThemeDictionary = new ResourceDictionary() { Source = uri };
        }

        public static void SetTheme(ThemeTypes theme)
        {
            string themeName = null;
            CurrentTheme = theme;
            switch (theme)
            {
                case ThemeTypes.Modern: themeName = "ModernTheme"; break;
                case ThemeTypes.Classic: themeName = "ClassicTheme"; break;
            }

            try
            {
                if (!string.IsNullOrEmpty(themeName))
                    ChangeTheme(new Uri($"ChangeTheme/{themeName}.xaml", UriKind.Relative));
            }
            catch { }
        }
    }
}
