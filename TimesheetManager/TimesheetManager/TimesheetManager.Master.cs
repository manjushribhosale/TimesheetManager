using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimesheetManager.classes;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Web.Services;
using System.Globalization;

namespace TimesheetManager
{
    public partial class TimesheetManager : System.Web.UI.MasterPage
    {
        // GLOBAL VARIABLE TO CHECK THE COMPANIES ACTIVE MODULES AND THAT THE USER IS LOGGED IN
        public string userVerification;
        public static string companyPageTitle = "";
        public static string imageSource = "";
        public static string stylesheetLink = "";
        public static string javascriptLink = "";
        public static string onLoadTag = "";
        public static string taskURL = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // ***********************************************************************
            // POPULATE COMPANY STYLES
            string bookmarkDomain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            include myIncludes = new include();

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
            //AUTHENTICATE USER IS LOGGED IN
            userVerification = myIncludes.userVerification();
            if (userVerification == "")
            {
                string URL = HttpContext.Current.Request.Url.PathAndQuery;
                string URLEncoded = "";

                URLEncoded = Server.UrlEncode(URL);
                Response.Redirect("login.aspx?from=" + URLEncoded);
            }

            if (Session["userName"] != null)
            {
                DateTime currentHeaderDate = DateTime.Now;
                string strYear = currentHeaderDate.Year.ToString();
                string strMonth = DateTime.Now.ToString("MMMM");
                string strDay = currentHeaderDate.Day.ToString();
                string strDayOfWeek = currentHeaderDate.DayOfWeek.ToString();
                string strOrdinal = include.AddOrdinal(Convert.ToInt32(currentHeaderDate.Day));

                currentDateLabel.Text = strDayOfWeek + ", " + strMonth + " " + strDay + strOrdinal + ", " + strYear;

                userNameLabel.Text = Session["firstName"].ToString();
                yearLabel.Text = currentHeaderDate.Year.ToString();
            }
            // ***********************************************************************

            
            // ***********************************************************************
            // MAIN HEADER NAV
            string currentPage = HttpContext.Current.Request.Url.AbsolutePath;
            string homePageActive = "nav_home tooltip";
            string tasksPageActive = "nav_task tooltip";
            string reportsPageActive = "nav_report tooltip";
            string adminPageActive = "nav_admin tooltip";

            if (currentPage == "/default.aspx") { homePageActive = "nav_home tooltip active"; }
            if (currentPage == "/workrequests.aspx") { tasksPageActive = "nav_task tooltip active"; }
            if (currentPage == "/reporting.aspx") { reportsPageActive = "nav_report tooltip active"; }
            if (currentPage == "/admin_user.aspx") { adminPageActive = "nav_admin tooltip active"; }

            string mainHeaderNav = "";

            mainHeaderNav += "<div id='nav_wrapper_left'>";
            mainHeaderNav += "<div id='nav_divider'></div>";
            mainHeaderNav += "<ul id='navigation'>";
            mainHeaderNav += "<li class='" + homePageActive + "'><a href='default.aspx'></a><span>HOME</span></li>";
            
            if (Session["displayModule_2"].ToString() != "0")
            {
                mainHeaderNav += "<li class='" + tasksPageActive + "'><a href='workrequests.aspx'></a><span>";
                if (Session["companyID"] != null && Session["companyID"].ToString() == "10")
                    mainHeaderNav += "WORKSPACE";
                else
                    mainHeaderNav += "WORKSPACE";

                mainHeaderNav += "</span></li>";
            }
            
            mainHeaderNav += "<li class='" + reportsPageActive + "'><a href='reporting.aspx'></a><span>REPORTS</span></li>";
            if (Session["departmentAdmin"].ToString() != "0")
            {
                mainHeaderNav += "<li class='" + adminPageActive + "'><a href='admin_user.aspx'></a><span>ADMINISTRATION</span></li>";
            }

            mainHeaderNav += "</ul>";

            mainHeaderNavLabel.Text = mainHeaderNav;
            // ***********************************************************************


            // ***********************************************************************
            // MAIN FOOTER NAV
            string mainFooterNav = "";

            mainFooterNav += "<div id='footer_divider'></div>";
            mainFooterNav += "<div id='footer_links'>";
            mainFooterNav += "<a href='default.aspx'>HOME</a> | ";
            
            if (Session["displayModule_2"].ToString() != "0")
            {
                mainFooterNav += "<a href='workrequests.aspx'>WORKSPACE</a> | ";
            }
            mainFooterNav += "<a href='reporting.aspx'>REPORTS</a> | ";
            if (Session["departmentAdmin"].ToString() != "0")
            {
                mainFooterNav += "<a href='admin_user.aspx'>ADMINISTRATION</a> | ";
            }
            mainFooterNav += "<a href='myaccount.aspx'>SETTINGS</a> | ";
            mainFooterNav += "<a href='contact.aspx'>SUPPORT</a> | ";
            mainFooterNav += "<a href='logout.aspx'>LOG OUT</a>";
            mainFooterNav += "</div>";

            mainFooterNavLabel.Text = mainFooterNav;
            // ***********************************************************************


        }
    }
}