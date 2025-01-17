﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace server.Util
{
    public static class Extensions
    {
        public static string ToHexString(this byte[] byteArray)
        {
            return byteArray.Aggregate("", (current, b) => current + b.ToString("X2"));
        }

        public static string Capitalize(this string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static int EnsureMinValue(this int value)
        {
            return value < 0 ? 1 : value;
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> collection, int count)
        {
            return collection.Skip(Math.Max(0, collection.Count() - count));
        }
    }
}
