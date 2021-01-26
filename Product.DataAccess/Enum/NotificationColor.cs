using System;
using System.Collections.Generic;
using System.Text;

namespace Product.DataAccess.Base.Enum
{
    public enum NotificationColor
    {
        Error,
        Success,
        Warning,
        danger
    }

    public static class ErrorLevelExtensions
    {
        /// <summary>
        /// Get Color Name
        /// </summary>
        /// <param name="jsonColor"></param>
        /// <returns>string</returns>
        public static string ToColorName(this NotificationColor jsonColor) => jsonColor.ToString().ToLower();
    }
}
