using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using EOS.Entidades.Datos;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceLogic.Enums;
using EOS.ServiceModel;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Linq;

namespace EOS.ServiceLogic.BLL.GestorDocumental
{
    public class GestorDocumentalServiceManager
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
        public List<string> ListaNoInsertados { get; set; }

        #endregion

        public GestorDocumentalServiceManager()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
            ListaNoInsertados = new List<string>();
        }


        #region DocumentacionExpedientes
        public bool UnvalidateDocument(string path)
        {
            var dovVersion =
                _session.Query<GestordocumentalDocumentoversion>().FirstOrDefault(x => x.Rutafichero == path.Replace("/", "\\"));

            if (dovVersion == null) return false;

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    dovVersion.Documentonovalido = true;
                    dovVersion.Fechamodificacion = DateTime.Now;
                    _session.SaveOrUpdate(dovVersion);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }

        }

        public List<GestordocumentalDocumento> GetDocumentInfo(GestordocumentalDocumento filter)
        {
            var query = _session.Query<GestordocumentalDocumento>();
            if (!String.IsNullOrWhiteSpace(filter.Idamec))
                query = query.Where(x => x.Idamec == filter.Idamec);
            if (filter.Idexpediente != Variables.NULL_INT && filter.Idexpediente != 0)
                query = query.Where(x => x.Idexpediente == filter.Idexpediente);
            if (filter.Id != Variables.NULL_INT && filter.Id != 0)
                query = query.Where(x => x.Id == filter.Id);
            if (filter.Idtipodoc != Variables.NULL_INT && filter.Idtipodoc != 0)
                query = query.Where(x => x.Idtipodoc == filter.Idtipodoc);
            if (filter.Idsubtipodoc != Variables.NULL_INT && filter.Idsubtipodoc != 0)
                query = query.Where(x => x.Idsubtipodoc == filter.Idsubtipodoc);
            if (filter.Idpassengerlist != Variables.NULL_INT)
                query = query.Where(x => x.Idpassengerlist == filter.Idpassengerlist);
            if (filter.Idpeticionario != Variables.NULL_INT && filter.Idpeticionario != 0)
                query = query.Where(x => x.Idpeticionario == filter.Idpeticionario);

            return query.ToList();
        }

        public List<GestordocumentalDocumento> GetAllDocumentInfo()
        {
            return GetDocumentInfo(new GestordocumentalDocumento()
            {
                Id = -1,
                Idamec = string.Empty,
                Idpassengerlist = -1,
                Idtipodoc = -1,
                Idexpediente = -1,
                Idpeticionario = -1,
                Idsubtipodoc = -1
            });
        }

        public bool DocumentExist(DDocumentUpload item)
        {
            return GetDocumentFilteredByNameList(item).Any();

        }

        public bool DeleteDocumentInfo(DDocumentUpload item)
        {
            try
            {

                var searchedDocVersion = GetLastVersion(item);

                using (var transaction = _session.BeginTransaction())
                {
                    _session.Delete(searchedDocVersion);
                    if (searchedDocVersion.Version == 0)
                    {
                        var doc = _session.Query<GestordocumentalDocumento>().FirstOrDefault(x => x.Id == searchedDocVersion.Iddocumento);
                        _session.Delete(doc);
                    }
                    transaction.Commit();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CopyDocsAmec(string idAmecOrigen, string idAmecDestino)
        {
            var docs = _session.Query<GestordocumentalDocumento>().Where(x => x.Idamec == idAmecOrigen).ToList();
            List<int> lstIdDocucs = docs.Select(x => x.Id).ToList();
            var docsVersion = _session.Query<GestordocumentalDocumentoversion>().Where(x => lstIdDocucs.Contains(x.Iddocumento)).ToList();

            foreach (var documento in docs)
            {
                GestordocumentalDocumento doc = new GestordocumentalDocumento()
                {
                    Idamec = idAmecDestino,
                    Idexpediente = documento.Idexpediente,
                    Idtipodoc = documento.Idtipodoc,
                    Idsubtipodoc = documento.Idsubtipodoc,
                    Idpeticionario = documento.Idpeticionario,
                    Idpassengerlist = documento.Idpassengerlist,
                    Fecha = documento.Fecha,
                    Campolibre = documento.Campolibre
                };
                _session.SaveOrUpdate(doc);

                var docsVersionFiltered = docsVersion.Where(x => x.Iddocumento == documento.Id).ToList();
                foreach (var documentoversion in docsVersionFiltered)
                {
                    string savePathOrigin = string.Format("{0}{1}", ConfigurationManager.AppSettings["AmecDocumentation"], idAmecOrigen);
                    string savePathReplace = string.Format("{0}{1}", ConfigurationManager.AppSettings["AmecDocumentation"], idAmecDestino);
                    GestordocumentalDocumentoversion docVersion = new GestordocumentalDocumentoversion()
                    {
                        Nombreoriginal = documentoversion.Nombreoriginal,
                        Rutafichero = documentoversion.Rutafichero.Replace(savePathOrigin, savePathReplace),
                        Version = documentoversion.Version,
                        Idpeticionario = documentoversion.Idpeticionario,
                        Fecha = documentoversion.Fecha,
                        Metadata = documentoversion.Metadata,
                        Documentonovalido = documentoversion.Documentonovalido,
                        Iddocumento = doc.Id
                    };
                    _session.SaveOrUpdate(docVersion);
                }

            }

            return true;
        }

        public bool InsertFirstVersion(DDocumentUpload item, string path)
        {
            int initialVersion = 0;
            try
            {
                if (DocumentExist(item))
                {
                    ListaNoInsertados.Add(item.Fichero.ToString().Split('\\').Last());
                    InsertNewVersion(item, path);
                    return true;
                }

                GestordocumentalDocumento doc = new GestordocumentalDocumento()
                {
                    Idamec = item.Amec,
                    Idexpediente = !string.IsNullOrWhiteSpace(item.Expediente) ? int.Parse(item.Expediente) : new int?(),
                    Idtipodoc = item.Tipo,
                    Idsubtipodoc = item.SubTipo == -1 ? null : item.SubTipo,
                    Idpeticionario = item.Peticionario,
                    Idpassengerlist = item.Asistente == "-1" ? new int?() : int.Parse(item.Asistente),
                    Fecha = DateTime.Now,
                    Campolibre = ""
                };
                _session.SaveOrUpdate(doc);

                int idDocument = GetDocumentInfo(doc).FirstOrDefault().Id;
                
                var tipo = GetTypeName(item.Tipo);
                var subTipo = GetSubTypeName(item.SubTipo.Value);

                string rutaFichero = path + item.Amec + "\\" + item.Expediente + "\\" + tipo + "\\" + "|" + GetFileName(item, initialVersion);
                rutaFichero = !string.IsNullOrEmpty(subTipo) ? rutaFichero.Replace("|", subTipo + '\\') : rutaFichero.Replace("|", "");

                GestordocumentalDocumentoversion docVersion = new GestordocumentalDocumentoversion()
                {
                    Nombreoriginal = item.Fichero.ToString().Split('\\').Last().Replace(' ', '_'),
                    Rutafichero = rutaFichero,
                    Version = 0,
                    Idpeticionario = item.Peticionario,
                    Fecha = DateTime.Now,
                    Metadata = "#~#~#~#~#~#~#~#~#~#~#",
                    Documentonovalido = false,
                    Iddocumento = idDocument
                };
                _session.SaveOrUpdate(docVersion);
                return true;
            }
            catch (Exception ex)
            {
                //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                Quodem.Monitor.Alerta.SendNotificacion("Error en GestorDocumentalService.InsertFirstVersion: " + ex.Message);
                return false;
            }
        }

        public bool InsertNewVersion(DDocumentUpload item, string path)
        {
            try
            {
                var searchedDocVersion = GetLastVersion(item);
                var tipo = GetTypeName(item.Tipo);
                var subTipo = GetSubTypeName(item.SubTipo.Value);
                
                if (searchedDocVersion != null)
                {
                    string rutaFichero = path + item.Amec + "\\" + item.Expediente + "\\" + tipo + "\\" + "|" + GetFileName(item, searchedDocVersion.Version + 1);
                    rutaFichero = !string.IsNullOrEmpty(subTipo) ? rutaFichero.Replace("|", subTipo + '\\') : rutaFichero.Replace("|", "");

                    GestordocumentalDocumentoversion docVersion = new GestordocumentalDocumentoversion()
                    {
                        Nombreoriginal = searchedDocVersion.Nombreoriginal.Replace(' ', '_'),
                        Rutafichero = rutaFichero,
                        Version = searchedDocVersion.Version + 1,
                        Idpeticionario = item.Peticionario,
                        Fecha = DateTime.Now,
                        Metadata = "",
                        Documentonovalido = false,
                        Iddocumento = searchedDocVersion.Iddocumento
                    };
                    _session.SaveOrUpdate(docVersion);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                Quodem.Monitor.Alerta.SendNotificacion("Error en GestorDocumentalService.InsertNewVersion: " + ex.Message);
                return false;
            }
        }

        public GestordocumentalDocumento GetDocumentInstance()
        {
            return new GestordocumentalDocumento();
        }

        public List<GestordocumentalDocumentoversion> GetAllDocVersionInfo()
        {
            return GetDocVersionInfo(new GestordocumentalDocumentoversion()
            {
                Id = -1,
                Idpeticionario = -1,
                Iddocumento = -1,
                Nombreoriginal = "",
                Rutafichero = "",
                Version = -1,

            });
        }

        public List<GestordocumentalDocumentoversion> GetDocVersionInfo(GestordocumentalDocumentoversion filter)
        {
            var query = _session.Query<GestordocumentalDocumentoversion>();
            if (filter.Id != Variables.NULL_INT && filter.Id != 0)
                query = query.Where(x => x.Id == filter.Id);
            if (filter.Iddocumento != Variables.NULL_INT && filter.Iddocumento != 0)
                query = query.Where(x => x.Iddocumento == filter.Iddocumento);
            if (!string.IsNullOrEmpty(filter.Nombreoriginal))
                query = query.Where(x => x.Nombreoriginal.Contains(filter.Nombreoriginal));
            if (!string.IsNullOrEmpty(filter.Rutafichero))
                query = query.Where(x => x.Rutafichero == filter.Rutafichero);
            if (filter.Idpeticionario != Variables.NULL_INT && filter.Idpeticionario != 0 && filter.Idpeticionario != null)
                query = query.Where(x => x.Idpeticionario == filter.Idpeticionario);

            if (filter.GestordocumentalDocumento != null && !String.IsNullOrWhiteSpace(filter.GestordocumentalDocumento.Idamec))
                query = query.Where(x => x.GestordocumentalDocumento.Idamec == filter.GestordocumentalDocumento.Idamec);
            if (filter.GestordocumentalDocumento != null && filter.GestordocumentalDocumento.Idexpediente != Variables.NULL_INT)
                query = query.Where(x => x.GestordocumentalDocumento.Idexpediente == filter.GestordocumentalDocumento.Idexpediente);
            if (filter.GestordocumentalDocumento != null && filter.GestordocumentalDocumento.Idtipodoc != Variables.NULL_INT)
                query = query.Where(x => x.GestordocumentalDocumento.Idtipodoc == filter.GestordocumentalDocumento.Idtipodoc);
            if (filter.GestordocumentalDocumento != null && filter.GestordocumentalDocumento.Idsubtipodoc != Variables.NULL_INT)
                query = query.Where(x => x.GestordocumentalDocumento.Idsubtipodoc == filter.GestordocumentalDocumento.Idsubtipodoc);
            if (filter.GestordocumentalDocumento != null && filter.GestordocumentalDocumento.Idpassengerlist != Variables.NULL_INT)
                query = query.Where(x => x.GestordocumentalDocumento.Idpassengerlist == filter.GestordocumentalDocumento.Idpassengerlist);

            return query.ToList();
        }

        public string GetFileName(DDocumentUpload item, int version)
        {
            string passengerResult = "COMUNES";

            if (item.Asistente != "-1")
            {
                var passenger = _session.Query<PassengersList>().FirstOrDefault(x => x.Idpassengerlist == int.Parse(item.Asistente));
                passengerResult = passenger.Nombre + passenger.Apel1 + passenger.Apel2;
            }
            var tipo = GetTypeName(item.Tipo);
            var subTipo = GetSubTypeName(item.SubTipo.Value);

            string result = "";
            result += item.Amec + "-";
            result += item.Expediente + "-";
            result += passengerResult.Trim().Replace(' ', '_') + "-";
            result += tipo.Replace(' ', '_') + "-";
            result += subTipo.Replace(' ', '_') + "-";
            result += item.NombreAdicional != "" ? item.NombreAdicional + "-" : "";
            result += DateTime.Now.ToString("yyyyMMddHHmmss") + "-";
            result += "V." + version + "-";
            result += string.IsNullOrEmpty(item.DocumentoOriginal) ? item.Fichero.ToString().Split('\\').Last().Replace(' ', '_') : item.DocumentoOriginal.Replace(' ', '_');
            return result;
        }

        public string GetTypeName(int id)
        {
            return _session.Query<GestordocumentalTipodoc>().FirstOrDefault(x => x.Id == id).Nombre.Replace(' ', '_');
        }

        public string GetSubTypeName(int id)
        {
            if (id == -1) return "";
            return _session.Query<GestordocumentalSubtipodoc>().FirstOrDefault(x => x.Id == id).Nombre.Replace(' ', '_');
        }

        public DDocumentUpload ObtenerObjetoDocumento(string[] info)
        {
            string original = info[7].Replace("\"", "").Replace("\\", "").Replace(' ', '_');
            DDocumentUpload doc = new DDocumentUpload
            {
                DocumentoOriginal = string.IsNullOrEmpty(original)
                    ? info[0].Replace("\"", "").Replace("\\", "").Replace(' ', '_')
                    : original.Replace(' ', '_'),
                Tipo = int.Parse(info[1]),
                SubTipo = int.Parse(info[2]),
                Asistente = info[3],
                Peticionario = int.Parse(info[4]),
                Expediente = info[5],
                Amec = info[6],
                Fichero = string.IsNullOrEmpty(original)
                    ? info[0].Replace("\"", "").Replace("\\", "").Replace(' ', '_')
                    : original.Replace(' ', '_')
            };
            return doc;
        }

        public string GetLatestVersionFullName(DDocumentUpload item)
        {
            return GetLastVersion(item).Rutafichero;
        }

        public bool SetMetadata(DDocumentUpload item)
        {
            GestordocumentalDocumentoversion docVer = GetLastVersion(item);
            string fileName = docVer.Rutafichero.Split('\\').Last();
            string path = docVer.Rutafichero.Replace(fileName, "");
            string[] files = Directory.GetFiles(path, fileName);
            ShellPropertyCollection collection = new ShellPropertyCollection(files[0]);
            List<KeyValuePair<string, string>> properties = new List<KeyValuePair<string, string>>();
            foreach (var prop in collection)
            {
                string canName = prop.CanonicalName;
                string value = "";
                try
                {
                    if (prop.ValueAsObject != null)
                    {
                        value = prop.ValueAsObject.ToString();
                        properties.Add(new KeyValuePair<string, string>(canName, value));
                    }
                }
                catch (Exception ex)
                {

                }

            }

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    var jsonSerialiser = new JavaScriptSerializer();
                    docVer.Metadata = jsonSerialiser.Serialize(properties);
                    _session.SaveOrUpdate(docVer);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }

            return true;
        }

        public List<GestordocumentalDocumento> GetDocumentFilteredByNameList(DDocumentUpload item)
        {
            int? idSubTipo = item.SubTipo == -1 ? null : item.SubTipo;
            if (item.IdAmecDocCategory != null)
            {
                idSubTipo = _session.Query<GestordocumentalSubtipodoc>().FirstOrDefault(x => x.Idcategoriadocumento == item.IdAmecDocCategory).Id;
            }

            var filter = new GestordocumentalDocumento()
            {
                Idamec = item.Amec,
                Idexpediente = !string.IsNullOrWhiteSpace(item.Expediente) ? int.Parse(item.Expediente) : -1,
                Idtipodoc = item.Tipo,
                Idsubtipodoc = idSubTipo,
                Idpeticionario = item.Peticionario,
                Idpassengerlist = item.Asistente == "COMUN" || item.Asistente == "-1"? 0 : int.Parse(item.Asistente),
            };

            string filename = string.IsNullOrEmpty(item.DocumentoOriginal)
                ? item.Fichero.ToString().Split('\\').Last().Replace(' ', '_')
                : item.DocumentoOriginal.Replace(' ', '_');

            var query = _session.Query<GestordocumentalDocumentoversion>()
             .Join(_session.Query<GestordocumentalDocumento>(),
                 docV => new { IdDoc = docV.Iddocumento },
                 doc => new { IdDoc = doc.Id }, (docV, doc) => new { doc, docV.Nombreoriginal })
             .Where(x => x.Nombreoriginal == filename);

            if (!String.IsNullOrWhiteSpace(filter.Idamec))
                query = query.Where(x => x.doc.Idamec == filter.Idamec);
            if (filter.Idexpediente != Variables.NULL_INT && filter.Idexpediente != 0)
                query = query.Where(x => x.doc.Idexpediente == filter.Idexpediente);
            if (filter.Id != Variables.NULL_INT && filter.Id != 0)
                query = query.Where(x => x.doc.Id == filter.Id);
            if (filter.Idtipodoc != Variables.NULL_INT && filter.Idtipodoc != 0)
                query = query.Where(x => x.doc.Idtipodoc == filter.Idtipodoc);
            if (filter.Idsubtipodoc != Variables.NULL_INT && filter.Idsubtipodoc != 0 && filter.Idsubtipodoc != null)
                query = query.Where(x => x.doc.Idsubtipodoc == filter.Idsubtipodoc);
            if (filter.Idpassengerlist != Variables.NULL_INT && filter.Idpassengerlist != 0 && filter.Idpassengerlist != null)
                query = query.Where(x => x.doc.Idpassengerlist == filter.Idpassengerlist);
            if (filter.Idpeticionario != Variables.NULL_INT && filter.Idpeticionario != 0 && filter.Idpeticionario != null)
                query = query.Where(x => x.doc.Idpeticionario == filter.Idpeticionario);

            return query.Select(x => x.doc).ToList();
        }

        public GestordocumentalDocumentoversion GetLastVersion(DDocumentUpload item)
        {
            var query = GetDocumentFilteredByNameList(item);

            if (query.Count == 0) return null;

            GestordocumentalDocumentoversion filterVer = new GestordocumentalDocumentoversion()
            {
                Iddocumento = query.FirstOrDefault().Id,
                Nombreoriginal = string.IsNullOrEmpty(item.DocumentoOriginal) ? item.Fichero.ToString().Split('\\').Last().Replace(' ','_') : item.DocumentoOriginal.Replace(' ', '_'),
            };

            var docVersionList = GetDocVersionInfo(filterVer);
            if (docVersionList.Count == 0) return null;
            
            var maxVersion = docVersionList.Max(x => x.Version);
            return docVersionList.FirstOrDefault(x => x.Version == maxVersion);
        }

        public string GetIdamecs(string idamec)
        {
            return _session.Query<Amec>().FirstOrDefault(x => x.Idamec == int.Parse(idamec)).Amec1;
        }
        #endregion

        #region DocumentacionAmecs
        public bool InsertDocAmecs(DDocumentUpload item, string file)
        {
            try
            {
                int? idSubTipo = null;
                if (item.IdAmecDocCategory != null)
                {
                    idSubTipo =_session.Query<GestordocumentalSubtipodoc>().FirstOrDefault(x => x.Idcategoriadocumento == item.IdAmecDocCategory).Id;
                }

                GestordocumentalDocumento doc = new GestordocumentalDocumento()
                {
                    Idamec = item.Amec,
                    Idexpediente = !string.IsNullOrWhiteSpace(item.Expediente) ? int.Parse(item.Expediente) : new int?(),
                    Idtipodoc = item.Tipo,
                    Idsubtipodoc = idSubTipo,
                    Idpeticionario = item.Peticionario,
                    Idpassengerlist = item.Asistente == "-1" ? new int?() : int.Parse(item.Asistente),
                    Fecha = DateTime.Now,
                    Campolibre = ""
                };
                _session.SaveOrUpdate(doc);
                int idDocument = GetDocumentInfo(doc).FirstOrDefault().Id;
                GestordocumentalDocumentoversion docVersion = new GestordocumentalDocumentoversion()
                {
                    Nombreoriginal = item.DocumentoOriginal,
                    Rutafichero = file,
                    Version = 0,
                    Idpeticionario = item.Peticionario,
                    Fecha = DateTime.Now,
                    Metadata = "#~#~#~#~#~#~#~#~#~#~#",
                    Documentonovalido = false,
                    Iddocumento = idDocument
                };
                _session.SaveOrUpdate(docVersion);
                return true;
            }
            catch (Exception ex)
            {
                //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                Quodem.Monitor.Alerta.SendNotificacion("Error en GestorDocumentalService.InsertDocAmecs: " + ex.Message);
                return false;
            }
        }
        #endregion
    }
}
