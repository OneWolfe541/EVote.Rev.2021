using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using EVote.DataModels;
using EVote.DataMethods;
using EVote.Filters;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.IO;
using EVote.Context;

namespace EVote.Controllers
{
    [VerifyUser]
    public class StatsController : Controller
    {
        // GET: Stats
        public ActionResult Index()
        {
            return View();
        }

        // Get Voter Counts page
        public ActionResult Counts()
        {
            ViewBag.TotalCounts = StatisticsMethods.VoterCount();
            ViewBag.ActiveCounts = StatisticsMethods.VotedCount();
            ViewBag.Percent = StatisticsMethods.ActivityPercent().ToString("0.00");
            return View();
        }

        public ActionResult EVCounts()
        {
            ViewBag.TotalCounts = StatisticsMethods.VoterCount();
            ViewBag.ActiveCounts = StatisticsMethods.EarlyVotingCount();
            ViewBag.Percent = StatisticsMethods.EVActivityPercent().ToString("0.00");
            return View();
        }

        public ActionResult DistrictCounts()
        {
            ViewBag.TotalCounts = StatisticsMethods.VoterCount();
            ViewBag.ActiveCounts = StatisticsMethods.DistrictCount();
            ViewBag.Percent = StatisticsMethods.DistrictActivityPercent().ToString("0.00");
            return View();
        }

        public ActionResult AllVoterCounts()
        {
            ViewBag.TotalCounts = StatisticsMethods.VoterCount();
            ViewBag.ActiveCounts = StatisticsMethods.VotedCount();
            ViewBag.Percent = StatisticsMethods.ActivityPercent().ToString("0.00");
            return View();
        }

        public ActionResult ElectionTracking()
        {
            ViewBag.TotalCounts = StatisticsMethods.VoterCount();
            ViewBag.ActiveCounts = StatisticsMethods.DistrictCount();
            ViewBag.Percent = StatisticsMethods.DistrictActivityPercent().ToString("0.00");
            return View();
        }

        public ActionResult AllMailTracking()
        {
            ViewBag.TotalCounts = StatisticsMethods.VoterCount();
            ViewBag.ActiveCounts = StatisticsMethods.DistrictCount();
            ViewBag.Percent = StatisticsMethods.DistrictActivityPercent().ToString("0.00");
            return View();
        }

        // Create a Chart
        public ActionResult VoterChart()
        {
            var voterCounts = StatisticsMethods.ActivityBySite();
            //var myChart = new Chart(width: 500, height: 500, theme: ChartTheme.Blue)
            var myChart = new System.Web.Helpers.Chart(width: 600, height: 600, themePath: "~/Content/ChartTheme.xml")
                    .AddTitle("Election Day Activity")
                    .AddSeries(
                        name: "Voters",
                        chartType: "Bar",
                        xValue: voterCounts, xField: "UserName",
                        yValues: voterCounts, yFields: "EDVoterCount")
                    .SetXAxis(title: "Pollsites").SetYAxis(title: "Voter Counts").Write("png");
            return null;
        }

        public ActionResult EVVoterChart()
        {
            var voterCounts = StatisticsMethods.EVActivityBySite();
            //var myChart = new Chart(width: 500, height: 500, theme: ChartTheme.Blue)
            var myChart = new System.Web.Helpers.Chart(width: 600, height: 600, themePath: "~/Content/ChartTheme.xml")
                    .AddTitle("Early Voting Activity")
                    .AddSeries(
                        name: "Voters",
                        chartType: "Bar",
                        xValue: voterCounts, xField: "UserName",
                        yValues: voterCounts, yFields: "EDVoterCount")
                    .SetXAxis(title: "Pollsites").SetYAxis(title: "Voter Counts").Write("png");
            return null;
        }

        // Create a Chart
        public ActionResult ElectionActivity()
        {
            var voterCounts = StatisticsMethods.ActivityByLogCode();
            //var myChart = new Chart(width: 500, height: 500, theme: ChartTheme.Blue)
            var myChart = new System.Web.Helpers.Chart(width: 600, height: 600, themePath: "~/Content/ChartTheme.xml")
                    .AddTitle("All Election Activity")
                    .AddSeries(
                        name: "Voters",
                        chartType: "Bar",
                        xValue: voterCounts, xField: "UserName",
                        yValues: voterCounts, yFields: "EDVoterCount")
                    .SetXAxis(title: "Log Description").SetYAxis(title: "Voter Count").Write("png");
            return null;
        }

        // Create a Chart
        public ActionResult ActivityByDistrict()
        {
            var voterCounts = StatisticsMethods.ActivityByDistrict();
            //var myChart = new Chart(width: 500, height: 500, theme: ChartTheme.Blue)
            var myChart = new System.Web.Helpers.Chart(width: 600, height: 600, themePath: "~/Content/ChartTheme.xml")
                    .AddTitle("Activity by District")
                    .AddSeries(
                        name: "Voters",
                        chartType: "Bar",
                        xValue: voterCounts, xField: "UserName",
                        yValues: voterCounts, yFields: "EDVoterCount")
                    .SetXAxis(title: "District Name").SetYAxis(title: "Voter Count").Write("png");
            return null;
        }

        public ActionResult SiteSummary(int? type)
        {
            if (type == null) type = 2;

            ViewBag.Type = type;

            // Pass Site Summary table to view            
            return View(StatisticsMethods.SiteSummary(type));            
        }

        public FileResult CreateChart()
        {
            int LogCode = 12;
            //if (Convert.ToInt32(Session["RoleID"]) == 9) LogCode = 11;
            IList<ElectionDayCountsModel> voters = GetElectionDayCountList2(LogCode);
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 600;
            chart.Height = 600;
            //chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle("Election Day Counts"));
            //chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreateSeries(voters, System.Web.UI.DataVisualization.Charting.SeriesChartType.Bar));
            chart.ChartAreas.Add(CreateChartArea());

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public FileResult ElectionDayColorChart()
        {
            int LogCode = 12;
            //if (Convert.ToInt32(Session["RoleID"]) == 9) LogCode = 11;
            IList<ElectionDayCountsModel> voters = GetElectionDayCountList2(LogCode);
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 600;
            chart.Height = 600;
            //chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle("Election Day Counts"));
            //chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreateColorSeries(voters, System.Web.UI.DataVisualization.Charting.SeriesChartType.Bar));
            chart.ChartAreas.Add(CreateChartArea());

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public FileResult CreateEVChart()
        {
            int LogCode = 11;
            //if (Convert.ToInt32(Session["RoleID"]) == 9) LogCode = 11;
            IList<ElectionDayCountsModel> voters = GetElectionDayCountList2(LogCode);
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 600;
            chart.Height = 600;
            //chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle("Early Voting Counts"));
            //chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreateSeries(voters, System.Web.UI.DataVisualization.Charting.SeriesChartType.Bar));
            chart.ChartAreas.Add(CreateChartArea());

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public FileResult ElectionActivityChart()
        {
            //int LogCode = 12;
            //if (Convert.ToInt32(Session["RoleID"]) == 9) LogCode = 11;
            List<ElectionDayCountsModel> voters = StatisticsMethods.ActivityByLogCode();
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 600;
            chart.Height = 600;
            //chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle("All Election Activity"));
            //chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreateSeries(voters, System.Web.UI.DataVisualization.Charting.SeriesChartType.Bar));
            chart.ChartAreas.Add(CreateChartArea());

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public FileResult ElectionActivityColorChart()
        {
            //int LogCode = 12;
            //if (Convert.ToInt32(Session["RoleID"]) == 9) LogCode = 11;
            List<ElectionDayCountsModel> voters = StatisticsMethods.ActivityByLogCode();
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 600;
            chart.Height = 600;
            //chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle("All Election Activity"));
            //chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreateColorSeries(voters, System.Web.UI.DataVisualization.Charting.SeriesChartType.Bar));
            chart.ChartAreas.Add(CreateChartArea());

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public FileResult DistrictChart()
        {
            //int LogCode = 12;
            //if (Convert.ToInt32(Session["RoleID"]) == 9) LogCode = 11;
            List<ElectionDayCountsModel> voters = StatisticsMethods.ActivityByDistrict();
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 600;
            chart.Height = 600;
            //chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle("District Counts"));
            //chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreateSeries(voters, System.Web.UI.DataVisualization.Charting.SeriesChartType.Bar));
            chart.ChartAreas.Add(CreateChartArea());

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public Series CreateSeries(IList<ElectionDayCountsModel> results, SeriesChartType chartType)
        {
            Series seriesDetail = new Series();
            seriesDetail.Name = "Result Chart";
            seriesDetail.IsValueShownAsLabel = true;
            seriesDetail.Color = Color.FromArgb(150, 150, 150);
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;

            DataPoint point;
            foreach (ElectionDayCountsModel result in results)
            {
                point = new DataPoint();
                point.AxisLabel = result.UserName;
                point.YValues = new double[] { (double)(result.EDVoterCount) };
                seriesDetail.Points.Add(point);
            }
            seriesDetail.ChartArea = "Result Chart";

            return seriesDetail;
        }

        public Series CreateColorSeries(IList<ElectionDayCountsModel> results, SeriesChartType chartType)
        {
            Series seriesDetail = new Series();
            seriesDetail.Name = "Result Chart";
            seriesDetail.IsValueShownAsLabel = true;
            //seriesDetail.Color = Color.FromArgb(150, 150, 150);
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;

            seriesDetail.Palette = ChartColorPalette.BrightPastel;

            DataPoint point;
            foreach (ElectionDayCountsModel result in results)
            {
                point = new DataPoint();
                point.AxisLabel = result.UserName;
                point.YValues = new double[] { (double)(result.EDVoterCount) };
                seriesDetail.Points.Add(point);
            }
            seriesDetail.ChartArea = "Result Chart";

            //Color[] colors = new Color[] { Color.LightBlue, Color.Goldenrod, Color.IndianRed, Color.Navy, Color.LightGray, Color.YellowGreen, Color.Green, Color.LightPink, Color.LightGreen, Color.Maroon, Color.Yellow, Color.Beige, Color.Blue, Color.Brown, Color.Indigo, Color.Red, Color.LightBlue, Color.Goldenrod, Color.IndianRed, Color.Navy, Color.LightGray, Color.YellowGreen, Color.Green, Color.LightPink, Color.LightGreen, Color.Maroon, Color.Yellow, Color.Beige, Color.Blue, Color.Brown, Color.Indigo, Color.Red };
            //foreach (DataPoint dPoint in seriesDetail.Points)
            //{
            //    //point.LabelBackColor = colors[series.Points.IndexOf(point)];
            //    dPoint.Color = colors[seriesDetail.Points.IndexOf(dPoint)];
            //    dPoint.BorderColor = Color.Black;
            //    dPoint.BorderWidth = 1;
            //    dPoint.BorderDashStyle = ChartDashStyle.Solid;
            //}

            return seriesDetail;
        }

        

        public ChartArea CreateChartArea()
        {
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "Result Chart";
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Interval = 1;

            return chartArea;
        }

        public FileResult CreateDistrictPie()
        {
            IList<ElectionDayCountsModel> voters = StatisticsMethods.ActivityByDistrictForPie();
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 600;
            chart.Height = 400;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle("District Counts"));

            chart.Series.Add(new Series("Data"));
            chart.Series["Data"].ChartType = SeriesChartType.Pie;
            chart.Series["Data"]["PieLabelStyle"] = "Outside";
            chart.Series["Data"]["PieLineColor"] = "Black";
            chart.Series["Data"].Points.DataBindXY(
                voters.Select(data => data.UserName).ToArray(),
                voters.Select(data => data.EDVoterCount).ToArray());

            chart.ChartAreas.Add(CreatePieChartArea());

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public FileResult CreateElectionPie()
        {
            IList<ElectionDayCountsModel> voters = StatisticsMethods.ActivityByLogCodeForPie();
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 600;
            chart.Height = 400;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle("Election Activity"));

            chart.Series.Add(new Series("Data"));
            chart.Series["Data"].ChartType = SeriesChartType.Pie;
            chart.Series["Data"]["PieLabelStyle"] = "Outside";
            chart.Series["Data"]["PieLineColor"] = "Black";
            chart.Series["Data"].Points.DataBindXY(
                voters.Select(data => data.UserName).ToArray(),
                voters.Select(data => data.EDVoterCount).ToArray());

            chart.ChartAreas.Add(CreatePieChartArea());

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public FileResult CreatePie()
        {
            IList<ElectionDayCountsModel> voters = StatisticsMethods.ActivityByDistrict();
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 600;
            chart.Height = 600;
            //chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle("District Counts"));

            //chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreatePieSeries(voters, System.Web.UI.DataVisualization.Charting.SeriesChartType.Pie));
            chart.ChartAreas.Add(CreatePieChartArea());

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public Series CreatePieSeries(IList<ElectionDayCountsModel> results, SeriesChartType chartType)
        {
            Series seriesDetail = new Series();
            seriesDetail.Name = "Result Chart";
            //seriesDetail.IsValueShownAsLabel = true;
            seriesDetail.Color = Color.FromArgb(150, 150, 150);
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;

            seriesDetail.SmartLabelStyle.Enabled = true;
            seriesDetail.SmartLabelStyle.CalloutStyle = LabelCalloutStyle.Underlined;
            seriesDetail.SmartLabelStyle.CalloutLineColor = Color.Black;
            seriesDetail.SmartLabelStyle.CalloutLineDashStyle = ChartDashStyle.Solid;
            seriesDetail.SmartLabelStyle.CalloutLineWidth = 3;
            seriesDetail.SmartLabelStyle.CalloutLineAnchorCapStyle = LineAnchorCapStyle.Arrow;

            seriesDetail["PieLabelStyle"] = "OutSite";

            DataPoint point;

            foreach (ElectionDayCountsModel result in results)
            {

                point = new DataPoint();
                //point.AxisLabel = result.UserName;
                point.YValues = new double[] { (double)(result.EDVoterCount) };
                point.AxisLabel = result.UserName;
                //point.LabelAngle = 20;

                seriesDetail.Points.Add(point);
            }
            seriesDetail.ChartArea = "Result Chart";
            return seriesDetail;
        }

        public ChartArea CreatePieChartArea()
        {
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "Result Chart";
            chartArea.BackColor = Color.Transparent;
            //chartArea.Area3DStyle.Enable3D = true;

            //chartArea.AxisX.IsLabelAutoFit = false;
            //chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            //chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            //chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            //chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            //chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            //chartArea.AxisX.Interval = 1;

            return chartArea;
        }

        public System.Web.UI.DataVisualization.Charting.Title CreateTitle(string titleText)
        {
            Title chartTitle = new Title();

            chartTitle.Text = titleText;
            chartTitle.Alignment = ContentAlignment.TopCenter;

            return chartTitle;
        }

        // Generate count list by site user
        public IList<ElectionDayCountsModel> GetElectionDayCountList2(int LogCode)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Create list object
                var voterList = bdEVote.VoterDatas.Where(o =>
                // LogCode 12 is "Voted at Polls"
                o.LogCode == LogCode
                // Group By site user
                ).GroupBy(o => o.UserName
                // Select list of fields
                ).Select(p => new ElectionDayCountsModel
                {
                    UserName = p.Key,           // Key field as defined in Group By
                    EDVoterCount = p.Count()    // Same as Count(*)
                                                // Order By Decending -- Bar Chart list items in reverse order
                }).OrderByDescending(o => o.EDVoterCount).ToList();

                // Return list object
                return voterList;
            }
        }

        #region AllMailCharts
        public FileResult CreateAllMailDistrictPie()
        {
            IList<ElectionDayCountsModel> voters = StatisticsMethods.AllMailActivityByDistrictForPie();
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 600;
            chart.Height = 400;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle("Issued Ballots"));

            chart.Series.Add(new Series("Data"));
            chart.Series["Data"].ChartType = SeriesChartType.Pie;
            chart.Series["Data"]["PieLabelStyle"] = "Outside";
            chart.Series["Data"]["PieLineColor"] = "Black";
            chart.Series["Data"].Points.DataBindXY(
                voters.Select(data => data.UserName).ToArray(),
                voters.Select(data => data.EDVoterCount).ToArray());

            chart.ChartAreas.Add(CreatePieChartArea());

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public FileResult CreateAllMailElectionPie()
        {
            IList<ElectionDayCountsModel> voters = StatisticsMethods.AllMailReturnedByDistrictForPie();
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 600;
            chart.Height = 400;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle("Returned Ballots"));

            chart.Series.Add(new Series("Data"));
            chart.Series["Data"].ChartType = SeriesChartType.Pie;
            chart.Series["Data"]["PieLabelStyle"] = "Outside";
            chart.Series["Data"]["PieLineColor"] = "Black";
            chart.Series["Data"].Points.DataBindXY(
                voters.Select(data => data.UserName).ToArray(),
                voters.Select(data => data.EDVoterCount).ToArray());

            chart.ChartAreas.Add(CreatePieChartArea());

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public FileResult AllMailEligibleChart()
        {
            IList<ElectionDayCountsModel> voters = GetEligibleCountList();
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = 600;
            chart.Height = 600;
            //chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle("Eligible Voters"));
            //chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreateColorSeries(voters, System.Web.UI.DataVisualization.Charting.SeriesChartType.Bar));
            chart.ChartAreas.Add(CreateChartArea());

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public IList<ElectionDayCountsModel> GetEligibleCountList()
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Create list object
                var voterList = bdEVote.VoterDatas.Where(o =>
                // LogCode 12 is "Voted at Polls"
                o.LogCode >= 1
                && o.BallotStyle != null
                // Group By site user
                ).GroupBy(o => o.BallotStyle
                // Select list of fields
                ).Select(p => new ElectionDayCountsModel
                {
                    UserName = p.Key,           // Key field as defined in Group By
                    EDVoterCount = p.Count()    // Same as Count(*)
                                                // Order By Decending -- Bar Chart list items in reverse order
                }).OrderByDescending(o => o.EDVoterCount).ToList();

                // Return list object
                return voterList;
            }
        }
        #endregion
    }
}