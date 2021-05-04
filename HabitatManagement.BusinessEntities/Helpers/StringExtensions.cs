using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HabitatManagement.Business
{
    public static class StringExtensions
    {
        public static string ReplaceLastOccurrence(this string str, string find, string replace)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            int place = str.LastIndexOf(find);
            if (place == -1)
                return str;
            string result = str.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        public static bool EqualsIgnoreCase(this string str, string comp)
        {
            if (comp == null)
                return false;
            return str.Equals(comp, StringComparison.InvariantCultureIgnoreCase);
        }

        public static T ToEnum<T>(this string value, bool ignoreCase = true)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
    }
}
