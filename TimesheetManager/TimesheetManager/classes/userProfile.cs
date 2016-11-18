using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;

namespace TimesheetManager.classes
{
    public class userProfile
    {
        // **************************************************
        //DECLARE GET AND SETS FOR VARIABLES
        public int userProfileID { get; set; }
        public int userID { get; set; }
        public int profileID { get; set; }
        public string profileValue { get; set; }
        // **************************************************


        // **************************************************
        // DISPLAY USER PROFILE DETAILS
        public void PopulateUserProfileDetails()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var userDetails = Data.usp_get_user_profile(userProfileID, userID);
                foreach (usp_get_user_profileResult userDetail in userDetails)
                {
                    userProfileID = userDetail.userProfileID;
                    userID = userDetail.userID;
                    profileID = userDetail.profileID;
                    profileValue = userDetail.profileValue.ToString();
                }
            }

        }
        // **************************************************


        // **************************************************
        // DELETE USER PROFILE DETAILS
        public void DeleteUserProfileDetails()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                Data.usp_delete_user_profile(userID, null);
            }

        }
        // **************************************************


        // **************************************************
        // INSERT USER PROFILE DETAILS
        public void UpdateUserProfileDetails()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                Data.usp_insert_user_profile(userID, profileID, profileValue);
            }
        }
        // **************************************************
    }
}