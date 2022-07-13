using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVote.DataModels;
using EVote.Conversions;
using EVote.Extensions;
using EVote.Context;

namespace EVote.DataMethods
{
    public static class VoterDataMethods
    {
        // Instantiate a private Database object
        //private static EVoteVoterDataDataContext _EVote = new EVoteVoterDataDataContext();

        // Define the query to pull all the voters
        //public static IQueryable<VoterDataModel> VoterListQuery()
        //{
        //    // Query voters using the Entity Framework method
        //    // then stuff the results into the custom model object
        //    // [Database Object].[Table Name].Join().Join().Where().Select().ToString();
        //    return _EVote.tblDistricts

        //        .Join(
        //        // Join Voter Data table to Districts table in order to get the District Name
        //        _EVote.tblVoterDatas,
        //        DistrictList => DistrictList.District,  // First Table's key field
        //        voter => voter.District,                // Second Table's key field
        //        (DistrictList, voter) => new { DistrictList, voter } // Create resulting join object, return object
        //        )

        //        .Join(
        //        // Join Log Codes table to previous join in order to get Log Code Description
        //        _EVote.tblLogCodes,
        //        voterData => voterData.voter.LogCode,   // First Table's key field
        //        logcodes => logcodes.LogCode,           // Second Table's key field
        //        (voterData, logcodes) => new { voterData, logcodes } // Create resulting join object, return object
        //        )

        //        .Select(p => new VoterDataModel
        //        {
        //            // Reduce list of fields for display
        //            // Stuff fields into custom Model object (VoterListModel)
        //            BarCode = p.voterData.voter.BarCode,
        //            ElectionID = p.voterData.voter.ElectionID,
        //            ComboNo = p.voterData.voter.ComboNo,
        //            District = p.voterData.voter.District,
        //            DistrictName = p.voterData.DistrictList.DistrictName,
        //            FirstName = p.voterData.voter.FirstName,
        //            LastName = p.voterData.voter.LastName,
        //            MiddleName = p.voterData.voter.MiddleName,
        //            Generation = p.voterData.voter.Generation,
        //            MaidenName = p.voterData.voter.MaidenName,
        //            Address1 = p.voterData.voter.Address1,
        //            Address2 = p.voterData.voter.Address2,
        //            City = p.voterData.voter.City,
        //            State = p.voterData.voter.State,
        //            Zip = p.voterData.voter.Zip,
        //            PhysicalAddress1 = p.voterData.voter.PhysicalAddress,
        //            PhysicalAddress2 = p.voterData.voter.PhysicalAddress2,
        //            PhysicalCity = p.voterData.voter.PhysicalCity,
        //            PhysicalState = p.voterData.voter.PhysicalState,
        //            PhysicalZip = p.voterData.voter.PhysicalZip,
        //            TempUsed = p.voterData.voter.TempUsed,
        //            TempAddress1 = p.voterData.voter.TempAddress1,
        //            TempAddress2 = p.voterData.voter.TempAddress2,
        //            TempCity = p.voterData.voter.TempCity,
        //            TempState = p.voterData.voter.TempState,
        //            TempZip = p.voterData.voter.TempZip,
        //            TempProvince = p.voterData.voter.TempProvince,
        //            TempCountry = p.voterData.voter.TempCountry,
        //            DOB = p.voterData.voter.DOB,
        //            DOBSearch = p.voterData.voter.DOBSearch,
        //            VoterID = p.voterData.voter.VoterID,
        //            SpoiledReasonID = p.voterData.voter.SpoiledReasonID,
        //            LogCode = p.voterData.voter.LogCode,
        //            LogDate = p.voterData.voter.LogDate,
        //            LogDescription = p.logcodes.LogDescription,
        //            CodeGroupState = p.logcodes.CodeGroupState,
        //            ActivityDate = p.voterData.voter.ActivityDate,
        //            UserName = p.voterData.voter.UserName
        //        });
        //}

        public static IQueryable<VoterDataModel> VoterListQuery(EVoteSQLDataContext dbEVote)
        {
            // Query voters using the Entity Framework method
            // then stuff the results into the custom model object
            // [Database Object].[Table Name].Join().Join().Where().Select().ToString();
            return dbEVote.Districts

                .Join(
                // Join Voter Data table to Districts table in order to get the District Name
                dbEVote.VoterDatas,
                DistrictList => DistrictList.District,  // First Table's key field
                voter => voter.District,                // Second Table's key field
                (DistrictList, voter) => new { DistrictList, voter } // Create resulting join object, return object
                )

                .Join(
                // Join Log Codes table to previous join in order to get Log Code Description
                dbEVote.LogCodes,
                voterData => voterData.voter.LogCode,   // First Table's key field
                logcodes => logcodes.LogCode,           // Second Table's key field
                (voterData, logcodes) => new { voterData, logcodes } // Create resulting join object, return object
                )

                .Select(p => new VoterDataModel
                {
                    // Reduce list of fields for display
                    // Stuff fields into custom Model object (VoterListModel)
                    BarCode = p.voterData.voter.BarCode,
                    ElectionID = p.voterData.voter.ElectionID,
                    ComboNo = p.voterData.voter.ComboNo,
                    District = p.voterData.voter.District,
                    DistrictName = p.voterData.DistrictList.DistrictName,
                    FirstName = p.voterData.voter.FirstName,
                    LastName = p.voterData.voter.LastName,
                    MiddleName = p.voterData.voter.MiddleName,
                    Generation = p.voterData.voter.Generation,
                    MaidenName = p.voterData.voter.MaidenName,
                    Address1 = p.voterData.voter.Address1,
                    Address2 = p.voterData.voter.Address2,
                    City = p.voterData.voter.City,
                    State = p.voterData.voter.State,
                    Zip = p.voterData.voter.Zip,
                    PhysicalAddress1 = p.voterData.voter.PhysicalAddress,
                    PhysicalAddress2 = p.voterData.voter.PhysicalAddress2,
                    PhysicalCity = p.voterData.voter.PhysicalCity,
                    PhysicalState = p.voterData.voter.PhysicalState,
                    PhysicalZip = p.voterData.voter.PhysicalZip,
                    PhysicalCSZ = p.voterData.voter.PhysicalCSZ,
                    TempUsed = p.voterData.voter.TempUsed,
                    TempAddress1 = p.voterData.voter.TempAddress1,
                    TempAddress2 = p.voterData.voter.TempAddress2,
                    TempCity = p.voterData.voter.TempCity,
                    TempState = p.voterData.voter.TempState,
                    TempZip = p.voterData.voter.TempZip,
                    TempProvince = p.voterData.voter.TempProvince,
                    TempCountry = p.voterData.voter.TempCountry,
                    DOB = p.voterData.voter.DOB,
                    DOBSearch = p.voterData.voter.DOBSearch,
                    VoterID = p.voterData.voter.VoterID,
                    SpoiledReasonID = p.voterData.voter.SpoiledReasonID,
                    LogCode = p.voterData.voter.LogCode,
                    LogDate = p.voterData.voter.LogDate,
                    LogDescription = p.logcodes.LogDescription,
                    CodeGroupState = p.logcodes.CodeGroupState,
                    ActivityDate = p.voterData.voter.ActivityDate,
                    UserName = p.voterData.voter.UserName,
                    Registered = p.voterData.voter.Registered,
                    RegisteredDate = p.voterData.voter.RegisteredDate
                });
        }

        // Return a single voter
        public static VoterDataModel SingleVoter(int? BarCode)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // When barcode is null return an empty voter object
                if (BarCode.IsNull()) return null;
                // Else get a single voter
                return VoterListQuery(bdEVote).BarCodeEquals(BarCode).FirstOrDefault();
            }
        }

        // Return a list of all the voters who have voted at the poll sites
        public static IEnumerable<VoterDataModel> VotedAtPollsList()
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Append log code filter to the query that gets all voters
                return VoterListQuery(bdEVote).VotedAtPolls().ToList();
            }
        }

        // Return a list of all the voters who have voted at the poll sites
        public static IEnumerable<VoterDataModel> VotedAtPollsPaged(int? intPage, int? intSize)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Append log code filter to the query that gets all voters
                return VoterListQuery(bdEVote).VotedAtPolls().Pages((int)intPage, (int)intSize).ToList();
            }
        }

        // Return a list of all the voters who have voted at a specific site
        public static IEnumerable<VoterDataModel> VotedAtSiteList(string strSite)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Append log code filter to the query that gets all voters
                return VoterListQuery(bdEVote)
                    .VotedAtPolls()
                    .SiteEquals(strSite)
                    .ToList();
            }
        }

        public static IEnumerable<VoterDataModel> RosterBySiteList(string strSite)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Append log code filter to the query that gets all voters
                return VoterListQuery(bdEVote)
                    .SiteEquals(strSite)
                    .ToList();
            }
        }

        public static IEnumerable<VoterDataModel> RosterList()
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Append log code filter to the query that gets all voters
                return VoterListQuery(bdEVote)
                    .ToList();
            }
        }

        // Returns a list of voters based on a full name and birthdate search
        public static IEnumerable<VoterDataModel> FullSearch(string strRoll, string strLastName, string strFirstName, string strDate)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // When no parameters are passed return an empty list
                if (strRoll.IsNullOrEmpty() &&
                strLastName.IsNullOrEmpty() &&
                strFirstName.IsNullOrEmpty() &&
                strDate.IsNullOrEmpty()
                ) return null;
                else
                    return VoterListQuery(bdEVote)
                            .RollNumberEquals(strRoll)
                            .LastNameStartsWith(strLastName)
                            .FirstNameStartsWith(strFirstName)
                            .BirthDateContains(strDate)
                            .ToList()
                            .OrderBy(o => o.LastName)
                            .ThenBy(o => o.FirstName);
            }
        }

        public static IEnumerable<VoterDataModel> ModelSearch(VoterSearchModel searchModel)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // When no parameters are passed return an empty list
                if (searchModel.IsNullOrEmpty()) return null;
                else
                    return VoterListQuery(bdEVote)
                            .RollNumberEquals(searchModel.RollNumber)
                            .LastNameStartsWith(searchModel.LastName)
                            .FirstNameStartsWith(searchModel.FirstName)
                            .BirthDateContains(searchModel.BirthDate)
                            .ToList()
                            .OrderBy(o => o.LastName)
                            .ThenBy(o => o.FirstName);
            }         
        }

        // Returns a list of voters based on a given birthdate
        public static IEnumerable<VoterDataModel> DateSearch(string strDate)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                if (strDate.IsNullOrEmpty()) return null;
                else
                    return VoterListQuery(bdEVote)
                            .BirthDateEquals(strDate)
                            .ToList()
                            .OrderBy(o => o.LastName)
                            .ThenBy(o => o.FirstName);
            }
        }

        // Returns a list of voters based on a full name and birthdate search
        public static IEnumerable<VoterDataModel> RosterSearch(string strRoll, string strLastName, string strFirstName, string strDate)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // When no parameters are passed return an empty model
                if (strRoll.IsNullOrEmpty() &&
                strLastName.IsNullOrEmpty() &&
                strFirstName.IsNullOrEmpty() &&
                strDate.IsNullOrEmpty()
                ) return null;
                else
                    return VoterListQuery(bdEVote)
                            //.VotedAtPolls()
                            .RollNumberEquals(strRoll)
                            .LastNameStartsWith(strLastName)
                            .FirstNameStartsWith(strFirstName)
                            .BirthDateContains(strDate)
                            .ToList()
                            .OrderBy(o => o.LogDate); // Sort by the date and time they voted at this site
            }
        }

        public static IEnumerable<VoterDataModel> RosterModelSearch(VoterSearchModel searchModel)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // When no parameters are passed return an empty model
                if (searchModel.IsNullOrEmpty()) return null;
                else
                    return VoterListQuery(bdEVote)
                            //.VotedAtPolls()
                            .RollNumberEquals(searchModel.RollNumber)
                            .LastNameStartsWith(searchModel.LastName)
                            .FirstNameStartsWith(searchModel.FirstName)
                            .BirthDateContains(searchModel.BirthDate)
                            .ToList()
                            .OrderBy(o => o.LogDate); // Sort by the date and time they voted at this site
            }
        }

        // Returns a list of voters based on a full name and birthdate search
        public static IEnumerable<VoterDataModel> RosterSiteSearch(string strRoll, string strLastName, string strFirstName, string strDate, string strSite)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // When no parameters are passed return an empty model
                if (strRoll.IsNullOrEmpty() &&
                strLastName.IsNullOrEmpty() &&
                strFirstName.IsNullOrEmpty() &&
                strDate.IsNullOrEmpty()
                ) return null;
                else
                    return VoterListQuery(bdEVote)
                            .VotedAtPolls()
                            .SiteEquals(strSite)
                            .RollNumberEquals(strRoll)
                            .LastNameStartsWith(strLastName)
                            .FirstNameStartsWith(strFirstName)
                            .BirthDateContains(strDate)
                            .ToList()
                            .OrderBy(o => o.LastName)
                            .ThenBy(o => o.FirstName);
            }
        }

        public static IEnumerable<VoterDataModel> RosterSiteModelSearch(VoterSearchModel searchModel, string strSite)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // When no parameters are passed return an empty model
                if (searchModel.IsNullOrEmpty()) return null;
                else
                    return VoterListQuery(bdEVote)
                            //.VotedAtPolls()
                            .SiteEquals(strSite)
                            .RollNumberEquals(searchModel.RollNumber)
                            .LastNameStartsWith(searchModel.LastName)
                            .FirstNameStartsWith(searchModel.FirstName)
                            .BirthDateContains(searchModel.BirthDate)
                            .ToList()
                            .OrderBy(o => o.LogDate); // Sort by the date and time they voted at this site
            }
        }

        public static int VoterCount(IEnumerable<VoterDataModel> voterList)
        {
            // Check for empty list
            if (voterList != null) return voterList.Count();
            else return 0;
        }

        // ADD UPDATE INSERT CREATE METHODS
        //-------------------------------------------------------------------//

        // Update full voter record
        public static bool UpdateVoter(VoterDataModel voterToUpdate)
        {
            bool result = false;

            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Create a new voter object
                tblVoterData voter = new tblVoterData();
                // Bind the voter objec to the voter table
                voter = bdEVote.VoterDatas.Where(o => o.BarCode == voterToUpdate.BarCode).SingleOrDefault();

                // Create Log History Record
                tblLogHistory newHistory = new tblLogHistory()
                {
                    BarCode = voter.BarCode,
                    LogCode = voter.LogCode,
                    LogDate = voter.LogDate,
                    LogToday = voter.LogToday,
                    District = voter.District,
                    Address1 = voter.Address1,
                    Address2 = voter.Address2,
                    City = voter.City,
                    State = voter.State,
                    Zip = voter.Zip
                };
                bdEVote.LogHistory.Add(newHistory);

                // Manualy stuff the changes into the voter object

                // List of all the fields in tblVoterData
                //,[ElectionID]
                //voter.ElectionID = voterToUpdate.ElectionID;
                //,[District]
                voter.District = voterToUpdate.District;
                //,[ComboNo]
                voter.ComboNo = voterToUpdate.ComboNo;
                //,[BallotStyle]
                //voter.BallotStyle = voterToUpdate.BallotStyle;
                //,[VoterID]
                //voter.VoterID = voterToUpdate.VoterID;
                //,[VoterNo]
                //voter.VoterNo = voterToUpdate.VoterNo;
                //,[RosterIndex]
                //voter.RosterIndex = voterToUpdate.RosterIndex;
                //,[CourtesyTitle]
                //voter.CourtesyTitle = voterToUpdate.CourtesyTitle;
                //,[LastName]
                voter.LastName = voterToUpdate.LastName;
                //,[FirstName]
                voter.FirstName = voterToUpdate.FirstName;
                //,[MiddleName]
                voter.MiddleName = voterToUpdate.MiddleName;
                //,[Generation]
                voter.Generation = voterToUpdate.Generation;
                //,[MaidenName]
                voter.MaidenName = voterToUpdate.MaidenName;
                //,[Address1]
                voter.Address1 = voterToUpdate.Address1;
                //,[Address2]
                voter.Address2 = voterToUpdate.Address2;
                //,[City]
                voter.City = voterToUpdate.City;
                //,[State]
                voter.State = voterToUpdate.State;
                //,[Zip]
                voter.Zip = voterToUpdate.Zip;
                //,[PhysicalAddress]
                voter.PhysicalAddress = voterToUpdate.PhysicalAddress1;
                //,[PhysicalAddress2]
                voter.PhysicalAddress2 = voterToUpdate.PhysicalAddress2;
                //,[PhysicalCity]
                voter.PhysicalCity = voterToUpdate.PhysicalCity;
                //,[PhysicalState]
                voter.PhysicalState = voterToUpdate.PhysicalState;
                //,[PhysicalZip]
                voter.PhysicalZip = voterToUpdate.PhysicalZip;
                //,[PhysicalCSZ]
                voter.PhysicalCSZ = voterToUpdate.PhysicalCSZ;
                //,[Phone]
                //voter.Phone = voterToUpdate.Phone;
                //,[DOB]
                voter.DOB = voterToUpdate.DOB;
                //,[DOBSearch]
                voter.DOBSearch = voterToUpdate.DOBSearch;
                //,[TempUsed]
                voter.TempUsed = voterToUpdate.TempUsed;
                //,[TempAddress1]
                voter.TempAddress1 = voterToUpdate.TempAddress1;
                //,[TempAddress2]
                voter.TempAddress2 = voterToUpdate.TempAddress2;
                //,[TempCity]
                voter.TempCity = voterToUpdate.TempCity;
                //,[TempState]
                voter.TempState = voterToUpdate.TempState;
                //,[TempZip]
                voter.TempZip = voterToUpdate.TempZip;
                //,[OutofCountry]
                voter.OutofCountry = voterToUpdate.OutofCountry;
                //,[TempProvince]
                voter.TempProvince = voterToUpdate.TempProvince;
                //,[TempCountry]
                voter.TempCountry = voterToUpdate.TempCountry;
                //,[LogCode]
                //voter.LogCode = voterToUpdate.LogCode;
                //,[LogDate]
                //voter.LogDate = voterToUpdate.LogDate;
                //,[LogToday]
                //voter.LogToday = voterToUpdate.LogToday;
                //,[BallotPrinted]
                //voter.BallotPrinted = voterToUpdate.BallotPrinted;
                //,[PrintedDate]
                //voter.PrintedDate = voterToUpdate.PrintedDate;
                //,[BallotNo]
                //voter.BallotNo = voterToUpdate.BallotNo;
                //,[SpoiledReasonID]
                //voter.SpoiledReasonID = voterToUpdate.SpoiledReasonID;
                //,[Site]
                //voter.Site = voterToUpdate.Site;
                //,[Machine]
                //voter.Machine = voterToUpdate.Machine;
                //,[UserName]
                voter.UserName = voterToUpdate.UserName;
                //,[ReqAbs]
                //voter.ReqAbs = voterToUpdate.ReqAbs;
                //,[Reservation]
                //voter.Reservation = voterToUpdate.Reservation;
                //,[Registered]
                //voter.Registered = voterToUpdate.Registered;
                //,[RegisteredDate]
                //voter. = voterToUpdate.;
                //,[DataModified]
                //voter.DataModified = voterToUpdate.DataModified;
                //,[DataModifiedDate]
                //voter. = voterToUpdate.;
                //,[RecUpdated]
                //voter.RecUpdated = voterToUpdate.RecUpdated;
                //,[RecAdded]
                //voter. = voterToUpdate.;
                //,[ActivityDate]
                //voter. = voterToUpdate.;
                //,[Queue]
                //voter. = voterToUpdate.;
                //,[OfflineRecord]
                //voter. = voterToUpdate.;
                //,[timestamp]
                //voter. = voterToUpdate.;

                // Set DOB from DOBSearch
                voter.DOB = Convert.ToDateTime(voterToUpdate.DOBSearch);

                //// Set record has been updated field
                //iVoter.RecUpdated = true;
                voter.RecUpdated = true;

                //// Set last date changed
                //iVoter.ActivityDate = DateTime.Now;
                voter.ActivityDate = DateTime.Now;

                //// Check if log code has been changed
                if (voter.LogCode != voterToUpdate.LogCode)
                {
                    // Update Log Dates
                    //iVoter.LogDate = DateTime.Now;
                    voter.LogDate = DateTime.Now;
                    //iVoter.LogToday = DateTime.Parse(DateTime.Now.ToShortDateString());
                    voter.LogToday = DateTime.Parse(DateTime.Now.ToShortDateString());

                    if(Convert.ToInt32(HttpContext.Current.Session["RollID"]) == 3)
                    {
                        voter.UserName = voterToUpdate.UserName;
                    }
                    else if (HttpContext.Current.Session["Username"] != null)
                    {
                        voter.UserName = HttpContext.Current.Session["Username"].ToString();
                    }
                }
                if (voterToUpdate.LogCode != null)
                {
                    voter.LogCode = voterToUpdate.LogCode;
                }

                // For some reason when I directly copy voterToUpdate into .Entry() it throws a "Primary Key Exists" error
                // But when I create a copy of the vote and manualy set each field the Update goes through
                //_EVote.Entry(voter).State = EntityState.Modified;

                // Turn off validation for this record
                //_EVote.Configuration.ValidateOnSaveEnabled = false;

                // Try to save the changes
                // Return false if update fails
                try
                {
                    // Save the changes
                    bdEVote.SaveChanges();
                    // And return true if the update succeeded
                    result = true;
                }
                catch
                {
                    // When the update throws an error the result flag is already set to False
                    // So nothing else needs to be done
                    // In the future I may want to catch and handle the SQL errors
                }
            }

            return result;
        }

        public static bool AddVoter(VoterDataModel voterToAdd)
        {
            bool result = false;

            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Convert date of birth to short string
                //voterToUpdate.DOBSearch = DateTime.Parse(voterToUpdate.DOB.ToString()).ToShortDateString();
                voterToAdd.DOB = DateTime.Parse(voterToAdd.DOBSearch);

                voterToAdd.LogCode = 1;

                voterToAdd.RecAdded = true;

                //bdEVote.tblVoterDatas.InsertOnSubmit(voterToAdd.ToDataTable());
                bdEVote.VoterDatas.Add(voterToAdd.ToVoterData());
                bdEVote.SaveChanges();
            }

            return result;
        }

        // Update a single voter record and mark them as voted
        public static bool UpdateVotedAtPolls(int BarCode, string UserName, int? BallotNumber)
        {
            bool result = false;

            // Check for Time Zone Adjustments            
            int timeAdjustment = Int32.Parse(ConfigurationMethods.GetConfigValue("TimeAdjust"));

            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {

                // Create a new voter object
                tblVoterData voter = new tblVoterData();
                // Bind the voter object to the voter table
                voter = bdEVote.VoterDatas.Where(o => o.BarCode == BarCode).SingleOrDefault();

                // Create Log History Record
                tblLogHistory newHistory = new tblLogHistory()
                {
                    BarCode = voter.BarCode,
                    LogCode = voter.LogCode,
                    LogDate = voter.LogDate,
                    LogToday = voter.LogToday,
                    District = voter.District,
                    Address1 = voter.Address1,
                    Address2 = voter.Address2,
                    City = voter.City,
                    State = voter.State,
                    Zip = voter.Zip
                };
                bdEVote.LogHistory.Add(newHistory);

                voter.LogCode = 12;
                voter.LogDate = DateTime.Now.AddHours(timeAdjustment); // Add 1 hour for central time
                voter.LogToday = DateTime.Parse(DateTime.Now.AddHours(timeAdjustment).ToShortDateString());
                voter.UserName = UserName; // Set voted at location to current user

                // Update the ballot number if a value is passed
                if (BallotNumber != null)
                {
                    voter.BallotNo = BallotNumber;
                }

                // Try to save the changes
                // Return false if update fails
                try
                {
                    // Save the changes
                    bdEVote.SaveChanges();
                    // And return true if the update succeeded
                    result = true;
                }
                catch
                {
                    // When the update throws an error the result flag is already set to False
                    // So nothing else needs to be done
                    // In the future I may want to catch and handle the SQL errors
                }
            }

            return result;
        }

        public static bool UpdateEarlyVoter(int BarCode, string UserName, int? BallotNumber)
        {
            bool result = false;

            // Check for Time Zone Adjustments            
            int timeAdjustment = Int32.Parse(ConfigurationMethods.GetConfigValue("TimeAdjust"));

            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {

                // Create a new voter object
                tblVoterData voter = new tblVoterData();
                // Bind the voter object to the voter table
                voter = bdEVote.VoterDatas.Where(o => o.BarCode == BarCode).SingleOrDefault();

                // Create Log History Record
                tblLogHistory newHistory = new tblLogHistory()
                {
                    BarCode = voter.BarCode,
                    LogCode = voter.LogCode,
                    LogDate = voter.LogDate,
                    LogToday = voter.LogToday,
                    District = voter.District,
                    Address1 = voter.Address1,
                    Address2 = voter.Address2,
                    City = voter.City,
                    State = voter.State,
                    Zip = voter.Zip
                };
                bdEVote.LogHistory.Add(newHistory);

                voter.LogCode = 11;
                voter.LogDate = DateTime.Now.AddHours(timeAdjustment); // Add 1 hour for central time
                voter.LogToday = DateTime.Parse(DateTime.Now.AddHours(timeAdjustment).ToShortDateString());
                voter.UserName = UserName; // Set voted at location to current user

                // Update the ballot number if a value is passed
                if (BallotNumber != null)
                {
                    voter.BallotNo = BallotNumber;
                }

                // Try to save the changes
                // Return false if update fails
                try
                {
                    // Save the changes
                    bdEVote.SaveChanges();
                    // And return true if the update succeeded
                    result = true;
                }
                catch
                {
                    // When the update throws an error the result flag is already set to False
                    // So nothing else needs to be done
                    // In the future I may want to catch and handle the SQL errors
                }
            }

            return result;
        }

        public static bool UpdateAbsenteeVoter(VoterDataModel voterToUpdate)
        {
            bool result = false;

            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // Create a new voter object
                tblVoterData voter = new tblVoterData();
                // Bind the voter objec to the voter table
                voter = bdEVote.VoterDatas.Where(o => o.BarCode == voterToUpdate.BarCode).SingleOrDefault();

                // Create Log History Record
                tblLogHistory newHistory = new tblLogHistory()
                {
                    BarCode = voter.BarCode,
                    LogCode = voter.LogCode,
                    LogDate = voter.LogDate,
                    LogToday = voter.LogToday,
                    District = voter.District,
                    Address1 = voter.Address1,
                    Address2 = voter.Address2,
                    City = voter.City,
                    State = voter.State,
                    Zip = voter.Zip
                };
                bdEVote.LogHistory.Add(newHistory);

                // Manualy stuff the changes into the voter object

                // List of all the fields in tblVoterData
                //,[ElectionID]
                //voter.ElectionID = voterToUpdate.ElectionID;
                //,[District]
                voter.District = voterToUpdate.District;
                //,[ComboNo]
                //voter.ComboNo = voterToUpdate.ComboNo;
                //,[BallotStyle]
                //voter.BallotStyle = voterToUpdate.BallotStyle;
                //,[VoterID]
                //voter.VoterID = voterToUpdate.VoterID;
                //,[VoterNo]
                //voter.VoterNo = voterToUpdate.VoterNo;
                //,[RosterIndex]
                //voter.RosterIndex = voterToUpdate.RosterIndex;
                //,[CourtesyTitle]
                //voter.CourtesyTitle = voterToUpdate.CourtesyTitle;
                //,[LastName]
                voter.LastName = voterToUpdate.LastName;
                //,[FirstName]
                voter.FirstName = voterToUpdate.FirstName;
                //,[MiddleName]
                voter.MiddleName = voterToUpdate.MiddleName;
                //,[Generation]
                voter.Generation = voterToUpdate.Generation;
                //,[MaidenName]
                voter.MaidenName = voterToUpdate.MaidenName;
                //,[Address1]
                voter.Address1 = voterToUpdate.Address1;
                //,[Address2]
                voter.Address2 = voterToUpdate.Address2;
                //,[City]
                voter.City = voterToUpdate.City;
                //,[State]
                voter.State = voterToUpdate.State;
                //,[Zip]
                voter.Zip = voterToUpdate.Zip;
                //,[PhysicalAddress]
                voter.PhysicalAddress = voterToUpdate.PhysicalAddress1;
                //,[PhysicalAddress2]
                voter.PhysicalAddress2 = voterToUpdate.PhysicalAddress2;
                //,[PhysicalCity]
                voter.PhysicalCity = voterToUpdate.PhysicalCity;
                //,[PhysicalState]
                voter.PhysicalState = voterToUpdate.PhysicalState;
                //,[PhysicalZip]
                voter.PhysicalZip = voterToUpdate.PhysicalZip;
                //,[PhysicalCSZ]
                voter.PhysicalCSZ = voterToUpdate.PhysicalCSZ;
                //,[Phone]
                //voter.Phone = voterToUpdate.Phone;
                //,[DOB]
                voter.DOB = voterToUpdate.DOB;
                //,[DOBSearch]
                voter.DOBSearch = voterToUpdate.DOBSearch;
                //,[TempUsed]
                voter.TempUsed = voterToUpdate.TempUsed;
                //,[TempAddress1]
                voter.TempAddress1 = voterToUpdate.TempAddress1;
                //,[TempAddress2]
                voter.TempAddress2 = voterToUpdate.TempAddress2;
                //,[TempCity]
                voter.TempCity = voterToUpdate.TempCity;
                //,[TempState]
                voter.TempState = voterToUpdate.TempState;
                //,[TempZip]
                voter.TempZip = voterToUpdate.TempZip;
                //,[OutofCountry]
                voter.OutofCountry = voterToUpdate.OutofCountry;
                //,[TempProvince]
                voter.TempProvince = voterToUpdate.TempProvince;
                //,[TempCountry]
                voter.TempCountry = voterToUpdate.TempCountry;
                //,[LogCode]
                //voter.LogCode = voterToUpdate.LogCode;
                //,[LogDate]
                //voter.LogDate = voterToUpdate.LogDate;
                //,[LogToday]
                //voter.LogToday = voterToUpdate.LogToday;
                //,[BallotPrinted]
                //voter.BallotPrinted = voterToUpdate.BallotPrinted;
                //,[PrintedDate]
                //voter.PrintedDate = voterToUpdate.PrintedDate;
                //,[BallotNo]
                //voter.BallotNo = voterToUpdate.BallotNo;
                //,[SpoiledReasonID]
                //voter.SpoiledReasonID = voterToUpdate.SpoiledReasonID;
                //,[Site]
                //voter.Site = voterToUpdate.Site;
                //,[Machine]
                //voter.Machine = voterToUpdate.Machine;
                //,[UserName]
                voter.UserName = voterToUpdate.UserName;
                //,[ReqAbs]
                //voter.ReqAbs = voterToUpdate.ReqAbs;
                //,[Reservation]
                //voter.Reservation = voterToUpdate.Reservation;
                //,[Registered]
                //voter.Registered = voterToUpdate.Registered;
                //,[RegisteredDate]
                //voter. = voterToUpdate.;
                //,[DataModified]
                //voter.DataModified = voterToUpdate.DataModified;
                //,[DataModifiedDate]
                //voter. = voterToUpdate.;
                //,[RecUpdated]
                //voter.RecUpdated = voterToUpdate.RecUpdated;
                //,[RecAdded]
                //voter. = voterToUpdate.;
                //,[ActivityDate]
                //voter. = voterToUpdate.;
                //,[Queue]
                //voter. = voterToUpdate.;
                //,[OfflineRecord]
                //voter. = voterToUpdate.;
                //,[timestamp]
                //voter. = voterToUpdate.;

                // Set DOB from DOBSearch
                voter.DOB = Convert.ToDateTime(voterToUpdate.DOBSearch);

                //// Set record has been updated field
                //iVoter.RecUpdated = true;
                voter.RecUpdated = true;

                //// Set last date changed
                //iVoter.ActivityDate = DateTime.Now;
                voter.ActivityDate = DateTime.Now;

                //// Check if log code has been changed
                if (voter.LogCode != voterToUpdate.LogCode)
                {
                    // Update Log Dates
                    //iVoter.LogDate = DateTime.Now;
                    //voter.LogDate = DateTime.Now;                    
                    //iVoter.LogToday = DateTime.Parse(DateTime.Now.ToShortDateString());
                    //voter.LogToday = DateTime.Parse(DateTime.Now.ToShortDateString());
                    int timeAdjustment = Int32.Parse(ConfigurationMethods.GetConfigValue("TimeAdjust"));
                    voter.LogDate = DateTime.Now.AddHours(timeAdjustment); // Add 1 hour for central time 
                    voter.LogToday = DateTime.Parse(DateTime.Now.AddHours(timeAdjustment).ToShortDateString());
                }
                voter.LogCode = voterToUpdate.LogCode;

                // For some reason when I directly copy voterToUpdate into .Entry() it throws a "Primary Key Exists" error
                // But when I create a copy of the vote and manualy set each field the Update goes through
                //_EVote.Entry(voter).State = EntityState.Modified;

                // Turn off validation for this record
                //_EVote.Configuration.ValidateOnSaveEnabled = false;

                // Try to save the changes
                // Return false if update fails
                try
                {
                    // Save the changes
                    bdEVote.SaveChanges();
                    // And return true if the update succeeded
                    result = true;
                }
                catch
                {
                    // When the update throws an error the result flag is already set to False
                    // So nothing else needs to be done
                    // In the future I may want to catch and handle the SQL errors
                }
            }

            return result;
        }

        public static bool UpdateRegistered(int BarCode, string UserName, int? BallotNumber)
        {
            bool result = false;

            // Check for Time Zone Adjustments            
            int timeAdjustment = Int32.Parse(ConfigurationMethods.GetConfigValue("TimeAdjust")); 

            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {

                // Create a new voter object
                tblVoterData voter = new tblVoterData();
                // Bind the voter object to the voter table
                voter = bdEVote.VoterDatas.Where(o => o.BarCode == BarCode).SingleOrDefault();

                voter.LogCode = 12;

                voter.Registered = true;
                voter.RegisteredDate = DateTime.Now.AddHours(timeAdjustment); // Add 1 hour for central time 
                //voter.LogToday = DateTime.Parse(DateTime.Now.AddHours(timeAdjustment).ToShortDateString());
                voter.UserName = UserName; // Set voted at location to current user

                // Try to save the changes
                // Return false if update fails
                try
                {
                    // Save the changes
                    bdEVote.SaveChanges();
                    // And return true if the update succeeded
                    result = true;
                }
                catch
                {
                    // When the update throws an error the result flag is already set to False
                    // So nothing else needs to be done
                    // In the future I may want to catch and handle the SQL errors
                }
            }

            return result;
        }

        // Spoil a ballot for a single voter
        public static bool SpoilABallot(int barCode, int spoiledReasonID)
        {
            bool result = false;

            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                var voter = bdEVote.VoterDatas.Where(o => o.BarCode == barCode).SingleOrDefault();

                voter.SpoiledReasonID = spoiledReasonID;

                //try
                //{
                    // Save the changes
                    bdEVote.SaveChanges();
                    // And return true if the update succeeded
                    result = true;
                //}
                //catch
                //{

                //}
            }

            return result;
        }

        public static bool ValidateBirthdate(this VoterDataModel voter)
        {
            bool result = false;

            DateTime temp;
            if ( DateTime.TryParse(voter.DOBSearch, out temp) )
            {
                result = true;
            }

            return result;
        }

        public static int GetNextBarcode()
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                // When barcode is null return an empty voter object
                //if (BarCode.IsNull()) return null;
                // Else get a single voter
                //return VoterListQuery(bdEVote).BarCodeEquals(BarCode).FirstOrDefault();
                int maxBarCode = bdEVote.VoterDatas.Max(b => b.BarCode);
                return maxBarCode + 1;
            }
        
        }

        //public static void InsertLogHistory(EVoteSQLDataContext bdEVote, int BarCode)
        //{
        //    bdEVote
        //}
    }

    public static class QueryableExtensions
    {
        // VoterDataModel query extensions are used to add interchangable WHERE clauses and filters
        /*-----------------------------------------------------------------------------------------------*/
        public static IQueryable<T> VotedAtPolls<T>(this IQueryable<T> queryable) where T : VoterDataModel
        {
            return queryable.Where(arg => arg.LogCode == 12);
        }

        public static IQueryable<T> Registered<T>(this IQueryable<T> queryable) where T : VoterDataModel
        {
            return queryable.Where(arg => arg.ComboNo > 0);
        }

        public static IQueryable<T> LogCodeGreaterThan<T>(this IQueryable<T> queryable, int? value) where T : VoterDataModel
        {
            return queryable.Where(arg => arg.LogCode > value);
        }

        public static IQueryable<T> LogCodeBetween<T>(this IQueryable<T> queryable, int? value1, int? value2) where T : VoterDataModel
        {
            return queryable.Where(arg => arg.LogCode >= value1 && arg.LogCode <= value2);
        }

        public static IQueryable<T> LogCodeEquals<T>(this IQueryable<T> queryable, int? value) where T : VoterDataModel
        {
            return queryable.Where(arg => arg.LogCode == value);
        }

        public static IQueryable<T> BarCodeEquals<T>(this IQueryable<T> queryable, int? BarCode) where T : VoterDataModel
        {
            return queryable.Where(arg => arg.BarCode == BarCode);
        }

        public static IQueryable<T> RollNumberEquals<T>(this IQueryable<T> queryable, string strRoll) where T : VoterDataModel
        {
            if (strRoll.IsNullOrEmpty()) return queryable;
            else
            return queryable.Where(arg => arg.VoterID == strRoll);
        }

        public static IQueryable<T> LastNameStartsWith<T>(this IQueryable<T> queryable, string strLastName) where T : VoterDataModel
        {
            if (strLastName.IsNullOrEmpty()) return queryable;
            else
                return queryable.Where(arg => arg.LastName.StartsWith(strLastName));
        }

        public static IQueryable<T> FirstNameStartsWith<T>(this IQueryable<T> queryable, string strFirstName) where T : VoterDataModel
        {
            if (strFirstName.IsNullOrEmpty()) return queryable;
            else
                return queryable.Where(arg => arg.FirstName.StartsWith(strFirstName));
        }

        public static IQueryable<T> BirthDateContains<T>(this IQueryable<T> queryable, string strDate) where T : VoterDataModel
        {
            if (strDate.IsNullOrEmpty()) return queryable;
            else
                return queryable.Where(arg => arg.DOBSearch.Contains(strDate));
        }

        public static IQueryable<T> BirthDateEquals<T>(this IQueryable<T> queryable, string strDate) where T : VoterDataModel
        {
            if (strDate.IsNullOrEmpty()) return queryable;
            else
                return queryable.Where(arg => arg.DOBSearch == strDate);
        }

        public static IQueryable<T> SiteEquals<T>(this IQueryable<T> queryable, string strSite) where T : VoterDataModel
        {
            if (strSite.IsNullOrEmpty()) return queryable;
            else
                return queryable.Where(arg => arg.UserName == strSite);
        }

        public static IQueryable<T> Pages<T>(this IQueryable<T> queryable, int PageNumber, int PageSize) where T : VoterDataModel
        {
            if (PageSize == 0) return queryable;
            else
                return queryable.Skip(PageSize * PageNumber).Take(PageSize);
        }
    }
}