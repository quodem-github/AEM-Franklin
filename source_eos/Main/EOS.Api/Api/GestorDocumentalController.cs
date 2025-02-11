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
using System.Web.Http.Cors;
using EOS.Entidades.Datos;
using EOS.ServiceLogic;
using EOS.ServiceLogic.BLL;
using EOS.ServiceLogic.BLL.GestorDocumental;
using EOS.ServiceLogic.BLL.Services;
using EOS.ServiceLogic.Data;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceLogic.Data.DTO.ServiceEdition;
using EOS.ServiceLogic.Data.Filters;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceLogic.Enums;

namespace EOS.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GestorDocumentalController : ApiController
    {
        private IServiceGestorDocumental _service;
        private readonly AgencyKeyService _agencyKeyService;

        public GestorDocumentalController()
        {
            _agencyKeyService = new AgencyKeyService();
        }
        /*Falta la consulta*/
        [HttpPost]
        [ActionName("ListDocVersion")]
        public HttpResponseMessage ListDocVersion([FromBody] ReceivedData encryptedFilter)
        {
            var result = new SrvResponse();

            try
            {
                //Compruebo que el filtro no sea nulo
                if (encryptedFilter == null || string.IsNullOrEmpty(encryptedFilter.Data))
                {
                    result.AddError(ErrorCode.NullFilter, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.NullFilter.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                //Obtengo el Token y la agencia.
                var token = Request.Headers.GetValues(Variables.X_TOKEN).First();

                var agencyKey = _agencyKeyService.GetAgencyKey(Request.Headers.GetValues(Variables.AGENCY_KEY).First(), token, EOS.Api.WebApiApplication.LstConfEmpresas, EOS.Api.WebApiApplication.Suscriptor);
                if (string.IsNullOrEmpty(agencyKey))
                {
                    result.AddError(ErrorCode.BadAgencyKey, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.BadAgencyKey.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                //Creo el servicio con los datos de las cabeceras
                _service = new GestorDocumentalService<FilterGestorDocumentalDocumentoVersion, GestorDocumentalDocumentoVersionDto, GestorDocumentalDocumentoEditorDto>(token, agencyKey);

                var filterValidateResult = _service.ValidFilter(EOS.Api.WebApiApplication.Suscriptor, encryptedFilter.Data);
                if (filterValidateResult.IsError)
                {
                    foreach (var error in filterValidateResult.ErrorList)
                    {
                        result.AddError(error.Code, error.Description);
                    }

                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }
                result.Response = _service.GetListDocVersion(EOS.Api.WebApiApplication.Suscriptor);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Error servicio ExpedienteController.GetList " + ex.Message + " - " + ex.StackTrace);
                result.AddError(ErrorCode.DataNotValid,
                    Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DataNotValid.ToString()));

                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPost]
        [ActionName("ListDoc")]
        public HttpResponseMessage ListDoc([FromBody] ReceivedData encryptedFilter)
        {
            var result = new SrvResponse();

            try
            {
                //Compruebo que el filtro no sea nulo
                if (encryptedFilter == null || string.IsNullOrEmpty(encryptedFilter.Data))
                {
                    result.AddError(ErrorCode.NullFilter, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.NullFilter.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                //Obtengo el Token y la agencia.
                var token = Request.Headers.GetValues(Variables.X_TOKEN).First();

                var agencyKey = _agencyKeyService.GetAgencyKey(Request.Headers.GetValues(Variables.AGENCY_KEY).First(), token, EOS.Api.WebApiApplication.LstConfEmpresas, EOS.Api.WebApiApplication.Suscriptor);
                if (string.IsNullOrEmpty(agencyKey))
                {
                    result.AddError(ErrorCode.BadAgencyKey, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.BadAgencyKey.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                //Creo el servicio con los datos de las cabeceras
                _service = new GestorDocumentalService<FilterGestorDocumentalDocumento, GestorDocumentalDocumentoDto, GestorDocumentalDocumentoEditorDto>(token, agencyKey);

                var filterValidateResult = _service.ValidFilter(EOS.Api.WebApiApplication.Suscriptor, encryptedFilter.Data);
                if (filterValidateResult.IsError)
                {
                    foreach (var error in filterValidateResult.ErrorList)
                    {
                        result.AddError(error.Code, error.Description);
                    }

                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }
                result.Response = _service.GetListDoc(EOS.Api.WebApiApplication.Suscriptor);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Error servicio ExpedienteController.GetList " + ex.Message + " - " + ex.StackTrace);
                result.AddError(ErrorCode.DataNotValid,
                    Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DataNotValid.ToString()));

                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPost]
        [ActionName("List")]
        public HttpResponseMessage GetList([FromBody] ReceivedData encryptedFilter)
        {
            var result = new SrvResponse();

            try
            {
                //Obtengo el Token y la agencia.
                var token = Request.Headers.GetValues(Variables.X_TOKEN).First();

                var agencyKey = _agencyKeyService.GetAgencyKey(Request.Headers.GetValues(Variables.AGENCY_KEY).First(), token, EOS.Api.WebApiApplication.LstConfEmpresas, EOS.Api.WebApiApplication.Suscriptor);
                if (string.IsNullOrEmpty(agencyKey))
                {
                    result.AddError(ErrorCode.BadAgencyKey, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.BadAgencyKey.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                //Creo el servicio con los datos de las cabeceras
                _service = new GestorDocumentalService<FilterGestorDocumentalDocumento, GestorDocumentalDocumentoDto, GestorDocumentalDocumentoEditorDto>(token, agencyKey);

                result.Response = _service.GetList(EOS.Api.WebApiApplication.Suscriptor);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Error servicio GestorDocumentalController.GetList " + ex.Message + " - " + ex.StackTrace);
                result.AddError(ErrorCode.DataNotValid, Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DataNotValid.ToString()));
                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPost]
        [ActionName("ListTipoSeccion")]
        public HttpResponseMessage ListTipoSeccion([FromBody] ReceivedData encryptedFilter)
        {
            var result = new SrvResponse();

            try
            {
                //Obtengo el Token y la agencia.
                var token = Request.Headers.GetValues(Variables.X_TOKEN).First();

                var agencyKey = _agencyKeyService.GetAgencyKey(Request.Headers.GetValues(Variables.AGENCY_KEY).First(), token, EOS.Api.WebApiApplication.LstConfEmpresas, EOS.Api.WebApiApplication.Suscriptor);
                if (string.IsNullOrEmpty(agencyKey))
                {
                    result.AddError(ErrorCode.BadAgencyKey, Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.BadAgencyKey.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                //Creo el servicio con los datos de las cabeceras
                _service = new GestorDocumentalService<FilterBase, GestorDocumentalBase, GestorDocumentalDocumentoEditorDto>(token, agencyKey);

                result.Response = _service.GetListTipoSeccion(EOS.Api.WebApiApplication.Suscriptor);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Error servicio GestorDocumentalController.ListTipoSeccion " + ex.Message + " - " + ex.StackTrace);
                result.AddError(ErrorCode.DataNotValid, Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DataNotValid.ToString()));
                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPost]
        [ActionName("ListTipoDoc")]
        public HttpResponseMessage ListTipoDoc([FromBody] ReceivedData encryptedFilter)
        {
            var result = new SrvResponse();

            try
            {
                //Obtengo el Token y la agencia.
                var token = Request.Headers.GetValues(Variables.X_TOKEN).First();

                var agencyKey = _agencyKeyService.GetAgencyKey(Request.Headers.GetValues(Variables.AGENCY_KEY).First(), token, EOS.Api.WebApiApplication.LstConfEmpresas, EOS.Api.WebApiApplication.Suscriptor);
                if (string.IsNullOrEmpty(agencyKey))
                {
                    result.AddError(ErrorCode.BadAgencyKey, Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.BadAgencyKey.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                //Creo el servicio con los datos de las cabeceras
                _service = new GestorDocumentalService<FilterBase, GestorDocumentalBase, GestorDocumentalDocumentoEditorDto>(token, agencyKey);

                result.Response = _service.GetListTipoDoc(EOS.Api.WebApiApplication.Suscriptor);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Error servicio GestorDocumentalController.ListTipoDoc " + ex.Message + " - " + ex.StackTrace);
                result.AddError(ErrorCode.DataNotValid, Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DataNotValid.ToString()));
                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPost]
        [ActionName("ListSubTipoDoc")]
        public HttpResponseMessage ListSubTipoDoc([FromBody] ReceivedData encryptedFilter)
        {
            var result = new SrvResponse();

            try
            {
                //Obtengo el Token y la agencia.
                var token = Request.Headers.GetValues(Variables.X_TOKEN).First();

                var agencyKey = _agencyKeyService.GetAgencyKey(Request.Headers.GetValues(Variables.AGENCY_KEY).First(), token, EOS.Api.WebApiApplication.LstConfEmpresas, EOS.Api.WebApiApplication.Suscriptor);
                if (string.IsNullOrEmpty(agencyKey))
                {
                    result.AddError(ErrorCode.BadAgencyKey, Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.BadAgencyKey.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                //Creo el servicio con los datos de las cabeceras
                _service = new GestorDocumentalService<FilterBase, GestorDocumentalBase, GestorDocumentalDocumentoEditorDto>(token, agencyKey);

                result.Response = _service.GetListSubTipoDoc(EOS.Api.WebApiApplication.Suscriptor);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Error servicio GestorDocumentalController.ListSubTipoDoc " + ex.Message + " - " + ex.StackTrace);
                result.AddError(ErrorCode.DataNotValid, Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DataNotValid.ToString()));
                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public HttpResponseMessage Delete([FromBody] ReceivedData encryptedFilter)
        {
            var result = new SrvResponse();

            try
            {
                //Compruebo que el filtro no sea nulo
                if (encryptedFilter == null || string.IsNullOrEmpty(encryptedFilter.Data))
                {
                    result.AddError(ErrorCode.NullFilter, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.NullFilter.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                //Obtengo el Token y la agencia.
                var token = Request.Headers.GetValues(Variables.X_TOKEN).First();

                var agencyKey = _agencyKeyService.GetAgencyKey(Request.Headers.GetValues(Variables.AGENCY_KEY).First(), token, EOS.Api.WebApiApplication.LstConfEmpresas, EOS.Api.WebApiApplication.Suscriptor);
                if (string.IsNullOrEmpty(agencyKey))
                {
                    result.AddError(ErrorCode.BadAgencyKey, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.BadAgencyKey.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                //Creo el servicio con los datos de las cabeceras
                _service = new GestorDocumentalService<FilterGestorDocumentalDocumentoVersionBaja, GestorDocumentalDocumentoVersionBajaDto, GestorDocumentalDocumentoEditorDto>(token, agencyKey);

                var filterValidateResult = _service.ValidData(EOS.Api.WebApiApplication.Suscriptor, encryptedFilter.Data);
                if (filterValidateResult.IsError)
                {
                    foreach (var error in filterValidateResult.ErrorList)
                    {
                        result.AddError(error.Code, error.Description);
                    }
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                return Request.CreateResponse(HttpStatusCode.OK, _service.Delete(EOS.Api.WebApiApplication.Suscriptor));
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Error servicio GestorDocumentalController.Delete " + ex.Message + " - " + ex.StackTrace);
                result.AddError(ErrorCode.DataNotValid,
                    Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DataNotValid.ToString()));

                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<HttpResponseMessage> Edit()
        {
            var result = new SrvResponse();

            try
            {
                Request.Content.Headers.ContentType.CharSet = "utf-8";
                // Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    result.AddError(ErrorCode.ContentTypeError, Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.ContentTypeError.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                //Obtengo el Token y la agencia.
                var token = Request.Headers.GetValues(Variables.X_TOKEN).First();

                var agencyKey = _agencyKeyService.GetAgencyKey(Request.Headers.GetValues(Variables.AGENCY_KEY).First(), token, EOS.Api.WebApiApplication.LstConfEmpresas, EOS.Api.WebApiApplication.Suscriptor);
                if (string.IsNullOrEmpty(agencyKey))
                {
                    result.AddError(ErrorCode.BadAgencyKey, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.BadAgencyKey.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                var currentHttpContext = HttpContext.Current;

                var tempPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AmecDocumentationTemp"]);
                var path = ConfigurationManager.AppSettings["AmecDocumentation"];

                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                var provider = new CustomBulkMultipartFormDataStreamProvider(tempPath);

                //Borramos los ficheros de la carpeta auxiliar
                string[] tempfiles = Directory.GetFiles(tempPath);
                foreach (var tempfile in tempfiles)
                {
                    File.Delete(tempfile);
                }

                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                _service = new GestorDocumentalService<FilterGestorDocumentalDocumentoVersionBaja, GestorDocumentalDocumentoVersionBajaDto, GestorDocumentalDocumentoEditorDto>(token, agencyKey);

                MultipartFileData file = provider.FileData.FirstOrDefault();
                if (file != null)
                {
                    Quodem.Monitor.Alerta.WriteLog("FileName " + file.Headers.ContentDisposition.Name.Replace("\"", string.Empty));
                    var filterValidateResult = _service.ValidDataEditor(EOS.Api.WebApiApplication.Suscriptor, file.Headers.ContentDisposition.Name.Replace("\"", string.Empty));
                    if (filterValidateResult.IsError)
                    {
                        foreach (var error in filterValidateResult.ErrorList)
                        {
                            result.AddError(error.Code, error.Description);
                        }
                        return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, _service.SaveFile(EOS.Api.WebApiApplication.Suscriptor, file.LocalFileName.Split('\\').Last(), tempPath, path, currentHttpContext));
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, new { IsError = true, info = "No se ha recibido fichero" });
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Error servicio GestorDocumentalController.Edit " + ex.Message + " - " + ex.StackTrace);
                result.AddError(ErrorCode.DataNotValid, Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DataNotValid.ToString()));
                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }
    }

    public class CustomBulkMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomBulkMultipartFormDataStreamProvider(string path) : base(path)
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