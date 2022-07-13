using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVote.DataModels;
using EVote.Context;

namespace EVote.DataMethods
{
    public static class UserMethods
    {
        //private static EVoteVoterDataDataContext _EVote = new EVoteVoterDataDataContext();

        public static tblSystemUser ValidateUser(tblSystemUser _user)
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext("EVoteSQLDataConnectionString"))
            {
                return dbEVote.SystemUsers
                .Where(
                    u => u.UserName.ToLower() == _user.UserName.ToLower()
                    &&
                    u.Password.ToLower() == _user.Password.ToLower()
                    ).FirstOrDefault();
            }
        }

        public static tblSystemUser ValidateUser(string name, string id)
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext("EVoteSQLDataConnectionString"))
            {
                return dbEVote.SystemUsers
                .Where(
                    u => u.UserName.ToLower() == name.ToLower()
                    &&
                    u.Password.ToLower() == id.ToLower()
                    ).FirstOrDefault();
            }
        }

        public static tblSystemUser GetUser(string name)
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext("EVoteSQLDataConnectionString"))
            {
                return dbEVote.SystemUsers
                .Where(
                    u => u.UserName.ToLower() == name.ToLower()
                    ).FirstOrDefault();
            }
        }
    }
}