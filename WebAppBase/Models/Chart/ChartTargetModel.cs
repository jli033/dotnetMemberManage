using SharedUtilitys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAppBase.Models.Chart
{
    public class ChartTargetModel : BaseModel
    {
        public static List<string> ChartColors { get; private set; }
        public string OrganizationID { get; set; }
        public string Title { get; set; }
        public string RedirectController { get; set; }
        public string RedirectAction { get; set; }
        public object RouteValues { get; set; }

        public string[] ChartTypes { get; set; }

        public string XAxesType { get; set; }
        public string XAxesTimeUnit { get; set; }
        public int XAxesUnitStepSize { get; set; }
        public Dictionary<string,string> XAxesDisplayFormats { get; set; }

        public bool ZoomEnable { get; set; }

        public List<string> Labels { get; set; }
        public List<Dictionary<string, string>> ChartYAxisLables { get; set; }
        public List<Dictionary<string, List<decimal>>> ChartYAxisValues { get; set; }

        public string RefUrl { get; set; }

        static ChartTargetModel()
        {
            ChartColors = new List<string>();
            ChartColors.Add("rgba(0, 152, 254, 1)");
            ChartColors.Add("rgba(221, 8, 0, 1)");
            ChartColors.Add("rgba(255, 170, 102, 1)");
            ChartColors.Add("rgba(91, 193, 70, 1)");
            ChartColors.Add("rgba(0, 0, 0, 1)");
        }

        public ChartTargetModel()
        {
            ChartTypes = new[] { "line", "bar" };
            XAxesType = "time";
            XAxesTimeUnit = "month";
            XAxesUnitStepSize = 1;
            XAxesDisplayFormats = new Dictionary<string, string>();
            XAxesDisplayFormats.Add("month", "YYYY-MM");
            Labels = new List<string>();
            ChartYAxisLables = new List<Dictionary<string, string>>();
            ChartYAxisValues = new List<Dictionary<string, List<decimal>>>();
            ZoomEnable = true;
        }        

        public static decimal ToDecimal(int? intValue )
        {
            if (!intValue.HasValue)
            {
                return 0;
            }
            return intValue.Value;
        }

        public static decimal ToDecimal(decimal? decimalValue)
        {
            if (!decimalValue.HasValue)
            {
                return 0;
            }
            return decimalValue.Value;
        }

        public static decimal ToDecimal(object decimalValue)
        {
            decimal r;
            if (decimal.TryParse(string.Format("{0}", decimalValue).Replace("%",""), out r))
            {
                return r;
            }
            else
            {
                return 0;
            }
        }
    }

    public class ChartAxi
    {
        public int Index { get; set; }
        public string Key { get; set; }
    }
}
