using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAppBase.Models.Chart
{
    public class BarConfig
    {
        public string type { get; set; }
        public BarChartData data { get; set; }
        public Options options { get; set; }
    }
    public class BarChartData
    {
        public string[] labels { get; set; }
        public BarChartDataSetBase[] datasets { get; set; }
    }

    public class BarChartDataSetBase
    {
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string pointBorderColor { get; set; }
        public string pointBackgroundColor { get; set; }
        public int pointBorderWidth { get; set; }
        public string yAxisID { get; set; }
        public int lineTension { get; set; }
        public bool fill { get; set; }
        public string DataKey { get; set; }
    }

    public class BarChartDataSet : BarChartDataSetBase
    {
        public decimal[] data { get; set; }
    }

    //public class BarChartDataSetXY : BarChartDataSetBase
    //{
    //    public ChartXYData[] data { get; set; }
    //}

    public class Options
    {
        public bool responsive { get; set; }
        public string hoverMode { get; set; }
        public int hoverAnimationDuration { get; set; }
        public bool stacked { get; set; }

        public Title title { get; set; }
        public Scales scales { get; set; }
        public Zoom zoom { get; set; }
    }

    public class Title
    {
        public bool display { get; set; }
        public string text { get; set; }
    }

    public class Scales
    {
        public XAxe[] xAxes { get; set; }
        public YAxe[] yAxes { get; set; }
    }

    public class XAxe
    {
        public string type { get; set; }
        public XAxeTime time { get; set; }

    }

    public class XAxeTime
    {
        public string unit { get; set; }
        public int unitStepSize { get; set; }
        public Dictionary<string, string> displayFormats { get; set; }
    }

    public class YAxe
    {
        public string type { get; set; }
        public bool display { get; set; }
        public string position { get; set; }
        public ScaleLabel scaleLabel { get; set; }
        public string id { get; set; }
        public GridLines gridLines { get; set; }
        public YAxesTicks ticks { get; set; }
    }

    public class ScaleLabel
    {
        public bool display { get; set; }
        public string labelString { get; set; }
    }

    public class GridLines
    {
        public bool drawOnChartArea { get; set; }
    }

    public class YAxesTicks
    {
        public decimal max { get; set; }
        public decimal min { get; set; }
    }

    public class Zoom
    {
        public bool enabled { get; set; }
        public bool drag { get; set; }
        public string mode { get; set; }
        public Limits limits { get; set; }

        public Zoom()
        {
            enabled = true;
            drag = true;
            mode = "x";
            limits = new Limits() { max = 10f, min = 0.5f };
        }
    }

    public class Limits
    {
        public float max { get; set; }
        public float min { get; set; }
    }

    //public class ChartXYData
    //{
    //    public string x { get; set; }
    //    public decimal y { get; set; }
    //}

}
