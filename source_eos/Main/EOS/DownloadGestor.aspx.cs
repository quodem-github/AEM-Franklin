using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

namespace EOS
{
    public partial class DownloadGestor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string key = Request.QueryString["file"];
            string unEnriptData = Quodem.Utility.Cryptography.SimpleEncryption.Decrypt(key);
            string fileName = unEnriptData.Split('/').Last();
            FileStream liveStream = new FileStream(unEnriptData, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)liveStream.Length];
            liveStream.Read(buffer, 0, (int)liveStream.Length);
            liveStream.Close();
            Response.ClearHeaders();
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName.Replace(' ', '_'));
            Response.BinaryWrite(buffer);
            Response.End();            
        }
    }
}