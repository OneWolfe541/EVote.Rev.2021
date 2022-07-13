using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EVote.DataMethods
{
    public static class TrainingModeMethods
    {
        public static string CheckTrainingMode()
        {
            if (HttpContext.Current.Session["TrainingMode"] != null && HttpContext.Current.Session["TrainingMode"].ToString() == "True")
            {
                return "TrainingConnectionString";
            }
            else
            {
                return "EVoteSQLDataConnectionString";
            }
        }
    }
}