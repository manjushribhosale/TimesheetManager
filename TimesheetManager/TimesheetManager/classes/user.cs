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
    public class user
    {
        // **************************************************
        //DECLARE GET AND SETS FOR VARIABLES
        public int userID { get; set; }
        public int permGroupID { get; set; }
        public int? departmentID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailAddress { get; set; }
        public int? failedLoginAttempt { get; set; }
        public int locked { get; set; }
        public int? userActive { get; set; }
        public DateTime passwordUpdatedDate { get; set; }
        public DateTime entryDate { get; set; }
        public int? departmentAdmin { get; set; }
        public int? companyAdmin { get; set; }
        public int? timeSheetReporting { get; set; }
        public int? COMPANYID { get; set; }
        public int? timeSheetCloseOutDays { get; set; }
        public int? timeSheetRounding { get; set; }
        public int? timeSheetMondayClose { get; set; }
        public int? timeSheetTuesdayClose { get; set; }
        public int? timeSheetWednesdayClose { get; set; }
        public int? timeSheetThursdayClose { get; set; }
        public int? timeSheetFridayClose { get; set; }
        public int? timeSheetSaturdayClose { get; set; }
        public int? timeSheetSundayClose { get; set; }
        public DateTime? timeSheetStartDate { get; set; }
        public int? vdmAccess { get; set; }
        public int? locationProfileAdmin { get; set; }
        public bool RETURNSUCCESS { get; set; }
        public int FOLDERID { get; set; }
        public int FOLDERCOUNT { get; set; }
        public int ASSETCOUNT { get; set; }
        public int LIGHTBOXCOUNT { get; set; }
        public int ASSETS_ADDED_THIS_MONTH { get; set; }
        public int ASSETS_DOWNLOADED_THIS_MONTH { get; set; }
        public int ASSETS_PREVIEW_THIS_MONTH { get; set; }
        public int ASSETS_EMAIL_THIS_MONTH { get; set; }
        public int ASSIGNEDUSERID { get; set; }
        public int INPROGRESS { get; set; }
        public int OPEN { get; set; }
        public int URGENT { get; set; }
        public int TOTALTASK { get; set; }
        public int DUETODAY { get; set; }
        public int BACKLOG { get; set; }
        public int COMPLETEDTHISMONTH { get; set; }
        public int TOTALWORKEDTHISMONTH { get; set; }
        public int TOTALREVENUE { get; set; }
        public int TOTALREVENUEWAIVED { get; set; }
        public int DEPARTMENTTOTALTASK { get; set; }
        public int DEPARTMENTTOTALTASK_NO_USER { get; set; }
        public int DUETHISWEEK { get; set; }
        public string SEARCH { get; set; }
        public int? USERTYPEID { get; set; }
        // **************************************************


        // **************************************************
        // AUTHENTICATE USER
        public void Authenticate()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var userDetails = Data.usp_get_user(username, null, null, COMPANYID, userActive);
                foreach (usp_get_userResult userDetail in userDetails)
                {
                    if (password == userDetail.password.ToString())
                    {
                        userID = userDetail.userID;
                        username = userDetail.username;
                        password = userDetail.password;
                        permGroupID = userDetail.permGroupID;
                        COMPANYID = userDetail.companyID;
                        firstName = userDetail.firstName;
                        lastName = userDetail.lastName;
                        emailAddress = userDetail.emailAddress;
                        departmentID = userDetail.departmentID;
                        failedLoginAttempt = userDetail.failedLoginAttempt;
                        departmentAdmin = userDetail.departmentAdmin;
                        companyAdmin = userDetail.companyAdmin;
                        timeSheetReporting = userDetail.timeSheetReporting;
                        vdmAccess = userDetail.vdmAccess;
                        locationProfileAdmin = userDetail.locationProfileAdmin;

                        if (userDetail.passwordUpdatedDate != null)
                        {
                            passwordUpdatedDate = (DateTime)userDetail.passwordUpdatedDate;
                        }
                        else
                        {
                            passwordUpdatedDate = (DateTime)userDetail.entryDate;
                        }


                        RETURNSUCCESS = true;
                    }
                    else
                    {
                        username = userDetail.username.ToString();
                        COMPANYID = userDetail.companyID;
                        failedLoginAttempt = userDetail.failedLoginAttempt;
                        locked = userDetail.locked;
                        RETURNSUCCESS = false;
                    }
                }
            }
        }
        // **************************************************




        // **************************************************
        // INSERT USER FAILED ATTEMPT
        public void IncrementFailedAttempt()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                Data.usp_increment_failed_attempt(username);
            }
        }
        // **************************************************


        // **************************************************
        // RESET USER FAILED ATTEMPT To 0
        public void resetFailedAttempt()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                Data.usp_update_failedLoginAttempt(userID);
            }
        }
        // **************************************************


        // **************************************************
        // INSERT USER LOCKED
        public void LockUser()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                Data.usp_lock_user(username);
            }
        }
        // **************************************************



        // **************************************************
        // DISPLAY USER DETAILS FOR LOGIN COOKIE
        public void PopulateCookieUserDetails()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var userDetails = Data.usp_get_user(null, null, userID, COMPANYID, userActive);
                foreach (usp_get_userResult userDetail in userDetails)
                {
                    userID = userDetail.userID;
                    firstName = userDetail.firstName.ToString();
                    lastName = userDetail.lastName.ToString();
                    emailAddress = userDetail.emailAddress.ToString();
                    username = userDetail.username.ToString();
                    password = userDetail.password.ToString();
                    permGroupID = userDetail.permGroupID;
                    departmentID = userDetail.departmentID;
                    COMPANYID = userDetail.companyID;
                    userActive = userDetail.userActive;
                    locked = userDetail.locked;
                    failedLoginAttempt = userDetail.failedLoginAttempt;
                    departmentAdmin = userDetail.departmentAdmin;
                    companyAdmin = userDetail.companyAdmin;
                    timeSheetReporting = userDetail.timeSheetReporting;
                    vdmAccess = userDetail.vdmAccess;
                    locationProfileAdmin = userDetail.locationProfileAdmin;
                    if (userDetail.passwordUpdatedDate != null)
                    {
                        passwordUpdatedDate = (DateTime)userDetail.passwordUpdatedDate;
                    }
                    else
                    {
                        passwordUpdatedDate = (DateTime)userDetail.entryDate;
                    }
                    RETURNSUCCESS = true;
                }
            }

        }
        // **************************************************


    }
}