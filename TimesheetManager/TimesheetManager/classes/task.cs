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
    public class task
    {
        // *********************************************	
	    // VARIABLE DECLARATIONS
	    public int taskID { get; set; }
	    public int? taskTypeID { get; set; }
	    public int? workflowID { get; set; }
	    public int? workflowStepID { get; set; }
	    public int? workflowPermGroupID { get; set; }
	    public int? workflowDocumentID { get; set; }
	    public int? taskStatusID { get; set; }
	    public int? departmentID { get; set; }
	    public int companyID { get; set; }
	    public string taskTitle { get; set; }
	    public string taskDescription { get; set; }
	    public int requestorUserID { get; set; }
	    public int? assignedUserID { get; set; }
	    public int? assignedByUserID { get; set; }
	    public DateTime? startDate { get; set; }
	    public DateTime dueDate { get; set; }
	    public int? urgent { get; set; }
	    public string resolution { get; set; }
	    public decimal? resolution_hours { get; set; }
	    public string priority { get; set; }
	    public DateTime entryDate { get; set; }
	    public int? projectID { get; set; }
	    public string statusName { get; set; }
	    public int active { get; set; }
	    public int? insufficientInfo { get; set; }
        public int? customerID { get; set; }
        public decimal? billCost { get; set; }
        public decimal? urgentCost { get; set; }
        public string costCode { get; set; }
        public int? revenueWaived { get; set; }
        public decimal? invoicePrice { get; set; }
        public decimal? quoteTime { get; set; }
        public decimal? quotePrice { get; set; }
        public int? completed { get; set; }

        public int PERMGROUPID { get; set; }
        public string CUSTOMERNAME { get; set; }
        public string WORKFLOWNAME { get; set; }
        public string REQUESTORNAME { get; set; }
        public string ASSIGNEDNAME { get; set; }
        public string ASSIGNEDBYNAME { get; set; }
        public bool RETURNSUCCESS { get; set; }

        public DateTime BEGDATE { get; set; }
        public DateTime ENDDATE { get; set; }
        public int EXCLUDECOMMENTS { get; set; }
        public string CRITERIA { get; set; }
        public int MOVE { get; set; }

        // **************************************************



        // **************************************************
        // DISPLAY USER DETAILS
        public void PopulateTask()
        {
            using (BM_DataClassesDataContext Data = new BM_DataClassesDataContext())
            {
                var taskDetails = Data.usp_get_task(taskID, null);
                foreach (usp_get_taskResult taskDetail in taskDetails)
                {
                    taskID = taskDetail.taskID;
                    taskTypeID = taskDetail.taskTypeID;
                    workflowID = taskDetail.workflowID;
                    workflowStepID = taskDetail.workflowStepID;
                    workflowPermGroupID = taskDetail.workflowPermGroupID;
                    workflowDocumentID = taskDetail.workflowDocumentID;
                    departmentID = taskDetail.departmentID;
                    taskStatusID = taskDetail.taskStatusID;
                    taskTitle = taskDetail.taskTitle;
                    taskDescription = taskDetail.taskDescription;
                    requestorUserID = taskDetail.requestorUserID;
                    assignedUserID = taskDetail.assignedUserID;
                    assignedByUserID = taskDetail.assignedByUserID;
                    startDate = taskDetail.startDate;
                    dueDate = (DateTime)taskDetail.dueDate;
                    urgent = taskDetail.urgent;
                    resolution = taskDetail.resolution;
                    resolution_hours = taskDetail.resolution_hours;
                    priority = taskDetail.priority;
                    entryDate = taskDetail.entryDate;
                    projectID = taskDetail.projectID;
                    REQUESTORNAME = taskDetail.requestorName;
                    ASSIGNEDNAME = taskDetail.assignedName;
                    ASSIGNEDBYNAME = taskDetail.assignedbyName;
                    insufficientInfo = taskDetail.insufficientInfo;
                    customerID = taskDetail.customerID;
                    CUSTOMERNAME = taskDetail.customerName;
                    billCost = taskDetail.billCost;
                    urgentCost = taskDetail.urgentCost;
                    costCode = taskDetail.costCode;
                    revenueWaived = taskDetail.revenueWaived;
                    invoicePrice = taskDetail.invoicedPrice;
                    quoteTime = taskDetail.quoteTime;
                    quotePrice = taskDetail.quotePrice;
                    completed = taskDetail.completed;
                }
            }

        }
        // **************************************************


        
    }
}