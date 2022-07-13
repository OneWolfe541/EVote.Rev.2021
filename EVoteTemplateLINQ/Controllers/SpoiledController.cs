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
    [VerifyUser]
    public class SpoiledController : Controller
    {
        public ActionResult Index(int? id)
        {
            if(id == null) return RedirectToAction("Index", "Roster");

            ViewBag.SpoiledReasonList = ListMethods.SpoiledReasonList();

            return View(VoterDataMethods.SingleVoter(id));
        }

        // Spoil a Ballot
        public ActionResult SpoilBallot(int BarCode, int SpoiledReasonID)
        {
            bool result = VoterDataMethods.SpoilABallot(BarCode, SpoiledReasonID);

            return RedirectToAction("Index", "Roster");
        }
    }
}