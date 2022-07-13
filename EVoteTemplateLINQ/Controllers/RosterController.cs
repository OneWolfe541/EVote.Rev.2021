using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EVote.DataModels;
using EVote.DataMethods;
using EVote.Filters;
using EVote.Extensions;

namespace EVote.Controllers
{
    [VerifyUser]
    public class RosterController : Controller
    {
        ConfigurationModel _Session = ConfigurationMethods.SessionConfigs();

        // GET: Roster
        public ActionResult Index(int? page)
        {
            return View("FullSearch", InitializeRoster(page));
        }

        // Full name at date search page
        public ActionResult FullSearch(int? page)
        {
            return View(InitializeRoster(page));
        }

        // Pull most recent voters page by page
        public IEnumerable<VoterDataModel> InitializeRoster(int? page)
        {
            // Initialize page number
            if (page.IsNull()) page = 1;

            int size = Int32.Parse(Session["PageSize"].ToString()); // Set to session varriable

            IEnumerable<VoterDataModel> voterRoster;

            // Get the list of voters or voters from a specific site
            if (Session["SiteOnlyRoster"].ToString() == "True")
            {
                var site = Session["Username"].ToString();
                voterRoster = VoterDataMethods.RosterBySiteList(site);
            }
            else
            {
                voterRoster = VoterDataMethods.RosterList();
            }

            // Check for empty list
            ViewBag.EmptyList = VoterDataMethods.VoterCount(voterRoster);

            int maxPage = (ViewBag.EmptyList / size) + 1;            

            // set page limits
            if (page < 1) page = 1;
            if (page > maxPage) page = maxPage;

            // Set page counter display
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = maxPage;

            // Returned a paged list of voters
            if (Session["Registration"].ToString() != "True")
            {
                return voterRoster.OrderByDescending(o => o.LogDate).Skip((int)(page - 1) * size).Take(size);
            }
            else
            {                
                return voterRoster.OrderByDescending(o => o.RegisteredDate).Skip((int)(page - 1) * size).Take(size);
            }                
        }

        // returns the partial view of voters from a full name at date search
        public ActionResult VoterExtendedSearch(string strRoll, string strLastName, string strFirstName, string strDate)
        {
            // Get the list of voters
            IEnumerable<VoterDataModel> voterRoster;
            if (Session["SiteOnlyRoster"].ToString() == "True")
            {
                var site = Session["Username"].ToString();
                voterRoster = VoterDataMethods.RosterSiteSearch(strRoll, strLastName, strFirstName, strDate, site);
            }
            else
            {
                voterRoster = VoterDataMethods.RosterSearch(strRoll, strLastName, strFirstName, strDate);
            }

            // Check for empty list
            ViewBag.EmptyList = VoterDataMethods.VoterCount(voterRoster);

            return PartialView("_List", voterRoster.OrderByDescending(o => o.LogDate));
        }

        public ActionResult VoterModelSearch(VoterSearchModel search)
        {
            // Get the list of voters            
            // Get the list of voters            
            IEnumerable<VoterDataModel> voterRoster;
            if (Session["SiteOnlyRoster"].ToString() == "True")
            {
                var site = Session["Username"].ToString();
                voterRoster = VoterDataMethods.RosterSiteModelSearch(search, site);
            }
            else
            {
                voterRoster = VoterDataMethods.RosterModelSearch(search);
            }

            // Check for empty list
            if (voterRoster != null) ViewBag.EmptyList = voterRoster.Count();
            else ViewBag.EmptyList = 0;

            return PartialView("_List", voterRoster.OrderByDescending(o => o.LogDate));
        }
    }
}