using System.Configuration;
namespace BookingRoom.Helpers
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
            EmailService,
            Password,
            ApplicationName,
            LoggerName,
            MapPath,
            TestCalendar
    };
}
