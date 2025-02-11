using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using EOS.Entidades.Datos;
using EOS.ServiceLogic.BLL.GestorDocumental;
using EOS.ServiceLogic.BLL.Validator;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceLogic.Data.DTO.ServiceEdition;
using EOS.ServiceLogic.Data.Filters;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceLogic.Enums;
using EOS.ServiceModel;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Linq;

namespace EOS.ServiceLogic.BLL.Services
{
    public class GestorDocumentalService<FT, DT, ET> : IServiceGestorDocumental
        where FT : FilterBase
        where DT: GestorDocumentalBase
        where ET : GestorDocumentalDocumentoEditorDto
    {
        #region Definitions

        private ISession _sessVariable;
        public ISession _session
        {
            get
            {
                if (!_sessVariable.IsOpen)
                {
                    _sessVariable = Quodem.ORM.NHibernate.Helper.GetCurrentSession(ServiceLogic.Enums.EDbConnection.Default.GetHashCode());
                }
                return _sessVariable;
            }
            set { _sessVariable = value; }
        }

        #endregion

        #region Properties

        private QuodemLogService _logService;
        private FT _filter;
        public DT _data { get; set; }
        private ET _dataEditor;
        private readonly string _token;
        private readonly string _agencyKey;

        #endregion

        public GestorDocumentalService(string token, string agencyKey)
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
            _token = token;
            _agencyKey = agencyKey;
            _logService = new QuodemLogService(_session);
        }

        public ServiceError ValidFilter(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<FT>
            {
                DateFromProp = "CreationDateFrom",
                DateToProp = "CreationDateTo",
                DateFromProp2 = "UpdateDateFrom",
                DateToProp2 = "UpdateDateTo"
            };
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public ServiceError ValidDataEditor(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<ET>();
            _dataEditor = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<DT>();
            _data = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }


        public string GetList(QSuscriptor suscriptor)
        {
            throw new NotImplementedException();
        }

        public SrvResponse Delete(QSuscriptor suscriptor)
        {
            var result = new SrvResponse()
            {
                ErrorList = new List<ErrorItem>()
            };

            var _dataFull = (GestorDocumentalDocumentoVersionBajaDto)(GestorDocumentalBase)_data;

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    var query = _session.Query<GestordocumentalDocumentoversion>()
                .Join(_session.Query<GestordocumentalDocumento>(), docv => new { IdDoc = docv.Iddocumento },
                    doc => new { IdDoc = doc.Id }, (docv, doc) => new { docv, doc.Id, doc.Idamec })
                .Join(_session.Query<Amecs>(), amdoc => new { IdAmec = amdoc.Idamec }, am => new { IdAmec = am.Idamecs },
                    (amdoc, am) => new { amdoc, am.Idconfempresa })
                .Join(_session.Query<Confempresa>(), amco => new { IdConf = amco.Idconfempresa ?? Variables.NULL_INT },
                    conf => new { IdConf = conf.Idconfempresa }, (amco, conf) => new { amco, conf.Idagencia })
                .Where(x => x.amco.amdoc.docv.Id == _dataFull.Id && x.amco.amdoc.docv.Iddocumento == _dataFull.IdDocumento && x.Idagencia == _agencyKey);

                    var results = query.ToList();

                    bool isAgencyService = results.Count >= 1;

                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.GestorDocumentalDeleteError.GetHashCode(),
                            Description =
                                Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.GestorDocumentalDeleteError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("gestordocumental_documentoversion", JsonConvert.SerializeObject(_data),
                            "Error en GestorDocumentalService.Delete: IdDocumento no pertenece a la agencia que está usando el servicio",
                            _agencyKey);
                        transaction.Commit();
                        return result;
                    }

                    _session.Flush();

                    var editData = results.FirstOrDefault().amco.amdoc.docv;
                    editData.Documentonovalido = true;
                    editData.Fechamodificacion = DateTime.Now;

                    _session.SaveOrUpdate(editData);



                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new GestorDocumentalDocumentoVersionResponseDto()
                    {
                        GestorDocumentalDocumentoVersionResult =
                            new List<GestorDocumentalDocumentoVersionDto>()
                            {
                                Mapper.Map<GestorDocumentalDocumentoVersionDto>(editData)
                            }
                    };

                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("gestordocumental_documentoversion", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(Mapper.Map<GestorDocumentalDocumentoVersionDto>(editData)),
                        _agencyKey);

                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en GestorDocumental.Delete: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.GestorDocumentalDeleteError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.GestorDocumentalDeleteError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("gestordocumental_documentoversion", JsonConvert.SerializeObject(_data),
                        "Error en GestorDocumental.Delete: " + e.Message, _agencyKey);

                    return result;
                }
            }
        }

        public SrvResponse Edit(QSuscriptor suscriptor)
        {
            throw new NotImplementedException();
        }

        public string GetListTipoSeccion(QSuscriptor suscriptor)
        {
            var query = _session.Query<GestordocumentalTipoSeccion>();

            var tipoSeccion = query.Select(x => Mapper.Map<GestorDocumentalTipoSeccionDto>(x)).ToList();

            var result = new GestorDocumentalTipoSeccionResponseDto()
            {
                GestorDocumentalTipoSeccionList = tipoSeccion
            };

            _logService.AddLogEntry("GestorDocumentalTipoSeccion", "", tipoSeccion.Count.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public string GetListTipoDoc(QSuscriptor suscriptor)
        {
            var query = _session.Query<GestordocumentalTipodoc>();

            var tipoDoc = query.Select(x => Mapper.Map<GestorDocumentalTipoDocDto>(x)).ToList();

            var result = new GestorDocumentalTipoDocResponseDto()
            {
                GestorDocumentalTipoDocList = tipoDoc
            };

            _logService.AddLogEntry("GetListTipoDoc", "", tipoDoc.Count.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public string GetListSubTipoDoc(QSuscriptor suscriptor)
        {
            var query = _session.Query<GestordocumentalSubtipodoc>();

            var subTipoDoc = query.Select(x => Mapper.Map<GestorDocumentalSubTipoDocDto>(x)).ToList();

            var result = new GestorDocumentalSubTipoDocResponseDto()
            {
                GestorDocumentalSubTipoDocList = subTipoDoc
            };

            _logService.AddLogEntry("GetListSubTipoDoc", "", subTipoDoc.Count.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public string GetListDoc(QSuscriptor suscriptor)
        {
            var query = _session.Query<GestordocumentalDocumento>()
                .Join(_session.Query<Amecs>(), doc => new { IdAmec = doc.Idamec }, am => new { IdAmec = am.Idamecs },
                    (doc, am) => new { doc, am.Idconfempresa })
                .Join(_session.Query<Confempresa>(),
                    amdocco => new { IdConf = amdocco.Idconfempresa ?? Variables.NULL_INT },
                    conf => new { IdConf = conf.Idconfempresa }, (amdocco, conf) => new { amdocco.doc, conf.Idagencia })
                .Where(x => x.Idagencia == _agencyKey);

            var filter = _filter as FilterGestorDocumentalDocumento;

            if (filter != null && filter.Id != null)
            {
                query = query.Where(x => x.doc.Id == filter.Id.Value);
            }
            if (filter != null && !String.IsNullOrWhiteSpace(filter.IdAmec))
            {
                query = query.Where(x => x.doc.Idamec == filter.IdAmec);
            }
            if (filter != null && filter.IdExpediente != null)
            {
                query = query.Where(x => x.doc.Idexpediente == filter.IdExpediente.Value);
            }
            if (filter != null && filter.IdSubTipoDoc != null)
            {
                query = query.Where(x => x.doc.Idsubtipodoc == filter.IdSubTipoDoc.Value);
            }
            if (filter != null && filter.IdTipoDoc != null)
            {
                query = query.Where(x => x.doc.Idtipodoc == filter.IdTipoDoc.Value);
            }
            if (filter != null && filter.CreationDateFrom != null && filter.CreationDateFrom != DateTime.MinValue)
            {
                query = query.Where(x => x.doc.Fecha >= filter.CreationDateFrom);
            }
            if (filter != null && filter.CreationDateTo != null && filter.CreationDateTo != DateTime.MinValue)
            {
                query = query.Where(x => x.doc.Fecha <= filter.CreationDateTo);
            }
            if (filter != null && filter.UpdateDateFrom != null && filter.UpdateDateFrom != DateTime.MinValue)
            {
                query = query.Where(x => x.doc.Fechamodificacion >= filter.UpdateDateFrom);
            }
            if (filter != null && filter.UpdateDateTo != null && filter.UpdateDateTo != DateTime.MinValue)
            {
                query = query.Where(x => x.doc.Fechamodificacion <= filter.UpdateDateTo);
            }

            var docs = query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                .Take(Variables.ServicePageSize)
                .Select(x => Mapper.Map<GestorDocumentalDocumentoDto>(x.doc)).ToList();

            int rowCount = query.Count();

            var result = new GestorDocumentalDocumentoResponseDto()
            {
                GestorDocumentalDocumentoList = docs,
                Id = filter.Id,
                IdExpediente = filter.IdExpediente,
                IdAmec = filter.IdAmec,
                IdSubTipoDoc = filter.IdSubTipoDoc,
                IdTipoDoc = filter.IdTipoDoc,
                CurrentPageIndex = filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                CreationDateFrom = filter.CreationDateFrom,
                CreationDateTo = filter.CreationDateTo,
                UpdateDateFrom = filter.UpdateDateFrom,
                UpdateDateTo = filter.UpdateDateTo,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("GetListDoc", JsonConvert.SerializeObject(filter), rowCount.ToString(), _agencyKey,
                false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public string GetListDocVersion(QSuscriptor suscriptor)
        {
            var query = _session.Query<GestordocumentalDocumentoversion>()
                .Join(_session.Query<GestordocumentalDocumento>(), ver => new { IdDoc = ver.Iddocumento },
                    doc => new { IdDoc = doc.Id }, (ver, doc) => new { doc, ver })
                .Join(_session.Query<Amecs>(), docver => new { IdAmec = docver.doc.Idamec },
                    am => new { IdAmec = am.Idamecs },
                    (docver, am) => new { docver, docver.ver, am.Idconfempresa })
                .Join(_session.Query<Confempresa>(),
                    amdocco => new { IdConf = amdocco.Idconfempresa ?? Variables.NULL_INT },
                    conf => new { IdConf = conf.Idconfempresa },
                    (amdocco, conf) => new { amdocco.docver.doc, amdocco.ver, conf.Idagencia })
                .Where(x => x.Idagencia == _agencyKey);

            var filter = _filter as FilterGestorDocumentalDocumentoVersion;

            if (filter != null && filter.Id != null)
            {
                query = query.Where(x => x.ver.Id == filter.Id.Value);
            }
            if (filter != null && filter.IdDocumento != null)
            {
                query = query.Where(x => x.doc.Id == filter.IdDocumento.Value);
            }
            if (filter != null && !String.IsNullOrWhiteSpace(filter.IdAmec))
            {
                query = query.Where(x => x.doc.Idamec == filter.IdAmec);
            }
            if (filter != null && filter.IdExpediente != null)
            {
                query = query.Where(x => x.doc.Idexpediente == filter.IdExpediente.Value);
            }
            if (filter != null && filter.CreationDateFrom != null && filter.CreationDateFrom != DateTime.MinValue)
            {
                query = query.Where(x => x.ver.Fecha >= filter.CreationDateFrom);
            }
            if (filter != null && filter.CreationDateTo != null && filter.CreationDateTo != DateTime.MinValue)
            {
                query = query.Where(x => x.ver.Fecha <= filter.CreationDateTo);
            }
            if (filter != null && filter.UpdateDateFrom != null && filter.UpdateDateFrom != DateTime.MinValue)
            {
                query = query.Where(x => x.ver.Fechamodificacion >= filter.UpdateDateFrom);
            }
            if (filter != null && filter.UpdateDateTo != null && filter.UpdateDateTo != DateTime.MinValue)
            {
                query = query.Where(x => x.ver.Fechamodificacion <= filter.UpdateDateTo);
            }

            var docs = query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                .Take(Variables.ServicePageSize)
                .Select(x => Mapper.Map<GestorDocumentalDocumentoVersionDto>(x.ver)).ToList();

            int rowCount = query.Count();

            var result = new GestorDocumentalDocumentoVersionResponseDto()
            {
                GestorDocumentalDocumentoVersionResult = docs,
                Id = filter.Id,
                IdDocumento = filter.IdDocumento,
                IdExpediente = filter.IdExpediente,
                IdAmec = filter.IdAmec,
                CurrentPageIndex = filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                CreationDateFrom = filter.CreationDateFrom,
                CreationDateTo = filter.CreationDateTo,
                UpdateDateFrom = filter.UpdateDateFrom,
                UpdateDateTo = filter.UpdateDateTo,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("GetListDocVersion", JsonConvert.SerializeObject(filter), rowCount.ToString(),
                _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public SrvResponse SaveFile(QSuscriptor suscriptor, string fileName, string tempDirectory, string targetDirectory, System.Web.HttpContext context)
        {
            var result = new SrvResponse()
            {
                ErrorList = new List<ErrorItem>()
            };

            //Comprobacion si amec pertenece a la agencia
            #region CheckAmecAgencia
            var isCorrectAgency = _session.Query<Amec>()
             .Join(_session.Query<Confempresa>(), amec => new { IdConf = amec.Idconfempresa ?? Variables.NULL_INT },
                 conf => new { IdConf = conf.Idconfempresa }, (amec, conf) => new { amec.Idamec, conf.Idagencia })
             .Any(x => x.Idagencia == _agencyKey && x.Idamec == _dataEditor.IdAmec);
            if (!isCorrectAgency)
            {
                result.ErrorList.Add(new ErrorItem()
                {
                    Code = ErrorCode.GestorDocumentalAmecAgenciaError.GetHashCode(),
                    Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.GestorDocumentalAmecAgenciaError.ToString())
                });
                _logService.AddLogEntry("gestordocumental_documentoversion", JsonConvert.SerializeObject(_dataEditor),
                    "Error en GestorDocumental.Edit: El amec no pertenece a la agencia.", _agencyKey);
                return result;
            }
            #endregion
            //Comprobación relacion amec - expediente
            #region CheckRelacionAmecExpediente
            var expedienteAmecList = _session.Query<Amec>()
                .Join(_session.Query<Expediente>(), amec => new { amec.Idamec }, exp => new { exp.Idamec },
                    (amec, exp) => new { amec.Idamec, amec.Amec1, exp.Idxpediente })
                .Where(x => x.Idamec == _dataEditor.IdAmec).ToList();

            var expedienteList = expedienteAmecList.Select(x => x.Idxpediente).ToList();

            if (!expedienteList.ToList().Contains(_dataEditor.IdExpediente))
            {
                result.ErrorList.Add(new ErrorItem()
                {
                    Code = ErrorCode.GestorDocumentalAmecExpedienteError.GetHashCode(),
                    Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.GestorDocumentalAmecExpedienteError.ToString())
                });
                _logService.AddLogEntry("gestordocumental_documentoversion", JsonConvert.SerializeObject(_dataEditor),
                    "Error en GestorDocumental.Edit: El IdExpediente no esta asociado al IdAmec.", _agencyKey);
                return result;
            }
            #endregion
            //Comprobación si los ids de los tipos y subtipos corresponden al tipo de expediente (individual o colectivo)
            #region CheckTipo
            var expediente = _session.Query<Expediente>().FirstOrDefault(x => x.Idxpediente == _dataEditor.IdExpediente);
            if (expediente != null && expediente.Idtiporeserva == EExpedienteType.Colectivo.GetHashCode())
            {
                var tiposList = _session.Query<GestordocumentalTipodoc>().Where(x => x.Idtipo == EExpedienteType.Colectivo.GetHashCode()).Select(x => x.Id);
                if (!tiposList.ToList().Contains(_dataEditor.IdTipodoc))
                {
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.GestorDocumentalTipoDocumentoError.GetHashCode(),
                        Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.GestorDocumentalTipoDocumentoError.ToString())
                    });
                    _logService.AddLogEntry("gestordocumental_documentoversion", JsonConvert.SerializeObject(_dataEditor),
                        "Error en GestorDocumental.Edit: El Tipo de documento no corresponde a la lista de tipos posibles para el tipo de Expediente", _agencyKey);
                    return result;
                }
            }
            #endregion


            var amecId = expedienteAmecList.Find(x => x.Idamec == _dataEditor.IdAmec).Amec1;

            GestorDocumentalServiceManager service = new GestorDocumentalServiceManager();

            var tipo = service.GetTypeName(_dataEditor.IdTipodoc);
            var subTipo = _dataEditor.IdSubTipodoc.HasValue ? service.GetSubTypeName(_dataEditor.IdSubTipodoc.Value) : "";
            var finaltargetDirectory = targetDirectory + "\\" + amecId + "\\" + _dataEditor.IdExpediente + "\\" + tipo + "\\";

            if (!string.IsNullOrEmpty(subTipo))
            {
                finaltargetDirectory += subTipo + "\\";
            }

            var data = new DDocumentUpload()
            {
                Amec = amecId,
                Asistente = string.IsNullOrEmpty(_dataEditor.IdPassengerList.ToString()) ? "-1" : _dataEditor.IdPassengerList.ToString(),
                DocumentoOriginal = _dataEditor.NombreOriginal,
                Expediente = _dataEditor.IdExpediente.ToString(),
                Fichero = fileName,
                NombreAdicional = _dataEditor.CampoLibre,
                NuevaVersion = _dataEditor.NuevaVersion,
                Tipo = _dataEditor.IdTipodoc,
                SubTipo = _dataEditor.IdSubTipodoc ?? -1,
                Peticionario = new int?(),
                IdAmecDocCategory = new int?()
            };

            //Comprobación de si existe una version anterior y se intenta guardar una nueva version no inicial
            #region CheckVersionAnteriorExists
            if (_dataEditor.NuevaVersion && service.GetLastVersion(data) == null)
            {
                result.ErrorList.Add(new ErrorItem()
                {
                    Code = ErrorCode.GestorDocumentalVersionAnteriorNoExistsError.GetHashCode(),
                    Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.GestorDocumentalVersionAnteriorNoExistsError.ToString())
                });
                _logService.AddLogEntry("gestordocumental_documentoversion", JsonConvert.SerializeObject(_dataEditor),
                    "Error en GestorDocumental.Edit: Se esta intentando insertar una nueva version de un documento que no existe.", _agencyKey);
                return result;
            }
            #endregion

            var isValidInsert = _dataEditor.NuevaVersion ? service.InsertNewVersion(data, targetDirectory) : service.InsertFirstVersion(data, targetDirectory);

            if (isValidInsert)
            {
                if (!Directory.Exists(finaltargetDirectory))
                {
                    Directory.CreateDirectory(finaltargetDirectory);
                }

                var latestVersionFileName = service.GetLatestVersionFullName(data).Split('\\').Last();
                try
                {
                    File.Move(tempDirectory + fileName, Path.Combine(finaltargetDirectory, latestVersionFileName));
                }
                catch (Exception e)
                {
                    try
                    {
                        File.Copy(tempDirectory + fileName, Path.Combine(finaltargetDirectory, latestVersionFileName));
                    }
                    catch (Exception exc)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.ErrorDocumentalRutaFichero.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.ErrorDocumentalRutaFichero.ToString())
                        });
                        _logService.AddLogEntry("gestordocumental_documentoversion", JsonConvert.SerializeObject(_data),
                            "Error en GestorDocumental.Edit: Almacenar el fichero " + fileName + " en el servidor. No se ha podido almacenar el fichero. Se procede a borrar los datos de la base de datos.", _agencyKey);
                        if (!service.DeleteDocumentInfo(data))
                        {
                            _logService.AddLogEntry("gestordocumental_documentoversion", JsonConvert.SerializeObject(_data),
                            "Error en GestorDocumental.Edit: No se ha podido borrar los datos de la base de datos del fichero " + fileName, _agencyKey);
                        }
                        return result;
                    }
                }
                if (result.ErrorList.Count == 0)
                {
                    //try
                    //{
                    //    service.SetMetadata(data);
                    //}
                    //catch(Exception ex)
                    //{
                    //    _logService.AddLogEntry("gestordocumental_documentoversion", JsonConvert.SerializeObject(_data), "Error en GestorDocumental.Edit - SetMetadata: No se ha asignar el metadata al fichero " + fileName, _agencyKey);
                    //}

                    GestordocumentalDocumentoversion version = service.GetLastVersion(data);
                    GestordocumentalDocumento documento = _session.Query<GestordocumentalDocumento>().FirstOrDefault(x => x.Id == version.Iddocumento);

                    GestorDocumentalDocumentoEditorResultResponseDto response = new GestorDocumentalDocumentoEditorResultResponseDto()
                    {
                        GestorDocumentalDocumentoEditor = _dataEditor,
                        GestorDocumentalDocumentoEditorResult = ConvertToEditorResultDto(version, documento)
                    };

                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);
                }

            }
            else
            {
                result.ErrorList.Add(new ErrorItem()
                {
                    Code = ErrorCode.GestorDocumentalEditorError.GetHashCode(),
                    Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.GestorDocumentalEditorError.ToString())
                });
                _logService.AddLogEntry("gestordocumental_documentoversion", JsonConvert.SerializeObject(_data),
                    "Error en GestorDocumental.Edit: Error al insertar el fichero " + fileName + " en la base de datos. No se ha podido almacenar el fichero.", _agencyKey);
            }

            //Generar el log en la tabla de auditoria
            _logService.AddLogEntry("gestordocumental_documentoversion", JsonConvert.SerializeObject(_data),
                JsonConvert.SerializeObject(_data),
                _agencyKey);

            return result;
        }

        protected GestorDocumentalDocumentoEditorResultDto ConvertToEditorResultDto(GestordocumentalDocumentoversion version, GestordocumentalDocumento documento)
        {
            return new GestorDocumentalDocumentoEditorResultDto()
            {
                Id = documento.Id,
                IdDocumentoVersion = version.Id,
                IdAmec = documento.Idamec,
                IdExpediente = documento.Idexpediente,
                IdTipodoc = documento.Idtipodoc,
                IdSubTipodoc = documento.Idsubtipodoc,
                IdPassengerList = documento.Idpassengerlist,
                FechaCreacionDocumento = documento.Fecha.ToString("MM/dd/yyyy"),
                FechaModificacionDocumento = documento.Fechamodificacion.ToString("MM/dd/yyyy"),
                FechaCreacionDocumentoVersion = version.Fecha.ToString("MM/dd/yyyy"),
                FechaModificacionDocumentoVersion = version.Fechamodificacion.ToString("MM/dd/yyyy"),
                CampoLibre = documento.Campolibre,
                NombreOriginal = version.Nombreoriginal,
                version = version.Version,
                DocumentoNoValido = version.Documentonovalido
            };
        }
    }
}
