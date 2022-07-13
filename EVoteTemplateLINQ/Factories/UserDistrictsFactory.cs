using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVote.DataModels;
using EVote.Context;
using EVote.DataMethods;
using System.Data.Entity;

namespace EVote.Factories
{
    public class UserDistrictsFactory
    {
        public List<UserDistrict> Create()
        {
            List<UserDistrict> ud = new List<UserDistrict>();

            ud.Add(new UserDistrict(5, 1));
            ud.Add(new UserDistrict(4, 3));
            //ud.Add(new UserDistrict(6, 1));
            //ud.Add(new UserDistrict(6, 3));

            return ud;
        }

        public List<UserDistrict> Create(int user)
        {
            return Create().Where(ud => ud.UserId == user).ToList();
        }

        public List<UserDistrict> Create(int user, int district)
        {
            List<UserDistrict> list = Create(user).Where(ud => ud.DistrictId == 0).ToList();
            if (list != null && list.Count() > 0)
            {
                return list;
            }
            else
            {
                return Create().Where(ud => ud.UserId == user && ud.DistrictId == district).ToList();
            }
        }

        public IEnumerable<tblUserDistricts> List()
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                return bdEVote.UserDistricts.ToList();
            }
        }

        public List<tblUserDistricts> GetUserDistricts(int user, int district)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                List<tblUserDistricts> list = bdEVote.UserDistricts
                    .Where(ud => ud.UserId == user)
                    .Where(ud => ud.DistrictId == 0)
                    .ToList();
                if (list != null && list.Count() > 0)
                {
                    return list;
                }
                else
                {
                    return bdEVote.UserDistricts.Where(ud => ud.UserId == user && ud.DistrictId == district).ToList();
                }
            }
        }

        public bool UpdateUserDistricts(int id, int user, int district)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                tblUserDistricts ud = bdEVote.UserDistricts.Find(id);

                if (ud != null)
                {
                    ud.UserId = user;
                    ud.DistrictId = district;

                    try
                    {
                        bdEVote.SaveChanges();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                
            }

            return false;
        }

        public bool InsertUserDistricts(int user, int district)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                tblUserDistricts ud = new tblUserDistricts();

                ud.UserId = user;
                ud.DistrictId = district;

                try
                {
                    bdEVote.UserDistricts.Add(ud);

                    bdEVote.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public bool DeleteUserDistricts(int id)
        {
            using (EVoteSQLDataContext bdEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {
                tblUserDistricts ud = bdEVote.UserDistricts.Find(id);

                if (ud != null)
                {                    
                    try
                    {
                        bdEVote.Entry(ud).State = EntityState.Deleted;
                        bdEVote.SaveChanges();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}