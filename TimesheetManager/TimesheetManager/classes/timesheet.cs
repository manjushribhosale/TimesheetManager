using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TimesheetManager.classes
{
    public class timesheet
    {
        // **************************************************
        //DECLARE GET AND SETS FOR VARIABLES
        public int timeSheetID { get; set; }
        public int userID { get; set; }
        public int timeSheetCategoryID { get; set; }
        public int? customerID { get; set; }
        public DateTime timeSheetStartDate { get; set; }
        public DateTime timeSheetEndDate { get; set; }
        public int timeSheetHours { get; set; }
        public int timeSheetMinutes { get; set; }
        public string timeSheetDescription { get; set; }
        public int? projectID { get; set; }

        public string CUSTOMERNAME { get; set; }
        public string TIMESHEETCATEGORYNAME { get; set; }
        public int CLOSEDRECORD { get; set; }
        public int? TIMESHEETCLOSEHOUR { get; set; }
        public string REQUIREDCLOSE { get; set; }
        // **************************************************




        // **************************************************
        // DISPLAY TIMESHEET DETAILS
        public void PopulateTimesheet()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var Details = Data.usp_get_time_sheet(timeSheetID);

                foreach (usp_get_time_sheetResult Detail in Details)
                {
                    timeSheetID = Detail.timeSheetID;
                    userID = Detail.userID;
                    timeSheetCategoryID = Detail.timeSheetCategoryID;
                    timeSheetDescription = Detail.timeSheetDescription;
                }
            }

        }
        // **************************************************




        // **************************************************
        // DISPLAY TIMESHEET DETAILS
        public void PopulateTimesheetOpenCount()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var Details = Data.usp_get_users_days_to_close_count(userID);

                foreach (usp_get_users_days_to_close_countResult Detail in Details)
                {
                    REQUIREDCLOSE = Detail.RequiredClose;
                }
            }

        }
        // **************************************************




        // **************************************************
        // DISPLAY USER TIMESHEET DEFAULTS
        public void PopulateUserTimesheetDefaults()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var Details = Data.usp_get_user_time_sheet_defaults(userID, timeSheetStartDate);

                foreach (usp_get_user_time_sheet_defaultsResult Detail in Details)
                {
                    TIMESHEETCLOSEHOUR = Detail.TimeSheetCloseHour;

                }
            }

        }
        // **************************************************




        // **************************************************
        // INSERT TIMESHEET RECORDS
        public void InsertTimesheet()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                Data.usp_insert_time_sheet(userID, timeSheetCategoryID, customerID, timeSheetStartDate, timeSheetEndDate, timeSheetHours, timeSheetMinutes, timeSheetDescription, projectID);
            }
        }
        // **************************************************




        // **************************************************
        // INSERT TIMESHEET CLOSEOUT RECORDS
        public void InsertTimesheetCloseOut()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                Data.usp_insert_time_sheet_closeout_date(userID, timeSheetStartDate);
            }
        }
        // **************************************************




        // **************************************************
        // UPDATE TIMESHEET RECORD
        public void UpdateTimesheet()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                Data.usp_update_time_sheet(timeSheetID, userID, timeSheetCategoryID, customerID, timeSheetStartDate, timeSheetEndDate, timeSheetHours, timeSheetMinutes, timeSheetDescription, projectID);
            }
        }
        // **************************************************




        // **************************************************
        // DELETE TIMESHEET RECORD
        public void DeleteTimesheet()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                Data.usp_delete_time_sheet(timeSheetID);
            }
        }
        // **************************************************




        // **************************************************
        //LIST TIMESHEET BY USER
        public DataTable listTimesheetByUser()
        {
            DataTable table = new DataTable();
            table.Columns.Clear();
            table.Rows.Clear();

            table.Columns.Add("timeSheetID");
            table.Columns.Add("userID");
            table.Columns.Add("timeSheetCategory");
            table.Columns.Add("timeSheetCategoryID");
            table.Columns.Add("CUSTOMERNAME");
            table.Columns.Add("customerID");
            table.Columns.Add("timeSheetStartDate");
            table.Columns.Add("timeSheetEndDate");
            table.Columns.Add("timeSheetHours");
            table.Columns.Add("timeSheetMinutes");
            table.Columns.Add("timeSheetDescription");
            table.Columns.Add("projectID");

            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var Details = Data.usp_list_time_sheet(userID, CLOSEDRECORD, timeSheetStartDate, timeSheetEndDate);
                foreach (usp_list_time_sheetResult Detail in Details)
                {
                    table.Rows.Add
                    (
                        Detail.timeSheetID,
                        Detail.userID,
                        Detail.timeSheetCategory,
                        Detail.timeSheetCategoryID,
                        Detail.customerName,
                        Detail.customerID,
                        Detail.timeSheetStartDate,
                        Detail.timeSheetEndDate,
                        Detail.timeSheetHours,
                        Detail.timeSheetMinutes,
                        Detail.timeSheetDescription,
                        Detail.projectID
                    );
                }
            }

            return table;
        }
        // **************************************************




        // **************************************************
        //LIST TIMESHEET TASKS WITH TIME
        public DataTable listTimesheetTasks()
        {
            DataTable table = new DataTable();
            table.Columns.Clear();
            table.Rows.Clear();

            table.Columns.Add("taskID");
            table.Columns.Add("userID");
            table.Columns.Add("timeSheetHours");
            table.Columns.Add("timeSheetMinutes");
            table.Columns.Add("timeSheetRounding");

            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var Details = Data.usp_get_time_sheet_tasks(userID, timeSheetStartDate);
                foreach (usp_get_time_sheet_tasksResult Detail in Details)
                {
                    table.Rows.Add
                    (
                        Detail.taskID,
                        Detail.userID,
                        Detail.total_hh,
                        Detail.total_mm,
                        Detail.timeSheetRounding
                    );
                }
            }

            return table;
        }
        // **************************************************




        // **************************************************
        //LIST TIMESHEET OPEN DATES
        public DataTable listTimesheetCloseDays()
        {
            DataTable table = new DataTable();
            table.Columns.Clear();
            table.Rows.Clear();

            table.Columns.Add("dayToClose");

            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var Details = Data.usp_get_days_to_close(userID);
                foreach (usp_get_days_to_closeResult Detail in Details)
                {
                    table.Rows.Add
                    (
                        Detail.DayToClose
                    );
                }
            }

            return table;
        }
        // **************************************************
    }
}