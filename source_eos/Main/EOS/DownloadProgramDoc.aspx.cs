using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace EOS
{
    public partial class DownloadProgramDoc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string dirAppSetting = System.Configuration.ConfigurationManager.AppSettings["AMECUpload"];
            string dir = dirAppSetting; //HttpContext.Current.Server.MapPath(dirAppSetting);
            //string key = Request.QueryString["key"];
            string file = Request.QueryString["file"];
            string Tipodoc = Request.QueryString["TipoDoc"];
            string idamec = Request.QueryString["idamec"];
            string streamAlt = "";
            string stream = "";
            if (Tipodoc == "Documentacion")
            {
                if (file.Split('|').Length > 1)
                {
                    stream = dir + "\\" + idamec + "\\" + Tipodoc + "\\" + file.Split('|').Last().Replace(' ', '_') + "\\" + file.Split('|').First();
                    streamAlt = dir + "\\" + idamec + "\\" + Tipodoc + "\\" + file.Split('|').First();
                }
                else
                {
                    stream = dir + "\\" + idamec + "\\" + Tipodoc + "\\" + file.Replace(' ', '_');
                    streamAlt = dir + "\\" + idamec + "\\" + Tipodoc + "\\" + file;
                }
            }
            else stream = dir + "\\" + idamec + "\\Programa\\" + file;
            FileStream liveStream;
            if (File.Exists(stream))
            {
                liveStream = new FileStream(stream, FileMode.Open, FileAccess.Read);
            }
            else
            {
                liveStream = new FileStream(streamAlt, FileMode.Open, FileAccess.Read);
            }
            byte[] buffer = new byte[(int) liveStream.Length];
            liveStream.Read(buffer, 0, (int) liveStream.Length);
            liveStream.Close();
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file.Split('|').First() + "\"");
            Response.BinaryWrite(buffer);
            Response.End();
        }
    }
}