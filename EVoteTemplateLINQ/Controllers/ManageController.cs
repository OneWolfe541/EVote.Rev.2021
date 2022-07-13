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
    public class ManageController : Controller
    {
        //ConfigurationModel _Session = ConfigurationMethods.SessionConfigs();

        public ActionResult Index()
        {
            return View("FullSearch");
        }

        // Full name at date search page
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
            // Admins, Devs and SuperUsers can access this page
            if (Convert.ToInt32(Session["RoleID"]) > 1)
            {
                // Set drop down list objects
                ViewBag.DistrictList = ListMethods.DistrictList(null);
                ViewBag.LogCodeList = ListMethods.LogCodeList(null);
                ViewBag.SitesList = ListMethods.SitesList(0);

                // Pass voter details to view
                var voter = VoterDataMethods.SingleVoter(BarCode);

                // Display the full voter status description
                ViewBag.VoterStatus = LogCodeMethods.LogDescription(voter.LogCode);

                return View(voter);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult Edit(VoterDataModel voterFromForm)
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
                    ViewBag.SaveStatus = "True";
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

            // Reset the drop down lists
            ViewBag.DistrictList = ListMethods.DistrictList(null);
            ViewBag.LogCodeList = ListMethods.LogCodeList(null);
            ViewBag.SitesList = ListMethods.SitesList(0);

            ViewBag.VoterStatus = LogCodeMethods.LogDescription(voterFromForm.LogCode);

            // Pass voter object to view
            return View(VoterDataMethods.SingleVoter(voterFromForm.BarCode));
            //return RedirectToAction("Index");
        }

        // Add Voter Details
        public ActionResult Add()
        {
            // Get list of districts for Drop Down Control
            ViewBag.DistrictList = ListMethods.DistrictList(null);
            ViewBag.newBarCode = VoterDataMethods.GetNextBarcode();

            return View();
        }

        [HttpPost]
        public ActionResult Add(VoterDataModel voterToCreate)
        {
            // Set drop down list objects
            ViewBag.DistrictList = ListMethods.DistrictList(null);
            ViewBag.LogCodeList = ListMethods.LogCodeList(null);
            ViewBag.LogCodeList = ListMethods.SitesList(0);

            // Get "Eligible to Vote" log code
            ViewBag.VoterStatus = LogCodeMethods.LogDescription(1);

            // Vadiate birthdate since standard validation dosnt work   
            if (voterToCreate.ValidateBirthdate())
            {
                // Get new combo number
                voterToCreate.ComboNo = DistrictMethods.GetDistrict(voterToCreate.District).ComboNo;

                // Set physical address
                voterToCreate.PhysicalAddress1 = voterToCreate.Address1 + " " + voterToCreate.Address2;
                string physicalCSZ = voterToCreate.City + ", " + voterToCreate.State + " " + voterToCreate.Zip;
                voterToCreate.PhysicalCSZ = physicalCSZ;

                voterToCreate.ActivityDate = DateTime.Now;

                voterToCreate.ElectionID = 1;

                // Add a new voter
                VoterDataMethods.AddVoter(voterToCreate);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("DOBSearch", "Enter a valid birthdate");
                return View();
            }

            //return RedirectToAction("Index");
        }
    }
}