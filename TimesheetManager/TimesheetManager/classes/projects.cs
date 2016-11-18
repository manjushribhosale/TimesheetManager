using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TimesheetManager.classes
{
    public class projects
    {
        // **************************************************
        //DECLARE GET AND SETS FOR VARIABLES
        public int projectID { get; set; }
        public string projectName { get; set; }
        public int projectOwnerUserID { get; set; }
        public DateTime projectStartDate { get; set; }
        public DateTime projectEndDate { get; set; }
        public int active { get; set; }
        public DateTime entryDate { get; set; }
        public string DEPARTMENTID { get; set; }
        

        // **************************************************

        // **************************************************
        //LIST PROJECTS FOR COMPANY
        public DataTable listCompanyProjects()
        {
            DataTable table = new DataTable();
            table.Columns.Clear();
            table.Rows.Clear();

            table.Columns.Add("projectID");
            table.Columns.Add("projectName");

            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var Details = Data.usp_get_projects_department(Convert.ToInt32(DEPARTMENTID), null);
                foreach (usp_get_projects_departmentResult Detail in Details)
                {
                    table.Rows.Add
                    (
                        Detail.projectID,
                        Detail.projectName
                    );
                }
            }

            return table;
        }
        // **************************************************
    }
}