using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TimesheetManager.classes
{
    public class timesheetCategory
    {
        // **************************************************
        //DECLARE GET AND SETS FOR VARIABLES
        public int timeSheetCategoryID { get; set; }
        public int departmentID { get; set; }
        public string timeSheetCategory { get; set; }
        public int timeSheetCategoryProductive { get; set; }
        public decimal timeSheetCategoryProductiveRate { get; set; }
        public int timeSheetCategoryProductiveExclude { get; set; }
        public int timeSheetCategoryActive { get; set; }
        // **************************************************




        // **************************************************
        //LIST TIMESHEET CATEGORIES BY DEPARTMENT
        public DataTable listTimesheetCategories()
        {
            DataTable table = new DataTable();
            table.Columns.Clear();
            table.Rows.Clear();

            table.Columns.Add("timeSheetCategoryID");
            table.Columns.Add("timeSheetCategory");

            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var Details = Data.usp_list_time_sheet_category(departmentID);
                foreach (usp_list_time_sheet_categoryResult Detail in Details)
                {
                    table.Rows.Add
                    (
                        Detail.timeSheetCategoryID,
                        Detail.timeSheetCategory
                    );
                }
            }

            return table;
        }
        // **************************************************
    }
}