using TimesheetManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimesheetManager.classes;

namespace TimesheetManager
{
    public partial class timesheets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["msg"] != null)
            {
                msgLabel.Text = Request.QueryString["msg"];
                msgLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#37AA28");
            }
            string closeDay = "";
            taskTotalHoursHidden.Value = "0";
            taskTotalMinutesHidden.Value = "0";
            minHoursHidden.Value = "0";
            int? minHours = 0;
            timesheet myUserTimesheet = new timesheet();


            // ***********************************************************************
            timesheet myTimesheet = new timesheet();
            myTimesheet.userID = Convert.ToInt32(Session["userID"]);
            DataTable myTimesheetCloseDaysList = myTimesheet.listTimesheetCloseDays();

            if (Request.Form["ctl00$ContentPlaceHolder1$closeDayHidden"] != null && Request.Form["ctl00$ContentPlaceHolder1$closeDayHidden"] != "" || closeDay != "")
            {
                closeDay = Request.Form["ctl00$ContentPlaceHolder1$closeDayHidden"];
            }

            string strTimesheetCloseDays = "";
            int daysToCloseCounter = 0;
            DateTime dayToClose = Convert.ToDateTime("01/01/1900");
            foreach (DataRow row in myTimesheetCloseDaysList.Rows)
            {
                daysToCloseCounter++;
            }
            foreach (DataRow row in myTimesheetCloseDaysList.Rows)
            {
                dayToClose = Convert.ToDateTime(row["dayToClose"]);

                strTimesheetCloseDays += "<option value='" + dayToClose.ToString("MM/dd/yyyy") + "'";
                if (closeDay != "")
                {
                    if (dayToClose.ToString("MM/dd/yyyy") == closeDay)
                    {
                        strTimesheetCloseDays += " SELECTED ";
                    }
                }
                else if (daysToCloseCounter == 1)
                {
                    strTimesheetCloseDays += " SELECTED ";
                    closeDay = dayToClose.ToString("MM/dd/yyyy");
                }

                string year = dayToClose.Year.ToString();
                string month = dayToClose.ToString("MMMM");
                string day = dayToClose.Day.ToString();
                string dayOfWeek = dayToClose.DayOfWeek.ToString();

                string ordinal = include.AddOrdinal(Convert.ToInt32(dayToClose.Day));

                strTimesheetCloseDays += ">" + dayOfWeek + ", " + month + " " + day + ordinal + ", " + year;
                strTimesheetCloseDays += "</option>";
            }

            closeDaysLabel.Text = strTimesheetCloseDays;

            // ***********************************************************************




            // ***************************************************************
            if (Request.Form["ctl00$ContentPlaceHolder1$closeDayHidden"] != null && Request.Form["ctl00$ContentPlaceHolder1$closeDayHidden"] != "" || closeDay != "")
            {
                if (Request.Form["ctl00$ContentPlaceHolder1$closeDayHidden"] != null && Request.Form["ctl00$ContentPlaceHolder1$closeDayHidden"] != "")
                {
                    closeDayHidden.Value = Request.Form["ctl00$ContentPlaceHolder1$closeDayHidden"];
                }
                else
                {
                    closeDayHidden.Value = closeDay;
                }

                if (closeDayHidden.Value != null && closeDayHidden.Value != "")
                {
                    closeDay = closeDayHidden.Value;
                    DateTime selectedDate = Convert.ToDateTime(closeDay);
                    string yearSelected = selectedDate.Year.ToString();
                    string monthSelected = selectedDate.ToString("MMMM");
                    string daySelected = selectedDate.Day.ToString();
                    string dayOfWeekSelected = selectedDate.DayOfWeek.ToString();

                    string ordinalSelected = include.AddOrdinal(Convert.ToInt32(selectedDate.Day));

                    string strSelectedDate = dayOfWeekSelected.ToUpper() + ", <b>" + monthSelected.ToUpper() + " " + daySelected + "<sup>" + ordinalSelected.ToUpper() + "</sup>, " + yearSelected + "</b>";

                    selectedDateLabel.Text = strSelectedDate;

                    totalHoursPanel.Visible = true;
                    submitPagePanel.Visible = true;

                    myUserTimesheet.userID = Convert.ToInt32(Session["userID"]);
                    myUserTimesheet.timeSheetStartDate = Convert.ToDateTime(closeDay);
                    myUserTimesheet.PopulateUserTimesheetDefaults();

                    minHoursHidden.Value = myUserTimesheet.TIMESHEETCLOSEHOUR.ToString();
                    minHoursLabel.Text = myUserTimesheet.TIMESHEETCLOSEHOUR.ToString() + ".0";
                    minHours = myUserTimesheet.TIMESHEETCLOSEHOUR;
                }
            }
            // ***************************************************************


            include myInclude = new include();
            // ***********************************************************************
            // RETRIEVE DROPDOWN LIST FOR TIMESHEET CATEGORIES
            int rowCounter = 0;
            int totalHours = 0;
            int totalMinutes = 0;
            string strTimesheetList = "";

            int totalTaskHour = 0;
            int totalTaskMinute = 0;
            int recordCounter = 0;

            if (closeDay != "")
            {
                myTimesheet.timeSheetStartDate = Convert.ToDateTime(closeDay);
                myTimesheet.timeSheetEndDate = Convert.ToDateTime(closeDay);
                myTimesheet.CLOSEDRECORD = 1;

                string timesheetTaskList = "";
                
                string taskIDEncoded = "";
                int rounding = 0;
                DataTable myTimeSheetTaskList = myTimesheet.listTimesheetTasks();
                foreach (DataRow row in myTimeSheetTaskList.Rows)
                {
                    taskIDEncoded = HttpUtility.UrlEncode(myInclude.Encrypt(row["taskID"].ToString()));

                    timesheetTaskList += "Task # " + "<a href='workrequest_edit.aspx?taskID=" + taskIDEncoded + "'>" + row["taskID"] + "</a><br />";

                    if (row["timeSheetHours"] != DBNull.Value)
                    {
                        totalTaskHour += Convert.ToInt32(row["timeSheetHours"]);
                    }
                    if (row["timeSheetMinutes"] != DBNull.Value)
                    {
                        totalTaskMinute += Convert.ToInt32(row["timeSheetMinutes"]);
                    }
                    recordCounter++;

                    rounding = Convert.ToInt32(row["timeSheetRounding"]);
                }

                if (timesheetTaskList != "")
                {
                    
                    strTimesheetList += "<tr style='background-color:#fff;'>";
                    strTimesheetList += "<td style='padding-top:5px;'>";
                    strTimesheetList += "<div class='timesheetTaskNumberList'>Project/Tasks<br />" + timesheetTaskList + "</div>";
                    strTimesheetList += "</td>";
                    strTimesheetList += "<td>";
                    strTimesheetList += "<select class='form_select_list_sm' disabled='disabled'><option>Project/Task</option></select>";
                    strTimesheetList += "</td>";
                    strTimesheetList += "<td>";
                    strTimesheetList += "n/a";
                    strTimesheetList += "</td>";
                    strTimesheetList += "<td>";
                    strTimesheetList += "<div style='text-align:center;'>" + totalTaskHour + "<div>";
                    strTimesheetList += "</td>";
                    strTimesheetList += "<td>";
                    strTimesheetList += "<div style='text-align:center;'>" + totalTaskMinute + "<div>";
                    strTimesheetList += "</td>";
                    strTimesheetList += "<td>";
                    strTimesheetList += "</td>";
                    strTimesheetList += "</tr>";
                }

                DataTable myTimeSheetList = myTimesheet.listTimesheetByUser();
                foreach (DataRow row in myTimeSheetList.Rows)
                {
                    DateTime startTimeSheet = Convert.ToDateTime(row["timeSheetStartDate"]);
                    strTimesheetList += "<tr>";
                    strTimesheetList += "<td>";
                    strTimesheetList += "<input type='hidden' id='tmID_" + rowCounter + "' name='tmID_" + rowCounter + "' value='" + row["timeSheetID"] + "'/>";
                    strTimesheetList += "<input type='hidden' id='tmDeleteHidden_" + rowCounter + "' name='tmDeleteHidden_" + rowCounter + "' value='0'/>";
                    strTimesheetList += "<textarea class='form_textarea' id='desc_" + rowCounter + "' name='desc_" + rowCounter + "' cols='50' rows='2'>" + row["timeSheetDescription"] + "</textarea>";
                    strTimesheetList += "</td>";
                    strTimesheetList += "<td>";
                    strTimesheetList += "<select id='category_" + rowCounter + "' name='category_" + rowCounter + "' class='form_select_list_sm'><option value=''>Choose Category</option>" + GetTimesheetCategory(row["timeSheetCategoryID"].ToString()) + "</select>";
                    strTimesheetList += "</td>";
                    strTimesheetList += "<td text-align='center'>";
                    if (displayClientDropdownsHidden.Value != "No")
                    {
                        strTimesheetList += "<div><select id='client_" + rowCounter + "' name='client_" + rowCounter + "' class='form_select_list_sm'><option value=''>Choose Client</option>" + GetTimesheetClient(row["customerID"].ToString()) + "</select></div><br />";
                    }

                    strTimesheetList += "<div><select id='project_" + rowCounter + "' name='project_" + rowCounter + "' class='form_select_list_sm'><option value=''>Choose Project</option>" + GetTimesheetProject(row["projectID"].ToString()) + "</select></div>";
                    strTimesheetList += "</td>";
                    strTimesheetList += "<td text-align='center'>";
                    strTimesheetList += "<input type='text' size='2' style='text-align:center;' maxlength='2' id='hours_" + rowCounter + "' name='hours_" + rowCounter + "' value='" + row["timeSheetHours"] + "' class='form_text_field_timesheet_sm'></input>";
                    strTimesheetList += "</td>";
                    strTimesheetList += "<td>";
                    strTimesheetList += "<input type='text' size='2' style='text-align:center;' maxlength='2' id='minutes_" + rowCounter + "' name='minutes_" + rowCounter + "' value='" + row["timeSheetMinutes"].ToString().PadLeft(2, '0') + "' class='form_text_field_timesheet_sm'></input>";
                    strTimesheetList += "</td>";
                    strTimesheetList += "<td>";
                    strTimesheetList += "<a href='javascript:void(0);' onclick=\"deleteTimeSheetEntry('" + row["timeSheetID"] + "', '" + rowCounter + "')\"><div id='deleteIcon_" + rowCounter + "' class='icon_delete_sm'></div></a>";
                    strTimesheetList += "</td>";
                    strTimesheetList += "</tr>";

                    rowCounter++;
                    recordCounter++;
                    totalHours += Convert.ToInt32(row["timeSheetHours"]);
                    totalMinutes += Convert.ToInt32(row["timeSheetMinutes"]);
                }
            }

            if (recordCounter == 0)
            {
                strTimesheetList += "<tr><td colspan='6' style='text-align:center; padding-top:30px;'>No Timesheet Records Available!</td></tr>";
            }

            timeListLabel.Text = strTimesheetList;

            //SET HOW MANY ROWS CAN BE EDITED
            rowCounterHidden.Value = rowCounter.ToString();
            // ***********************************************************************

        }
        // ***********************************************************************


        // ***********************************************************************
        public static string GetTimesheetCategory(string inValue)
        {
            timesheetCategory myTimesheetCategories = new timesheetCategory();
            myTimesheetCategories.departmentID = Convert.ToInt32(HttpContext.Current.Session["departmentID"]);
            DataTable myTimesheetCatList = myTimesheetCategories.listTimesheetCategories();

            string strTimesheetCat = "";
            foreach (DataRow row in myTimesheetCatList.Rows)
            {
                strTimesheetCat += "<option value='" + row["timeSheetCategoryID"] + "'";
                if (row["timeSheetCategoryID"].ToString() == inValue)
                {
                    strTimesheetCat += " SELECTED ";
                }
                strTimesheetCat += ">" + row["timeSheetCategory"];
                strTimesheetCat += "</option>";

            }

            return strTimesheetCat;
        }
        // ***********************************************************************


        // ***********************************************************************
        public static string GetTimesheetClient(string inValue)
        {
            customer myCustomer = new customer();
            myCustomer.companyID = Convert.ToInt32(HttpContext.Current.Session["companyID"]);
            DataTable myCustomerList = myCustomer.listActiveCustomers();
            string strCustomerList = "";

            foreach (DataRow row in myCustomerList.Rows)
            {
                strCustomerList += "<option value='" + row["customerID"] + "'";
                if (row["customerID"].ToString() == inValue)
                {
                    strCustomerList += " SELECTED ";
                }
                strCustomerList += ">" + row["customerName"];
                strCustomerList += "</option>";

            }

            return strCustomerList;
        }
        // ***********************************************************************


        // ***********************************************************************
        public static string GetTimesheetProject(string inValue)
        {
            projects myProject = new projects();
            myProject.DEPARTMENTID = HttpContext.Current.Session["departmentID"].ToString();
            DataTable myProjectList = myProject.listCompanyProjects();
            string strProjectList = "";

            foreach (DataRow row in myProjectList.Rows)
            {
                strProjectList += "<option value='" + row["projectID"] + "'";
                if (row["projectID"].ToString() == inValue)
                {
                    strProjectList += " SELECTED ";
                }
                strProjectList += ">" + row["projectName"];
                strProjectList += "</option>";
            }

            return strProjectList;
        }
        // ***********************************************************************


        // ***********************************************************************
        [WebMethod]
        public static string GetCategoryAJAX()
        {
            timesheetCategory myTimesheetCategories = new timesheetCategory();
            myTimesheetCategories.departmentID = Convert.ToInt32(HttpContext.Current.Session["departmentID"]);
            DataTable myTimesheetCatList = myTimesheetCategories.listTimesheetCategories();

            string strTimesheetCat = "";
            foreach (DataRow row in myTimesheetCatList.Rows)
            {
                strTimesheetCat += "<option value='" + row["timeSheetCategoryID"] + "'";
                strTimesheetCat += ">" + row["timeSheetCategory"];
                strTimesheetCat += "</option>";
            }

            return strTimesheetCat;
        }
        // ***********************************************************************


        // ***********************************************************************
        [WebMethod]
        public static string GetClientAJAX()
        {
            customer myCustomer = new customer();
            myCustomer.companyID = Convert.ToInt32(HttpContext.Current.Session["companyID"]);
            DataTable myCustomerList = myCustomer.listActiveCustomers();
            string strCustomerList = "";

            foreach (DataRow row in myCustomerList.Rows)
            {
                strCustomerList += "<option value='" + row["customerID"] + "'";
                strCustomerList += ">" + row["customerName"];
                strCustomerList += "</option>";

            }

            return strCustomerList;
        }
        // ***********************************************************************


        // ***********************************************************************
        [WebMethod]
        public static string GetProjectAJAX()
        {
            projects myProject = new projects();
            myProject.DEPARTMENTID = HttpContext.Current.Session["departmentID"].ToString();
            DataTable myProjectList = myProject.listCompanyProjects();
            string strProjectList = "";

            foreach (DataRow row in myProjectList.Rows)
            {
                strProjectList += "<option value='" + row["projectID"] + "'";
                strProjectList += ">" + row["projectName"];
                strProjectList += "</option>";
            }

            return strProjectList;
        }
        // ***********************************************************************




        // ***********************************************************************
        protected void buttonSubmit_Click(object sender, EventArgs e)
        {
            timesheet myTimesheet = new timesheet();
            int i = 0;
            int rowCounter = Convert.ToInt32(Request.Form["ctl00$ContentPlaceHolder1$rowCounterHidden"]);

            //INSERT NEW TIMESHEET ROWS
            int t = 1;
            int newRowCounter = Convert.ToInt32(Request.Form["ctl00$ContentPlaceHolder1$newRowsHidden"]);
            while (t <= newRowCounter)
            {
                if (Request.Form["newDesc_" + t + ""] != null && Request.Form["newDesc_" + t + ""].Trim() != "")
                {
                    myTimesheet.userID = Convert.ToInt32(Session["userID"]);
                    myTimesheet.timeSheetCategoryID = Convert.ToInt32(Request.Form["newCategory_" + t + ""]);
                    if (Request.Form["newClient_" + t + ""] != null && Request.Form["newClient_" + t + ""] != "")
                    {
                        myTimesheet.customerID = Convert.ToInt32(Request.Form["newClient_" + t + ""]);
                    }
                    else
                    {
                        myTimesheet.customerID = null;
                    }

                    if (Request.Form["newProject_" + t + ""] != null && Request.Form["newProject_" + t + ""] != "")
                    {
                        myTimesheet.projectID = Convert.ToInt32(Request.Form["newProject_" + t + ""]);
                    }
                    else
                    {
                        myTimesheet.projectID = null;
                    }

                    myTimesheet.timeSheetStartDate = Convert.ToDateTime(Request.Form["timesheetDate"]);
                    myTimesheet.timeSheetEndDate = Convert.ToDateTime(Request.Form["timesheetDate"]);
                    if (Request.Form["newHours_" + t + ""] != null && Request.Form["newHours_" + t + ""] != "")
                    {
                        myTimesheet.timeSheetHours = Convert.ToInt32(Request.Form["newHours_" + t + ""]);
                    }
                    else
                    {
                        myTimesheet.timeSheetHours = 0;
                    }

                    if (Request.Form["newMinutes_" + t + ""] != null && Request.Form["newMinutes_" + t + ""] != "")
                    {
                        myTimesheet.timeSheetMinutes = Convert.ToInt32(Request.Form["newMinutes_" + t + ""]);
                    }
                    else
                    {
                        myTimesheet.timeSheetMinutes = 0;
                    }

                    myTimesheet.timeSheetDescription = Request.Form["newDesc_" + t + ""];
                    myTimesheet.InsertTimesheet();
                }

                t++;
            }

            if (Request.Form["ctl00$ContentPlaceHolder1$submitTypeHidden"] != "close")
            {
                Response.Redirect("timesheets.aspx?msg=Timesheet+Day+Successfully+Updated");
            }
            else
            {
                //INSERT CLOSE OUT RECORD WHEN USER CLICKS CLOSE OUT DAY
                myTimesheet.userID = Convert.ToInt32(Session["userID"]);
                myTimesheet.timeSheetStartDate = Convert.ToDateTime(Request.Form["timesheetDate"]);
                myTimesheet.InsertTimesheetCloseOut();

                Response.Redirect("timesheets.aspx?msg=Timesheet+Day+Successfully+Closed");
            }
        }
        // ***********************************************************************
    }
}