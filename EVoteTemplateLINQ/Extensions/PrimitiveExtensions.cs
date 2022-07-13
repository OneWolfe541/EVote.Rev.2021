using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVote.Extensions
{
    public static class PrimitiveExtensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNull(this int? value)
        {
            if (value == null) return true;
            else return false;
        }

        public static bool IsNullOrZero(this int? value)
        {
            if (value == null || value == 0) return true;
            else return false;
        }

        public static bool IsNull(this float? value)
        {
            if (value == null) return true;
            else return false;
        }

        public static bool IsNull(this decimal? value)
        {
            if (value == null) return true;
            else return false;
        }

        public static bool IsNull(this double? value)
        {
            if (value == null) return true;
            else return false;
        }

        public static bool IsNull(this char? value)
        {
            if (value == null) return true;
            else return false;
        }

        public static bool IsNull(this byte? value)
        {
            if (value == null) return true;
            else return false;
        }

        public static bool IsNull(this short? value)
        {
            if (value == null) return true;
            else return false;
        }

        public static bool IsNull(this sbyte? value)
        {
            if (value == null) return true;
            else return false;
        }

        public static bool IsNull(this DateTime? value)
        {
            if (value == null) return true;
            else return false;
        }

        public static bool IsNull(this long? value)
        {
            if (value == null) return true;
            else return false;
        }
    }
}