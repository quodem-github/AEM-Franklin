using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EOS.Entidades.Datos;
using EOS.ServiceLogic.BLL.GestorDocumental;



namespace EOS.Api
{
    public class UploadFileController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetList()
        {
            DDocumentUploadList list = new DDocumentUploadList() { List = new List<DDocumentUpload>() };
            return Request.CreateResponse(HttpStatusCode.OK, new { List = list, obj = new DDocumentUpload() });
        }

        [HttpGet]
        public HttpResponseMessage UnvalidateFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                string unEnriptData = Quodem.Utility.Cryptography.SimpleEncryption.Decrypt(filePath);
                GestorDocumentalServiceManager service = new GestorDocumentalServiceManager();
                service.UnvalidateDocument(unEnriptData);
                string mensaje = service.UnvalidateDocument(unEnriptData)
                    ? "Documento marcado como no valido"
                    : "Error al marcar el documento";
                return Request.CreateResponse(HttpStatusCode.OK, mensaje);

            }
            return Request.CreateResponse(HttpStatusCode.NoContent, "Error en el envio del fichero");
            
        }

        [HttpPost]
        public HttpResponseMessage SaveFileInfo([FromBody]DDocumentUploadList data)
        {
            GestorDocumentalServiceManager service = new GestorDocumentalServiceManager();
            string path = ConfigurationManager.AppSettings["AmecDocumentation"];
            foreach (var item in data.List)
            {
                if (item.NuevaVersion)
                    service.InsertNewVersion(item, path);
                else
                    service.InsertFirstVersion(item, path);

            }
            return Request.CreateResponse(HttpStatusCode.OK, service.ListaNoInsertados);
        }

        /*
         Otra posibilidad es 
         -> var httpRequest = currentHttpContext.Request;
         -> foreach (string file in httpRequest.Files)
         -> var postedFile = httpRequest.Files[file];
         -> postedFile.SaveAs(root + postedFile.FileName); 
         */
        [HttpPost]
        public async Task<HttpResponseMessage> PostFormData()
        {
            string errorMessage = string.Empty;
            try
            {
                Request.Content.Headers.ContentType.CharSet = "utf-8";
                // Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                var currentHttpContext = HttpContext.Current;
                string tempPath = ConfigurationManager.AppSettings["AmecDocumentationTemp"];
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(tempPath)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(tempPath));
                }

                string root = HttpContext.Current.Server.MapPath(tempPath);
                var provider = new CustomBulkMultipartFormDataStreamProvider(root);


                string[] tempfiles = Directory.GetFiles(root);
                foreach (var tempfile in tempfiles)
                {
                    File.Delete(tempfile);
                }
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                GestorDocumentalServiceManager service = new GestorDocumentalServiceManager();

                foreach (MultipartFileData file in provider.FileData)
                {
                    string[] info = file.Headers.ContentDisposition.Name.Split('|');
                    DDocumentUpload doc = service.ObtenerObjetoDocumento(info);
                    var tipo = service.GetTypeName(doc.Tipo);
                    var subTipo = service.GetSubTypeName(doc.SubTipo.Value);
                    string tmpPath = ConfigurationManager.AppSettings["AmecDocumentationTemp"];
                    string mapPath = ConfigurationManager.AppSettings["AmecDocumentation"] + (doc.Amec) + "/" + doc.Expediente + "/" + tipo + "/" + subTipo + "/";
                    string mapPathTemp = currentHttpContext.Server.MapPath(tmpPath);
                    if (!Directory.Exists(mapPath))
                    {
                        Directory.CreateDirectory(mapPath);
                    }
                    string latestFileFullPath = service.GetLatestVersionFullName(doc).Split('\\').Last();

                    try
                    {
                        File.Move(mapPathTemp + file.LocalFileName.Split('\\').Last(), Path.Combine(mapPath, latestFileFullPath));
                    }
                    catch (Exception e)
                    {
                        errorMessage += "*" + e.Message + "*" + e.StackTrace;
                        try
                        {
                            File.Copy(mapPathTemp + file.LocalFileName.Split('\\').Last(), Path.Combine(mapPath, latestFileFullPath));
                        }
                        catch (Exception exc)
                        {
                            errorMessage += "*" + exc.Message + "*" + exc.StackTrace;
                            Global.SendApplicationError(new Exception("UploadFileController.PostFormData -> Error al copiar el fichero: " + exc.Message), HttpContext.Current.Request, HttpContext.Current.Session, GetType().Name);

                            if (!service.DeleteDocumentInfo(doc))
                            {
                                Global.SendApplicationError(new Exception("UploadFileController.PostFormData -> Error al borrar los datos del fichero: " + exc.Message), HttpContext.Current.Request, HttpContext.Current.Session, GetType().Name);
                            }
                        }
                    }
                    try
                    {
                        service.SetMetadata(doc);
                    }
                    catch (Exception exc)
                    {
                        errorMessage += "*" + exc.Message + "*" + exc.StackTrace;
                    }

                }

                return Request.CreateResponse(HttpStatusCode.OK, new { IsError = false, info = "" });
            }
            catch (System.Exception ex)
            {
                errorMessage += "*" + ex.Message + "*" + ex.StackTrace;
                try
                {
                    Global.SendApplicationError(ex, HttpContext.Current.Request, HttpContext.Current.Session, GetType().Name);
                }
                catch (Exception exF) {
                    errorMessage += "*" + exF.Message + "*" + exF.StackTrace;
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new { IsError = true, Text = errorMessage, TextToShow = string.Empty });
            }
        }
        
        public class CustomBulkMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {
            public CustomBulkMultipartFormDataStreamProvider(string path): base(path)
            {
                
            }

            

            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                if (!string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName))
                {
                    var partes = headers.ContentDisposition.FileName;
                    return partes.Replace("\"", string.Empty).Replace(" ", "_");
                }
                else
                {
                    return null;
                }
            }
        }
    }
}