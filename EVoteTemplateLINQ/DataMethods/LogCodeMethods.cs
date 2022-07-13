using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVote.DataModels;
using EVote.Context;

namespace EVote.DataMethods
{
    public static class LogCodeMethods
    {
        // Instantiate a private Database object
        //private static EVoteVoterDataDataContext _EVote = new EVoteVoterDataDataContext();

        // Get the full description for a given log code
        public static string LogDescription(int? logCode)
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                return dbEVote.LogCodes.Where(o => o.LogCode == logCode).FirstOrDefault().LogDescription;
            }
        }
    }
}