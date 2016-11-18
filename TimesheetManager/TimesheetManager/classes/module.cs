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
    public class module
    {
        // **************************************************
        //DECLARE GET AND SETS FOR VARIABLES
        public int companyID { get; set; }
        public int moduleID { get; set; }
        public int active { get; set; }
        public DateTime entryDate { get; set; }
        public DateTime modifiedDate { get; set; }
        public string MODULENAME { get; set; }
        // **************************************************


        // **************************************************
        //LIST MODULES FOR COMPANY
        public DataTable listCompanyModules()
        {
            DataTable table = new DataTable();
            table.Columns.Clear();
            table.Rows.Clear();

            table.Columns.Add("moduleID");
            table.Columns.Add("active");

            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var moduleDetails = Data.usp_get_company_modules(companyID);
                foreach (usp_get_company_modulesResult moduleDetail in moduleDetails)
                {
                    table.Rows.Add
                    (
                        moduleDetail.moduleID.ToString(), 
                        moduleDetail.active.ToString()
                    );
                }
            }

            return table;  
        }
        // **************************************************     
    }
}