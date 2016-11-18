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
using TimesheetManager.classes;

namespace TimesheetManager
{
    public partial class login1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // ***********************************************************************
            // POPULATE COMPANY STYLES
            if (!IsPostBack)
            {
                if (Session["cookieAccess"] != null && Session["cookieAccess"].ToString() == "0")
                {
                    Session.Abandon();
                }
            }

            // SET BOOKMARK DOMAIN
            string bookmarkDomain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            // RETRIEVE COMPANY PROFILE BASED ON URL
            company myCompanyStyle = new company();

            myCompanyStyle.companyURL = bookmarkDomain.Trim();
            myCompanyStyle.PopulateCompanyStyles();

            Session["companyID"] = myCompanyStyle.companyID;
            Session["companyPageTitle"] = myCompanyStyle.companyPageTitle;
            Session["companyShortName"] = myCompanyStyle.companyShortName;
            Session["companyFolderName"] = myCompanyStyle.companyFolderName;
            Session["companyStylesheet"] = myCompanyStyle.companyStylesheet;
            Session["companyJavascript"] = myCompanyStyle.companyJavascript;
            Session["companyUserEmail"] = myCompanyStyle.userEmail;

            if (Session["companyStylesheet"] != null)
            {
                Session["stylesheetLink"] = "<link rel='stylesheet' type='text/css' href='themes/" + Session["companyFolderName"] + "/css/" + Session["companyStylesheet"] + "' />";
                Session["imageSource"] = "themes/" + Session["companyFolderName"] + "/images";
            }
            else
            {
                // HAVE FAILSAFE STYLESHEET
            }

            if (Session["companyJavascript"] != null)
            {
                Session["javascriptLink"] = "<script type='text/javascript' src='themes/" + Session["companyFolderName"] + "/js/" + Session["companyJavascript"] + "'></script>";
            }

            //ENABLE REGISTRATION AND FORGOT PASSWORD
            module myLoginModule = new module();
            myLoginModule.companyID = (int)myCompanyStyle.companyID;
            DataTable myList = myLoginModule.listCompanyModules();

            string sessionname = "";
            foreach (DataRow row in myList.Rows)
            {
                if (Convert.ToInt32(row["moduleID"]) == 7 || Convert.ToInt32(row["moduleID"]) == 8)
                {
                    sessionname = "displayModule_" + row["moduleID"];
                    Session[sessionname] = row["active"];
                }
            }
            // ***********************************************************************


            // ***********************************************************************
            if (Request.QueryString["msg"] != null)
            {
                msgLabel.Text = Request.QueryString["msg"].ToString();
            }
            // ***********************************************************************


            // ***********************************************************************
            if (Request.Cookies["loginCookie"] != null)
            {
                Session["cookieAccess"] = 1;
                loginSubmit_Click(login_submit, EventArgs.Empty);
            }
            else
            {
                Session["cookieAccess"] = 0;
            }
            // ***********************************************************************


            // ***********************************************************************
            if (Session["displayModule_7"] != null && Session["displayModule_7"].ToString() != "0")
            {
                registerPanel.Visible = true;
            }
            else
            {
                registerPanel.Visible = false;
            }

            if (Session["displayModule_8"] != null && Session["displayModule_8"].ToString() != "0")
            {
                forgotPassPanel.Visible = true;
            }
            else
            {
                forgotPassPanel.Visible = false;
            }
            // ***********************************************************************
        }

        protected void loginSubmit_Click(object sender, EventArgs e)
        {
            // *****************************************************************
            // AUTHENTICATE USER INFORMATION FROM USER CLASS
            user myUser = new user();
            if (Request.Form["ctl00$ContentPlaceHolder1$usernameLogin"] != null)
            {
                myUser.username = Request.Form["ctl00$ContentPlaceHolder1$usernameLogin"].ToLower().Trim();
            }
            if (Request.Form["ctl00$ContentPlaceHolder1$passwordLogin"] != null)
            {
                myUser.password = Request.Form["ctl00$ContentPlaceHolder1$passwordLogin"].Trim();
            }
            myUser.userActive = 1;
            myUser.COMPANYID = Convert.ToInt32(Session["companyID"]);
            if (Request.Cookies["loginCookie"] != null)
            {
                myUser.userID = Convert.ToInt32(Request.Cookies["loginCookie"].Value);
                myUser.PopulateCookieUserDetails();
            }
            else
            {
                myUser.Authenticate();
            }

            // SET SESSION VARIABLES FOR MODULES
            module myModule = new module();
            myModule.companyID = Convert.ToInt32(myUser.COMPANYID);
            DataTable myList = myModule.listCompanyModules();

            string sessionname = "";
            foreach (DataRow row in myList.Rows)
            {
                sessionname = "displayModule_" + row["moduleID"];
                Session[sessionname] = row["active"];
            }

            if ((myUser.RETURNSUCCESS) && myUser.failedLoginAttempt < 3)
            {
                // ********************************************
                //SET SESSION VARIABLES
                Session["userID"] = myUser.userID;
                Session["username"] = myUser.username;
                Session["permGroupID"] = myUser.permGroupID;
                Session["departmentID"] = myUser.departmentID;
                Session["firstName"] = myUser.firstName;
                Session["lastName"] = myUser.lastName;
                Session["emailAddress"] = myUser.emailAddress;
                Session["companyID"] = myUser.COMPANYID;
                Session["userEmail"] = myUser.emailAddress;
                Session["departmentAdmin"] = myUser.departmentAdmin;
                Session["companyAdmin"] = myUser.companyAdmin;
                Session["timeSheetReporting"] = myUser.timeSheetReporting;
                Session["vdmAccess"] = myUser.vdmAccess;
                Session["lpmAccess"] = myUser.locationProfileAdmin;
                // ********************************************


                // ********************************************
                // SET PROFILE OPTIONS AS SESSIONS
                userProfile myUserProfile = new userProfile();
                myUserProfile.userID = (int)Session["userID"];

                // VIEW AS
                myUserProfile.userProfileID = 1;

                myUserProfile.PopulateUserProfileDetails();

                if (myUserProfile.profileValue != null) { Session["viewAs"] = myUserProfile.profileValue; }

                // PER PAGE
                myUserProfile.userProfileID = 2;
                myUserProfile.PopulateUserProfileDetails();

                if (myUserProfile.profileValue != null) { Session["perPage"] = myUserProfile.profileValue; }
                // ********************************************



                // ********************************************
                // SET REMEMBER ME COOKIE
                if (Request.Form["rememberMe"] != null)
                {
                    int loginCookie = Convert.ToInt32(Session["userID"]);

                    HttpCookie myCookie = new HttpCookie("loginCookie");
                    myCookie.Expires = DateTime.Now.AddDays(365d);
                    myCookie.Value = loginCookie.ToString();
                    Response.Cookies.Add(myCookie);
                }
                // ********************************************


                // ********************************************
                // CHECK IF PASSWORD IS ABOUT TO EXPIRE
                // SETTING MODULE 6 (PASASWORD EXPIRATION) COMPANY SETTING TO EITHER ENABLE OR DISABLE
                string displayModule_6 = "1";
                if (Session["displayModule_6"] != null) { displayModule_6 = Session["displayModule_6"].ToString(); }


                // UPDATE USERS FAILED LOGIN ATTEMPTS AFTER THEY SUCCESSFULLY LOG IN TO 0
                user myUserLoginAttempt = new user();
                myUserLoginAttempt.userID = Convert.ToInt32(Session["userID"]);
                myUserLoginAttempt.resetFailedAttempt();

                Response.Redirect("timesheets.aspx");
                // ********************************************
            }
            else
            {
                // SETTING MODULE 5 (LOGIN LOCKOUT) COMPANY SETTING TO EITHER ENABLE OR DISABLE
                string displayModule_5 = "1";
                if (myUser.failedLoginAttempt != null)
                {
                    if (Session["displayModule_5"] != null) { displayModule_5 = Session["displayModule_5"].ToString(); }
                    if (myUser.failedLoginAttempt < 3)
                    {
                        // IF CUSTOMER SETS LOCKOUT TO 0 THEN DO NOT LOCK OUT USER AFTER THREE ATTEMPTS
                        if (displayModule_5 != "0")
                        {
                            myUser.IncrementFailedAttempt();
                        }

                        msgLabel.Text = "<font color='#b00000'><b>Login Failed. Please try again.</b></font>";
                    }
                    else
                    {
                        // IF CUSTOMER SETS LOCKOUT TO 0 THEN DO NOT LOCK OUT USER AFTER THREE ATTEMPTS
                        if (displayModule_5 != "0")
                        {
                            myUser.LockUser();
                            msgLabel.Text = "<font color='#b00000'><b>Your account is Locked! Please contact your administrator to unlock your account.</b></font>";
                        }
                        else
                        {
                            msgLabel.Text = "<font color='#b00000'><b>Login Failed. Please try again.</b></font>";
                        }
                    }
                }
                else
                {

                    msgLabel.Text = "<font color='#b00000'><b>Username or Password does not match our records.</b></font>";
                }
            }
        }
        // *****************************************************************
    }
}