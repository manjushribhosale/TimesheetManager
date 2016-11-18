using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TimesheetManager.classes
{
    public class customer
    {
        // *********************************************	
        // VARIABLE DECLARATIONS
        public int customerID { get; set; }
        public int companyID { get; set; }
        public string customerName { get; set; }
        public string customerCode { get; set; }
        public int active { get; set; }
        public DateTime entryDate { get; set; }
        public int monthRange { get; set; }
        // *********************************************	


        // **************************************************
        // DISPLAY CUSTOMER DETAILS
        public void PopulateCustomer()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var userDetails = Data.usp_get_customer(companyID, customerID, active);
                foreach (usp_get_customerResult userDetail in userDetails)
                {
                    customerID = userDetail.customerID;
                    companyID = userDetail.companyID;
                    customerName = userDetail.customerName;
                    customerCode = userDetail.customerCode;
                    active = Convert.ToInt32(userDetail.active);
                    entryDate = Convert.ToDateTime(userDetail.entryDate);
                }
            }

        }
        // **************************************************


        // **************************************************
        //LIST PROJECT DEPARTMENTS FOR COMPANY
        public DataTable listActiveCustomers()
        {
            DataTable table = new DataTable();
            table.Columns.Clear();
            table.Rows.Clear();

            table.Columns.Add("customerID");
            table.Columns.Add("customerName");

            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var Details = Data.usp_get_customer(companyID, null, 1);
                foreach (usp_get_customerResult Detail in Details)
                {
                    table.Rows.Add
                    (
                        Detail.customerID,
                        Detail.customerName
                    );
                }
            }

            return table;
        }
        // **************************************************


    }
}