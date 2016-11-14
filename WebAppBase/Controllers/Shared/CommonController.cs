using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace WebAppBase.Controllers
{
    public class CommonController : Controller
    {
        public string GetDisplay(bool flag)
        {
            if (flag)
            {
                return "●";
            }
            return "";
        }

        public string GetNumUnit(int? num,string unit)
        {
            if (num==null)
            {
                return "";
            }
            return string.Format("{0:N0}{1}", num, unit);
        }

        public string GetDecimalUnit(decimal? num, string unit)
        {
            if (num == null)
            {
                return "";
            }
            return num.Value.ToString("0.0") + unit;
        }

        public string GetDecimalUnits(decimal? num, string unit)
        {
            if (num == null)
            {
                return "";
            }
            return string.Format("{0:N2}{1}" ,num , unit);
        }

        public string GetWeightCount(int? num, int DecimalPosition = 2, string unit="Kg")
        {
            if (!num.HasValue)
            {
                return "";
            }

            return string.Format("{0:N" + DecimalPosition + "}{1}", num.Value / 1000.0, unit);
        } 
        
        public string GetFormatDate(DateTime date)
        {
            return date.ToString("yyyy/MM/dd HH:mm");
        }
        public string GetFormatOnlyDate(DateTime date)
        {
            return date.ToString("yyyy/MM/dd");
        }

        public string Commafy(decimal num, int fixeds)
        {
            string rnum = num.ToString(string.Format("N{0}", fixeds));
            string pattern = @"(-?\d+)(\d{3})";
            while (Regex.IsMatch(rnum, pattern))
            {
                rnum = Regex.Replace(rnum, pattern, "$1,$2", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
            return rnum;
        }
        public string Commafy(decimal? num)
        {
            if (num == null)
            {
                return "";
            }
            return Commafy(num.Value, 0);
        }

        public string FormatValue(decimal? Value, string Field)
        {
            if (Value == null)
            {
                return "0";
            }
            string val = string.Empty;

            switch (Field)
            {
                case "Weight":
                    val = Value.Value.ToString("#.#");
                    break;
                case "Length":
                    val = Value.Value.ToString("#.#");
                    break;
                case "Yield":
                    val = string.Format("{0:0%}", Value.Value);
                    break;
                case "FeedCount":
                    val = Value.Value.ToString("#.#");
                    break;
                default:
                    break;
            }

            return val;
        }
    }
}
