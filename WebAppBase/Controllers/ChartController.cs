using AspMvcLibrary.Attributes;
using WebAppBase.Configs;
using WebAppBase.Models.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppBase.Controllers
{
    public class ChartController : Controller
    {
        public ActionResult Index()
        {
            var chartTargetModel = Session[SessionKeyConfig.ChartTargetModel] as ChartTargetModel;

            if (chartTargetModel == null)
            {
                return RedirectToAction("index", "home");
            }
            return View(chartTargetModel);
        }

        public ActionResult DeadReportChart()
        {
            var chartTargetModel = Session[SessionKeyConfig.ChartTargetModel] as ChartTargetModel;

            if (chartTargetModel == null)
            {
                return RedirectToAction("index", "home");
            }
            return View(chartTargetModel);
        }


        [HttpPost]
        public ActionResult InitChart(FormCollection formData)
        {
            var ChartAxis = new List<ChartAxi>();
            for (int i = 0; i < formData.Count / 2; i++)
            {
                var index = Convert.ToInt32(formData[i * 2]);
                var key = string.Format("{0}", formData[i * 2 + 1]);
                if (key.Equals("0"))
                {
                    continue;
                }
                ChartAxis.Add(new ChartAxi()
                {
                    Index = index,// Convert.ToInt32(formData[i * 2]),
                    Key = key// formData[i * 2 + 1]
                });
            }

            var barConfig = new BarConfig();
            var chartTargetModel = Session[SessionKeyConfig.ChartTargetModel] as ChartTargetModel;

            if (chartTargetModel == null)
            {
                barConfig.options.zoom.enabled = false;
                barConfig.options.zoom.drag = false;
                return Json(barConfig);
            }
            var axisLength = ChartAxis.Count;
            barConfig.type = chartTargetModel.ChartTypes[0];
            barConfig.data = new BarChartData();
            barConfig.data.labels = chartTargetModel.Labels.ToArray();
            barConfig.data.datasets = new BarChartDataSet[axisLength];
            for (int i = 0; i < axisLength; i++)
            {
                var key = ChartAxis[i].Key;
                var dataset = new BarChartDataSet();
                var yAxisLables = chartTargetModel.ChartYAxisLables[i];
                var listData = chartTargetModel.ChartYAxisValues[i];

                dataset.label = yAxisLables[key];
                dataset.backgroundColor = ChartTargetModel.ChartColors[i];
                dataset.borderColor = ChartTargetModel.ChartColors[i];
                dataset.pointBorderColor = ChartTargetModel.ChartColors[i];
                dataset.pointBackgroundColor = ChartTargetModel.ChartColors[i];
                dataset.pointBorderWidth = 1;
                dataset.yAxisID = string.Format("y-axis-{0}", i + 1);
                dataset.data = listData[key].ToArray();
                dataset.lineTension = 0;
                dataset.fill = false;
                dataset.DataKey = key;
                barConfig.data.datasets[i] = dataset;
            }

            barConfig.options = new Options();

            barConfig.options.responsive = true;
            barConfig.options.hoverMode = "index";
            barConfig.options.hoverAnimationDuration = 400;
            barConfig.options.stacked = false;
            barConfig.options.title = new Title()
            {
                display = false,
                text = chartTargetModel.Title
            };

            barConfig.options.scales = new Scales();
            if (!string.IsNullOrEmpty(chartTargetModel.XAxesType) && !string.IsNullOrEmpty(chartTargetModel.XAxesTimeUnit))
            {
                barConfig.options.scales.xAxes = new XAxe[1];
                var time = new XAxeTime()
                    {
                        unit = chartTargetModel.XAxesTimeUnit,
                        unitStepSize = chartTargetModel.XAxesUnitStepSize,
                        displayFormats = chartTargetModel.XAxesDisplayFormats
                    };
                barConfig.options.scales.xAxes[0] = new XAxe()
                {
                    type = chartTargetModel.XAxesType,
                    time = time,
                };
            }

            barConfig.options.scales.yAxes = new YAxe[axisLength];
            for (int i = 0; i < axisLength; i++)
            {
                var key = ChartAxis[i].Key;
                //var showAxis=(key.Equals("0") ? false : true);
                var yAxisLables = chartTargetModel.ChartYAxisLables[i];
                barConfig.options.scales.yAxes[i] = new YAxe();

                barConfig.options.scales.yAxes[i].type = "linear"; // only linear but allow scale type registration. This allows extensions to exist solely for log scale for instance
                barConfig.options.scales.yAxes[i].display = true;
                barConfig.options.scales.yAxes[i].scaleLabel = new ScaleLabel()
                {
                    display = true,
                    labelString = yAxisLables[key]
                };
                if (i % 2 == 0)
                {
                    barConfig.options.scales.yAxes[i].position = "left";
                }
                else
                {
                    barConfig.options.scales.yAxes[i].position = "right";
                }
                barConfig.options.scales.yAxes[i].id = string.Format("y-axis-{0}", i + 1);
                barConfig.options.scales.yAxes[i].gridLines = new GridLines()
                {
                    drawOnChartArea = i == 0
                };
                var listData = chartTargetModel.ChartYAxisValues[i];
                var dMax = listData[key].Max();
                if (dMax > 10)
                {
                    var len = Math.Floor(dMax).ToString().Length - 1;
                    var div = Convert.ToInt32(Math.Pow(10, len));
                    var ticksMax = (Math.Ceiling(dMax / div) * div);
                    if (ticksMax == dMax)
                    {
                        dMax = Convert.ToDecimal(Math.Ceiling(dMax / div) * div) * 1.25m;
                    }
                    else
                    {
                        dMax = ticksMax;
                    }
                }
                else
                {
                    dMax = Math.Ceiling(dMax * 1.25m);
                }
                dMax = Convert.ToDecimal(Math.Max(1, dMax));
                barConfig.options.scales.yAxes[i].ticks = new YAxesTicks()
                {
                    max = dMax
                };
            }
            barConfig.options.zoom = new Zoom();
            if (!chartTargetModel.ZoomEnable)
            {
                barConfig.options.zoom.enabled = false;
                barConfig.options.zoom.drag = false;
            }
            var r = new { barConfig = barConfig };
            return Json(r);
        }

        [HttpPost]
        public ActionResult GetChartData(string DataKey, int lineIdx)
        {
            var chartTargetModel = Session[SessionKeyConfig.ChartTargetModel] as ChartTargetModel;
            var dataLabel = chartTargetModel.ChartYAxisLables[0][DataKey];
            //var data = new List<ChartXYData>();
            //for (int i = 0; i < chartTargetModel.Labels.Count; i++)
            //{
            //    data.Add(new ChartXYData() { x = chartTargetModel.Labels[i], y = chartTargetModel.ChartYAxisValues[0][DataKey][i] });
            //}
            var data = chartTargetModel.ChartYAxisValues[0][DataKey];
            var idx = lineIdx % ChartTargetModel.ChartColors.Count;
            var key = DataKey;
            var dataset = new BarChartDataSet()
            {
                label = dataLabel,
                backgroundColor = ChartTargetModel.ChartColors[idx],
                borderColor = ChartTargetModel.ChartColors[idx],
                pointBorderColor = ChartTargetModel.ChartColors[idx],
                pointBackgroundColor = ChartTargetModel.ChartColors[idx],
                pointBorderWidth = 1,
                yAxisID = string.Format("y-axis-{0}", 0),
                data = data.ToArray(),
                lineTension = 0,
                fill = false,
                DataKey = DataKey
            };

            var dMax = data.Max();
            if (dMax > 10)
            {
                var len = Math.Floor(dMax).ToString().Length - 1;
                var div = Convert.ToInt32(Math.Pow(10, len));
                var ticksMax = (Math.Ceiling(dMax / div) * div);
                if (ticksMax == dMax)
                {
                    dMax = Convert.ToDecimal(Math.Ceiling(dMax / div) * div) * 1.25m;
                }
                else
                {
                    dMax = ticksMax;
                }
            }
            else
            {
                dMax = Math.Ceiling(dMax * 1.25m);
            }
            dMax = Convert.ToDecimal(Math.Max(1, dMax));

            var r = new { dataset = dataset, ticksMax = dMax };
            return Json(r);
        }

        [HttpPost]
        [ActionName("Index")]
        [SubmitCommand("back")]
        public ActionResult Back()
        {
            return _back();
        }

        [HttpPost]
        [ActionName("DeadReportChart")]
        [SubmitCommand("back")]
        public ActionResult Back2()
        {
            return _back();
        }

        public ActionResult _back()
        {
            var model = _getTarget();
            if (model == null)
            {
                return RedirectToAction("index", "home");
            }
            var dic = model.RouteValues as Dictionary<string, string>;
            if (dic != null)
            {
                System.Web.Routing.RouteValueDictionary routeDic = new System.Web.Routing.RouteValueDictionary();
                foreach (var kv in dic)
                {
                    routeDic.Add(kv.Key, kv.Value);
                }
                return RedirectToAction(model.RedirectAction, model.RedirectController, routeDic);
            }
            else
            {
                return RedirectToAction(model.RedirectAction, model.RedirectController, model.RouteValues);
            }
        }


        private ChartTargetModel _getTarget()
        {
            var model = Session[SessionKeyConfig.ChartTargetModel] as ChartTargetModel;
            return model;
        }

    }
}
