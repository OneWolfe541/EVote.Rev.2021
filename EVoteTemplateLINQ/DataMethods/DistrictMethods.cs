using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVote.Context;

namespace EVote.DataMethods
{
    public static class DistrictMethods
    {
        public static tblDistrict GetDistrict(int? district)
        {
            using (EVoteSQLDataContext dbEVote = new EVoteSQLDataContext(TrainingModeMethods.CheckTrainingMode()))
            {                
                return dbEVote.Districts.Where(d => d.District == district).FirstOrDefault();
            }
        }
    }
}