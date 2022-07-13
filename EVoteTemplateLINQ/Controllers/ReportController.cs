using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EVote.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            //ViewBag.CString = System.Configuration.ConfigurationManager.ConnectionStrings["EVoteSQLDataConnectionString"].ConnectionString;
            return View();
        }

        // Get a PDF report object
        public FileStreamResult TodaysActivityPDF()
        {
            // Disable extra notifications while on the web
            FastReport.Utils.Config.WebMode = true;

            // Create new report object
            FastReport.Report rptVoterList = new FastReport.Report();
            // Load Report 1 into the report opbject
            rptVoterList.Load(Server.MapPath("../Reports/DailyDetailsBySiteEVote.frx"));

            rptVoterList.Dictionary.Connections[0].ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EVoteSQLDataConnectionString"].ConnectionString;

            // Define new query
            //var sql = "SELECT [tblVoterData].*, [tblDistricts].[DistrictName], [tblLogCodes].[LogDescription] FROM [dbo].[tblVoterData] LEFT JOIN [dbo].[tblDistricts] ON [tblVoterData].[District] = [tblDistricts].[District] LEFT JOIN [dbo].[tblLogCodes] ON [tblVoterData].[LogCode] = [tblLogCodes].[LogCode] Where BarCode = " + BarCode;
            //int key = 10515;
            var sql = TodaysActivitySQL();

            // Get data source from report
            FastReport.Data.TableDataSource tds = (FastReport.Data.TableDataSource)rptVoterList.GetDataSource("DailyDetailData");

            // Save query to data source
            tds.SelectCommand = sql;

            //rptVoterList.SetParameterValue("P1", key);

            // Prepare the report for printing or viewing
            rptVoterList.Prepare();

            // Define PDF export object
            FastReport.Export.Pdf.PDFExport export = new FastReport.Export.Pdf.PDFExport();

            // Set PDF settings
            export.PrintOptimized = false;
            export.OpenAfterExport = false;
            export.EmbeddingFonts = true;

            // Create Memory Stream object
            System.IO.MemoryStream s = new System.IO.MemoryStream();

            // Export PDF to memory stream
            rptVoterList.Export(export, s);
            s.Position = 0;

            ViewBag.Result = "True";

            // Return the memory stream object preloaded with the report PDF
            return new FileStreamResult(s, "application/pdf");
        }

        public FileStreamResult ActivityToDatePDF()
        {
            // Disable extra notifications while on the web
            FastReport.Utils.Config.WebMode = true;

            // Create new report object
            FastReport.Report rptVoterList = new FastReport.Report();
            // Load Report 1 into the report opbject
            rptVoterList.Load(Server.MapPath("../Reports/DailyDetailsBySiteEVote.frx"));

            rptVoterList.Dictionary.Connections[0].ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EVoteSQLDataConnectionString"].ConnectionString;

            // Define new query
            //var sql = "SELECT [tblVoterData].*, [tblDistricts].[DistrictName], [tblLogCodes].[LogDescription] FROM [dbo].[tblVoterData] LEFT JOIN [dbo].[tblDistricts] ON [tblVoterData].[District] = [tblDistricts].[District] LEFT JOIN [dbo].[tblLogCodes] ON [tblVoterData].[LogCode] = [tblLogCodes].[LogCode] Where BarCode = " + BarCode;
            //int key = 10515;
            var sql = ActivityToDateSQL();

            // Get data source from report
            FastReport.Data.TableDataSource tds = (FastReport.Data.TableDataSource)rptVoterList.GetDataSource("DailyDetailData");

            // Save query to data source
            tds.SelectCommand = sql;

            //rptVoterList.SetParameterValue("P1", key);

            // Prepare the report for printing or viewing
            rptVoterList.Prepare();

            // Define PDF export object
            FastReport.Export.Pdf.PDFExport export = new FastReport.Export.Pdf.PDFExport();

            // Set PDF settings
            export.PrintOptimized = false;
            export.OpenAfterExport = false;
            export.EmbeddingFonts = true;

            // Create Memory Stream object
            System.IO.MemoryStream s = new System.IO.MemoryStream();

            // Export PDF to memory stream
            rptVoterList.Export(export, s);
            s.Position = 0;

            ViewBag.Result = "True";

            // Return the memory stream object preloaded with the report PDF
            return new FileStreamResult(s, "application/pdf");
        }

        // Get a PDF report object
        public FileStreamResult TodaysActivitySummaryPDF()
        {
            // Disable extra notifications while on the web
            FastReport.Utils.Config.WebMode = true;

            // Create new report object
            FastReport.Report rptVoterList = new FastReport.Report();
            // Load Report 1 into the report opbject
            rptVoterList.Load(Server.MapPath("../Reports/SummaryByDistrictEVote.frx"));

            rptVoterList.Dictionary.Connections[0].ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EVoteSQLDataConnectionString"].ConnectionString;

            // Define new query
            //var sql = "SELECT [tblVoterData].*, [tblDistricts].[DistrictName], [tblLogCodes].[LogDescription] FROM [dbo].[tblVoterData] LEFT JOIN [dbo].[tblDistricts] ON [tblVoterData].[District] = [tblDistricts].[District] LEFT JOIN [dbo].[tblLogCodes] ON [tblVoterData].[LogCode] = [tblLogCodes].[LogCode] Where BarCode = " + BarCode;
            //int key = 10515;
            var sql = TodaysActivitySummarySQL();

            // Get data source from report
            FastReport.Data.TableDataSource tds = (FastReport.Data.TableDataSource)rptVoterList.GetDataSource("DailyDetailData");

            // Save query to data source
            tds.SelectCommand = sql;

            //rptVoterList.SetParameterValue("P1", key);

            // Prepare the report for printing or viewing
            rptVoterList.Prepare();

            // Define PDF export object
            FastReport.Export.Pdf.PDFExport export = new FastReport.Export.Pdf.PDFExport();

            // Set PDF settings
            export.PrintOptimized = false;
            export.OpenAfterExport = false;
            export.EmbeddingFonts = true;

            // Create Memory Stream object
            System.IO.MemoryStream s = new System.IO.MemoryStream();

            // Export PDF to memory stream
            rptVoterList.Export(export, s);
            s.Position = 0;

            ViewBag.Result = "True";

            // Return the memory stream object preloaded with the report PDF
            return new FileStreamResult(s, "application/pdf");
        }

        public FileStreamResult ActivityToDateSummaryPDF()
        {
            // Disable extra notifications while on the web
            FastReport.Utils.Config.WebMode = true;

            // Create new report object
            FastReport.Report rptVoterList = new FastReport.Report();
            // Load Report 1 into the report opbject
            rptVoterList.Load(Server.MapPath("../Reports/SummaryByDistrictEVote.frx"));

            rptVoterList.Dictionary.Connections[0].ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EVoteSQLDataConnectionString"].ConnectionString;

            // Define new query
            //var sql = "SELECT [tblVoterData].*, [tblDistricts].[DistrictName], [tblLogCodes].[LogDescription] FROM [dbo].[tblVoterData] LEFT JOIN [dbo].[tblDistricts] ON [tblVoterData].[District] = [tblDistricts].[District] LEFT JOIN [dbo].[tblLogCodes] ON [tblVoterData].[LogCode] = [tblLogCodes].[LogCode] Where BarCode = " + BarCode;
            //int key = 10515;
            var sql = ActivityToDateSummarySQL();

            // Get data source from report
            FastReport.Data.TableDataSource tds = (FastReport.Data.TableDataSource)rptVoterList.GetDataSource("DailyDetailData");

            // Save query to data source
            tds.SelectCommand = sql;

            //rptVoterList.SetParameterValue("P1", key);

            // Prepare the report for printing or viewing
            rptVoterList.Prepare();

            // Define PDF export object
            FastReport.Export.Pdf.PDFExport export = new FastReport.Export.Pdf.PDFExport();

            // Set PDF settings
            export.PrintOptimized = false;
            export.OpenAfterExport = false;
            export.EmbeddingFonts = true;

            // Create Memory Stream object
            System.IO.MemoryStream s = new System.IO.MemoryStream();

            // Export PDF to memory stream
            rptVoterList.Export(export, s);
            s.Position = 0;

            ViewBag.Result = "True";

            // Return the memory stream object preloaded with the report PDF
            return new FileStreamResult(s, "application/pdf");
        }

        public FileStreamResult SiteSummaryPDF()
        {
            // Disable extra notifications while on the web
            FastReport.Utils.Config.WebMode = true;

            // Create new report object
            FastReport.Report rptVoterList = new FastReport.Report();
            // Load Report 1 into the report opbject
            rptVoterList.Load(Server.MapPath("../Reports/SummaryBySiteEVote.frx"));

            rptVoterList.Dictionary.Connections[0].ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EVoteSQLDataConnectionString"].ConnectionString;

            // Define new query
            //var sql = "SELECT [tblVoterData].*, [tblDistricts].[DistrictName], [tblLogCodes].[LogDescription] FROM [dbo].[tblVoterData] LEFT JOIN [dbo].[tblDistricts] ON [tblVoterData].[District] = [tblDistricts].[District] LEFT JOIN [dbo].[tblLogCodes] ON [tblVoterData].[LogCode] = [tblLogCodes].[LogCode] Where BarCode = " + BarCode;
            //int key = 10515;
            var sql = ActivityToDateSummarySQL();

            // Get data source from report
            FastReport.Data.TableDataSource tds = (FastReport.Data.TableDataSource)rptVoterList.GetDataSource("DailyDetailData");

            // Save query to data source
            tds.SelectCommand = sql;

            //rptVoterList.SetParameterValue("P1", key);

            // Prepare the report for printing or viewing
            rptVoterList.Prepare();

            // Define PDF export object
            FastReport.Export.Pdf.PDFExport export = new FastReport.Export.Pdf.PDFExport();

            // Set PDF settings
            export.PrintOptimized = false;
            export.OpenAfterExport = false;
            export.EmbeddingFonts = true;

            // Create Memory Stream object
            System.IO.MemoryStream s = new System.IO.MemoryStream();

            // Export PDF to memory stream
            rptVoterList.Export(export, s);
            s.Position = 0;

            ViewBag.Result = "True";

            // Return the memory stream object preloaded with the report PDF
            return new FileStreamResult(s, "application/pdf");
        }

        private string TodaysActivitySQL()
        {
            string SQL = "SELECT ";

            SQL += "ReportTitle = 'Today''s Activity' ";
            SQL += ",[BarCode] ";
            SQL += ",[ElectionID] ";
            SQL += ",[tblVoterDatas].[District] ";
            SQL += ",[tblDistricts].[DistrictName] ";
            SQL += ",[tblVoterDatas].[BallotStyle] ";
            SQL += ",[VoterID] ";
            SQL += ",[VoterNo] ";
            SQL += ",[RosterIndex] ";
            SQL += ",[CourtesyTitle] ";
            SQL += ",[LastName] ";
            SQL += ",[FirstName] ";
            SQL += ",[MiddleName] ";
            SQL += ",[Generation] ";
            SQL += ",[MaidenName] ";
            SQL += ",[Address1] ";
            SQL += ",[Address2] ";
            SQL += ",[City] ";
            SQL += ",[State] ";
            SQL += ",[Zip] ";
            SQL += ",[PhysicalAddress] ";
            SQL += ",[PhysicalAddress2] ";
            SQL += ",[PhysicalCity] ";
            SQL += ",[PhysicalState] ";
            SQL += ",[PhysicalZip] ";
            SQL += ",[PhysicalCSZ] ";
            SQL += ",[Phone] ";
            SQL += ",[DOB] ";
            SQL += ",[DOBSearch] ";
            SQL += ",[TempUsed] ";
            SQL += ",[TempAddress1] ";
            SQL += ",[TempAddress2] ";
            SQL += ",[TempCity] ";
            SQL += ",[TempState] ";
            SQL += ",[TempZip] ";
            SQL += ",[OutofCountry] ";
            SQL += ",[TempProvince] ";
            SQL += ",[TempCountry] ";
            SQL += ",[tblVoterDatas].[LogCode] ";
            SQL += ",[tblLogCodes].LogDescription ";
            SQL += ",[LogDate] ";
            SQL += ",[LogToday] ";
            SQL += ",[BallotPrinted] ";
            SQL += ",[PrintedDate] ";
            SQL += ",[BallotNo] ";
            SQL += ",[SpoiledReasonID] ";
            SQL += ",[Site] ";
            SQL += ",[Machine] ";
            SQL += ",[UserName] ";
            SQL += ",[ReqAbs] ";
            SQL += ",[Reservation] ";
            SQL += ",[Registered] ";
            SQL += ",[RegisteredDate] ";
            SQL += ",[ActivityDate] ";
            SQL += ",ElectionName = '" + Session["ElectionName"].ToString() + "' ";

            string electionDate = Session["ElectionDate"].ToString();
            SQL += ",DATENAME(dw, '" + electionDate + "') + ', ' + DATENAME(MONTH, '" + electionDate + "') + ' ' + CONVERT(VARCHAR(2), DAY('" + electionDate + "')) + ', ' + CONVERT(VARCHAR(4), YEAR('" + electionDate + "')) AS ElectionDateLong ";

            SQL += "FROM [dbo].[tblVoterDatas] ";

            SQL += "LEFT OUTER JOIN [dbo].[tblLogCodes] ";
            SQL += "ON [tblVoterDatas].[LogCode] = [tblLogCodes].LogCode ";

            SQL += "LEFT OUTER JOIN [dbo].[tblDistricts] ";
            SQL += "ON [tblVoterDatas].[District] = [tblDistricts].[District] ";

            SQL += "WHERE [tblVoterDatas].[LogCode] IN (7,10,11,12) ";
            SQL += "AND CAST([LogDate] AS DATE) = CAST(CONVERT(DATETIME,'" + DateTime.Now + "') AS DATE) ";

            return SQL;
        }

        private string ActivityToDateSQL()
        {
            string SQL = "SELECT ";

            SQL += "ReportTitle = 'Activity To Date' ";
            SQL += ",[BarCode] ";
            SQL += ",[ElectionID] ";
            SQL += ",[tblVoterDatas].[District] ";
            SQL += ",[tblDistricts].[DistrictName] ";
            SQL += ",[tblVoterDatas].[BallotStyle] ";
            SQL += ",[VoterID] ";
            SQL += ",[VoterNo] ";
            SQL += ",[RosterIndex] ";
            SQL += ",[CourtesyTitle] ";
            SQL += ",[LastName] ";
            SQL += ",[FirstName] ";
            SQL += ",[MiddleName] ";
            SQL += ",[Generation] ";
            SQL += ",[MaidenName] ";
            SQL += ",[Address1] ";
            SQL += ",[Address2] ";
            SQL += ",[City] ";
            SQL += ",[State] ";
            SQL += ",[Zip] ";
            SQL += ",[PhysicalAddress] ";
            SQL += ",[PhysicalAddress2] ";
            SQL += ",[PhysicalCity] ";
            SQL += ",[PhysicalState] ";
            SQL += ",[PhysicalZip] ";
            SQL += ",[PhysicalCSZ] ";
            SQL += ",[Phone] ";
            SQL += ",[DOB] ";
            SQL += ",[DOBSearch] ";
            SQL += ",[TempUsed] ";
            SQL += ",[TempAddress1] ";
            SQL += ",[TempAddress2] ";
            SQL += ",[TempCity] ";
            SQL += ",[TempState] ";
            SQL += ",[TempZip] ";
            SQL += ",[OutofCountry] ";
            SQL += ",[TempProvince] ";
            SQL += ",[TempCountry] ";
            SQL += ",[tblVoterDatas].[LogCode] ";
            SQL += ",[tblLogCodes].LogDescription ";
            SQL += ",[LogDate] ";
            SQL += ",[LogToday] ";
            SQL += ",[BallotPrinted] ";
            SQL += ",[PrintedDate] ";
            SQL += ",[BallotNo] ";
            SQL += ",[SpoiledReasonID] ";
            SQL += ",[Site] ";
            SQL += ",[Machine] ";
            SQL += ",[UserName] ";
            SQL += ",[ReqAbs] ";
            SQL += ",[Reservation] ";
            SQL += ",[Registered] ";
            SQL += ",[RegisteredDate] ";
            SQL += ",[ActivityDate] ";
            SQL += ",ElectionName = '" + Session["ElectionName"].ToString() + "' ";

            string electionDate = Session["ElectionDate"].ToString();
            SQL += ",DATENAME(dw, '" + electionDate + "') + ', ' + DATENAME(MONTH, '" + electionDate + "') + ' ' + CONVERT(VARCHAR(2), DAY('" + electionDate + "')) + ', ' + CONVERT(VARCHAR(4), YEAR('" + electionDate + "')) AS ElectionDateLong ";

            SQL += "FROM [dbo].[tblVoterDatas] ";

            SQL += "LEFT OUTER JOIN [dbo].[tblLogCodes] ";
            SQL += "ON [tblVoterDatas].[LogCode] = [tblLogCodes].LogCode ";

            SQL += "LEFT OUTER JOIN [dbo].[tblDistricts] ";
            SQL += "ON [tblVoterDatas].[District] = [tblDistricts].[District] ";

            SQL += "WHERE [tblVoterDatas].[LogCode] IN (7,10,11,12) ";

            return SQL;
        }

        private string TodaysActivitySummarySQL()
        {
            string SQL = "SELECT ";

            SQL += "ReportTitle = 'Today''s Activity' ";
            SQL += ",[BarCode] ";
            SQL += ",[ElectionID] ";
            SQL += ",[tblVoterDatas].[District] ";
            SQL += ",[tblDistricts].[DistrictName] ";
            SQL += ",[tblVoterDatas].[BallotStyle] ";
            SQL += ",[VoterID] ";
            SQL += ",[VoterNo] ";
            SQL += ",[RosterIndex] ";
            SQL += ",[CourtesyTitle] ";
            SQL += ",[LastName] ";
            SQL += ",[FirstName] ";
            SQL += ",[MiddleName] ";
            SQL += ",[Generation] ";
            SQL += ",[MaidenName] ";
            SQL += ",[Address1] ";
            SQL += ",[Address2] ";
            SQL += ",[City] ";
            SQL += ",[State] ";
            SQL += ",[Zip] ";
            SQL += ",[PhysicalAddress] ";
            SQL += ",[PhysicalAddress2] ";
            SQL += ",[PhysicalCity] ";
            SQL += ",[PhysicalState] ";
            SQL += ",[PhysicalZip] ";
            SQL += ",[PhysicalCSZ] ";
            SQL += ",[Phone] ";
            SQL += ",[DOB] ";
            SQL += ",[DOBSearch] ";
            SQL += ",[TempUsed] ";
            SQL += ",[TempAddress1] ";
            SQL += ",[TempAddress2] ";
            SQL += ",[TempCity] ";
            SQL += ",[TempState] ";
            SQL += ",[TempZip] ";
            SQL += ",[OutofCountry] ";
            SQL += ",[TempProvince] ";
            SQL += ",[TempCountry] ";
            SQL += ",[tblVoterDatas].[LogCode] ";
            SQL += ",[tblLogCodes].LogDescription ";
            SQL += ",[LogDate] ";
            SQL += ",[LogToday] ";
            SQL += ",[BallotPrinted] ";
            SQL += ",[PrintedDate] ";
            SQL += ",[BallotNo] ";
            SQL += ",[SpoiledReasonID] ";
            SQL += ",[Site] ";
            SQL += ",[Machine] ";
            SQL += ",[UserName] ";
            SQL += ",[ReqAbs] ";
            SQL += ",[Reservation] ";
            SQL += ",[Registered] ";
            SQL += ",[RegisteredDate] ";
            SQL += ",[ActivityDate] ";
            SQL += ",ElectionName = '" + Session["ElectionName"].ToString() + "' ";

            string electionDate = Session["ElectionDate"].ToString();
            SQL += ",DATENAME(dw, '" + electionDate + "') + ', ' + DATENAME(MONTH, '" + electionDate + "') + ' ' + CONVERT(VARCHAR(2), DAY('" + electionDate + "')) + ', ' + CONVERT(VARCHAR(4), YEAR('" + electionDate + "')) AS ElectionDateLong ";

            SQL += "FROM [dbo].[tblVoterDatas] ";

            SQL += "LEFT OUTER JOIN [dbo].[tblLogCodes] ";
            SQL += "ON [tblVoterDatas].[LogCode] = [tblLogCodes].LogCode ";

            SQL += "LEFT OUTER JOIN [dbo].[tblDistricts] ";
            SQL += "ON [tblVoterDatas].[District] = [tblDistricts].[District] ";

            SQL += "WHERE [tblVoterDatas].[LogCode] > 1";
            SQL += "AND CAST([LogDate] AS DATE) = CAST(CONVERT(DATETIME,'" + DateTime.Now + "') AS DATE) ";

            return SQL;
        }

        private string ActivityToDateSummarySQL()
        {
            string SQL = "SELECT ";

            SQL += "ReportTitle = 'Activity To Date' ";
            SQL += ",[BarCode] ";
            SQL += ",[ElectionID] ";
            SQL += ",[tblVoterDatas].[District] ";
            SQL += ",[tblDistricts].[DistrictName] ";
            SQL += ",[tblVoterDatas].[BallotStyle] ";
            SQL += ",[VoterID] ";
            SQL += ",[VoterNo] ";
            SQL += ",[RosterIndex] ";
            SQL += ",[CourtesyTitle] ";
            SQL += ",[LastName] ";
            SQL += ",[FirstName] ";
            SQL += ",[MiddleName] ";
            SQL += ",[Generation] ";
            SQL += ",[MaidenName] ";
            SQL += ",[Address1] ";
            SQL += ",[Address2] ";
            SQL += ",[City] ";
            SQL += ",[State] ";
            SQL += ",[Zip] ";
            SQL += ",[PhysicalAddress] ";
            SQL += ",[PhysicalAddress2] ";
            SQL += ",[PhysicalCity] ";
            SQL += ",[PhysicalState] ";
            SQL += ",[PhysicalZip] ";
            SQL += ",[PhysicalCSZ] ";
            SQL += ",[Phone] ";
            SQL += ",[DOB] ";
            SQL += ",[DOBSearch] ";
            SQL += ",[TempUsed] ";
            SQL += ",[TempAddress1] ";
            SQL += ",[TempAddress2] ";
            SQL += ",[TempCity] ";
            SQL += ",[TempState] ";
            SQL += ",[TempZip] ";
            SQL += ",[OutofCountry] ";
            SQL += ",[TempProvince] ";
            SQL += ",[TempCountry] ";
            SQL += ",[tblVoterDatas].[LogCode] ";
            SQL += ",[tblLogCodes].LogDescription ";
            SQL += ",[LogDate] ";
            SQL += ",[LogToday] ";
            SQL += ",[BallotPrinted] ";
            SQL += ",[PrintedDate] ";
            SQL += ",[BallotNo] ";
            SQL += ",[SpoiledReasonID] ";
            SQL += ",[Site] ";
            SQL += ",[Machine] ";
            SQL += ",[UserName] ";
            SQL += ",[ReqAbs] ";
            SQL += ",[Reservation] ";
            SQL += ",[Registered] ";
            SQL += ",[RegisteredDate] ";
            SQL += ",[ActivityDate] ";
            SQL += ",ElectionName = '" + Session["ElectionName"].ToString() + "' ";

            string electionDate = Session["ElectionDate"].ToString();
            SQL += ",DATENAME(dw, '" + electionDate + "') + ', ' + DATENAME(MONTH, '" + electionDate + "') + ' ' + CONVERT(VARCHAR(2), DAY('" + electionDate + "')) + ', ' + CONVERT(VARCHAR(4), YEAR('" + electionDate + "')) AS ElectionDateLong ";

            SQL += "FROM [dbo].[tblVoterDatas] ";

            SQL += "LEFT OUTER JOIN [dbo].[tblLogCodes] ";
            SQL += "ON [tblVoterDatas].[LogCode] = [tblLogCodes].LogCode ";

            SQL += "LEFT OUTER JOIN [dbo].[tblDistricts] ";
            SQL += "ON [tblVoterDatas].[District] = [tblDistricts].[District] ";

            SQL += "WHERE [tblVoterDatas].[LogCode] > 1 ";

            return SQL;
        }
    }
}