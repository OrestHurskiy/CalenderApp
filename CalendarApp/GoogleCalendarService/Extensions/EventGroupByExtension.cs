﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleCalendarService.Extensions
{
    public static class EventGroupByExtension
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();

            return source.Where(element => seenKeys.Add(keySelector(element)));
        }
    }
}
