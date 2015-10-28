using System;
using GoogleCalendarService.Manager;

namespace NUnitTestProject.Helpers
{
    public static class RuleHelper
    {
        public static string GetRuleString(FrequencyEnumaretion frequencyEnumaretion, FrequencyType freqType, int count = 0, DateTime untilDateTime = default(DateTime), params DayOfWeek[] dayOfWeek)
        {
            string result = "RRULE:FREQ=" + frequencyEnumaretion.ToString()+";";

            switch (freqType)
            {
                case FrequencyType.COUNT:
                    result = result + "COUNT=" + count;
                    break;
                case FrequencyType.UNTIL:
                    result = result + "UNTIL=" + GetRfcFormatDate(untilDateTime);
                    break;
            }
            //If there no BYDAY tag count/until must be without ';'
            if (dayOfWeek.Length !=0)
            {
                result = result + ";BYDAY=";
                for (int i = 0; i < dayOfWeek.Length; i++)
                {
                    result = result + ToShortDayOfWeek(dayOfWeek[i]);
                    if (i == dayOfWeek.Length - 1)
                        continue;
                    else
                        result = result + ",";
                }
            }

            return result;
        }

        private static string GetRfcFormatDate(DateTime dateTime)
        {
            return dateTime.Year + dateTime.Month.ToString().PadLeft(2,'0') + dateTime.Day.ToString().PadLeft(2, '0') + "T" +
                   dateTime.Hour.ToString().PadLeft(2, '0') + dateTime.Minute.ToString().PadLeft(2, '0') + dateTime.Second.ToString().PadLeft(2, '0')+"Z";
        }

        private static string  ToShortDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "SU";
                case DayOfWeek.Monday:
                    return "MO";
                case DayOfWeek.Tuesday:
                    return "TU";
                case DayOfWeek.Wednesday:
                    return "WE";
                case DayOfWeek.Thursday:
                    return "TH";
                case DayOfWeek.Friday:
                    return "FR";
                case DayOfWeek.Saturday:
                    return "SA";
            }

            return null;
        }
    }
}
