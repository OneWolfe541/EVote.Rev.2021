using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing.Imaging;
using System.Text;
using SuperSignature;
using EVote.DataMethods;
using EVote.DataModels;
using EVote.Filters;

namespace EVote.Controllers
{
    [VerifyUser]
    public class SignatureController : Controller
    {
        public ActionResult Index(int? barCode)
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
        public ActionResult Index(FormCollection collection)
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
                        return RedirectToAction("Index", tVoter);
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
            if (Session["BallotNumOnSig"].ToString() == "False")
            {
                return RedirectToAction("SignResult", tVoter);
            }
            else
            {
                return RedirectToAction("SignResultBallot", tVoter);
            }

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
            if (Session["BallotNumOnSig"].ToString() == "False")
            {
                //return RedirectToAction("SignResult", tVoter);
                newURL = root + "Signature/SignResult?barCode=" + strBarCode;
            }
            else
            {
                //return RedirectToAction("SignResultBallot", tVoter);
                newURL = root + "Signature/SignResultBallot?barCode=" + strBarCode;
            }

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
            if (BarCode == null) return RedirectToAction("Index", "Voter");

            ViewBag.BarCode = BarCode;
            ViewBag.SignFileURL = "../Signatures/" + BarCode + ".jpg";
            VoterDataModel tVoter = VoterDataMethods.SingleVoter(Int32.Parse(BarCode));
            return View(tVoter);
        }

        public ActionResult SignResultBallot(string BarCode)
        {
            // New more compact method for checking if user is logged in
            //if (Session["UserID"] == null) return RedirectToAction("Login", "Home");
            if (BarCode == null) return RedirectToAction("Index", "Voter");

            ViewBag.BarCode = BarCode;
            ViewBag.SignFileURL = "../Signatures/" + BarCode + ".jpg";
            VoterDataModel tVoter = VoterDataMethods.SingleVoter(Int32.Parse(BarCode));
            return View(tVoter);
        }

        public ActionResult RosterResult(string BarCode)
        {
            // New more compact method for checking if user is logged in
            //if (Session["UserID"] == null) return RedirectToAction("Login", "Home");
            if (BarCode == null) return RedirectToAction("Index", "Voter");

            ViewBag.BarCode = BarCode;
            ViewBag.SignFileURL = "../Signatures/" + BarCode + ".jpg";
            VoterDataModel tVoter = VoterDataMethods.SingleVoter(Int32.Parse(BarCode));
            return View(tVoter);

        }

        public ActionResult SignInFinish(int? BarCode, int? BallotNumber)
        {
            // New more compact method for checking if user is logged in
            //if (Session["UserID"] == null) return RedirectToAction("Login", "Home");
            if (BarCode == null) return RedirectToAction("Index", "Voter");

            bool result = false;
            // Roll ID 9 is early voting
            if (Convert.ToInt32(Session["RoleID"]) == 9)
            {
                result = VoterDataMethods.UpdateEarlyVoter((int)BarCode, Session["UserName"].ToString(), BallotNumber);
            }
            else
            {
                result = VoterDataMethods.UpdateVotedAtPolls((int)BarCode, Session["UserName"].ToString(), BallotNumber);
            }

            return RedirectToAction("Index", "Voter");
        }
    }
}