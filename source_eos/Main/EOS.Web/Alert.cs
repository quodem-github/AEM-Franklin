using System.Web;
using System.Text;
using System.Web.UI;
using System;
 using EOS.Web.Extensions;

namespace EOS.Web
{
    public class Alert
    {

        /// <summary>
        /// Shows a client-side JavaScript alert in the browser.
        /// </summary>
        /// <param name="message">The message to appear in the alert.</param>
        public static void Show(string message)
        {
            
            Show(message, null);
        }

        /// <summary>
        /// Shows a client-side JavaScript alert in the browser.
        /// </summary>
        /// <param name="message">The message to appear in the alert.</param>
        public static void Show(string message, string navegacion)
        {
            // Cleans the message to allow single quotation marks
            string cleanMessage = message.Replace("'", "\\'");

            // Clean the message from new lines
            if (cleanMessage.Contains(Environment.NewLine))
            {
                cleanMessage = cleanMessage.Replace(Environment.NewLine, " ");
            }
            string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";
            
            // Gets the executing web page
            Page page = HttpContext.Current.CurrentHandler as Page;
            
            // Checks if the handler is a Page and that the script isn't allready on the Page
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(Alert), "alert", script);
            }

            if (navegacion != null)
            {
                string scriptBack = "<script type=\"text/javascript\">location.href = '" + navegacion +"';</script>";
                if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("back"))
                {
                    page.ClientScript.RegisterClientScriptBlock(typeof(Alert), "back", scriptBack);
                }
            }
        }


        /// <summary>
        /// Shows a client-side JavaScript alert in the browser.
        /// </summary>
        /// <param name="message">The message to appear in the alert.</param>
        public static void ShowBack(string message, string navegacion)
        {
            // Cleans the message to allow single quotation marks
            string cleanMessage = message.Replace("'", "\\'");
            string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";

            // Gets the executing web page
            Page page = HttpContext.Current.CurrentHandler as Page;

            // Checks if the handler is a Page and that the script isn't allready on the Page
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(Alert), "alert", script);
            }

            if (navegacion != null)
            {
                string scriptBack = "<script type=\"text/javascript\">" + navegacion + "</script>";
                if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("back"))
                {
                    page.ClientScript.RegisterClientScriptBlock(typeof(Alert), "back", scriptBack);
                }
            }
        }

        public static void Confirm(string message, string navegacionOK, string navegacionCANCEL)
        {
            // Cleans the message to allow single quotation marks
            string cleanMessage = message.Replace("'", "\\'");
            string script = "<script type=\"text/javascript\">if(confirm('" + cleanMessage + "')) location.href = '" + navegacionOK + "';else location.href = '" + navegacionCANCEL + "';</script>";

            // Gets the executing web page
            Page page = HttpContext.Current.CurrentHandler as Page;

            // Checks if the handler is a Page and that the script isn't allready on the Page
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(Alert), "alert", script);
            }

            if (navegacionOK != null)
            {
                string scriptBack = "<script type=\"text/javascript\">location.href = '" + navegacionOK + "';</script>";
                if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("back"))
                {
                    page.ClientScript.RegisterClientScriptBlock(typeof(Alert), "back", scriptBack);
                }
            }
        }
    }
}
