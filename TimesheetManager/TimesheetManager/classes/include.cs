using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using TimesheetManager.classes;
using System.Security.Cryptography;
using System.Text;

namespace TimesheetManager.classes
{
    public class include
    {
        // **********************************************************************
        // SET BOOKMARK DOMAIN GLOBAL VARIABLE
        public static string bookmarkDomain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
        // **********************************************************************


        // **************************************************
        //SEND HTML EMAIL METHOD
        public void HTMLMailSend(string strfrom, string strFromName, string strto, string strSubject, string strBody, string attachmentName)
        {
            //try
            //{
                string emlFrom = "";
                if (HttpContext.Current.Session["companyUserEmail"] != null)
                {
                    if (HttpContext.Current.Session["companyUserEmail"].ToString() == "1")
                    {
                        emlFrom = strfrom;
                    }
                    else
                    {
                        //emlFrom = "noreply@plygem.com";
                    }
                }
                else
                {
                    //emlFrom = "noreply@plygem.com";
                }

                string[] emailTo = strto.Split(',');
                string emlTo = "";
                string secondaryEmail = "";
                string comma = "";
                for (int i = 0; i < emailTo.GetLength(0); i++)
                {
                    if (i == 0)
                    {
                        emlTo = emailTo[i];
                    }
                    else
                    {
                        if (i > 0) { comma = ","; } else { comma = ""; }
                        secondaryEmail += comma + emailTo[i];
                    }
                }
                MailAddress from = new MailAddress(emlFrom, strFromName);
                MailAddress to = new MailAddress(emlTo);
                MailMessage msg = new MailMessage(from, to);

                string[] emailSecondaryTo = secondaryEmail.Split(',');
                for (int t = 0; t < emailSecondaryTo.GetLength(0); t++)
                {
                    msg.To.Add(emailTo[t]);
                }
                msg.Body = strBody;
                msg.Subject = strSubject;
                msg.IsBodyHtml = true;
                // ADD ATTACHMENT IF IT IS PASSED IN
                if (attachmentName.Length > 0)
                {
                    Attachment att = new Attachment(HttpContext.Current.Server.MapPath(VirtualPathUtility.ToAbsolute("~/" + attachmentName)));
                    msg.Attachments.Add(att);
                }

                //CONNECTION TO MAIL SERVER
                //System.Net.Mail.SmtpClient cli = new System.Net.Mail.SmtpClient("smtp.gmailsrvr.com", 587);
                //cli.Credentials = new NetworkCredential("noreply@gmail.com", "TM@Admin");
                //cli.Send(msg);
                msg.Dispose();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "message", "alert('Email submitted sucessfully');", true);
            //}
            //catch (Exception ex)
            //{
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "message", "alert('" + ex.Message + "');", true);
            //}
        }
        // **************************************************


        // **************************************************
        // GENERATE ORDINAL FOR DATE
        public static string AddOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return "th";
            }

            switch (num % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
        // **************************************************

        // **************************************************
        //USER VERIFICATION METHOD
        public string userVerification()
        {
            string strUserPopup = "";

            if (System.Web.HttpContext.Current.Session["username"] != null)
            {
                strUserPopup = System.Web.HttpContext.Current.Session["username"].ToString();
            }

            return strUserPopup;
        }
        // **************************************************

        // **************************************************
        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99919";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        // **************************************************


        // **************************************************
        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99919";
            cipherText = cipherText.Replace(" ", "+");
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("An error occurred: '{0}'", exception);
                cipherText = "ERROR: INVALID QUERYSTRING";
            }
            return cipherText;

        }
        // **************************************************




    }
}