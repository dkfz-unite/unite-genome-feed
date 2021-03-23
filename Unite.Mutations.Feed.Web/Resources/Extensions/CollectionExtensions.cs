﻿using System;
using System.Collections.Generic;

namespace Unite.Mutations.Feed.Web.Resources.Extensions
{
    public static class CollectionExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }
    }
}