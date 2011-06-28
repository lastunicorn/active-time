using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustInTheWind.ActiveTime
{
    internal class CsvUtil
    {
        /// <summary>
        /// Doubles all the '"'. That means it replaces '"' with '""'.
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string CsvEncode(string text)
        {
            if (text == null || text.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return "\"" + text.Replace("\"", "\"\"") + "\"";
            }
        }
    }
}
