using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVote.DataModels;
using System.Web.Mvc;
using EVote.Context;

namespace EVote.DataMethods
{
    public static class ListMethods
    {
        // Instantiate a private Database object
        //private static EVoteVoterDataDataContext _EVote = new EVoteVoterDataDataContext();

        public static SelectList DistrictList(int? districtId)
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                return new SelectList(dbEVote.Districts.ToList(), "District", "DistrictName", districtId);
            }
        }

        public static List<tblDistrict> DistrictList()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                return dbEVote.Districts.ToList();
            }
        }

        public static SelectList LogCodeList(int? logCode)
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                return new SelectList(dbEVote.LogCodes.ToList(), "LogCode", "LogDescription", logCode);
            }
        }

        //public static SelectList SitesList(string userName)
        //{
        //    return new SelectList(_EVote.tblSystemUsers.Where(o => o.UserName != "AESUser"), "UserName", "UserName", userName);
        //}

        public static List<tblSystemUser> SitesList()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                return dbEVote.SystemUsers.ToList();
            }
        }

        public static SelectList SitesList(int? userId)
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                return new SelectList(dbEVote.SystemUsers.ToList(), "UserName", "UserName", userId);
            }
        }

        public static SelectList SitesList(string userName)
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                return new SelectList(dbEVote.SystemUsers.ToList(), "UserName", "UserName", userName);
            }
        }

        public static SelectList SpoiledReasonList()
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                return new SelectList(dbEVote.SpoiledReasons.ToList(), "SpoiledReasonID", "SpoiledReason");
            }
        }

        public static SelectList AbsenteeLogCodeList(int? logCode)
        {
            // Set list of acceptable log codes for drop down list
            string sNumbers = "1,2,3,7,9,12";
            var numbers = sNumbers.Split(',').Select(Int32.Parse).ToList();

            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Try to load the list from database
                // If the list isnt properly formated this code will default back to the preset list
                try
                {
                    // Ensure that a new copy of the data is returned from the DB
                    //dbEVote.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, dbEVote.tblWebConfigs);

                    sNumbers = dbEVote.WebConfigs.Where(o => o.ConfigSetting == "AbsenteeLogCodes").FirstOrDefault().ConfigValue.ToString();
                    numbers = sNumbers.Split(',').Select(Int32.Parse).ToList();
                }
                catch
                {

                }

                return new SelectList(dbEVote.LogCodes.Where(o => numbers.Contains(o.LogCode)).ToList(), "LogCode", "LogDescription", logCode);
            }
        }

        // Get List of years from starting year till today
        public static IEnumerable<SelectListItem> BirthYearList(int startYear, int defaultAge)
        {
            return Enumerable.Range(startYear, (DateTime.Now.Year - defaultAge) - startYear + 1)
                .Select(i => new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    // Set default year to 20 years ago or default voter age
                    Selected = i.Equals(DateTime.Now.Year - defaultAge)
                }).Reverse();

        }

        // Get list of months by number
        public static IEnumerable<SelectListItem> BirthMonthList()
        {
            return Enumerable.Range(1, 12)
                .Select(i => new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
        }

        // Get a list of month names
        public static IEnumerable<SelectListItem> BirthMonthNames()
        {
            IEnumerable<SelectListItem> MonthNames = new List<SelectListItem>()
                {
                new SelectListItem () { Text="January", Value="1"},
                new SelectListItem() { Text = "February", Value = "2" },
                new SelectListItem () { Text="March", Value="3"},
                new SelectListItem () { Text="April", Value="4"},
                new SelectListItem () { Text="May", Value="5"},
                new SelectListItem () { Text="June", Value="6"},
                new SelectListItem () { Text="July", Value="7"},
                new SelectListItem () { Text="August", Value="8"},
                new SelectListItem () { Text="September", Value="9"},
                new SelectListItem () { Text="October", Value="10"},
                new SelectListItem () { Text="November", Value="11"},
                new SelectListItem () { Text="December", Value="12"}
                };

            return MonthNames;
        }

        // Get a list of days in a month (January is always selected first)
        public static IEnumerable<SelectListItem> BirthDayList()
        {
            return Enumerable.Range(1, 31)
                .Select(i => new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
        }

        // Get list of days in month
        // Year parameter added for calculating leap years
        public static IEnumerable<SelectListItem> BirthDayLeap(int intMonth, int intYear)
        {
            // Initialize to 31 days
            int numberOfDays = 31;

            if (intMonth == 2) // February has 28 days
            {
                // Check for leap year // Remove every 100 years // Add every 400 years
                numberOfDays = ((intYear % 4) == 0) && ((intYear % 100) != 0) || ((intYear % 400) == 0) ? 29 : 28;
            }

            // April, June, September and November have 30 days
            else if (new[] { 4, 6, 9, 11 }.Contains(intMonth)) numberOfDays = 30;

            // Generate list of days for a given month
            return Enumerable.Range(1, numberOfDays)
                .Select(i => new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
        }

        public static IEnumerable<SelectListItem> StatesList()
        {
            IEnumerable<SelectListItem> States = new List<SelectListItem>()
                {
                new SelectListItem () { Text="AL", Value="AL"},
                new SelectListItem () { Text="AK", Value="AK"},
                new SelectListItem () { Text="AZ", Value="AZ"},
                new SelectListItem () { Text="AR", Value="AR"},
                new SelectListItem () { Text="CA", Value="CA"},
                new SelectListItem () { Text="CO", Value="CO"},
                new SelectListItem () { Text="CT", Value="CT"},
                new SelectListItem () { Text="DE", Value="DE"},
                new SelectListItem () { Text="DC", Value="DC"},
                new SelectListItem () { Text="FL", Value="FL"},
                new SelectListItem () { Text="GA", Value="GA"},
                new SelectListItem () { Text="HI", Value="HI"},
                new SelectListItem () { Text="ID", Value="ID"},
                new SelectListItem () { Text="IL", Value="IL"},
                new SelectListItem () { Text="IN", Value="IN"},
                new SelectListItem () { Text="IA", Value="IA"},
                new SelectListItem () { Text="KS", Value="KS"},
                new SelectListItem () { Text="KY", Value="KY"},
                new SelectListItem () { Text="LA", Value="LA"},
                new SelectListItem () { Text="MA", Value="MA"},
                new SelectListItem () { Text="ME", Value="ME"},
                new SelectListItem () { Text="MD", Value="MD"},
                new SelectListItem () { Text="MI", Value="MI"},
                new SelectListItem () { Text="MN", Value="MN"},
                new SelectListItem () { Text="MO", Value="MO"},
                new SelectListItem () { Text="MS", Value="MS"},
                new SelectListItem () { Text="MT", Value="MT"},
                new SelectListItem () { Text="NE", Value="NE"},
                new SelectListItem () { Text="NV", Value="NV"},
                new SelectListItem () { Text="NH", Value="NH"},
                new SelectListItem () { Text="NJ", Value="NJ"},
                new SelectListItem () { Text="NM", Value="NM"},
                new SelectListItem () { Text="NY", Value="NY"},
                new SelectListItem () { Text="NC", Value="NC"},
                new SelectListItem () { Text="ND", Value="ND"},
                new SelectListItem () { Text="OH", Value="OH"},
                new SelectListItem () { Text="OK", Value="OK"},
                new SelectListItem () { Text="OR", Value="OR"},
                new SelectListItem () { Text="PA", Value="PA"},
                new SelectListItem () { Text="RI", Value="RI"},
                new SelectListItem () { Text="SC", Value="SC"},
                new SelectListItem () { Text="SD", Value="SD"},
                new SelectListItem () { Text="TN", Value="TN"},
                new SelectListItem () { Text="TX", Value="TX"},
                new SelectListItem () { Text="UT", Value="UT"},
                new SelectListItem () { Text="VT", Value="VT"},
                new SelectListItem () { Text="VA", Value="VA"},
                new SelectListItem () { Text="WA", Value="WA"}
                };

            return States.OrderBy(o => o.Text);
        }
    }
}