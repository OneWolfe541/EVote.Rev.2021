using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EVote.DataModels;
using EVote.DataMethods;
using EVote.Filters;

namespace EVote.Controllers
{
    [VerifySuperUser]
    public class AbsenteeController : Controller
    {
        //private static EVoteVoterDataDataContext _EVote = new EVoteVoterDataDataContext();

        public ActionResult Index()
        {
            return View("FullSearch");
        }

        // Full name and date search page
        public ActionResult FullSearch()
        {
            return View();
        }

        // returns the partial view of voters from a full name at date search
        public ActionResult VoterExtendedSearch(string strRoll, string strLastName, string strFirstName, string strDate)
        {
            // Get the list of voters            
            IEnumerable<VoterDataModel> voterList = VoterDataMethods.FullSearch(strRoll, strLastName, strFirstName, strDate);

            // Check for empty list
            if (voterList != null) ViewBag.EmptyList = voterList.Count();
            else ViewBag.EmptyList = 0;

            return PartialView("_List", voterList);
        }

        public ActionResult VoterModelSearch(VoterSearchModel search)
        {
            // Get the list of voters            
            IEnumerable<VoterDataModel> voterList = VoterDataMethods.ModelSearch(search);

            // Check for empty list
            if (voterList != null) ViewBag.EmptyList = voterList.Count();
            else ViewBag.EmptyList = 0;

            return PartialView("_List", voterList);
        }

        // Edit Voter Details Page
        public ActionResult Edit(int BarCode)
        {
            // Pass voter details to view
            var voter = VoterDataMethods.SingleVoter(BarCode);

            // Set drop down list objects
            ViewBag.DistrictList = ListMethods.DistrictList(null);
            ViewBag.LogCodeList = ListMethods.AbsenteeLogCodeList(voter.LogCode);
            //ViewBag.LogCodeList = ListMethods.LogCodeList(null);
            ViewBag.SitesList = ListMethods.SitesList(0);

            // Display the full voter status description
            ViewBag.VoterStatus = LogCodeMethods.LogDescription(voter.LogCode);

            return View(voter);
        }

        [HttpPost]
        public ActionResult Edit(VoterDataModel voterFromForm)
        {
            if (Session["Username"] != null)
            {
                voterFromForm.UserName = Session["Username"].ToString();
            }

            // Update the changes submitted from the client
            var result = VoterDataMethods.UpdateAbsenteeVoter(voterFromForm);

            if (result == true)
            {
                ViewBag.Results = "Voter saved successfully";
                ViewBag.SaveStatus = "True";
            }
            else
            {
                ViewBag.Results = "Voter save unsuccessful";
            }

            // Reset the drop down lists
            ViewBag.DistrictList = ListMethods.DistrictList(null);
            ViewBag.LogCodeList = ListMethods.AbsenteeLogCodeList(null);
            ViewBag.SitesList = ListMethods.SitesList(0);

            // Display the full voter status description
            ViewBag.VoterStatus = LogCodeMethods.LogDescription(voterFromForm.LogCode);

            // Pass voter object to view
            return View(VoterDataMethods.SingleVoter(voterFromForm.BarCode));
            //return RedirectToAction("Index");
        }
    }
}