using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthTracker.ChangeTheme
{
    public static class Config
    {
        #region Fields
        private static readonly Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        #endregion Fields

        #region Properties
        public static string SelectedTheme
        {
            get => GetProperty<string>(nameof(SelectedTheme));
            set => SetProperty(nameof(SelectedTheme), value);
        }
        #endregion Properties

        #region Methods
        public static T GetProperty<T>(string propertyName)
        {
            return (T)Convert.ChangeType(_config.AppSettings.Settings[propertyName].Value, typeof(T));
        }

        public static void SetProperty(string propertyName, object value)
        {
            _config.AppSettings.Settings[propertyName].Value = value.ToString();
            _config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion Methods
    }
}
