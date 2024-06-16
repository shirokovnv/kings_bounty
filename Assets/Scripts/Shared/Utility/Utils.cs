using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Scripts.Shared.Utility
{
    public static class Utils
    {
        public static T RandomEnumValue<T>()
        {
            var values = Enum.GetValues(typeof(T));
            int random = UnityEngine.Random.Range(0, values.Length);
            return (T)values.GetValue(random);
        }

        public static T RandomEnumValueExcept<T>(T value)
        {
            var values = Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .Where(x => x.ToString() != value.ToString())
                .ToArray();

            int random = UnityEngine.Random.Range(0, values.Length);
            return (T)values.GetValue(random);
        }

        public static int ManDistance(int x1, int y1, int x2, int y2)
        {
            return Mathf.Max(Mathf.Abs(x1 - x2), Mathf.Abs(y1 - y2));
        }

        public static int EuclidDistanceSquared(int x1, int y1, int x2, int y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }

        public static string Split(
            string str,
            int length,
            string delimiter = "\\s",
            string separator = "\r\n"
            )
        {
            if (length <= 0)
            {
                return string.Empty;
            }

            var pattern = @"(\b.{1," + length + @"})(?:" + delimiter + @"+|$)";

            var chunks = Regex
                .Matches(str, pattern)
                .Cast<Match>()
                .Select(p => p.Groups[1].Value)
                .ToList();

            return string.Join(separator, chunks);
        }
    }
}