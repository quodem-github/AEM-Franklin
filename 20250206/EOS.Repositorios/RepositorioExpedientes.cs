using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Entidades.Modelo;
using NHibernate;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Linq;
using NHibernate.Transform;

namespace EOS.Repositorios
{
    public class RepositorioExpedientes : IRepositorioExpedientes
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

        private enum enAlternativa
        {
            alternativa1,
            alternativa2,
            alternativa3
        }

        #region Constructor
        public RepositorioExpedientes()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(EOS.ServiceLogic.Enums.EDbConnection.Default.GetHashCode());
        }
        #endregion

        #region "Obtener Expedientes"

        public DExpediente ObtenerExpediente(string filtroIDExpediente)
        {
            string consulta = string.Format("select * from expediente where idxpediente={0}", filtroIDExpediente);
            return DExpediente.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
        }

        public ICollection<DCabeceraExpedienteAmpliado> ObtenerExpedientes(FiltroExpedientesAvanzado filtro)
        {

            string consulta = string.Format("select exp.Idexpediente AS Idexpediente,exp.idconfempresa as idconfempresa, exp.Idamec AS Idamec,Amec AS Amec,Fechacreacion AS Fechacreacion,Idactividad AS Idactividad,Actividad AS Actividad, Desde AS FechaDesde, Hasta AS FechaHasta, Poblacion AS Poblacion, CAST(idvaloracionfi as int) AS Idvaloracionfi, unidad AS Unidad, area AS Area, region AS Region, distrito AS Distrito, Peticionario AS Peticionario, Cargo AS Cargo, idestado AS Idestado, Tiporeserva AS Tiporeserva, IdTipoReserva AS Idtiporeserva, Pedido AS Pedido, tipoPagoFee AS TipoPagoFee, CAST(urgente AS BIT) AS Urgente, importe AS Importe from cv_expedientes_total_fa_fee exp {0}",
                CrearSeccionWhereFiltroExpediente(filtro));
            return ObtenerConsulta(consulta, filtro);
        }

        public List<DRelacionFicheroVersion> ObteneRelacionFicheroVersions(int idExpediente)
        {
            string query = string.Format("SELECT * FROM gestordocumental_documento doc INNER JOIN " +
                                        "gestordocumental_documentoversion ver ON doc.Id = ver.IdDocumento where doc.IdExpediente = {0}", idExpediente);
            return DRelacionFicheroVersion.ConvertTo(Quodem.Sql.SqlServerClient.GetQuery(query));
        }

        public DAmecExpediente ObtenerRelacionAmecExpediente(int idExpediente)
        {
            DAmecExpediente relacion = new DAmecExpediente();
            string query = string.Format("SELECT a.amec as idamecs,e.idxpediente as idexpediente from amec a INNER JOIN expediente e ON e.idamec=a.idamec WHERE e.idxpediente = {0}", idExpediente);
            return relacion.ConvertTo(Quodem.Sql.SqlServerClient.GetQuery(query));

        }

        public List<DDocumentoVersion> ObtenerDatosDocumentos(string rutaDir)
        {
            string query = string.Format("SELECT doc.Id as idDocumento,doc.Fecha as FechaDoc,ver.Fecha as FechaVer,* " +
                                         "FROM gestordocumental_documento doc INNER JOIN gestordocumental_documentoversion ver" +
                                         " ON doc.Id = ver.IdDocumento where " +
                                         "ver.RutaFichero LIKE '{0}\\%' AND ver.RutaFichero NOT LIKE '{0}\\%/%' AND ver.RutaFichero NOT LIKE '{0}\\%\\%'", rutaDir);

            if (rutaDir.EndsWith("DocsAntiguos"))
            {
                query = string.Format("SELECT doc.Id as idDocumento,doc.Fecha as FechaDoc,ver.Fecha as FechaVer,* " +
                             "FROM gestordocumental_documento doc INNER JOIN gestordocumental_documentoversion ver" +
                             " ON doc.Id = ver.IdDocumento where " +
                             "ver.RutaFichero LIKE '{0}\\%' AND ver.RutaFichero NOT LIKE '{0}\\%/%'", rutaDir);
            }
            return DDocumentoVersion.ConvertTo(Quodem.Sql.SqlServerClient.GetQuery(query));
        }

        public List<DPassenger> ObtenerPassengers(int idExpediente)
        {
            var query = string.Format("SELECT p.idpassengerlist as Id, p.nombre,p.apel1,p.apel2 from passengers_list p " +
                        "INNER JOIN reservas_passengers_list r ON r.idpassengerlist = p.idpassengerlist " +
                        "INNER JOIN expediente e ON r.idxpediente = e.idxpediente WHERE e.idxpediente = {0}", idExpediente);
            return DPassenger.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query));
        }

        public List<DDocumentNameTypeSubType> ObtenerRelacionTipoVersion(int idExpediente)
        {
            var query = string.Format("SELECT DISTINCT dv.NombreOriginal,t.Id as idTipo,t.Nombre as nTipo,s.Id as idSubTipo,s.Nombre as nSubTipo FROM gestordocumental_documentoversion dv INNER JOIN gestordocumental_documento d ON dv.IdDocumento = d.Id" +
                                        " INNER JOIN gestordocumental_tipodoc t ON t.Id = d.IdTipodoc"+
                                        " LEFT JOIN gestordocumental_subtipodoc s ON s.Id = d.IdSubTipodoc"+
                                        " WHERE d.IdExpediente = {0}", idExpediente);
            return DDocumentNameTypeSubType.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query));
        }

        public List<string> ObtenerNombresOriginalesDoc(int idExpediente)
        {
            List<string> result = new List<string>();
            var query =
                string.Format(
                    "SELECT dv.NombreOriginal FROM gestordocumental_documentoversion dv INNER JOIN gestordocumental_documento d " +
                    " ON dv.idDocumento = d.Id WHERE d.IdExpediente = {0}", idExpediente);
            DataTable dt = Quodem.Sql.SqlServerClient.GetQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                result.Add(Quodem.Utility.DataLayerUtil.GetStringValue(row, "NombreOriginal"));
            }
            return result;
        }

        public ICollection<DCabeceraExpedienteAmpliado> ObtenerExpedientesTotal(FiltroExpedientesAvanzado filtro, out int count)
        {
            try
            {
                string consulta = "";
                if (!string.IsNullOrEmpty(filtro.Actividad) &&
                    string.IsNullOrEmpty(filtro.Amec) && string.IsNullOrEmpty(filtro.AreaNegocio) &&
                    string.IsNullOrEmpty(filtro.Asistente) && string.IsNullOrEmpty(filtro.Distrito)
                    && string.IsNullOrEmpty(filtro.EstadoExpediente) && string.IsNullOrEmpty(filtro.EstadoReserva) &&
                    string.IsNullOrEmpty(filtro.Peticionario) && string.IsNullOrEmpty(filtro.Producto) &&
                    string.IsNullOrEmpty(filtro.ProveedorServicio)
                    && string.IsNullOrEmpty(filtro.Region) && string.IsNullOrEmpty(filtro.TipoFiltroImporte) &&
                    string.IsNullOrEmpty(filtro.Unidad) && string.IsNullOrEmpty(filtro.Año.ToString()) &&
                    string.IsNullOrEmpty(filtro.Desde.ToString())
                    && string.IsNullOrEmpty(filtro.Hasta.ToString()) &&
                    string.IsNullOrEmpty(filtro.IdExpediente.ToString()) &&
                    string.IsNullOrEmpty(filtro.Importe.ToString()) && string.IsNullOrEmpty(filtro.Mes.ToString()) &&
                    string.IsNullOrEmpty(filtro.Tipo.ToString()) && string.IsNullOrEmpty(filtro.GroupParameter))
                {
                    consulta =
                        string.Format(
                             " select e.idxpediente AS Idexpediente,e.idamec AS Idamec,e.amec AS Amec,  "
                            + " e.fechacreacion AS Fechacreacion,c2.IdCongreso AS Idactividad,  "
                            +
                              " c2.Congreso AS Actividad,c2.Desde AS FechaDesde,c2.Hasta AS FechaHasta,  "
                            +
                              " pb.Poblacion AS Poblacion,CAST(c2.idvaloracionfi as int) AS Idvaloracionfi,u.unidad AS Unidad,  "
                            +
                              " ar.area AS Area,r.region AS Region,d.distrito AS Distrito, RTRIM(LTRIM(ISNULL(p.Nombre, '') + ' ' + ISNULL(p.Apellido1, ''))) AS Peticionario,ca.Cargo AS Cargo,e.idestado AS Idestado,  "
                            +
                              " t.Descripcion AS Tiporeserva,e.IdTipoReserva AS Idtiporeserva,e.codexpediente AS Pedido,  "
                            + " CAST(e.urgente AS BIT) AS Urgente, e.importeTotal AS Importe, e.idconfempresa as IdConfEmpresa, e.tipoPagoFee as TipoPagoFee  into #Result "
                            + " from cv_congresos_con_amec c2  " +
                              " left join cv_amec_expediente e on e.idcongreso = c2.IdCongreso or e.idpeticionactividad = c2.IdCongreso and e.idcongreso is null   "
                            + " left join unidades u on u.idunidad = e.idunidad   "
                            + " left join areas ar on ar.Idarea = e.idarea   "
                            + " left join regiones r on r.idregion = e.idregion   "
                            + " left join distritos d on d.iddistrito = e.iddistrito   "
                            +
                              " left join peticionarios p on p.IdPeticionario = e.idpeticionario   "
                            +
                              " left join tiposreservas_web t on t.IdTipoReserva = e.IdTipoReserva   "
                            +
                              " left join poblaciones pb on c2.IdPoblacion = pb.IdPoblacion   "
                            +
                              " left join cargos ca on p.IdCargo = ca.IdCargo {0} ",
                        CrearSeccionWhereFiltroExpedienteTemporal(filtro, false));
                }
                else
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(" IF OBJECT_ID('tempdb..#Temp') IS NOT NULL DROP TABLE #Temp;");
                    query.Append(" CREATE TABLE #Temp( idamecs varchar(20) PRIMARY KEY CLUSTERED);");
                    query.AppendFormat(" INSERT INTO #Temp EXEC sp_obtener_amecs_puedo_ver @idPeticionario = {0}, @isCreateMode = 0;", filtro.IdPeticionarioSession);
                    query.Append(" INSERT INTO #Temp SELECT distinct (amec) from amec WHERE amec not like '4________' AND amec COLLATE Modern_Spanish_CI_AS NOT IN (SELECT * FROM #Temp);");
                    query.Append("");
                    query.Append(" IF OBJECT_ID('tempdb..#Result') IS NOT NULL DROP TABLE #Result;");
                    query.Append(" SELECT exp.* into #Result ");
                    query.Append(" FROM   (SELECT e.idxpediente                           AS Idexpediente, ");
                    query.Append("               e.idamec                                AS Idamec, ");
                    query.Append("               e.amec                                  AS Amec, ");
                    query.Append("               e.fechacreacion                         AS Fechacreacion, ");
                    query.Append("               e.idcongreso                            AS Idactividad, ");
                    query.Append("               Isnull(cong.congreso, pet.nombre)       AS Actividad, ");
                    query.Append("               Isnull(cong.desde, pet.desde)           AS FechaDesde, ");
                    query.Append("               Isnull(cong.hasta, pet.hasta)           AS FechaHasta, ");
                    query.Append("               Isnull(pb.poblacion, pb2.poblacion)     AS Poblacion, ");
                    query.Append("               Cast(cong.idvaloracionfi AS INT)        AS Idvaloracionfi, ");
                    query.Append("               u.unidad                                AS Unidad, ");
                    query.Append("               ar.area                                 AS Area, ");
                    query.Append("               r.region                                AS Region, ");
                    query.Append("               d.distrito                              AS Distrito, ");
                    query.Append("               Rtrim(Ltrim(Isnull(p.nombre, '') + ' ' ");
                    query.Append("                           + Isnull(p.apellido1, ''))) AS Peticionario, ");
                    query.Append("               ca.cargo                                AS Cargo, ");
                    query.Append("               e.idestado                              AS Idestado, ");
                    query.Append("               t.descripcion                           AS Tiporeserva, ");
                    query.Append("               ta.tipoactividad                        AS tipoactividad, ");
                    query.Append("               e.idtiporeserva                         AS Idtiporeserva, ");
                    query.Append("               e.codexpediente                         AS Pedido, ");
                    query.Append("               e.iddistrict                            AS iddistrict,");
                    query.Append("               distr.district                          AS district,");
                    query.Append("               e.iddepartament                         AS iddepartament,");
                    query.Append("               dept.departament                        AS departament,");
                    query.Append("               e.idsaleforce                           AS idsaleforce, ");
                    query.Append("               sale.saleforce                          AS saleforce, ");
                    query.Append("               Cast(e.urgente AS BIT)                  AS Urgente, ");
                    query.Append("               e.importetotal                          AS Importe, ");
                    query.Append("               e.idconfempresa                         AS IdConfEmpresa, ");
                    query.Append("               e.tipoPagoFee                         AS TipoPagoFee ");
                    query.Append("        FROM   cv_amec_expediente e ");
                    query.Append("               INNER JOIN #Temp");
                    query.Append("                     ON #Temp.idamecs COLLATE MODERN_SPANISH_CI_AI= e.amec COLLATE MODERN_SPANISH_CI_AI ");
                    query.Append("               LEFT JOIN congresos cong");
                    query.Append("                      ON e.idcongreso = cong.idcongreso ");
                    query.Append("               LEFT JOIN peticiones_actividad pet ");
                    query.Append("                      ON e.idpeticionactividad = pet.idpeticionactividad ");
                    query.Append("               LEFT JOIN unidades u ");
                    query.Append("                      ON u.idunidad = e.idunidad ");
                    query.Append("               LEFT JOIN areas ar ");
                    query.Append("                      ON ar.idarea = e.idarea ");
                    query.Append("               LEFT JOIN regiones r ");
                    query.Append("                      ON r.idregion = e.idregion ");
                    query.Append("               LEFT JOIN distritos d ");
                    query.Append("                      ON d.iddistrito = e.iddistrito ");
                    query.Append("               LEFT JOIN peticionarios p ");
                    query.Append("                      ON p.idpeticionario = e.idpeticionario ");
                    query.Append("               LEFT JOIN departaments dept ");
                    query.Append("                      ON p.iddepartament = dept.iddepartament ");
                    query.Append("               LEFT JOIN districts distr ");
                    query.Append("                      ON p.iddistrict = distr.iddistrict ");
                    query.Append("               LEFT JOIN salesforce sale ");
                    query.Append("                      ON p.idsaleforce = sale.idsaleforce ");
                    query.Append("               LEFT JOIN tiposreservas_web t ");
                    query.Append("                      ON t.idtiporeserva = e.idtiporeserva ");
                    query.Append("               LEFT JOIN poblaciones pb ");
                    query.Append("                      ON cong.idpoblacion = pb.idpoblacion ");
                    query.Append("               LEFT JOIN poblaciones pb2 ");
                    query.Append("                      ON cong.idpoblacion = pb2.idpoblacion ");
                    query.Append("               LEFT JOIN cargos ca ON p.IdCargo = ca.IdCargo  ");
                    query.Append("               LEFT JOIN amecs ams ");
                    query.Append("                      ON cast(ams.idamecs AS CHAR) COLLATE MODERN_SPANISH_CI_AI = #Temp.idamecs COLLATE MODERN_SPANISH_CI_AI");
                    query.Append("               LEFT JOIN tipoactividad ta ");
                    query.Append("                      ON ta.idtipoactividad = ams.idtipoactividad ");

                    consulta = string.Format(query + ") exp {0}",
                        CrearSeccionWhereFiltroExpediente(filtro));
                }

                int countTemp = 0;

                if(consulta.IndexOf("***") > 0)
                {
                    int indexof = consulta.IndexOf("***");
                    int lastindexof = consulta.LastIndexOf("***");
                    string firstQueryPart = consulta.Substring(0, indexof);
                    string lastQueryPart = consulta.Substring(lastindexof + 3);
                    string procedurePart = consulta.Substring(indexof + 3, lastindexof - indexof - 3);

                    consulta = "IF OBJECT_ID('tempdb..#TempExpedientesPuedoVer') IS NOT NULL DROP TABLE #TempExpedientesPuedoVer;CREATE TABLE #TempExpedientesPuedoVer( idexpediente int not null);insert into #TempExpedientesPuedoVer " + procedurePart + ";";
                    consulta += firstQueryPart + " SELECT * FROM #TempExpedientesPuedoVer " + lastQueryPart;
                }

                if (!consulta.Equals(Constantes.ConsultaTotalExpedientes))
                {
                    Constantes.ConsultaTotalExpedientes = consulta;
                    Constantes.TotalExpedientes = ObtenerConsultaCustom(consulta, filtro, out countTemp);
                }
                count = countTemp;
                return Constantes.TotalExpedientes;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        private ICollection<DCabeceraExpedienteAmpliado> ObtenerConsultaCustom(string consulta, FiltroExpedientesAvanzado filtro, out int count)
        {
            try
            {
                Quodem.Monitor.Alerta.WriteLog("*************************");
                Quodem.Monitor.Alerta.WriteLog("Datos usuario conectado: " + Newtonsoft.Json.JsonConvert.SerializeObject(filtro));
                Quodem.Monitor.Alerta.WriteLog("Consulta ejecutada: " + consulta);
                Quodem.Monitor.Alerta.WriteLog("*************************");
            }
            catch (Exception ex) { }
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    _session.CreateSQLQuery(consulta).ExecuteUpdate();
                    string query = "select * from #Result order by fechacreacion desc";

                    count = (int)_session.CreateSQLQuery("SELECT count(*) FROM #Result").List()[0];

                    if (filtro.MaximumRows != null && filtro.StartRowIndex != null)
                    {
                        var result = _session.CreateSQLQuery(query)
                            .SetFirstResult(filtro.StartRowIndex.Value)
                            .SetMaxResults(filtro.MaximumRows.Value)
                            .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DCabeceraExpedienteAmpliado)))
                            .List<DCabeceraExpedienteAmpliado>();
                        transaction.Commit();
                        return result;
                    }

                    var resultList = _session.CreateSQLQuery(query)
                        .SetResultTransformer(
                            NHibernate.Transform.Transformers.AliasToBean(typeof(DCabeceraExpedienteAmpliado)))
                        .List<DCabeceraExpedienteAmpliado>();
                    transaction.Commit();
                    return resultList;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        private ICollection<DCabeceraExpedienteAmpliado> ObtenerConsulta(string consulta, FiltroExpedientesAvanzado filtro)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    if (consulta.IndexOf("***") > 0)
                    {
                        int indexof = consulta.IndexOf("***");
                        int lastindexof = consulta.LastIndexOf("***");
                        string firstQueryPart = consulta.Substring(0, indexof);
                        string lastQueryPart = consulta.Substring(lastindexof + 3);
                        string procedurePart = consulta.Substring(indexof + 3, lastindexof - indexof - 3);

                        consulta = "IF OBJECT_ID('tempdb..#TempExpedientesPuedoVer') IS NOT NULL DROP TABLE #TempExpedientesPuedoVer;CREATE TABLE #TempExpedientesPuedoVer( idexpediente int not null);insert into #TempExpedientesPuedoVer " + procedurePart + ";";
                        consulta += firstQueryPart + " SELECT * FROM #TempExpedientesPuedoVer " + lastQueryPart;
                    }

                    if (filtro.MaximumRows != null && filtro.StartRowIndex != null)
                    {
                        var result = _session.CreateSQLQuery(consulta)
                            .SetFirstResult(filtro.StartRowIndex.Value)
                            .SetMaxResults(filtro.MaximumRows.Value)
                            .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DCabeceraExpedienteAmpliado)))
                            .List<DCabeceraExpedienteAmpliado>();
                        transaction.Commit();
                        return result;
                    }
                    var resultList = _session.CreateSQLQuery(consulta)
                        .SetResultTransformer(
                            NHibernate.Transform.Transformers.AliasToBean(typeof(DCabeceraExpedienteAmpliado)))
                        .List<DCabeceraExpedienteAmpliado>();
                    transaction.Commit();
                    return resultList;


                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public long ObtenerNumeroExpedientes(FiltroExpedientesAvanzado filtro)
        {
            //TODO Este cambio está hecho porque la vista expediente_total_fa va muy lenta y da error de timeout pero no es la solución.
            string consulta = "";
            if (!string.IsNullOrEmpty(filtro.Actividad) &&
                (string.IsNullOrEmpty(filtro.Roles) || !string.IsNullOrEmpty(filtro.Roles)) &&
                string.IsNullOrEmpty(filtro.Amec) && string.IsNullOrEmpty(filtro.AreaNegocio) &&
                string.IsNullOrEmpty(filtro.Asistente) && string.IsNullOrEmpty(filtro.Distrito)
                && string.IsNullOrEmpty(filtro.EstadoExpediente) && string.IsNullOrEmpty(filtro.EstadoReserva) &&
                string.IsNullOrEmpty(filtro.Peticionario) && string.IsNullOrEmpty(filtro.Producto) &&
                string.IsNullOrEmpty(filtro.ProveedorServicio)
                && string.IsNullOrEmpty(filtro.Region) && string.IsNullOrEmpty(filtro.TipoFiltroImporte) &&
                string.IsNullOrEmpty(filtro.Unidad) && string.IsNullOrEmpty(filtro.Año.ToString()) &&
                string.IsNullOrEmpty(filtro.Desde.ToString())
                && string.IsNullOrEmpty(filtro.Hasta.ToString()) &&
                string.IsNullOrEmpty(filtro.IdExpediente.ToString()) &&
                string.IsNullOrEmpty(filtro.Importe.ToString()) && string.IsNullOrEmpty(filtro.Mes.ToString()) &&
                string.IsNullOrEmpty(filtro.Tipo.ToString()) && string.IsNullOrEmpty(filtro.GroupParameter))
            {
                consulta =
                    string.Format(" select count(*) from cv_congresos_con_amec c2 " +
                                  " left join cv_amec_expediente e on e.idcongreso = c2.IdCongreso or e.idpeticionactividad = c2.IdCongreso and e.idcongreso is null "
                                  + " left join unidades u on u.idunidad = e.idunidad "
                                  + " left join areas ar on  ar.Idarea = e.idarea "
                                  + " left join regiones r on r.idregion = e.idregion "
                                  + " left join distritos d on  d.iddistrito = e.iddistrito " +
                                  " left join peticionarios p on p.IdPeticionario = e.idpeticionario "
                                  +
                                  " left join tiposreservas_web t on t.IdTipoReserva = e.IdTipoReserva "
                                  +
                                  " left join poblaciones pb on c2.IdPoblacion = pb.IdPoblacion "
                                  +
                                  " left join cargos ca on p.IdCargo = ca.IdCargo {0}",
                        CrearSeccionWhereFiltroExpedienteTemporal(filtro, true));
            }
            else
            {


                StringBuilder query = new StringBuilder();
                query.Append(" IF OBJECT_ID('tempdb..#Temp') IS NOT NULL DROP TABLE #Temp;");
                query.Append(" CREATE TABLE #Temp( idamecs varchar(20) PRIMARY KEY CLUSTERED);");
                query.AppendFormat(" INSERT INTO #Temp EXEC sp_obtener_amecs_puedo_ver @idPeticionario = {0}, @isCreateMode = 0;", filtro.IdPeticionarioSession);
                query.Append(" INSERT INTO #Temp SELECT distinct (amec) from amec WHERE amec not like '4________' AND amec COLLATE Modern_Spanish_CI_AS NOT IN (SELECT * FROM #Temp);");
                query.Append("");
                query.Append(" SELECT count(*) ");
                query.Append(" FROM ");
                query.Append(" ( ");
                query.Append("     SELECT  ");
                query.Append("         e.idxpediente AS IDEXPEDIENTE, ");
                query.Append("         e.idamec AS IDAMEC, ");
                query.Append("         e.amec AS AMEC, ");
                query.Append("         e.fechacreacion AS FECHACREACION, ");
                query.Append("         e.IdCongreso AS IDACTIVIDAD, ");
                query.Append("         ISNULL(cong.congreso, pet.nombre) AS ACTIVIDAD, ");
                query.Append("         ISNULL(cong.Desde, pet.Desde) AS DESDE, ");
                query.Append("         ISNULL(cong.Hasta, pet.Hasta) AS Hasta, ");
                query.Append("         ISNULL(pb.Poblacion, pb2.Poblacion) AS POBLACION, ");
                query.Append("         cong.idvaloracionfi AS IDVALORACIONFI, ");
                query.Append("         u.unidad AS UNIDAD, ");
                query.Append("         ar.area AS AREA, ");
                query.Append("         r.region AS REGION, ");
                query.Append("         d.distrito AS DISTRITO, ");
                query.Append("         RTRIM(LTRIM(ISNULL(p.Nombre, '') + ' ' + ISNULL(p.Apellido1, ''))) AS PETICIONARIO, ");
                query.Append("         ca.Cargo AS CARGO, ");
                query.Append("         e.idestado AS IDESTADO, ");
                query.Append("         t.Descripcion AS TIPORESERVA, ");
                query.Append("         ta.tipoactividad  AS tipoactividad, ");
                query.Append("         e.IdTipoReserva AS IDTIPORESERVA, ");
                query.Append("         e.codexpediente AS PEDIDO, ");
                query.Append("         e.iddistrict AS iddistrict, ");
                query.Append("         e.iddepartament AS iddepartament, ");
                query.Append("         e.idsaleforce AS idsaleforce, ");
                query.Append("         e.urgente AS URGENTE, ");
                query.Append("         e.importeTotal AS IMPORTE ");
                query.Append("     FROM ");
                query.Append("         cv_amec_expediente e ");
                query.Append("               INNER JOIN #Temp");
                query.Append("                     ON #Temp.idamecs COLLATE MODERN_SPANISH_CI_AI= e.amec COLLATE MODERN_SPANISH_CI_AI ");
                query.Append("         LEFT JOIN congresos cong on e.idcongreso = cong.idcongreso ");
                query.Append("         LEFT JOIN peticiones_actividad pet on e.idpeticionactividad = pet.idpeticionactividad ");
                query.Append("         LEFT JOIN unidades u ON u.idunidad = e.idunidad ");
                query.Append("         LEFT JOIN areas ar ON ar.Idarea = e.idarea ");
                query.Append("         LEFT JOIN regiones r ON r.idregion = e.idregion ");
                query.Append("         LEFT JOIN distritos d ON d.iddistrito = e.iddistrito ");
                query.Append("         LEFT JOIN peticionarios p ON p.IdPeticionario = e.idpeticionario ");
                query.Append("         LEFT JOIN tiposreservas_web t ON t.IdTipoReserva = e.IdTipoReserva ");
                query.Append("         LEFT JOIN poblaciones pb ON cong.IdPoblacion = pb.IdPoblacion ");
                query.Append("         LEFT JOIN poblaciones pb2 ON cong.IdPoblacion = pb2.IdPoblacion ");
                query.Append("         LEFT JOIN cargos ca ON p.IdCargo = ca.IdCargo ");
                query.Append("               LEFT JOIN amecs ams ");
                query.Append("                      ON cast(ams.idamecs AS CHAR) COLLATE MODERN_SPANISH_CI_AI = #Temp.idamecs COLLATE MODERN_SPANISH_CI_AI");
                query.Append("               LEFT JOIN tipoactividad ta ");
                query.Append("                      ON ta.idtipoactividad = ams.idtipoactividad )");
                consulta =
                     string.Format(
                         query + " exp {0}",
                         CrearSeccionWhereFiltroExpediente(filtro, true));
            }

            if (!consulta.Equals(Constantes.ConsultaNumExpedientes))
            {
                Constantes.ConsultaNumExpedientes = consulta;
                Constantes.NumExpedientes = long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            }

            return Constantes.NumExpedientes;
        }

        public DataSet ObtenerJustificaciones()
        {
            string consulta = string.Format("SELECT * FROM justificaciones_datosadicionales where inactivo = 0");
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }

        public DataSet ObtenerRiskLevel()
        {
            string consulta = string.Format("SELECT * FROM nivel_riesgo_HCP_datosadicionales");
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }

        public DataSet ObtenerTiposActividadPax()
        {
            string consulta = string.Format("SELECT * FROM tipo_actividad_pax_datosadicionales where Visible = 1");
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;

            return dt;
        }

        public DataSet ObtenerTiposAsistente()
        {
            string consulta = string.Format("SELECT * FROM tipo_asistente_datosadicionales where inactivo=0 order by idtipoasistente");
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }

        public DataSet ObtenerTiposAsistenteConCalculadora()
        {
            string consulta = string.Format("SELECT * FROM tipo_asistente_datosadicionales where idtipoABC=1 and inactivo=0 order by idtipoasistente");
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }

        #endregion "Obtener Expedientes"

        #region "Crear Seccion WHERE"

        private string CrearSeccionWhereFiltroExpedienteTemporal(FiltroExpedientesAvanzado filtro, bool count = false)
        {
            try
            {
                StringBuilder db = new StringBuilder();
                bool whereAppended = false;
                if (filtro.Roles != null)
                {
                    whereAppended = true;
                    filtro.Roles = filtro.Roles.ToLower().Replace("exp.idexpediente", "e.idxpediente");
                    filtro.Roles = filtro.Roles.ToLower().Replace("exp.idconfempresa", "e.idconfempresa");
                    db.AppendFormat("{0}", filtro.Roles);
                }
                if (whereAppended)
                {
                    db.AppendFormat("and e.idxpediente is not null");
                }
                else
                {
                    db.AppendFormat("where e.idxpediente is not null");
                }
                if (filtro.Actividad != null)
                {
                    db.AppendFormat(" and c2.Congreso like '{0}'", filtro.Actividad.Replace("'", "''"));
                }
                if (!string.IsNullOrEmpty(filtro.SortParameter))
                {
                    //Ismael Ameller 25-02-2011 Ordenar expedientes
                    db.AppendFormat(" ORDER BY exp.{0}", filtro.SortParameter);
                    //FIN Ismael Ameller 25-02-2011 Ordenar expedientes
                }
                else
                {
                    if (!count)
                    {
                        db.AppendFormat(" ORDER BY e.IDXPEDIENTE desc");
                    }
                }
                /*
                if (filtro.MaximumRows != null && filtro.MaximumRows > 0)
                {
                    db.AppendFormat(" LIMIT {0}", filtro.MaximumRows);
                }
                if (filtro.StartRowIndex != null && filtro.StartRowIndex > 0)
                {
                    db.AppendFormat(" OFFSET {0}", filtro.StartRowIndex);
                }
                */
                return db.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string CrearSeccionWhereFiltroExpediente(FiltroExpedientesAvanzado filtro, bool countMethod = false)
        {
            if (filtro == null) return null;

            //Procedimiento almacenado
            var listIdAmecs = _session.GetNamedQuery("SpObtenerAmecsPuedoVer").SetInt32("idPeticionario", filtro.IdPeticionarioSession).SetInt32("isCreateMode", 0).List<string>();

            StringBuilder db = new StringBuilder();

            bool primero = true;

            // A continuación creamos los filtros de las querys que precisan inner joins
            if (filtro.Asistente != null)
            {
                db.AppendFormat(
                    " inner join (select distinct (idexpediente) from cv_filtro_participante_expediente where participante like '{0}') par on exp.idexpediente = par.idexpediente ",
                    filtro.Asistente);
            }
            if (filtro.Producto != null)
            {
                //Ismael Ameller Vidal 23-03-2011 Filtramos por producto
                //db.AppendFormat(" inner join (select distinct (idamec) from cv_filtro_producto_amec where producto like '{0}' or productoempresa like '{0}') pro on exp.idamec = pro.idamec ", filtro.Producto);
                db.AppendFormat(
                    " inner join (select distinct (idexpediente) from cv_productos_expedientes where IdAreaProductoEmpresa='{0}') pro on exp.idexpediente=pro.idexpediente ",
                    filtro.Producto);
                //FIN Ismael Ameller Vidal 23-03-2011 Filtramos por producto
            }
            if (filtro.EstadoReserva != null)
            {
                db.AppendFormat(
                    " inner join (select distinct (idexpediente) from cv_filtro_estado_expediente where IdEstado = '{0}') est on exp.idexpediente = est.idexpediente ",
                    filtro.EstadoReserva);
            }
            if (filtro.ProveedorServicio != null)
            {
                db.AppendFormat(
                    " inner join (select distinct (idexpediente) from cv_filtro_proveedor_servicio where Proveedor like '{0}') pros on exp.idexpediente = pros.idexpediente ",
                    filtro.ProveedorServicio);
            }

            // Primero creamos los filtros relacionados con los roles
            if (filtro.Roles != null)
            {
                db.AppendFormat("{0}", filtro.Roles);
                primero = false;
            }

            ////Finalmente se crean los filtros de los campos normales
            //if (listIdAmecs.Count > 0)
            //{
            //    string filtroIdAmecs = "('";
            //    foreach (var idamec in listIdAmecs)
            //    {
            //        filtroIdAmecs += listIdAmecs.Count > listIdAmecs.IndexOf(idamec) + 1 ? idamec + "','" : idamec + "')";
            //    }
            //    AgregarAndSiProcede(db, ref primero);
            //    db.AppendFormat(" exp.amec IN {0} ", filtroIdAmecs);
            //}
            if (filtro.IdExpediente != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" exp.IDEXPEDIENTE = {0}", filtro.IdExpediente);
            }

            if (!string.IsNullOrWhiteSpace(filtro.TipoActividad))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" UPPER(exp.TIPOACTIVIDAD) like '%{0}%'", filtro.TipoActividad.ToUpper());
            }

            if (filtro.Actividad != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" ACTIVIDAD like '{0}'", filtro.Actividad);
            }
            if (filtro.EstadoExpediente != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" IDESTADO = '{0}'", filtro.EstadoExpediente);
            }
            if (filtro.Amec != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" AMEC like '{0}'", filtro.Amec);
            }
            if (filtro.Tipo != null)
            {
                if (filtro.Tipo == 2)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" IDTIPORESERVA = {0} ", "2");
                }
                else if (filtro.Tipo > 0)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" IDTIPORESERVA = {0} ", "1");
                    if (filtro.Tipo == 3)
                    {
                        AgregarAndSiProcede(db, ref primero);
                        db.AppendFormat(" TipoPagoFee = {0} ", "3");
                    }
                    else {
                        AgregarAndSiProcede(db, ref primero);
                        db.AppendFormat(" TipoPagoFee != {0} ", "3");
                    }
                }
            }
            if (filtro.Año != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" YEAR(FECHACREACION) = {0}", filtro.Año);
            }
            if (filtro.Mes != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" MONTH(FECHACREACION) = {0}", filtro.Mes);
            }
            //Ismael Ameller 24/02/2011 Corrección en filtros de expedientes por fecha (cuando es la misma fecha para el desde y el hasta)
            bool iguales = false;
            if ((filtro.Desde != null) && (filtro.Hasta != null))
            {
                if (filtro.Desde == filtro.Hasta)
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.AppendFormat(" FECHACREACION >= '{0}' AND FECHACREACION < '{1}'",
                        filtro.Desde.Value.ToString("yyyy/MM/dd"), filtro.Desde.Value.AddDays(1).ToString("yyyy/MM/dd"));
                    iguales = true;
                }
            }
            if ((filtro.Desde != null) && (iguales == false))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" FECHACREACION >= '{0}'", filtro.Desde.Value.ToString("yyyy/MM/dd"));
            }
            if ((filtro.Hasta != null) && (iguales == false))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" FECHACREACION <= '{0}'", filtro.Hasta.Value.AddDays(1).ToString("yyyy/MM/dd"));
            }
            //FIN Ismael Ameller 24/02/2011 Corrección en filtros de expedientes por fecha (cuando es la misma fecha para el desde y el hasta)
            if (filtro.Unidad != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" UNIDAD like '{0}'", filtro.Unidad);
            }
            if (filtro.AreaNegocio != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" AREA like '{0}'", filtro.AreaNegocio);
            }
            if (filtro.Region != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" REGION like '{0}'", filtro.Region);
            }
            if (filtro.Distrito != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" DISTRITO like '{0}'", filtro.Distrito);
            }
            if (filtro.Peticionario != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" PETICIONARIO like '{0}'", filtro.Peticionario);
            }
            if (filtro.Importe != null)
            {
                AgregarAndSiProcede(db, ref primero);
                string sCantidadSpa = string.Format(" IMPORTE {0} {1}", filtro.TipoFiltroImporte, filtro.Importe);
                sCantidadSpa = sCantidadSpa.Replace(',', '.');
                db.AppendFormat(" {0}", sCantidadSpa);
            }

            if (!string.IsNullOrEmpty(filtro.IdDepartament))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" iddepartament = {0}", filtro.IdDepartament);
            }
            if (!string.IsNullOrEmpty(filtro.IdSaleForce))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" idsaleforce = {0}", filtro.IdSaleForce);
            }
            if (!string.IsNullOrEmpty(filtro.IdDistrict))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" iddistrict = {0}", filtro.IdDistrict);
            }

            if (!string.IsNullOrEmpty(filtro.GroupParameter))
            {
                //Jose Laguna 02-03-2011 Agrupar por participantes si estamos pidiendo el resumen
                db.AppendFormat(" GROUP BY {0} ", filtro.GroupParameter);
                //FIN Jose Laguna 02-03-2011 Agrupar por participantes si estamos pidiendo el resumen
            }

            if (!string.IsNullOrEmpty(filtro.SortParameter))
            {
                //Ismael Ameller 25-02-2011 Ordenar expedientes
                db.AppendFormat(" ORDER BY exp.{0}", filtro.SortParameter);
                //FIN Ismael Ameller 25-02-2011 Ordenar expedientes
            }

            else
            {
                if (!countMethod)
                {
                    db.AppendFormat(" ORDER BY exp.Idexpediente desc");
                }
            }

            /*if (filtro.MaximumRows != null && filtro.MaximumRows > 0)
            {
                db.AppendFormat(" LIMIT {0}", filtro.MaximumRows);
            }
            if (filtro.StartRowIndex != null && filtro.StartRowIndex > 0)
            {
                db.AppendFormat(" OFFSET {0}", filtro.StartRowIndex);
            }*/

            return db.ToString();
        }

        /// <summary>
        /// Pauferrer 09/06/20111 Crea select filtro Actividad
        /// Carlos Serra añadir IdEventoFormulario
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        private string CrearSeccionSelectFiltroActividad(FiltroActividades filtro)
        {
            StringBuilder db = new StringBuilder();

            db.Append(" select cv_actividades_congreso.*, IdEventoFormulario,LinkGestorInvitados,case cv_actividades_congreso.idconfempresa when 1 then 'GP' when 2 then 'MT' when 3 then 'AMEX' else '' end as agencia ");

            return db.ToString();
        }

        /// <summary>
        /// Pau Ferrer  09-06-2011 Crea from filtro actividad
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        //Carlos Serra
        private string CrearSeccionFromFiltroActividad(FiltroActividades filtro)
        {
            StringBuilder db = new StringBuilder();

            if (filtro.isGestorInvitados) {
                db.Append(
                    " From cv_actividades_congreso inner join gestorinvitados on  cv_actividades_congreso.IdCongreso = gestorinvitados.IdCongreso and gestorinvitados.inactivo=0");
            }
            else
            {
                db.Append(
                    " From cv_actividades_congreso left join gestorinvitados on  cv_actividades_congreso.IdCongreso = gestorinvitados.IdCongreso and gestorinvitados.inactivo=0");
            }
            return db.ToString();
        }

        private string CrearSeccionWhereFiltroActividad(FiltroActividades filtro)
        {
            if (filtro == null) return null;

            StringBuilder db = new StringBuilder();
            string strSQL = string.Empty;

            bool primero = true;

            if (filtro.IdCongreso != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" cv_actividades_congreso.IdCongreso = {0}", filtro.IdCongreso);
            }
            if (filtro.NombreLike != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" Congreso like '{0}'", filtro.NombreLike.Replace("'", "''"));
            }
            if (filtro.IDPoblacion != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" IDPoblacion = {0}", filtro.IDPoblacion);
            }
            if (filtro.AmecLike != null)
            {
                AgregarAndSiProcede(db, ref primero);

                strSQL = "(";
                strSQL +=
                    String.Format(
                        "(cv_actividades_congreso.IdCongreso IN (SELECT idpeticionactividad FROM amec a inner join expediente e on a.idamec=e.idamec WHERE amec like '%{0}%')) ",
                        filtro.AmecLike);
                strSQL += " or ";
                strSQL +=
                    String.Format(
                        "(cv_actividades_congreso.IdCongreso IN (SELECT idcongreso FROM amec a inner join expediente e on a.idamec=e.idamec WHERE amec like '%{0}%'))",
                        filtro.AmecLike);
                strSQL += ")";

                db.Append(strSQL);
            }
            if (filtro.TipoActividad != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" IdTipoCongreso = {0}", filtro.TipoActividad);
            }
            if (filtro.Desde != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" Desde >= '{0}'", filtro.Desde.Value.ToString("yyyy/MM/dd"));
            }
            if (filtro.Hasta != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" Hasta <= '{0}'", filtro.Hasta.Value.ToString("yyyy/MM/dd"));
            }

            if (filtro.Publicar != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" Publicar = 1");
            }

            if (filtro.IdConfEmpresa.HasValue && filtro.IdConfEmpresa.Value > 0)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" (cv_actividades_congreso.IdConfEmpresa = " + filtro.IdConfEmpresa + " OR cv_actividades_congreso.IdConfEmpresa is null) ");
            }

            return db.ToString();
        }        

        private string CrearSeccionWhereFiltroActividadSort(FiltroActividades filtro)
        {
            StringBuilder db = new StringBuilder();            

            return db.ToString();
        }

        private string CrearSeccionWhereFiltroEventosForms(FiltroEForm filtro)
        {
            if (filtro == null) return null;

            StringBuilder db = new StringBuilder();
            string strSQL = string.Empty;
            bool primero = true;

            if (filtro.IdCongreso != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" gestorinvitados.IdCongreso = {0} AND gestorinvitados.inactivo=0",
                    filtro.IdCongreso);
            }

            return db.ToString();
        }

        private string CrearSeccionWhereFiltroEventosFormsSort(FiltroEForm filtro)
        {
            StringBuilder db = new StringBuilder();

            if (!string.IsNullOrEmpty(filtro.SortParameter))
            {
                db.AppendFormat(" ORDER BY {0}", filtro.SortParameter);
            }
            else
            {
                // Ordenamos por nombe por defecto
                db.AppendFormat(" ORDER BY DESCRIPCIONGESTOR");
            }
            if (filtro.StartRowIndex != null && filtro.StartRowIndex > 0)
            {
                db.AppendFormat(" OFFSET {0}", filtro.StartRowIndex);
            }

            return db.ToString();
        }

        private string CrearSeccionWhereFiltroAMECs(FiltroAMECs filtro)
        {
            if (filtro == null) return null;

            StringBuilder db = new StringBuilder();

            bool primero = true;

            // Primero creamos los filtros relacionados con los roles
            if (filtro.Roles != null)
            {
                db.AppendFormat("{0}", filtro.Roles);
            }

            if (filtro.IdAMEC != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" IdAmec = {0}", filtro.IdAMEC);
            }
            if (filtro.IdCongreso != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" IdCongreso = {0}", filtro.IdCongreso);
            }
            if (filtro.AMECLike != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" AMEC like '{0}'", filtro.AMECLike);
            }
            if (filtro.CongresoLike != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" Actividad like '{0}'", filtro.CongresoLike.Replace("'", "''"));
            }
            if (filtro.Solicitante != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" Peticionario like '{0}'", filtro.Solicitante);
            }
            if (filtro.Aprobado != null)
            {
                AgregarAndSiProcede(db, ref primero);
                // 7/1/2010: Ahora No-Aprobado es cuando Aprobado está a NO o cuando aún no ha aprobado (Aprobado = null)
                if (filtro.Aprobado == "0")
                    db.AppendFormat(" (Aprobado is null or Aprobado= '{0}') ", filtro.Aprobado);
                else
                    db.AppendFormat(" Aprobado = '{0}' ", filtro.Aprobado);
            }
            if (filtro.PendientesAprobar != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat("{0}", filtro.PendientesAprobar);
            }
            if (filtro.Año != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" YEAR(FechaAMEC) = {0}", filtro.Año);
            }
            if (filtro.Mes != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" MONTH(FechaAMEC) = {0}", filtro.Mes);
            }
            if (filtro.Importe != null)
            {
                AgregarAndSiProcede(db, ref primero);
                string sCantidadSpa = string.Format(" IMPORTE {0} {1}", filtro.TipoFiltroImporte, filtro.Importe);
                sCantidadSpa = sCantidadSpa.Replace(',', '.');
                db.AppendFormat(" {0}", sCantidadSpa);
            }

            if (!string.IsNullOrEmpty(filtro.SortParameter))
            {
                db.AppendFormat(" ORDER BY cva.{0}", filtro.SortParameter);
            }
            else
            {
                db.AppendFormat(" ORDER BY cva.IDAMEC");
            }

            if (filtro.MaximumRows != null && filtro.MaximumRows > 0)
            {
                db.AppendFormat(" LIMIT {0}", filtro.MaximumRows);
            }
            if (filtro.StartRowIndex != null && filtro.StartRowIndex > 0)
            {
                db.AppendFormat(" OFFSET {0}", filtro.StartRowIndex);
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

        #endregion "Crear Seccion WHERE"

        #region "Obtener IDs para inserción"

        public int ObtenerSiguienteIdExpediente()
        {
            string consulta = "SELECT ISNULL(MAX(idxpediente),0) FROM expediente;";
            int max = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));

            return ++max;
        }

        public int ObtenerSiguienteIdPeticionActividad()
        {
            string consulta = "SELECT ISNULL(MAX(idpeticionactividad),0) FROM peticiones_actividad;";
            int max = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));

            return ++max;
        }

        public int ObtenerSiguienteIdAMEC()
        {
            string consulta = "SELECT ISNULL(MAX(idamec),0) FROM amec;";
            int max = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));

            return ++max;
        }

        public string ObtenerSiguienteNombreAMEC(string sCodAgencia)
        {
            string consulta =
                string.Format("SELECT amec FROM amec where amec like '{0}%' order by amec desc limit 1;",
                    sCodAgencia);
            string codigo = Quodem.Sql.SqlServerClient.GetValue(consulta);

            int nContador = 0;
            if (!string.IsNullOrEmpty(codigo))
            {
                int.TryParse(codigo.Substring(2), out nContador);
            }
            string sCodigoFinal = string.Format("{0}{1:00000}", sCodAgencia, ++nContador);

            return sCodigoFinal;
        }

        public int ObtenerSiguienteIDCopia(string consulta)
        {
            int max = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            return ++max;
        }

        public int ObtenerSiguienteIdReserva()
        {
            return ObtenerSiguienteIDCopia("SELECT ISNULL(max(idreserva),0) from reservasviajes;");
        }

        public int ObtenerSiguienteIdServicio()
        {
            return ObtenerSiguienteIDCopia("SELECT ISNULL(max(idservicio),0) from serviciosreservasviajes;");
        }

        public int ObtenerSiguienteIdServicioActividad()
        {
            return ObtenerSiguienteIDCopia("SELECT ISNULL(max(idservicioactividad),0) from serviciosreservasactividades;");
        }

        public int ObtenerSiguienteIdServicioHotel()
        {
            return ObtenerSiguienteIDCopia("SELECT ISNULL(max(idserviciohotel),0) from serviciosreservashotel;");
        }

        public int ObtenerSiguienteIdServicioInscripcion()
        {
            return
                ObtenerSiguienteIDCopia("SELECT ISNULL(max(idservicioinscripcion),0) from serviciosreservasinscripciones;");
        }

        public int ObtenerSiguienteIdServicioTransporte()
        {
            return ObtenerSiguienteIDCopia("SELECT ISNULL(max(idserviciotransporte),0) from serviciosreservastransporte;");
        }

        public int ObtenerSiguienteIdPassengeActividad()
        {
            return
                ObtenerSiguienteIDCopia("SELECT ISNULL(max(idactividadpassengerlist),0) from actividades_passengers_list;");
        }

        public int ObtenerSiguienteIdEstadoReserva()
        {
            return ObtenerSiguienteIDCopia("SELECT ISNULL(max(idregistre),0) from estados_reservas;");
        }

        public int ObtenerSiguienteIdTramitacion()
        {
            return ObtenerSiguienteIDCopia("SELECT ISNULL(max(idtramitacion),0) from tramitacionesserviciosreservas;");
        }

        #endregion "Obtener IDs para inserción"

        #region "Actividades"


        public ICollection<DCabeceraActividad> ObtenerActividades(FiltroActividades filtro)
        {
            try
            {
                string consulta = string.Empty;
                if (filtro.isAdmin.HasValue && filtro.isAdmin.Value)
                {
                    consulta = string.Format("select IdCongreso, Congreso + case agencia when '' then '' else ' (' + agencia + ')' end  Congreso, IdTipoCongreso, TipoCongreso, IdPoblacion, Poblacion, IdProvincia, Provincia, Desde, Hasta, Comunicar, Internacional, IdValoracionfi, min(IdEventoFormulario) as IdEventoFormulario, min(LinkGestorInvitados) as LinkGestorInvitados, min(EsCongreso) as EsCongreso, min(idconfempresa) as idconfempresa from ({0} {1} {2}) a group by IdCongreso,agencia,Congreso,IdTipoCongreso,TipoCongreso,IdPoblacion,Poblacion,IdProvincia,Provincia,Desde,Hasta,Comunicar,Internacional,IdValoracionfi {3}", CrearSeccionSelectFiltroActividad(filtro), CrearSeccionFromFiltroActividad(filtro), CrearSeccionWhereFiltroActividad(filtro), CrearSeccionWhereFiltroActividadSort(filtro));
                }
                else if (filtro.newco.HasValue && filtro.newco.Value)
                {
                    consulta = string.Format("select IdCongreso, Congreso + case agencia when '' then '' else ' (' + agencia + ')' end  Congreso, IdTipoCongreso, TipoCongreso, IdPoblacion, Poblacion, IdProvincia, Provincia, Desde, Hasta, Comunicar, Internacional, IdValoracionfi, min(IdEventoFormulario) as IdEventoFormulario, min(LinkGestorInvitados) as LinkGestorInvitados, min(EsCongreso) as EsCongreso, min(idconfempresa) as idconfempresa from ({0} {1} {2}) a where a.IdCongreso not in (select idCongreso from amec where amec.IdCongreso = a.IdCongreso and a.EsCongreso = 1 and amec.newco = 0 UNION select idpeticionActividad from amec where amec.IdPeticionActividad = a.IdCongreso and a.EsCongreso = 0 and amec.newco = 0) group by IdCongreso,agencia,Congreso,IdTipoCongreso,TipoCongreso,IdPoblacion,Poblacion,IdProvincia,Provincia,Desde,Hasta,Comunicar,Internacional,IdValoracionfi {3}", CrearSeccionSelectFiltroActividad(filtro), CrearSeccionFromFiltroActividad(filtro), CrearSeccionWhereFiltroActividad(filtro), CrearSeccionWhereFiltroActividadSort(filtro));
                }
                else
                {
                    consulta = string.Format("select IdCongreso, Congreso + case agencia when '' then '' else ' (' + agencia + ')' end  Congreso, IdTipoCongreso, TipoCongreso, IdPoblacion, Poblacion, IdProvincia, Provincia, Desde, Hasta, Comunicar, Internacional, IdValoracionfi, min(IdEventoFormulario) as IdEventoFormulario, min(LinkGestorInvitados) as LinkGestorInvitados, min(EsCongreso) as EsCongreso, min(idconfempresa) as idconfempresa from ({0} {1} {2}) a where a.IdCongreso not in (select idCongreso from amec where amec.IdCongreso = a.IdCongreso and a.EsCongreso = 1 and amec.newco = 1 UNION select idpeticionActividad from amec where amec.IdPeticionActividad = a.IdCongreso and a.EsCongreso = 0 and amec.newco = 1) group by IdCongreso,agencia,Congreso,IdTipoCongreso,TipoCongreso,IdPoblacion,Poblacion,IdProvincia,Provincia,Desde,Hasta,Comunicar,Internacional,IdValoracionfi {3}", CrearSeccionSelectFiltroActividad(filtro), CrearSeccionFromFiltroActividad(filtro), CrearSeccionWhereFiltroActividad(filtro), CrearSeccionWhereFiltroActividadSort(filtro));
                }                

                if (filtro.MaximumRows != null && filtro.StartRowIndex != null && !filtro.isGestorInvitados)
                {
                    return _session.CreateSQLQuery(consulta).SetFirstResult(filtro.StartRowIndex.Value).SetMaxResults(filtro.MaximumRows.Value).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DCabeceraActividad))).List<DCabeceraActividad>();
                }

                return _session.CreateSQLQuery(consulta).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DCabeceraActividad))).List<DCabeceraActividad>();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public long ObtenerNumeroActividades(FiltroActividades filtro)
        {
            //Pau Ferrer  09-06-2011 modificacion consulta para filtro amec
            string consulta = string.Empty;
            if (filtro.isAdmin.HasValue && filtro.isAdmin.Value)
            {
                consulta = string.Format("select count(*) from (select * from ({0} {1} {2}) a group by IdCongreso, agencia, Congreso,IdTipoCongreso,TipoCongreso,IdPoblacion,Poblacion,IdProvincia,Provincia,Desde,Hasta,Comunicar,Internacional,IdValoracionfi,IdProveedor,Publicar,EsCongreso,IdEventoFormulario,LinkGestorInvitados,idconfempresa) b", CrearSeccionSelectFiltroActividad(filtro), CrearSeccionFromFiltroActividad(filtro), CrearSeccionWhereFiltroActividad(filtro));
            }
            else if (filtro.newco.HasValue && filtro.newco.Value)
            {
                consulta = string.Format("select count(*) from (select * from ({0} {1} {2}) a where a.IdCongreso not in (select idCongreso from amec where amec.IdCongreso = a.IdCongreso and a.EsCongreso = 1 and amec.newco = 0 UNION select idpeticionActividad from amec where amec.IdPeticionActividad = a.IdCongreso and a.EsCongreso = 0 and amec.newco = 0) group by IdCongreso, agencia, Congreso,IdTipoCongreso,TipoCongreso,IdPoblacion,Poblacion,IdProvincia,Provincia,Desde,Hasta,Comunicar,Internacional,IdValoracionfi,IdProveedor,Publicar,EsCongreso,IdEventoFormulario,LinkGestorInvitados,idconfempresa) b", CrearSeccionSelectFiltroActividad(filtro), CrearSeccionFromFiltroActividad(filtro), CrearSeccionWhereFiltroActividad(filtro));
            }
            else
            {
                consulta = string.Format("select count(*) from (select * from ({0} {1} {2}) a  where a.IdCongreso not in (select idCongreso from amec where amec.IdCongreso = a.IdCongreso and a.EsCongreso = 1 and amec.newco = 1 UNION select idpeticionActividad from amec where amec.IdPeticionActividad = a.IdCongreso and a.EsCongreso = 0 and amec.newco = 1) group by IdCongreso, agencia, Congreso,IdTipoCongreso,TipoCongreso,IdPoblacion,Poblacion,IdProvincia,Provincia,Desde,Hasta,Comunicar,Internacional,IdValoracionfi,IdProveedor,Publicar,EsCongreso,IdEventoFormulario,LinkGestorInvitados,idconfempresa) b", CrearSeccionSelectFiltroActividad(filtro), CrearSeccionFromFiltroActividad(filtro), CrearSeccionWhereFiltroActividad(filtro));                
            }

            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }
        
        public ICollection<DCabeceraEForm> ObtenerDatosEventosFormulario(FiltroEForm filtro)
        {
            string consulta = string.Format("select * from gestorinvitados {0} {1}", CrearSeccionWhereFiltroEventosForms(filtro), CrearSeccionWhereFiltroEventosFormsSort(filtro));
            return DCabeceraEForm.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public long ObtenerNumDatosEventosFormulario(FiltroEForm filtro)
        {
            string consulta = string.Format("select count(*) from gestorinvitados {0}", CrearSeccionWhereFiltroEventosForms(filtro));
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }

        #endregion "Actividades"

        #region "AMECs"

        public ICollection<DVAmecCongreso> ObtenerAMECs(FiltroAMECs filtro)
        {
            string consulta = string.Format("select cva.* from cv_amec_congreso cva {0}", CrearSeccionWhereFiltroAMECs(filtro));
            return DVAmecCongreso.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public long ObtenerNumeroAMECs(FiltroAMECs filtro)
        {
            string consulta = string.Format("SELECT count(*) FROM cv_amec_congreso cva {0}", CrearSeccionWhereFiltroAMECs(filtro));
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }


        //public ICollection<DVAmecCongresoConcatSolicitante> ObtenerAMECPorCongresoConcatSolicitante(int idcongreso,
        //    string AmecPorRoles, int amecReuniones, DVPeticionariosRoles roles, int? idconfempresa)
        //{
        //    string consulta = string.Format("({0}) union ({1}) union ({2}) union ({3})",
        //        CrearSeccionAmecViejo(idcongreso, idconfempresa), CrearSeccionAmecNoParaguas(roles.IdPeticionario, idconfempresa),
        //        CrearSeccionAmecDefecto(amecReuniones, idconfempresa), CrearSeccionAmecParaguas(roles, idconfempresa));

        //    return DVAmecCongresoConcatSolicitante.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        //}

        public ICollection<DVAmecCongresoConcatSolicitante> ObtenerAMECPorCongresoConcatSolicitanteProcedure(
            int idcongreso, string AmecPorRoles, int amecReuniones, DVPeticionariosRoles roles, int? idconfempresa, bool includeOldAmecs = true, string idAmecsLike = null)
        {
            string idregionSinRegion = "1";
            string idregion = string.Empty;
            string idarea = string.Empty;
            string iddistrito = string.Empty;
            string idunidad = string.Empty;
            int idpeticionario = roles.IdPeticionario;

            if (idconfempresa.HasValue && (idconfempresa.Value == 2 || idconfempresa.Value == 3))
            {

                string queryAmex = "select top 1 idpeticionario, idunidad, idarea, idregion, iddistrito from peticionarios where login = 'AMEX_" +roles.login + "'";
                if (idconfempresa.Value == 2)
                {
                    queryAmex = "select top 1 idpeticionario, idunidad, idarea, idregion, iddistrito from peticionarios where login = 'MT_" + roles.login + "'";
                }

                DataTable amexTable = Quodem.Sql.SqlServerClient.GetQuery(queryAmex);
                foreach (DataRow row in amexTable.Rows)
                {
                    idregionSinRegion = "300000001";

                    if (idconfempresa.Value == 2)
                    {
                        idregionSinRegion = "200000001";
                    }

                    idpeticionario = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idpeticionario");
                    string pidunidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idunidad");
                    string pidarea = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idarea");
                    string pidregion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idregion");
                    string piddistrito = Quodem.Utility.DataLayerUtil.GetStringValue(row, "iddistrito");

                    if (pidunidad != String.Empty)
                    {
                        idunidad = pidunidad;
                    }
                    else
                    {
                        idunidad = "null";
                    }

                    if (pidarea != String.Empty)
                    {
                        idarea = pidarea;
                    }
                    else
                    {
                        idarea = "null";
                    }

                    if (pidregion != String.Empty)
                    {
                        idregion = pidregion;
                    }
                    else
                    {
                        idregion = "null";
                    }

                    if (piddistrito != String.Empty)
                    {
                        iddistrito = piddistrito;
                    }
                    else
                    {
                        iddistrito = "null";
                    }
                }

                if (amexTable.Rows.Count == 0)
                {
                    if (roles.idregion == null)
                        idregion = "null";
                    else
                        idregion = roles.idregion.ToString();

                    if (roles.Idarea == null)
                        idarea = "null";
                    else
                        idarea = roles.Idarea.ToString();

                    if (roles.iddistrito == null)
                        iddistrito = "null";
                    else
                        iddistrito = roles.iddistrito.ToString();

                    if (roles.idunidad == null)
                        idunidad = "null";
                    else
                        idunidad = roles.idunidad.ToString();
                }
            }
            else
            {
                if (roles.idregion == null)
                    idregion = "null";
                else
                    idregion = roles.idregion.ToString();

                if (roles.Idarea == null)
                    idarea = "null";
                else
                    idarea = roles.Idarea.ToString();

                if (roles.iddistrito == null)
                    iddistrito = "null";
                else
                    iddistrito = roles.iddistrito.ToString();

                if (roles.idunidad == null)
                    idunidad = "null";
                else
                    idunidad = roles.idunidad.ToString();
            }
            string query = "";
            query += " ( ";
            query += " 	SELECT  ";
            query += " 		ams.idamecs as idamecs,  ";
            query += " 		max(amcong.IdAmec) as IdAmec,   ";
            query += " 		max(amcong.AMEC) as AMEC,  ";
            query += " 		max(amcong.IdCongreso) as IdCongreso,  ";
            query += " 		max(amcong.Actividad) as Actividad,  ";
            query += " 		max(ISNULL(ams.idsolicitante,0)) as idsolicitante,";
            query += " 		max(case when amcong.amec is null then rtrim(ltrim(cast(ams.idamecs as CHAR))) + ' ' else  rtrim(ltrim(amcong.amec)) end  + ' - ' + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) + ' - ' + SUBSTRING(ams.descripcion, 0, 30)) as  AmecConcatSolicitante ";
            query += " 	from peticionarios pet  ";
            query += " 	left join amecs ams on ";
            query += " 		pet.idpeticionario = ams.idsolicitante      ";
            query += " 	left join cv_amec_congreso amcong on  ";
            query += " 		amcong.idAMEC = ams.idamecs      ";
            query += " 	inner join unidorganizamec unids on  ";
            query += " 		unids.idamecs = ams.idamecs        ";
            query += " 	where ams.idamecs not like '4________' and ams.veeva = 0 AND ";

            if (!includeOldAmecs) {
                query += "  (ams.fechafinalizacion > DATEADD(YEAR, -1, GETDATE())) AND ";
            }

            if (idconfempresa.HasValue)
            {
                query += " 		ams.idconfempresa = " + idconfempresa.Value + " AND ";
            }

            query += " 	( ";
            //query += " 		ams.idestado <> 3 and ams.idestado <> 4 ";
            query += " 		ams.idestado = 1 ";
            query += " 	)  ";
            query += "     AND ";
            query += "     ( ";
            //CAMBIO MARGA ABRIR CREACIÓN DE EXPEDIENTES ASOCIADOS A CUALQUIER AMEC PARA ADMINISTRADORES
            query += "         (  ";
            query += "              (select administrador from peticionarios where idpeticionario = " + idpeticionario + " ) = 1 ";
            query += "         )  ";
            query += "         OR  ";
            //FIN CAMBIO MARGA ABRIR CREACIÓN DE EXPEDIENTES ASOCIADOS A CUALQUIER AMEC PARA ADMINISTRADORES
            query += " 		   ( ";
            query += " 			    ams.paraguas <> 1 and (ams.idcreadopor = " + idpeticionario + " or ams.idsolicitante = " + idpeticionario + ")  ";
            query += "         ) ";
            query += "         OR ";
            query += "         ( ";
            query += " 			ams.paraguas = 1 and ";
            query += " 			( ";
            query += " 				( ";
            query += " 					(" + idunidad + " is null and " + idarea + " is null and (" + idregion + " is null or " + idregion + " = " + idregionSinRegion + " ) and " + iddistrito + " is null) ";
            query += " 				)  ";
            query += " 				or ";
            query += " 				( ";
            query += " 					(" + idunidad + " is not null and " + idarea + " is null and (" + idregion + " is null or " + idregion + " = " + idregionSinRegion + " ) and " + iddistrito + " is null) and ";
            query += " 					(" + idunidad + " = unids.idunidad) ";
            query += " 				)  ";
            query += " 				or ";
            query += " 				( ";
            query += " 					(" + idunidad + " is not null and " + idarea + " is not null and (" + idregion + " is null or " + idregion + " = " + idregionSinRegion + " ) and " + iddistrito + " is null) and ";
            query += " 					( ";
            query += " 						(" + idunidad + " = unids.idunidad and " + idarea + " = unids.Idarea) or ";
            query += " 						(" + idunidad + " = unids.idunidad and unids.idarea is null and (unids.idregion is null or unids.idregion = " + idregionSinRegion + ") and unids.iddistrito is null) ";
            query += " 					) ";
            query += " 				)  ";
            query += " 				or ";
            query += " 				( ";
            query += " 					(" + idunidad + " is not null and " + idarea + " is not null and " + idregion + " is not null and " + iddistrito + " is null) and ";
            query += " 					( ";
            query += " 						(" + idunidad + " = unids.idunidad and " + idarea + " = unids.Idarea and " + idregion + " = unids.idregion) or ";
            query += " 						(" + idunidad + " = unids.idunidad and " + idarea + " = unids.idarea and (unids.idregion is null or unids.idregion = " + idregionSinRegion + ") and unids.iddistrito is null) or ";
            query += " 						(" + idunidad + " = unids.idunidad and unids.idarea is null and (unids.idregion is null or unids.idregion = " + idregionSinRegion + ") and unids.iddistrito is null) ";
            query += " 					)			 ";
            query += " 				)		 ";
            query += " 				or ";
            query += " 				( ";
            query += " 					(" + idunidad + " is not null and " + idarea + " is not null and (" + idregion + " is null or " + idregion + " = " + idregionSinRegion + " ) and " + iddistrito + " is not null) and ";
            query += " 					( ";
            query += " 						(" + idunidad + " = unids.idunidad and " + idarea + " = unids.Idarea and " + iddistrito + " = unids.iddistrito) or ";
            query += " 						(" + idunidad + " = unids.idunidad and " + idarea + " = unids.idarea and (unids.idregion is null or unids.idregion = " + idregionSinRegion + ") and unids.iddistrito is null) or ";
            query += " 						(" + idunidad + " = unids.idunidad and unids.idarea is null and (unids.idregion is null or unids.idregion = " + idregionSinRegion + ") and unids.iddistrito is null) ";
            query += " 					)			 ";
            query += " 				)	 ";
            query += " 				or ";
            query += " 				( ";
            query += " 					(" + idunidad + " is not null and " + idarea + " is not null and " + idregion + " is not null and " + iddistrito + " is not null) and ";
            query += " 					( ";
            query += " 						(" + idunidad + " = unids.idunidad and " + idarea + " = unids.Idarea and " + idregion + " = unids.idregion and " + iddistrito + " = unids.iddistrito) or ";
            query += " 						(" + idunidad + " = unids.idunidad and " + idarea + " = unids.Idarea and " + idregion + " = unids.idregion and unids.iddistrito is null) or ";
            query += " 						(" + idunidad + " = unids.idunidad and " + idarea + " = unids.Idarea and (unids.idregion is null or unids.idregion = 1) and unids.iddistrito is null) or ";
            query += " 						(" + idunidad + " = unids.idunidad and unids.idarea is null and (unids.idregion is null or unids.idregion = " + idregionSinRegion + ") and unids.iddistrito is null)                 ";
            query += " 					)			 ";
            query += " 				)	 ";
            query += " 				or ";
            query += " 				( ";
            query += " 					(" + idunidad + " is not null and " + idarea + " is null and " + idregion + " is not null and " + iddistrito + " is null) and ";
            query += " 					( ";
            query += " 						(" + idunidad + " = unids.idunidad and " + idregion + " = unids.idregion) or ";
            query += " 						(" + idunidad + " = unids.idunidad and unids.idarea is null and (unids.idregion is null or unids.idregion = " + idregionSinRegion + ") and unids.iddistrito is null)                 ";
            query += " 					)			 ";
            query += " 				)	 ";
            query += " 				or ";
            query += " 				( ";
            query += " 					(unids.idunidad is not null and unids.idregion is not null and unids.idarea is null and unids.iddistrito is null) and ";
            query += " 					( ";
            query += " 						(" + idunidad + " = unids.idunidad and " + idregion + " = unids.idregion)               ";
            query += " 					)			 ";
            query += " 				)	 ";
            query += " 			) ";
            query += " 		) ";
            query += "     ) ";
            query += "     group by ams.idamecs ";
            query += " ) ";

            query = string.Format(query + " union ({0}) union ({1}) ", CrearSeccionAmecViejo(idcongreso, idconfempresa, includeOldAmecs), CrearSeccionAmecDefecto(amecReuniones, idconfempresa, includeOldAmecs));
            DataTable result = Quodem.Sql.SqlServerClient.GetQuery(query).Copy();
            result.Merge(Quodem.Sql.SqlServerClient.GetQuery(CrearSeccionAmecNuevo(roles.IdPeticionario, idconfempresa, includeOldAmecs)));
            if ((!roles.newco.HasValue || !roles.newco.Value) || (roles.administrador.HasValue && roles.administrador.Value))
            {
                result.Merge(Quodem.Sql.SqlServerClient.GetQuery(CrearSeccionAmecVeeva(roles.IdPeticionario, idconfempresa, includeOldAmecs, idAmecsLike)));
            }
            if ((roles.newco.HasValue && roles.newco.Value) || (roles.administrador.HasValue && roles.administrador.Value))
            {
                result.Merge(Quodem.Sql.SqlServerClient.GetQuery(CrearSeccionAmecNewco(roles.IdPeticionario, idconfempresa, includeOldAmecs)));
            }
            return DVAmecCongresoConcatSolicitante.ConvertToDto(result);
            
        }


        public ICollection<DAmec> ObtenerEntidadAMECs(FiltroAMECs filtro)
        {
            string consulta = string.Format("select * from amec cva {0}", CrearSeccionWhereFiltroAMECs(filtro));
            return DAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public int NumeroExpedientesPorAmec(string NumAmec)
        {
            string consulta = string.Format("select count(*) from amec am right join expediente ex on ex.idamec = am.idamec where am.amec = '{0}'", NumAmec);
            return Convert.ToInt32(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }

        public ICollection<DExpediente> ExpedientesPorAmec(string NumAmec)
        {
            string consulta = string.Format("select * from amec am right join expediente ex on ex.idamec = am.idamec where ex.idestado!= 'AN' and ex.idestado != 'CN' and am.amec = '{0}'", NumAmec);
            return DExpediente.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        #endregion "AMECs"

        #region "Servicios"

        public DVCongresos ObtenerCongreso(int idActividad)
        {
            string consulta = string.Format("SELECT * FROM cv_congresos c where idcongreso={0}", idActividad);
            return DVCongresos.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
        }

        private IEnumerable<DVServicioHotel> ObtenerTramitacionesAlternativaHotel(int idExpediente, enAlternativa eAlt)
        {
            Dictionary<int, DVServicioHotel> dicTramitaciones = new Dictionary<int, DVServicioHotel>();
            string Proveedor;

            switch (eAlt)
            {
                case enAlternativa.alternativa1:
                    Proveedor = "1";
                    break;
                case enAlternativa.alternativa2:
                    Proveedor = "2";
                    break;
                case enAlternativa.alternativa3:
                    Proveedor = "3";
                    break;
                default:
                    Proveedor = string.Empty;
                    break;
            }

            string consultaIdConfEmpresa = string.Format("select idconfempresa from amec where idamec = (select top 1 idamec from expediente where idxpediente = {0})", idExpediente);
            string idConfEmpresa = Quodem.Sql.SqlServerClient.GetValue(consultaIdConfEmpresa);

            if (string.IsNullOrWhiteSpace(idConfEmpresa)) {
                idConfEmpresa = "1";
            }

            string consulta = string.Format(@"select
			vis.idexpediente,vis.idreserva,vis.idservicio,vis.idserviciohotel,
			p.Proveedor as hotel,
			vis.fechahorallegada,vis.fechahorasalida,vis.idtipohab,
			vis.pvp,vis.observaciones, vis.pax, vis.cotizado,vis.IdEstado
            from cv_serviciohoteles vis
			    left join tramitacionesserviciosreservas tramitaciones on
				    tramitaciones.fkidservicio = vis.idservicio
                left join proveedores p on p.idproveedor = tramitaciones.IdProveedor{2} and p.IdConfEmpresa = {3}
            where idexpediente = {0} AND ok{1} = 'AC'", new object[] { idExpediente, eAlt, Proveedor, idConfEmpresa });

            return DVServicioHotel.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public IEnumerable<DVServicioHotel> ObtenerServiciosHoteles(int idExpediente)
        {
            Dictionary<int, DVServicioHotel> dicTramitaciones = new Dictionary<int, DVServicioHotel>();

            string consulta = string.Format("select * from cv_serviciohoteles where idexpediente = {0}", idExpediente);
            IEnumerable<DVServicioHotel> lstServHotel = DVServicioHotel.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));

            //Obtener cotizaciones alt1
            IEnumerable<DVServicioHotel> lstAlternativa1 = ObtenerTramitacionesAlternativaHotel(idExpediente,
                enAlternativa.alternativa1);

            //Obtener cotizaciones alt2
            IEnumerable<DVServicioHotel> lstAlternativa2 = ObtenerTramitacionesAlternativaHotel(idExpediente,
                enAlternativa.alternativa2);

            //Obtener cotizaciones alt3
            IEnumerable<DVServicioHotel> lstAlternativa3 = ObtenerTramitacionesAlternativaHotel(idExpediente,
                enAlternativa.alternativa3);

            foreach (var alternativa1 in lstAlternativa1)
            {
                if (dicTramitaciones.ContainsKey(alternativa1.idservicio))
                {
                    //si se duplica una clave tramitacionesserviciosreservas ya no dara error la aplicación.
                    Console.Write("clave duplicada evitada " + alternativa1.idservicio + " en RepositorioExpedientes.cs");
                }
                else
                {
                    dicTramitaciones.Add(alternativa1.idservicio, alternativa1);
                }
            }

            foreach (var alternativa2 in lstAlternativa2)
            {
                dicTramitaciones.Add(alternativa2.idservicio, alternativa2);
            }

            foreach (var alternativa3 in lstAlternativa3)
            {
                dicTramitaciones.Add(alternativa3.idservicio, alternativa3);
            }

            foreach (DVServicioHotel oDVServHotel in lstServHotel)
            {
                if (dicTramitaciones.ContainsKey(oDVServHotel.idservicio))
                {
                    oDVServHotel.hotel = dicTramitaciones[oDVServHotel.idservicio].hotel;
                }
            }

            return lstServHotel;
        }

        private IEnumerable<DVServicioActividades> ObtenerTramitacionesAlternativaOtros(int idExpediente,
            enAlternativa eAlt)
        {
            Dictionary<int, DVServicioActividades> dicTramitaciones = new Dictionary<int, DVServicioActividades>();

            string consulta = string.Format(
                @"select vis.idexpediente,vis.idreserva,vis.idservicio,vis.idservicioactividad,vis.idtarifaactividad,vis.observaciones,vis.pvp,
                        vis.locked,vis.sede,{1} AS Descripcion,vis.tipo,vis.fechainicio,vis.horainicio,vis.minutosinicio,vis.fechafin,vis.horafin,
                        vis.paxvis,vis.minutosfin,vis.IdEstado,vis.pax,vis.cotizado
            from cv_servicioactividades vis
			    left join tramitacionesserviciosreservas tramitaciones on
				    tramitaciones.fkidservicio = vis.idservicio
            where idexpediente = {0} AND ok{1} = 'AC'", idExpediente, eAlt);

            return DVServicioActividades.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)); ;
        }

        public IEnumerable<DVServicioActividades> ObtenerOtrosServicios(int idExpediente)
        {
            Dictionary<int, DVServicioActividades> dicTramitaciones = new Dictionary<int, DVServicioActividades>();
            string consulta = string.Format("select * from cv_servicioactividades where idexpediente = {0}",
                idExpediente);

            IEnumerable<DVServicioActividades> lstServOtros = DVServicioActividades.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));

            //Obtener cotizaciones alt1
            IEnumerable<DVServicioActividades> lstAlternativa1 = ObtenerTramitacionesAlternativaOtros(idExpediente,
                enAlternativa.alternativa1);

            //Obtener cotizaciones alt2
            IEnumerable<DVServicioActividades> lstAlternativa2 = ObtenerTramitacionesAlternativaOtros(idExpediente,
                enAlternativa.alternativa2);

            //Obtener cotizaciones alt3
            IEnumerable<DVServicioActividades> lstAlternativa3 = ObtenerTramitacionesAlternativaOtros(idExpediente,
                enAlternativa.alternativa3);

            foreach (var alternativa1 in lstAlternativa1)
            {
                dicTramitaciones.Add(alternativa1.idservicio, alternativa1);
            }

            foreach (var alternativa2 in lstAlternativa2)
            {
                dicTramitaciones.Add(alternativa2.idservicio, alternativa2);
            }

            foreach (var alternativa3 in lstAlternativa3)
            {
                dicTramitaciones.Add(alternativa3.idservicio, alternativa3);
            }

            foreach (var oDVServOtros in lstServOtros)
            {
                if (dicTramitaciones.ContainsKey(oDVServOtros.idservicio))
                {
                    oDVServOtros.Descripcion = dicTramitaciones[oDVServOtros.idservicio].Descripcion;
                }
            }

            return lstServOtros;
        }

        private IEnumerable<DVServicioInscripciones> ObtenerTramitacionesAlternativaInscripcion(int idExpediente,
            enAlternativa eAlt)
        {
            Dictionary<int, DVServicioInscripciones> dicTramitaciones = new Dictionary<int, DVServicioInscripciones>();

            string consulta = string.Format(
                @"select
                    vis.idexpediente,vis.idreserva,vis.idservicio,vis.idservicioinscripcion,{1} AS inscripcion,vis.pvp,vis.observaciones,vis.pax,vis.cotizado,
                    vis.IdEstado,vis.descripcion
            from cv_servicioinscripciones vis
			    left join tramitacionesserviciosreservas tramitaciones on
				    tramitaciones.fkidservicio = vis.idservicio
            where idexpediente = {0} AND ok{1} = 'AC'", idExpediente, eAlt);

            return DVServicioInscripciones.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public IEnumerable<DVServicioInscripciones> ObtenerServiciosInscripciones(int idExpediente)
        {
            Dictionary<int, DVServicioInscripciones> dicTramitaciones = new Dictionary<int, DVServicioInscripciones>();

            string consulta = string.Format("select * from cv_servicioinscripciones where idexpediente = {0}", idExpediente);

            IEnumerable<DVServicioInscripciones> lstServInscripcion = DVServicioInscripciones.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));

            //Obtener cotizaciones alt1
            IEnumerable<DVServicioInscripciones> lstAlternativa1 =
                ObtenerTramitacionesAlternativaInscripcion(idExpediente, enAlternativa.alternativa1);

            //Obtener cotizaciones alt2
            IEnumerable<DVServicioInscripciones> lstAlternativa2 =
                ObtenerTramitacionesAlternativaInscripcion(idExpediente, enAlternativa.alternativa2);

            //Obtener cotizaciones alt3
            IEnumerable<DVServicioInscripciones> lstAlternativa3 =
                ObtenerTramitacionesAlternativaInscripcion(idExpediente, enAlternativa.alternativa3);

            foreach (var alternativa1 in lstAlternativa1)
            {
                dicTramitaciones.Add(alternativa1.idservicio, alternativa1);
            }

            foreach (var alternativa2 in lstAlternativa2)
            {
                dicTramitaciones.Add(alternativa2.idservicio, alternativa2);
            }

            foreach (var alternativa3 in lstAlternativa3)
            {
                dicTramitaciones.Add(alternativa3.idservicio, alternativa3);
            }

            foreach (DVServicioInscripciones oDVServInscripcion in lstServInscripcion)
            {
                if (dicTramitaciones.ContainsKey(oDVServInscripcion.idservicio))
                {
                    oDVServInscripcion.inscripcion = dicTramitaciones[oDVServInscripcion.idservicio].inscripcion;
                }
            }

            return lstServInscripcion;
        }

        public IEnumerable<DVServicioTransporte> ObtenerServiciosTransportes(int idExpediente)
        {
            string consulta = string.Format("select *, null as observaciones from cv_serviciotransportes where idexpediente = {0}", idExpediente);

            return DVServicioTransporte.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public DVServicioTransporte ObtenerServicioTransporte(int idExpediente)
        {
            string consulta = string.Format("select * from cv_serviciotransportes where idexpediente = {0}",
                idExpediente);

            return DVServicioTransporte.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
        }

        public ICollection<DVResumenEstados> ObtenerResumenEstados(string sFiltroRol, bool newQuery)
        {
            string sConsultaFinal;
            if (string.IsNullOrEmpty(sFiltroRol))
            {
                sConsultaFinal = string.Format("select * from cv_resumen_estados");
            }
            else
            {
                if (newQuery)
                {
                    sConsultaFinal = " select ";
                    sConsultaFinal += " 	count(res.IdExpediente) as Total, ";
                    sConsultaFinal += " res.IdEstado as IdEstado ";
                    sConsultaFinal += " FROM cv_resumen_estados_aux1 res ";
                    sConsultaFinal += " inner join cv_amec_expediente exp on ";
                    sConsultaFinal += " res.idexpediente = exp.idxpediente ";

                    sConsultaFinal = string.Format(sConsultaFinal + " {0} group by res.IdEstado", sFiltroRol);
                }
                else
                {
                    string sConsultaRoles = string.Format(
                        "select exp.IdExpediente from cv_expedientes_total_fa exp {0}",
                        sFiltroRol);
                    string sConsultaInner =
                        string.Format(
                            "select res.IdExpediente, res.IdEstado FROM cv_resumen_estados_aux1 res inner join ({0}) exp on res.IdExpediente = exp.IdExpediente",
                            sConsultaRoles);
                    sConsultaFinal =
                        string.Format(
                            "select IdEstado AS IdEstado,count(IdExpediente) AS Total from ({0}) final group by IdEstado",
                            sConsultaInner);
                }
            }

            return DVResumenEstados.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(sConsultaFinal));
        }

        public ICollection<DVServicioPassengerResumen> ObtenerResumenParticipantes(FiltroExpedientesAvanzado filtro)
        {
            string consulta = " select  ";
            consulta += " cv.IdPassengerList,cv.IdExpediente,cv.Nombre,cv.Apel1,cv.Apel2, ";
            consulta += " max(cv.IdServicioINS) as IdServicioINS, max(cv.IdSerResINS) as IdSerResINS, max(cv.IdReservaINS) as IdReservaINS, max(cv.IdEstadoINS) as IdEstadoINS, max(cv.IdServicioDSP) as IdServicioDSP, max(cv.IdSerResDSP) as IdSerResDSP, max(cv.IdReservaDSP) as IdReservaDSP, max(cv.IdEstadoDSP) as IdEstadoDSP, max(cv.IdServicioHOT) as IdServicioHOT, max(cv.IdSerResHOT) as IdSerResHOT, max(cv.IdReservaHOT) as IdReservaHOT, max(cv.IdEstadoHOT) as IdEstadoHOT, max(cv.IdServicioACT) as IdServicioACT, max(cv.IdSerResACT) as IdSerResACT, max(cv.IdReservaACT) as IdReservaACT, max(cv.IdEstadoACT) as IdEstadoACT, max(cv.ImporteINS) as ImporteINS, max(cv.ImporteDSP) as ImporteDSP, max(cv.ImporteHOT) as ImporteHOT, max(cv.ImporteACT) as ImporteACT, max(cv.ImporteFee) as ImporteFee, max(cv.Total) as Total ";
            consulta += " from cv_ser_passenger_resumen_fee cv   ";
            consulta += " WHERE  ";
            consulta += "	cv.IDEXPEDIENTE = " + filtro.IdExpediente + " and ";
            consulta += "	( ";
            consulta += "	    ((cv.IdReservaINS is not null and cv.IdEstadoINS != 'CN' and cv.IdEstadoINS != 'AN' and cv.IdEstadoINS != 'CNTR') or (cv.IdReservaINS is null) ) or ";
            consulta += "	    cv.IdReservaDSP is not null or ";
            consulta += "	    cv.IdReservaHOT is not null or ";
            consulta += "	    cv.IdReservaACT is not null ";
            consulta += "	) ";
            consulta += " GROUP BY cv.IdPassengerList,cv.IdExpediente,cv.Nombre,cv.Apel1,cv.Apel2  ORDER BY cv.IDEXPEDIENTE desc    ";

            return DVServicioPassengerResumen.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public long ObtenerNumeroResumenParticipantes(FiltroExpedientesAvanzado filtro)
        {
            string consulta = string.Format("SELECT count(*) FROM cv_ser_passenger_resumen exp {0}", CrearSeccionWhereFiltroExpediente(filtro, true));
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }

        #endregion "Servicios"

        public ICollection<DAuxId> ObtenerIDs(string sConsulta)
        {
            return DAuxId.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(sConsulta));
        }

        public ICollection<DAuxIdSer> ObtenerIDsSer(string sConsulta)
        {
            return DAuxIdSer.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(sConsulta));
        }

        //JLV 10052012----------------- start
        // Obtener lista de reservas por expediente a iterar
        // validar estado amec de reserva
        // validar presupuesto amec de la reserva
        // Si validaciones OK entonces Update de estado
        // Se validaciones KO activar flag de advertencia y siguiente registro
        public int CambiaEstadoExpedienteEnviado(int nIDExpediente)
        {
            string consulta;
            DbCommand comando;
            bool flgAdvEstadoAmec = false;
            bool flgAdvPresupuestoAmec = false;
            int nCambios = 0;

            //Obtener listaServicios
            ICollection<DAuxId> colReservasViajes = ObtenerIDs(string.Format("select rv.idreserva as IDENTIFICADOR from expediente exp inner join reservasviajes rv on exp.idxpediente=rv.fkIdExpediente inner join serviciosreservasviajes srv on srv.idreserva=rv.idreserva where idxpediente = {0} and  rv.idestado in ('AB')", nIDExpediente));
            foreach (DAuxId reservaID in colReservasViajes)
            {
                consulta = string.Format("select dbo.validar_estado_amec(srv.idservicio) from expediente exp inner join reservasviajes rv on exp.idxpediente=rv.fkIdExpediente inner join serviciosreservasviajes srv on srv.idreserva=rv.idreserva where idxpediente = {0} and rv.idreserva = {1} and rv.idestado in ('AB')", nIDExpediente, reservaID.IDENTIFICADOR);
                int nResult = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
                if (nResult == 1) //Si estado amec es true valido presupuesto amec
                {
                    //LJM 25/01/2018 - Eliminar restricción
                    //consulta = string.Format("select dbo.validar_presupuesto_amec(srv.idservicio) from expediente exp inner join reservasviajes rv on exp.idxpediente=rv.fkIdExpediente inner join serviciosreservasviajes srv on srv.idreserva=rv.idreserva where idxpediente = {0} and rv.idreserva = {1} and rv.idestado in ('AB')", nIDExpediente, reservaID.IDENTIFICADOR);
                    //nResult = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
                    //if (nResult == 1) //Si presupuesto amec es true entonces update del estado de la reserva
                    //{
                    consulta = string.Format("update reservasviajes set idestado = 'CR', LastUpd = getdate() where fkidexpediente ={0} and idreserva = {1} and idestado in ('AB')", nIDExpediente, reservaID.IDENTIFICADOR);
                        nCambios += Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
                    //}
                    //else // Marco el flag de advertencia presupuesto amec no valido -2
                    //{
                    //    flgAdvPresupuestoAmec = true;
                    //}
                }
                else // Marco el flag de advertencia estado amec no valido -1
                {
                    flgAdvEstadoAmec = true;
                }
            }


            if (flgAdvEstadoAmec == true)
            {
                return -1;
            }
            else if (flgAdvPresupuestoAmec == true)
            {
                return -2;
            }
            else // Si ha existido algun cambio he de actualizar el estado del expediente
            {
                return nCambios;
            }
        }

        // Obtener lista de reservas por expediente a iterar
        // validar estado amec de reserva
        // validar presupuesto amec de la reserva
        // Si validaciones OK entonces Update de estado
        // Se validaciones KO sumar el count de advertencia y siguiente registro
        public int CambiaEstadoExpedienteAprobado(int nIDExpediente, ref int nValidaEstadoAmec,
            ref int nValidaPresupAmec)
        {
            string consulta;
            DbCommand comando;
            int nCambios = 0;

            //Obtener listaServicios
            ICollection<DAuxId> colReservasViajes =
                ObtenerIDs(
                    string.Format(
                        "select rv.idreserva as IDENTIFICADOR from expediente exp inner join reservasviajes rv on exp.idxpediente=rv.fkIdExpediente inner join serviciosreservasviajes srv on srv.idreserva=rv.idreserva where idxpediente = {0} and  rv.idestado in ('CTZ')",
                        nIDExpediente));
            foreach (DAuxId reservaID in colReservasViajes)
            {
                int nResult = 0;
                //Valido estado AMEC
                consulta = string.Format("select dbo.validar_estado_amec(srv.idservicio) from expediente exp inner join reservasviajes rv on exp.idxpediente=rv.fkIdExpediente inner join serviciosreservasviajes srv on srv.idreserva=rv.idreserva where idxpediente = {0} and rv.idreserva = {1} and rv.idestado in ('CTZ')", nIDExpediente, reservaID.IDENTIFICADOR);
                nResult = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
                if (nResult == 1) //Si estado amec es true valido presupuesto amec
                {
                    //LJM 12/01/2018 - Eliminar restricción - Aunque sl importe del amec se inferior al del expediente puedan ser aprobado
                    //consulta = string.Format("select dbo.validar_presupuesto_amec(srv.idservicio) from expediente exp inner join reservasviajes rv on exp.idxpediente=rv.fkIdExpediente inner join serviciosreservasviajes srv on srv.idreserva=rv.idreserva where idxpediente = {0} and rv.idreserva = {1} and rv.idestado in ('CTZ')", nIDExpediente, reservaID.IDENTIFICADOR);
                  
                    //nResult = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
                    //if (nResult == 1) //Si presupuesto amec es true entonces update del estado de la reserva
                    //{
                        consulta = string.Format("update reservasviajes set idestado = 'CFP', LastUpd = getdate() where fkidexpediente ={0} and idreserva = {1} and idestado in ('CTZ')", nIDExpediente, reservaID.IDENTIFICADOR);
                        nCambios += Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
                    //}
                    //else // Marco el flag de advertencia presupuesto amec no valido -2
                    //{
                       //nValidaPresupAmec += 1;
                    //}
                }
                else // Marco el flag de advertencia estado amec no valido -1
                {
                    nValidaEstadoAmec += 1;
                }
            }
            return nCambios;
        }


        public int CambiaEstadoExpedienteCancelado(int nIDExpediente)
        {
            string consulta = string.Format("UPDATE expediente SET idestado = 'AN' WHERE idxpediente = {0} and idestado in ('AB', 'NC')", nIDExpediente);
            int nCambios = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            consulta = string.Format("UPDATE expediente SET idestado = 'CN' WHERE idxpediente = {0} and idestado in ('CN', 'FZ')", nIDExpediente);
            int nCambiosExpCN = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
            nCambios = (nCambios <= 0) ? nCambiosExpCN : nCambios;

            consulta = string.Format("update reservasviajes set idestado = 'AN', LastUpd = getdate() where fkidexpediente ={0} and idestado in ('AB','CR','CTZ','CFG','CTZD')", nIDExpediente);
            int nCambiosAN = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            consulta = string.Format("update reservasviajes set idestado = 'CN', LastUpd = getdate() where fkidexpediente ={0} and idestado in ('CFP', 'PTR','TR')", nIDExpediente);
            int nCambiosCN = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            return nCambios;
        }

        public DEmpGpPetAmecExp ObtenerEmpleadoGpPorPetAmecExp(int idPet, string idAmec, int idExp)
        {
            string consulta = string.Format("SELECT * FROM cv_empleadogp_by_peticionario_expediente_amec WHERE idPeticionario = {0} AND amec = '{1}' AND idxpediente = {2}", idPet,idAmec,idExp);
            return DEmpGpPetAmecExp.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
        }

        public int CambiaEstadoExpedienteFinalizado(int idexpediente)
        {
            string consulta = string.Format("UPDATE expediente SET idestado = 'FZ' WHERE idxpediente = {0}", idexpediente);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        public int CambiaEstadoServicio(int nIDReserva, string sIDNuevoEstado)
        {
            string consulta = string.Format("UPDATE reservasviajes SET idestado = '{1}', LastUpd = getdate() WHERE idreserva = {0}", nIDReserva, sIDNuevoEstado);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        /// <summary>
        /// Cambios JMM -> Ticket SOPMSDEOS-230
        /// </summary>
        /// <param name="nIDReserva"></param>
        /// <returns></returns>
        public int CambiaEstadoServicioCancelado(int nIDReserva)
        {
            string consulta = string.Format("update reservasviajes set idestado = 'AN', LastUpd = getdate() where idreserva={0} and idestado in ('AB','CFG','CR','CTZ','CTZD')", nIDReserva);
            int nCambiosAN = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            consulta = string.Format("update reservasviajes set idestado = 'CN', LastUpd = getdate() where idreserva={0} and idestado in ('CFP','PTR','TR') ", nIDReserva);
            int nCambiosCN = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            return (nCambiosAN + nCambiosCN);
        }

        public bool PuedeAprobar(int nIDExpediente)
        {
            bool bPuedeAprobar = false;

            // Primero comprueba que haya servicios en estado Cotizado. Si no los hubiera ya sabemos que no puede aprobar.
            string consulta = string.Format("select count(*) from reservasviajes where fkidexpediente={0} and idestado in ('CTZ')", nIDExpediente);
            long nCotizados = long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            //Ismael Ameller 22/02/2011 Deja aprobar aunque el estado del expediente sea en curso
            if (nCotizados == 0)
            {
                return bPuedeAprobar;
            }
            else
            {
                bPuedeAprobar = true;
                return bPuedeAprobar;
            }
        }

        public string GetEstado(int idexpediente)
        {
            string consulta = string.Format("select idestado from expediente where idxpediente = {0}", idexpediente);
            return Quodem.Sql.SqlServerClient.GetValue(consulta);
        }

        public bool EsCancelableAMEC(string nIDAmec)
        {
            string consulta = string.Format("select dbo.validar_cancelacion_amec('{0}')", nIDAmec);
            int nReservas = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));

            return (nReservas == 1);
        }

        public int CrearCopiaExpediente(int nIDExpediente, DDatosPersonalesUsuario datosUsuario)
        {
            int nIDNuevoExp = ObtenerSiguienteIdExpediente();
            string consulta = string.Format(
                    "insert into expediente (idxpediente, idregion, idamec, expediente, idunidad, Idarea, IdPeticionario, IdTipoReserva, IdEmpresa, idestado, iddistrito, fechacreacion, idempleadogp, locked, codexpediente, urgente, importeTotal, iddepartament, idsaleforce, iddistrict) select {0}, {3},idamec, expediente, {4}, {5}, {6}, IdTipoReserva, {7}, 'AB', {8}, '{2}', idempleadogp, locked, codexpediente, urgente, importeTotal, {9}, {10}, {11} from expediente where idxpediente={1}",
                    nIDNuevoExp, nIDExpediente, System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                    datosUsuario.idregion.HasValue ? datosUsuario.idregion.Value.ToString() : "null",
                    datosUsuario.idunidad.HasValue ? datosUsuario.idunidad.Value.ToString() : "null",
                    datosUsuario.Idarea.HasValue ? datosUsuario.Idarea.Value.ToString() : "null",
                    datosUsuario.IdPeticionario, datosUsuario.FKIdEmpresa,
                    datosUsuario.iddistrito.HasValue ? datosUsuario.iddistrito.ToString() : "null",
                    datosUsuario.iddepartament.HasValue ? datosUsuario.iddepartament.ToString() : "null",
                    datosUsuario.idsaleforce.HasValue ? datosUsuario.idsaleforce.ToString() : "null",
                    datosUsuario.iddistrict.HasValue ? datosUsuario.iddistrict.ToString() : "null"
                    );

            Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            // Copia las Reservas de este expediente
            CrearCopiaReservas(nIDNuevoExp, nIDExpediente);

            // Copia los participantes
            ICollection<DAuxId> colPassengersList = ObtenerIDs(string.Format("select idreservapassengerlist as IDENTIFICADOR from reservas_passengers_list where idxpediente = {0}", nIDExpediente));
            foreach (DAuxId reservaID in colPassengersList)
            {
                string consultaReserva = string.Format("insert into reservas_passengers_list select (select max(idreservapassengerlist)+1 from reservas_passengers_list), idpassengerlist,locked, {0} from reservas_passengers_list where idreservapassengerlist={1}", nIDNuevoExp, reservaID.IDENTIFICADOR);
                Quodem.Sql.SqlServerClient.ExecuteQuery(consultaReserva);
            }

            return nIDNuevoExp;
        }

        public int CrearCopiaReservas(int nIdExpedienteNew, int nIdExpedienteOld)
        {
            // Recorre todas las reservas de este Expediente
            ICollection<DAuxId> colReservasViajes =
                ObtenerIDs(string.Format("select idreserva as IDENTIFICADOR from reservasviajes where fkidexpediente = {0} and idestado <> 'AN' and idestado <> 'CN' and idestado <> 'CNTR'", nIdExpedienteOld));
            foreach (DAuxId reservaID in colReservasViajes)
            {
                int nSgteReserva = ObtenerSiguienteIdReserva();
                string consultaReserva =
                    string.Format(
                        "insert into reservasviajes (idreserva, reserva, fechapeticion, idestado, LastUpd, LastLog, IdPeticionario, Observaciones, observ_agencia, mainreserva, fkidexpediente, locked) select {2}, reserva, '{3}', 'AB', LastUpd, LastLog, IdPeticionario, Observaciones, observ_agencia, mainreserva, {0}, locked from reservasviajes where idreserva={1}",
                        nIdExpedienteNew, reservaID.IDENTIFICADOR, nSgteReserva,
                        System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                Quodem.Sql.SqlServerClient.ExecuteQuery(consultaReserva);

                // Crea una copia de cada servicio
                CrearCopiaServicios(nSgteReserva, reservaID.IDENTIFICADOR);
            }

            return 1;
        }

        public int CrearCopiaServicios(int nIdReservaNew, int nIdReservaOld)
        {
            DbCommand comando = null;

            // Recorre todos los servicios de esta reserva
            ICollection<DAuxIdSer> colServicios = ObtenerIDsSer(string.Format("select idservicio as IDENTIFICADOR, idservicioinscripcion, idserviciohotel, idservicioactividad, idserviciotransporte from serviciosreservasviajes where idreserva = {0}", nIdReservaOld));
            foreach (DAuxIdSer servicioID in colServicios)
            {
                string consultaServicios = "";
                int nSgteServicio = ObtenerSiguienteIdServicio();

                if (servicioID.idservicioactividad.HasValue && servicioID.idservicioactividad.Value > 0)
                {
                    // Copia las actividades
                    int nSgteActividad = ObtenerSiguienteIdServicioActividad();

                    string consultaServiciosActividades = string.Format("insert into serviciosreservasactividades select {1},idtarifaactividad, observaciones, pvp, locked, sede, Descripcion, tipo, fechainicio, horainicio, minutosinicio, fechafin, horafin, minutosfin,pax, idconfempresa from serviciosreservasactividades where idservicioactividad={0}", servicioID.idservicioactividad, nSgteActividad);
                    Quodem.Sql.SqlServerClient.ExecuteQuery(consultaServiciosActividades);

                    // Copia los participantes de este servicio
                    CrearCopiaServicioTipo(nSgteActividad, servicioID.idservicioactividad.Value, 0);

                    consultaServicios = string.Format("insert into serviciosreservasviajes select {2}, {1}, IdTipoBono, '{4}', resumenservicio, Cotizado, idservicioinscripcion, idserviciohotel, {3}, idserviciotransporte, locked, idtransporte_servicioavion1, idtransporte_serviciotren1,idtransporte_serviciocar1, importeReserva from serviciosreservasviajes where idreserva={0}", nIdReservaOld, nIdReservaNew, nSgteServicio, nSgteActividad, System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                }
                else if (servicioID.idserviciohotel.HasValue && servicioID.idserviciohotel.Value > 0)
                {
                    // Copia los Hoteles
                    int nSgteHotel = ObtenerSiguienteIdServicioHotel();

                    string consultaServiciosHoteles =
                        string.Format(
                            "insert into serviciosreservashotel select {1},fechahorallegada, fechahorasalida, IdPais, pais, IdProvincia, provincia,IdPoblacion,poblacion,Idproveedor,hotel,IdTipoAloj,categoria,observaciones,idtipohab,num_habitaciones,desc_tipoalojamiento,desc_idtipo_habitacion,idtarifaaloj,pvp,locked,idconfempresa from serviciosreservashotel where idserviciohotel={0}",
                            servicioID.idserviciohotel, nSgteHotel);

                    Quodem.Sql.SqlServerClient.ExecuteQuery(consultaServiciosHoteles);

                    // Copia los participantes de este servicio
                    CrearCopiaServicioTipo(nSgteHotel, servicioID.idserviciohotel.Value, 1);

                    consultaServicios = string.Format("insert into serviciosreservasviajes select {2}, {1}, IdTipoBono, fechapeticion, resumenservicio, Cotizado, idservicioinscripcion, {3}, idservicioactividad, idserviciotransporte, locked, idtransporte_servicioavion1, idtransporte_serviciotren1,idtransporte_serviciocar1, importeReserva from serviciosreservasviajes where idreserva={0}", nIdReservaOld, nIdReservaNew, nSgteServicio, nSgteHotel);
                }
                else if (servicioID.idservicioinscripcion.HasValue && servicioID.idservicioinscripcion.Value > 0)
                {
                    // Copia las actividades
                    int nSgteInscripcion = ObtenerSiguienteIdServicioInscripcion();

                    string consultaServiciosActividades =
                        string.Format(
                            "insert into serviciosreservasinscripciones select {1}, inscripcion,envioboletin,tipoinscripcion,Otros,observaciones,observ_agencia,iddatosentrega,idtarifainscripcion,pvp,locked,idconfempresa from serviciosreservasinscripciones where idservicioinscripcion={0}",
                            servicioID.idservicioinscripcion, nSgteInscripcion);

                    Quodem.Sql.SqlServerClient.ExecuteQuery(consultaServiciosActividades);

                    // Copia los participantes de este servicio
                    CrearCopiaServicioTipo(nSgteInscripcion, servicioID.idservicioinscripcion.Value, 2);

                    consultaServicios = string.Format("insert into serviciosreservasviajes select {2}, {1}, IdTipoBono, fechapeticion, resumenservicio, Cotizado, {3}, idserviciohotel, idservicioactividad, idserviciotransporte, locked, idtransporte_servicioavion1, idtransporte_serviciotren1,idtransporte_serviciocar1, importeReserva from serviciosreservasviajes where idreserva={0}", nIdReservaOld, nIdReservaNew, nSgteServicio, nSgteInscripcion);
                }
                else if (servicioID.idserviciotransporte.HasValue && servicioID.idserviciotransporte.Value > 0)
                {
                    // Copia las actividades
                    int nSgteTransporte = ObtenerSiguienteIdServicioTransporte();

                    string consultaServiciosActividades = string.Format("insert into serviciosreservastransporte select {1},IdTipoBono_ida1,ida1_fechasalida,ida1_origen,ida1_destino,ida1_numvuelo_tren,ida1_horasalida,ida1_horallegada,IdTipoBono_ida2,ida2_fechasalida,ida2_origen,ida2_destino,ida2_numvuelo_tren,ida2_horasalida,ida2_horallegada,IdTipoBono_reg1,reg1_fechasalida,reg1_origen,reg1_destino,reg1_numvuelo_tren,reg1_horasalida,reg1_horallegada,IdTipoBono_reg2,reg2_fechasalida,reg2_origen,reg2_destino,reg2_numvuelo_tren,reg2_horasalida,reg2_horallegada,importe_max,observaciones_ida,observaciones_reg, observaciones, gastos_cancelacion,ida,regreso,locked, idconfempresa from serviciosreservastransporte where idserviciotransporte={0}", servicioID.idserviciotransporte, nSgteTransporte);

                    Quodem.Sql.SqlServerClient.ExecuteQuery(consultaServiciosActividades);

                    // Copia los participantes de este servicio
                    CrearCopiaServicioTipo(nSgteTransporte, servicioID.idserviciotransporte.Value, 3);

                    consultaServicios = string.Format("insert into serviciosreservasviajes select {2}, {1}, IdTipoBono, fechapeticion, resumenservicio, Cotizado, idservicioinscripcion, idserviciohotel, idservicioactividad, {3}, locked, idtransporte_servicioavion1, idtransporte_serviciotren1,idtransporte_serviciocar1, importeReserva from serviciosreservasviajes where idreserva={0}", nIdReservaOld, nIdReservaNew, nSgteServicio, nSgteTransporte);
                }

                // Ahora sí crea el servicio porque ya conoce el identificador de su servicio asociado
                if (!string.IsNullOrEmpty(consultaServicios))
                {
                    Quodem.Sql.SqlServerClient.ExecuteQuery(consultaServicios);
                }
            }

            return 1;
        }

        public int CrearCopiaServicioTipo(int nIdServicioTipoNew, int nIdServicioTipoOld,
            int nTipo)
        {
            // Recorre todas los participantes en este servicio
            string[] sTablaOrigen =
            {
                "select idactividadpassengerlist as IDENTIFICADOR from actividades_passengers_list where idservicioactividad = {0}",
                "select idhotelpassengerlist as IDENTIFICADOR from hotel_passengers_list where idserviciohotel = {0}",
                "select idinspassengerlist as IDENTIFICADOR from ins_passengers_list where idservicioinscripcion = {0}",
                "select idtransportepassengerlist as IDENTIFICADOR from transportepassengerslist where idserviciotransporte = {0}"
            };

            string[] sQueryNext =
            {
                "SELECT ISNULL(max(idactividadpassengerlist),0) from actividades_passengers_list;",
                "SELECT ISNULL(max(idhotelpassengerlist),0) from hotel_passengers_list;",
                "SELECT ISNULL(max(idinspassengerlist),0) from ins_passengers_list;",
                "SELECT ISNULL(max(idtransportepassengerlist),0) from transportepassengerslist;",
            };

            string[] sQueryIns =
            {
                "insert into actividades_passengers_list select {2}, {0}, idpassengerlist, locked from actividades_passengers_list where idactividadpassengerlist={1}",
                "insert into hotel_passengers_list select {2}, {0}, idpassengerlist, locked from hotel_passengers_list where idhotelpassengerlist={1}",
                "insert into ins_passengers_list select {2}, idpassengerlist, {0}, locked from ins_passengers_list where idinspassengerlist={1}",
                "insert into transportepassengerslist select {2}, {0}, idpassengerlist, locked from transportepassengerslist where idtransportepassengerlist={1}"
            };

            ICollection<DAuxId> colServiciosTipo = ObtenerIDs(string.Format(sTablaOrigen[nTipo], nIdServicioTipoOld));
            foreach (DAuxId reservaID in colServiciosTipo)
            {
                int nSgteServicio = ObtenerSiguienteIDCopia(sQueryNext[nTipo]);

                string consultaReserva = string.Format(sQueryIns[nTipo], nIdServicioTipoNew, reservaID.IDENTIFICADOR,
                    nSgteServicio);

                Quodem.Sql.SqlServerClient.ExecuteQuery(consultaReserva);
            }

            return 1;
        }

        private string CrearSeccionAmecNoParaguas(int idpeticionario, int? idconfempresa)
        {
            StringBuilder db = new StringBuilder();
            if (idconfempresa.HasValue)
            {
                db.AppendFormat(
                    "SELECT CAST(ams.idamecs AS VARCHAR(20)) as idamecs, amcong.*, ams.idsolicitante, case amcong.amec when null then cast(ams.idamecs as CHAR) + ' ' else  amcong.amec + ' ' end + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) as  AmecConcatSolicitante from amecs ams left join cv_amec_congreso amcong on amcong.idAMEC = ams.idamecs right join peticionarios pet on pet.idpeticionario = ams.idsolicitante where ams.idconfempresa = " + idconfempresa.Value + " and ams.paraguas = false and ams.idsolicitante={0} and ams.idestado != 3 and ams.idestado != 4",
                    idpeticionario);
            }
            else
            {
                db.AppendFormat(
                    "SELECT CAST(ams.idamecs AS VARCHAR(20)) as idamecs, amcong.*, ams.idsolicitante, case amcong.amec when null then cast(ams.idamecs as CHAR) + ' ' else  amcong.amec + ' ' end + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) as  AmecConcatSolicitante from amecs ams left join cv_amec_congreso amcong on amcong.idAMEC = ams.idamecs right join peticionarios pet on pet.idpeticionario = ams.idsolicitante where ams.paraguas = false and ams.idsolicitante={0} and ams.idestado != 3 and ams.idestado != 4",
                    idpeticionario);
            }
            return db.ToString();
        }

        private string CrearSeccionAmecParaguas(DVPeticionariosRoles peticionario, int? idconfempresa)
        {
            StringBuilder db = new StringBuilder();

            if (idconfempresa.HasValue)
            {
                if (peticionario.idunidad != null && peticionario.Idarea == null && peticionario.idregion == null &&
                    peticionario.iddistrito == null)
                {
                    db.AppendFormat(
                        "SELECT CAST(ams.idamecs AS VARCHAR(20)) as idamecs, amcong.*, ams.idsolicitante, case amcong.amec when null then cast(ams.idamecs as CHAR) + ' ' else  amcong.amec + ' ' end + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) as  AmecConcatSolicitante from amecs ams left join cv_amec_congreso amcong on amcong.idAMEC = ams.idamecs right join peticionarios pet on pet.idpeticionario = ams.idsolicitante inner join unidorganizamec unids on unids.idamecs = ams.idamecs where ams.idconfempresa = " + idconfempresa.Value + " and ams.paraguas = true and ams.idestado != 3 and ams.idestado != 4 and unids.idunidad = {0} ",
                        peticionario.idunidad);
                }

                if (peticionario.idunidad.HasValue && peticionario.Idarea != null && peticionario.idregion == null &&
                    peticionario.iddistrito == null)
                {
                    db.AppendFormat(
                        "SELECT CAST(ams.idamecs AS VARCHAR(20)) as idamecs, amcong.*, ams.idsolicitante, case amcong.amec when null then cast(ams.idamecs as CHAR) + ' ' else  amcong.amec + ' ' end + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) as  AmecConcatSolicitante from amecs ams left join cv_amec_congreso amcong on amcong.idAMEC = ams.idamecs right join peticionarios pet on pet.idpeticionario = ams.idsolicitante inner join unidorganizamec unids on unids.idamecs = ams.idamecs  where ams.idconfempresa = " + idconfempresa.Value + " and ams.paraguas = true and ams.idestado != 3 and ams.idestado != 4 and ((unids.idunidad = {0} and unids.idarea = {1}) or (unids.idunidad = {0} and unids.idarea IS null and unids.idregion is null and unids.iddistrito is null)) ",
                        peticionario.idunidad, peticionario.Idarea);
                }

                if (peticionario.idunidad != null && peticionario.Idarea != null && peticionario.idregion != null &&
                    peticionario.iddistrito == null)
                {
                    db.AppendFormat(
                        "SELECT CAST(ams.idamecs AS VARCHAR(20)) as idamecs, amcong.*, ams.idsolicitante, case amcong.amec when null then cast(ams.idamecs as CHAR) + ' ' else  amcong.amec + ' ' end + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) as  AmecConcatSolicitante from amecs ams left join cv_amec_congreso amcong on amcong.idAMEC = ams.idamecs right join peticionarios pet on pet.idpeticionario = ams.idsolicitante inner join unidorganizamec unids on unids.idamecs = ams.idamecs where ams.idconfempresa = " + idconfempresa.Value + " and ams.paraguas = true and ams.idestado != 3 and ams.idestado != 4 and ((unids.idunidad = {0} and unids.idarea = {1} and unids.idregion = {2}) or (unids.idunidad = {0} and unids.idarea = {1} and unids.idregion is null and unids.iddistrito is null) or (unids.idunidad = {0} and unids.idarea is null and unids.idregion is null and unids.iddistrito is null)) ",
                        peticionario.idunidad, peticionario.Idarea, peticionario.idregion);
                }

                if (peticionario.idunidad != null && peticionario.Idarea != null && peticionario.idregion == null &&
                    peticionario.iddistrito != null)
                {
                    db.AppendFormat(
                        "SELECT CAST(ams.idamecs AS VARCHAR(20)) as idamecs, amcong.*, ams.idsolicitante, case amcong.amec when null then cast(ams.idamecs as CHAR) + ' ' else  amcong.amec + ' ' end + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) as  AmecConcatSolicitante from amecs ams left join cv_amec_congreso amcong on amcong.idAMEC = ams.idamecs right join peticionarios pet on pet.idpeticionario = ams.idsolicitante inner join unidorganizamec unids on unids.idamecs = ams.idamecs where ams.idconfempresa = " + idconfempresa.Value + " and ams.paraguas = true and ams.idestado != 3 and ams.idestado != 4 and ((unids.idunidad = {0} and unids.idarea = {1} and unids.iddistrito = {2}) or (unids.idunidad = {0} and unids.idarea is null and unids.idregion is null and unids.iddistrito is null) or (unids.idunidad = {0} and unids.idarea = {1} and unids.idregion is null and unids.iddistrito is null)) ",
                        peticionario.idunidad, peticionario.Idarea, peticionario.iddistrito);
                }

                if (peticionario.idunidad != null && peticionario.Idarea != null && peticionario.idregion != null &&
                    peticionario.iddistrito != null)
                {
                    db.AppendFormat(
                        "SELECT CAST(ams.idamecs AS VARCHAR(20)) as idamecs, amcong.*, ams.idsolicitante, case amcong.amec when null then cast(ams.idamecs as CHAR) + ' ' else  amcong.amec + ' ' end + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) as  AmecConcatSolicitante from amecs ams left join cv_amec_congreso amcong on amcong.idAMEC = ams.idamecs right join peticionarios pet on pet.idpeticionario = ams.idsolicitante inner join unidorganizamec unids on unids.idamecs = ams.idamecs where ams.idconfempresa = " + idconfempresa.Value + " and ams.paraguas = true and ams.idestado != 3 and ams.idestado != 4 and ((unids.idunidad = {0} and unids.idarea = {1} and unids.idregion = {2} and unids.iddistrito = {3}) or (unids.idunidad = {0} and unids.idarea = {1} and unids.idregion = {2} and unids.iddistrito is null) or (unids.idunidad = {0} and unids.idarea = {1} and unids.idregion is null and unids.iddistrito is null) or (unids.idunidad = {0} and unids.idarea is null and unids.idregion is null and unids.iddistrito is null)) ",
                        peticionario.idunidad, peticionario.Idarea, peticionario.idregion, peticionario.iddistrito);
                }
            }
            else
            {

                if (peticionario.idunidad != null && peticionario.Idarea == null && peticionario.idregion == null &&
                    peticionario.iddistrito == null)
                {
                    db.AppendFormat(
                        "SELECT CAST(ams.idamecs AS VARCHAR(20)) as idamecs, amcong.*, ams.idsolicitante, case amcong.amec when null then cast(ams.idamecs as CHAR) + ' ' else  amcong.amec + ' ' end + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) as  AmecConcatSolicitante from amecs ams left join cv_amec_congreso amcong on amcong.idAMEC = ams.idamecs right join peticionarios pet on pet.idpeticionario = ams.idsolicitante inner join unidorganizamec unids on unids.idamecs = ams.idamecs where ams.paraguas = true and ams.idestado != 3 and ams.idestado != 4 and unids.idunidad = {0} ",
                        peticionario.idunidad);
                }

                if (peticionario.idunidad.HasValue && peticionario.Idarea != null && peticionario.idregion == null &&
                    peticionario.iddistrito == null)
                {
                    db.AppendFormat(
                        "SELECT CAST(ams.idamecs AS VARCHAR(20)) as idamecs, amcong.*, ams.idsolicitante, case amcong.amec when null then cast(ams.idamecs as CHAR) + ' ' else  amcong.amec + ' ' end + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) as  AmecConcatSolicitante from amecs ams left join cv_amec_congreso amcong on amcong.idAMEC = ams.idamecs right join peticionarios pet on pet.idpeticionario = ams.idsolicitante inner join unidorganizamec unids on unids.idamecs = ams.idamecs  where ams.paraguas = true and ams.idestado != 3 and ams.idestado != 4 and ((unids.idunidad = {0} and unids.idarea = {1}) or (unids.idunidad = {0} and unids.idarea IS null and unids.idregion is null and unids.iddistrito is null)) ",
                        peticionario.idunidad, peticionario.Idarea);
                }

                if (peticionario.idunidad != null && peticionario.Idarea != null && peticionario.idregion != null &&
                    peticionario.iddistrito == null)
                {
                    db.AppendFormat(
                        "SELECT CAST(ams.idamecs AS VARCHAR(20)) as idamecs, amcong.*, ams.idsolicitante, case amcong.amec when null then cast(ams.idamecs as CHAR) + ' ' else  amcong.amec + ' ' end + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) as  AmecConcatSolicitante from amecs ams left join cv_amec_congreso amcong on amcong.idAMEC = ams.idamecs right join peticionarios pet on pet.idpeticionario = ams.idsolicitante inner join unidorganizamec unids on unids.idamecs = ams.idamecs where ams.paraguas = true and ams.idestado != 3 and ams.idestado != 4 and ((unids.idunidad = {0} and unids.idarea = {1} and unids.idregion = {2}) or (unids.idunidad = {0} and unids.idarea = {1} and unids.idregion is null and unids.iddistrito is null) or (unids.idunidad = {0} and unids.idarea is null and unids.idregion is null and unids.iddistrito is null)) ",
                        peticionario.idunidad, peticionario.Idarea, peticionario.idregion);
                }

                if (peticionario.idunidad != null && peticionario.Idarea != null && peticionario.idregion == null &&
                    peticionario.iddistrito != null)
                {
                    db.AppendFormat(
                        "SELECT CAST(ams.idamecs AS VARCHAR(20)) idamecs, amcong.*, ams.idsolicitante, case amcong.amec when null then cast(ams.idamecs as CHAR) + ' ' else  amcong.amec + ' ' end + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) as  AmecConcatSolicitante from amecs ams left join cv_amec_congreso amcong on amcong.idAMEC = ams.idamecs right join peticionarios pet on pet.idpeticionario = ams.idsolicitante inner join unidorganizamec unids on unids.idamecs = ams.idamecs where ams.paraguas = true and ams.idestado != 3 and ams.idestado != 4 and ((unids.idunidad = {0} and unids.idarea = {1} and unids.iddistrito = {2}) or (unids.idunidad = {0} and unids.idarea is null and unids.idregion is null and unids.iddistrito is null) or (unids.idunidad = {0} and unids.idarea = {1} and unids.idregion is null and unids.iddistrito is null)) ",
                        peticionario.idunidad, peticionario.Idarea, peticionario.iddistrito);
                }

                if (peticionario.idunidad != null && peticionario.Idarea != null && peticionario.idregion != null &&
                    peticionario.iddistrito != null)
                {
                    db.AppendFormat(
                        "SELECT CAST(ams.idamecs AS VARCHAR(20)) as idamecs, amcong.*, ams.idsolicitante, case amcong.amec when null then cast(ams.idamecs as CHAR) + ' ' else  amcong.amec + ' ' end + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) as  AmecConcatSolicitante from amecs ams left join cv_amec_congreso amcong on amcong.idAMEC = ams.idamecs right join peticionarios pet on pet.idpeticionario = ams.idsolicitante inner join unidorganizamec unids on unids.idamecs = ams.idamecs where ams.paraguas = true and ams.idestado != 3 and ams.idestado != 4 and ((unids.idunidad = {0} and unids.idarea = {1} and unids.idregion = {2} and unids.iddistrito = {3}) or (unids.idunidad = {0} and unids.idarea = {1} and unids.idregion = {2} and unids.iddistrito is null) or (unids.idunidad = {0} and unids.idarea = {1} and unids.idregion is null and unids.iddistrito is null) or (unids.idunidad = {0} and unids.idarea is null and unids.idregion is null and unids.iddistrito is null)) ",
                        peticionario.idunidad, peticionario.Idarea, peticionario.idregion, peticionario.iddistrito);
                }
            }

            return db.ToString();
        }


        private string CrearSeccionAmecSinCongreso(int idpeticionario)
        {
            StringBuilder db = new StringBuilder();

            db.AppendFormat(
                "SELECT CAST(ams.idamecs AS VARCHAR(20)) as idamecs, amcong.*, ams.idsolicitante, case amcong.amec when null then cast(ams.idamecs as CHAR) + ' ' else  amcong.amec + ' ' end + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) as  AmecConcatSolicitante from amecs ams left join cv_amec_congreso amcong on amcong.idAMEC = ams.idamecs right join peticionarios pet on pet.idpeticionario = ams.idsolicitante where ams.idamecs not in (Select am.amec from amec am) and ams.idsolicitante={0}",
                idpeticionario);
            return db.ToString();
        }


        private string CrearSeccionAmecViejo(int idcongreso, int? idconfempresa, bool includeOldAmecs = true)
        {
            StringBuilder db = new StringBuilder();
            if (idconfempresa.HasValue)
            {
                if (includeOldAmecs)
                {
                    db.AppendFormat("SELECT am.amec as idamecs, max(amcong.IdAmec) as IdAmec, max(amcong.AMEC) as AMEC, max(amcong.IdCongreso) as IdCongreso, max(amcong.Actividad) as Actividad, max(ISNULL(ams.idsolicitante,null)) as idsolicitante, am.amec as  AmecConcatSolicitante  FROM amec am inner join cv_amec_congreso amcong on amcong.idAMEC = am.idamec left join amecs ams on CAST(ams.idamecs AS VARCHAR(20)) = am.amec where  (ams.veeva = 0 or ams.veeva is null) AND am.idconfempresa = " + idconfempresa.Value + " and am.inactivo = 0 and am.amec Not in (select CONVERT(varchar(20),ames.idamecs) from amecs ames where CONVERT(varchar(20),ames.idamecs) = am.amec) group by am.amec ");
                }
                else {
                    db.AppendFormat("SELECT am.amec as idamecs, max(amcong.IdAmec) as IdAmec, max(amcong.AMEC) as AMEC, max(amcong.IdCongreso) as IdCongreso, max(amcong.Actividad) as Actividad, max(ISNULL(ams.idsolicitante,null)) as idsolicitante, am.amec as  AmecConcatSolicitante  FROM amec am inner join cv_amec_congreso amcong on amcong.idAMEC = am.idamec left join amecs ams on CAST(ams.idamecs AS VARCHAR(20)) = am.amec where  (ams.veeva = 0 or ams.veeva is null) AND am.idconfempresa = " + idconfempresa.Value + " and am.inactivo = 0 and am.amec Not in (select CONVERT(varchar(20),ames.idamecs) from amecs ames where CONVERT(varchar(20),ames.idamecs) = am.amec) /*and (ams.fechafinalizacion > DATEADD(YEAR, -1, GETDATE()))*/ group by am.amec ");
                }
            }
            else
            {
                if (includeOldAmecs)
                {
                    db.AppendFormat("SELECT am.amec as idamecs, max(amcong.IdAmec) as IdAmec, max(amcong.AMEC) as AMEC, max(amcong.IdCongreso) as IdCongreso, max(amcong.Actividad) as Actividad, max(ISNULL(ams.idsolicitante,null)) as idsolicitante, am.amec as  AmecConcatSolicitante  FROM amec am inner join cv_amec_congreso amcong on amcong.idAMEC = am.idamec left join amecs ams on CAST(ams.idamecs AS VARCHAR(20)) = am.amec where (ams.veeva = 0 or ams.veeva is null) AND am.inactivo = 0 and am.amec Not in (select CONVERT(varchar(20),ames.idamecs) from amecs ames where CONVERT(varchar(20),ames.idamecs) = am.amec) group by am.amec ");
                }
                else {
                    db.AppendFormat("SELECT am.amec as idamecs, max(amcong.IdAmec) as IdAmec, max(amcong.AMEC) as AMEC, max(amcong.IdCongreso) as IdCongreso, max(amcong.Actividad) as Actividad, max(ISNULL(ams.idsolicitante,null)) as idsolicitante, am.amec as  AmecConcatSolicitante  FROM amec am inner join cv_amec_congreso amcong on amcong.idAMEC = am.idamec left join amecs ams on CAST(ams.idamecs AS VARCHAR(20)) = am.amec where (ams.veeva = 0 or ams.veeva is null) AND am.inactivo = 0 and am.amec Not in (select CONVERT(varchar(20),ames.idamecs) from amecs ames where CONVERT(varchar(20),ames.idamecs) = am.amec) /*and (ams.fechafinalizacion > DATEADD(YEAR, -1, GETDATE()))*/ group by am.amec ");
                }
            }
            return db.ToString();
        }

        private string CrearSeccionAmecNuevo(int idpeticionario, int? idconfempresa, bool includeOldAmecs = true)
        {
            StringBuilder db = new StringBuilder();

            
            db.Append(" IF Object_id('tempdb..#Temp') IS NOT NULL ");
            db.Append("   DROP TABLE #temp; ");
            db.Append(" ");
            db.Append(" CREATE TABLE #temp ");
            db.Append("   ( ");
            db.Append("      idamecs VARCHAR(20) PRIMARY KEY CLUSTERED ");
            db.Append("   ); ");
            db.Append(" ");
            db.Append(" INSERT INTO #temp ");
            db.Append(" EXEC Sp_obtener_amecs_puedo_ver ");
            //db.AppendFormat("   @idPeticionario = {0}, @isCreateMode = 1; ", idpeticionario);
            //CAMBIO MARGA ABRIR CREACIÓN DE EXPEDIENTES ASOCIADOS A CUALQUIER AMEC PARA ADMINISTRADORES
            db.AppendFormat("   @idPeticionario = {0}, @isCreateMode = 0; ", idpeticionario);
            db.Append(" ");
            db.Append(" SELECT ");
            db.Append("     CAST(ams.idamecs AS VARCHAR(20)) as idamecs,   ");
            db.Append("     max(amcong.IdAmec) as IdAmec,    ");
            db.Append("     max(amcong.AMEC) as AMEC,   ");
            db.Append("     max(amcong.IdCongreso) as IdCongreso,   ");
            db.Append(" 	max(amcong.Actividad) as Actividad,   ");
            db.Append(" 	max(ISNULL(ams.idsolicitante, 0)) as idsolicitante, ");
            db.Append(" 	max(case when amcong.amec is null then rtrim(ltrim(cast(ams.idamecs as CHAR))) + ' ' else  rtrim(ltrim(amcong.amec)) end  + ' - ' + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) + ' - ' + ams.descripcion) as AmecConcatSolicitante ");
            db.Append(" FROM amecs ams ");
            db.Append(" INNER JOIN #temp ON  ");
            db.Append(" 	ams.idamecs = #temp.idamecs  ");
            db.Append(" left join cv_amec_congreso amcong on ");
            db.Append("     cast(amcong.AMEC as varchar(20)) = cast(ams.idamecs as varchar(20)) ");
            db.Append(" LEFT JOIN peticionarios pet ON ");
            db.Append("     ams.idsolicitante = pet.idpeticionario ");
            db.Append(" WHERE ");
            db.Append("     ams.idconfempresa IS NOT NULL  ");
            //Descomentar si se quiere filtrar los amecs para que solo aparezcan los aprobados y con agencia. Actualmente aparecen todos los que tienen agencia
            //db.Append("    AND ams.idestado in (1, 5) ");
            db.Append("    AND ams.idestado <> 3 AND ams.idestado <> 4 ");
            if (idconfempresa.HasValue)
            {
                db.Append(" and ams.idconfempresa = " + idconfempresa.ToString());
            }
            if (!includeOldAmecs) {
                db.Append(" and (ams.fechafinalizacion > DATEADD(YEAR, -1, GETDATE())) ");
            }
            db.Append(" GROUP BY ams.idamecs ");

            return db.ToString();
        }

        private string CrearSeccionAmecVeeva(int idpeticionario, int? idconfempresa, bool includeOldAmecs = true, string idAmecsLike = null)
        {
            StringBuilder db = new StringBuilder();

            db.Append(" SELECT ");
            db.Append("     CAST(ams.idamecs AS VARCHAR(20)) as idamecs,   ");
            db.Append("     max(amcong.IdAmec) as IdAmec,    ");
            db.Append("     max(amcong.AMEC) as AMEC,   ");
            db.Append("     max(amcong.IdCongreso) as IdCongreso,   ");
            db.Append(" 	max(amcong.Actividad) as Actividad,   ");
            db.Append(" 	max(ISNULL(ams.idsolicitante, 0)) as idsolicitante, ");
            db.Append(" 	max(case when amcong.amec is null then rtrim(ltrim(cast(ams.idamecs as CHAR))) + ' ' else  rtrim(ltrim(amcong.amec)) end + ' - ' + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) + ' - ' + isnull(ams.descripcion, '')) as AmecConcatSolicitante ");
            db.Append(" FROM amecs ams ");
            db.Append(" left join cv_amec_congreso amcong on ");
            db.Append("     cast(amcong.AMEC as varchar(20)) = cast(ams.idamecs as varchar(20)) ");
            db.Append(" LEFT JOIN peticionarios pet ON ");
            db.Append("     ams.idsolicitante = pet.idpeticionario ");
            db.Append(" WHERE ");
            db.Append("     ams.veeva = 1 ");
            if (!string.IsNullOrWhiteSpace(idAmecsLike)) {
                db.Append("     and ams.idamecs like '%" + idAmecsLike + "%'  ");
            }
            db.Append("     AND ams.idconfempresa IS NOT NULL  ");
            db.Append("    AND ams.idestado = 47 ");
            if (idconfempresa.HasValue)
            {
                db.Append("and ams.idconfempresa = " + idconfempresa.ToString());
            }
            if (!includeOldAmecs)
            {
                db.Append(" and (ams.fechafinalizacion > DATEADD(YEAR, -1, GETDATE())) ");
            }
            db.Append(" GROUP BY ams.idamecs ");

            return db.ToString();
        }

        private string CrearSeccionAmecNewco(int idpeticionario, int? idconfempresa, bool includeOldAmecs = true)
        {
            StringBuilder db = new StringBuilder();

            db.Append(" SELECT ");
            db.Append("     CAST(ams.idamecs AS VARCHAR(20)) as idamecs,   ");
            db.Append("     max(amcong.IdAmec) as IdAmec,    ");
            db.Append("     max(amcong.AMEC) as AMEC,   ");
            db.Append("     max(amcong.IdCongreso) as IdCongreso,   ");
            db.Append(" 	max(amcong.Actividad) as Actividad,   ");
            db.Append(" 	max(ISNULL(ams.idsolicitante, 0)) as idsolicitante, ");
            db.Append(" 	max(case when amcong.amec is null then rtrim(ltrim(cast(ams.idamecs as CHAR))) + ' ' else  rtrim(ltrim(amcong.amec)) end + ' - ' + RTRIM(LTRIM(ISNULL(pet.nombre, '') + ' ' + ISNULL(pet.apellido1, ''))) + ' - ' + isnull(ams.descripcion, '')) as AmecConcatSolicitante ");
            db.Append(" FROM amecs ams ");
            db.Append(" left join cv_amec_congreso amcong on ");
            db.Append("     cast(amcong.AMEC as varchar(20)) = cast(ams.idamecs as varchar(20)) ");
            db.Append(" LEFT JOIN peticionarios pet ON ");
            db.Append("     ams.idsolicitante = pet.idpeticionario ");
            db.Append(" WHERE ");
            db.Append("     ams.newco = 1 ");
            db.Append("     AND ams.idconfempresa IS NOT NULL  ");
            db.Append("    AND ams.idestado = 47 ");
            if (idconfempresa.HasValue)
            {
                db.Append("and ams.idconfempresa = " + idconfempresa.ToString());
            }
            if (!includeOldAmecs)
            {
                db.Append(" and (ams.fechafinalizacion > DATEADD(YEAR, -1, GETDATE())) ");
            }
            db.Append(" GROUP BY ams.idamecs ");

            return db.ToString();
        }

        private string CrearSeccionAmecDefecto(int amecReuniones, int? idconfempresa, bool includeOldAmecs = true)
        {
            StringBuilder db = new StringBuilder();

            if (idconfempresa.HasValue)
            {
                if (includeOldAmecs)
                {
                    db.AppendFormat("SELECT am.amec as idamecs, max(amcong.IdAmec) as IdAmec, max(amcong.AMEC) as AMEC, max(amcong.IdCongreso) as IdCongreso, max(amcong.Actividad) as Actividad, max(ISNULL(ams.idsolicitante,null)) as idsolicitante, am.amec as  AmecConcatSolicitante  FROM amec am inner join cv_amec_congreso amcong on amcong.idAMEC = am.idamec left join amecs ams on ams.idamecs = am.amec where (ams.veeva = 0 or ams.veeva is null) AND am.idconfempresa = " + idconfempresa.Value + " and am.amec = '{0}' /*and am.amec Not in (select idamecs from amecs ames where ames.idamecs = am.amec)*/ group by am.amec ", amecReuniones);
                }
                else {
                    db.AppendFormat("SELECT am.amec as idamecs, max(amcong.IdAmec) as IdAmec, max(amcong.AMEC) as AMEC, max(amcong.IdCongreso) as IdCongreso, max(amcong.Actividad) as Actividad, max(ISNULL(ams.idsolicitante,null)) as idsolicitante, am.amec as  AmecConcatSolicitante  FROM amec am inner join cv_amec_congreso amcong on amcong.idAMEC = am.idamec left join amecs ams on ams.idamecs = am.amec where (ams.veeva = 0 or ams.veeva is null) and (ams.fechafinalizacion > DATEADD(YEAR, -1, GETDATE())) AND am.idconfempresa = " + idconfempresa.Value + " and am.amec = '{0}' /*and am.amec Not in (select idamecs from amecs ames where ames.idamecs = am.amec)*/ group by am.amec ", amecReuniones);
                }
            }
            else
            {
                if (includeOldAmecs)
                {
                    db.AppendFormat("SELECT am.amec as idamecs, max(amcong.IdAmec) as IdAmec, max(amcong.AMEC) as AMEC, max(amcong.IdCongreso) as IdCongreso, max(amcong.Actividad) as Actividad, max(ISNULL(ams.idsolicitante,null)) as idsolicitante, am.amec as  AmecConcatSolicitante  FROM amec am inner join cv_amec_congreso amcong on amcong.idAMEC = am.idamec left join amecs ams on ams.idamecs = am.amec where (ams.veeva = 0 or ams.veeva is null) AND am.amec = '{0}' /*and am.amec Not in (select idamecs from amecs ames where ames.idamecs = am.amec)*/ group by am.amec ", amecReuniones);
                }
                else {
                    db.AppendFormat("SELECT am.amec as idamecs, max(amcong.IdAmec) as IdAmec, max(amcong.AMEC) as AMEC, max(amcong.IdCongreso) as IdCongreso, max(amcong.Actividad) as Actividad, max(ISNULL(ams.idsolicitante,null)) as idsolicitante, am.amec as  AmecConcatSolicitante  FROM amec am inner join cv_amec_congreso amcong on amcong.idAMEC = am.idamec left join amecs ams on ams.idamecs = am.amec where (ams.veeva = 0 or ams.veeva is null) and (ams.fechafinalizacion > DATEADD(YEAR, -1, GETDATE())) AND am.amec = '{0}' /*and am.amec Not in (select idamecs from amecs ames where ames.idamecs = am.amec)*/ group by am.amec ", amecReuniones);
                }
            }

            return db.ToString();
        }

        private string CrearSeccionWhereFiltroAmecCongreso(string AmecFiltro, int idcongreso)
        {
            StringBuilder db = new StringBuilder();
            bool primero = true;

            if (AmecFiltro != null)
            {
                db.AppendFormat(AmecFiltro);
            }

            AgregarAndSiProcede(db, ref primero);
            db.AppendFormat("(ams.idestado !=3 and ams.idestado !=4)");

            return db.ToString();
        }

        private string CrearSeccionWhereFiltroAmecCongresoDos(string AmecFiltro, int idpeticionario)
        {
            StringBuilder db = new StringBuilder();
            bool primero = true;

            AgregarAndSiProcede(db, ref primero);
            db.AppendFormat("(ams.idestado !=3 and ams.idestado !=4 and ams.idsolicitante = {0})", idpeticionario);

            return db.ToString();
        }

        public int CrearCopiaTramitaciones(int nIdServicioNew, int nIdServicioOld)
        {
            ICollection<DAuxId> colServiciosTramitacion =
                ObtenerIDs(
                    string.Format(
                        "select idtramitacion as IDENTIFICADOR from tramitacionesserviciosreservas where fkidservicio = {0}",
                        nIdServicioOld));
            foreach (DAuxId reservaID in colServiciosTramitacion)
            {
                int nSgteServicioTramitacion =
                    ObtenerSiguienteIDCopia("SELECT max(idtramitacion) from tramitacionesserviciosreservas;");

                string consultaReserva = string.Format("insert into tramitacionesserviciosreservas select {2}, {0},  linea,requerimientos,alternativa1,okalternativa1,alternativa2,okalternativa2,alternativa3,okalternativa3,idestado,pvp1,pvp2,pvp3,requerimientos2,IdProveedor1,IdProveedor2,IdProveedor3,IdProductoPrv1,IdProductoPrv2,IdProductoPrv3,requerimientos3,Validez1,Validez2,Validez3,IdProveedor4,IdProveedor5,IdProveedor6,gastoscancelacion1,gastoscancelacion2,gastoscancelacion3,locked from tramitacionesserviciosreservas where idtramitacion={1}", nIdServicioNew, reservaID.IDENTIFICADOR, nSgteServicioTramitacion);
                Quodem.Sql.SqlServerClient.ExecuteQuery(consultaReserva);
            }

            return 1;
        }

        public string ObtenerLabelEstado(string idEstado)
        {
            string consulta = string.Format("select estado from estadosreservas  where idestado='{0}'", idEstado);
            return Quodem.Sql.SqlServerClient.GetValue(consulta);
        }

        public string ProductosExpediente(string IdExp)
        {
            string CadenaProductos = string.Empty;

            string consulta = string.Format("select IdareaProductoempresa, Productoempresa,porcentaje from cv_productos_expedientes  where idexpediente='{0}'", IdExp);

            ICollection<DProductoPorcentajeVista> ListaProductos = DProductoPorcentajeVista.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));

            foreach (DProductoPorcentajeVista item in ListaProductos)
            {
                CadenaProductos += item.Producto + " - " + item.Porcentaje + "% || ";
            }
            if (string.IsNullOrEmpty(CadenaProductos))
            {
                CadenaProductos = "NINGUNO";
            }

            return CadenaProductos;
        }


        public bool CheckJustificanteInscripcion(int idExpediente, int idReserva, int idServicio, int idServicioAlojamiento, int idServicioTransporte)
        {
            try
            {
                string consulta = string.Format("select [dbo].[validar_justificante_inscripcion] ({0},{1},{2},{3},{4})", idExpediente, idReserva, idServicio, idServicioAlojamiento, idServicioTransporte);
                string result = Quodem.Sql.SqlServerClient.GetValue(consulta);
                return result == "1";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public ICollection<DProductoPorcentajeVista> ProductosExpedienteItems(string idExp)
        {
            string consulta = "select IdareaProductoempresa, Productoempresa, porcentaje from cv_productos_expedientes where idexpediente= " + idExp;
            return DProductoPorcentajeVista.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public void ClearProductosExpediente(int idExp)
        {
            string consulta = "delete from expedientes_areasproductosempresa where idexpediente = " + idExp;
            Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        //FIN Ismael Ameller 23-03-2011 Recupera productos por expediente

        #region DatosAdicionales

        public bool GuardarHonorariosPassenger(int idExpediente, DataTable honorariosCreados)
        {
            int i = 0;
            try
            {
                for (i = 0; i < honorariosCreados.Rows.Count; i++)
                {
                    if (honorariosCreados.Rows[i]["justificaciones"].ToString() == "-1" ||
                        honorariosCreados.Rows[i]["justificaciones"].ToString() == " ")
                        honorariosCreados.Rows[i]["justificaciones"] = "";
                    if (honorariosCreados.Rows[i]["pagosociedad"].ToString() != "")
                        honorariosCreados.Rows[i]["pagosociedad"] =
                            honorariosCreados.Rows[i]["pagosociedad"].ToString().Replace("'", "´");
                    if (honorariosCreados.Rows[i]["idtipoactividadpax"].ToString() == " ")
                        honorariosCreados.Rows[i]["idtipoactividadpax"] = "";
                    //No puede existir más de un honorario con el mismo idexpediente y el mismo passengerlist
                    if (!ExisteHonorarios(idExpediente, honorariosCreados.Rows[i]["idpassengerlist"].ToString()))
                    {
                        string consulta =
                            string.Format(
                                "insert into datosadicionales_reservas_passenger (idpassengerlist, idexpediente, idtipoactividadpax, idtipoasistente, idnivelriesgo, ficherogenesis, idjustificaciones, honorarios, pagodirecto, pagosociedad, datacreacion, tiporeunion, ponentes_duracionactividad, ponentes_preparacion, ponentes_nivelps, ponentes_honorariosmaximos, ponentes_idhonorariosmaximos, abeif_preparacion, abeif_duracionactividad, abeif_nivelps, abeif_honorariosmaximos, ponencia_centro_salud, talleres, videoconferencia_repetida, tipo_ponente, justificacion, idTipoContratoConsultoria, numero_dias_consultoria) " +
                                "values ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27})",
                                honorariosCreados.Rows[i]["idpassengerlist"].ToString() == "" ? "null" : honorariosCreados.Rows[i]["idpassengerlist"],
                                idExpediente,
                                honorariosCreados.Rows[i]["idtipoactividadpax"].ToString() == "" ? "null" : honorariosCreados.Rows[i]["idtipoactividadpax"],
                                honorariosCreados.Rows[i]["idtipoasistente"].ToString() == "" ? "null" : honorariosCreados.Rows[i]["idtipoasistente"],
                                honorariosCreados.Rows[i]["idnivelriesgo"].ToString() == "" ? "null" : honorariosCreados.Rows[i]["idnivelriesgo"],
                                honorariosCreados.Rows[i]["ficherogenesis"].ToString() == "" ? "null" : honorariosCreados.Rows[i]["ficherogenesis"],
                                honorariosCreados.Rows[i]["justificaciones"].ToString() == "" ? "null" : "" + honorariosCreados.Rows[i]["justificaciones"] + "",
                                honorariosCreados.Rows[i]["honorarios"].ToString() == "" ? "null" : "'" + honorariosCreados.Rows[i]["honorarios"] + "'",
                                honorariosCreados.Rows[i]["pagodirecto"].ToString() == "" ? "null" : honorariosCreados.Rows[i]["pagodirecto"],
                                honorariosCreados.Rows[i]["pagosociedad"].ToString() == "" ? "null" : "'" + honorariosCreados.Rows[i]["pagosociedad"] + "'",
                                "CURRENT_TIMESTAMP ",
                                honorariosCreados.Rows[i]["tiporeunion"].ToString().Trim() == "" ? "null" : "'" + honorariosCreados.Rows[i]["tiporeunion"] + "'",
                                honorariosCreados.Rows[i]["ponentes_duracionactividad"].ToString().Trim() == "" ? "null" : "'" + honorariosCreados.Rows[i]["ponentes_duracionactividad"] + "'",
                                honorariosCreados.Rows[i]["ponentes_preparacion"].ToString().Trim() == "" ? "null" : "'" + honorariosCreados.Rows[i]["ponentes_preparacion"] + "'",
                                honorariosCreados.Rows[i]["ponentes_nivelps"].ToString().Trim() == "" ? "null" : "'" + honorariosCreados.Rows[i]["ponentes_nivelps"] + "'",
                                honorariosCreados.Rows[i]["ponentes_honorariosmaximos"].ToString().Trim() == "" ? "null" : "'" + honorariosCreados.Rows[i]["ponentes_honorariosmaximos"].ToString().Replace(',', '.') + "'",
                                honorariosCreados.Rows[i]["ponentes_idhonorariosmaximos"].ToString().Trim() == "" ? "null" : "'" + honorariosCreados.Rows[i]["ponentes_idhonorariosmaximos"].ToString().Replace(',', '.') + "'",
                                honorariosCreados.Rows[i]["abeif_preparacion"].ToString().Trim() == "" ? "null" : "'" + honorariosCreados.Rows[i]["abeif_preparacion"] + "'",
                                honorariosCreados.Rows[i]["abeif_duracionactividad"].ToString().Trim() == "" ? "null" : "'" + honorariosCreados.Rows[i]["abeif_duracionactividad"] + "'",
                                honorariosCreados.Rows[i]["abeif_nivelps"].ToString().Trim() == "" ? "null" : "'" + honorariosCreados.Rows[i]["abeif_nivelps"] + "'",
                                honorariosCreados.Rows[i]["abeif_honorariosmaximos"].ToString().Trim() == "" ? "null" : "'" + honorariosCreados.Rows[i]["abeif_honorariosmaximos"].ToString().Replace(',', '.') + "'",
                                honorariosCreados.Rows[i]["ponencia_centro_salud"].ToString().Trim() == "" ? "null" : "" + honorariosCreados.Rows[i]["ponencia_centro_salud"].ToString() + "",
                                honorariosCreados.Rows[i]["talleres"].ToString().Trim() == "" ? "null" : "" + honorariosCreados.Rows[i]["talleres"].ToString() + "",
                                honorariosCreados.Rows[i]["videoconferencia_repetida"].ToString().Trim() == "" ? "null" : "" + honorariosCreados.Rows[i]["videoconferencia_repetida"].ToString() + "",
                                honorariosCreados.Rows[i]["tipo_ponente"].ToString().Trim() == "" ? "null" : "'" + honorariosCreados.Rows[i]["tipo_ponente"].ToString().Replace(',', '.') + "'",
                                honorariosCreados.Rows[i]["justificacion"].ToString().Replace("'", "''") == "" ? "null" : "'" + honorariosCreados.Rows[i]["justificacion"].ToString().Replace("'", "''") + "'",
                                honorariosCreados.Rows[i]["idTipoContratoConsultoria"].ToString() == "" ? "null" : "'" + honorariosCreados.Rows[i]["idTipoContratoConsultoria"] + "'",
                                honorariosCreados.Rows[i]["numero_dias_consultoria"].ToString() == "" ? "null" : "'" + honorariosCreados.Rows[i]["numero_dias_consultoria"] + "'"
                                );

                        Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ExisteHonorarios(int idExpediente, string idpassengerlist)
        {
            string consulta = string.Format("select count(*) from datosadicionales_reservas_passenger where idexpediente = {0} and idpassengerlist = {1} ", idExpediente, idpassengerlist);

            long iddatosadicionales = long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));

            if (iddatosadicionales != 0)
                return true;

            return false;
        }

        public bool EliminarHonorariosPassenger(int idexpediente, List<int> participantesEliminados)
        {
            try
            {
                foreach (var participanteEliminado in participantesEliminados)
                {
                    string consulta = string.Format("DELETE from datosadicionales_reservas_passenger where idexpediente ={0} and idpassengerlist = {1}", idexpediente, participanteEliminado);
                    Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
                }
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public string ObtenerMensajeAMostrar(string tipoActividad, string tipoAsistente, string riskLevel)
        {
            string consulta = string.Format("select mensaje from mensaje_reservas_passenger_datosadicionales where idtipoactividadpax = {0} and idtipoasistente = {1} and idnivelriesgo = {2} ", tipoActividad, tipoAsistente, riskLevel);
            return Quodem.Sql.SqlServerClient.GetValue(consulta);
        }

        #endregion DatosAdicionales

        public bool EsEventoInternacional(int evento)
        {
            try
            {
                string consulta = string.Format("select top 1 ISNULL((select internacional from congresos where IDCongreso = {0} ), ISNULL((select top 1 internacional from peticiones_actividad where idpeticionactividad = {0} ), 0)) as internacional", evento);
                return Quodem.Sql.SqlServerClient.GetValue(consulta) == "1";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool EstaIdAreaProductoEmpresaInactivo(string idAreaProductoEmpresa)
        {
            try
            {
                string consulta = string.Format("select inactivo from areas_productosempresa where idAreaProductoempresa = {0} ", idAreaProductoEmpresa);
                int inactivo = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
                if (inactivo == 1)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


    }
}



