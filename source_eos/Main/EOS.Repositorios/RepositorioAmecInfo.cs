using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS;
using System.Data.SqlClient;
using System.Runtime.InteropServices.ComTypes;
using EOS.ServiceLogic.BLL.GestorDocumental;
using NHibernate;

namespace EOS.Repositorios
{
    public class RepositorioAmecInfo : IRepositorioAmecInfo
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

        #region Constructor

        public RepositorioAmecInfo()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(EOS.ServiceLogic.Enums.EDbConnection.Default.GetHashCode());
        }

        #endregion

        public DAmecInfo NuevoAmec(DAmecInfo amec)
        {
            string consulta = string.Empty;
            //funcion para insertar el nuevo amec
            //Obtener numero amec.
            string IdamecSiguiente = amec.idamecs;

            //int IdamecSiguiente = 25;


            string fechacomienzo = null;
            string fechafinalizacion = null;
            string participantesmsd = null;
            string duracionhoras = null;
            string ponentespatrocinados = null;
            string profesionalessanitarios = "null";
            string medicosfichero = "0";
            string paraguas = "0";
            string politicaN20 = "0";
            string casosclinicos = "0";
            string farmaindustria = "0";
            string preaprobadaamed = "0";
            string iddepartament = "null";
            string iddistrict = "null";
            string idsaleforce = "null";
            string preaprobadaneg = "0";
            string preaprobadaleg = "0";
            string confempresa = "null"; 
            string cartascontrato = null;

            if (amec.fechacomienzo != null) { fechacomienzo = "'" + String.Format("{0:yyyyMMdd}", amec.fechacomienzo) + "'"; } else fechacomienzo = "null";
            if (amec.fechafinalizacion != null) { fechafinalizacion = "'" + String.Format("{0:yyyyMMdd}", amec.fechafinalizacion) + "'"; } else fechafinalizacion = "null";


            if (amec.participantesmsd != null) { participantesmsd = amec.participantesmsd.ToString(); } else participantesmsd = "null";
            if (amec.profesionalessanitarios > 0) profesionalessanitarios = amec.profesionalessanitarios.ToString();
            if (amec.medicosfichero.HasValue && amec.medicosfichero.Value) medicosfichero = "1";
            if (amec.politicaN20.HasValue && amec.politicaN20.Value) politicaN20 = "1";
            if (amec.farmaindustria.HasValue && amec.farmaindustria.Value) farmaindustria = "1";
            if (amec.casosclinicos.HasValue && amec.casosclinicos.Value) casosclinicos = "1";
            if (amec.paraguas.HasValue && amec.paraguas.Value) paraguas = "1";
            if (amec.preaprobadaleg.HasValue && amec.preaprobadaleg.Value) preaprobadaleg = "1";
            if (amec.preaprobadaamed.HasValue && amec.preaprobadaamed.Value) preaprobadaamed = "1";
            if (amec.preaprobadaneg.HasValue && amec.preaprobadaneg.Value) preaprobadaneg = "1";
            if (amec.duracionhoras != null) { duracionhoras = amec.duracionhoras.ToString(); } else duracionhoras = "null";
            if (amec.ponentespatrocinados != null) { ponentespatrocinados = amec.ponentespatrocinados.ToString(); } else ponentespatrocinados = "null";
            if (amec.importegasto == null) amec.importegasto = 0;
            if (amec.cartascontrato != null) { cartascontrato = amec.cartascontrato.ToString(); } else cartascontrato = "null";
            if (amec.descripcionobjetivo != null) amec.descripcionobjetivo = amec.descripcionobjetivo.Replace("'", "´");
            if (amec.conceptogastos != null) amec.conceptogastos = amec.conceptogastos.Replace("'", "´");
            if (amec.descripcion != null) amec.descripcion = amec.descripcion.Replace("'", "´");
            if (amec.urlprograma != null) amec.urlprograma = amec.urlprograma.Replace("'", "´");
            if (amec.lugarsede != null) amec.lugarsede = amec.lugarsede.Replace("'", "´");
            if (amec.idconfempresa > 0) confempresa = amec.idconfempresa.ToString();
            string amecposition = "null";
            if (amec.idposition.HasValue && amec.idposition > 0)
                amecposition = amec.idposition.Value.ToString();
            string ameccargo = "null";
            if (amec.idcargo.HasValue && amec.idcargo > 0)
                ameccargo = amec.idcargo.Value.ToString();

            consulta = "INSERT INTO amecs (idamecs, idestado, idsolicitante, idcargo, idposition, idcreadopor, fechaamecs, nwein,idtipoactividad, preaprobadaamed, preaprobadaneg, preaprobadaleg, farmaindustria, casosclinicos, politicaN20, participantesmsd, detallecriterios, medicosfichero, duracionhoras, ponentespatrocinados,conceptogastos, cargoadaxas,importegasto, cartascontrato,fechacomienzo,fechafinalizacion, lugarsede, idcriterioseleccion,criterioespecificado, programaamecs, urlprograma, descripcionobjetivo, descripcion, paraguas, profesionalessanitarios,idconfempresa,fechaultimaactualizacion) " +
                       "VALUES('" + IdamecSiguiente + "',5," + amec.idsolicitante + "," + ameccargo + "," + amecposition + "," + amec.idcreadopor + ", GETDATE(), '" + amec.nwein + "'," + amec.idtipoactividad + "," + preaprobadaamed + "," + preaprobadaneg + "," + preaprobadaleg + "," + farmaindustria + "," + casosclinicos + "," + politicaN20 + "," + participantesmsd + ",'" + amec.detallecriterios.ToString() + "'," + medicosfichero + "," + duracionhoras + "," + ponentespatrocinados + ",'" + amec.conceptogastos + "','" + amec.cargoadaxas + "'," + amec.importegasto.ToString().Replace(",", ".") + "," + cartascontrato + "," + fechacomienzo + "," + fechafinalizacion + ",'" + amec.lugarsede + "'," + amec.idcriterioseleccion + ",'" + amec.criterioespecificado + "','" + amec.programaamecs + "','" + amec.urlprograma + "','" + amec.descripcionobjetivo + "','" + amec.descripcion + "'," + paraguas + "," + profesionalessanitarios + "," + confempresa + ",GETDATE())";

             int resultado = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);


            string query = string.Format("SELECT *, rtrim(ltrim(isnull(Nombre, '') + ' ' + isnull(Apellido1, ''))) as NombreCompleto FROM peticionarios WHERE peticionarios.IdPeticionario = {0}", amec.idcreadopor);
            DDatosPersonalesUsuario dPeticionario = DDatosPersonalesUsuario.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query)).FirstOrDefault();
            if (dPeticionario.iddepartament > 0) iddepartament = dPeticionario.iddepartament.ToString();
            if (dPeticionario.iddistrict > 0) iddistrict = dPeticionario.iddistrict.ToString();
            if (dPeticionario.idsaleforce > 0) idsaleforce = dPeticionario.idsaleforce.ToString();
            consulta = "INSERT INTO histasocamecs (idamecs, idestado, idnivelaprobacion, comentariosaprob, idcreadopor, fechacreacion,iddistrict,iddepartament,idsaleforce)  VALUES('" + IdamecSiguiente + "'," + amec.idestado + "," + "1" + "," + "' '" + "," + amec.idcreadopor + ", CURRENT_TIMESTAMP," + iddistrict + ","+ iddepartament+ ","+ idsaleforce + ")";
            int resultadohist = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            consulta = "SELECT * FROM amecs where idamecs='" + IdamecSiguiente + "'";
            DataTable dt = Quodem.Sql.SqlServerClient.GetQuery(consulta);
            return DAmecInfo.ConvertToDto(dt).ElementAt<DAmecInfo>(0);
        }

        public DAmecInfo NuevoAmecNewCo(DAmecInfo amec)
        {
            string consulta = string.Empty;
            //funcion para insertar el nuevo amec
            //Obtener numero amec.
            string IdamecSiguiente = amec.idamecs;

            //int IdamecSiguiente = 25;


            string fechacomienzo = null;
            string fechafinalizacion = null;
            string participantesmsd = null;
            string duracionhoras = null;
            string ponentespatrocinados = null;
            string profesionalessanitarios = "null";
            string medicosfichero = "0";
            string paraguas = "0";
            string politicaN20 = "0";
            string casosclinicos = "0";
            string farmaindustria = "0";
            string preaprobadaamed = "0";
            string iddepartament = "null";
            string iddistrict = "null";
            string idsaleforce = "null";
            string preaprobadaneg = "0";
            string preaprobadaleg = "0";
            string confempresa = "null";
            string cartascontrato = null;
            string idcriterioseleccion = "null";

            if (amec.idcriterioseleccion > 0) idcriterioseleccion = amec.idcriterioseleccion.ToString();
            if (amec.fechacomienzo != null) { fechacomienzo = "'" + String.Format("{0:yyyyMMdd}", amec.fechacomienzo) + "'"; } else fechacomienzo = "null";
            if (amec.fechafinalizacion != null) { fechafinalizacion = "'" + String.Format("{0:yyyyMMdd}", amec.fechafinalizacion) + "'"; } else fechafinalizacion = "null";


            if (amec.participantesmsd != null) { participantesmsd = amec.participantesmsd.ToString(); } else participantesmsd = "null";
            if (amec.profesionalessanitarios > 0) profesionalessanitarios = amec.profesionalessanitarios.ToString();
            if (amec.medicosfichero.HasValue && amec.medicosfichero.Value) medicosfichero = "1";
            if (amec.politicaN20.HasValue && amec.politicaN20.Value) politicaN20 = "1";
            if (amec.farmaindustria.HasValue && amec.farmaindustria.Value) farmaindustria = "1";
            if (amec.casosclinicos.HasValue && amec.casosclinicos.Value) casosclinicos = "1";
            if (amec.paraguas.HasValue && amec.paraguas.Value) paraguas = "1";
            if (amec.preaprobadaleg.HasValue && amec.preaprobadaleg.Value) preaprobadaleg = "1";
            if (amec.preaprobadaamed.HasValue && amec.preaprobadaamed.Value) preaprobadaamed = "1";
            if (amec.preaprobadaneg.HasValue && amec.preaprobadaneg.Value) preaprobadaneg = "1";
            if (amec.duracionhoras != null) { duracionhoras = amec.duracionhoras.ToString(); } else duracionhoras = "null";
            if (amec.ponentespatrocinados != null) { ponentespatrocinados = amec.ponentespatrocinados.ToString(); } else ponentespatrocinados = "null";
            if (amec.importegasto == null) amec.importegasto = 0;
            if (amec.cartascontrato != null) { cartascontrato = amec.cartascontrato.ToString(); } else cartascontrato = "null";
            if (amec.descripcionobjetivo != null) amec.descripcionobjetivo = amec.descripcionobjetivo.Replace("'", "´");
            if (amec.conceptogastos != null) amec.conceptogastos = amec.conceptogastos.Replace("'", "´");
            if (amec.descripcion != null) amec.descripcion = amec.descripcion.Replace("'", "´");
            if (amec.urlprograma != null) amec.urlprograma = amec.urlprograma.Replace("'", "´");
            if (amec.lugarsede != null) amec.lugarsede = amec.lugarsede.Replace("'", "´");
            if (amec.idconfempresa > 0) confempresa = amec.idconfempresa.ToString();
            string amecposition = "null";
            if (amec.idposition.HasValue && amec.idposition > 0)
                amecposition = amec.idposition.Value.ToString();
            string ameccargo = "null";
            if (amec.idcargo.HasValue && amec.idcargo > 0)
                ameccargo = amec.idcargo.Value.ToString();

            consulta = "INSERT INTO amecs (idamecs, idestado, idsolicitante, idcargo, idposition, idcreadopor, fechaamecs, nwein,idtipoactividad, preaprobadaamed, preaprobadaneg, preaprobadaleg, farmaindustria, casosclinicos, politicaN20, participantesmsd, medicosfichero, duracionhoras, ponentespatrocinados,importegasto, cartascontrato,fechacomienzo,fechafinalizacion, idcriterioseleccion, descripcion, paraguas, profesionalessanitarios,idconfempresa,fechaultimaactualizacion,veeva, newco) " +
                       "VALUES('" + IdamecSiguiente + "',5," + amec.idsolicitante + "," + ameccargo + "," + amecposition + "," + amec.idcreadopor + ", GETDATE(), '" + amec.nwein + "'," + amec.idtipoactividad + "," + preaprobadaamed + "," + preaprobadaneg + "," + preaprobadaleg + "," + farmaindustria + "," + casosclinicos + "," + politicaN20 + "," + participantesmsd + "," + medicosfichero + "," + duracionhoras + "," + ponentespatrocinados + "," + amec.importegasto.ToString().Replace(",", ".") + "," + cartascontrato + "," + fechacomienzo + "," + fechafinalizacion + "," + idcriterioseleccion + ",'" + amec.descripcion + "'," + paraguas + "," + profesionalessanitarios + "," + confempresa + ",GETDATE(), 0, 1)";

            int resultado = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);


            string query = string.Format("SELECT *, rtrim(ltrim(isnull(Nombre, '') + ' ' + isnull(Apellido1, ''))) as NombreCompleto FROM peticionarios WHERE peticionarios.IdPeticionario = {0}", amec.idcreadopor);
            DDatosPersonalesUsuario dPeticionario = DDatosPersonalesUsuario.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query)).FirstOrDefault();
            if (dPeticionario.iddepartament > 0) iddepartament = dPeticionario.iddepartament.ToString();
            if (dPeticionario.iddistrict > 0) iddistrict = dPeticionario.iddistrict.ToString();
            if (dPeticionario.idsaleforce > 0) idsaleforce = dPeticionario.idsaleforce.ToString();
            consulta = "INSERT INTO histasocamecs (idamecs, idestado, idnivelaprobacion, comentariosaprob, idcreadopor, fechacreacion,iddistrict,iddepartament,idsaleforce)  VALUES('" + IdamecSiguiente + "'," + amec.idestado + "," + "1" + "," + "' '" + "," + amec.idcreadopor + ", CURRENT_TIMESTAMP," + iddistrict + "," + iddepartament + "," + idsaleforce + ")";
            int resultadohist = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            consulta = "SELECT * FROM amecs where idamecs='" + IdamecSiguiente + "'";
            DataTable dt = Quodem.Sql.SqlServerClient.GetQuery(consulta);
            return DAmecInfo.ConvertToDto(dt).ElementAt<DAmecInfo>(0);
        }

        public DAmecInfo ClonarAmec(DAmecInfo amec, string idAmecOriginal)
        {
            string consulta = string.Empty;
            //funcion para insertar el nuevo amec
            //Obtener numero amec.
            string IdamecSiguiente = amec.idamecs;

            //int IdamecSiguiente = 25;


            string fechacomienzo = null;
            string fechafinalizacion = null;
            string participantesmsd = null;
            string duracionhoras = null;
            string ponentespatrocinados = null;
            string profesionalessanitarios = "null";
            string medicosfichero = "0";
            string paraguas = "0";
            string politicaN20 = "0";
            string casosclinicos = "0";
            string farmaindustria = "0";
            string preaprobadaamed = "0";
            string iddepartament = "null";
            string iddistrict = "null";
            string idsaleforce = "null";
            string preaprobadaneg = "0";
            string preaprobadaleg = "0";
            string confempresa = "null";
            string cartascontrato = null;

            if (amec.fechacomienzo != null) { fechacomienzo = "'" + String.Format("{0:yyyyMMdd}", amec.fechacomienzo) + "'"; } else fechacomienzo = "null";
            if (amec.fechafinalizacion != null) { fechafinalizacion = "'" + String.Format("{0:yyyyMMdd}", amec.fechafinalizacion) + "'"; } else fechafinalizacion = "null";


            if (amec.participantesmsd != null) { participantesmsd = amec.participantesmsd.ToString(); } else participantesmsd = "null";
            if (amec.profesionalessanitarios > 0) profesionalessanitarios = amec.profesionalessanitarios.ToString();
            if (amec.medicosfichero.HasValue && amec.medicosfichero.Value) medicosfichero = "1";
            if (amec.politicaN20.HasValue && amec.politicaN20.Value) politicaN20 = "1";
            if (amec.farmaindustria.HasValue && amec.farmaindustria.Value) farmaindustria = "1";
            if (amec.casosclinicos.HasValue && amec.casosclinicos.Value) casosclinicos = "1";
            if (amec.paraguas.HasValue && amec.paraguas.Value) paraguas = "1";
            if (amec.preaprobadaleg.HasValue && amec.preaprobadaleg.Value) preaprobadaleg = "1";
            if (amec.preaprobadaamed.HasValue && amec.preaprobadaamed.Value) preaprobadaamed = "1";
            if (amec.preaprobadaneg.HasValue && amec.preaprobadaneg.Value) preaprobadaneg = "1";
            if (amec.duracionhoras != null) { duracionhoras = amec.duracionhoras.ToString(); } else duracionhoras = "null";
            if (amec.ponentespatrocinados != null) { ponentespatrocinados = amec.ponentespatrocinados.ToString(); } else ponentespatrocinados = "null";
            if (amec.importegasto == null) amec.importegasto = 0;
            if (amec.cartascontrato != null) { cartascontrato = amec.cartascontrato.ToString(); } else cartascontrato = "null";
            if (amec.descripcionobjetivo != null) amec.descripcionobjetivo = amec.descripcionobjetivo.Replace("'", "´");
            if (amec.conceptogastos != null) amec.conceptogastos = amec.conceptogastos.Replace("'", "´");
            if (amec.descripcion != null) amec.descripcion = amec.descripcion.Replace("'", "´");
            if (amec.urlprograma != null) amec.urlprograma = amec.urlprograma.Replace("'", "´");
            if (amec.lugarsede != null) amec.lugarsede = amec.lugarsede.Replace("'", "´");
            if (amec.idconfempresa > 0) confempresa = amec.idconfempresa.ToString();
            string amecposition = "null";
            if (amec.idposition.HasValue && amec.idposition > 0)
                amecposition = amec.idposition.Value.ToString();
            string ameccargo = "null";
            if (amec.idcargo.HasValue && amec.idcargo > 0)
                ameccargo = amec.idcargo.Value.ToString();

            consulta = "INSERT INTO amecs (idamecs, idestado, idsolicitante, idcargo, idposition, idcreadopor, fechaamecs, nwein,idtipoactividad, preaprobadaamed, preaprobadaneg, preaprobadaleg, farmaindustria, casosclinicos, politicaN20, participantesmsd, detallecriterios, medicosfichero, duracionhoras, ponentespatrocinados,conceptogastos, cargoadaxas,importegasto, cartascontrato,fechacomienzo,fechafinalizacion, lugarsede, idcriterioseleccion,criterioespecificado, programaamecs, urlprograma, descripcionobjetivo, descripcion, paraguas, profesionalessanitarios,idconfempresa,fechaultimaactualizacion,idamecsrelacionado) " +
                       "VALUES('" + IdamecSiguiente + "',5," + amec.idsolicitante + "," + ameccargo + "," + amecposition + "," + amec.idcreadopor + ", GETDATE(), '" + amec.nwein + "'," + amec.idtipoactividad + "," + preaprobadaamed + "," + preaprobadaneg + "," + preaprobadaleg + "," + farmaindustria + "," + casosclinicos + "," + politicaN20 + "," + participantesmsd + ",'" + amec.detallecriterios.ToString() + "'," + medicosfichero + "," + duracionhoras + "," + ponentespatrocinados + ",'" + amec.conceptogastos + "','" + amec.cargoadaxas + "'," + amec.importegasto.ToString().Replace(",", ".") + "," + cartascontrato + "," + fechacomienzo + "," + fechafinalizacion + ",'" + amec.lugarsede + "'," + amec.idcriterioseleccion + ",'" + amec.criterioespecificado + "','" + amec.programaamecs + "','" + amec.urlprograma + "','" + amec.descripcionobjetivo + "','" + amec.descripcion + "'," + paraguas + "," + profesionalessanitarios + "," + confempresa + ",GETDATE(), " + idAmecOriginal + ")";

            int resultado = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);


            string query = string.Format("SELECT *, rtrim(ltrim(isnull(Nombre, '') + ' ' + isnull(Apellido1, ''))) as NombreCompleto FROM peticionarios WHERE peticionarios.IdPeticionario = {0}", amec.idcreadopor);
            DDatosPersonalesUsuario dPeticionario = DDatosPersonalesUsuario.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query)).FirstOrDefault();
            if (dPeticionario.iddepartament > 0) iddepartament = dPeticionario.iddepartament.ToString();
            if (dPeticionario.iddistrict > 0) iddistrict = dPeticionario.iddistrict.ToString();
            if (dPeticionario.idsaleforce > 0) idsaleforce = dPeticionario.idsaleforce.ToString();
            consulta = "INSERT INTO histasocamecs (idamecs, idestado, idnivelaprobacion, comentariosaprob, idcreadopor, fechacreacion,iddistrict,iddepartament,idsaleforce)  VALUES('" + IdamecSiguiente + "'," + amec.idestado + "," + "1" + "," + "' '" + "," + amec.idcreadopor + ", CURRENT_TIMESTAMP," + iddistrict + "," + iddepartament + "," + idsaleforce + ")";
            int resultadohist = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            consulta = string.Format("update amec set amec='{0}' where amec='{1}'", IdamecSiguiente, idAmecOriginal);
            int resultadoUpdateAmec = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            consulta = string.Format("update amecs set idamecsrelacionado='{0}' where idamecs='{1}'", IdamecSiguiente, idAmecOriginal);
            int resultadoUpdateAmecs = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            //Copia de toda la documentacion de la base de datos
            consulta = string.Format("insert into docasocamecs (idamecs, ubicaciondoc, tipodoc, nombredoc, comentariosdoc, adjuntaraemail, idcreadopor, fechacreacion, idcategoriadocumento) select '{0}', ubicaciondoc, tipodoc, nombredoc, comentariosdoc, adjuntaraemail, idcreadopor, fechacreacion, idcategoriadocumento from docasocamecs where idamecs = '{1}' ", IdamecSiguiente, idAmecOriginal);
            int resultadoCopiaDocumentacion = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            //Copia de todos los comentarios de la base de datos
            consulta = string.Format("insert into comentasocamecs (idamecs, comentariosdoc, idcreadopor, fechacreacion) select '{0}', comentariosdoc, idcreadopor, fechacreacion from comentasocamecs where idamecs = '{1}' ", IdamecSiguiente, idAmecOriginal);
            int resultadoCopiaComentarios = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            //Copia del contenido del gestor documental
            GestorDocumentalServiceManager service = new GestorDocumentalServiceManager();
            service.CopyDocsAmec(idAmecOriginal, IdamecSiguiente);

            consulta = "SELECT * FROM amecs where idamecs='" + IdamecSiguiente + "'";
            DataTable dt = Quodem.Sql.SqlServerClient.GetQuery(consulta);
            return DAmecInfo.ConvertToDto(dt).ElementAt<DAmecInfo>(0);
        }

        public int CambiarEstadoAmec(string idestado, string idamecs, int nivelaprobacion = -1)
        {
            try
            {
                string consulta = string.Empty;
                consulta = nivelaprobacion < 0 ?
                      string.Format("update amecs set idestado = {0} where idamecs ='{1}'", idestado, idamecs)
                    : string.Format("update amecs set idestado = {0}, idnivelaprobacion = {1} where idamecs = '{2}' ", idestado, nivelaprobacion, idamecs);
                int resultado = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
                return resultado;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public int CambiarEstadoAmec(string idestado, string idamecs, string idpeticionario, string idunidad, string idarea, string idregion, string iddistrito)
        {
            string consulta = string.Empty;
            int resultadohist = 0;
            consulta = string.Format("update amecs set idestado = {0} where idamecs ='{1}'", idestado, idamecs);

            int resultado = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            if (resultado != 0)
            {
                string consulta2 = string.Format("insert into histasocamecs (idamecs,idestado,idnivelaprobacion,idcreadopor, fechacreacion,idunidad,idarea,idregion,idDistrito) values ('{0}', {1}, {2}, {3}, CURRENT_TIMESTAMP, {4}, {5}, {6}, {7})", idamecs, idestado, "1", idpeticionario, idunidad, idarea, idregion, iddistrito);
                resultadohist = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta2);
            }
            return resultadohist;
        }

        public int HayAmecRelacionExpediente(string idamec)
        {
            string consulta = string.Empty;
            consulta = string.Format("select count(*) from amec am inner join expediente ex on ex.idamec = am.idamec  where amec = '{0}'", idamec);
            long NumRelacion = long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            return Convert.ToInt32(NumRelacion);
        }

        public bool EsGestorArchivo(int nIdPeticionario)
        {
            string consulta = string.Empty;
            consulta = string.Format("select count(*) from peticionarios where idnivelaprobacion = 10 and idpeticionario = {0}", nIdPeticionario);
            long NumRelacion = long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            if (NumRelacion != 0) return true;
            else return false;
        }

        public int GuardaLogMail(string idamecs, string tipo_mail, string message_to, string message_subject, string message_body, string message_fileattach, bool envioCorrecto, string error)
        {
            string consulta = string.Empty;
            string envioOK = envioCorrecto ? "1" : "0";
            consulta = "INSERT INTO logmail_flujoamec (idamecs, tipo_mail, mail_to, mail_subject, mail_body, mail_fileattach, envio_correcto, error, fecha_envio) " +
                       "VALUES('" + idamecs + "','" + tipo_mail + "','" + message_to + "','" + message_subject + "','" + message_body + "','" + message_fileattach + "'," + envioOK + ",'" + error + "',CURRENT_TIMESTAMP)";
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        //No cambiar a string
        public bool TieneExpedientesAsociados(int idamec)
        {
            string consulta = string.Format("SELECT COUNT(1) FROM expediente where idamec ={0}", idamec);
            return int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta)) > 0;
        }

        public bool TieneExpedientesAsociadosPorIdAmecs(string idamecs)
        {
            string consulta = string.Format("SELECT count(*) FROM expediente ex inner join amec am on ex.idamec = am.idamec where am.amec='{0}'", idamecs);
            return int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta)) > 0;
        }

        public ICollection<ListadoAmecs> ObtenerListadoAMECsProcedure(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles, out int count)
        {
            try
            {

                string oldAmecs = string.Empty;
                string newAmecs = string.Empty;
                string newAmecsVeeva = string.Empty;
                string newAmecsNewCo = string.Empty;
                //Pendiente de aprobar por mí
                if (filtro.idestadoamec == 6)
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(" INSERT INTO #ResultAMEC ");
                    query.Append(" SELECT DISTINCT ams.idamecs, ");
                    query.Append("        Cast(Max(ams.preaprobadaneg) AS BIT)  AS preaprobadaneg, ");
                    query.Append("        Cast(Max(ams.preaprobadaleg) AS BIT)  AS preaprobadaleg, ");
                    query.Append("        Cast(Max(ams.farmaindustria) AS BIT)  AS farmaindustria, ");
                    query.Append("        Cast(Max(ams.casosclinicos) AS BIT)   AS casosclinicos, ");
                    query.Append("        Max(ams.participantesmsd)             AS participantesmsd, ");
                    query.Append("        Max(ams.detallecriterios)             AS detallecriterios, ");
                    query.Append("        Cast(Max(ams.medicosfichero) AS BIT)  AS medicosfichero, ");
                    query.Append("        Max(ams.duracionhoras)                AS duracionhoras, ");
                    query.Append("        Max(ams.ponentespatrocinados)         AS ponentespatrocinados, ");
                    query.Append("        Max(ams.conceptogastos)               AS conceptogastos, ");
                    query.Append("        Max(ams.importegasto)                 AS importegasto, ");
                    query.Append("        Max(ams.cartascontrato)               AS cartascontrato, ");
                    query.Append("        Max(ams.programaamecs)                AS programaamecs, ");
                    query.Append("        Max(ams.urlprograma)                  AS urlprograma, ");
                    query.Append("        Max(ams.descripcionobjetivo)          AS descripcionobjetivo, ");
                    query.Append("        Max(ams.descripcion)                  AS descripcion, ");
                    query.Append("        Max(ams.nwein)                        AS nwein, ");
                    query.Append("        Max(ams.cargoadaxas)                  AS cargoadaxas, ");
                    query.Append("        Cast(Max(ams.preaprobadaamed) AS BIT) AS preaprobadaamed, ");
                    query.Append("        Max(ams.idsolicitante)                AS idsolicitante, ");
                    query.Append("        Max(pet.idcargo)                      AS idcargo, ");
                    query.Append("        Max(ams.idcreadopor)                  AS idcreadopor,");
                    query.Append("        ams.fechaamecs                        AS fechaamecs, ");
                    query.Append("        Max(ams.idtipoactividad)              AS idtipoactividad, ");
                    query.Append("        Cast(Max(ams.paraguas) AS BIT)        AS paraguas, ");
                    query.Append("        Max(est.idestado)                     AS idestado, ");
                    query.Append("        Max(est.estado)                       AS estado, ");
                    query.Append("        Cast(Max(CASE ");
                    query.Append("                   WHEN Isnull(unid.idunidadamec, -1) < 0 THEN 1 ");
                    query.Append("                   ELSE 0 ");
                    query.Append("                 END) AS BIT)                 AS nuevosFlujosAprobacion," );
                    query.Append("        ams.veeva              AS veeva, ");
                    query.Append("        ams.newco              AS newco ");
                    query.Append(" FROM   amecs ams ");
                    query.Append("        INNER JOIN peticionarios pet ");
                    query.Append("                ON pet.idpeticionario = ams.idsolicitante ");
                    query.Append("        INNER JOIN estadoamecs est ");
                    query.Append("                ON est.idestado = ams.idestado ");
                    query.Append("        LEFT JOIN unidorganizamec unid ");
                    query.Append("               ON unid.idamecs = ams.idamecs ");

                    oldAmecs = string.Format(query + " {0}",CrearSeccionWhereFiltroListadoAmecProcedure(filtro, datosRoles, false, false));
                }
                else
                {

                    StringBuilder query = new StringBuilder();
                    query.Append(" INSERT INTO #ResultAMEC ");
                    query.Append(" SELECT DISTINCT ams.idamecs as idamecs, ");
                    query.Append("        Cast(Max(ams.preaprobadaneg) AS BIT)  AS preaprobadaneg, ");
                    query.Append("        Cast(Max(ams.preaprobadaleg) AS BIT)  AS preaprobadaleg, ");
                    query.Append("        Cast(Max(ams.farmaindustria) AS BIT)  AS farmaindustria, ");
                    query.Append("        Cast(Max(ams.casosclinicos) AS BIT)   AS casosclinicos, ");
                    query.Append("        Max(ams.participantesmsd)             AS participantesmsd, ");
                    query.Append("        Max(ams.detallecriterios)             AS detallecriterios, ");
                    query.Append("        Cast(Max(ams.medicosfichero) AS BIT)  AS medicosfichero, ");
                    query.Append("        Max(ams.duracionhoras)                AS duracionhoras, ");
                    query.Append("        Max(ams.ponentespatrocinados)         AS ponentespatrocinados, ");
                    query.Append("        Max(ams.conceptogastos)               AS conceptogastos, ");
                    query.Append("        Max(ams.importegasto)                 AS importegasto, ");
                    query.Append("        Max(ams.cartascontrato)               AS cartascontrato, ");
                    query.Append("        Max(ams.programaamecs)                AS programaamecs, ");
                    query.Append("        Max(ams.urlprograma)                  AS urlprograma, ");
                    query.Append("        Max(ams.descripcionobjetivo)          AS descripcionobjetivo, ");
                    query.Append("        Max(ams.descripcion)                  AS descripcion, ");
                    query.Append("        Max(ams.nwein)                        AS nwein, ");
                    query.Append("        Max(ams.cargoadaxas)                  AS cargoadaxas, ");
                    query.Append("        Cast(Max(ams.preaprobadaamed) AS BIT) AS preaprobadaamed, ");
                    query.Append("        Max(ams.idsolicitante)                AS idsolicitante, ");
                    query.Append("        Max(pet.idcargo)                      AS idcargo, ");
                    query.Append("        Max(ams.idcreadopor)                  AS idcreadopor,");
                    query.Append("        ams.fechaamecs                        AS fechaamecs, ");
                    query.Append("        Max(ams.idtipoactividad)              AS idtipoactividad, ");
                    query.Append("        Cast(Max(ams.paraguas) AS BIT)        AS paraguas, ");
                    query.Append("        Max(est.idestado)                     AS idestado, ");
                    query.Append("        Max(est.estado)                       AS estado, ");
                    query.Append("        Cast(Max(CASE ");
                    query.Append("                   WHEN Isnull(unid.idunidadamec, -1) < 0 THEN 1 ");
                    query.Append("                   ELSE 0 ");
                    query.Append("                 END) AS BIT)                 AS nuevosFlujosAprobacion," );
                    query.Append("        ams.veeva              AS veeva, ");
                    query.Append("        ams.newco              AS newco ");
                    query.Append(" FROM   amecs ams ");
                    query.Append("        INNER JOIN peticionarios pet ");
                    query.Append("                ON pet.idpeticionario = ams.idsolicitante ");
                    query.Append("        INNER JOIN estadoamecs est ");
                    query.Append("                ON est.idestado = ams.idestado ");
                    query.Append("        LEFT JOIN unidorganizamec unid ");
                    query.Append("               ON unid.idamecs = ams.idamecs ");


                    oldAmecs = string.Format(query + " {0}", CrearSeccionWhereFiltroListadoAmecProcedure(filtro, datosRoles, false, false));
                }
                newAmecs = ObtenerConsultaListadoAMECsNuevo(filtro, datosRoles);
                if ((!datosRoles.newco.HasValue || !datosRoles.newco.Value) || (datosRoles.administrador.HasValue && datosRoles.administrador.Value))
                {
                    newAmecsVeeva = ObtenerConsultaListadoAMECsVeeva(filtro, datosRoles);
                }
                if ((datosRoles.newco.HasValue && datosRoles.newco.Value) || (datosRoles.administrador.HasValue && datosRoles.administrador.Value)) 
                {
                    newAmecsNewCo = ObtenerConsultaListadoAMECsNewCo(filtro, datosRoles);
                }
                using (var transaccion = _session.BeginTransaction())
                {
                    if (filtro.MaximumRows != null && filtro.MaximumRows.Value != 0 && filtro.StartRowIndex != null && 
                        (filtro.idestadoamec != 6 || (filtro.idestadoamec == 6 &&
                        (datosRoles.medico.Value || datosRoles.legal.Value /*|| !datosRoles.administrador.Value*/))))
                    {
                        _session.CreateSQLQuery(newAmecs).ExecuteUpdate();
                        _session.CreateSQLQuery(oldAmecs).ExecuteUpdate();
                        if ((!datosRoles.newco.HasValue || !datosRoles.newco.Value) || (datosRoles.administrador.HasValue && datosRoles.administrador.Value))
                        {
                            _session.CreateSQLQuery(newAmecsVeeva).ExecuteUpdate();
                        }
                        if ((datosRoles.newco.HasValue && datosRoles.newco.Value) || (datosRoles.administrador.HasValue && datosRoles.administrador.Value))
                        {
                            _session.CreateSQLQuery(newAmecsNewCo).ExecuteUpdate();
                        }

                        string sorting = "IF OBJECT_ID('tempdb..#SortedResultAMEC') IS NOT NULL DROP TABLE #SortedResultAMEC; SELECT DISTINCT * INTO #SortedResultAMEC FROM #ResultAMEC ORDER BY 1 DESC";
                        _session.CreateSQLQuery(sorting).ExecuteUpdate();
                        string query = "SELECT * FROM #SortedResultAMEC order by fechaamecs desc ";

                        count = (int)_session.CreateSQLQuery("SELECT count(*) FROM #SortedResultAMEC").List()[0];

                        if (filtro.MaximumRows != null && filtro.MaximumRows.Value != 0 && filtro.StartRowIndex != null &&
                            (filtro.idestadoamec == 6))
                        {
                            var newAmecsList = _session.CreateSQLQuery(query).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ListadoAmecs))).List<ListadoAmecs>();
                            transaccion.Commit();
                            return newAmecsList;
                        }
                        else
                        {
                            var newAmecsList = _session.CreateSQLQuery(query).SetFirstResult(filtro.StartRowIndex.Value).SetMaxResults(filtro.MaximumRows.Value).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ListadoAmecs))).List<ListadoAmecs>();
                            transaccion.Commit();
                            return newAmecsList;
                        }
                    }
                    else
                    {
                        _session.CreateSQLQuery(newAmecs).ExecuteUpdate();
                        _session.CreateSQLQuery(oldAmecs).ExecuteUpdate();
                        if ((!datosRoles.newco.HasValue || !datosRoles.newco.Value) || (datosRoles.administrador.HasValue && datosRoles.administrador.Value))
                        {
                            _session.CreateSQLQuery(newAmecsVeeva).ExecuteUpdate();
                        }
                        if ((datosRoles.newco.HasValue && datosRoles.newco.Value) || (datosRoles.administrador.HasValue && datosRoles.administrador.Value))
                        {
                            _session.CreateSQLQuery(newAmecsNewCo).ExecuteUpdate();
                        }
                        string sorting = "IF OBJECT_ID('tempdb..#SortedResultAMEC') IS NOT NULL DROP TABLE #SortedResultAMEC; SELECT DISTINCT * INTO #SortedResultAMEC FROM #ResultAMEC ORDER BY 1 DESC";
                        _session.CreateSQLQuery(sorting).ExecuteUpdate();
                        string query = "SELECT * FROM #SortedResultAMEC  order by fechaamecs desc ";

                        count = (int)_session.CreateSQLQuery("SELECT count(*) FROM #SortedResultAMEC").List()[0];

                        var newAmecsList = _session.CreateSQLQuery(query)
                           .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ListadoAmecs))).List<ListadoAmecs>();
                        transaccion.Commit();
                        return newAmecsList;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<ListadoAmecs> CheckMeCorrespondeAprobarNegocio(List<ListadoAmecs> lista, DVPeticionariosRoles datosRoles)
        {
            List<ListadoAmecs> result = new List<ListadoAmecs>();
            RepositorioAprobadorAMEC repoAprob = new RepositorioAprobadorAMEC();
            

            foreach (var item in lista)
            {
                //if ((item.idamecs < 400000000 && item.idamecs > 500000000))
                if (!item.idamecs.StartsWith("4") && item.idamecs.Length == 9)
                {
                    result.Add(item);
                }
                else
                {
                    
                }
                //else
                //{
                //    List<int> aprobadores = repoAprob.ObernerIdsAprobadoresAmec(item.idamecs.ToString());
                //    if (aprobadores.Contains(datosRoles.IdPeticionario))
                //    {
                //        result.Add(item);
                //    }
                //}
            }

            return result;
        }

        public string ObtenerConsultaListadoAMECsNuevo(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles)
        {
            string consulta = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(" IF OBJECT_ID('tempdb..#ResultAMEC') IS NOT NULL DROP TABLE #ResultAMEC;");
            sb.Append(" IF OBJECT_ID('tempdb..#Temp') IS NOT NULL DROP TABLE #Temp;");
            sb.Append(" CREATE TABLE #Temp( idamecs varchar(20) PRIMARY KEY CLUSTERED);");
            sb.AppendFormat(" INSERT INTO #Temp EXEC sp_obtener_amecs_puedo_ver @idPeticionario = {0}, @isCreateMode = 0;", filtro.IdPeticionarioSession);
            sb.Append(" ");
            sb.Append(" SELECT DISTINCT ams.idamecs, ");
            sb.Append("        Cast(Max(ams.preaprobadaneg) AS BIT)  AS preaprobadaneg, ");
            sb.Append("        Cast(Max(ams.preaprobadaleg) AS BIT)  AS preaprobadaleg, ");
            sb.Append("        Cast(Max(ams.farmaindustria) AS BIT)  AS farmaindustria, ");
            sb.Append("        Cast(Max(ams.casosclinicos) AS BIT)   AS casosclinicos, ");
            sb.Append("        Max(ams.participantesmsd)             AS participantesmsd, ");
            sb.Append("        Max(ams.detallecriterios)             AS detallecriterios, ");
            sb.Append("        Cast(Max(ams.medicosfichero) AS BIT)  AS medicosfichero, ");
            sb.Append("        Max(ams.duracionhoras)                AS duracionhoras, ");
            sb.Append("        Max(ams.ponentespatrocinados)         AS ponentespatrocinados, ");
            sb.Append("        Max(ams.conceptogastos)               AS conceptogastos, ");
            sb.Append("        Max(ams.importegasto)                 AS importegasto, ");
            sb.Append("        Max(ams.cartascontrato)               AS cartascontrato, ");
            sb.Append("        Max(ams.programaamecs)                AS programaamecs, ");
            sb.Append("        Max(ams.urlprograma)                  AS urlprograma, ");
            sb.Append("        Max(ams.descripcionobjetivo)          AS descripcionobjetivo, ");
            sb.Append("        Max(ams.descripcion)                  AS descripcion, ");
            sb.Append("        Max(ams.nwein)                        AS nwein, ");
            sb.Append("        Max(ams.cargoadaxas)                  AS cargoadaxas, ");
            sb.Append("        Cast(Max(ams.preaprobadaamed) AS BIT) AS preaprobadaamed, ");
            sb.Append("        Max(ams.idsolicitante)                AS idsolicitante, ");
            sb.Append("        Max(pet.idcargo)                      AS idcargo, ");
            sb.Append("        Max(ams.idcreadopor)                  AS idcreadopor,");
            sb.Append("        ams.fechaamecs                        AS fechaamecs, ");
            sb.Append("        Max(ams.idtipoactividad)              AS idtipoactividad, ");
            sb.Append("        Cast(Max(ams.paraguas) AS BIT)        AS paraguas, ");
            sb.Append("        Max(est.idestado)                     AS idestado, ");
            sb.Append("        Max(est.estado)                       AS estado, ");
            sb.Append("        Cast(Max(CASE ");
            sb.Append("                   WHEN Isnull(unid.idunidadamec, -1) < 0 THEN 1 ");
            sb.Append("                   ELSE 0 ");
            sb.Append("                 END) AS BIT)                 AS nuevosFlujosAprobacion, ams.veeva AS veeva, ams.newco AS newco into #ResultAMEC");
            sb.Append(" FROM   amecs ams ");
            sb.Append("        inner JOIN #Temp ON cast(ams.idamecs as CHAR) COLLATE MODERN_SPANISH_CI_AI = #Temp.idamecs COLLATE MODERN_SPANISH_CI_AI");
            sb.Append("        INNER JOIN peticionarios pet ");
            sb.Append("                ON pet.idpeticionario = ams.idsolicitante ");
            sb.Append("        INNER JOIN estadoamecs est ");
            sb.Append("                ON est.idestado = ams.idestado ");
            sb.Append("        LEFT JOIN unidorganizamec unid ");
            sb.Append("               ON unid.idamecs = ams.idamecs ");
            consulta = string.Format(sb + " {0} ", CrearSeccionWhereFiltroListadoAmecProcedure(filtro, datosRoles, false, true));

 /*           if (filtro.idestadoamec == 6)
            {
                consulta +=
                consulta += " SELECT idestado, idnivelaprobacion ";
                consulta += "  into #nivelAprobacionUsuario ";
                consulta += " FROM histasocamecs WHERE histasocamecs.fechacreacion >= (SELECT TOP 1 fechacreacion FROM histasocamecs WHERE idamecs = 400000313  AND idestado = 29 ORDER BY fechacreacion DESC) ";
                consulta += " AND idaprobador = 100010658 ";
                consulta += " delete from #ResultAMEC where idestado not in (10,39,43) and (select count(*) from #nivelAprobacionUsuario where idestado in (10,39,43)) > 0 ";
            }*/

            return consulta;
        }

        public string ObtenerConsultaListadoAMECsNewCo(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles)
        {
            string consulta = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO #ResultAMEC ");
            sb.Append(" SELECT DISTINCT ams.idamecs, ");
            sb.Append("        Cast(Max(ams.preaprobadaneg) AS BIT)  AS preaprobadaneg, ");
            sb.Append("        Cast(Max(ams.preaprobadaleg) AS BIT)  AS preaprobadaleg, ");
            sb.Append("        Cast(Max(ams.farmaindustria) AS BIT)  AS farmaindustria, ");
            sb.Append("        Cast(Max(ams.casosclinicos) AS BIT)   AS casosclinicos, ");
            sb.Append("        Max(ams.participantesmsd)             AS participantesmsd, ");
            sb.Append("        Max(ams.detallecriterios)             AS detallecriterios, ");
            sb.Append("        Cast(Max(ams.medicosfichero) AS BIT)  AS medicosfichero, ");
            sb.Append("        Max(ams.duracionhoras)                AS duracionhoras, ");
            sb.Append("        Max(ams.ponentespatrocinados)         AS ponentespatrocinados, ");
            sb.Append("        Max(ams.conceptogastos)               AS conceptogastos, ");
            sb.Append("        Max(ams.importegasto)                 AS importegasto, ");
            sb.Append("        Max(ams.cartascontrato)               AS cartascontrato, ");
            sb.Append("        Max(ams.programaamecs)                AS programaamecs, ");
            sb.Append("        Max(ams.urlprograma)                  AS urlprograma, ");
            sb.Append("        Max(ams.descripcionobjetivo)          AS descripcionobjetivo, ");
            sb.Append("        Max(ams.descripcion)                  AS descripcion, ");
            sb.Append("        Max(ams.nwein)                        AS nwein, ");
            sb.Append("        Max(ams.cargoadaxas)                  AS cargoadaxas, ");
            sb.Append("        Cast(Max(ams.preaprobadaamed) AS BIT) AS preaprobadaamed, ");
            sb.Append("        Max(ams.idsolicitante)                AS idsolicitante, ");
            sb.Append("        Max(pet.idcargo)                      AS idcargo, ");
            sb.Append("        Max(ams.idcreadopor)                  AS idcreadopor,");
            sb.Append("        ams.fechaamecs                        AS fechaamecs, ");
            sb.Append("        Max(ams.idtipoactividad)              AS idtipoactividad, ");
            sb.Append("        Cast(Max(ams.paraguas) AS BIT)        AS paraguas, ");
            sb.Append("        Max(est.idestado)                     AS idestado, ");
            sb.Append("        Max(est.estado)                       AS estado, ");
            sb.Append("        Cast(Max(CASE ");
            sb.Append("                   WHEN Isnull(unid.idunidadamec, -1) < 0 THEN 1 ");
            sb.Append("                   ELSE 0 ");
            sb.Append("                 END) AS BIT)                 AS nuevosFlujosAprobacion, ams.veeva AS veeva, ams.newco AS newco ");
            sb.Append(" FROM   amecs ams ");
            sb.Append("        INNER JOIN peticionarios pet ");
            sb.Append("                ON pet.idpeticionario = ams.idsolicitante ");
            sb.Append("        INNER JOIN estadoamecs est ");
            sb.Append("                ON est.idestado = ams.idestado ");
            sb.Append("        LEFT JOIN unidorganizamec unid ");
            sb.Append("               ON unid.idamecs = ams.idamecs ");
            consulta = string.Format(sb + " {0} ", CrearSeccionWhereFiltroListadoAmecProcedureNewCo(filtro, datosRoles, false, true));

            return consulta;
        }

        public string ObtenerConsultaListadoAMECsVeeva(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles)
        {
            string consulta = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(" INSERT INTO #ResultAMEC ");
            sb.Append(" SELECT DISTINCT ams.idamecs, ");
            sb.Append("        Cast(Max(ams.preaprobadaneg) AS BIT)  AS preaprobadaneg, ");
            sb.Append("        Cast(Max(ams.preaprobadaleg) AS BIT)  AS preaprobadaleg, ");
            sb.Append("        Cast(Max(ams.farmaindustria) AS BIT)  AS farmaindustria, ");
            sb.Append("        Cast(Max(ams.casosclinicos) AS BIT)   AS casosclinicos, ");
            sb.Append("        Max(ams.participantesmsd)             AS participantesmsd, ");
            sb.Append("        Max(ams.detallecriterios)             AS detallecriterios, ");
            sb.Append("        Cast(Max(ams.medicosfichero) AS BIT)  AS medicosfichero, ");
            sb.Append("        Max(ams.duracionhoras)                AS duracionhoras, ");
            sb.Append("        Max(ams.ponentespatrocinados)         AS ponentespatrocinados, ");
            sb.Append("        Max(ams.conceptogastos)               AS conceptogastos, ");
            sb.Append("        Max(ams.importegasto)                 AS importegasto, ");
            sb.Append("        Max(ams.cartascontrato)               AS cartascontrato, ");
            sb.Append("        Max(ams.programaamecs)                AS programaamecs, ");
            sb.Append("        Max(ams.urlprograma)                  AS urlprograma, ");
            sb.Append("        Max(ams.descripcionobjetivo)          AS descripcionobjetivo, ");
            sb.Append("        Max(ams.descripcion)                  AS descripcion, ");
            sb.Append("        Max(ams.nwein)                        AS nwein, ");
            sb.Append("        Max(ams.cargoadaxas)                  AS cargoadaxas, ");
            sb.Append("        Cast(Max(ams.preaprobadaamed) AS BIT) AS preaprobadaamed, ");
            sb.Append("        Max(ams.idsolicitante)                AS idsolicitante, ");
            sb.Append("        Max(pet.idcargo)                      AS idcargo, ");
            sb.Append("        Max(ams.idcreadopor)                  AS idcreadopor,");
            sb.Append("        ams.fechaamecs                        AS fechaamecs, ");
            sb.Append("        Max(ams.idtipoactividad)              AS idtipoactividad, ");
            sb.Append("        Cast(Max(ams.paraguas) AS BIT)        AS paraguas, ");
            sb.Append("        Max(est.idestado)                     AS idestado, ");
            sb.Append("        Max(est.estado)                       AS estado, ");
            sb.Append("        Cast(Max(CASE ");
            sb.Append("                   WHEN Isnull(unid.idunidadamec, -1) < 0 THEN 1 ");
            sb.Append("                   ELSE 0 ");
            sb.Append("                 END) AS BIT)                 AS nuevosFlujosAprobacion, ams.veeva AS veeva, ams.newco AS newco ");
            sb.Append(" FROM   amecs ams ");
            sb.Append("        INNER JOIN peticionarios pet ");
            sb.Append("                ON pet.idpeticionario = ams.idsolicitante ");
            sb.Append("        INNER JOIN estadoamecs est ");
            sb.Append("                ON est.idestado = ams.idestado ");
            sb.Append("        LEFT JOIN unidorganizamec unid ");
            sb.Append("               ON unid.idamecs = ams.idamecs ");
            consulta = string.Format(sb + " {0} ", CrearSeccionWhereFiltroListadoAmecProcedureVeeva(filtro, datosRoles, false, true));

            return consulta;
        }

        public int ObtenerNumeroListadoAMECsProcedure(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles)
        {
            try
            {

                string oldAmecs = string.Empty;
                string newAmecs = string.Empty;
                string newAmecsVeeva = string.Empty;

                if (filtro.idestadoamec == 6)
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(" INSERT INTO #ResultAMEC ");
                    query.Append(" SELECT DISTINCT ams.idamecs, ");
                    query.Append("        Cast(Max(ams.preaprobadaneg) AS BIT)  AS preaprobadaneg, ");
                    query.Append("        Cast(Max(ams.preaprobadaleg) AS BIT)  AS preaprobadaleg, ");
                    query.Append("        Cast(Max(ams.farmaindustria) AS BIT)  AS farmaindustria, ");
                    query.Append("        Cast(Max(ams.casosclinicos) AS BIT)   AS casosclinicos, ");
                    query.Append("        Max(ams.participantesmsd)             AS participantesmsd, ");
                    query.Append("        Max(ams.detallecriterios)             AS detallecriterios, ");
                    query.Append("        Cast(Max(ams.medicosfichero) AS BIT)  AS medicosfichero, ");
                    query.Append("        Max(ams.duracionhoras)                AS duracionhoras, ");
                    query.Append("        Max(ams.ponentespatrocinados)         AS ponentespatrocinados, ");
                    query.Append("        Max(ams.conceptogastos)               AS conceptogastos, ");
                    query.Append("        Max(ams.importegasto)                 AS importegasto, ");
                    query.Append("        Max(ams.cartascontrato)               AS cartascontrato, ");
                    query.Append("        Max(ams.programaamecs)                AS programaamecs, ");
                    query.Append("        Max(ams.urlprograma)                  AS urlprograma, ");
                    query.Append("        Max(ams.descripcionobjetivo)          AS descripcionobjetivo, ");
                    query.Append("        Max(ams.descripcion)                  AS descripcion, ");
                    query.Append("        Max(ams.nwein)                        AS nwein, ");
                    query.Append("        Max(ams.cargoadaxas)                  AS cargoadaxas, ");
                    query.Append("        Cast(Max(ams.preaprobadaamed) AS BIT) AS preaprobadaamed, ");
                    query.Append("        Max(ams.idsolicitante)                AS idsolicitante, ");
                    query.Append("        Max(pet.idcargo)                      AS idcargo, ");
                    query.Append("        Max(ams.idcreadopor)                  AS idcreadopor,");
                    query.Append("        ams.fechaamecs                        AS fechaamecs, ");
                    query.Append("        Max(ams.idtipoactividad)              AS idtipoactividad, ");
                    query.Append("        Cast(Max(ams.paraguas) AS BIT)        AS paraguas, ");
                    query.Append("        Max(est.idestado)                     AS idestado, ");
                    query.Append("        Max(est.estado)                       AS estado, ");
                    query.Append("        Cast(Max(CASE ");
                    query.Append("                   WHEN Isnull(unid.idunidadamec, -1) < 0 THEN 1 ");
                    query.Append("                   ELSE 0 ");
                    query.Append("                 END) AS BIT)                 AS nuevosFlujosAprobacion, ams.veeva AS veeva, ams.newco AS newco ");
                    query.Append(" FROM   amecs ams ");
                    query.Append("        INNER JOIN peticionarios pet ");
                    query.Append("                ON pet.idpeticionario = ams.idsolicitante ");
                    query.Append("        INNER JOIN estadoamecs est ");
                    query.Append("                ON est.idestado = ams.idestado ");
                    query.Append("        LEFT JOIN unidorganizamec unid ");
                    query.Append("               ON unid.idamecs = ams.idamecs ");

                    oldAmecs = string.Format(query + " {0}", CrearSeccionWhereFiltroListadoAmecProcedure(filtro, datosRoles, false, false));
                }
                else
                {

                    StringBuilder query = new StringBuilder();
                    query.Append(" INSERT INTO #ResultAMEC ");
                    query.Append(" SELECT DISTINCT ams.idamecs as idamecs, ");
                    query.Append("        Cast(Max(ams.preaprobadaneg) AS BIT)  AS preaprobadaneg, ");
                    query.Append("        Cast(Max(ams.preaprobadaleg) AS BIT)  AS preaprobadaleg, ");
                    query.Append("        Cast(Max(ams.farmaindustria) AS BIT)  AS farmaindustria, ");
                    query.Append("        Cast(Max(ams.casosclinicos) AS BIT)   AS casosclinicos, ");
                    query.Append("        Max(ams.participantesmsd)             AS participantesmsd, ");
                    query.Append("        Max(ams.detallecriterios)             AS detallecriterios, ");
                    query.Append("        Cast(Max(ams.medicosfichero) AS BIT)  AS medicosfichero, ");
                    query.Append("        Max(ams.duracionhoras)                AS duracionhoras, ");
                    query.Append("        Max(ams.ponentespatrocinados)         AS ponentespatrocinados, ");
                    query.Append("        Max(ams.conceptogastos)               AS conceptogastos, ");
                    query.Append("        Max(ams.importegasto)                 AS importegasto, ");
                    query.Append("        Max(ams.cartascontrato)               AS cartascontrato, ");
                    query.Append("        Max(ams.programaamecs)                AS programaamecs, ");
                    query.Append("        Max(ams.urlprograma)                  AS urlprograma, ");
                    query.Append("        Max(ams.descripcionobjetivo)          AS descripcionobjetivo, ");
                    query.Append("        Max(ams.descripcion)                  AS descripcion, ");
                    query.Append("        Max(ams.nwein)                        AS nwein, ");
                    query.Append("        Max(ams.cargoadaxas)                  AS cargoadaxas, ");
                    query.Append("        Cast(Max(ams.preaprobadaamed) AS BIT) AS preaprobadaamed, ");
                    query.Append("        Max(ams.idsolicitante)                AS idsolicitante, ");
                    query.Append("        Max(pet.idcargo)                      AS idcargo, ");
                    query.Append("        Max(ams.idcreadopor)                  AS idcreadopor,");
                    query.Append("        ams.fechaamecs                        AS fechaamecs, ");
                    query.Append("        Max(ams.idtipoactividad)              AS idtipoactividad, ");
                    query.Append("        Cast(Max(ams.paraguas) AS BIT)        AS paraguas, ");
                    query.Append("        Max(est.idestado)                     AS idestado, ");
                    query.Append("        Max(est.estado)                       AS estado, ");
                    query.Append("        Cast(Max(CASE ");
                    query.Append("                   WHEN Isnull(unid.idunidadamec, -1) < 0 THEN 1 ");
                    query.Append("                   ELSE 0 ");
                    query.Append("                 END) AS BIT)                 AS nuevosFlujosAprobacion, ams.veeva AS veeva, ams.newco AS newco ");
                    query.Append(" FROM   amecs ams ");
                    query.Append("        INNER JOIN peticionarios pet ");
                    query.Append("                ON pet.idpeticionario = ams.idsolicitante ");
                    query.Append("        INNER JOIN estadoamecs est ");
                    query.Append("                ON est.idestado = ams.idestado ");
                    query.Append("        LEFT JOIN unidorganizamec unid ");
                    query.Append("               ON unid.idamecs = ams.idamecs ");


                    oldAmecs = string.Format(query + " {0}", CrearSeccionWhereFiltroListadoAmecProcedure(filtro, datosRoles, false, false));
                }
                newAmecs = ObtenerConsultaListadoAMECsNuevo(filtro, datosRoles);
                newAmecsVeeva = ObtenerConsultaListadoAMECsVeeva(filtro, datosRoles);
                using (var transaccion = _session.BeginTransaction())
                {
                    _session.CreateSQLQuery(newAmecs).ExecuteUpdate();
                    _session.CreateSQLQuery(oldAmecs).ExecuteUpdate();
                    _session.CreateSQLQuery(newAmecsVeeva).ExecuteUpdate();

                    string query = "SELECT DISTINCT * FROM #ResultAMEC";
                    var newAmecsList = _session.CreateSQLQuery(query)
                       .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ListadoAmecs))).List<ListadoAmecs>();
                    transaccion.Commit();
                    if (filtro.idestadoamec == 6 && !datosRoles.medico.Value && !datosRoles.legal.Value /*&& !datosRoles.administrador.Value*/)
                    {
                        return CheckMeCorrespondeAprobarNegocio(newAmecsList.ToList(), datosRoles).ToList().Count;
                    }
                    return newAmecsList.Count;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ListadoAmecs> ObtenerNumeroListadoAMECsProcedureAux(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles)
        {
            try
            {

                string oldAmecs = string.Empty;
                string newAmecs = string.Empty;
                string newAmecsVeeva = string.Empty;

                if (filtro.idestadoamec == 6)
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(" INSERT INTO #ResultAMEC ");
                    query.Append(" SELECT DISTINCT ams.idamecs, ");
                    query.Append("        Cast(Max(ams.preaprobadaneg) AS BIT)  AS preaprobadaneg, ");
                    query.Append("        Cast(Max(ams.preaprobadaleg) AS BIT)  AS preaprobadaleg, ");
                    query.Append("        Cast(Max(ams.farmaindustria) AS BIT)  AS farmaindustria, ");
                    query.Append("        Cast(Max(ams.casosclinicos) AS BIT)   AS casosclinicos, ");
                    query.Append("        Max(ams.participantesmsd)             AS participantesmsd, ");
                    query.Append("        Max(ams.detallecriterios)             AS detallecriterios, ");
                    query.Append("        Cast(Max(ams.medicosfichero) AS BIT)  AS medicosfichero, ");
                    query.Append("        Max(ams.duracionhoras)                AS duracionhoras, ");
                    query.Append("        Max(ams.ponentespatrocinados)         AS ponentespatrocinados, ");
                    query.Append("        Max(ams.conceptogastos)               AS conceptogastos, ");
                    query.Append("        Max(ams.importegasto)                 AS importegasto, ");
                    query.Append("        Max(ams.cartascontrato)               AS cartascontrato, ");
                    query.Append("        Max(ams.programaamecs)                AS programaamecs, ");
                    query.Append("        Max(ams.urlprograma)                  AS urlprograma, ");
                    query.Append("        Max(ams.descripcionobjetivo)          AS descripcionobjetivo, ");
                    query.Append("        Max(ams.descripcion)                  AS descripcion, ");
                    query.Append("        Max(ams.nwein)                        AS nwein, ");
                    query.Append("        Max(ams.cargoadaxas)                  AS cargoadaxas, ");
                    query.Append("        Cast(Max(ams.preaprobadaamed) AS BIT) AS preaprobadaamed, ");
                    query.Append("        Max(ams.idsolicitante)                AS idsolicitante, ");
                    query.Append("        Max(pet.idcargo)                      AS idcargo, ");
                    query.Append("        Max(ams.idcreadopor)                  AS idcreadopor,");
                    query.Append("        ams.fechaamecs                        AS fechaamecs, ");
                    query.Append("        Max(ams.idtipoactividad)              AS idtipoactividad, ");
                    query.Append("        Cast(Max(ams.paraguas) AS BIT)        AS paraguas, ");
                    query.Append("        Max(est.idestado)                     AS idestado, ");
                    query.Append("        Max(est.estado)                       AS estado, ");
                    query.Append("        Cast(Max(CASE ");
                    query.Append("                   WHEN Isnull(unid.idunidadamec, -1) < 0 THEN 1 ");
                    query.Append("                   ELSE 0 ");
                    query.Append("                 END) AS BIT)                 AS nuevosFlujosAprobacion, ams.veeva AS veeva, ams.newco AS newco ");
                    query.Append(" FROM   amecs ams ");
                    query.Append("        INNER JOIN peticionarios pet ");
                    query.Append("                ON pet.idpeticionario = ams.idsolicitante ");
                    query.Append("        INNER JOIN estadoamecs est ");
                    query.Append("                ON est.idestado = ams.idestado ");
                    query.Append("        LEFT JOIN unidorganizamec unid ");
                    query.Append("               ON unid.idamecs = ams.idamecs ");

                    oldAmecs = string.Format(query + " {0}", CrearSeccionWhereFiltroListadoAmecProcedure(filtro, datosRoles, false, false));
                }
                else
                {

                    StringBuilder query = new StringBuilder();
                    query.Append(" INSERT INTO #ResultAMEC ");
                    query.Append(" SELECT DISTINCT ams.idamecs as idamecs, ");
                    query.Append("        Cast(Max(ams.preaprobadaneg) AS BIT)  AS preaprobadaneg, ");
                    query.Append("        Cast(Max(ams.preaprobadaleg) AS BIT)  AS preaprobadaleg, ");
                    query.Append("        Cast(Max(ams.farmaindustria) AS BIT)  AS farmaindustria, ");
                    query.Append("        Cast(Max(ams.casosclinicos) AS BIT)   AS casosclinicos, ");
                    query.Append("        Max(ams.participantesmsd)             AS participantesmsd, ");
                    query.Append("        Max(ams.detallecriterios)             AS detallecriterios, ");
                    query.Append("        Cast(Max(ams.medicosfichero) AS BIT)  AS medicosfichero, ");
                    query.Append("        Max(ams.duracionhoras)                AS duracionhoras, ");
                    query.Append("        Max(ams.ponentespatrocinados)         AS ponentespatrocinados, ");
                    query.Append("        Max(ams.conceptogastos)               AS conceptogastos, ");
                    query.Append("        Max(ams.importegasto)                 AS importegasto, ");
                    query.Append("        Max(ams.cartascontrato)               AS cartascontrato, ");
                    query.Append("        Max(ams.programaamecs)                AS programaamecs, ");
                    query.Append("        Max(ams.urlprograma)                  AS urlprograma, ");
                    query.Append("        Max(ams.descripcionobjetivo)          AS descripcionobjetivo, ");
                    query.Append("        Max(ams.descripcion)                  AS descripcion, ");
                    query.Append("        Max(ams.nwein)                        AS nwein, ");
                    query.Append("        Max(ams.cargoadaxas)                  AS cargoadaxas, ");
                    query.Append("        Cast(Max(ams.preaprobadaamed) AS BIT) AS preaprobadaamed, ");
                    query.Append("        Max(ams.idsolicitante)                AS idsolicitante, ");
                    query.Append("        Max(pet.idcargo)                      AS idcargo, ");
                    query.Append("        Max(ams.idcreadopor)                  AS idcreadopor,");
                    query.Append("        ams.fechaamecs                        AS fechaamecs, ");
                    query.Append("        Max(ams.idtipoactividad)              AS idtipoactividad, ");
                    query.Append("        Cast(Max(ams.paraguas) AS BIT)        AS paraguas, ");
                    query.Append("        Max(est.idestado)                     AS idestado, ");
                    query.Append("        Max(est.estado)                       AS estado, ");
                    query.Append("        Cast(Max(CASE ");
                    query.Append("                   WHEN Isnull(unid.idunidadamec, -1) < 0 THEN 1 ");
                    query.Append("                   ELSE 0 ");
                    query.Append("                 END) AS BIT)                 AS nuevosFlujosAprobacion, ams.veeva AS veeva, ams.newco AS newco ");
                    query.Append(" FROM   amecs ams ");
                    query.Append("        INNER JOIN peticionarios pet ");
                    query.Append("                ON pet.idpeticionario = ams.idsolicitante ");
                    query.Append("        INNER JOIN estadoamecs est ");
                    query.Append("                ON est.idestado = ams.idestado ");
                    query.Append("        LEFT JOIN unidorganizamec unid ");
                    query.Append("               ON unid.idamecs = ams.idamecs ");


                    oldAmecs = string.Format(query + " {0}", CrearSeccionWhereFiltroListadoAmecProcedure(filtro, datosRoles, false, false));
                }
                newAmecs = ObtenerConsultaListadoAMECsNuevo(filtro, datosRoles);
                newAmecsVeeva = ObtenerConsultaListadoAMECsVeeva(filtro, datosRoles);
                using (var transaccion = _session.BeginTransaction())
                {
                    _session.CreateSQLQuery(newAmecs).ExecuteUpdate();
                    _session.CreateSQLQuery(oldAmecs).ExecuteUpdate();
                    _session.CreateSQLQuery(newAmecsVeeva).ExecuteUpdate();

                    string query = "SELECT DISTINCT * FROM #ResultAMEC";
                    var newAmecsList = _session.CreateSQLQuery(query)
                       .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ListadoAmecs))).List<ListadoAmecs>();
                    transaccion.Commit();
                    return newAmecsList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public ICollection<ListadoAmecs> ObtenerListadoAMECs(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles)
        {

            filtro.Roles = ObtenerFiltroRolAmecCliente(datosRoles, filtro);
            string consulta = string.Empty;
            List<ListadoAmecs> ResultadoFinal = new List<ListadoAmecs>();

            if (filtro.idestadoamec == 6)
            {
                consulta =
                    string.Format(
                        "SELECT distinct (ams.idamecs), ams.preaprobadaneg, ams.preaprobadaleg, ams.farmaindustria, ams.casosclinicos, ams.participantesmsd, ams.detallecriterios, ams.medicosfichero, ams.duracionhoras, ams.ponentespatrocinados, ams.conceptogastos, ams.importegasto, ams.cartascontrato, ams.programaamecs, ams.urlprograma, ams.descripcionobjetivo, ams.descripcion, ams.nwein, ams.cargoadaxas, ams.preaprobadaamed, ams.idsolicitante, pet.idcargo, ams.idcreadopor, ams.fechaamecs, ams.idtipoactividad, ams.paraguas, est.idestado, est.estado, case when ISNULL(unid.idunidadamec, -1) < 0 then cast(1 as bit) else cast(0 as bit) end as nuevosFlujosAprobacion, ams.veeva FROM amecs ams inner join peticionarios pet on pet.idpeticionario=ams.idsolicitante inner join  estadoamecs est on est.idestado = ams.idestado left join unidorganizamec unid on unid.idamecs=ams.idamecs {0}",
                        CrearSeccionWhereFiltroListadoAmec(filtro, datosRoles));
            }
            else
            {   consulta =
                    string.Format(
                        "SELECT distinct (ams.idamecs), ams.preaprobadaneg, ams.preaprobadaleg, ams.farmaindustria, ams.casosclinicos, ams.participantesmsd, ams.detallecriterios, ams.medicosfichero, ams.duracionhoras, ams.ponentespatrocinados, ams.conceptogastos, ams.importegasto, ams.cartascontrato, ams.programaamecs, ams.urlprograma, ams.descripcionobjetivo, ams.descripcion, ams.nwein, ams.cargoadaxas, ams.preaprobadaamed, ams.idsolicitante, pet.idcargo, ams.idcreadopor, ams.fechaamecs, ams.idtipoactividad, ams.paraguas, est.idestado, est.estado, case when ISNULL(unid.idunidadamec, -1) < 0 then cast(1 as bit) else cast(0 as bit) end as nuevosFlujosAprobacion, ams.veeva FROM amecs ams inner join peticionarios pet on pet.idpeticionario=ams.idsolicitante inner join  estadoamecs est on est.idestado = ams.idestado left join unidorganizamec unid on unid.idamecs=ams.idamecs {0}",
                        CrearSeccionWhereFiltroListadoAmec(filtro, datosRoles));
            }


            if (filtro.MaximumRows > 0 && filtro.StartRowIndex > 0)
            {
                return ListadoAmecs.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).Skip(filtro.StartRowIndex.Value).Take(filtro.MaximumRows.Value).ToList();
            }
            if (filtro.StartRowIndex > 0)
            {
                return ListadoAmecs.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).Skip(filtro.StartRowIndex.Value).ToList();
            }

            return ListadoAmecs.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));

        }

        public int ObtenerNumeroListadoAMECs(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles)
        {

            filtro.Roles = ObtenerFiltroRolAmecCliente(datosRoles, filtro);

            string consulta = string.Empty;

            if (filtro.idestadoamec == 6)
                consulta = string.Format("SELECT count(distinct(ams.idamecs)) FROM amecs ams inner join peticionarios pet on pet.idpeticionario=ams.idsolicitante inner join  estadoamecs est on est.idestado = ams.idestado left join unidorganizamec unid on unid.idamecs=ams.idamecs {0}", CrearSeccionWhereFiltroListadoAmec(filtro, datosRoles));
            else
                consulta = string.Format("SELECT count(distinct(ams.idamecs)) FROM amecs ams inner join peticionarios pet on pet.idpeticionario=ams.idsolicitante inner join  estadoamecs est on est.idestado = ams.idestado left join unidorganizamec unid on unid.idamecs=ams.idamecs {0}", CrearSeccionWhereFiltroListadoAmec(filtro, datosRoles));

            long numDocumentos = long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            return Convert.ToInt32(numDocumentos);
        }

        public int EliminarDocumentacion(int iddocumentacion)
        {
            string consulta = string.Format("DELETE from docasocamecs where iddocumentacion ={0}", iddocumentacion);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        public int GuardaDocumentacion(DDocumentacionAmec DocAmec)
        {
            string consulta = string.Empty;
            if (DocAmec.comentariosdoc != null) DocAmec.comentariosdoc = DocAmec.comentariosdoc.Replace("'", "´");
            int adjEmail = DocAmec.adjuntaraemail.ToString().ToUpper() == "FALSE" ? 0 : 1;
            consulta = "INSERT INTO docasocamecs (idamecs, ubicaciondoc, tipodoc, nombredoc, comentariosdoc, adjuntaraemail, idcreadopor, fechacreacion, idcategoriadocumento) VALUES('" + DocAmec.idamecs + "','" + DocAmec.ubicaciondoc + "','" + DocAmec.tipodoc + "','" + DocAmec.nombredoc + "','" + DocAmec.comentariosdoc + "'," + adjEmail + "," + DocAmec.idcreadopor + ",CURRENT_TIMESTAMP," + DocAmec.idcategoriadocumento + ")";
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        public int ObtenerNumeroDocumentacionAMEC(string idamec)
        {
            string consulta = string.Empty;

            consulta = "SELECT count(*) FROM docasocamecs docas inner join categoria_documento catdoc on docas.idcategoriadocumento = catdoc.idcategoriadocumento inner join peticionarios pet on pet.idpeticionario = docas.idcreadopor where idamecs='" + idamec + "'";

            long numDocumentos = long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            return Convert.ToInt32(numDocumentos);
        }

        public ICollection<DDocumentacionAmec> ObtenerDocumentacionAMEC(string idamec, string sortParameter, int startRowIndex, int maximumRows)
        {
            string consulta = string.Empty;
            consulta = string.Format("SELECT catdoc.categoriadocumento, docas.idcategoriadocumento, docas.fechacreacion, " +
                                     "docas.adjuntaraemail, docas.comentariosdoc, docas.nombredoc, docas.ubicaciondoc," +
                                     " docas.idcreadopor, docas.tipodoc, docas.idamecs, docas.iddocumentacion, " +
                                     "(rtrim(ltrim(isnull(Nombre, '') + ' ' + isnull(Apellido1, '')))) as nombreusuario " +
                                     "FROM docasocamecs docas inner join categoria_documento catdoc on docas.idcategoriadocumento = catdoc.idcategoriadocumento " +
                                     "inner join peticionarios pet on pet.idpeticionario = docas.idcreadopor where idamecs='{0}' {1}", idamec, CrearSeccionOrderbyDocumentacion(sortParameter, startRowIndex, maximumRows));

            if (maximumRows > 0 && startRowIndex > 0)
            {
                return DDocumentacionAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).Skip(startRowIndex).Take(maximumRows).ToList();
            }
            if (startRowIndex > 0)
            {
                return DDocumentacionAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).Skip(startRowIndex).ToList();
            }
            return DDocumentacionAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public DataSet ObtenerCategoriasDocumento()
        {
            try
            {
                string consulta = string.Format("SELECT * FROM categoria_documento");
                var dt = new DataSet();
                dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public string DameSiguienteIdAmecs()
        {
            string consulta = "SELECT dbo.func_contador('AMEC')";

            int idamec = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));

            string update = string.Format("update dbo.contador set numero ={0} where entidad = 'AMEC';", idamec);
            

            int updateConfirmation = Quodem.Sql.SqlServerClient.ExecuteQuery(update);

            return updateConfirmation > 0 ? idamec.ToString() : 0.ToString();
        }

        public int ObtenerNumPendienteSometer(string idpeticionario)
        {
            string consulta = string.Empty;
            consulta = string.Format("SELECT count(*) FROM amecs where idestado = 35 and (idcreadopor = {0} or idsolicitante = {0}) ", idpeticionario);
            return int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }

        public DAmecInfo CargarTodosValoresAmec(string idamec)
        {
            string consulta = string.Empty;
            consulta = "SELECT * FROM amecs where idamecs='" + idamec + "'";
            ICollection<DAmecInfo> amec = DAmecInfo.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));

            if (amec.Count == 0)
                return null;
            else
                return amec.ElementAt<DAmecInfo>(0);
        }

        public string ObtenerNombreTipoRiesgo(string idtiporiesgo)
        {
            string consulta = "SELECT tiporiesgo from tipo_riesgo_fcpa where idtiporiesgo=" + idtiporiesgo;
            return Quodem.Sql.SqlServerClient.GetValue(consulta);
        }

        public string ObtenerNombreTipoActividad(string idtipoactividad)
        {
            string consulta = "SELECT tipoactividad from tipoactividad where idtipoactividad=" + idtipoactividad;
            return Quodem.Sql.SqlServerClient.GetValue(consulta);
        }

        public DataSet ObtenerInformeFCPA(string idamec, string nombrePrograma, string estado, string fechaInicio, string fechaFin, string unidad, string area,
            string region, string distrito, string year, string mes, string iddistrict, string idsaleforce, string iddepartament)
        {
            string consulta = string.Empty;
            DataSet dt = new DataSet();
            consulta = String.Format("SELECT * FROM riesgo_fcpa LEFT JOIN departaments dep ON riesgo_fcpa.iddepartament = dep.iddepartament LEFT JOIN salesforce sal ON	riesgo_fcpa.idsaleforce = sal.idsaleforce LEFT JOIN districts dist ON riesgo_fcpa.iddistrict = dist.iddistrict LEFT JOIN positions pos ON riesgo_fcpa.idposition = POS.idposition {0}", CrearSeccionWhereFiltroInformeFCPA(idamec, nombrePrograma, estado, fechaInicio, fechaFin, unidad, area, region, distrito, year, mes,iddistrict,idsaleforce,iddepartament));
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }
        public int guardarEnInformeFCPA(string idamecs, string tipoactividad, string descripcion, DateTime? fechacomienzo,
           DateTime? fechafinalizacion, string idestado, string usuario, string cargo, string idunidad, string idarea, string idregion,
           string iddistrito, string tiporiesgo, int idtiporiesgo, string nombrePax, string apellido1Pax, string msdid, string tiporeservacolectivo, string idexpediente, int? iddepartament, int? iddistrict, int? idsaleforce, int? idposition)
        {
            try
            {
                string fechacomienz = null;
                string fechafinalizacio = null;
                string idDept = iddepartament.HasValue && iddepartament.Value != -1 ? iddepartament.Value.ToString() : "null";
                string idDist = iddistrict.HasValue && iddistrict.Value != -1 ? iddistrict.Value.ToString() : "null";
                string idSal = idsaleforce.HasValue && idsaleforce.Value != -1 ? idsaleforce.Value.ToString() : "null";
                string idpos = idposition.HasValue && idposition.Value != -1 ? idposition.Value.ToString() : "null";


                if (fechacomienzo != null) { fechacomienz = String.Format("'{0:yyyyMMdd}'", fechacomienzo); } else fechacomienz = "null";
                if (fechafinalizacion != null) { fechafinalizacio = String.Format("'{0:yyyyMMdd}'", fechafinalizacion); } else fechafinalizacio = "null";
                if (nombrePax == "null") nombrePax = "";
                if (descripcion.IndexOf("'") != -1) descripcion = descripcion.Replace("'", "''");
                string consulta = string.Empty;

                consulta =
                    string.Format(
                        "INSERT INTO riesgo_fcpa (idamecs, tipoactividad, nombreprogramaactividad, fechainicio, fechafin, estadoamec," +
                        "usuario, cargo, unidad, area, region, distrito, fechacertificacionFCPA, tiporiesgo, idtiporiesgo," +
                        "nombrepax, apellido1pax, msdid, tiporeservacolectivo, idexpediente,idDistrict,idDepartament,idSaleforce, idposition)" +
                        " VALUES('{0}', '{1}', '{2}', {3}, {4}, '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', CURRENT_TIMESTAMP," +
                        " '{12}', {13}, '{14}', '{15}','{16}', '{17}', {18}, {19}, {20}, {21}, {22} )",
                        idamecs, tipoactividad, descripcion, fechacomienz, fechafinalizacio, idestado, usuario.Replace("'", "''"), cargo,
                        idunidad, idarea, idregion, iddistrito, tiporiesgo, idtiporiesgo, nombrePax.Replace("'", "''"), apellido1Pax.Replace("'", "''"), msdid, tiporeservacolectivo, idexpediente, idDist, idDept, idSal, idpos);


                return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        public IList<string> ObtenerNombreUnidadOrganizativa(string idunidad, string idarea, string idregion, string iddistrito)
        {
            string[] nombreUnidadOrganizativa = new string[4];
            string consulta = string.Empty;
            consulta = "(SELECT unidad as UnidadOrganizativa from unidades where idunidad =" + idunidad + ") UNION ALL (SELECT area from areas where idarea =" + idarea + ") UNION ALL                                   "
                       + "(SELECT region from regiones where idregion =" + idregion + ") UNION ALL (SELECT distrito from distritos where iddistrito =" + iddistrito + ")";

            IList<string> lstString = Helper.IListStringConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));

            for (int i = 0; i < lstString.Count; i++)
            {
                nombreUnidadOrganizativa[i] = lstString[i];
            }

            return nombreUnidadOrganizativa;
        }
        public DataSet ObtenerDocumentacionAdicionalAmec(string idamec)
        {
            try
            {
                string consulta = string.Empty;
                DataSet dt = new DataSet();

                consulta = String.Format("SELECT amecs.idamecs, estadoamecs.estado, amecs.fechaAmecs, rtrim(ltrim(rtrim(ltrim(peticionarios_1.nombre)) + ' ' + peticionarios_1.apellido1)) AS Usuario, rtrim(ltrim(rtrim(ltrim(peticionarios.nombre)) + ' ' + peticionarios.apellido1)) AS SolicitanteAmec, amecs.descripcion AS NombreProgramaActividad, tipoactividad.tipoactividad, amecs.fechacomienzo, amecs.fechafinalizacion,  amecs.ponentespatrocinados AS NumPonentes, amecs.participantesmsd, docasocamecs.fechacreacion AS FechaDocumento, categoria_documento.categoriadocumento, docasocamecs.nombredoc, docasocamecs.comentariosdoc, case when Paraguas=1 then 'SI' ELSE 'NO' END AS Paraguas,case when farmaindustria=1 then 'SI' ELSE 'NO' END AS FarmaIndustria, case when Adjuntaraemail=1 then 'SI' ELSE 'NO' END AS AdjuntadoaFI" +
                " FROM ((((((amecs INNER JOIN estadoamecs ON amecs.idestado = estadoamecs.idestado) INNER JOIN peticionarios ON amecs.idsolicitante = peticionarios.IdPeticionario) INNER JOIN tipoactividad ON amecs.idtipoactividad = tipoactividad.idtipoactividad) INNER JOIN criteriosseleccion ON amecs.idcriterioseleccion = criteriosseleccion.idcriterio) INNER JOIN docasocamecs ON amecs.idamecs = docasocamecs.idamecs) INNER JOIN peticionarios AS peticionarios_1 ON docasocamecs.idcreadopor = peticionarios_1.IdPeticionario) LEFT JOIN categoria_documento ON docasocamecs.idcategoriadocumento = categoria_documento.idcategoriadocumento" +
                " WHERE ( amecs.idamecs = '{0}' and ((amecs.importegasto)>0) AND ((estadoamecs.idestado)<>5 And (estadoamecs.idestado)<>3))" +
                " ORDER BY docasocamecs.fechacreacion", idamec);

                dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet CargarTodosValoresAmecExcel(string idamec)
        {
            string consulta = string.Empty;
            DataSet dt = new DataSet();
            consulta = "SELECT amecs.idamecs, estadoamecs.estado, amecs.fechaAmecs," +
            " rtrim(ltrim(rtrim(ltrim(peticionarios.nombre)) + ' ' + peticionarios.apellido1)) AS Solicitante, " +
            " Year(amecs.fechaAmecs) AS AñoAmec, uni.unidad, area.area, reg.region, dist.distrito as [Distrito*], dep.departament as Departamento, sal.saleforce as [Fuerza de Ventas], distr.district as [Distrito], " +
            " rtrim(ltrim(rtrim(ltrim(peticionarios_1.nombre)) + ' ' + peticionarios_1.apellido1)) AS Creador, " +
            "amecs.descripcion AS NombreProgramaActividad, tipoactividad.tipoactividad," +
            "amecs.fechacomienzo,amecs.fechaFinalizacion, Year(amecs.fechacomienzo) AS AñoInicioActividad," +
            " Year(amecs.fechaFinalizacion) AS AñoFinActividad," +
            " amecs.cargoadaxas AS Productos, amecs.lugarsede," +
            "amecs.ponentespatrocinados as NumPonentes, amecs.participantesmsd, amecs.conceptogastos," +
            "amecs.ProfesionalesSanitarios as ProfSanitario_HonorariosMsd,cartascontrato," +
            "criteriosseleccion.criterioseleccion, amecs.importegasto," +
            " case when Paraguas=1 then 'SI' ELSE 'NO' END AS Paraguas,case when PoliticaN20=1 then 'SI' ELSE 'NO' END AS N20_FCPA," +
            "case when farmaindustria=1 then 'SI' ELSE 'NO' END AS Farma_Industria, case when CasosClinicos=1 then 'SI' ELSE 'NO' END AS Casos_Clinicos," +
            "case when preaprobadaamed=1 then 'SI' ELSE 'NO' END AS Preaprobado_Medico, case when preaprobadaNeg=1 then 'SI' ELSE 'NO' END AS Preaprobado_Negocio," +
            "case when preaprobadaleg=1 then 'SI' ELSE 'NO' END AS Preaprobado_Legal" +
            " FROM amecs INNER JOIN estadoamecs ON amecs.idestado = estadoamecs.idestado" +
            " INNER JOIN peticionarios ON amecs.idsolicitante = peticionarios.IdPeticionario" +
            " INNER JOIN peticionarios as peticionarios_1 ON amecs.idcreadopor = peticionarios_1.IdPeticionario" +
            " LEFT JOIN tipoactividad ON amecs.idtipoactividad = tipoactividad.idtipoactividad" +
            " LEFT JOIN criteriosseleccion ON amecs.idcriterioseleccion = criteriosseleccion.idcriterio" +
            " LEFT JOIN  unidades uni ON peticionarios.idunidad = uni.idunidad" +
            " LEFT JOIN  areas area ON peticionarios.idarea = area.idarea" +
            " LEFT JOIN  regiones reg ON peticionarios.idregion = reg.idregion" +
            " LEFT JOIN  distritos dist ON peticionarios.iddistrito = dist.iddistrito" +
            " LEFT JOIN departaments dep ON " +
            "     peticionarios.iddepartament = dep.iddepartament " +
            " LEFT JOIN salesforce sal ON " +
            "     peticionarios.idsaleforce = sal.idsaleforce " +
            " LEFT JOIN districts distr ON " +
            "     peticionarios.iddistrict = distr.iddistrict " +
            " where idamecs='" + idamec + "' ";

            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }

        public DAmecInfo ObtenerProgramaAMEC(string idamec, string sortParameter, int startRowIndex, int maximumRows)
        {
            string consulta = string.Empty;
            consulta = "SELECT * FROM amecs where idamecs='" + idamec + "'";
            ICollection<DAmecInfo> amec = DAmecInfo.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            if (amec.Count == 0)
                return null;
            else
                return amec.ElementAt<DAmecInfo>(0);
        }

        public int ObtenerNumeroProgramaAMEC(string idamec)
        {
            string consulta = string.Empty;
            consulta = "SELECT count(*) FROM amecs where idamecs='" + idamec + "'";
            return Convert.ToInt32(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }
        
        public ICollection<DAmecInfo> ObtenerAMECsInfo(FiltroAmecInfo filtroaMEC, DVPeticionariosRoles datosRoles)
        {
            string consulta = string.Empty;
            consulta = "SELECT * FROM amecs";
            return DAmecInfo.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public long ObtenerNumeroAMECsInfo(FiltroAmecInfo filtroaMEC, DVPeticionariosRoles datosRoles)
        {
            string consulta = string.Empty;
            consulta = "SELECT count(*) FROM amecs";
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }


        public ICollection<DEstadoAmec> ObtenerEstadosAmec()
        {
            string consulta = string.Empty;
            consulta = "SELECT * FROM estadoamecs";
            return DEstadoAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }
        public uint ObtenerEstadoAmec(string idamecs)
        {
            string consulta = string.Empty;
            consulta = "SELECT ams.idestado from amecs ams where ams.idamecs ='" + idamecs + "'";
            uint idestado = uint.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            return idestado;
        }
        public DAmecInfo ModificarAMEC(DAmecInfo amec, out bool cambioAgencia, bool updateFechaComienzoFinalizacion)
        {
            string consultaAgencia = "select isnull(idconfempresa, -1) from amecs where idamecs='" + amec.idamecs + "'";
            int agenciaAntigua = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consultaAgencia));

            string fechacomienzo = null;
            string fechafinalizacion = null;
            string participantesmsd = null;
            string duracionhoras = null;
            string ponentespatrocinados = null;
            //decimal importegasto;
            string cartascontrato = null;
            string confempresa = "null";
            string profesionalessanitarios = "null";
            string medicosfichero = "0";
            string politicaN20 = "0";
            string paraguas = "0";
            string casosclinicos = "0";
            string farmaindustria = "0";
            string preaprobadaamed = "0";
            string preaprobadaneg = "0";
            string preaprobadaleg = "0";


            if (amec.fechacomienzo != null) { fechacomienzo = "fechacomienzo='" + String.Format("{0:yyyyMMdd}", amec.fechacomienzo) + "'"; } else fechacomienzo = "fechacomienzo=null";
            if (amec.fechafinalizacion != null) { fechafinalizacion = "fechafinalizacion='" + String.Format("{0:yyyyMMdd}", amec.fechafinalizacion) + "'"; } else fechafinalizacion = "fechafinalizacion=null";


            if (amec.participantesmsd != null) { participantesmsd = amec.participantesmsd.ToString(); } else participantesmsd = "null";
            if (amec.profesionalessanitarios > 0) profesionalessanitarios = amec.profesionalessanitarios.ToString();
            if (amec.paraguas.HasValue && amec.paraguas.Value) paraguas = "1";
            if (amec.medicosfichero.HasValue && amec.medicosfichero.Value) medicosfichero = "1";
            if (amec.politicaN20.HasValue && amec.politicaN20.Value) politicaN20 = "1";
            if (amec.casosclinicos.HasValue && amec.casosclinicos.Value) casosclinicos = "1";
            if (amec.preaprobadaleg.HasValue && amec.preaprobadaleg.Value) preaprobadaleg = "1";
            if (amec.preaprobadaamed.HasValue && amec.preaprobadaamed.Value) preaprobadaamed = "1";
            if (amec.preaprobadaneg.HasValue && amec.preaprobadaneg.Value) preaprobadaneg = "1";
            if (amec.farmaindustria.HasValue && amec.farmaindustria.Value) farmaindustria = "1";
            if (!string.IsNullOrEmpty(amec.duracionhoras)) { duracionhoras = amec.duracionhoras; } else duracionhoras = "null";
            if (amec.ponentespatrocinados != null) { ponentespatrocinados = amec.ponentespatrocinados.ToString(); } else ponentespatrocinados = "null";
            if (amec.importegasto == null) amec.importegasto = 0;
            if (amec.cartascontrato != null) { cartascontrato = amec.cartascontrato.ToString(); } else cartascontrato = "null";
            if (amec.descripcionobjetivo != null) amec.descripcionobjetivo = amec.descripcionobjetivo.Replace("'", "´");
            if (amec.conceptogastos != null) amec.conceptogastos = amec.conceptogastos.Replace("'", "´");
            if (amec.descripcion != null) amec.descripcion = amec.descripcion.Replace("'", "´");
            if (amec.urlprograma != null) amec.urlprograma = amec.urlprograma.Replace("'", "´");
            if (amec.lugarsede != null) amec.lugarsede = amec.lugarsede.Replace("'", "´");
            if (amec.idconfempresa > 0) confempresa = amec.idconfempresa.ToString();
            string amecposition = "null";
            if (amec.idposition.HasValue && amec.idposition > 0)
                amecposition = amec.idposition.Value.ToString();
            string ameccargo = "null";
            if (amec.idcargo.HasValue && amec.idcargo > 0)
                ameccargo = amec.idcargo.Value.ToString();
            //funcion para modificar los datos del amec
            //UPDATE DE TODOS LOS DATOS NECESARIOS           
            string consulta = string.Empty;

            if (updateFechaComienzoFinalizacion)
            {
                consulta = "UPDATE amecs SET idestado =" + amec.idestado + "," + "idsolicitante=" + amec.idsolicitante + "," + "idcargo=" + ameccargo + "," + "idposition=" + amecposition + "," + "nwein = '" + amec.nwein + "'," + " idtipoactividad=" + amec.idtipoactividad + "," + "" +
                                       "preaprobadaamed=" + preaprobadaamed + "," + "preaprobadaneg=" + preaprobadaneg + "," + "preaprobadaleg=" + preaprobadaleg + "," + "farmaindustria=" + farmaindustria + "," +
                                       "casosclinicos=" + casosclinicos + "," + "participantesmsd=" + participantesmsd + "," + "detallecriterios='" + amec.detallecriterios + "'," + "medicosfichero=" + medicosfichero + "," +
                                       "duracionhoras=" + duracionhoras + "," + "ponentespatrocinados=" + ponentespatrocinados + "," + "conceptogastos='" + amec.conceptogastos + "'," + "cargoadaxas='" + amec.cargoadaxas + "'," +
                                       "importegasto=" + amec.importegasto.ToString().Replace(",", ".") + "," + "cartascontrato=" + cartascontrato + "," + "programaamecs='" + amec.programaamecs + "'," + "urlprograma='" + amec.urlprograma + "'," +
                                       "descripcionobjetivo='" + amec.descripcionobjetivo + "'," + "descripcion='" + amec.descripcion + "'," + "politicaN20=" + politicaN20 + "," + fechacomienzo + "," +
                                       fechafinalizacion + "," + "lugarsede='" + amec.lugarsede + "'," + "idcriterioseleccion=" + (amec.idcriterioseleccion == 0 ? "null" : amec.idcriterioseleccion.ToString()) + "," + "criterioespecificado='" + amec.criterioespecificado + "'," +
                                       "paraguas=" + paraguas + "," + "profesionalessanitarios=" + profesionalessanitarios + "," + "idconfempresa=" + confempresa + ",fechaultimaactualizacion = GETDATE() " +
                                       " where idamecs='" + amec.idamecs + "'";
            }
            else {
                consulta = "UPDATE amecs SET idestado =" + amec.idestado + "," + "idsolicitante=" + amec.idsolicitante + "," + "idcargo=" + ameccargo + "," + "idposition=" + amecposition + "," + "nwein = '" + amec.nwein + "'," + " idtipoactividad=" + amec.idtipoactividad + "," + "" +
                           "preaprobadaamed=" + preaprobadaamed + "," + "preaprobadaneg=" + preaprobadaneg + "," + "preaprobadaleg=" + preaprobadaleg + "," + "farmaindustria=" + farmaindustria + "," +
                           "casosclinicos=" + casosclinicos + "," + "participantesmsd=" + participantesmsd + "," + "detallecriterios='" + amec.detallecriterios + "'," + "medicosfichero=" + medicosfichero + "," +
                           "duracionhoras=" + duracionhoras + "," + "ponentespatrocinados=" + ponentespatrocinados + "," + "conceptogastos='" + amec.conceptogastos + "'," + "cargoadaxas='" + amec.cargoadaxas + "'," +
                           "importegasto=" + amec.importegasto.ToString().Replace(",", ".") + "," + "cartascontrato=" + cartascontrato + "," + "programaamecs='" + amec.programaamecs + "'," + "urlprograma='" + amec.urlprograma + "'," +
                           "descripcionobjetivo='" + amec.descripcionobjetivo + "'," + "descripcion='" + amec.descripcion + "'," + "politicaN20=" + politicaN20 + "," + "lugarsede='" + amec.lugarsede + "'," + "idcriterioseleccion=" + (amec.idcriterioseleccion == 0 ? "null" : amec.idcriterioseleccion.ToString()) + "," + "criterioespecificado='" + amec.criterioespecificado + "'," +
                           "paraguas=" + paraguas + "," + "profesionalessanitarios=" + profesionalessanitarios + "," + "idconfempresa=" + confempresa + ",fechaultimaactualizacion = GETDATE() " +
                           " where idamecs='" + amec.idamecs + "'";
            }


            int resultado = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            consulta = "SELECT * FROM amecs where idamecs='" + amec.idamecs + "'";
            ICollection<DAmecInfo> amecs = DAmecInfo.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));

            cambioAgencia = false;
            if (amecs.First().idconfempresa != agenciaAntigua)
            {
                cambioAgencia = true;
            }

            return amecs.ElementAt<DAmecInfo>(0);

        }
        public int CrearRelacionAmecCongreso(int idcongres, string idamecs, int idpeticionario)
        {
            string consulta = string.Empty;
            consulta = "SELECT count(*) FROM peticiones_actividad where IdCongreso is null and idpeticionactividad=" + idcongres + "";
            long esUnaPeticionActividad = long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));

            consulta = "SELECT MAX(idamec)  FROM amec";
            int IdamecactividadSiguiente = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));

            IdamecactividadSiguiente = IdamecactividadSiguiente + 1;

            string consultaIdConfEmpresa = string.Format("select idconfempresa from amecs where idamecs = '{0}'", idamecs);
            string idConfEmpresa = Quodem.Sql.SqlServerClient.GetValue(consultaIdConfEmpresa);
            if (string.IsNullOrWhiteSpace(idConfEmpresa))
            {
                idConfEmpresa = "null";
            }

            string consultaNewCo = string.Format("select * from amecs where idamecs = '{0}'", idamecs);
            var dtTable = Quodem.Sql.SqlServerClient.GetQuery(consultaNewCo);
            var newco = false;
            foreach (DataRow row in dtTable.Rows)
            {
                newco = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "newco") != null ? Quodem.Utility.DataLayerUtil.GetBoolValue(row, "newco") : false;
            }
            string newcoStr = newco ? "1" : "0";

            if (Convert.ToInt32(esUnaPeticionActividad) == 0)
            {
                //Si entra aquí voldrà dir que es un congreso
                consulta = "INSERT INTO amec (idamec, amec, IdCongreso,idEmpresa, idconfempresa, newco) VALUES(" + IdamecactividadSiguiente + ",'" + idamecs + "'," + idcongres + "," + ConfigurationManager.AppSettings["IdEmpresa"] + "," + idConfEmpresa + "," + newcoStr + ")";
            }
            else
            {
                consulta = "INSERT INTO amec (idamec, amec, idpeticionactividad,idEmpresa, idconfempresa, newco) VALUES(" + IdamecactividadSiguiente + ",'" + idamecs + "'," + idcongres + "," + ConfigurationManager.AppSettings["IdEmpresa"] + "," + idConfEmpresa + "," + newcoStr + ")";
                //Si entra aquí serà una peticio de actividad
            }

            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

        }
        public bool EstaGuardadoAmec(string idamec)
        {
            string consulta = string.Format("SELECT count(*) from amecs where idamecs = '{0}'", idamec);
            long numAmec = long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            return numAmec > 0;
        }

        public int EliminarRelacionAmecCongreso(int idameccongreso)
        {
            string consulta = string.Format("DELETE from amec where idamec ={0}", idameccongreso);
            Quodem.Sql.SqlServerClient.GetQuery(consulta);
            return 1;
        }


        public ICollection<DCongresos> ObtenerEventosNoRelacionadosAMEC(FiltroAMECCongreso filtroAMECCongreso)
        {
            ///Construir select para congresos
            string consulta = string.Format("select * from ({0} union {1}) tablaauxcongrepatic {2}", CrearSeccionFiltroPeticionesActividadNoAssigAMEC(filtroAMECCongreso.IdAMEC), CrearSeccionFiltroCongresosNoAssigAMEC(filtroAMECCongreso.IdAMEC), CrearSeccionWhereFiltroAMECCongreso(filtroAMECCongreso));

            if (filtroAMECCongreso.MaximumRows > 0 && filtroAMECCongreso.StartRowIndex > 0)
            {
                return DCongresos.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).Skip(filtroAMECCongreso.StartRowIndex.Value).Take(filtroAMECCongreso.MaximumRows.Value).ToList();
            }
            if (filtroAMECCongreso.StartRowIndex > 0)
            {
                return DCongresos.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).Skip(filtroAMECCongreso.StartRowIndex.Value).ToList();
            }
            DataTable dt = Quodem.Sql.SqlServerClient.GetQuery(consulta);
            return DCongresos.ConvertToDto(dt);
        }


        public bool ComprobarSiSeEnvioAFarma(string idamec)
        {
            string consulta = string.Empty;
            consulta = string.Format("select farmaindustria from amecs where idamecs = '{0}'", idamec);

            return int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta)) == 1;
        }

        public bool ComprobarSiSeEnvioACasosClinicos(string idamec)
        {
            string consulta = string.Empty;
            consulta = string.Format("select casosclinicos from amecs where idamecs = '{0}'", idamec);

            return int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta)) == 1;
            
        }

        public int IdAmecAsignadoCongreso(string idamec, int idcongreso)
        {
            string consulta = string.Format("SELECT count(*) from amec amec INNER JOIN cv_actividades_congreso act on act.IdCongreso = amec.IdCongreso or act.IdCongreso = amec.idpeticionactividad where amec.amec = '{0}' and act.IdCongreso = '{1}'", idamec, idcongreso);

            long EstaAsignado = long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            if (EstaAsignado != 0)
            {
                consulta = string.Format("SELECT idamec from amec amec INNER JOIN cv_actividades_congreso act on act.IdCongreso = amec.IdCongreso or act.IdCongreso = amec.idpeticionactividad where amec.amec = '{0}' and act.IdCongreso = '{1}'", idamec, idcongreso);
                return int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));

            }
                return 0;
        }

        public int EventoYaEstaAsignadoAmec(string idamec, int idcongreso)
        {
            string consulta =
            string.Format("SELECT count(*) from amec amec INNER JOIN cv_actividades_congreso act on act.IdCongreso = amec.IdCongreso or act.IdCongreso = amec.idpeticionactividad where amec.amec = '{0}' and act.IdCongreso = '{1}'", idamec, idcongreso);
            return Convert.ToInt32(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }
        public ICollection<DCongresos> CargarEventoActividadesRelacionadasAmec(string idamec)
        {
            string consulta = string.Format("SELECT act.IdCongreso, act.Congreso, act.IdPoblacion, act.Poblacion, act.Desde, act.Hasta from amec amec INNER JOIN cv_actividades_congreso act on act.IdCongreso = amec.idcongreso where amec.amec = '{0}'", idamec);
            return DCongresos.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }


        public long ObtenerNumeroEventosNoRelacionadosAMEC(FiltroAMECCongreso filtroAMECCongreso)
        {
            string consulta = string.Format("select count(*) from ({0} union {1}) tablaauxcongrepatic {2}", CrearSeccionFiltroPeticionesActividadNoAssigAMEC(filtroAMECCongreso.IdAMEC), CrearSeccionFiltroCongresosNoAssigAMEC(filtroAMECCongreso.IdAMEC), CrearSeccionWhereFiltroAMECCongreso(filtroAMECCongreso));
            return long.Parse(ObtenerEventosNoRelacionadosAMEC(filtroAMECCongreso).Count.ToString());
            //return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            
        }
        
        public DataSet MailsAEnviarCuandoAprobado(string IdAMEC)
        {
            string consulta = string.Format("SELECT pet.email, hist.idamecs, pet.Nombre FROM histasocamecs hist inner join peticionarios pet on pet.idpeticionario = hist.idcreadopor where idestado >=21 and idestado <=26 and idamecs = '{0}'", IdAMEC);
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }


        public DataSet BUDdelaUnidad(string IdAMEC)
        {
            string consulta = string.Format("SELECT top 1 pet.email, dirunid.idunidad, dirunid.IdPeticionario " +
                                            "FROM amecs ams " +
                                            "inner join unidorganizamec unid on unid.idamecs = ams.idamecs " +
                                            "inner join directores_unidades dirunid on dirunid.idunidad = unid.idunidad " +
                                            "inner join peticionarios pet on pet.IdPeticionario = dirunid.IdPeticionario" +
                                            " where ams.idamecs= '{0}' ", IdAMEC);
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }

        public DataSet DataSetObtenerActividadesAsigAmec(string IdAMEC, string sortParameter, int startRowIndex, int maximumRows)
        {
            if (IdAMEC == "0" || string.IsNullOrWhiteSpace(IdAMEC))
            {
                return null;
            }
            else
            {
                string consulta = string.Format("SELECT act.IdCongreso, max(act.Congreso) as Congreso, max(act.IdPoblacion) as IdPoblacion, max(act.Poblacion) as Poblacion, max(act.Desde) as Desde, max(act.Hasta) as Hasta, max(amec.idamec) as idameccongreso from amec amec INNER JOIN cv_actividades_congreso act on act.IdCongreso = amec.IdCongreso or act.IdCongreso = amec.idpeticionactividad {0}", CrearSeccionFromFiltroActividadAsigAmec(IdAMEC, sortParameter, startRowIndex, maximumRows));
                DataSet dt = new DataSet();
                if (maximumRows > 0 && startRowIndex > 0)
                {
                    IEnumerable<DataRow> myDataPage =Quodem.Sql.SqlServerClient.GetQuery(consulta).AsEnumerable().Skip(startRowIndex).Take(maximumRows);
                    dt.Tables.Add(myDataPage.CopyToDataTable());
                }
                if (startRowIndex > 0)
                {
                    IEnumerable<DataRow> myDataPage = Quodem.Sql.SqlServerClient.GetQuery(consulta).AsEnumerable().Skip(startRowIndex);
                    dt.Tables.Add(myDataPage.CopyToDataTable());
                }

                dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
                
                return dt;
            }
        }

        public ICollection<DVCongresoAmec> ObtenerActividadesAsigAmec(string IdAMEC, string sortParameter, int startRowIndex, int maximumRows)
        {
            if (IdAMEC == "0" || string.IsNullOrWhiteSpace(IdAMEC))
            {
                return null;
            }
            else
            {
            string sort = string.IsNullOrEmpty(sortParameter) ? "1" : sortParameter;
            string consulta = "SELECT max(amec.idamec) as idamec, " + "act.IdCongreso as IdCongreso, " + "max(act.Congreso) as Congreso, " + "max(act.IdPoblacion) as IdPoblacion, " +
                              " max(act.Poblacion) as Poblacion, " + "max(act.Desde) as Desde, " + "max(act.Hasta) as Hasta, " + "max(amec.idamec) as idameccongreso" +
                              " from amec amec " + "INNER JOIN cv_actividades_congreso act on " +
                              " act.IdCongreso = amec.IdCongreso or " + "act.IdCongreso = amec.idpeticionactividad " +
                              String.Format(" WHERE amec.amec = '{0}' GROUP BY act.idCongreso ORDER BY {1}", IdAMEC,
                                  sort);



                if ( maximumRows > 0 && startRowIndex > 0)
                {
                    return DVCongresoAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).Skip(startRowIndex).Take(maximumRows).ToList();
                }
                if (startRowIndex > 0)
                {
                    return DVCongresoAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).Skip(startRowIndex).ToList();
                }

                return DVCongresoAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            }
        }

        private string CrearSeccionFromFiltroActividadAsigAmec(string idamec, string sortParameter, int startRowIndex, int maximumRows)
        {
            StringBuilder db = new StringBuilder();
            bool primero = true;

            if (idamec != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" amec.amec = '{0}' group by act.IdCongreso", idamec);
            }
            if (!string.IsNullOrEmpty(sortParameter))
            {
                db.AppendFormat(" ORDER BY {0}", sortParameter);
            }
            return db.ToString();
        }

        public DVCongresoAmec ObtenerCongreso(int idcongreso)
        {
            string consulta = "SELECT act.IdCongreso, act.Congreso, act.IdPoblacion, act.Poblacion, act.Desde, act.Hasta, null as idameccongreso from cv_actividades_congreso act where IdCongreso =" + idcongreso + "";

            ICollection<DVCongresoAmec> CollCongresos = DVCongresoAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return CollCongresos.ElementAt<DVCongresoAmec>(0);
        }

        public long ObtenerNumeroActividadesAsigAmec(string IdAMEC)
        {
            string consulta = string.Format("SELECT count(*) from amec amec INNER JOIN cv_actividades_congreso act on act.IdCongreso = amec.IdCongreso or act.IdCongreso = amec.idpeticionactividad where amec.amec = '{0}'", IdAMEC);
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }


        public string ObtenerNombreEstado(UInt32 idestado)
        {
            //OBTENER NOMBRE ESTADO
            string consulta = "SELECT estado from estadoamecs where idestado=" + idestado;
            return Quodem.Sql.SqlServerClient.GetValue(consulta);
        }

        //Este se va a borrar
        public int GuardarEstadoAHistorialAMEC(string idamec, int idestado, int idcreadopor, DAmecInfo miAmec)
        {
            string consulta = string.Empty;
            if (idestado == 29)
            {
                consulta = "INSERT INTO histasocamecs (idamecs, idestado, idcreadopor, fechacreacion, idnivelaprobacion, importe, farmaindustria, casosclinicos) VALUES('" + idamec + "',29," + idcreadopor + ",CURRENT_TIMESTAMP,1," + miAmec.importegasto.ToString().Replace(",", ".") + "," + miAmec.farmaindustria.Value + "," + miAmec.casosclinicos.Value + ")";
                return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
            }
            else
            {
                consulta = "INSERT INTO histasocamecs (idamecs, idestado, idcreadopor, fechacreacion, idnivelaprobacion) VALUES('" + idamec + "'," + idestado + "," + idcreadopor + ",CURRENT_TIMESTAMP,1)";
                return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
            }
        }
        //Se va a borrar, dado que necesitamos uno nuevo para cuando se integre con el nuevo formato
        public ICollection<DHistEstadosAMEC> ObtenerHistorialEstadosAMEC(string idamec, string sortParameter, int startRowIndex, int maximumRows)
        {
            //OBTENER HISTORIAL ESTADOS AMEC2
            string consulta = "SELECT hist.idestadoamechist, hist.idamecs, hist.idestado, estado.estado, hist.idnivelaprobacion, np.nivelaprobacion, hist.comentariosaprob, hist.idcreadopor, " +
                              "hist.fechacreacion, positions.position as cargo, rtrim(ltrim(isnull(pet.Nombre, '') + ' ' + isnull(pet.Apellido1, ''))) as nombreusuario, rtrim(ltrim(isnull(pethist.Nombre, '') + ' ' + isnull(pethist.Apellido1, ''))) as nombreusuariopendiente FROM histasocamecs hist " +
                              "INNER JOIN nivelesaprobacion np ON np.idnivelaprobacion=hist.idnivelaprobacion " +
                              "INNER JOIN peticionarios pet ON pet.idpeticionario=hist.idcreadopor " +
                              "LEFT JOIN positions positions ON positions.idposition=pet.idposition " +
                              "INNER JOIN estadoamecs estado ON estado.idestado=hist.idestado " +
                              "LEFT JOIN peticionarios pethist ON pethist.idpeticionario=hist.idaprobador " +
                              "where hist.idamecs='" + idamec + "' order by hist.fechacreacion, hist.idestadoamechist" + CrearSeccionHistorialAmec(sortParameter, startRowIndex, maximumRows);


            if (maximumRows > 0 && startRowIndex > 0)
            {
                return DHistEstadosAMEC.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).Skip(startRowIndex).Take(maximumRows).ToList();
            }
            if (startRowIndex > 0)
            {
                return DHistEstadosAMEC.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).Skip(startRowIndex).ToList();
            }
            return DHistEstadosAMEC.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }


        public long ObtenerNumeroHistorialEstadosAMEC(string idamec)
        {
            string consulta = "SELECT count(*) FROM histasocamecs  where idamecs='" + idamec + "'";
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }

        //Se va a borrar, se ha hecho la logica en GestionPermisos de BussinesLogic
        public ICollection<String> ObtenerCorreoParaEnviar(string idAMEC, string connectionString)
        {
            string _connectionString = "";
            _connectionString = connectionString;
            ICollection<String> Correos = null;
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand();
            SqlConnection conx = new SqlConnection();
            SqlDataAdapter addapter = new SqlDataAdapter();

            try
            {
                command.CommandText = "amecNotificaciones";
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = conx;
                conx.ConnectionString = _connectionString;

                command.Parameters.Add("in_idamec", SqlDbType.VarChar, 128);
                conx.Open();
                command.Parameters["in_idamec"].Value = idAMEC;

                SqlDataAdapter slData = new SqlDataAdapter(command);
                slData.Fill(ds);

                if (ds.Tables.Count != 0)
                {
                    foreach (DataRow fila in ds.Tables[0].Rows)
                    {
                        Correos.Add(fila.ToString());
                    }
                }

            }
            catch (SqlException sqle)
            {
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", sqle.Message, sqle);
            }
            finally
            {
                conx.Close();
            }
            return Correos;
        }

        #region SeccionWhereFiltros

        private string CrearSeccionOrderbyDocumentacion(string sortParameter, int startRowIndex, int maximumRows)
        {
            StringBuilder db = new StringBuilder();

            try
            {
                if (!string.IsNullOrEmpty(sortParameter))
                {
                    if (sortParameter == "USUARIO") db.AppendFormat(" ORDER BY nombreusuario");
                    else if (sortParameter == "CATEGORIA") db.AppendFormat(" ORDER BY catdoc.categoriadocumento DESC");

                    else db.AppendFormat(" ORDER BY docas.{0}", sortParameter);
                }
                else
                {
                    db.AppendFormat(" ORDER BY docas.fechacreacion DESC");
                }
            }
            catch (Exception)
            {

                throw;
            }
            return db.ToString();
        }

        private string CrearSeccionFiltroPeticionesActividadNoAssigAMEC(string idamec)
        {
            StringBuilder db = new StringBuilder();
            db.AppendFormat("select pet.* from amec amec right join cv_actividades_congreso_aux1 pet on pet.idcongreso = amec.idpeticionactividad where (amec.amec != '{0}' or amec.amec is null) ", idamec);
            return db.ToString();
        }

        private string CrearSeccionFiltroCongresosNoAssigAMEC(string idamec)
        {
            StringBuilder db = new StringBuilder();
            db.AppendFormat("select cong.* from amec amecaux2 right join cv_actividades_congreso_aux2 cong on cong.idcongreso = amecaux2.idcongreso where (amecaux2.amec != '{0}' or amecaux2.amec is null) ", idamec);
            return db.ToString();
        }

        //Borrar
        private string CrearSeccionHistorialAmec(string sortParameter, int startRowIndex, int maximumRows)
        {
            StringBuilder db = new StringBuilder();

            //if (maximumRows != null && maximumRows > 0)
            //{
            //    db.AppendFormat(" LIMIT {0}", maximumRows);
            //}
            //if (startRowIndex != null && startRowIndex > 0)
            //{
            //    db.AppendFormat(" OFFSET {0}", startRowIndex);
            //}


            return db.ToString();
        }

        private string CrearSeccionWhereFiltroListadoAmec(FiltroListadoAMECs filtroListadoAmec, DVPeticionariosRoles datosRoles)
        {

            if (filtroListadoAmec == null) return null;

            //Devuelve todos los ids de los amec que el peticionario tiene permisos para visualizar (en funcion de la jerarquia)
            var listIdAmecs = _session.GetNamedQuery("SpObtenerAmecsPuedoVer").SetInt32("idPeticionario", filtroListadoAmec.IdPeticionarioSession).SetInt32("isCreateMode", 0).List<int>();

            StringBuilder db = new StringBuilder();
            bool primero = true;

            if (filtroListadoAmec.XecUnidades == true)
            {
                string UnidadesFiltro = ObtenerFiltroUnidades(filtroListadoAmec);
                if (UnidadesFiltro != null)
                {
                    db.AppendFormat(UnidadesFiltro);
                }
            }

            if (filtroListadoAmec.XecUnidades == false)
            {
                if (filtroListadoAmec.Roles != null)
                {
                    db.AppendFormat(filtroListadoAmec.Roles);
                }
            }

            if (filtroListadoAmec.ProductoLike != null && filtroListadoAmec.ProductoLike != "")
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.cargoadaxas like '{0}'", filtroListadoAmec.ProductoLike);
            }

            if (filtroListadoAmec.AMECLike != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.idamecs like '{0}'", filtroListadoAmec.AMECLike);
            }
            if (filtroListadoAmec.idsolicitante != null && filtroListadoAmec.idsolicitante != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.IdPeticionario = {0}", filtroListadoAmec.idsolicitante);
            }
            if (!string.IsNullOrEmpty(filtroListadoAmec.IdDepartament))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.iddepartament = {0}", filtroListadoAmec.IdDepartament);
            }
            if (!string.IsNullOrEmpty(filtroListadoAmec.IdSaleForce))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.idsaleforce = {0}", filtroListadoAmec.IdSaleForce);
            }
            if (!string.IsNullOrEmpty(filtroListadoAmec.IdDistrict))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.iddistrict = {0}", filtroListadoAmec.IdDistrict);
            }
            if (filtroListadoAmec.NombrePrograma != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.descripcion like '{0}'", filtroListadoAmec.NombrePrograma);
            }
            if (filtroListadoAmec.idestadoamec != null && filtroListadoAmec.idestadoamec != -1)
            {
                if (filtroListadoAmec.idestadoamec == 2)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado IN (27,39,41,39,43,45) OR (ams.idestado >=6 and ams.idestado <=18))");
                }
                else if (filtroListadoAmec.idestadoamec == 6)
                {
                    string idregion, idarea, iddistrito, idunidad;
                    AgregarAndSiProcede(db, ref primero);
                    if (filtroListadoAmec.IdRegionUsuarioConectado == null) idregion = "null";
                    else idregion = filtroListadoAmec.IdRegionUsuarioConectado.ToString();
                    if (filtroListadoAmec.IdAreaUsuarioConectado == null) idarea = "null";
                    else idarea = filtroListadoAmec.IdAreaUsuarioConectado.ToString();
                    if (filtroListadoAmec.IdDistritoUsuarioConectado == null) iddistrito = "null";
                    else iddistrito = filtroListadoAmec.IdDistritoUsuarioConectado.ToString();
                    if (filtroListadoAmec.IdUnidadUsuarioConectado == null) idunidad = "null";
                    else idunidad = filtroListadoAmec.IdUnidadUsuarioConectado.ToString();
                    string IdCargoUsuarioConectado = filtroListadoAmec.IdCargoUsuarioConectado == null ? "null" : filtroListadoAmec.IdCargoUsuarioConectado.ToString();
                    db.Append("(amecAutorizar(" + filtroListadoAmec.IdPeticionarioUsuarioConectado + ", ams.idsolicitante ," + idunidad + "," + idarea + "," + idregion + "," + iddistrito + "," + IdCargoUsuarioConectado + ", ams.idestado, unid.idunidad, unid.idarea, unid.idregion, unid.iddistrito, ams.idnivelaprobacion, ams.idamecs))");

                    

                    // Segun la organizacion nueva
                    if (datosRoles.medico.Value)
                    {
                        AgregarAndSiProcede(db, ref primero);
                        db.Append("(ams.idestado = 6)");
                    }
                    else if (datosRoles.legal.Value)
                    {
                        AgregarAndSiProcede(db, ref primero);
                        db.Append("(ams.idestado = 14)");
                    }
                    else
                    {
                        RepositorioAprobadorAMEC repo = new RepositorioAprobadorAMEC();
                        if (listIdAmecs == null) listIdAmecs = new List<int>();
                        ((List<int>)listIdAmecs).AddRange(repo.ObtenerAmecsDeLosQueSoyAprobador(datosRoles.IdPeticionario));
                    }
                }
                else
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.idestado = {0}", filtroListadoAmec.idestadoamec);
                }

            }
            if (filtroListadoAmec.paraguas != null && filtroListadoAmec.paraguas != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.paraguas = {0}", filtroListadoAmec.paraguas);
            }
            if (filtroListadoAmec.estadoAprobacion != null && filtroListadoAmec.estadoAprobacion != -1)
            {
                if (filtroListadoAmec.estadoAprobacion == 1)
                {
                    //Pendiente aprobar Médico
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.preaprobadamed = false");
                }

                if (filtroListadoAmec.estadoAprobacion == 2)
                {
                    //Pendiente aprobar Negocio
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.preaprobadaneg = false");
                }

                if (filtroListadoAmec.estadoAprobacion == 3)
                {
                    //Pendiente aprobar Legal Compliance
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.preaprobadaleg = false");
                }

            }
            if (filtroListadoAmec.year != null && filtroListadoAmec.year != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" YEAR(fechaamecs) = {0}", filtroListadoAmec.year);
            }
            if (filtroListadoAmec.mes != null && filtroListadoAmec.mes != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" MONTH(fechaamecs) = {0}", filtroListadoAmec.mes);
            }

            if (filtroListadoAmec.Importe != null)
            {
                AgregarAndSiProcede(db, ref primero);
                string sCantidadSpa = string.Format(" ams.importegasto {0} {1}", filtroListadoAmec.TipoFiltroImporte, filtroListadoAmec.Importe);
                sCantidadSpa = sCantidadSpa.Replace(',', '.');
                db.AppendFormat(" {0}", sCantidadSpa);
            }
            if (listIdAmecs.Count > 0)
            {
                string filtroIdAmecs = "('";
                foreach (var idamec in listIdAmecs)
                {
                    filtroIdAmecs += listIdAmecs.Count > listIdAmecs.IndexOf(idamec) + 1 ? idamec + "','" : idamec + "')";
                }
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.idamecs IN {0} ", filtroIdAmecs);
            }
            if (!string.IsNullOrEmpty(filtroListadoAmec.SortParameter))
            {
                if (filtroListadoAmec.SortParameter == "ESTADO") db.AppendFormat(" ORDER BY est.estado");
                else if (filtroListadoAmec.SortParameter == "ESTADO DESC") db.AppendFormat(" ORDER BY est.estado DESC");

                else db.AppendFormat(" ORDER BY ams.{0}", filtroListadoAmec.SortParameter);
            }
            else
            {
                db.AppendFormat(" ORDER BY ams.fechaamecs DESC,ams.idamecs DESC");
            }

            return db.ToString();
        }

        private string CrearSeccionWhereFiltroListadoAmecProcedure(FiltroListadoAMECs filtroListadoAmec, DVPeticionariosRoles datosRoles, bool count = false,bool nuevo = false)
        {
            string idregion, idarea, iddistrito, idunidad;
            bool esdelegado;
            idregion = filtroListadoAmec.IdRegionUsuarioConectado == null ? "null" : filtroListadoAmec.IdRegionUsuarioConectado.ToString();
            idarea = filtroListadoAmec.IdAreaUsuarioConectado == null ? "null" : filtroListadoAmec.IdAreaUsuarioConectado.ToString();
            iddistrito = filtroListadoAmec.IdDistritoUsuarioConectado == null ? "null" : filtroListadoAmec.IdDistritoUsuarioConectado.ToString();
            idunidad = filtroListadoAmec.IdUnidadUsuarioConectado == null ? "null" : filtroListadoAmec.IdUnidadUsuarioConectado.ToString();
            esdelegado = datosRoles.delegado != null && datosRoles.delegado.Value;
            if (filtroListadoAmec == null) return null;


            string queryAmex = "select top 1 idpeticionario, idunidad, idarea, idregion, iddistrito, idcargo from peticionarios where login = 'AMEX_" + datosRoles.login + "'";
            string idunidadAmex = "null";
            string idareaAmex = "null";
            string idregionAmex = "null";
            string iddistritoAmex = "null";
            int idpeticionarioAmex = -1;
            int idcargoAmex = -1;
            DataTable amexTable = Quodem.Sql.SqlServerClient.GetQuery(queryAmex);
            foreach (DataRow row in amexTable.Rows)
            {
                idpeticionarioAmex = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idpeticionario");
                idcargoAmex = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcargo");
                string pidunidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idunidad");
                string pidarea = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idarea");
                string pidregion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idregion");
                string piddistrito = Quodem.Utility.DataLayerUtil.GetStringValue(row, "iddistrito");

                if (pidunidad != String.Empty)
                {
                    idunidadAmex = pidunidad;
                }
                else
                {
                    idunidadAmex = "null";
                }

                if (pidarea != String.Empty)
                {
                    idareaAmex = pidarea;
                }
                else
                {
                    idareaAmex = "null";
                }

                if (pidregion != String.Empty)
                {
                    idregionAmex = pidregion;
                }
                else
                {
                    idregionAmex = "null";
                }

                if (piddistrito != String.Empty)
                {
                    iddistritoAmex = piddistrito;
                }
                else
                {
                    iddistritoAmex = "null";
                }
            }

            //Devuelve todos los ids de los amec que el peticionario tiene permisos para visualizar (en funcion de la jerarquia)
            //var listIdAmecs = _session.GetNamedQuery("SpObtenerAmecsPuedoVer").SetInt32("idPeticionario", filtroListadoAmec.IdPeticionarioSession).List<int>();

            StringBuilder db = new StringBuilder();
            bool primero = true;
            string IdCargoUsuarioConectado = filtroListadoAmec.IdCargoUsuarioConectado == null ? "null" : filtroListadoAmec.IdCargoUsuarioConectado.ToString();
            if (!nuevo) { 
                if (filtroListadoAmec.XecUnidades == true )
                {
                    idregion = filtroListadoAmec.IdRegionFiltro == null ? "null" : filtroListadoAmec.IdRegionFiltro.ToString();
                    idarea = filtroListadoAmec.IdAreaFiltro == null ? "null" : filtroListadoAmec.IdAreaFiltro.ToString();
                    iddistrito = filtroListadoAmec.IdDistritoFiltro == null ? "null" : filtroListadoAmec.IdDistritoFiltro.ToString();
                    idunidad = filtroListadoAmec.IdUnidadFiltro == null ? "null" : filtroListadoAmec.IdUnidadFiltro.ToString();
                    db.AppendFormat(" where ( (dbo.listadoAmec(ams.idamecs," + filtroListadoAmec.IdPeticionarioUsuarioConectado + "," + idunidad + "," + idarea + "," + idregion + "," + iddistrito + "," + IdCargoUsuarioConectado + "," + (esdelegado ? "1":"0") + "," + (datosRoles.administrador == null? "0": datosRoles.administrador.Value? "1" : "0") + "," + (datosRoles.aprobador == null ? "0" : datosRoles.aprobador.Value ? "1" : "0") + ",ams.idsolicitante, ams.idcreadopor, ams.paraguas, 1) = 1 ) OR ((dbo.listadoAmec(ams.idamecs," + idpeticionarioAmex + "," + idunidadAmex + "," + idareaAmex + "," + idregionAmex + "," + iddistritoAmex + "," + idpeticionarioAmex + "," + (esdelegado ? "1" : "0") + "," + (datosRoles.administrador == null ? "0" : datosRoles.administrador.Value ? "1" : "0") + "," + (datosRoles.aprobador == null ? "0" : datosRoles.aprobador.Value ? "1" : "0") + ",ams.idsolicitante, ams.idcreadopor, ams.paraguas, 1) = 1 )))");

                }
                else db.AppendFormat(" where ( (dbo.listadoAmec(ams.idamecs," + filtroListadoAmec.IdPeticionarioUsuarioConectado + "," + idunidad + "," + idarea + "," + idregion + "," + iddistrito + "," + IdCargoUsuarioConectado + "," + (esdelegado ? "1" : "0") + "," + (datosRoles.administrador == null ? "0" : datosRoles.administrador.Value ? "1" : "0") + "," + (datosRoles.aprobador == null ? "0" : datosRoles.aprobador.Value ? "1" : "0") + ",ams.idsolicitante, ams.idcreadopor, ams.paraguas,0) = 1 ) OR ((dbo.listadoAmec(ams.idamecs," + idpeticionarioAmex + "," + idunidadAmex + "," + idareaAmex + "," + idregionAmex + "," + iddistritoAmex + "," + idpeticionarioAmex + "," + (esdelegado ? "1" : "0") + "," + (datosRoles.administrador == null ? "0" : datosRoles.administrador.Value ? "1" : "0") + "," + (datosRoles.aprobador == null ? "0" : datosRoles.aprobador.Value ? "1" : "0") + ",ams.idsolicitante, ams.idcreadopor, ams.paraguas,0) = 1 )))");
                primero = false;
            }

            if (filtroListadoAmec.AMECLike != null && nuevo)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.idamecs like '{0}'", filtroListadoAmec.AMECLike);
            }

            if (!string.IsNullOrEmpty(filtroListadoAmec.ProductoLike))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.cargoadaxas like '{0}'", filtroListadoAmec.ProductoLike);
            }

            if (filtroListadoAmec.idsolicitante != null && filtroListadoAmec.idsolicitante != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.IdPeticionario in ({0}, {1})", filtroListadoAmec.idsolicitante, idpeticionarioAmex);
            }

            if (filtroListadoAmec.AMECLike != null && !nuevo)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.idamecs like '{0}'", filtroListadoAmec.AMECLike);
            }
            if (filtroListadoAmec.NombrePrograma != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.descripcion like '{0}'", filtroListadoAmec.NombrePrograma.Replace("'", "''"));
            }

            if (!string.IsNullOrEmpty(filtroListadoAmec.FechaInicio))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.fechacomienzo >= '{0}'", filtroListadoAmec.FechaInicio);
            }

            if (!string.IsNullOrEmpty(filtroListadoAmec.FechaFin))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.fechafinalizacion <= '{0}'", filtroListadoAmec.FechaFin);
            }
            if (!string.IsNullOrEmpty(filtroListadoAmec.IdDepartament))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.iddepartament = {0}", filtroListadoAmec.IdDepartament);
            }
            if (!string.IsNullOrEmpty(filtroListadoAmec.IdSaleForce))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.idsaleforce = {0}", filtroListadoAmec.IdSaleForce);
            }
            if (!string.IsNullOrEmpty(filtroListadoAmec.IdDistrict))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.iddistrict = {0}", filtroListadoAmec.IdDistrict);
            }
            if (filtroListadoAmec.idestadoamec != null && filtroListadoAmec.idestadoamec != -1)
            {
                if (filtroListadoAmec.idestadoamec == 2)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado IN (27,39,41,39,43,45) OR (ams.idestado >=6 and ams.idestado <=18))");
                }
                else if (filtroListadoAmec.idestadoamec == 6)
                {

                    if (!nuevo)
                    {
                    // Segun la organizacion antigua. La necesitamos mantener para poder filtrar los viejos amec
                    AgregarAndSiProcede(db, ref primero);
                    idregion = filtroListadoAmec.IdRegionUsuarioConectado == null ? "null" : filtroListadoAmec.IdRegionUsuarioConectado.ToString();
                    idarea = filtroListadoAmec.IdAreaUsuarioConectado == null ? "null" : filtroListadoAmec.IdAreaUsuarioConectado.ToString();
                    iddistrito = filtroListadoAmec.IdDistritoUsuarioConectado == null ? "null" : filtroListadoAmec.IdDistritoUsuarioConectado.ToString();
                    idunidad = filtroListadoAmec.IdUnidadUsuarioConectado == null ? "null" : filtroListadoAmec.IdUnidadUsuarioConectado.ToString();

                        db.Append("( ( dbo.amecAutorizar(" + filtroListadoAmec.IdPeticionarioUsuarioConectado + ", ams.idsolicitante ," + idunidad + "," + idarea + "," + idregion + "," + iddistrito + "," + IdCargoUsuarioConectado + ", ams.idestado, unid.idunidad, unid.idarea, unid.idregion, unid.iddistrito, ams.idnivelaprobacion, ams.idamecs)=1) OR ( dbo.amecAutorizar(" + idpeticionarioAmex + ", ams.idsolicitante ," + idunidadAmex + "," + idareaAmex + "," + idregionAmex + "," + iddistritoAmex + "," + idcargoAmex + ", ams.idestado, unid.idunidad, unid.idarea, unid.idregion, unid.iddistrito, ams.idnivelaprobacion, ams.idamecs)=1))");
                    }
                   
                    // Segun la organizacion nueva
                    if (datosRoles.medico.Value)
                    {
                        AgregarAndSiProcede(db, ref primero);
                        //db.Append("(ams.idestado = 6)");
                        //Casos en los que los que son perfil médico y además pueden ser aprobadores de negocio
                        db.Append("(ams.idestado IN (6,8,10,39,41,43,45))");
                    }
                    else if(datosRoles.legal.Value)
                    {
                        AgregarAndSiProcede(db, ref primero);
                        //db.Append("(ams.idestado = 14)");
                        //Casos en los que los que son perfil legal y además pueden ser aprobadores de negocio
                        db.Append("(ams.idestado IN (8,10,14,39,41,43,45))");
                    }
                    else
                    {
                        AgregarAndSiProcede(db, ref primero);
                        db.Append("(ams.idestado IN (8,10,39,41,43,45)");
                        //if (nuevo)
                        //{
                        //    db.Append("AND ");
                        //    db.Append("		ams.idamecs not IN (SELECT hist.idamecs ");
                        //    db.Append("							from histasocamecs hist ");
                        //    db.Append("							INNER JOIN (SELECT idamecs, MAX(fechacreacion) AS MAX_DATE ");
                        //    db.Append("										FROM histasocamecs ");
                        //    db.Append("										WHERE idestado = 29 GROUP BY idamecs ) hist_max_date ");
                        //    db.Append("										ON  hist_max_date.idamecs = hist.idamecs AND hist.fechacreacion > hist_max_date.MAX_DATE ");
                        //    db.AppendFormat("										where hist.idcreadopor = {0} )", filtroListadoAmec.IdPeticionarioSession);
                        //    //AND hist.idestado not in (21,22,40, 42, 44, 46)
                        //}
                        db.Append(")");
                    }


                }
                else if (filtroListadoAmec.idestadoamec == 7)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.idestado = 35 and (ams.idsolicitante = {0} or ams.idcreadopor = {0})", datosRoles.IdPeticionario);
                }
                else if (filtroListadoAmec.idestadoamec == 8)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.idestado = 49 and (ams.idsolicitante = {0} or ams.idcreadopor = {0})", datosRoles.IdPeticionario);
                }
                else if (filtroListadoAmec.idestadoamec == 9)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.idestado = 50 and (ams.idsolicitante = {0} or ams.idcreadopor = {0})", datosRoles.IdPeticionario);
                }
                else
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.idestado = {0}", filtroListadoAmec.idestadoamec);
                }

            }
            if (filtroListadoAmec.paraguas != null && filtroListadoAmec.paraguas != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.paraguas = {0}", filtroListadoAmec.paraguas);
            }
            if (filtroListadoAmec.estadoAprobacion != null && filtroListadoAmec.estadoAprobacion != -1)
            {
                if (filtroListadoAmec.estadoAprobacion == 1)
                {
                    //Pendiente aprobar Médico
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.preaprobadamed = 0");
                }

                if (filtroListadoAmec.estadoAprobacion == 2)
                {
                    //Pendiente aprobar Negocio
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.preaprobadaneg = 0");
                }

                if (filtroListadoAmec.estadoAprobacion == 3)
                {
                    //Pendiente aprobar Legal Compliance
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.preaprobadaleg = 0");
                }

            }
            if (filtroListadoAmec.year != null && filtroListadoAmec.year != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" YEAR(fechaamecs) = {0}", filtroListadoAmec.year);
            }
            if (filtroListadoAmec.mes != null && filtroListadoAmec.mes != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" MONTH(fechaamecs) = {0}", filtroListadoAmec.mes);
            }

            if (filtroListadoAmec.Importe != null)
            {
                AgregarAndSiProcede(db, ref primero);
                string sCantidadSpa = string.Format(" ams.importegasto {0} {1}", filtroListadoAmec.TipoFiltroImporte, filtroListadoAmec.Importe);
                sCantidadSpa = sCantidadSpa.Replace(',', '.');
                db.AppendFormat(" {0}", sCantidadSpa);
            }



            //if (listIdAmecs.Count > 0)
            //{
            //    string filtroIdAmecs = "('";
            //    foreach (var idamec in listIdAmecs)
            //    {
            //        filtroIdAmecs += listIdAmecs.Count > listIdAmecs.IndexOf(idamec) + 1 ? idamec + "','" : idamec + "')";
            //    }
            //    AgregarAndSiProcede(db, ref primero);
            //    db.AppendFormat(" ams.idamecs IN {0} ", filtroIdAmecs);
            //}

            AgregarAndSiProcede(db, ref primero);
            db.Append(" ams.veeva = 0 and ams.newco = 0 ");

            if (!nuevo)
            {
                db.Append("  and ams.idamecs not like '4________'  ");
            }

            if (!count)
                db.Append(" GROUP BY ams.idamecs, ams.fechaamecs, ams.veeva, ams.newco ");

            

            if (!string.IsNullOrEmpty(filtroListadoAmec.SortParameter))
            {
                if (filtroListadoAmec.SortParameter == "ESTADO") db.AppendFormat(" ORDER BY est.estado");
                else if (filtroListadoAmec.SortParameter == "ESTADO DESC") db.AppendFormat(" ORDER BY est.estado DESC");

                else db.AppendFormat(" ORDER BY ams.{0}", filtroListadoAmec.SortParameter);
            }
            else
            {
                if (!count)
                {
                    db.AppendFormat(" ORDER BY ams.fechaamecs DESC,ams.idamecs DESC");
                }
            }
            return db.ToString();
        }

        private string CrearSeccionWhereFiltroListadoAmecProcedureNewCo(FiltroListadoAMECs filtroListadoAmec, DVPeticionariosRoles datosRoles, bool count = false, bool nuevo = false)
        {
            string idregion, idarea, iddistrito, idunidad;
            bool esdelegado;
            idregion = filtroListadoAmec.IdRegionUsuarioConectado == null ? "null" : filtroListadoAmec.IdRegionUsuarioConectado.ToString();
            idarea = filtroListadoAmec.IdAreaUsuarioConectado == null ? "null" : filtroListadoAmec.IdAreaUsuarioConectado.ToString();
            iddistrito = filtroListadoAmec.IdDistritoUsuarioConectado == null ? "null" : filtroListadoAmec.IdDistritoUsuarioConectado.ToString();
            idunidad = filtroListadoAmec.IdUnidadUsuarioConectado == null ? "null" : filtroListadoAmec.IdUnidadUsuarioConectado.ToString();
            esdelegado = datosRoles.delegado != null && datosRoles.delegado.Value;
            if (filtroListadoAmec == null) return null;


            string queryAmex = "select top 1 idpeticionario, idunidad, idarea, idregion, iddistrito, idcargo from peticionarios where login = 'AMEX_" + datosRoles.login + "'";
            string idunidadAmex = "null";
            string idareaAmex = "null";
            string idregionAmex = "null";
            string iddistritoAmex = "null";
            int idpeticionarioAmex = -1;
            int idcargoAmex = -1;
            DataTable amexTable = Quodem.Sql.SqlServerClient.GetQuery(queryAmex);
            foreach (DataRow row in amexTable.Rows)
            {
                idpeticionarioAmex = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idpeticionario");
                idcargoAmex = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcargo");
                string pidunidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idunidad");
                string pidarea = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idarea");
                string pidregion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idregion");
                string piddistrito = Quodem.Utility.DataLayerUtil.GetStringValue(row, "iddistrito");

                if (pidunidad != String.Empty)
                {
                    idunidadAmex = pidunidad;
                }
                else
                {
                    idunidadAmex = "null";
                }

                if (pidarea != String.Empty)
                {
                    idareaAmex = pidarea;
                }
                else
                {
                    idareaAmex = "null";
                }

                if (pidregion != String.Empty)
                {
                    idregionAmex = pidregion;
                }
                else
                {
                    idregionAmex = "null";
                }

                if (piddistrito != String.Empty)
                {
                    iddistritoAmex = piddistrito;
                }
                else
                {
                    iddistritoAmex = "null";
                }
            }

            //Devuelve todos los ids de los amec que el peticionario tiene permisos para visualizar (en funcion de la jerarquia)
            //var listIdAmecs = _session.GetNamedQuery("SpObtenerAmecsPuedoVer").SetInt32("idPeticionario", filtroListadoAmec.IdPeticionarioSession).List<int>();

            StringBuilder db = new StringBuilder();
            bool primero = true;
            string IdCargoUsuarioConectado = filtroListadoAmec.IdCargoUsuarioConectado == null ? "null" : filtroListadoAmec.IdCargoUsuarioConectado.ToString();
            if (!nuevo)
            {
                if (filtroListadoAmec.XecUnidades == true)
                {
                    idregion = filtroListadoAmec.IdRegionFiltro == null ? "null" : filtroListadoAmec.IdRegionFiltro.ToString();
                    idarea = filtroListadoAmec.IdAreaFiltro == null ? "null" : filtroListadoAmec.IdAreaFiltro.ToString();
                    iddistrito = filtroListadoAmec.IdDistritoFiltro == null ? "null" : filtroListadoAmec.IdDistritoFiltro.ToString();
                    idunidad = filtroListadoAmec.IdUnidadFiltro == null ? "null" : filtroListadoAmec.IdUnidadFiltro.ToString();
                    db.AppendFormat(" where ( (dbo.listadoAmec(ams.idamecs," + filtroListadoAmec.IdPeticionarioUsuarioConectado + "," + idunidad + "," + idarea + "," + idregion + "," + iddistrito + "," + IdCargoUsuarioConectado + "," + (esdelegado ? "1" : "0") + "," + (datosRoles.administrador == null ? "0" : datosRoles.administrador.Value ? "1" : "0") + "," + (datosRoles.aprobador == null ? "0" : datosRoles.aprobador.Value ? "1" : "0") + ",ams.idsolicitante, ams.idcreadopor, ams.paraguas, 1) = 1 ) OR ((dbo.listadoAmec(ams.idamecs," + idpeticionarioAmex + "," + idunidadAmex + "," + idareaAmex + "," + idregionAmex + "," + iddistritoAmex + "," + idpeticionarioAmex + "," + (esdelegado ? "1" : "0") + "," + (datosRoles.administrador == null ? "0" : datosRoles.administrador.Value ? "1" : "0") + "," + (datosRoles.aprobador == null ? "0" : datosRoles.aprobador.Value ? "1" : "0") + ",ams.idsolicitante, ams.idcreadopor, ams.paraguas, 1) = 1 )))");

                }
                else db.AppendFormat(" where ( (dbo.listadoAmec(ams.idamecs," + filtroListadoAmec.IdPeticionarioUsuarioConectado + "," + idunidad + "," + idarea + "," + idregion + "," + iddistrito + "," + IdCargoUsuarioConectado + "," + (esdelegado ? "1" : "0") + "," + (datosRoles.administrador == null ? "0" : datosRoles.administrador.Value ? "1" : "0") + "," + (datosRoles.aprobador == null ? "0" : datosRoles.aprobador.Value ? "1" : "0") + ",ams.idsolicitante, ams.idcreadopor, ams.paraguas,0) = 1 ) OR ((dbo.listadoAmec(ams.idamecs," + idpeticionarioAmex + "," + idunidadAmex + "," + idareaAmex + "," + idregionAmex + "," + iddistritoAmex + "," + idpeticionarioAmex + "," + (esdelegado ? "1" : "0") + "," + (datosRoles.administrador == null ? "0" : datosRoles.administrador.Value ? "1" : "0") + "," + (datosRoles.aprobador == null ? "0" : datosRoles.aprobador.Value ? "1" : "0") + ",ams.idsolicitante, ams.idcreadopor, ams.paraguas,0) = 1 )))");
                primero = false;
            }

            if (filtroListadoAmec.AMECLike != null && nuevo)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.idamecs like '{0}'", filtroListadoAmec.AMECLike);
            }

            if (!string.IsNullOrEmpty(filtroListadoAmec.ProductoLike))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.cargoadaxas like '{0}'", filtroListadoAmec.ProductoLike);
            }

            if (filtroListadoAmec.idsolicitante != null && filtroListadoAmec.idsolicitante != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.IdPeticionario in ({0}, {1})", filtroListadoAmec.idsolicitante, idpeticionarioAmex);
            }

            if (filtroListadoAmec.AMECLike != null && !nuevo)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.idamecs like '{0}'", filtroListadoAmec.AMECLike);
            }
            if (filtroListadoAmec.NombrePrograma != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.descripcion like '{0}'", filtroListadoAmec.NombrePrograma.Replace("'", "''"));
            }

            if (!string.IsNullOrEmpty(filtroListadoAmec.FechaInicio))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.fechacomienzo >= '{0}'", filtroListadoAmec.FechaInicio);
            }

            if (!string.IsNullOrEmpty(filtroListadoAmec.FechaFin))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.fechafinalizacion <= '{0}'", filtroListadoAmec.FechaFin);
            }
            if (!string.IsNullOrEmpty(filtroListadoAmec.IdDepartament))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.iddepartament = {0}", filtroListadoAmec.IdDepartament);
            }
            if (!string.IsNullOrEmpty(filtroListadoAmec.IdSaleForce))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.idsaleforce = {0}", filtroListadoAmec.IdSaleForce);
            }
            if (!string.IsNullOrEmpty(filtroListadoAmec.IdDistrict))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.iddistrict = {0}", filtroListadoAmec.IdDistrict);
            }
            if (filtroListadoAmec.idestadoamec != null && filtroListadoAmec.idestadoamec != -1)
            {
                //Aprobado
                if (filtroListadoAmec.idestadoamec == 1)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 47 )");
                }
                //Pendiente de Aprobar
                else if (filtroListadoAmec.idestadoamec == 2)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 999 )");
                }
                //Cancelado
                else if (filtroListadoAmec.idestadoamec == 3)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 48 )");
                }
                //Rechazado
                else if (filtroListadoAmec.idestadoamec == 4)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 999 )");
                }
                //Borrador
                else if (filtroListadoAmec.idestadoamec == 5)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 5 )");
                }
                //Pendiente de Aprobar por mi
                else if (filtroListadoAmec.idestadoamec == 6)
                {
                    //No pueden haber pendientes de aprobar por mi en veeva
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 999 )");
                }
                //Pendiente de Someter
                else if (filtroListadoAmec.idestadoamec == 7)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 999 )");
                }
                //Cerrado
                else if (filtroListadoAmec.idestadoamec == 8)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 49 )");
                }
                //Completado
                else if (filtroListadoAmec.idestadoamec == 9)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 50 )");
                }
                else
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.idestado = {0}", filtroListadoAmec.idestadoamec);
                }

            }
            if (filtroListadoAmec.paraguas != null && filtroListadoAmec.paraguas != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.paraguas = {0}", filtroListadoAmec.paraguas);
            }
            if (filtroListadoAmec.estadoAprobacion != null && filtroListadoAmec.estadoAprobacion != -1)
            {
                if (filtroListadoAmec.estadoAprobacion == 1)
                {
                    //Pendiente aprobar Médico
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.preaprobadamed = 0");
                }

                if (filtroListadoAmec.estadoAprobacion == 2)
                {
                    //Pendiente aprobar Negocio
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.preaprobadaneg = 0");
                }

                if (filtroListadoAmec.estadoAprobacion == 3)
                {
                    //Pendiente aprobar Legal Compliance
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.preaprobadaleg = 0");
                }

            }
            if (filtroListadoAmec.year != null && filtroListadoAmec.year != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" YEAR(fechaamecs) = {0}", filtroListadoAmec.year);
            }
            if (filtroListadoAmec.mes != null && filtroListadoAmec.mes != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" MONTH(fechaamecs) = {0}", filtroListadoAmec.mes);
            }

            if (filtroListadoAmec.Importe != null)
            {
                AgregarAndSiProcede(db, ref primero);
                string sCantidadSpa = string.Format(" ams.importegasto {0} {1}", filtroListadoAmec.TipoFiltroImporte, filtroListadoAmec.Importe);
                sCantidadSpa = sCantidadSpa.Replace(',', '.');
                db.AppendFormat(" {0}", sCantidadSpa);
            }



            //if (listIdAmecs.Count > 0)
            //{
            //    string filtroIdAmecs = "('";
            //    foreach (var idamec in listIdAmecs)
            //    {
            //        filtroIdAmecs += listIdAmecs.Count > listIdAmecs.IndexOf(idamec) + 1 ? idamec + "','" : idamec + "')";
            //    }
            //    AgregarAndSiProcede(db, ref primero);
            //    db.AppendFormat(" ams.idamecs IN {0} ", filtroIdAmecs);
            //}

            AgregarAndSiProcede(db, ref primero);
            db.Append(" ams.newco = 1 ");

            if (!count)
                db.Append(" GROUP BY ams.idamecs, ams.fechaamecs, ams.veeva, ams.newco ");



            if (!string.IsNullOrEmpty(filtroListadoAmec.SortParameter))
            {
                if (filtroListadoAmec.SortParameter == "ESTADO") db.AppendFormat(" ORDER BY est.estado");
                else if (filtroListadoAmec.SortParameter == "ESTADO DESC") db.AppendFormat(" ORDER BY est.estado DESC");

                else db.AppendFormat(" ORDER BY ams.{0}", filtroListadoAmec.SortParameter);
            }
            else
            {
                if (!count)
                {
                    db.AppendFormat(" ORDER BY ams.fechaamecs DESC,ams.idamecs DESC");
                }
            }
            return db.ToString();
        }

        private string CrearSeccionWhereFiltroListadoAmecProcedureVeeva(FiltroListadoAMECs filtroListadoAmec, DVPeticionariosRoles datosRoles, bool count = false, bool nuevo = false)
        {
            string idregion, idarea, iddistrito, idunidad;
            bool esdelegado;
            idregion = filtroListadoAmec.IdRegionUsuarioConectado == null ? "null" : filtroListadoAmec.IdRegionUsuarioConectado.ToString();
            idarea = filtroListadoAmec.IdAreaUsuarioConectado == null ? "null" : filtroListadoAmec.IdAreaUsuarioConectado.ToString();
            iddistrito = filtroListadoAmec.IdDistritoUsuarioConectado == null ? "null" : filtroListadoAmec.IdDistritoUsuarioConectado.ToString();
            idunidad = filtroListadoAmec.IdUnidadUsuarioConectado == null ? "null" : filtroListadoAmec.IdUnidadUsuarioConectado.ToString();
            esdelegado = datosRoles.delegado != null && datosRoles.delegado.Value;
            if (filtroListadoAmec == null) return null;


            string queryAmex = "select top 1 idpeticionario, idunidad, idarea, idregion, iddistrito, idcargo from peticionarios where login = 'AMEX_" + datosRoles.login + "'";
            string idunidadAmex = "null";
            string idareaAmex = "null";
            string idregionAmex = "null";
            string iddistritoAmex = "null";
            int idpeticionarioAmex = -1;
            int idcargoAmex = -1;
            DataTable amexTable = Quodem.Sql.SqlServerClient.GetQuery(queryAmex);
            foreach (DataRow row in amexTable.Rows)
            {
                idpeticionarioAmex = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idpeticionario");
                idcargoAmex = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcargo");
                string pidunidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idunidad");
                string pidarea = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idarea");
                string pidregion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idregion");
                string piddistrito = Quodem.Utility.DataLayerUtil.GetStringValue(row, "iddistrito");

                if (pidunidad != String.Empty)
                {
                    idunidadAmex = pidunidad;
                }
                else
                {
                    idunidadAmex = "null";
                }

                if (pidarea != String.Empty)
                {
                    idareaAmex = pidarea;
                }
                else
                {
                    idareaAmex = "null";
                }

                if (pidregion != String.Empty)
                {
                    idregionAmex = pidregion;
                }
                else
                {
                    idregionAmex = "null";
                }

                if (piddistrito != String.Empty)
                {
                    iddistritoAmex = piddistrito;
                }
                else
                {
                    iddistritoAmex = "null";
                }
            }

            //Devuelve todos los ids de los amec que el peticionario tiene permisos para visualizar (en funcion de la jerarquia)
            //var listIdAmecs = _session.GetNamedQuery("SpObtenerAmecsPuedoVer").SetInt32("idPeticionario", filtroListadoAmec.IdPeticionarioSession).List<int>();

            StringBuilder db = new StringBuilder();
            bool primero = true;
            string IdCargoUsuarioConectado = filtroListadoAmec.IdCargoUsuarioConectado == null ? "null" : filtroListadoAmec.IdCargoUsuarioConectado.ToString();
            if (!nuevo)
            {
                if (filtroListadoAmec.XecUnidades == true)
                {
                    idregion = filtroListadoAmec.IdRegionFiltro == null ? "null" : filtroListadoAmec.IdRegionFiltro.ToString();
                    idarea = filtroListadoAmec.IdAreaFiltro == null ? "null" : filtroListadoAmec.IdAreaFiltro.ToString();
                    iddistrito = filtroListadoAmec.IdDistritoFiltro == null ? "null" : filtroListadoAmec.IdDistritoFiltro.ToString();
                    idunidad = filtroListadoAmec.IdUnidadFiltro == null ? "null" : filtroListadoAmec.IdUnidadFiltro.ToString();
                    db.AppendFormat(" where ( (dbo.listadoAmec(ams.idamecs," + filtroListadoAmec.IdPeticionarioUsuarioConectado + "," + idunidad + "," + idarea + "," + idregion + "," + iddistrito + "," + IdCargoUsuarioConectado + "," + (esdelegado ? "1" : "0") + "," + (datosRoles.administrador == null ? "0" : datosRoles.administrador.Value ? "1" : "0") + "," + (datosRoles.aprobador == null ? "0" : datosRoles.aprobador.Value ? "1" : "0") + ",ams.idsolicitante, ams.idcreadopor, ams.paraguas, 1) = 1 ) OR ((dbo.listadoAmec(ams.idamecs," + idpeticionarioAmex + "," + idunidadAmex + "," + idareaAmex + "," + idregionAmex + "," + iddistritoAmex + "," + idpeticionarioAmex + "," + (esdelegado ? "1" : "0") + "," + (datosRoles.administrador == null ? "0" : datosRoles.administrador.Value ? "1" : "0") + "," + (datosRoles.aprobador == null ? "0" : datosRoles.aprobador.Value ? "1" : "0") + ",ams.idsolicitante, ams.idcreadopor, ams.paraguas, 1) = 1 )))");

                }
                else db.AppendFormat(" where ( (dbo.listadoAmec(ams.idamecs," + filtroListadoAmec.IdPeticionarioUsuarioConectado + "," + idunidad + "," + idarea + "," + idregion + "," + iddistrito + "," + IdCargoUsuarioConectado + "," + (esdelegado ? "1" : "0") + "," + (datosRoles.administrador == null ? "0" : datosRoles.administrador.Value ? "1" : "0") + "," + (datosRoles.aprobador == null ? "0" : datosRoles.aprobador.Value ? "1" : "0") + ",ams.idsolicitante, ams.idcreadopor, ams.paraguas,0) = 1 ) OR ((dbo.listadoAmec(ams.idamecs," + idpeticionarioAmex + "," + idunidadAmex + "," + idareaAmex + "," + idregionAmex + "," + iddistritoAmex + "," + idpeticionarioAmex + "," + (esdelegado ? "1" : "0") + "," + (datosRoles.administrador == null ? "0" : datosRoles.administrador.Value ? "1" : "0") + "," + (datosRoles.aprobador == null ? "0" : datosRoles.aprobador.Value ? "1" : "0") + ",ams.idsolicitante, ams.idcreadopor, ams.paraguas,0) = 1 )))");
                primero = false;
            }

            if (filtroListadoAmec.AMECLike != null && nuevo)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.idamecs like '{0}'", filtroListadoAmec.AMECLike);
            }

            if (!string.IsNullOrEmpty(filtroListadoAmec.ProductoLike))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.cargoadaxas like '{0}'", filtroListadoAmec.ProductoLike);
            }

            if (filtroListadoAmec.idsolicitante != null && filtroListadoAmec.idsolicitante != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.IdPeticionario in ({0}, {1})", filtroListadoAmec.idsolicitante, idpeticionarioAmex);
            }

            if (filtroListadoAmec.AMECLike != null && !nuevo)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.idamecs like '{0}'", filtroListadoAmec.AMECLike);
            }
            if (filtroListadoAmec.NombrePrograma != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.descripcion like '{0}'", filtroListadoAmec.NombrePrograma.Replace("'", "''"));
            }

            if (!string.IsNullOrEmpty(filtroListadoAmec.FechaInicio))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.fechacomienzo >= '{0}'", filtroListadoAmec.FechaInicio);
            }

            if (!string.IsNullOrEmpty(filtroListadoAmec.FechaFin))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.fechafinalizacion <= '{0}'", filtroListadoAmec.FechaFin);
            }
            if (!string.IsNullOrEmpty(filtroListadoAmec.IdDepartament))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.iddepartament = {0}", filtroListadoAmec.IdDepartament);
            }
            if (!string.IsNullOrEmpty(filtroListadoAmec.IdSaleForce))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.idsaleforce = {0}", filtroListadoAmec.IdSaleForce);
            }
            if (!string.IsNullOrEmpty(filtroListadoAmec.IdDistrict))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pet.iddistrict = {0}", filtroListadoAmec.IdDistrict);
            }
            if (filtroListadoAmec.idestadoamec != null && filtroListadoAmec.idestadoamec != -1)
            {
                //Aprobado
                if (filtroListadoAmec.idestadoamec == 1)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 47 )");
                }
                //Pendiente de Aprobar
                else if (filtroListadoAmec.idestadoamec == 2)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 999 )");
                }
                //Cancelado
                else if (filtroListadoAmec.idestadoamec == 3)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 48 )");
                }
                //Rechazado
                else if (filtroListadoAmec.idestadoamec == 4)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 999 )");
                }
                //Borrador
                else if (filtroListadoAmec.idestadoamec == 5)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 999 )");
                }
                //Pendiente de Aprobar por mi
                else if (filtroListadoAmec.idestadoamec == 6)
                {
                    //No pueden haber pendientes de aprobar por mi en veeva
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 999 )");
                }
                //Pendiente de Someter
                else if (filtroListadoAmec.idestadoamec == 7)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 999 )");
                }
                //Cerrado
                else if (filtroListadoAmec.idestadoamec == 8)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 49 )");
                }
                //Completado
                else if (filtroListadoAmec.idestadoamec == 9)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (ams.idestado = 50 )");
                }
                else
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.idestado = {0}", filtroListadoAmec.idestadoamec);
                }

            }
            if (filtroListadoAmec.paraguas != null && filtroListadoAmec.paraguas != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ams.paraguas = {0}", filtroListadoAmec.paraguas);
            }
            if (filtroListadoAmec.estadoAprobacion != null && filtroListadoAmec.estadoAprobacion != -1)
            {
                if (filtroListadoAmec.estadoAprobacion == 1)
                {
                    //Pendiente aprobar Médico
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.preaprobadamed = 0");
                }

                if (filtroListadoAmec.estadoAprobacion == 2)
                {
                    //Pendiente aprobar Negocio
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.preaprobadaneg = 0");
                }

                if (filtroListadoAmec.estadoAprobacion == 3)
                {
                    //Pendiente aprobar Legal Compliance
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" ams.preaprobadaleg = 0");
                }

            }
            if (filtroListadoAmec.year != null && filtroListadoAmec.year != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" YEAR(fechaamecs) = {0}", filtroListadoAmec.year);
            }
            if (filtroListadoAmec.mes != null && filtroListadoAmec.mes != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" MONTH(fechaamecs) = {0}", filtroListadoAmec.mes);
            }

            if (filtroListadoAmec.Importe != null)
            {
                AgregarAndSiProcede(db, ref primero);
                string sCantidadSpa = string.Format(" ams.importegasto {0} {1}", filtroListadoAmec.TipoFiltroImporte, filtroListadoAmec.Importe);
                sCantidadSpa = sCantidadSpa.Replace(',', '.');
                db.AppendFormat(" {0}", sCantidadSpa);
            }



            //if (listIdAmecs.Count > 0)
            //{
            //    string filtroIdAmecs = "('";
            //    foreach (var idamec in listIdAmecs)
            //    {
            //        filtroIdAmecs += listIdAmecs.Count > listIdAmecs.IndexOf(idamec) + 1 ? idamec + "','" : idamec + "')";
            //    }
            //    AgregarAndSiProcede(db, ref primero);
            //    db.AppendFormat(" ams.idamecs IN {0} ", filtroIdAmecs);
            //}

            AgregarAndSiProcede(db, ref primero);
            db.Append(" ams.veeva = 1 ");

            if (!count)
                db.Append(" GROUP BY ams.idamecs, ams.fechaamecs, ams.veeva, ams.newco ");



            if (!string.IsNullOrEmpty(filtroListadoAmec.SortParameter))
            {
                if (filtroListadoAmec.SortParameter == "ESTADO") db.AppendFormat(" ORDER BY est.estado");
                else if (filtroListadoAmec.SortParameter == "ESTADO DESC") db.AppendFormat(" ORDER BY est.estado DESC");

                else db.AppendFormat(" ORDER BY ams.{0}", filtroListadoAmec.SortParameter);
            }
            else
            {
                if (!count)
                {
                    db.AppendFormat(" ORDER BY ams.fechaamecs DESC,ams.idamecs DESC");
                }
            }
            return db.ToString();
        }

        public string ObtenerFiltroUnidades(FiltroListadoAMECs filtro)
        {
            string sFiltroRolAmec = null;
            StringBuilder db = new StringBuilder();
            if (filtro.IdUnidadFiltro.HasValue && filtro.IdUnidadFiltro != 0)
            {
                db.Append("select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE (");
                db.AppendFormat(" unidams.IdUnidad = {0} ", filtro.IdUnidadFiltro);
            }
            if (filtro.IdAreaFiltro.HasValue && filtro.IdAreaFiltro != 0)
            {
                if (db.Length > 0) db.Append(" AND "); else db.Append(" select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE (");
                db.AppendFormat(" unidams.IdArea = {0} ", filtro.IdAreaFiltro);
            }

            if (filtro.IdRegionFiltro.HasValue && filtro.IdRegionFiltro != 0)
            {
                if (db.Length > 0) db.Append(" AND "); else db.Append(" select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE (");
                db.AppendFormat(" unidams.IdRegion = {0} ", filtro.IdRegionFiltro);
            }
            if (filtro.IdDistritoFiltro.HasValue && filtro.IdDistritoFiltro != 0)
            {
                if (db.Length > 0) db.Append(" AND "); else db.Append(" select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE (");
                db.AppendFormat(" unidams.IdDistrito = {0} ", filtro.IdDistritoFiltro);
            }

            if (db.Length > 0)
            {
                sFiltroRolAmec = string.Format(" inner join ({0})) rol on ams.idamecs = rol.idamecs", db.ToString());
            }
            return sFiltroRolAmec;

        }

        //Borrar, no se usa
        public string ObtenerFiltroRolAmecDefinitivo(DVPeticionariosRoles datosRoles, FiltroListadoAMECs filtro)
        {
            string sFiltroRolAmec = null;
            if (datosRoles == null) return sFiltroRolAmec;
            StringBuilder dbNOParaguas = new StringBuilder();
            StringBuilder dbParaguas = new StringBuilder();
            StringBuilder dbTotal = new StringBuilder();
            StringBuilder dbcreadopor = new StringBuilder();

            bool bAprobador = (datosRoles.aprobador.HasValue) ? datosRoles.aprobador.Value : false;
            //Si es administrador pot veure tots els Amecs
            //IdCargo usuario Legal: 554
            //IdCargo usuario Compliance: 555
            //IdCargo usuario Departamento Medico: 547
            //IdCargo usuario MD: 534
            //Segun el login 
            if ((datosRoles.administrador.HasValue && datosRoles.administrador.Value) || datosRoles.IdCargo == 554 || datosRoles.IdCargo == 555 || datosRoles.IdCargo == 547 || datosRoles.IdCargo == 534)
            {
                return sFiltroRolAmec;
            }

            if (!bAprobador)
            {
                ////////////////Farem el filtre en base a totes les unitats organitzatives del usuaris que existeixen en l'aplicació////////////
                //1= Solo unidad (BUD)
                //2= Unidad y Area (FM/BUM/BCM)
                //3= Unidad, Area y Región (RBD, RBM)
                //4= Unidad, Area y distrito (HSA)
                //5= Unidad, area, región y distrito (DELEGADOS Y GERENTES)

                //1
                if (datosRoles.idunidad.HasValue && !datosRoles.Idarea.HasValue && !datosRoles.idregion.HasValue && !datosRoles.iddistrito.HasValue)
                {
                    dbTotal.AppendFormat("select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE unidams.IdUnidad = {0}", datosRoles.idunidad);
                }

                //2
                if (datosRoles.idunidad.HasValue && datosRoles.Idarea.HasValue && !datosRoles.idregion.HasValue && !datosRoles.iddistrito.HasValue)
                {
                    dbNOParaguas.AppendFormat("select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE unidams.IdUnidad = {0} AND unidams.Idarea = {1} and ams.paraguas = false", datosRoles.idunidad, datosRoles.Idarea);
                    dbParaguas.AppendFormat("select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE (unidams.IdUnidad = {0} and unidams.Idarea = null and unidams.Idregion = null and unidams.Iddistrito = null) or (unidams.IdUnidad = {0} AND unidams.Idarea = {1}) and ams.paraguas = true", datosRoles.idunidad, datosRoles.Idarea);
                }

                //3
                if (datosRoles.idunidad.HasValue && datosRoles.Idarea.HasValue && datosRoles.idregion.HasValue && !datosRoles.iddistrito.HasValue)
                {
                    dbNOParaguas.AppendFormat("select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE unidams.IdUnidad = {0} AND unidams.Idarea = {1} and unidams.Idregion = {2} and ams.paraguas = false", datosRoles.idunidad, datosRoles.Idarea, datosRoles.idregion);
                    dbParaguas.AppendFormat("select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE (unidams.IdUnidad = {0} and unidams.Idarea = null and unidams.Idregion = null and unidams.Iddistrito = null) or (unidams.IdUnidad = {0} AND unidams.Idarea = {1} AND unidams.Idregion = null and unidams.Iddistrito = null) or (unidams.IdUnidad = {0} AND unidams.Idarea = {1} AND unidams.Idregion = {2}) and ams.paraguas = true", datosRoles.idunidad, datosRoles.Idarea, datosRoles.idregion);
                }

                //4
                if (datosRoles.idunidad.HasValue && datosRoles.Idarea.HasValue && !datosRoles.idregion.HasValue && datosRoles.iddistrito.HasValue)
                {
                    dbNOParaguas.AppendFormat("select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE unidams.IdUnidad = {0} AND unidams.Idarea = {1} and unidams.iddistrito = {2} and ams.paraguas = false", datosRoles.idunidad, datosRoles.Idarea, datosRoles.iddistrito);
                    dbParaguas.AppendFormat("select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE (unidams.IdUnidad = {0} and unidams.Idarea = null and unidams.Idregion = null and unidams.Iddistrito = null) or (unidams.IdUnidad = {0} AND unidams.Idarea = {1} AND unidams.Idregion = null and unidams.Iddistrito = null) or (unidams.IdUnidad = {0} AND unidams.Idarea = {1} AND unidams.distrito = {2}) and ams.paraguas = true", datosRoles.idunidad, datosRoles.Idarea, datosRoles.iddistrito);

                }

                //5
                if (datosRoles.idunidad.HasValue && datosRoles.Idarea.HasValue && datosRoles.idregion.HasValue && datosRoles.iddistrito.HasValue)
                {
                    dbNOParaguas.AppendFormat("select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE unidams.IdUnidad = {0} AND unidams.Idarea = {1} and unidams.Idregion = {2}  and unidams.Iddistrito = {3} and ams.paraguas = false", datosRoles.idunidad, datosRoles.Idarea, datosRoles.idregion, datosRoles.iddistrito);
                    dbParaguas.AppendFormat("select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE (unidams.IdUnidad = {0} and unidams.Idarea = null and unidams.Idregion = null and unidams.Iddistrito = null) or (unidams.IdUnidad = {0} AND unidams.Idarea = {1} AND unidams.Idregion = null and unidams.Iddistrito = null) or (unidams.IdUnidad = {0} AND unidams.Idarea = {1} AND unidams.Idregion = {2} and unidams.Iddistrito = null) or (unidams.IdUnidad = {0} AND unidams.Idarea = {1} AND unidams.Idregion = {2} AND unidams.Iddistrito = {3}) and ams.paraguas = true", datosRoles.idunidad, datosRoles.Idarea, datosRoles.idregion, datosRoles.iddistrito);
                }


                dbcreadopor.AppendFormat("select idamecs from amecs where idcreadopor = {0} Or idsolicitante = {0}", datosRoles.IdPeticionario);

                if (dbcreadopor.Length > 0)
                {
                    sFiltroRolAmec = string.Format(" inner join (({0}) union ({1}) union ({2})) rol on ams.idamecs = rol.idamecs", dbParaguas.ToString(), dbNOParaguas.ToString(), dbcreadopor.ToString());
                }

                else
                {
                    sFiltroRolAmec = string.Format(" inner join (({0}) union ({1})) rol on ams.idamecs = rol.idamecs", dbParaguas.ToString(), dbNOParaguas.ToString());

                }

                if (datosRoles.delegado == true)
                {
                    sFiltroRolAmec = string.Format(" inner join ({0}) rol on ams.idamecs = rol.idamecs", dbcreadopor.ToString());
                }

                return sFiltroRolAmec;
            }
            return sFiltroRolAmec;



        }

        public string ObtenerFiltroRolAmecCliente(DVPeticionariosRoles datosRoles, FiltroListadoAMECs filtro)
        {
            string sFiltroRolAmec = null;
            if (datosRoles == null) return sFiltroRolAmec;
            StringBuilder db = new StringBuilder();
            StringBuilder dbcreadopor = new StringBuilder();
            bool bAprobador = (datosRoles.aprobador.HasValue) ? datosRoles.aprobador.Value : false;
            //Si es administrador pot veure tots els Amecs
            //IdCargo usuario Legal: 554
            //IdCargo usuario Compliance: 555
            //IdCargo usuario Departamento Medico: 547
            //IdCargo usuario MD: 534
            //Segun el login 
            if ((datosRoles.administrador.HasValue && datosRoles.administrador.Value) || datosRoles.IdCargo == 554 || datosRoles.IdCargo == 555 || datosRoles.IdCargo == 547 || datosRoles.IdCargo == 534)
            {
                return sFiltroRolAmec;
            }

            if (!bAprobador)
            {
                if (datosRoles.idunidad.HasValue)
                {
                    db.Append("select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE (");
                    db.AppendFormat(" unidams.IdUnidad = {0} ", datosRoles.idunidad);
                }
                if (datosRoles.Idarea.HasValue)
                {
                    if (db.Length > 0) db.Append(" AND "); else db.Append(" select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE (");
                    db.AppendFormat(" unidams.IdArea = {0} ", datosRoles.Idarea);
                }

                if (datosRoles.idregion.HasValue)
                {
                    if (db.Length > 0) db.Append(" AND "); else db.Append(" select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE (");
                    db.AppendFormat(" unidams.IdRegion = {0} ", datosRoles.idregion);
                }
                if (datosRoles.iddistrito.HasValue)
                {
                    if (db.Length > 0) db.Append(" AND "); else db.Append(" select distinct (idamecs) as idamecs from unidorganizamec unidams WHERE (");
                    db.AppendFormat(" unidams.IdDistrito = {0} ", datosRoles.iddistrito);
                }

                dbcreadopor.AppendFormat("select idamecs from amecs where idcreadopor = {0} Or idsolicitante = {0}", datosRoles.IdPeticionario);

                if (db.Length > 0 && dbcreadopor.Length > 0)
                {
                    sFiltroRolAmec = string.Format(" inner join ({0}) union ({1})) rol on ams.idamecs = rol.idamecs", db.ToString(), dbcreadopor.ToString());
                }
                else if (db.Length == 0 || dbcreadopor.Length > 0)
                {
                    sFiltroRolAmec = string.Format(" inner join ({0}) rol on ams.idamecs = rol.idamecs", dbcreadopor.ToString());
                }
                else if (db.Length > 0 || dbcreadopor.Length == 0)
                {
                    sFiltroRolAmec = string.Format(" inner join ({0})) rol on ams.idamecs = rol.idamecs", db.ToString());
                }
                return sFiltroRolAmec;
            }
            return sFiltroRolAmec;
        }

        private string CrearSeccionWhereFiltroInformeFCPA(string idamec, string nombrePrograma, string estado, string fechaInicio, string fechaFin, string unidad, string area,
string region, string distrito, string year, string mes, string iddistrict, string idsaleforce, string iddepartament)
        {
            StringBuilder db = new StringBuilder();
            string strSQL = string.Empty;
            bool primero = true;
            if (idamec != null && !string.IsNullOrWhiteSpace(idamec))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" idamecs = '{0}'", idamec);
            }
            if (nombrePrograma != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" nombreprogramaactividad like '{0}'", nombrePrograma.Replace("'", "''"));
            }
            if (!string.IsNullOrEmpty(iddistrict))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" riesgo_fcpa.idDistrict = {0}", iddistrict);
            }
            if (!string.IsNullOrEmpty(idsaleforce))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" riesgo_fcpa.idSaleforce = {0}", idsaleforce);
            }
            if (!string.IsNullOrEmpty(iddepartament))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" riesgo_fcpa.idDepartament = {0}", iddepartament);
            }
            if (estado != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" estadoamec = '{0}'", estado);
            }
            if (!string.IsNullOrEmpty(fechaInicio))
            {
                fechaInicio = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(fechaInicio));

                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" fechaInicio >= '{0}'", fechaInicio);
            }

            if (!string.IsNullOrEmpty(fechaFin))
            {
                fechaFin = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(fechaFin));

                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" fechaFin <= '{0}'", fechaFin);
            }
            if (unidad != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" unidad = '{0}'", unidad);
            }
            if (area != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" area = '{0}'", area);
            }
            if (region != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" region = '{0}'", region);
            }
            if (distrito != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" distrito = '{0}'", distrito);
            }
            if (year != null && year != "-1")
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" YEAR(fechacertificacionFCPA) = {0}", int.Parse(year));
            }
            if (mes != null && mes != "-1")
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" MONTH(fechacertificacionFCPA) = {0}", int.Parse(mes));
            }
            return db.ToString();
        }

        private string CrearSeccionWhereFiltroAMECCongreso(FiltroAMECCongreso filtroAMECCongreso)
        {
            if (filtroAMECCongreso == null) return null;

            StringBuilder db = new StringBuilder();
            string strSQL = string.Empty;
            bool primero = true;

            if (filtroAMECCongreso.IdCongreso != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" IdCongreso = {0}", filtroAMECCongreso.IdCongreso.Value);
            }
            if (filtroAMECCongreso.NombreEvento != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" Congreso like '{0}'", filtroAMECCongreso.NombreEvento.Replace("'", "''"));
            }
            if (filtroAMECCongreso.IdPoblacion != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" IDPoblacion = {0}", filtroAMECCongreso.IdPoblacion);
            }
            if (filtroAMECCongreso.TipoActividad != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" IdTipoCongreso = {0}", filtroAMECCongreso.TipoActividad);
            }

            AgregarAndSiProcede(db, ref primero);
            db.AppendFormat(" publicar = 1");

            if (filtroAMECCongreso.AmecLike != null)
            {
                AgregarAndSiProcede(db, ref primero);

                strSQL = "(";
                strSQL += String.Format("(IdCongreso IN (SELECT idpeticionactividad FROM amec WHERE amec like '%{0}%')) ", filtroAMECCongreso.AmecLike);
                strSQL += " or ";
                strSQL += String.Format("(IdCongreso IN (SELECT idcongreso FROM amec WHERE amec like '%{0}%'))", filtroAMECCongreso.AmecLike);
                strSQL += ")";

                db.Append(strSQL);
            }

            if (filtroAMECCongreso.FechaDesde != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" Desde >= '{0}'", filtroAMECCongreso.FechaDesde.Value.ToString("yyyy/MM/dd"));
            }

            if (filtroAMECCongreso.FechaHasta != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" Hasta <= '{0}'", filtroAMECCongreso.FechaHasta.Value.ToString("yyyy/MM/dd"));
            }
            if (!string.IsNullOrEmpty(filtroAMECCongreso.SortParameter))
            {
                //Solo Ordenar Por Fecha
                db.AppendFormat(" ORDER BY {0}", filtroAMECCongreso.SortParameter);
            }
            else
            {
                db.AppendFormat(" ORDER BY Hasta Desc");
            }

            return db.ToString();
        }

        private void AgregarAndSiProcede(StringBuilder sb, ref bool primero)
        {
            if (primero == true)
            {
                primero = false;
                sb.Append(" WHERE ");
            }
            else sb.Append(" AND ");
        }

        public int CambiarEstadoAmec(string idestado, string idamecs, string idpeticionario, string idaprobador)
        {
            try
            {
                string consulta = string.Empty;
                int resultadohist = 0;
                consulta = string.Format("update amecs set idestado = {0} where idamecs ='{1}'", idestado, idamecs);
                int resultado = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

                if (resultado != 0)
                {
                    string consulta2 = string.Format("insert into histasocamecs (idamecs,idestado,idnivelaprobacion,idcreadopor,fechacreacion,iddistrict,iddepartament,idsaleforce) values ('{0}', {1}, {2}, {3}, GETDATE(),1,1,1)", idamecs, idestado, "1", idpeticionario);
                    resultadohist = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta2);
                }
                return resultadohist;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        

        public int InsertarHistorialAmec(int idestado, string idamecs, DDatosPersonalesUsuario peticionario, int nivelaprobacion, int? idAprobador)
        {
            string iddepartament = "null";
            string iddistrict = "null";
            string idsaleforce = "null";
            if (peticionario.iddepartament > 0) iddepartament = peticionario.iddepartament.ToString();
            if (peticionario.iddistrict > 0) iddistrict = peticionario.iddistrict.ToString();
            if (peticionario.idsaleforce > 0) idsaleforce = peticionario.idsaleforce.ToString();
            string consulta = string.Format("insert into histasocamecs (idamecs,idestado,idnivelaprobacion,idcreadopor,fechacreacion,iddistrict,idsaleforce,iddepartament, idaprobador) " +
                                            "values ('{0}', {1}, {2}, {3}, CURRENT_TIMESTAMP,"+ iddistrict+ ","+ idsaleforce + ","+ iddepartament + ", {4})", idamecs, idestado, nivelaprobacion, peticionario.IdPeticionario, idAprobador == null ? "null" : idAprobador.ToString());
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        public int BorrarHistorialAmec(int idestado, string idamecs, int idPeticionario,int nivelaprobacion)
        {
            string consulta =
                String.Format(
                    "DELETE FROM histasocamecs WHERE idamecs='{0}' AND idestado={1} AND idcreadopor={2} AND idnivelaprobacion={3}",
                    idamecs, idestado, idPeticionario, nivelaprobacion);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        #endregion SeccionWhereFiltros

    }
}
