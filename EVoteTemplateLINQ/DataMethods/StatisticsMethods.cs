using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVote.DataModels;
using EVote.DataMethods;
using EVote.Context;

namespace EVote.DataMethods
{
    public static class StatisticsMethods
    {
        // Instantiate a private Database object
        //private static EVoteVoterDataDataContext _EVote = new EVoteVoterDataDataContext();

        public static List<tblSiteSummary> SiteSummary(int? type)
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                if (type == null) type = 2;
                return dbEVote.SiteSummarys.Where(o => o.SiteTypeID == type).OrderBy(o => o.SiteName).ToList();
            }
        }

        // Returns a count of everyone who is eligible to vote
        public static int VoterCount()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Append combo number filter to the query that gets all voters
                // Then count the rows
                return VoterDataMethods.VoterListQuery(dbEVote).Where(o => o.ComboNo > 0).Count();
            }
        }

        // Returns a count of everyone who has voted at the polls
        public static int VotedCount()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Append log code filter to the query that gets all voters
                // Then count the rows
                return VoterDataMethods.VoterListQuery(dbEVote).VotedAtPolls().Count();
            }
        }

        public static int EarlyVotingCount()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Append log code filter to the query that gets all voters
                // Then count the rows
                return VoterDataMethods.VoterListQuery(dbEVote).LogCodeEquals(11).Count();
            }
        }

        public static int DistrictCount()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Append log code filter to the query that gets all voters
                // Then count the rows
                return VoterDataMethods.VoterListQuery(dbEVote).LogCodeBetween(6, 14).Count();
            }
        }

        public static float ActivityPercent()
        {
            return (((float)VotedCount() / (float)VoterCount()) * 100);
        }

        public static float EVActivityPercent()
        {
            return (((float)EarlyVotingCount() / (float)VoterCount()) * 100);
        }

        public static float DistrictActivityPercent()
        {
            return (((float)DistrictCount() / (float)VoterCount()) * 100);
        }

        // Generate count list by site user
        public static IEnumerable<ElectionDayCountsModel> ActivityBySite()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Create list object
                return VoterDataMethods.VoterListQuery(dbEVote)
                .LogCodeEquals(12)
                .GroupBy(o => o.UserName
                // Select list of fields
                ).Select(p => new ElectionDayCountsModel
                {
                    UserName = p.Key,           // Key field as defined in Group By
                    EDVoterCount = p.Count()    // Same as Count(*)
                                                // Order By Decending -- a Bar Chart lists its items in reverse order
                }).OrderByDescending(o => o.EDVoterCount).ToList();
            }
        }

        public static IEnumerable<ElectionDayCountsModel> EVActivityBySite()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Create list object
                return VoterDataMethods.VoterListQuery(dbEVote)
                .LogCodeEquals(11)
                .GroupBy(o => o.UserName
                // Select list of fields
                ).Select(p => new ElectionDayCountsModel
                {
                    UserName = p.Key,           // Key field as defined in Group By
                    EDVoterCount = p.Count()    // Same as Count(*)
                                                // Order By Decending -- a Bar Chart lists its items in reverse order
                }).OrderByDescending(o => o.EDVoterCount).ToList();
            }
        }

        // Generate count list by log code
        public static List<ElectionDayCountsModel> ActivityByLogCode()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Create list object
                return VoterDataMethods.VoterListQuery(dbEVote)
                .LogCodeGreaterThan(1)
                .GroupBy(o => o.LogDescription
                // Select list of fields
                ).Select(p => new ElectionDayCountsModel
                {
                    UserName = p.Key,           // Key field as defined in Group By
                    EDVoterCount = p.Count()    // Same as Count(*)
                                                // Order By Decending -- a Bar Chart lists its items in reverse order
                }).OrderByDescending(o => o.EDVoterCount).ToList();
            }
        }

        // Generate count list by log code
        public static List<ElectionDayCountsModel> ActivityByDistrict()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Create list object
                return VoterDataMethods.VoterListQuery(dbEVote)
                .LogCodeBetween(6,14)
                .GroupBy(o => o.DistrictName)
                // Select list of fields
                .Select(p => new ElectionDayCountsModel
                {
                    UserName = p.Key,          // Key field as defined in Group By
                    EDVoterCount = p.Count()  // Same as Count(*)
                                              // Order By Decending -- a Bar Chart lists its items in reverse order
                }).OrderByDescending(o => o.EDVoterCount).ToList();
            }
        }

        public static List<ElectionDayCountsModel> ActivityByDistrictForPie()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                var list = VoterDataMethods.VoterListQuery(dbEVote)
                .LogCodeBetween(6, 14)
                .GroupBy(o => o.DistrictName)
                // Select list of fields
                .Select(p => new ElectionDayCountsModel
                {
                    UserName = p.Key + " - " + p.Count().ToString(),          // Key field as defined in Group By
                    EDVoterCount = p.Count()  // Same as Count(*)
                                              // Order By Decending -- a Bar Chart lists its items in reverse order
                }).OrderByDescending(o => o.EDVoterCount).ToList();

                // Create list object
                return list;
            }
        }

        public static List<ElectionDayCountsModel> ActivityByLogCodeForPie()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                List<int?> codes = new List<int?>() { 7, 10, 11, 12 };

                // Create list object
                return VoterDataMethods.VoterListQuery(dbEVote)
                .Where(l => codes.Contains(l.LogCode))
                .GroupBy(o => o.LogDescription
                // Select list of fields
                ).Select(p => new ElectionDayCountsModel
                {
                    UserName = p.Key + " - " + p.Count().ToString(),           // Key field as defined in Group By
                    EDVoterCount = p.Count()    // Same as Count(*)
                                                // Order By Decending -- a Bar Chart lists its items in reverse order
                }).OrderByDescending(o => o.EDVoterCount).ToList();
            }
        }

        public static List<ElectionDayCountsModel> AllMailActivityByDistrictForPie()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                var list = VoterDataMethods.VoterListQuery(dbEVote)
                .LogCodeBetween(2, 3)
                .GroupBy(o => o.DistrictName)
                // Select list of fields
                .Select(p => new ElectionDayCountsModel
                {
                    UserName = p.Key + " - " + p.Count().ToString(),          // Key field as defined in Group By
                    EDVoterCount = p.Count()  // Same as Count(*)
                                              // Order By Decending -- a Bar Chart lists its items in reverse order
                }).OrderByDescending(o => o.EDVoterCount).ToList();

                // Create list object
                return list;
            }
        }

        public static List<ElectionDayCountsModel> AllMailReturnedByDistrictForPie()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                List<int?> codes = new List<int?>() { 7 }; // Returned ballots only

                // Create list object
                return VoterDataMethods.VoterListQuery(dbEVote)
                .Where(l => codes.Contains(l.LogCode))
                .GroupBy(o => o.DistrictName
                // Select list of fields
                ).Select(p => new ElectionDayCountsModel
                {
                    UserName = p.Key + " - " + p.Count().ToString(),           // Key field as defined in Group By
                    EDVoterCount = p.Count()    // Same as Count(*)
                                                // Order By Decending -- a Bar Chart lists its items in reverse order
                }).OrderByDescending(o => o.EDVoterCount).ToList();
            }
        }
    }
}