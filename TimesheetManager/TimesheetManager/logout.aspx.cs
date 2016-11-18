using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TimesheetManager
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // ********************************************
            // DESTROY REMEMBER ME COOKIE
            if (Request.Cookies["loginCookie"] != null)
            {
                HttpCookie myCookie = new HttpCookie("loginCookie");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
            // ********************************************
            Session.Abandon();

            Response.Redirect("login.aspx");
        }
    }
}