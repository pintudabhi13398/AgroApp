﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace AgroApp.Common
{
    public class CommonHelper
    {
        public const string RegexEmail = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}";
        public const string PleaseSelect = "---Select---";
        public const string DateFormat = "dd/MM/yyyy";
        public const string TimeFormate = "HH:mm";
        public static readonly Dictionary<string, object> CenterColumnStyle = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" } };
        public static readonly Dictionary<string, object> LeftColumnStyle = new Dictionary<string, object> { { "align", "left" }, { "style", "text-align:left;vertical-align:middle !important;" } };
        public const int PazeSize = 50;
        public static IEnumerable<int> PageSizes = new[] { 10, 20, 50, 100 };
        public static IEnumerable<int> SmallPageSizes = new[] { 5, 10, 20 };

        public static readonly Dictionary<string, object> ActionCenterColumnStyleWithCanEdit = new Dictionary<string, object> { { "align", "right" }, { "style", "text-align:right;vertical-align:right !important;" } };

        public static readonly Dictionary<string, object> ActionCenterColumnStyleWithCanStatus = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" } };

       public static readonly Dictionary<string, object> StatusColumnStyle = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "60px" } };

       public static readonly Dictionary<string, object> SmallColumnStyle = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" }, { "width", "30px" } };


        //public static readonly Dictionary<string, object> CenterColumnStyle = new Dictionary<string, object> { { "align", "center" }, { "style", "text-align:center;vertical-align:middle !important;" } };
       //public static readonly Dictionary<string, object> RightColumnStyle = new Dictionary<string, object> { { "align", "right" }, { "style", "text-align:right;vertical-align:right !important;" } };


        /// <summary>
        /// WeekOfYear
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int WeekOfYear(DateTime date)
        {
            // var day = (int)CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(date);
            //return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date.AddDays(4 - (day == 0 ? 7 : day)), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, DayOfWeek.Monday);

        }

        /// <summary>
        /// GetFirstDateOfWeek
        /// </summary>
        /// <param name="dayInWeek"></param>
        /// <param name="firstDay"></param>
        /// <returns></returns>
        public static DateTime GetFirstDateOfWeek(DateTime dayInWeek, DayOfWeek firstDay)
        {
            var firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

        /// <summary>
        /// GetLastDateOfWeek
        /// </summary>
        /// <param name="dayInWeek"></param>
        /// <param name="firstDay"></param>
        /// <returns></returns>
        public static DateTime GetLastDateOfWeek(DateTime dayInWeek, DayOfWeek firstDay)
        {
            var lastDayInWeek = dayInWeek.Date;
            while (lastDayInWeek.DayOfWeek != firstDay)
                lastDayInWeek = lastDayInWeek.AddDays(1);

            return lastDayInWeek;
        }
       
        public static string GetErrorMessage(Exception ex, bool getStactRetrace = true)
        {
            try
            {
                var errorMessage = string.Empty;
                if (ex == null) return errorMessage;
                errorMessage = ex.Message;
                if (ex.InnerException != null)
                    errorMessage += Environment.NewLine + GetErrorMessage(ex.InnerException, getStactRetrace);
                if (getStactRetrace)
                    errorMessage += Environment.NewLine + ex.StackTrace;

                errorMessage = errorMessage.Replace("An error occurred while updating the entries. See the inner exception for details.", "");
                return errorMessage;
            }
            catch
            {
                return ex != null ? ex.Message : string.Empty;
            }
        }

        public static string GetDeleteErrorMessage(Exception ex, bool getStactRetrace = true)
        {
            try
            {
                var errorMessage = string.Empty;
                if (ex == null) return errorMessage;
                errorMessage = ex.Message;
                if (ex.InnerException != null)
                    errorMessage += Environment.NewLine + GetDeleteErrorMessage(ex.InnerException, getStactRetrace);
                if (getStactRetrace)
                    errorMessage += Environment.NewLine + ex.StackTrace;

                errorMessage = errorMessage.Replace("\"", " ");
                errorMessage = errorMessage.Replace("'", " ");
                return errorMessage;
            }
            catch
            {
                return ex != null ? ex.Message : string.Empty;
            }
        }

        public static string GetDeleteException(Exception exception)
        {
            string errorMessage = GetDeleteErrorMessage(exception, false);
            return errorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint") ? ParseDeleteMessage(errorMessage) : errorMessage;
        }
        private static string ParseDeleteMessage(string message)
        {
            try
            {
                const string str = "This record link to another record(s), you can not delete this record";
                return str;
            }
            catch
            {
                return message;
            }
        }
    }
}
