using System;
using Cysharp.Text;
using UnityEngine;

namespace Game
{
    public static class StringUtility
    {
        /// <summary>
        /// 千
        /// </summary>
        private const int Qian = 1000;
        /// <summary>
        /// 万
        /// </summary>
        private const int Wan = 10000;
        /// <summary>
        /// 10万
        /// </summary>
        private const int TenWan = 100000;
        /// <summary>
        /// 亿
        /// </summary>
        private const int Yi = 100000000;

        /// <summary>
        /// version_1>version_2返回1，
        /// version_1>version_2返回-1，
        /// version_1=version_2返回0
        /// </summary>
        public static int CompareVersion(string version_1, string version_2)
        {
            var numChars_1 = version_1.Split('.');
            var chars_2 = version_2.Split('.');
            var maxLength = Mathf.Max(numChars_1.Length, chars_2.Length);

            for (int i = 0; i < maxLength; i++)
            {
                int num_1 = numChars_1.Length > i ? int.Parse(numChars_1[i]) : 0;
                int num_2 = chars_2.Length > i ? int.Parse(chars_2[i]) : 0;

                if (num_1 > num_2)
                    return 1;
                else if (num_1 < num_2)
                    return -1;
            }
            return 0;
        }
        public static string Abbreviate(long num)
        {
            if (num < Wan)
            {
                return num.ToString();
            }
            else if (num >= Wan && num < Yi)
            {
                return ZString.Format("{0}万", num / Wan);
            }
            else {
                return ZString.Format("{0}亿", num / Yi);
            }
        }

        public static string AbbreviateToDouble(long num)
        {
            if (num < Wan)
            {
                return num.ToString();
            }
            else if (num >= Wan && num < Yi)
            {
                return ZString.Format("{0:0.##}万", (num / (double)Wan * 100) /100);
            }
            else
            {
                return ZString.Format("{0:0.##}亿", (num / (double)Yi * 100) / 100);
            }
        }
        public static string Abbreviate_Accuracy(long num)
        {
            if (num <= Wan)
            {
                return num.ToString();
            }
            else
            {
                if (num % Qian == 0)
                {
                    return ZString.Format("{0:0.#}万", (num / (float)Wan));
                }
                else
                {
                    return num.ToString();
                }
            }
        }
        public static string GetTime(int seconds)
        {
            var timeSpan = new TimeSpan(0, 0, seconds);
            if (timeSpan.Hours > 0)
            {
                return ZString.Format("{0}时{1}分{2}秒", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            }
            else if (timeSpan.Minutes > 0)
            {
                return ZString.Format("{0}分{1}秒", timeSpan.Minutes, timeSpan.Seconds);
            }
            else
            {
                return ZString.Format("{0}秒", timeSpan.Seconds);
            }
        }
    }
}