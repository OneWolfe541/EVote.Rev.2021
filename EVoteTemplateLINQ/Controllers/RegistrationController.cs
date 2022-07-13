using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EVote.DataModels;
using EVote.DataMethods;
using EVote.Filters;
using System.Drawing;
using System.Drawing.Imaging;
using SuperSignature;
using System.Text;

namespace EVote.Controllers
{
    [VerifySuperUser]
    public class RegistrationController : Controller
    {
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
            // Set drop down list objects
            ViewBag.DistrictList = ListMethods.DistrictList(null);
            ViewBag.LogCodeList = ListMethods.AbsenteeLogCodeList(null);
            ViewBag.SitesList = ListMethods.SitesList(0);

            // Pass voter details to view
            var voter = VoterDataMethods.SingleVoter(BarCode);

            // Display the full voter status description
            ViewBag.VoterStatus = LogCodeMethods.LogDescription(voter.LogCode);

            return View(voter);
        }

        [HttpPost]
        public ActionResult Edit(VoterDataModel voterFromForm)
        {
            // Update the changes submitted from the client
            var result = VoterDataMethods.UpdateVoter(voterFromForm);

            if (result == true)
            {
                ViewBag.Results = "Voter saved successfully";

                // Go to signature page
                return RedirectToAction("Signature", VoterDataMethods.SingleVoter(voterFromForm.BarCode));
            }
            else
            {
                ViewBag.Results = "Voter save unsuccessful";

                // Reset the drop down lists
                ViewBag.DistrictList = ListMethods.DistrictList(null);
                ViewBag.LogCodeList = ListMethods.AbsenteeLogCodeList(null);
                ViewBag.SitesList = ListMethods.SitesList(0);

                // Stay here and display the error message
                return View(VoterDataMethods.SingleVoter(voterFromForm.BarCode));
            }            
        }

        public ActionResult Signature(int? barCode)
        {
            // New more compact method for checking if user is logged in
            //if (Session["UserID"] == null) return RedirectToAction("Login", "Home");

            //if (barCode != null)
            //{
            //Session["CheckNetwork"] = _EVote.tblWebConfigs.Where(o => o.ConfigSetting == "SignatureCheckNetwork").FirstOrDefault().ConfigValue;

            VoterDataModel tVoter = VoterDataMethods.SingleVoter(barCode);
            ViewBag.BirthDateString = tVoter.DOB.ToString().Substring(0, tVoter.DOB.ToString().IndexOf(" ") + 1);
            return View(tVoter);
            //}
            //else
            //{
            //    return RedirectToAction("Index","Voter");
            //}
        }

        [HttpPost]
        public ActionResult Signature(FormCollection collection)
        {
            // Get BarCode from hidden field
            string strBarCode = Request["BarCode"];
            string signData = Request["ctlSignature_data"];
            string signDataSmooth = Request["ctlSignature_data_smooth"];

            // Get voter record from BarCode
            VoterDataModel tVoter = VoterDataMethods.SingleVoter(Int32.Parse(strBarCode));

            // Create bitmap object from signature control
            Bitmap bmpSign = GetSignatureBitmap(signData, signDataSmooth);

            FileContentResult result;

            using (var memStream = new System.IO.MemoryStream())
            {
                if (bmpSign != null)
                {
                    // Check if voter actually signed
                    if (bmpSign.Height == 200 && bmpSign.Width == 650)
                    {
                        // Save bitmap object to file
                        bmpSign.Save(HttpContext.Server.MapPath("~/Signatures/" + strBarCode + ".jpg"), ImageFormat.Jpeg);
                    }
                    else
                    {
                        // Get voter birthdate               
                        ViewBag.BirthDateString = tVoter.DOB.ToString().Substring(0, tVoter.DOB.ToString().IndexOf(" ") + 1);
                        // Return to signature page
                        return RedirectToAction("Index", "Registration", tVoter);
                    }
                    result = this.File(memStream.GetBuffer(), "image/jpg");
                }
            }

            // Pass signed image location to next page
            ViewBag.SignFileURL = "../Signatures/" + strBarCode + ".jpg";
            // Pass voters ID to next page
            ViewBag.BarCode = strBarCode;

            ViewBag.Signed = "Signature";
            // Redirect to signature review page
            return RedirectToAction("SignResult", "Registration", tVoter);
        }

        public JsonResult SaveSignature(FormCollection collection)
        {
            string newURL;

            // Get BarCode from hidden field
            string strBarCode = Request["BarCode"];
            string signData = Request["ctlSignature_data"];
            string signDataSmooth = Request["ctlSignature_data_smooth"];

            // Get voter record from BarCode
            VoterDataModel tVoter = VoterDataMethods.SingleVoter(Int32.Parse(strBarCode));

            // Create bitmap object from signature control
            Bitmap bmpSign = GetSignatureBitmap(signData, signDataSmooth);

            FileContentResult result;

            using (var memStream = new System.IO.MemoryStream())
            {
                if (bmpSign != null)
                {
                    // Check if voter actually signed
                    if (bmpSign.Height == 200 && bmpSign.Width == 650)
                    {
                        // Save bitmap object to file
                        bmpSign.Save(HttpContext.Server.MapPath("~/Signatures/" + strBarCode + ".jpg"), ImageFormat.Jpeg);
                    }
                    else
                    {
                        // Get voter birthdate               
                        ViewBag.BirthDateString = tVoter.DOB.ToString().Substring(0, tVoter.DOB.ToString().IndexOf(" ") + 1);
                        // Return to signature page
                        //return RedirectToAction("Index", tVoter);
                        newURL = "/epollbook/Signature/Index?barCode=" + strBarCode;
                    }
                    result = this.File(memStream.GetBuffer(), "image/jpg");
                }
            }

            // Pass signed image location to next page
            ViewBag.SignFileURL = "../Signatures/" + strBarCode + ".jpg";
            // Pass voters ID to next page
            ViewBag.BarCode = strBarCode;

            ViewBag.Signed = "Signature";
            string root = Url.Content("~/");
            // Redirect to signature review page
            newURL = root + "Registration/SignResult?barCode=" + strBarCode;

            //JavaScriptSerializer js = new JavaScriptSerializer();
            return Json(newURL, JsonRequestBehavior.AllowGet); ;
        }

        private Bitmap GetSignatureBitmap(string signData, string signDataSmooth)
        {
            MouseSignature ctlSignature = new MouseSignature();

            byte[] arrayOfBytes = Convert.FromBase64String(signData);
            signData = Encoding.UTF8.GetString(arrayOfBytes);

            byte[] arrayOfBytesSmooth = Convert.FromBase64String(signDataSmooth);
            signDataSmooth = Encoding.UTF8.GetString(arrayOfBytesSmooth);

            ctlSignature.SignDataSmooth = signDataSmooth;

            Bitmap bmpSign = ctlSignature.GenerateSignature(signData, "");

            return bmpSign;
        }

        public ActionResult SignResult(string BarCode)
        {
            // New more compact method for checking if user is logged in
            //if (Session["UserID"] == null) return RedirectToAction("Login", "Home");
            if (BarCode == null) return RedirectToAction("Index", "Registration");

            ViewBag.BarCode = BarCode;
            ViewBag.SignFileURL = "../Signatures/" + BarCode + ".jpg";
            VoterDataModel tVoter = VoterDataMethods.SingleVoter(Int32.Parse(BarCode));
            return View(tVoter);
        }

        public ActionResult SignInFinish(int? BarCode, int? BallotNumber)
        {
            // New more compact method for checking if user is logged in
            //if (Session["UserID"] == null) return RedirectToAction("Login", "Home");
            if (BarCode == null) return RedirectToAction("Index", "Registration");

            bool result = false;

            result = VoterDataMethods.UpdateRegistered((int)BarCode, Session["UserName"].ToString(), BallotNumber);

            return RedirectToAction("Index", "Registration");
        }
    }
}