using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVote.DataModels;
using System.Web.SessionState;
using System.Runtime.Remoting.Contexts;
using EVote.Context;

namespace EVote.DataMethods
{
    public static class ConfigurationMethods
    {
        // Instantiate a private Database object
        //private static EVoteVoterDataDataContext _EVote = new EVoteVoterDataDataContext();

        public static bool UserHasValues(int userid)
        {
            using (var ctx = new EVoteSQLDataContext("EVoteSQLDataConnectionString"))
            {
                var user = ctx.SystemUsers.Where(x => x.UserId == userid).FirstOrDefault();
                if (user != null)
                {
                    var configs = ctx.WebConfigs.Where(x => x.UserId == user.UserId).ToList();
                    if (configs.Count() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public static string GetConfigValue(string settingName)
        {
            using (EVoteSQLDataContext _EVote = new EVoteSQLDataContext("EVoteSQLDataConnectionString"))
            {
                return _EVote.WebConfigs.Where(o => o.ConfigSetting == settingName).FirstOrDefault().ConfigValue;
            }
        }

        public static bool SetConfigValue(int configId, string value)
        {
            bool result = false;

            using (EVoteSQLDataContext _EVote = new EVoteSQLDataContext("EVoteSQLDataConnectionString"))
            {
                // Get Configuration Record and change the value field to new setting
                _EVote.WebConfigs.Where(o => o.WebConfigID == configId).FirstOrDefault().ConfigValue = value;

                try
                {
                    _EVote.SaveChanges();
                    //_EVote.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, _EVote.tblWebConfigs);
                    result = true;
                }
                catch
                {

                }
            }

            return result;
        }

        public static IEnumerable<tblWebConfig> ActiveConfigsList(int? userId)
        {
            return GetActiveConfigsQuery(userId).OrderBy(o => o.Description).ToList();
        }

        public static IEnumerable<tblWebConfig> BasicConfigsList(int? userId)
        {
            //return GetBasicConfigsQuery(userId).OrderBy(o => o.Description).ToList();
            using (EVoteSQLDataContext _EVote = new EVoteSQLDataContext("EVoteSQLDataConnectionString"))
            {
                return _EVote.WebConfigs
                    .Where(o => o.UserId == userId)
                    .Where(o => o.Active == true)
                    .Where(o => o.Type > 2)
                    .ToList();
            }
        }

        public static IEnumerable<tblWebConfig> SetupConfigsList(int? userId)
        {
            //return GetSetupConfigsQuery(userId).OrderBy(o => o.Description).ToList();
            using (EVoteSQLDataContext _EVote = new EVoteSQLDataContext("EVoteSQLDataConnectionString"))
            {
                return _EVote.WebConfigs
                    .Where(o => o.UserId == userId)
                    .Where(o => o.Active == true)
                    .Where(o => o.Type < 3)
                    .ToList();
            }
        }

        public static IEnumerable<tblWebConfig> ElectionDetailsList()
        {
            //return GetElectionDetailsQuery().ToList();
            using (EVoteSQLDataContext _EVote = new EVoteSQLDataContext("EVoteSQLDataConnectionString"))
            {
                return _EVote.WebConfigs.Where(o => o.UserId == null).ToList();
            }
        }

        public static IEnumerable<WebConfigModel> ElectionDetailsListWithViewModel()
        {
            //return GetElectionDetailsQuery().ToList();
            using (EVoteSQLDataContext _EVote = new EVoteSQLDataContext("EVoteSQLDataConnectionString"))
            {
                return _EVote.WebConfigs
                    .Where(o => o.UserId == null)
                    .Select(wc => new WebConfigModel
                    {
                        WebConfigID = wc.WebConfigID,
                        ConfigSetting = wc.ConfigSetting,
                        ConfigValue = wc.ConfigValue,
                        UserId = wc.UserId,
                        Description = wc.Description,
                        Active = wc.Active,
                        Type = wc.Type,
                        TypeDescription = wc.TypeDescription
                    });
            }
        }

        public static IQueryable<tblWebConfig> GetConfigsQuery(int? userId)
        {
            using (EVoteSQLDataContext _EVote = new EVoteSQLDataContext("EVoteSQLDataConnectionString"))
            {
                return _EVote.WebConfigs.Where(o => o.UserId == userId);
            }
        }

        public static IQueryable<tblWebConfig> GetActiveConfigsQuery(int? userId)
        {
            return GetConfigsQuery(userId).Where(o => o.Active == true);
        }

        public static IQueryable<tblWebConfig> GetBasicConfigsQuery(int? userId)
        {
            return GetActiveConfigsQuery(userId).Where(o => o.Type < 3);
        }

        public static IQueryable<tblWebConfig> GetSetupConfigsQuery(int? userId)
        {
            return GetActiveConfigsQuery(userId).Where(o => o.Type > 2);
        }

        public static IQueryable<tblWebConfig> GetElectionDetailsQuery()
        {
            using (EVoteSQLDataContext _EVote = new EVoteSQLDataContext("EVoteSQLDataConnectionString"))
            {
                return _EVote.WebConfigs.Where(o => o.UserId == null);
            }
        }

        public static string GetElectionValue(string settingName)
        {
            using (EVoteSQLDataContext DB_EVote = new EVoteSQLDataContext("EVoteSQLDataConnectionString"))
            {
                return DB_EVote.WebConfigs.Where(o => o.UserId == null).Where(o => o.ConfigSetting == settingName).FirstOrDefault().ConfigValue;
            }
        }

        public static string GetActiveUserValue(string settingName, int? userId)
        {
            //return GetActiveConfigsQuery(userId)
            //        .Where(o => o.ConfigSetting == settingName)
            //        .FirstOrDefault().ConfigValue;
            string value;
            using (EVoteSQLDataContext _EVote = new EVoteSQLDataContext("EVoteSQLDataConnectionString"))
            {
                try
                {
                    value = _EVote.WebConfigs
                        .Where(o => o.UserId == userId)
                        .Where(o => o.ConfigSetting == settingName)
                        .Where(o => o.Active == true)
                        .FirstOrDefault().ConfigValue;
                }
                catch
                {
                    value = "";
                }
            }
            return value;
        }

        public static string GetDefaultValue(string settingName)
        {
            string value = "False";
            try
            {
                value = GetActiveConfigsQuery(0)
                    .Where(o => o.ConfigSetting == settingName)
                    .FirstOrDefault().ConfigValue;
            }
            catch
            { }
            return value;
        }

        public static ConfigurationModel SessionConfigs()
        {
            return (ConfigurationModel)HttpContext.Current.Session["SiteSettings"];
        }

        //public static bool? IsTrue(this string item)
        //{
        //    bool? result = null;
        //    if (item == "True") result = true;
        //    else if (item == "False") result = false;
        //    return result;
        //}

        public static bool IsTrue(this string item)
        {
            bool result = false;
            if (item == "True") result = true;            
            return result;
        }


        public static ConfigurationModel SiteConfigs(int? userId)
        {
            ConfigurationModel siteSettings = new ConfigurationModel();

            try
            {
                siteSettings.ElectionName = ConfigurationMethods.GetElectionValue("ElectionName");
                siteSettings.ElectionDate = ConfigurationMethods.GetElectionValue("ElectionDate");

                siteSettings.ShowDistrict = ConfigurationMethods.GetActiveUserValue("ShowDistrict", userId);
                //Session["MaidenName"] = ConfigurationMethods.GetActiveUserValue("ShowMaidenName", userId);
                siteSettings.SiteOnlyRoster = ConfigurationMethods.GetActiveUserValue("SiteOnlyRoster", userId);
                //Session["SiteOnlyVotes"] = ConfigurationMethods.GetActiveUserValue("SiteOnlyVotes", userId);
                siteSettings.SpoilBallots = ConfigurationMethods.GetActiveUserValue("SpoilBallots", userId);
                siteSettings.DateSearch = ConfigurationMethods.GetActiveUserValue("UseDateSearch", userId);
                siteSettings.MonthNames = ConfigurationMethods.GetActiveUserValue("UseMonthNames", userId);
                siteSettings.NoDistrictNoVote = ConfigurationMethods.GetActiveUserValue("NoDistrictNoVote", userId);
                siteSettings.BallotNumOnSig = ConfigurationMethods.GetActiveUserValue("BallotNumOnSig", userId);
                //Session["PrintOnSign"] = ConfigurationMethods.GetActiveUserValue("PrintOnSign", userId);
                siteSettings.Absentee = ConfigurationMethods.GetActiveUserValue("Absentee", userId);
                siteSettings.DistrictSignIn = ConfigurationMethods.GetActiveUserValue("DistrictSignIn", userId);
                siteSettings.ePollBook = ConfigurationMethods.GetActiveUserValue("ePollBook", userId);
                siteSettings.ShowEDRoster = ConfigurationMethods.GetActiveUserValue("ShowEDRoster", userId);
                siteSettings.ShowEDActivity = ConfigurationMethods.GetActiveUserValue("ShowEDActivity", userId);
                siteSettings.SiteSummary = ConfigurationMethods.GetActiveUserValue("SiteSummary", userId);
                siteSettings.Registration = ConfigurationMethods.GetActiveUserValue("Registration", userId);
                siteSettings.AllElectionCharts = ConfigurationMethods.GetActiveUserValue("AllElectionCharts", userId);
                siteSettings.CheckNetwork = ConfigurationMethods.GetActiveUserValue("SignatureCheckNetwork", userId);
                siteSettings.PageSize = ConfigurationMethods.GetActiveUserValue("PageSize", userId);

                siteSettings.UserSet = "True";
            }
            catch
            {
                siteSettings.ElectionName = ConfigurationMethods.GetElectionValue("ElectionName");
                siteSettings.ElectionDate = ConfigurationMethods.GetElectionValue("ElectionDate");

                siteSettings.ShowDistrict = ConfigurationMethods.GetDefaultValue("ShowDistrict");
                //Session["MaidenName"] = ConfigurationMethods.GetDefaultValue("ShowMaidenName");
                siteSettings.SiteOnlyRoster = ConfigurationMethods.GetDefaultValue("SiteOnlyRoster");
                //Session["SiteOnlyVotes"] = ConfigurationMethods.GetDefaultValue("SiteOnlyVotes");
                siteSettings.SpoilBallots = ConfigurationMethods.GetDefaultValue("SpoilBallots");
                siteSettings.DateSearch = ConfigurationMethods.GetDefaultValue("UseDateSearch");
                siteSettings.MonthNames = ConfigurationMethods.GetDefaultValue("UseMonthNames");
                siteSettings.NoDistrictNoVote = ConfigurationMethods.GetDefaultValue("NoDistrictNoVote");
                siteSettings.BallotNumOnSig = ConfigurationMethods.GetDefaultValue("BallotNumOnSig");
                //Session["PrintOnSign"] = ConfigurationMethods.GetDefaultValue("PrintOnSign");
                siteSettings.Absentee = ConfigurationMethods.GetDefaultValue("Absentee");
                siteSettings.DistrictSignIn = ConfigurationMethods.GetDefaultValue("DistrictSignIn");
                siteSettings.ePollBook = ConfigurationMethods.GetDefaultValue("ePollBook");
                siteSettings.ShowEDRoster = ConfigurationMethods.GetDefaultValue("ShowEDRoster");
                siteSettings.ShowEDActivity = ConfigurationMethods.GetDefaultValue("ShowEDActivity");
                siteSettings.SiteSummary = ConfigurationMethods.GetDefaultValue("SiteSummary");
                siteSettings.Registration = ConfigurationMethods.GetDefaultValue("Registration");
                siteSettings.AllElectionCharts = ConfigurationMethods.GetDefaultValue("AllElectionCharts");
                siteSettings.CheckNetwork = ConfigurationMethods.GetDefaultValue("SignatureCheckNetwork");
                siteSettings.PageSize = ConfigurationMethods.GetDefaultValue("PageSize");

                siteSettings.UserSet = "False";
            }

            return siteSettings;
        }

        //public static void AddConfig(string settingName, string settingValue, string settingDescription, int? userId, int type, string typeDescription)
        //{
        //    tblWebConfig configToCreate = new tblWebConfig();

        //    configToCreate.ConfigSetting = settingName;
        //    configToCreate.ConfigValue = settingValue;
        //    configToCreate.Description = settingDescription;
        //    configToCreate.UserId = userId;
        //    configToCreate.Active = true;
        //    configToCreate.Type = type;
        //    configToCreate.TypeDescription = typeDescription;

        //    // Add new configs
        //    _EVote.tblWebConfigs.Add(configToCreate);
        //    try
        //    {
        //        _EVote.SaveChanges();
        //    }
        //    catch
        //    {

        //    }
        //}

        //public static void RemoveConfigsForSingleSite(int userId)
        //{
        //    // Never delete Site #0 "AESUser"
        //    if (userId == 0) return;

        //    // Get old list
        //    var settings = GetConfigsQuery(userId).ToList();

        //    // Clear the old list
        //    foreach (tblWebConfig config in settings)
        //    {
        //        // Remove reason record from table
        //        _EVote.tblWebConfigs.Remove(config);
        //        _EVote.SaveChanges();
        //    }
        //}

        //public static void AddNewConfigsForSingleSite(int userId)
        //{
        //    // Never use Site #0 "AESUser"
        //    if (userId == 0) return;

        //    // Create new configuration settings for selected user
        //    AddConfig("UseDateSearch", "False", "Use Date Search", userId, 1, "Behavioral");
        //    AddConfig("SpoilBallots", "False", "Roster by Site", userId, 1, "Behavioral");
        //    AddConfig("UseMonthNames", "False", "Use Month Names", userId, 2, "Visual");
        //    AddConfig("SiteOnlyRoster", "False", "Roster by Site", userId, 1, "Behavioral");
        //    AddConfig("ShowDistrict", "False", "Show Districts", userId, 2, "Visual");
        //    //AddConfig("SignatureCheckNetwork", "False", "Network Check", UserId);
        //    AddConfig("NoDistrictNoVote", "False", "Voting District Filter", userId, 1, "Behavioral");
        //    AddConfig("SiteOnlyVotes", "False", "Vote by Site", userId, 1, "Behavioral");
        //    AddConfig("BallotNumOnSig", "False", "Ballot Number Required", userId, 1, "Behavioral");
        //    //AddConfig("PrintOnSign", "False", "Print Permit after Signature", UserId);
        //    AddConfig("Absentee", "False", "Absentee Mode", userId, 3,  "Full Page");
        //    AddConfig("DistrictSignIn", "False", "Set District at Sign-In", userId, 1, "Behavioral");
        //    AddConfig("AbsenteeLogCodes", "1,2,3,7,9", "Absentee Status List", userId, 4, "Specific");
        //    AddConfig("ePollBook", "False", "e-PollBook Mode", userId, 3, "Full Page");
        //    AddConfig("ShowEDRoster", "False", "Show Election Day Roster Page", userId, 3, "Full Page");
        //    AddConfig("ShowEDActivity", "False", "Show Election Day Activity Charts", userId, 3, "Full Page");
        //    AddConfig("SiteSummary", "False", "Show Site Summary Page", userId, 3, "Full Page");
        //    //AddConfig("TempAddress", "False", "Show Temp Address", UserId);
        //    AddConfig("AllElectionCharts", "False", "All Activity Page", userId, 3, "Full Page");
        //    AddConfig("Registration", "False", "Registration Page", userId, 3, "Full Page");
        //}

        //public static void ResetConfigs(int? userId)
        //{
        //    if (userId == 0 || userId == null) return;
        //    RemoveConfigsForSingleSite((int)userId);
        //    AddNewConfigsForSingleSite((int)userId);
        //}
    }


}