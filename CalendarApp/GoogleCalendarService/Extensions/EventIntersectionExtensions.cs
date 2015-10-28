using Google.Apis.Calendar.v3.Data;

namespace GoogleCalendarService.Extensions
{
    public static class EventIntersectionExtensions
    {
        public static bool LeftIntersection(this Event source, Event target)
        {
            return ((source.Start.DateTime.Value.CompareTo(target.Start.DateTime.Value) < 0) &&
                    (source.End.DateTime.Value.CompareTo(target.Start.DateTime.Value) > 0));
        }

        public static bool RightIntersection(this Event source, Event target)
        {
            return ((source.Start.DateTime.Value.CompareTo(target.End.DateTime.Value) < 0) &&
                    (source.End.DateTime.Value.CompareTo(target.End.DateTime.Value) > 0));
        }

        public static bool InnerIntersection(this Event source, Event target)
        {
            return ((source.Start.DateTime.Value.CompareTo(target.Start.DateTime.Value) >= 0) &&
                    (source.End.DateTime.Value.CompareTo(target.End.DateTime.Value) <= 0));
        }

        public static bool OuterIntersection(this Event source, Event target)
        {
            return ((source.Start.DateTime.Value.CompareTo(target.Start.DateTime.Value) < 0) &&
                    (source.End.DateTime.Value.CompareTo(target.End.DateTime.Value) > 0));
        }
    }
}
