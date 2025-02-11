using System;
using System.Web;

namespace Quodem.Msd.SAML.Integrator.Test
{
    public static class Cookie
    {
        public static void SaveCookie(string name, string value)
        {
            HttpCookie myCookie = new HttpCookie(name);
            DateTime now = DateTime.Now;
            myCookie.Value = value;
            myCookie.Expires = now.AddDays(1);
            System.Web.HttpContext.Current.Response.Cookies.Add(myCookie);
        }
    }
}
