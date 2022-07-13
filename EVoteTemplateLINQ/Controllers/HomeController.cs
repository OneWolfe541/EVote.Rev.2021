using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EVote.DataMethods;
using EVote.DataModels;
using EVote.Filters;
using EVote.Context;

namespace EVote.Controllers
{
    public class HomeController : Controller
    {
        [VerifyUser]
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                if (Session["ClerkMode"].ToString() == "True")
                {
                    if (Session["AllMailMode"].ToString() == "True")
                    {
                        return RedirectToAction("AllMailTracking", "Stats");
                    }
                    else
                    {
                        return RedirectToAction("ElectionTracking", "Stats");
                    }
                }

                if (Session["ePollBook"].ToString() == "True")
                    return RedirectToAction("Index", "Voter");

                if (Session["Absentee"].ToString() == "True")
                    return RedirectToAction("Index", "Absentee");

                if (Session["Registration"].ToString() == "True")
                    return RedirectToAction("Index", "Registration");

                //return RedirectToAction("Empty", "Home");
                return RedirectToAction("Index", "Voter");
            }
            else
            {
                ViewBag.LogError = "A general error occured while processing this request.";
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Reload()
        {
            return RedirectToAction("Login", "Home");
        }

        // Create navbar menu based on user settings
        public ActionResult NavMenu()
        {
            // Convert Session Varriables to a configuration object
            ConfigurationModel _Session = ConfigurationMethods.SessionConfigs();

            // Create empty list object
            List<NavigationMenuModel> menu = new List<NavigationMenuModel>();

            if (Session["ClerkMode"].ToString() == "True")
            {
                if (Session["AllMailMode"].ToString() == "True")
                {
                    menu.Add(new NavigationMenuModel { Name = "Stats", Action = "AllMailTracking", Controler = "Stats" });
                }
                else
                {
                    menu.Add(new NavigationMenuModel { Name = "Stats", Action = "ElectionTracking", Controler = "Stats" });
                }

                menu.Add(new NavigationMenuModel { Name = "Absentee", Action = "Index", Controler = "Absentee" });

                menu.Add(new NavigationMenuModel { Name = "Roster", Action = "Index", Controler = "Roster" });

                menu.Add(new NavigationMenuModel { Name = "Add Voter", Action = "Add", Controler = "Manage" });

                menu.Add(new NavigationMenuModel { Name = "Edit Voter", Action = "FullSearch", Controler = "Manage" });

                menu.Add(new NavigationMenuModel { Name = "Reports", Action = "Index", Controler = "Report" });
            }
            else
            {
                // Add list items from user settings
                //if (Session["ePollBook"].ToString() == "True")
                //{
                //    menu.Add(new NavigationMenuModel { Name = "Voter Lookup", Action = "Index", Controler = "Voter" });
                //}
                //else if (Session["Absentee"].ToString() == "True")
                //{
                //    menu.Add(new NavigationMenuModel { Name = "Absentee", Action = "Index", Controler = "Absentee" });
                //}
                //else if (Session["Registration"].ToString() == "True")
                //{
                //    menu.Add(new NavigationMenuModel { Name = "Registration", Action = "Index", Controler = "Registration" });
                //}

                menu.Add(new NavigationMenuModel { Name = "Voter Lookup", Action = "Index", Controler = "Voter" });

                if (Session["ShowEDRoster"].ToString() == "True")
                    menu.Add(new NavigationMenuModel { Name = "Roster", Action = "Index", Controler = "Roster" });

                if (Session["SiteSummary"].ToString() == "True")
                    menu.Add(new NavigationMenuModel { Name = "Summary", Action = "SiteSummary", Controler = "Stats" });

                if (Session["ShowEDActivity"].ToString() == "True")
                    menu.Add(new NavigationMenuModel { Name = "Voting Activity", Action = "Counts", Controler = "Stats" });

                if (Session["ShowEVActivity"].ToString() == "True")
                    menu.Add(new NavigationMenuModel { Name = "Voting Activity", Action = "EVCounts", Controler = "Stats" });

                if (Session["AllElectionCharts"].ToString() == "True")
                    menu.Add(new NavigationMenuModel { Name = "District Activity", Action = "DistrictCounts", Controler = "Stats" });
            }

            return PartialView("_NavMenu", menu);
        }

        [HttpPost]
        public ActionResult Login(tblSystemUser _user)
        {
            try
            {
                var usr = UserMethods.ValidateUser(_user);
                if (usr != null)
                {
                    Session["UserID"] = usr.UserId.ToString();
                    Session["Username"] = usr.UserName.ToString();
                    Session["RoleID"] = usr.RoleID.ToString();
                    SetUserSettings();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is incorrect.");
                    ViewBag.LogError = "Username or Password is incorrect.";
                    return View("Login");
                }
            }
            catch
            { }
            return View("Login");
        }

        public ActionResult Training(string name, string id)
        {
            try
            {
                var usr = UserMethods.ValidateUser(name, id);
                if (usr != null)
                {
                    Session["UserID"] = usr.UserId.ToString();
                    Session["Username"] = usr.UserName.ToString();
                    Session["RoleID"] = usr.RoleID.ToString();
                    SetUserSettings();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is incorrect.");
                    ViewBag.LogError = "Username or Password is incorrect.";
                    return View("Login");
                }
            }
            catch
            { }
            return View("Login");
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["Username"] = null;
            Session["RoleID"] = null;
            Session.Abandon();
            return View("Logout");
        }

        public int? GetUserId()
        {
            if (Session == null || Session["UserID"] == null)
            {
                return null;
            }
            else
            {
                return Convert.ToInt32(Session["UserID"]);
            }
        }

        public void SetUserSettings()
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            // Check if site is in training mode
            string trainingMode = ConfigurationMethods.GetActiveUserValue("TrainingMode", Convert.ToInt32(Session["UserID"]));
            Session["TrainingMode"] = trainingMode;

            // Check if site is in clerk mode
            string clerkMode = ConfigurationMethods.GetActiveUserValue("ClerkMode", Convert.ToInt32(Session["UserID"]));
            Session["ClerkMode"] = clerkMode;

            string allMailMode = ConfigurationMethods.GetActiveUserValue("AllMail", Convert.ToInt32(Session["UserID"]));
            Session["AllMailMode"] = allMailMode;

            // If site has not configs set or is in training mode then use default settings
            if (!ConfigurationMethods.UserHasValues(userId) || trainingMode == "True" || clerkMode == "True")
            {
                userId = 0;
            }
            Session["ElectionName"] = ConfigurationMethods.GetElectionValue("ElectionName");
            Session["ElectionDate"] = ConfigurationMethods.GetElectionValue("ElectionDate");
            Session["ShowDistrict"] = ConfigurationMethods.GetActiveUserValue("ShowDistrict", userId);
            //Session["MaidenName"] = ConfigurationMethods.GetActiveUserValue("ShowMaidenName", userId);
            Session["SiteOnlyRoster"] = ConfigurationMethods.GetActiveUserValue("SiteOnlyRoster", userId);
            Session["SiteOnlyVotes"] = ConfigurationMethods.GetActiveUserValue("SiteOnlyVotes", userId);
            Session["Spoilable"] = ConfigurationMethods.GetActiveUserValue("SpoilBallots", userId);
            Session["SearchDate"] = ConfigurationMethods.GetActiveUserValue("UseDateSearch", userId);
            Session["MonthNames"] = ConfigurationMethods.GetActiveUserValue("UseMonthNames", userId);
            Session["NoDistrictNoVote"] = ConfigurationMethods.GetActiveUserValue("NoDistrictNoVote", userId);
            Session["BallotNumOnSig"] = ConfigurationMethods.GetActiveUserValue("BallotNumOnSig", userId);
            //Session["PrintOnSign"] = ConfigurationMethods.GetActiveUserValue("PrintOnSign", userId);
            Session["Absentee"] = ConfigurationMethods.GetActiveUserValue("Absentee", userId);
            Session["DistrictSignIn"] = ConfigurationMethods.GetActiveUserValue("DistrictSignIn", userId);
            Session["ePollBook"] = ConfigurationMethods.GetActiveUserValue("ePollBook", userId);
            Session["ShowEDRoster"] = ConfigurationMethods.GetActiveUserValue("ShowEDRoster", userId);
            Session["ShowEDActivity"] = ConfigurationMethods.GetActiveUserValue("ShowEDActivity", userId);
            Session["ShowEVActivity"] = ConfigurationMethods.GetActiveUserValue("ShowEVActivity", userId);
            Session["SiteSummary"] = ConfigurationMethods.GetActiveUserValue("SiteSummary", userId);
            Session["Registration"] = ConfigurationMethods.GetActiveUserValue("Registration", userId);
            Session["AllElectionCharts"] = ConfigurationMethods.GetActiveUserValue("AllElectionCharts", userId);
            //Session["CheckNetwork"] = ConfigurationMethods.GetActiveUserValue("SignatureCheckNetwork", userId);
            Session["PageSize"] = ConfigurationMethods.GetActiveUserValue("PageSize", userId);
            Session["DatePicker"] = ConfigurationMethods.GetActiveUserValue("DatePickerSearch", userId);
            Session["DistrictOnly"] = ConfigurationMethods.GetActiveUserValue("DistrictOnlyVoting", userId);

            // Force claerk mode to use the absentee screen
            if (clerkMode == "True")
            {
                Session["ePollBook"] = "False";
                Session["Absentee"] = "True";
                Session["SiteOnlyRoster"] = "False";
            }

            Session["UserSet"] = "True";
        }

        //public void DBContextTest()
        //{
        //    using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
        //    {
        //        var test = bdEVote.Spoileds.ToList();
        //    }
        //}
    }
}