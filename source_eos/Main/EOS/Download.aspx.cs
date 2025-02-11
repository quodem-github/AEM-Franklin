using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace EOS
{
    public partial class Download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string dir = System.Configuration.ConfigurationManager.AppSettings["PathDocuments"];
            string key = Request.QueryString["key"];
            string file = EOS.Logica.Utility.ValueDecrypt(Request.QueryString["file"]);
            string idexp = EOS.Web.Encriptacion.Decrypt(key.Substring(0, key.Length - 4), "Documents" + key.Substring(key.Length - 4));
            string stream = dir + idexp + "\\" + file;
            FileStream liveStream = new FileStream(stream, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)liveStream.Length];
            liveStream.Read(buffer, 0, (int)liveStream.Length);
            liveStream.Close();
            Response.ClearHeaders();
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Replace(' ', '_'));
            Response.BinaryWrite(buffer);
            Response.End();            
        }
    }
}