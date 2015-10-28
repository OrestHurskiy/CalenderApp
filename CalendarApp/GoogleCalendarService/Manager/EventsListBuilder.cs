using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Google.Apis.Calendar.v3.Data;

namespace GoogleCalendarService.Manager
{
    public class EventsListBuilder
    {
        private FrequencyEnumaretion _frequencyEnumaretion;
        private Event _realEvent;
        private FrequencyType _freqType;
        private int _count;
        private DateTime _untilDateTime;
        private List<DayOfWeek> _dayOfWeeksList;
        private List<int> _dayJumpList;
        private List<Event> _generatedEvents; 

        public List<Event> Build()
        {
            return _generatedEvents;
        }

        public EventsListBuilder WithInsertedParameters()
        {
            DateTime startTime = _realEvent.Start.DateTime.Value;
            DateTime endTime = _realEvent.End.DateTime.Value;
            _generatedEvents = new List<Event>();
            int i = 0;

            while (true)
            {
                _generatedEvents.Add(new Event()
                {
                    Id = _realEvent.Id,
                    Start = new EventDateTime()
                    {
                        DateTime = startTime,
                    },
                    End = new EventDateTime()
                    {
                        DateTime = endTime,
                    }
                });

                switch (_frequencyEnumaretion)
                {
                    case FrequencyEnumaretion.DAILY:
                        startTime = startTime.AddDays(1);
                        endTime = endTime.AddDays(1);
                        break;
                    case FrequencyEnumaretion.WEEKLY:
                        startTime = startTime.AddDays(_dayJumpList[i]);
                        endTime = endTime.AddDays(_dayJumpList[i]);
                        break;
                    case FrequencyEnumaretion.MONTHLY:
                        int day = startTime.Day;
                        do
                        {
                            startTime = startTime.AddMonths(1);
                            endTime = endTime.AddMonths(1);

                        } while (day != startTime.Day);
                        break;
                    case FrequencyEnumaretion.YEARLY:
                        startTime = startTime.AddYears(1);
                        endTime = endTime.AddYears(1);
                        break;
                }
                //Need to break here becouse of i and startTime
                if (CheckForIterationBreak(i, startTime))
                    break;
                i++;
            }
            return this;
        }

        public EventsListBuilder SetFrequencyEnumaretion(string freqEnum)
        {
            switch (freqEnum)
            {
                case "DAILY": _frequencyEnumaretion = FrequencyEnumaretion.DAILY; break;
                case "WEEKLY": _frequencyEnumaretion = FrequencyEnumaretion.WEEKLY; break;
                case "MONTHLY": _frequencyEnumaretion = FrequencyEnumaretion.MONTHLY; break;
                case "YEARLY": _frequencyEnumaretion = FrequencyEnumaretion.YEARLY; break;
            }

            return this;
        }

        public EventsListBuilder SetSingleEvent(Event realEvent)
        {
            _freqType = FrequencyType.COUNT;
            _count = 1;
            _frequencyEnumaretion = FrequencyEnumaretion.DAILY;
            return this;
        }

        public EventsListBuilder SetRealEvent(Event realEvent)
        {
            _realEvent = realEvent;
            return this;
        }


        public EventsListBuilder SetCount(string count)
        {
            if (count != string.Empty)
            {
                _freqType = FrequencyType.COUNT;
                _count = Convert.ToInt32(count);
            }

            return this;
        }

        public EventsListBuilder SetUntil(string until)
        {
            if (until != string.Empty)
            {
                _freqType = FrequencyType.UNTIL;
                _untilDateTime = GetUntilDateTime(until);
            }

            return this;
        }

        public EventsListBuilder SetByDay(string byDay)
        {
            if(byDay != string.Empty)
                SetDayOfWeeksList(byDay);
            return this;
        }

        private void SetDayOfWeeksList(string byDay)
        {
            _dayOfWeeksList = byDay.Split(',').Select(ToDayOfWeek).ToList();
            _dayJumpList = GetDayJumpsList();
        }

        private DayOfWeek ToDayOfWeek(string day)
        {
            switch (day)
            {
                case "SU":
                    return DayOfWeek.Sunday;
                case "MO":
                    return DayOfWeek.Monday;
                case "TU":
                    return DayOfWeek.Tuesday;
                case "WE":
                    return DayOfWeek.Wednesday;
                case "TH":
                    return DayOfWeek.Thursday;
                case "FR":
                    return DayOfWeek.Friday;
                case "SA":
                    return DayOfWeek.Saturday;
                default:
                    return DayOfWeek.Sunday;
            }
        }
        //Method is used while building conflicted list for knowing where to stop creating conflict Events
        private bool CheckForIterationBreak(int i, DateTime startTime)
        {
            switch (_freqType)
            {
                case FrequencyType.COUNT:
                    return i == _count - 1;
                case FrequencyType.UNTIL:
                    return startTime.CompareTo(_untilDateTime) > 0;
                default:
                    return false;
            }
        }

        // Parsing RCF format to DateTime Format
        private DateTime GetUntilDateTime(string untilDate)
        {
            string pattern = @"(?<Year>\d{4})(?<Month>\d{2})(?<Day>\d{2})T(?<Hour>\d{2})(?<Minute>\d{2})(?<Second>\d{2})Z";
            var regex = new Regex(pattern);
            var match = regex.Match(untilDate);

            var hour = match.Groups["Hour"].Value == "00" ? 0 : Convert.ToInt32(match.Groups["Hour"].Value);
            var minute = match.Groups["Minute"].Value == "00" ? 0 : Convert.ToInt32(match.Groups["Minute"].Value);
            var second = match.Groups["Second"].Value == "00" ? 0 : Convert.ToInt32(match.Groups["Second"].Value);

            return new DateTime(
                Convert.ToInt32(match.Groups["Year"].Value),
                Convert.ToInt32(match.Groups["Month"].Value),
                Convert.ToInt32(match.Groups["Day"].Value),
                hour,
                minute,
                second
                );
        }

        // Method is used for geting a range between all day of the weeks set in the private _dayOfWeeksList
        private List<int> GetDayJumpsList()
        {
            var resultList = new List<int>();
            DateTime startTime = _realEvent.Start.DateTime.Value;

            var eventCountIterator = -1;
            //If there is only one day in weekList,range will be 7
            if (_dayOfWeeksList.Count == 1)
            {
                while (!CheckForIterationBreak(eventCountIterator, startTime))
                {
                    resultList.Add(7);
                    startTime = startTime.AddDays(7);
                    eventCountIterator++;
                }
                return resultList;
            }
            //start is a number of weeks day from which loop start to count a range to target weeks day
            for (int weekListIterator = 0, start = 0, target = 0;!CheckForIterationBreak(eventCountIterator, startTime); weekListIterator++)
            {
                eventCountIterator++;
                //If loop come to the end of dayofweekList
                if (weekListIterator == _dayOfWeeksList.Count - 1)
                {
                    start = (int) _dayOfWeeksList[weekListIterator];
                    target = (int) _dayOfWeeksList[0];
                    weekListIterator = -1;
                }
                else
                {
                    start = (int) _dayOfWeeksList[weekListIterator];
                    target = (int) _dayOfWeeksList[weekListIterator + 1];
                }

                var jump = 0;
                //If days are located in the same week
                if ((target - start) > 0)
                    resultList.Add(jump = target - start);
                else
                    resultList.Add(jump = (6 - start) + (target + 1));

                startTime = startTime.AddDays(jump);
            }

            return resultList;
        }
    }
}