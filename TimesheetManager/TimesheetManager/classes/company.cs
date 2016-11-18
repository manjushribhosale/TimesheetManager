using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TimesheetManager.classes
{
    public class company
    {
        // **************************************************
        //DECLARE GET AND SETS FOR VARIABLES
        public int companyID { get; set; }
        public string companyName { get; set; }
        public string companyShortName { get; set; }
        public string companyPageTitle { get; set; }
        public string companyEmailAddress { get; set; }
        public string companyPhoneNumber { get; set; }
        public int companyActive { get; set; }
        public string companyURL { get; set; }
        public string companyFolderName { get; set; }
        public string companyStylesheet { get; set; }
        public string companyJavascript { get; set; }
        public int defaultPermGroupID { get; set; }
        public int defaultDepartmentID { get; set; }
        public string passwordExpirationDays { get; set; }
        public string defaultPageCopy { get; set; }
        public string contactPageCopy { get; set; }
        public string lightboxPageCopy { get; set; }
        public string documentsPageCopy { get; set; }
        public string settingsPageCopy { get; set; }
        public string faqPageCopy { get; set; }
        public string taskSearchPageCopy { get; set; }
        public string assetSearchPageCopy { get; set; }
        public DateTime entryDate { get; set; }
        public int? userEmail { get; set; }

        // **************************************************


        // **************************************************
        // POPULATE COMPANY STYLES
        public void PopulateCompanyStyles()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var userDetails = Data.usp_get_company(null, companyURL, 1);
                foreach (usp_get_companyResult userDetail in userDetails)
                {
                    companyID = userDetail.companyID;
                    companyShortName = userDetail.companyShortName.ToString();
                    companyPageTitle = userDetail.companyPageTitle.ToString();
                    companyFolderName = userDetail.companyFolderName.ToString();
                    companyStylesheet = userDetail.companyStylesheet.ToString();
                    companyJavascript = userDetail.companyJavascript.ToString();
                    if (userDetail.defaultPermGroupID != null)
                    {
                        defaultPermGroupID = Convert.ToInt32(userDetail.defaultPermGroupID);
                    }
                    if (userDetail.defaultDepartmentID != null)
                    {
                        defaultDepartmentID = Convert.ToInt32(userDetail.defaultDepartmentID);
                    }

                    defaultPageCopy = userDetail.defaultPageCopy;
                    if (userDetail.contactPageCopy != null)
                    {
                        contactPageCopy = userDetail.contactPageCopy.ToString();
                    }
                    if (userDetail.lightboxPageCopy != null)
                    {
                        lightboxPageCopy = userDetail.lightboxPageCopy.ToString();
                    }
                    if (userDetail.documentsPageCopy != null)
                    {
                        documentsPageCopy = userDetail.documentsPageCopy.ToString();
                    }
                    if (userDetail.settingsPageCopy != null)
                    {
                        settingsPageCopy = userDetail.settingsPageCopy.ToString();
                    }
                    faqPageCopy = userDetail.faqPageCopy;
                    taskSearchPageCopy = userDetail.taskSearchPageCopy;
                    userEmail = userDetail.userEmail;
                }
            }

        }
        // **************************************************


        // **************************************************
        // POPULATE COMPANY DETAILS
        public void PopulateCompanyDetails()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var userDetails = Data.usp_get_company(companyID, null, 1);
                foreach (usp_get_companyResult userDetail in userDetails)
                {
                    companyID = userDetail.companyID;
                    companyName = userDetail.companyName;
                    companyShortName = userDetail.companyShortName;
                    companyPageTitle = userDetail.companyPageTitle;
                    companyEmailAddress = userDetail.companyEmailAddress;
                    companyPhoneNumber = userDetail.companyPhoneNumber;
                    companyActive = userDetail.companyActive;
                    companyURL = userDetail.companyURL;
                    companyFolderName = userDetail.companyFolderName;
                    companyStylesheet = userDetail.companyStylesheet;
                    companyJavascript = userDetail.companyJavascript;
                    entryDate = userDetail.entryDate;
                    if (userDetail.passwordExpirationDays != null)
                    {
                        passwordExpirationDays = userDetail.passwordExpirationDays.ToString();
                    }

                    defaultPageCopy = userDetail.defaultPageCopy;

                    if (userDetail.contactPageCopy != null)
                    {
                        contactPageCopy = userDetail.contactPageCopy;
                    }
                    if (userDetail.lightboxPageCopy != null)
                    {
                        lightboxPageCopy = userDetail.lightboxPageCopy;
                    }
                    if (userDetail.documentsPageCopy != null)
                    {
                        documentsPageCopy = userDetail.documentsPageCopy;
                    }
                    if (userDetail.settingsPageCopy != null)
                    {
                        settingsPageCopy = userDetail.settingsPageCopy;
                    }
                    if (userDetail.assetSearchPageCopy != null)
                    {
                        assetSearchPageCopy = userDetail.assetSearchPageCopy;
                    }
                    
                    faqPageCopy = userDetail.faqPageCopy;
                    taskSearchPageCopy = userDetail.taskSearchPageCopy;
                }
            }
        }
        // **************************************************


        // **************************************************
        //LIST COMPANIES
        public DataTable listCompanies()
        {
            DataTable table = new DataTable();
            table.Columns.Clear();
            table.Rows.Clear();

            table.Columns.Add("companyID");
            table.Columns.Add("companyName");

            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var Details = Data.usp_list_company();
                foreach (usp_list_companyResult Detail in Details)
                {
                    table.Rows.Add
                    (
                        Detail.companyID,
                        Detail.companyName
                    );
                }
            }

            return table;
        }
        // **************************************************

    }
}