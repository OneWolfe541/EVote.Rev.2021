using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EVote.DataModels;
using static EVote.DataMethods.ConfigurationMethods;
using static EVote.DataMethods.ListMethods;
using EVote.Filters;
using EVote.DataMethods;
using EVote.Factories;

namespace EVote.Controllers
{
    [VerifyAdmin]
    public class SetupController : Controller
    {        
        public ActionResult Index(string id)
        {
            //ViewBag.UserList = SitesList(0);

            // All of this redirection just causes infinite loops!!!!
            //if(id == "pagesetup") return View("Setup", SetupConfigsList(0));
            //else if(id == "GetElectionDetails") return PartialView("_ElectionDetails", ElectionDetailsList());
            //else
            //    return View("Configure", BasicConfigsList(0));

            //if (id == "basic") return View("Configure", BasicConfigsList(0));
            //else if (id == "pagesetup")
            //{
            //    return View("Setup", SetupConfigsList(0));
            //}
            return View();
        }

        public ActionResult Configure(int? UserId)
        {
            if (UserId == null)
            {
                UserId = 0;
            }

            // Get system user list, also used for site names
            ViewBag.UserList = SitesList(UserId);

            return View(BasicConfigsList(UserId));
        }

        //public ActionResult Setup(int? UserId)
        //{
        //    if (UserId == null)
        //    {
        //        UserId = 0;
        //    }

        //    // Get system user list, also used for site names
        //    ViewBag.UserList = SitesList(UserId);

        //    return View(SetupConfigsList(UserId));
        //}

        public ActionResult Setup(string UserName)
        {
            int UserId = 0;
            if (UserName != null)
            {
                try
                {
                    UserId = UserMethods.GetUser(UserName).UserId;
                }
                catch { }
            }

            // Get system user list, also used for site names
            ViewBag.UserList = SitesList(UserName);

            return View(SetupConfigsList(UserId));
        }

        public ActionResult SaveSetting(string source, int WebConfigID, string ConfigSetting, string ConfigValue, int? UserId)
        {
            if (UserId == null || UserId == 0)
            {
                UserId = 0;
            }

            // Get system user list, also used for site names
            ViewBag.UserList = SitesList(UserId);

            // Set configuration to the new value
            SetConfigValue(WebConfigID, ConfigValue);

            // Return to the source page
            if (source == "Setup")
            {
                return View("Setup", SetupConfigsList(UserId));
            }
            else
            {
                return View("Configure", BasicConfigsList(UserId));
            }
        }

        public ActionResult GetElectionDetails()
        {
            // Election details can be found under userId "NULL"
            return PartialView("_ElectionDetails", ElectionDetailsList());
        }

        public ActionResult SiteDistricts()
        {
            ViewBag.DistrictList = ListMethods.DistrictList();

            // Get list of sites that does not include system users
            List<int> invalidUsers = new List<int> { 0, 1, 2, 3 };
            ViewBag.SiteList = ListMethods.SitesList().Where(s => !invalidUsers.Contains(s.UserId));

            var factory = new UserDistrictsFactory();
            return View(factory.List().OrderBy(o => o.DistrictId).ThenBy(o => o.UserId));
        }

        public JavaScriptResult AddSiteDistrict(int Site, int District)
        {
            // Save the record
            var factory = new UserDistrictsFactory();
            var result = factory.InsertUserDistricts(Site, District);

            return JavaScript("location.reload(true)");
        }

        public bool SaveSiteDistrict(int Id, int Site, int District)
        {
            // Save the record
            var factory = new UserDistrictsFactory();
            return factory.UpdateUserDistricts(Id, Site, District);
        }

        public bool DeleteSiteDistrict(int Id)
        {
            // Save the record
            var factory = new UserDistrictsFactory();
            return factory.DeleteUserDistricts(Id);
        }
    }
}