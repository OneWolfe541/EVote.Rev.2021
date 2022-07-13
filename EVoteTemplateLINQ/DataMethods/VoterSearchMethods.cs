using EVote.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVote.DataMethods
{
    public static class VoterSearchMethods
    {
    }

    public static class VoterSearchExtensions
    {
        // Check if the model has no values
        public static bool IsEmpty(this VoterSearchModel search)
        {
            bool result = false;

            // Check for nulls or empty strings
            if (
                (search.RollNumber == "")
                && (search.LastName == "")
                && (search.FirstName == "")
                && (search.BirthDate == "")
                ) result = true;

            return result;
        }

        public static bool IsNullOrEmpty(this VoterSearchModel search)
        {
            bool result = false;

            // Check for nulls or empty strings
            if (
                (search.RollNumber == null || search.RollNumber == "")
                && (search.LastName == null || search.LastName == "")
                && (search.FirstName == null || search.FirstName == "")
                && (search.BirthDate == null || search.BirthDate == "")
                ) result = true;

            return result;
        }
    }
}