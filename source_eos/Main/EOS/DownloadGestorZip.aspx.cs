using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EOS
{
    public partial class DownloadGestorZip : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string key = Request.QueryString["file"];
            string fileZip = key + "\\Temp.zip";
            string[] files = Directory.GetFiles(key);
            string[] directories = Directory.GetDirectories(key);
            string nameZip = key.Split('\\').Last();

            using (FileStream fsOut = File.Create(fileZip))

            using (var zipStream = new ZipOutputStream(fsOut))
            {

                foreach (string filePath in files)
                {
                    //var fi = new FileInfo(filePath);

                    byte[] fileBytes = File.ReadAllBytes(filePath);

                    var fileEntry = new ZipEntry(Path.GetFileName(filePath))
                    {
                        Size = fileBytes.Length
                    };

                    zipStream.PutNextEntry(fileEntry);

                    zipStream.Write(fileBytes, 0, fileBytes.Length);

                }
                foreach (string directory in directories)
                {
                    string[] fil = Directory.GetFiles(directory);

                    foreach (string f in fil)
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(f);

                        var fileEntry = new ZipEntry(Path.GetFileName(f))
                        {
                            Size = fileBytes.Length
                        };

                        zipStream.PutNextEntry(fileEntry);
                        zipStream.Write(fileBytes, 0, fileBytes.Length);
                    }
                }

                    zipStream.Finish();
            }

            try
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + nameZip + ".zip");
                Response.ContentType = "application/zip";
                Response.TransmitFile(fileZip);
                //Response.Close();
                //Response.End();
                Response.Flush();
            }
            finally
            {
                File.Delete(fileZip);
            }



        }


    }
}