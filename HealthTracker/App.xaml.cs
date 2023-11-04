using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HealthTracker.ChangeTheme;

namespace HealthTracker
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ThemesController.ThemeTypes SelectedTheme;
            if (Config.SelectedTheme == "Classic") SelectedTheme = ThemesController.ThemeTypes.Classic;
            else SelectedTheme = ThemesController.ThemeTypes.Modern;
            ThemesController.SetTheme(SelectedTheme);
            base.OnStartup(e);
        }
    }
}
