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
    public partial class DownloadGestorAllZip : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string key = Request.QueryString["file"];
            DirectoryInfo dir = new DirectoryInfo(key);
            string pathzip = dir.Parent.FullName;
            string fileZip = pathzip + "\\Temp.zip";
            string expedient = pathzip.Split('\\').Last();

            string[] directories = Directory.GetDirectories(pathzip);
            using (FileStream fsOut = File.Create(fileZip))

            using (var zipStream = new ZipOutputStream(fsOut))
            {
                foreach (string directory in directories)
                {
                    string[] files = Directory.GetFiles(directory);
                    string[] director = Directory.GetDirectories(directory);

                    foreach (string filePath in files)
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                        var fileEntry = new ZipEntry(Path.GetFileName(filePath))
                        {
                            Size = fileBytes.Length
                        };

                        zipStream.PutNextEntry(fileEntry);
                        zipStream.Write(fileBytes, 0, fileBytes.Length);
                    }

                    foreach (string d in director)
                    {
                        string[] fil = Directory.GetFiles(d);

                        foreach (string fp in fil)
                        {
                            byte[] fileBytes = System.IO.File.ReadAllBytes(fp);

                            var fileEntry = new ZipEntry(Path.GetFileName(fp))
                            {
                                Size = fileBytes.Length
                            };

                            zipStream.PutNextEntry(fileEntry);
                            zipStream.Write(fileBytes, 0, fileBytes.Length);
                        }

                    }

                }

                zipStream.Finish();
            }
            try
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + expedient + ".zip");
                Response.ContentType = "application/zip";
                Response.TransmitFile(fileZip);
                Response.Flush();
            }
            finally
            {
                File.Delete(fileZip);
            }
        }
    }
}