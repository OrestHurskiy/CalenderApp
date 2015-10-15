using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendarService.Helpers
{
    public static class AppSettingsHelper
    {
        public static string GetAppSetting(AppSetingsConst appSettingKey)
        {
            return ConfigurationManager.AppSettings[appSettingKey.ToString()];
        }

        public static void SetAppSetting(AppSetingsConst appSettingKey, string value)
        {
            ConfigurationManager.AppSettings.Add(appSettingKey.ToString(), value);
        }
    }

    public enum AppSetingsConst
    {
        MaxResults,
    };
}
