using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EVote.DataModels;
using EVote.DataMethods;
using EVote.Filters;
using EVote.Factories;

namespace EVote.Controllers
{
    [VerifyUser]
    public class VoterController : Controller
    {
        ConfigurationModel _Session = ConfigurationMethods.SessionConfigs();

        public ActionResult Index(string id)
        {
            // Change the default behavior based on config setting
            if (Session["SearchDate"].ToString() == "True")
            {
                // Default to the Date Search page
                if (id == "fullname") return View("FullSearch");
                else
                {
                    SetDateLists();
                    return View("DateSearch");
                }
            }
            else
            {
                // Default to the Name Search page
                if (id == "date")
                {
                    SetDateLists();
                    return View("DateSearch");
                }
                else
                {
                    return View("FullSearch");
                }                
            }
        }

        // Full name and date search page
        public ActionResult FullSearch()
        {
            //ViewBag.Results = string.Format("../Home/Login", HttpContext.Request.Url.AbsolutePath);
            ViewBag.Results = Url.Content("~/").ToString();
            return View();
        }

        // Search by birth date page
        public ActionResult DateSearch()
        {
            SetDateLists();

            return View();
        }

        // Get the Month, Day, and Year lists for the birthday search page
        public void SetDateLists()
        {
            int startYear = 1900; // Initialize Date Range
            int defaultAge = 20; // This sets the value the Year drop down displays when the page loads // Example: Now - 20 = 1997

            // Get list of years
            ViewBag.BirthYearList = ListMethods.BirthYearList(startYear, defaultAge);

            // Get list of months 
            if (Session["MonthNames"].ToString() == "True")
            {
                ViewBag.BirthMonthList = ListMethods.BirthMonthNames();
            }
            else
            {
                ViewBag.BirthMonthList = ListMethods.BirthMonthList();
            }

            // Get list of Days in each month
            ViewBag.BirthDayList = ListMethods.BirthDayList();
        }

        // returns the partial view of voters from a full name at date search
        public ActionResult VoterExtendedSearch(string strRoll, string strLastName, string strFirstName, string strDate)
        {
            // Get the list of voters            
            IEnumerable<VoterDataModel> voterList = VoterDataMethods.FullSearch(strRoll, strLastName, strFirstName, strDate);

            List<VoterDataModel> newVoterList = new List<VoterDataModel>();
            foreach (var voter in voterList)
            {
                voter.ValidLocation = ValidateVotersDistrict(voter);
                newVoterList.Add(voter);
            }

            // Check for empty list
            if (voterList != null) ViewBag.EmptyList = voterList.Count();
            else ViewBag.EmptyList = 0;

            ViewBag.NoDistrictNoVote = (bool)(Session["NoDistrictNoVote"].ToString() == "True");
            ViewBag.DistrictSignIn = (bool)(Session["DistrictSignIn"].ToString() == "True");
            ViewBag.ShowDistrict = (bool)(Session["ShowDistrict"].ToString() == "True");

            return PartialView("_List", voterList);
        }

        public ActionResult VoterModelSearch(VoterSearchModel search)
        {
            //Session.Abandon();

            // Get the list of voters            
            IEnumerable<VoterDataModel> voterList = VoterDataMethods.ModelSearch(search);

            List<VoterDataModel> newVoterList = new List<VoterDataModel>();
            foreach (var voter in voterList)
            {
                voter.ValidLocation = ValidateVotersDistrict(voter);
                newVoterList.Add(voter);
            }

            // Check for empty list
            if (voterList != null) ViewBag.EmptyList = voterList.Count();
            else ViewBag.EmptyList = 0;

            ViewBag.NoDistrictNoVote = (bool)(Session["NoDistrictNoVote"].ToString() == "True");
            ViewBag.DistrictSignIn = (bool)(Session["DistrictSignIn"].ToString() == "True");
            ViewBag.ShowDistrict = (bool)(Session["ShowDistrict"].ToString() == "True");

            return PartialView("_List", voterList);
        }

        // returns the partial view of voters from a birth date search
        public ActionResult VoterDateSearch(string strDate)
        {
            //Session["UserID"] = null;
            //Session.Abandon(); 

            // Get the list of voters            
            IEnumerable<VoterDataModel> voterList = VoterDataMethods.DateSearch(strDate);

            List<VoterDataModel> newVoterList = new List<VoterDataModel>();
            foreach (var voter in voterList)
            {
                voter.ValidLocation = ValidateVotersDistrict(voter);
                newVoterList.Add(voter);
            }

            // Check for empty list
            if (voterList != null) ViewBag.EmptyList = voterList.Count();
            else ViewBag.EmptyList = 0;

            ViewBag.NoDistrictNoVote = (bool)(Session["NoDistrictNoVote"].ToString() == "True");
            ViewBag.DistrictSignIn = (bool)(Session["DistrictSignIn"].ToString() == "True");
            ViewBag.ShowDistrict = (bool)(Session["ShowDistrict"].ToString() == "True");

            return PartialView("_List", voterList);
        }

        // Get Json list of days in month 
        // Json list will be used to stuff the client side drop down list
        // Year parameter added for calculating leap years
        public virtual JsonResult BirthDayListJson(int intMonth, int intYear)
        {
            var DayList = ListMethods.BirthDayLeap(intMonth, intYear);

            // Convert list object to Json object
            //string jsonList = new JavaScriptSerializer().Serialize(DayList);
            return Json(DayList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MonthsOfTheYear(string selectedmonth)
        {
            var list = ListMethods.BirthMonthNames();
            return PartialView("_MonthsOfYear", new SelectList(list, "Value", "Text", selectedmonth));
        }

        public ActionResult DaysOfTheMonth(string selectedday)
        {
            var list = ListMethods.BirthDayList();            
            return PartialView("_DaysOfMonth", new SelectList(list, "Value", "Text", selectedday));
        }

        public ActionResult BirthYears(string selectedyear)
        {
            int startYear = 1900; // Initialize Date Range
            int defaultAge = 18; // This sets the value the Year drop down displays when the page loads // Example: Now - 20 = 1997

            // Get list of years
            var list = ListMethods.BirthYearList(startYear, defaultAge);
            return PartialView("_BirthYears", new SelectList(list, "Value", "Text", selectedyear));
        }

        public ActionResult TestSearch()
        {
            return View();
        }

        // Edit Voter Details Page
        public ActionResult District(int BarCode)
        {
            //// Admins, Devs and SuperUsers can access this page
            //if (Convert.ToInt32(Session["RoleID"]) > 1)
            //{
                // Set drop down list objects
                ViewBag.DistrictList = ListMethods.DistrictList(null);
                ViewBag.LogCodeList = ListMethods.LogCodeList(null);
                ViewBag.SitesList = ListMethods.SitesList(0);

                // Pass voter details to view
                var voter = VoterDataMethods.SingleVoter(BarCode);

                // Display the full voter status description
                ViewBag.VoterStatus = LogCodeMethods.LogDescription(voter.LogCode);

                ViewBag.Error = "";
                return View(voter);
            //}
            //else
            //{
            //    return RedirectToAction("Login", "Home");
            //}
        }

        [HttpPost]
        public ActionResult District(VoterDataModel voterFromForm)
        {
            if (Session["DistrictOnly"].ToString() == "True")
            {
                var result = ValidateVoterDistrict(voterFromForm);
                if (result != null)
                {
                    return result;
                }
            }
            else
            {
                // Vadiate birthdate since standard validation dosnt work            
                if (voterFromForm.ValidateBirthdate())
                {
                    // GEt new combo number
                    voterFromForm.ComboNo = DistrictMethods.GetDistrict(voterFromForm.District).ComboNo;

                    // Update the changes submitted from the client
                    var result = VoterDataMethods.UpdateVoter(voterFromForm);

                    if (result == true)
                    {
                        ViewBag.Results = "Voter saved successfully";
                    }
                    else
                    {
                        ViewBag.Results = "Voter save unsuccessful";
                    }
                }
                else
                {
                    ModelState.AddModelError("DOBSearch", "Enter a valid birthdate");
                }

                // Compare Site to selected district
                if (voterFromForm.District != 0)
                {
                    return RedirectToAction("Index", "Signature", new { barCode = voterFromForm.BarCode });
                }
            }

            // Reset the drop down lists
            ViewBag.DistrictList = ListMethods.DistrictList(null);
            ViewBag.LogCodeList = ListMethods.LogCodeList(null);
            ViewBag.SitesList = ListMethods.SitesList(0);

            // Pass voter object to view
            ViewBag.Error = "Incorrect district for this site.";
            return View(VoterDataMethods.SingleVoter(voterFromForm.BarCode));
        }

        private ActionResult ValidateVoterDistrict(VoterDataModel voterFromForm)
        {
            int userId = 0;
            if (Int32.TryParse(Session["UserID"].ToString(), out userId))
            {
                // Get the list of user sites from the voters district
                var udFactory = new UserDistrictsFactory();
                var userDistrictsList = udFactory.GetUserDistricts(userId, voterFromForm.District ?? 0);

                if ((userDistrictsList != null && userDistrictsList.Count() > 0))
                {
                    return RedirectToAction("Index", "Signature", new { barCode = voterFromForm.BarCode });
                }
            }

            // Default result
            return null;
        }

        private bool ValidateVotersDistrict(VoterDataModel voter)
        {
            int userId = 0;
            if (Int32.TryParse(Session["UserID"].ToString(), out userId))
            {
                // Get the list of user sites from the voters district
                var udFactory = new UserDistrictsFactory();
                var userDistrictsList = udFactory.GetUserDistricts(userId, voter.District ?? 0);

                if ((userDistrictsList != null && userDistrictsList.Count() > 0))
                {
                    return true;
                }
            }

            // Default result
            return false;
        }
    }
}