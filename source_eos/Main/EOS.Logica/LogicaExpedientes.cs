using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Entidades.Mapeadores;
using EOS.Repositorios;

namespace EOS.Logica
{
    public class LogicaExpedientes : ILogicaExpedientes
    {
        private IRepositorioExpedientes MiRepositorioExpedientes { get; set; }

        public LogicaExpedientes()
        {
            MiRepositorioExpedientes = new RepositorioExpedientes();
        }

        public DataSet ObtenerRiskLevel()
        {
            return MiRepositorioExpedientes.ObtenerRiskLevel();
        }

        public DataSet ObtenerTiposActividadPax()
        {
            return MiRepositorioExpedientes.ObtenerTiposActividadPax();
        }

        public DataSet ObtenerTiposAsistente()
        {
            return MiRepositorioExpedientes.ObtenerTiposAsistente();
        }

        public DataSet ObtenerTiposAsistenteConCalculadora()
        {
            return MiRepositorioExpedientes.ObtenerTiposAsistenteConCalculadora();
        }

        public DataSet ObtenerJustificaciones()
        {
            return MiRepositorioExpedientes.ObtenerJustificaciones();
        }

        public ICollection<DCabeceraExpedienteAmpliado> ObtenerExpedientes(FiltroExpedientesAvanzado filtro, DVPeticionariosRoles datosRoles)
        {
            filtro.Roles = ObtenerFiltroRol(datosRoles, "exp");
            return MiRepositorioExpedientes.ObtenerExpedientes(filtro);
        }

        public DAmecExpediente ObtenerRelacionAmecExpediente(int idExpediente)
        {
            return MiRepositorioExpedientes.ObtenerRelacionAmecExpediente(idExpediente);
        }

        public List<DDocumentoVersion> ObtenerDatosDocumentos(string rutaDir)
        {
            return MiRepositorioExpedientes.ObtenerDatosDocumentos(rutaDir);
        }

        public List<DPassenger> ObtenerPassengers(int idExpediente)
        {
            return MiRepositorioExpedientes.ObtenerPassengers(idExpediente);
        }

        public List<DDocumentNameTypeSubType> ObtenerRelacionTipoVersion(int idExpediente)
        {
            return MiRepositorioExpedientes.ObtenerRelacionTipoVersion(idExpediente);
        }

        public List<string> ObtenerNombresOriginalesDoc(int idExpediente)
        {
            return MiRepositorioExpedientes.ObtenerNombresOriginalesDoc(idExpediente);
        }
        public ICollection<DCabeceraExpedienteAmpliado> ObtenerExpedientesTotal(FiltroExpedientesAvanzado filtro, DVPeticionariosRoles datosRoles, out int count)
        {
            filtro.Roles = ObtenerFiltroRol(datosRoles, "exp");
            return MiRepositorioExpedientes.ObtenerExpedientesTotal(filtro, out count);
        }


        public long ObtenerNumeroExpedientes(FiltroExpedientesAvanzado filtro, DVPeticionariosRoles datosRoles)
        {
            filtro.Roles = ObtenerFiltroRol(datosRoles, "exp");
            return MiRepositorioExpedientes.ObtenerNumeroExpedientes(filtro);
        }

        public string ObtenerFiltroRol(DVPeticionariosRoles datosRoles, string sDBName)
        {
            return ObtenerFiltroRol(datosRoles, sDBName, "idexpediente");
        }

        public bool EstaIdAreaProductoEmpresaInactivo(string idAreaProductoEmpresa)
        {
            return MiRepositorioExpedientes.EstaIdAreaProductoEmpresaInactivo(idAreaProductoEmpresa);
        }

        public string ObtenerFiltroRolExpNew(DVPeticionariosRoles datosRoles, string sDBName, string nombreidexpediente, out bool newQuery)
        {
            newQuery = false;
            string sFiltroRol = null;
            // Qurius (EAS) 26/20/2011
            if (datosRoles == null) return sFiltroRol;

            bool bAprobador = (datosRoles.aprobador.HasValue) && datosRoles.aprobador.Value;
            bool marketingDelegado = false;

            //Xavier Morell: el administrador puede ver todos los expediente, prevalece sobre el resto de roles.
            if (datosRoles.administrador.HasValue && datosRoles.administrador.Value)
            {
                return sFiltroRol;
            }
            if (!bAprobador)
            {
                if (datosRoles.marketing.HasValue && !marketingDelegado)
                {
                    if (datosRoles.marketing.Value)
                    {
                        // En caso de que el usuario tenga para su ID en la tabla iw_peticionarios el campo marketing a TRUE, solo verá los AMEC cuyo distrito y área coincidan con los del peticionario, por tanto solo filtraremos por estos campos.

                        // OJO: Puede ocurrir que tenga el campo marketing a true, pero no tenga configurado area o distrito, por eso hacemos estas validaciones:
                        string sWhere = "";
                        if (datosRoles.Idarea.HasValue)
                            //sWhere = string.Format(" WHERE ( IdArea = {0} or Idarea1 = {0} or Idarea2 = {0} or Idarea3 = {0} )", datosRoles.Idarea.ToString());
                            sWhere = string.Format(" WHERE ( IdArea = {0} )", datosRoles.Idarea.ToString());
                        if (datosRoles.iddistrito.HasValue)
                            sWhere += (!string.IsNullOrEmpty(sWhere) ? " AND " : " WHERE ") + " iddistrito = " + datosRoles.iddistrito.ToString();

                        //sFiltroRol = string.Format(" inner join (select distinct (idamec) from amec {0}) rol on {1}.idamec = rol.idamec ", sWhere, sDBName);
                        sFiltroRol = string.Format(" inner join (select distinct (idamec), (idxpediente) from expediente {0}) rol on {1}.IDEXPEDIENTE = rol.idxpediente ", sWhere, sDBName);

                        //Jose: 22-03-2011 si entra en alguna de las condiciones, se debe saltar el siguiente filtro
                        marketingDelegado = true;
                    }
                }
                if (datosRoles.delegado.HasValue && !marketingDelegado)
                {
                    if (datosRoles.delegado.Value)
                    {
                        // Si el usuario tiene el campo delegado a true, solo verá sus datos, es decir, los que ha creado el.
                        if (sDBName == "cva")
                        {
                            sFiltroRol = string.Format(" inner join (select distinct (idamec) as idamec from amec where idpeticionario = {0}) rol on {1}.idamec = rol.idamec ", datosRoles.IdPeticionario, sDBName);
                        }
                        else
                        {
                            //sFiltroRol = string.Format(" inner join (select distinct (idxpediente) as idexpediente from expediente where idpeticionario = {0}) rol on {1}." + nombreidexpediente + " = rol.idexpediente ", datosRoles.IdPeticionario, sDBName);
                            sFiltroRol = string.Format(" where idpeticionario = {0} ", datosRoles.IdPeticionario);
                            newQuery = true;
                        }
                        //Jose: 22-03-2011 si entra en alguna de las condiciones, se debe saltar el siguiente filtro
                        marketingDelegado = true;
                    }
                }
                if (datosRoles.IdProductoempresa.HasValue && string.IsNullOrEmpty(sFiltroRol) && !marketingDelegado)
                {
                    // Si un usuario es jefe de producto. Es porque está dado de alta en la tabla iw_jefes_productoempresa, el modo de filtrar los expedientes y los AMEC será solo filtrando por aquellos expedientes o por aquellos amec en los que se incluye el IdProductoempresa del jefe de producto.
                    sFiltroRol = string.Format(" inner join (select distinct (idexpediente) from cv_filtro_producto_expediente where IdProductoempresa = {0} ) rol on {1}.idexpediente = rol.idexpediente ", datosRoles.IdProductoempresa, sDBName);
                    //Jose: 22-03-2011 si entra en alguna de las condiciones, se debe saltar el siguiente filtro
                    marketingDelegado = true;
                }
            }
            // Si no se ha hecho ningún filtro previo de roles, entonces se aplica el 4º criterio:
            if (string.IsNullOrEmpty(sFiltroRol))
            {
                // Si el usuario logueado no cumple ninguna de las indicaciones anteriores, filtraremos por la Unidad, área, región y distrito del usuario logueado y en este orden.Si alguno de estos campos, en la tabla de iw_peticionarios está en blanco, significará que no es necesario filtrar por ese campo pues tiene acceso a todo ese nivel.
                // unidad, area, región y distrito

                try
                {
                    StringBuilder db = new StringBuilder();
                    if (datosRoles.administrador.HasValue && datosRoles.administrador.Value)
                    {
                        return sFiltroRol;
                    }
                    else
                    {
                        if (sDBName == "cva")
                        {
                            // 19/1/2011 : Aquí diferenciamos a la hora de filtrar por roles de AMEC y por roles de Expediente según la página 13 del documento
                            if (datosRoles.Idarea.HasValue)
                            {
                                if (db.Length > 0) db.Append(" AND "); else db.Append(" WHERE ");
                                db.AppendFormat(" IdArea = {0} or Idarea1 = {0} or Idarea2 = {0} or Idarea3 = {0} ", datosRoles.Idarea);
                            }
                        }
                        else
                        {
                            if (datosRoles.idunidad.HasValue)
                            {
                                db.Append(" WHERE (");
                                db.AppendFormat(" IdUnidad = {0} ", datosRoles.idunidad);
                            }
                            if (datosRoles.Idarea.HasValue)
                            {
                                if (db.Length > 0) db.Append(" AND "); else db.Append(" WHERE (");
                                db.AppendFormat(" IdArea = {0} ", datosRoles.Idarea);
                            }

                            if (datosRoles.idregion.HasValue)
                            {
                                if (db.Length > 0) db.Append(" AND "); else db.Append(" WHERE (");
                                db.AppendFormat(" IdRegion = {0} ", datosRoles.idregion);
                            }
                            if (datosRoles.iddistrito.HasValue)
                            {
                                if (db.Length > 0) db.Append(" AND "); else db.Append(" WHERE (");
                                db.AppendFormat(" IdDistrito = {0} ", datosRoles.iddistrito);
                            }
                            if (datosRoles.IdDepartament.HasValue && datosRoles.IdDepartament.Value > 0)
                            {
                                if (db.Length > 0) db.Append(" AND "); else db.Append(" WHERE (");
                                db.AppendFormat(" iddepartament = {0} ", datosRoles.IdDepartament);
                            }
                            if (datosRoles.IdSaleforce.HasValue && datosRoles.IdSaleforce.Value > 0)
                            {
                                if (db.Length > 0) db.Append(" AND "); else db.Append(" WHERE (");
                                db.AppendFormat(" idsaleforce = {0} ", datosRoles.IdSaleforce);
                            }
                            if (datosRoles.IdDistrict.HasValue && datosRoles.IdDistrict.Value > 0)
                            {
                                if (db.Length > 0) db.Append(" AND "); else db.Append(" WHERE (");
                                db.AppendFormat(" iddistrict = {0} ", datosRoles.IdDistrict);
                            }

                            if (db.Length > 0) db.Append(") OR "); else db.Append(" WHERE ");
                            db.AppendFormat(" IdPeticionario = {0} ", datosRoles.IdPeticionario);
                        }
                    }
                    if (db.Length > 0)
                    {
                        if (sDBName == "cva")
                        {
                            sFiltroRol = string.Format(" inner join (select distinct (idamec) as idamec from amec {0}) rol on {1}.idamec = rol.idamec ", db.ToString(), sDBName);
                        }
                        else
                        {
                            //20160802 - ALC: El filtro establecido hace que la consulta de un TimeOut en BBDD.
                            sFiltroRol = db.ToString();//string.Format(" inner join (select distinct (idxpediente) as idexpediente from expediente {0}) rol on {1}." + nombreidexpediente + " = rol.idexpediente ", db.ToString(), sDBName);
                            newQuery = true;
                            //sFiltroRol = string.Format(" WHERE {0}." + nombreidexpediente + " IN (select distinct (idxpediente) as idexpediente from expediente  {1}) ", sDBName, db);
                        }
                    }
                }
                catch
                {
                }
            }
            return sFiltroRol;
        }

        public string ObtenerFiltroRol(DVPeticionariosRoles datosRoles, string sDBName, string nombreidexpediente)
        {
            string sFiltroRol = null;
            // Qurius (EAS) 26/20/2011
            if (datosRoles == null) return sFiltroRol;

            bool bAprobador = (datosRoles.aprobador.HasValue) && datosRoles.aprobador.Value;
            bool marketingDelegado = false;

            //Xavier Morell: el administrador puede ver todos los expediente, prevalece sobre el resto de roles.
            if (datosRoles.administrador.HasValue && datosRoles.administrador.Value)
            {
                if(datosRoles.IdConfEmpresa.HasValue && datosRoles.IdConfEmpresa.Value > 0)
                    sFiltroRol = string.Format(" WHERE {0}.IdConfempresa = {1} ", sDBName,datosRoles.IdConfEmpresa.Value);
                return sFiltroRol;
            }
            if (!bAprobador)
            {
                if (datosRoles.marketing.HasValue && !marketingDelegado)
                {
                    if (datosRoles.marketing.Value)
                    {
                        // En caso de que el usuario tenga para su ID en la tabla iw_peticionarios el campo marketing a TRUE, solo verá los AMEC cuyo distrito y área coincidan con los del peticionario, por tanto solo filtraremos por estos campos.

                        // OJO: Puede ocurrir que tenga el campo marketing a true, pero no tenga configurado area o distrito, por eso hacemos estas validaciones:
                        string sWhere = "";
                        if (datosRoles.Idarea.HasValue)
                            //sWhere = string.Format(" WHERE ( IdArea = {0} or Idarea1 = {0} or Idarea2 = {0} or Idarea3 = {0} )", datosRoles.Idarea.ToString());
                            sWhere = string.Format(" WHERE ( IdArea = {0} )", datosRoles.Idarea.ToString());
                        if (datosRoles.iddistrito.HasValue)
                            sWhere += (!string.IsNullOrEmpty(sWhere) ? " AND " : " WHERE ") + " iddistrito = " + datosRoles.iddistrito.ToString();

                        //sFiltroRol = string.Format(" inner join (select distinct (idamec) from amec {0}) rol on {1}.idamec = rol.idamec ", sWhere, sDBName);
                        sFiltroRol = string.Format(" inner join (select distinct (idamec), (idxpediente) from expediente {0}) rol on {1}.IDEXPEDIENTE = rol.idxpediente ", sWhere, sDBName);

                        //Jose: 22-03-2011 si entra en alguna de las condiciones, se debe saltar el siguiente filtro
                        marketingDelegado = true;
                    }
                }
                if (datosRoles.delegado.HasValue && !marketingDelegado)
                {
                    if (datosRoles.delegado.Value)
                    {
                        // Si el usuario tiene el campo delegado a true, solo verá sus datos, es decir, los que ha creado el.
                        if (sDBName == "cva")
                        {
                            sFiltroRol = string.Format(" inner join (select distinct (idamec) as idamec from amec where idpeticionario in (SELECT idpeticionario from peticionarios where login in (select LOGIN FROM peticionarios WHERE IdPeticionario = {0} UNION select 'AMEX_' + LOGIN FROM peticionarios WHERE IdPeticionario = {0} UNION select 'MT_' + LOGIN FROM peticionarios WHERE IdPeticionario = {0}))) rol on {1}.idamec = rol.idamec ", datosRoles.IdPeticionario, sDBName);
                        }
                        else
                        {
                            sFiltroRol = string.Format(" inner join (select distinct (idxpediente) as idexpediente from expediente where idpeticionario in (SELECT idpeticionario from peticionarios where login in (select LOGIN FROM peticionarios WHERE IdPeticionario = {0} UNION select 'AMEX_' + LOGIN FROM peticionarios WHERE IdPeticionario = {0} UNION select 'MT_' + LOGIN FROM peticionarios WHERE IdPeticionario = {0}))) rol on {1}." + nombreidexpediente + " = rol.idexpediente ", datosRoles.IdPeticionario, sDBName);
                        }
                        //Jose: 22-03-2011 si entra en alguna de las condiciones, se debe saltar el siguiente filtro
                        marketingDelegado = true;
                    }
                }
                if (datosRoles.IdProductoempresa.HasValue && string.IsNullOrEmpty(sFiltroRol) && !marketingDelegado)
                {
                    // Si un usuario es jefe de producto. Es porque está dado de alta en la tabla iw_jefes_productoempresa, el modo de filtrar los expedientes y los AMEC será solo filtrando por aquellos expedientes o por aquellos amec en los que se incluye el IdProductoempresa del jefe de producto.
                    sFiltroRol = string.Format(" inner join (select distinct (idexpediente) from cv_filtro_producto_expediente where IdProductoempresa = {0} ) rol on {1}.idexpediente = rol.idexpediente ", datosRoles.IdProductoempresa, sDBName);
                    //Jose: 22-03-2011 si entra en alguna de las condiciones, se debe saltar el siguiente filtro
                    marketingDelegado = true;
                }
            }
            // Si no se ha hecho ningún filtro previo de roles, entonces se aplica el 4º criterio:
            if (string.IsNullOrEmpty(sFiltroRol))
            {
                // Si el usuario logueado no cumple ninguna de las indicaciones anteriores, filtraremos por la Unidad, área, región y distrito del usuario logueado y en este orden.Si alguno de estos campos, en la tabla de iw_peticionarios está en blanco, significará que no es necesario filtrar por ese campo pues tiene acceso a todo ese nivel.
                // unidad, area, región y distrito

                try
                {
                    int idpeticionario = -1;
                    string pidunidad = "null";
                    string pidarea = "null";
                    string pidregion = "null";
                    string piddistrito = "null";

                    StringBuilder db = new StringBuilder();
                    if (datosRoles.administrador.HasValue && datosRoles.administrador.Value)
                    {
                        return sFiltroRol;
                    }
                    else
                    {
                        if (sDBName == "cva")
                        {
                            // 19/1/2011 : Aquí diferenciamos a la hora de filtrar por roles de AMEC y por roles de Expediente según la página 13 del documento
                            if (datosRoles.Idarea.HasValue)
                            {
                                if (db.Length > 0) db.Append(" AND "); else db.Append(" WHERE ");
                                db.AppendFormat(" IdArea = {0} or Idarea1 = {0} or Idarea2 = {0} or Idarea3 = {0} ", datosRoles.Idarea);
                            }
                        }
                        else
                        {
                            if (datosRoles.idunidad.HasValue)
                            {
                                db.Append(" WHERE (");
                                db.AppendFormat(" IdUnidad = {0} ", datosRoles.idunidad);
                            }
                            if (datosRoles.Idarea.HasValue)
                            {
                                if (db.Length > 0) db.Append(" AND "); else db.Append(" WHERE (");
                                db.AppendFormat(" IdArea = {0} ", datosRoles.Idarea);
                            }

                            if (datosRoles.idregion.HasValue)
                            {
                                if (db.Length > 0) db.Append(" AND "); else db.Append(" WHERE (");
                                db.AppendFormat(" IdRegion = {0} ", datosRoles.idregion);
                            }
                            if (datosRoles.iddistrito.HasValue)
                            {
                                if (db.Length > 0) db.Append(" AND "); else db.Append(" WHERE (");
                                db.AppendFormat(" IdDistrito = {0} ", datosRoles.iddistrito);
                            }

                            string queryAmex = "select top 1 idpeticionario, idunidad, idarea, idregion, iddistrito from peticionarios where login = 'AMEX_" + datosRoles.login + "'";
                            DataTable amexTable = Quodem.Sql.SqlServerClient.GetQuery(queryAmex);

                            foreach (DataRow row in amexTable.Rows)
                            {
                                idpeticionario = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idpeticionario");
                                pidunidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idunidad");
                                pidarea = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idarea");
                                pidregion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idregion");
                                piddistrito = Quodem.Utility.DataLayerUtil.GetStringValue(row, "iddistrito");

                                if (db.Length > 0)
                                {
                                    db.Append(") OR ");
                                }
                                else
                                {
                                    db.Append(" WHERE ");
                                }

                                if (pidunidad != String.Empty)
                                {
                                    db.Append(" (");
                                    db.AppendFormat(" IdUnidad = {0} ", pidunidad);
                                }
                                

                                if (pidarea != String.Empty)
                                {
                                    if (db.Length > 0) db.Append(" AND "); else db.Append(" (");
                                    db.AppendFormat(" IdArea = {0} ", pidarea);
                                }

                                if (pidregion != String.Empty)
                                {
                                    if (db.Length > 0) db.Append(" AND "); else db.Append(" (");
                                    db.AppendFormat(" IdRegion = {0} ", pidregion);
                                }


                                if (piddistrito != String.Empty)
                                {
                                    if (db.Length > 0) db.Append(" AND "); else db.Append(" (");
                                    db.AppendFormat(" IdDistrito = {0} ", piddistrito);
                                }

                                if (db.Length > 0) db.Append(") OR "); else db.Append(" WHERE ");
                                
                            }

                            if (amexTable.Rows.Count == 0 && (datosRoles.idunidad.HasValue || datosRoles.Idarea.HasValue || datosRoles.idregion.HasValue || datosRoles.iddistrito.HasValue))
                            {
                                db.Append(") OR ");
                            }

                            if (datosRoles.IdDepartament.HasValue && datosRoles.IdDepartament.Value > 0)
                            {
                                if (db.Length > 0) db.Append(" ( "); else db.Append(" WHERE (");
                                db.AppendFormat(" iddepartament = {0} ", datosRoles.IdDepartament);
                            }
                            if (datosRoles.IdSaleforce.HasValue && datosRoles.IdSaleforce.Value > 0)
                            {
                                if (db.Length > 0) db.Append(" AND "); else db.Append(" WHERE (");
                                db.AppendFormat(" idsaleforce = {0} ", datosRoles.IdSaleforce);
                            }
                            if (datosRoles.IdDistrict.HasValue && datosRoles.IdDistrict.Value > 0)
                            {
                                if (db.Length > 0) db.Append(" AND "); else db.Append(" WHERE (");
                                db.AppendFormat(" iddistrict = {0} ", datosRoles.IdDistrict);
                            }

                            if(db.Length > 0) db.Append(") OR "); else db.Append(" WHERE ");
                            db.AppendFormat(" IdPeticionario in ({0}, {1}) ", datosRoles.IdPeticionario, idpeticionario);

                        }
                    }
                    if (db.Length > 0)
                    {
                        if (sDBName == "cva")
                        {
                            sFiltroRol = string.Format(" inner join (select distinct (idamec) as idamec from amec {0}) rol on {1}.idamec = rol.idamec ", db.ToString(), sDBName);
                        }
                        else
                        {
                            string query = "Exec [dbo].[sp_expedientes_puedo_ver]" + (datosRoles.idunidad.HasValue ? datosRoles.idunidad.Value.ToString(): "null") + "," + (datosRoles.Idarea.HasValue ? datosRoles.Idarea.Value.ToString() : "null") + "," + (datosRoles.idregion.HasValue ? datosRoles.idregion.Value.ToString() : "null") + "," + (datosRoles.iddistrito.HasValue ? datosRoles.iddistrito.Value.ToString() : "null") + "," + (string.IsNullOrWhiteSpace(pidunidad) ? "null" : pidunidad) + "," + (string.IsNullOrWhiteSpace(pidarea) ? "null" : pidarea) + "," + (string.IsNullOrWhiteSpace(pidregion) ? "null" : pidregion) + "," + (string.IsNullOrWhiteSpace(piddistrito) ? "null" : piddistrito) + "," + (datosRoles.IdDepartament.HasValue ? datosRoles.IdDepartament.Value.ToString() : "null") + ", " + (datosRoles.IdSaleforce.HasValue ? datosRoles.IdSaleforce.Value.ToString() : "null") + ", " + (datosRoles.IdDistrict.HasValue ? datosRoles.IdDistrict.Value.ToString() : "null") + " ," + datosRoles.IdPeticionario + "," + idpeticionario + "";
                            DataTable expedientesPuedoVer = Quodem.Sql.SqlServerClient.GetQuery(query);

                            /*List<string> expedientesAccess = new List<string>();
                            foreach (DataRow row in expedientesPuedoVer.Rows)
                            {
                                expedientesAccess.Add(row["idexpediente"].ToString());
                            }*/
                            string queryIn = " ";
                            if(expedientesPuedoVer.Rows.Count > 0)
                            {
                                queryIn = " and exp.idexpediente in (***" + query + "***)";
                            }
                                //20160802 - ALC: El filtro establecido hace que la consulta de un TimeOut en BBDD.
                                sFiltroRol = string.Format(" inner join (select distinct (idxpediente) as idexpediente from expediente {0}) rol on {1}." + nombreidexpediente + " = rol.idexpediente " + queryIn, db.ToString(), sDBName);
                            //sFiltroRol = string.Format(" WHERE {0}." + nombreidexpediente + " IN (select distinct (idxpediente) as idexpediente from expediente  {1}) ", sDBName, db);
                        }
                    }
                }
                catch
                {
                }
            }
            return sFiltroRol;
        }


        public int NumeroExpedientesPorAmec(string NumAmec)
        {

            return MiRepositorioExpedientes.NumeroExpedientesPorAmec(NumAmec);
        }

        public ICollection<DExpediente> ExpedientesPorAmec(string NumAmec)
        {
            return MiRepositorioExpedientes.ExpedientesPorAmec(NumAmec);
        }

        public string ObtenerFiltroRolAMECs(DVPeticionariosRoles datosRoles, bool? bPendienteAprobar)
        {
            string sFiltroRol = null;

            if (bPendienteAprobar.HasValue)
            {
                if (datosRoles != null)
                {
                    bool bAprobador = (datosRoles.aprobador.HasValue) ? datosRoles.aprobador.Value : false;

                    if (bAprobador)
                    {
                        if (datosRoles.idtipoaprobador == 2)
                        {
                            if (bPendienteAprobar.Value)
                                sFiltroRol = string.Format(" AprobadoLegal is null AND AprobadoComplaice is null AND AprobadoDG is null ");
                            else
                                sFiltroRol = string.Format(" AprobadoLegal is not null ");
                        }
                        else if (datosRoles.idtipoaprobador == 3)
                        {
                            if (bPendienteAprobar.Value)
                                sFiltroRol = string.Format(" AprobadoLegal = 1 AND AprobadoComplaice is null AND AprobadoDG is null ");
                            else
                                sFiltroRol = string.Format(" AprobadoComplaice is not null ");
                        }
                        else if (datosRoles.idtipoaprobador == 4)
                        {
                            if (bPendienteAprobar.Value)
                                //sFiltroRol = string.Format(" AprobadoLegal is not null AND AprobadoComplaice is not null AND AprobadoDG is null ");
                                sFiltroRol = string.Format(" AprobadoLegal = 1 AND AprobadoComplaice = 1 AND AprobadoDG is null ");
                            else
                                sFiltroRol = string.Format(" AprobadoDG is not null ");
                        }
                    }
                }
            }
            else
            {
                // Se incluyen todos
            }
            return sFiltroRol;
        }

        /*
        public DCabeceraExpedienteAmpliado ObtenerExpedientePorID(string filtroIdExp)
        {
            return MiRepositorioExpedientes.ObtenerExpedientePorID(filtroIdExp);
        }
        */

        public ICollection<DCabeceraActividad> ObtenerActividades(FiltroActividades filtro)
        {
            return MiRepositorioExpedientes.ObtenerActividades(filtro);
        }

        public long ObtenerNumeroActividades(FiltroActividades filtro)
        {
            return MiRepositorioExpedientes.ObtenerNumeroActividades(filtro);
        }

        public ICollection<DCabeceraEForm> ObtenerDatosEventosFormulario(FiltroEForm filtro)
        {
            return MiRepositorioExpedientes.ObtenerDatosEventosFormulario(filtro);
        }

        public long ObtenerNumDatosEventosFormulario(FiltroEForm filtro)
        {
            return MiRepositorioExpedientes.ObtenerNumDatosEventosFormulario(filtro);
        }

        public int NuevoExpediente(DExpediente expediente)
        {
            expediente.idxpediente = MiRepositorioExpedientes.ObtenerSiguienteIdExpediente();

            string consulta =
                string.Format(
                    "INSERT INTO expediente (idxpediente, idregion, idamec, expediente, idunidad, Idarea, IdPeticionario, IdTipoReserva, IdEmpresa, idestado, iddistrito, fechacreacion, idempleadogp, locked, codexpediente, urgente, importeTotal, visible, iddepartament, idsaleforce, iddistrict, tipoPagoFee ) VALUES ({0} ,{1} ,{2} ,{3} ,{4} ,{5} ,{6} ,{7} ,{8} ,{9} ,{10} ,{11} ,{12} ,{13} ,{14} ,{15} ,{16} ,{17} ,{18} ,{19} ,{20}, {21})",
                    expediente.idxpediente,
                    expediente.idregion == null ? "null" : expediente.idregion.ToString(),
                    expediente.idamec,
                    expediente.expediente == null ? "null" : "'" + expediente.expediente + "'",
                    expediente.idunidad == null ? "null" : expediente.idunidad.ToString(),
                    expediente.Idarea == null ? "null" : expediente.Idarea.ToString(),
                    expediente.IdPeticionario,
                    expediente.IdTipoReserva,
                    expediente.IdEmpresa,
                    "'" + expediente.idestado + "'",
                    (expediente.iddistrito == null || expediente.iddistrito < 0) ? "null" : expediente.iddistrito.ToString(),
                    expediente.fechacreacion == null ? "null" : "'" + expediente.fechacreacion.Value.ToString("yyyyMMdd HH:mm:ss") + "'",
                    string.IsNullOrEmpty(expediente.idempleadogp) ? "null" : "'" + expediente.idempleadogp + "'",
                    expediente.locked == null ? "null" : expediente.locked.ToString(),
                    expediente.codexpediente == null ? "null" : "'" + expediente.codexpediente + "'",
                    expediente.urgente == null ? "null" : expediente.urgente.Value ? "1" : "0",
                    0,
                    1,
                    expediente.iddepartament == null ? "null" : expediente.iddepartament.ToString(),
                    expediente.idsaleforce == null ? "null" : expediente.idsaleforce.ToString(),
                    expediente.iddistrict == null ? "null" : expediente.iddistrict.ToString(),
                    expediente.tipoPagoFee == null ? "null" : expediente.tipoPagoFee.ToString());

            Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
            return expediente.idxpediente;

        }

        public bool ActualizarExpediente(DExpediente expediente)
        {
            string consulta =
                string.Format(
                    "UPDATE expediente SET idregion = {1}, idamec = {2}, idunidad = {3}, Idarea = {4}, IdPeticionario = {5}, IdTipoReserva = {6}, IdEmpresa = {7}, idestado = {8}, iddistrito = {9}, idempleadogp = {10}, codexpediente = {11}, urgente = {12}, iddepartament = {13}, idsaleforce = {14}, iddistrict = {15}, tipoPagoFee = {16}  WHERE idxpediente = {0}",
                    expediente.idxpediente.ToString(),
                    expediente.idregion == null ? "null" : expediente.idregion.ToString(),
                    expediente.idamec,
                    expediente.idunidad == null ? "null" : expediente.idunidad.ToString(),
                    expediente.Idarea == null ? "null" : expediente.Idarea.ToString(),
                    expediente.IdPeticionario,
                    expediente.IdTipoReserva,
                    expediente.IdEmpresa,
                    "'" + expediente.idestado + "'",
                    (expediente.iddistrito == null || expediente.iddistrito < 0) ? "null" : expediente.iddistrito.ToString(),
                     string.IsNullOrEmpty(expediente.idempleadogp) ? "null" : "'" + expediente.idempleadogp + "'",
                    expediente.codexpediente == null ? "null" : "'" + expediente.codexpediente + "'",
                    expediente.urgente == null ? "null" : expediente.urgente.Value ? "1" : "0",
                    expediente.iddepartament == null ? "null" : expediente.iddepartament.ToString(),
                    expediente.idsaleforce == null ? "null" : expediente.idsaleforce.ToString(),
                    expediente.iddistrict == null ? "null" : expediente.iddistrict.ToString(),
                    expediente.tipoPagoFee == null ? "null" : expediente.tipoPagoFee.ToString());

            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta) > 0;
        }

        public int NuevoAMEC(DAmec amec)
        {
            string consulta = string.Empty;

            string consultaIdConfEmpresa = string.Format("select idconfempresa from amecs where idamecs = {0}", amec.amec);
            string idConfEmpresa = amec.idconfempresa != null ? amec.idconfempresa.ToString() : Quodem.Sql.SqlServerClient.GetValue(consultaIdConfEmpresa);

            if (amec.idamec <= 0)
            {
                amec.idamec = MiRepositorioExpedientes.ObtenerSiguienteIdAMEC();

                consulta =
                    string.Format(
                        "INSERT INTO amec ( idamec, amec, IdEmpresa, idpeticionactividad, IdCongreso, inactivo, idestado, aprobado, idpeticionario, fechaaprobadolegal, aprobadolegal, idpeticionario2, fechaaprobadocomplaice, aprobadocomplaice, Idarea, idarea1, idarea2, idarea3, idunidad, idtipoactividadcongreso, iddistrito, paraguas, especificarotras, nombreprograma, codgenesis, objetivosprograma, contenido, lugar, ambitogeografico, duracion, numparticipantes, fechacomienzop, fechafinp, gastosdes_aloj, idcriterio, idcriterio2, idcriterio3, otros, relponentes, numponentes, observacioneslegal, honorarios, conceptogastos, importetotal, numpropuestas, numcartas, numhojasinscripcion, idtipopatrocinio, mailautorizadofi, ficheroprograma, fechaaprobadodg, aprobadodg, observacionescomplaice, observacionesdg, fecha, idpeticionario3, amec_comunicado, comunicar_amec, observaciones, idregion, area, numboletines, PrevisionInicial, idpeticionario_responsable, idconfempresa, newco) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}, {31}, {32}, {33}, {34}, {35}, {36}, {37}, {38}, {39}, {40}, {41}, {42}, {43}, {44}, {45}, {46}, {47}, {48}, {49}, {50}, {51}, {52}, {53}, {54}, {55}, {56}, {57}, {58}, {59}, {60}, {61}, {62}, {63}, {64}, {65})",
                        "'" + amec.idamec + "'",
                        "'" + amec.amec + "'",
                        "'" + amec.IdEmpresa + "'",
                        amec.idpeticionactividad == null ? "null" : "'" + amec.idpeticionactividad.ToString() + "'",
                        amec.IdCongreso == null ? "null" : "'" + amec.IdCongreso.ToString() + "'",
                        0,
                        "'" + amec.idestado + "'",
                        amec.aprobado == null ? "null" : amec.aprobado.Value ? "1" : "0",
                        amec.idpeticionario == null ? "null" : "'" + amec.idpeticionario.ToString() + "'",
                        amec.fechaaprobadolegal == null ? "null" : "'" + amec.fechaaprobadolegal.Value.ToString("yyyyMMdd HH:mm:ss") + "'",
                        amec.aprobadolegal == null ? "null" : amec.aprobadolegal.Value ? "1" : "0",
                        amec.idpeticionario2 == null ? "null" : "'" + amec.idpeticionario2.ToString() + "'",
                        amec.fechaaprobadocomplaice == null ? "null" : "'" + amec.fechaaprobadocomplaice.Value.ToString("yyyyMMdd HH:mm:ss") + "'",
                        amec.aprobadocomplaice == null ? "null" : amec.aprobadocomplaice.Value ? "1" : "0",
                        amec.Idarea == null ? "null" : "'" + amec.Idarea.ToString() + "'",
                        amec.idarea1 == null ? "null" : "'" + amec.idarea1.ToString() + "'",
                        amec.idarea2 == null ? "null" : "'" + amec.idarea2.ToString() + "'",
                        amec.idarea3 == null ? "null" : "'" + amec.idarea3.ToString() + "'",
                        amec.idunidad == null ? "null" : "'" + amec.idunidad.ToString() + "'",
                        amec.idtipoactividadcongreso == null ? "null" : "'" + amec.idtipoactividadcongreso.ToString() + "'",
                        amec.iddistrito == null ? "null" : "'" + amec.iddistrito.ToString() + "'",
                        amec.paraguas == null ? "null" : amec.paraguas.Value ? "1" : "0",
                        amec.especificarotras == null ? "null" : "'" + amec.especificarotras.ToString() + "'",
                        amec.nombreprograma == null ? "null" : "'" + amec.nombreprograma.ToString() + "'",
                        amec.codgenesis == null ? "null" : "'" + amec.codgenesis.ToString() + "'",
                        amec.objetivosprograma == null ? "null" : "'" + amec.objetivosprograma.ToString() + "'",
                        amec.contenido == null ? "null" : "'" + amec.contenido.ToString() + "'",
                        amec.lugar == null ? "null" : "'" + amec.lugar.ToString() + "'",
                        amec.ambitogeografico == null ? "null" : "'" + amec.ambitogeografico.ToString() + "'",
                        amec.duracion == null ? "null" : "'" + amec.duracion.ToString() + "'",
                        amec.numparticipantes == null ? "null" : "'" + amec.numparticipantes.ToString() + "'",
                        amec.fechacomienzop == null ? "null" : "'" + amec.fechacomienzop.Value.ToString("yyyyMMdd HH:mm:ss") + "'",
                        amec.fechafinp == null ? "null" : "'" + amec.fechafinp.Value.ToString("yyyyMMdd HH:mm:ss") + "'",
                        amec.gastosdes_aloj == null ? "null" : "'" + amec.gastosdes_aloj.ToString() + "'",
                        "null",
                        "null",
                        "null",
                        amec.otros == null ? "null" : "'" + amec.otros.ToString() + "'",
                        amec.relponentes == null ? "null" : "'" + amec.relponentes.ToString() + "'",
                        amec.numponentes == null ? "null" : "'" + amec.numponentes.ToString() + "'",
                        amec.observacioneslegal == null ? "null" : "'" + amec.observacioneslegal.ToString() + "'",
                        amec.honorarios == null ? "null" : "'" + amec.honorarios.ToString() + "'",
                        amec.conceptogastos == null ? "null" : "'" + amec.conceptogastos.ToString() + "'",
                        amec.importetotal == null ? "null" : "'" + amec.importetotal.ToString() + "'",
                        amec.numpropuestas == null ? "null" : "'" + amec.numpropuestas.ToString() + "'",
                        amec.numcartas == null ? "null" : "'" + amec.numcartas.ToString() + "'",
                        amec.numhojasinscripcion == null ? "null" : "'" + amec.numhojasinscripcion.ToString() + "'",
                        amec.idtipopatrocinio == null ? "null" : "'" + amec.idtipopatrocinio.ToString() + "'",
                        amec.mailautorizadofi == null ? "null" : "'" + amec.mailautorizadofi.ToString() + "'",
                        amec.ficheroprograma == null ? "null" : "'" + amec.ficheroprograma.ToString() + "'",
                        amec.fechaaprobadodg == null ? "null" : "'" + amec.fechaaprobadodg.Value.ToString("yyyyMMdd HH:mm:ss") + "'",
                        amec.aprobadodg == null ? "null" : "'" + amec.aprobadodg.ToString() + "'",
                        amec.observacionescomplaice == null ? "null" : "'" + amec.observacionescomplaice.ToString() + "'",
                        amec.observacionesdg == null ? "null" : "'" + amec.observacionesdg.ToString() + "'",
                        amec.fecha == null ? "null" : "'" + amec.fecha.Value.ToString("yyyyMMdd HH:mm:ss") + "'",
                        "null",
                        amec.amec_comunicado == null ? "null" : amec.amec_comunicado.Value ? "1" : "0",
                        amec.comunicar_amec == null ? "null" : amec.comunicar_amec.Value ? "1" : "0",
                        amec.observaciones == null ? "null" : "'" + amec.observaciones.ToString() + "'",
                        "null",
                        amec.area == null ? "null" : "'" + amec.area.ToString() + "'",
                        amec.numboletines == null ? "null" : "'" + amec.numboletines.ToString() + "'",
                        amec.PrevisionInicial == null ? "null" : "'" + amec.PrevisionInicial.ToString() + "'",
                        "null",
                        idConfEmpresa,
                        amec.newco == null ? "null" : amec.newco.Value ? "1" : "0");
            }
            else
            {
                consulta =
                    string.Format(
                        "UPDATE [dbo].[amec] SET [amec] = {1}, [IdEmpresa] = {2}, [idpeticionactividad] = {3}, [IdCongreso] = {4}, [inactivo] = {5}, [idestado] = {6}, [aprobado] = {7}, [idpeticionario] = {8}, [fechaaprobadolegal] = {9}, [aprobadolegal] = {10}, [idpeticionario2] = {11}, [fechaaprobadocomplaice] = {12}, [aprobadocomplaice] = {13}, [Idarea] = {14}, [idarea1] = {15}, [idarea2] = {16}, [idarea3] = {17}, [idunidad] = {18}, [idtipoactividadcongreso] = {19}, [iddistrito] = {20}, [paraguas] = {21}, [especificarotras] = {22}, [nombreprograma] = {23}, [codgenesis] = {24}, [objetivosprograma] = {25}, [contenido] = {26}, [lugar] = {27}, [ambitogeografico] = {28}, [duracion] = {29}, [numparticipantes] = {30}, [fechacomienzop] = {31}, [fechafinp] = {32}, [gastosdes_aloj] = {33}, [idcriterio] = {34}, [idcriterio2] = {35}, [idcriterio3] = {36}, [otros] = {37}, [relponentes] = {38}, [numponentes] = {39}, [observacioneslegal] = {40}, [honorarios] = {41}, [conceptogastos] = {42}, [importetotal] = {43}, [numpropuestas] = {44}, [numcartas] = {45}, [numhojasinscripcion] = {46}, [idtipopatrocinio] = {47}, [mailautorizadofi] = {48}, [ficheroprograma] = {49}, [fechaaprobadodg] = {50}, [aprobadodg] = {51}, [observacionescomplaice] = {52}, [observacionesdg] = {53}, [fecha] = {54}, [idpeticionario3] = {55}, [amec_comunicado] = {56}, [comunicar_amec] = {57}, [observaciones] = {58}, [idregion] = {59}, [area] = {60}, [numboletines] = {61}, [PrevisionInicial] = {62}, [idpeticionario_responsable] = {63}, [idconfempresa] = {64}, [newco] = {65} WHERE [idamec] = {0};",
                        "'" + amec.idamec + "'",
                        "'" + amec.amec + "'",
                        "'" + amec.IdEmpresa + "'",
                        amec.idpeticionactividad == null ? "null" : "'" + amec.idpeticionactividad.ToString() + "'",
                        amec.IdCongreso == null ? "null" : "'" + amec.IdCongreso.ToString() + "'",
                        0,
                        "'" + amec.idestado + "'",
                        amec.aprobado == null ? "null" : amec.aprobado.Value ? "1" : "0",
                        amec.idpeticionario == null ? "null" : "'" + amec.idpeticionario.ToString() + "'",
                        amec.fechaaprobadolegal == null ? "null" : "'" + amec.fechaaprobadolegal.Value.ToString("yyyyMMdd HH:mm:ss") + "'",
                        amec.aprobadolegal == null ? "null" : amec.aprobadolegal.Value ? "1" : "0",
                        amec.idpeticionario2 == null ? "null" : "'" + amec.idpeticionario2.ToString() + "'",
                        amec.fechaaprobadocomplaice == null ? "null" : "'" + amec.fechaaprobadocomplaice.Value.ToString("yyyyMMdd HH:mm:ss") + "'",
                        amec.aprobadocomplaice == null ? "null" : amec.aprobadocomplaice.Value ? "1" : "0",
                        amec.Idarea == null ? "null" : "'" + amec.Idarea.ToString() + "'",
                        amec.idarea1 == null ? "null" : "'" + amec.idarea1.ToString() + "'",
                        amec.idarea2 == null ? "null" : "'" + amec.idarea2.ToString() + "'",
                        amec.idarea3 == null ? "null" : "'" + amec.idarea3.ToString() + "'",
                        amec.idunidad == null ? "null" : "'" + amec.idunidad.ToString() + "'",
                        amec.idtipoactividadcongreso == null ? "null" : "'" + amec.idtipoactividadcongreso.ToString() + "'",
                        amec.iddistrito == null ? "null" : "'" + amec.iddistrito.ToString() + "'",
                        amec.paraguas == null ? "null" : amec.paraguas.Value ? "1" : "0",
                        amec.especificarotras == null ? "null" : "'" + amec.especificarotras.ToString() + "'",
                        amec.nombreprograma == null ? "null" : "'" + amec.nombreprograma.ToString() + "'",
                        amec.codgenesis == null ? "null" : "'" + amec.codgenesis.ToString() + "'",
                        amec.objetivosprograma == null ? "null" : "'" + amec.objetivosprograma.ToString() + "'",
                        amec.contenido == null ? "null" : "'" + amec.contenido.ToString() + "'",
                        amec.lugar == null ? "null" : "'" + amec.lugar.ToString() + "'",
                        amec.ambitogeografico == null ? "null" : "'" + amec.ambitogeografico.ToString() + "'",
                        amec.duracion == null ? "null" : "'" + amec.duracion.ToString() + "'",
                        amec.numparticipantes == null ? "null" : "'" + amec.numparticipantes.ToString() + "'",
                        amec.fechacomienzop == null ? "null" : "'" + amec.fechacomienzop.Value.ToString("yyyyMMdd HH:mm:ss") + "'",
                        amec.fechafinp == null ? "null" : "'" + amec.fechafinp.Value.ToString("yyyyMMdd HH:mm:ss") + "'",
                        amec.gastosdes_aloj == null ? "null" : "'" + amec.gastosdes_aloj.ToString() + "'",
                        "null",
                        "null",
                        "null",
                        amec.otros == null ? "null" : "'" + amec.otros.ToString() + "'",
                        amec.relponentes == null ? "null" : "'" + amec.relponentes.ToString() + "'",
                        amec.numponentes == null ? "null" : "'" + amec.numponentes.ToString() + "'",
                        amec.observacioneslegal == null ? "null" : "'" + amec.observacioneslegal.ToString() + "'",
                        amec.honorarios == null ? "null" : "'" + amec.honorarios.ToString() + "'",
                        amec.conceptogastos == null ? "null" : "'" + amec.conceptogastos.ToString() + "'",
                        amec.importetotal == null ? "null" : "'" + amec.importetotal.ToString() + "'",
                        amec.numpropuestas == null ? "null" : "'" + amec.numpropuestas.ToString() + "'",
                        amec.numcartas == null ? "null" : "'" + amec.numcartas.ToString() + "'",
                        amec.numhojasinscripcion == null ? "null" : "'" + amec.numhojasinscripcion.ToString() + "'",
                        amec.idtipopatrocinio == null ? "null" : "'" + amec.idtipopatrocinio.ToString() + "'",
                        amec.mailautorizadofi == null ? "null" : "'" + amec.mailautorizadofi.ToString() + "'",
                        amec.ficheroprograma == null ? "null" : "'" + amec.ficheroprograma.ToString() + "'",
                        amec.fechaaprobadodg == null ? "null" : "'" + amec.fechaaprobadodg.Value.ToString("yyyyMMdd HH:mm:ss") + "'",
                        amec.aprobadodg == null ? "null" : "'" + amec.aprobadodg.ToString() + "'",
                        amec.observacionescomplaice == null ? "null" : "'" + amec.observacionescomplaice.ToString() + "'",
                        amec.observacionesdg == null ? "null" : "'" + amec.observacionesdg.ToString() + "'",
                        amec.fecha == null ? "null" : "'" + amec.fecha.Value.ToString("yyyyMMdd HH:mm:ss") + "'",
                        "null",
                        amec.amec_comunicado == null ? "null" : amec.amec_comunicado.Value ? "1" : "0",
                        amec.comunicar_amec == null ? "null" : amec.comunicar_amec.Value ? "1" : "0",
                        amec.observaciones == null ? "null" : "'" + amec.observaciones.ToString() + "'",
                        "null",
                        amec.area == null ? "null" : "'" + amec.area.ToString() + "'",
                        amec.numboletines == null ? "null" : "'" + amec.numboletines.ToString() + "'",
                        amec.PrevisionInicial == null ? "null" : "'" + amec.PrevisionInicial.ToString() + "'",
                        "null",
                        idConfEmpresa,
                        amec.newco == null ? "null" : amec.newco.Value ? "1" : "0");

            }
            Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            return amec.idamec;
        }

        public int NuevaPeticionActividad(DPeticionActividad peticion)
        {
            peticion.idpeticionactividad = MiRepositorioExpedientes.ObtenerSiguienteIdPeticionActividad();

            string consulta =
                string.Format(
                    "INSERT INTO peticiones_actividad (idpeticionactividad, nombre, desde, hasta, IdPoblacion, IdCongreso, idespecialidad, sede, IdTipoCongreso, locked, internacional, url_web, email_secretaria, especialidad, fechacreacion, IdPeticionario, comunicar, valoracion_farmaindustria, idconfempresa ) VALUES ({0} ,{1} ,{2} ,{3} ,{4} ,{5} ,{6} ,{7} ,{8} ,{9} ,{10} ,{11} ,{12} ,{13} ,{14} ,{15} ,{16} ,{17}, {18})",
                    peticion.idpeticionactividad,
                    "'" + peticion.nombre + "'",
                    "'" + peticion.desde.ToString("yyyyMMdd HH:mm:ss") + "'",
                    "'" + peticion.hasta.ToString("yyyyMMdd HH:mm:ss") + "'",
                    "'" + peticion.IDPoblacion + "'",
                    peticion.IdCongreso == null ? "null" : "'" + peticion.IdCongreso.ToString() + "'",
                    peticion.idespecialidad == null ? "null" : "'" + peticion.idespecialidad.ToString() + "'",
                    "'" + peticion.sede + "'",
                    peticion.IdTipoCongreso == null ? "null" : "'" + peticion.IdTipoCongreso.ToString() + "'",
                    peticion.locked == null ? "null" : peticion.locked.ToString(),
                    peticion.internacional == null ? "null" : peticion.internacional.Value ? "1" : "0",
                    "'" + peticion.url_web + "'",
                    "'" + peticion.email_secretaria + "'",
                    "'" + peticion.especialidad + "'",
                    peticion.fechacreacion == null ? "null" : "'" + peticion.fechacreacion.Value.ToString("yyyyMMdd HH:mm:ss") + "'",
                    peticion.idpeticionario == null ? "null" : "'" + peticion.idpeticionario.ToString() + "'",
                    peticion.comunicar == null ? "null" : peticion.comunicar.Value ? "1" : "0",
                    "'" + peticion.valoracion_farmaindustria + "'",
                    peticion.idconfempresa == null ? "null" : peticion.idconfempresa.Value.ToString());

            Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            return peticion.idpeticionactividad;
        }

        public int CrearCopiaExpediente(int nIDExpediente, DDatosPersonalesUsuario datosUsuario)
        {
            return MiRepositorioExpedientes.CrearCopiaExpediente(nIDExpediente, datosUsuario);
        }

        public ICollection<DVAmecCongreso> ObtenerAMECs(FiltroAMECs filtro, DVPeticionariosRoles datosRoles)
        {
            ICollection<DVAmecCongreso> iLista = null;
            bool bConRoles = false;
            if (datosRoles != null)
            {
                bConRoles = (datosRoles.aprobador.HasValue) ? datosRoles.aprobador.Value : false;
            }

            if (!bConRoles)
            {
                // 25/1/2011: Se quitan los filtros de Rol
                //filtro.Roles = ObtenerFiltroRol(datosRoles, "cva");
                iLista = MiRepositorioExpedientes.ObtenerAMECs(filtro);
            }
            else
            {
                // 25/1/2011: Se quitan los filtros de Rol
                //bool? bPendienteAprobar = string.IsNullOrEmpty(filtro.PendientesAprobar) ? new Nullable<bool>() : ((filtro.PendientesAprobar == "1") ? true : false);
                //filtro.PendientesAprobar = ObtenerFiltroRolAMECs(datosRoles, bPendienteAprobar);
                iLista = MiRepositorioExpedientes.ObtenerAMECs(filtro);
            }
            return iLista;
        }

        public long ObtenerNumeroAMECs(FiltroAMECs filtro, DVPeticionariosRoles datosRoles)
        {
            long lNumero = 0;
            bool bConRoles = false;

            if (datosRoles != null)
            {
                bConRoles = (datosRoles.aprobador.HasValue) ? datosRoles.aprobador.Value : false;
            }

            if (!bConRoles)
            {
                // 25/1/2011: Se quitan los filtros de Rol
                //filtro.Roles = ObtenerFiltroRol(datosRoles, "cva");
                lNumero = MiRepositorioExpedientes.ObtenerNumeroAMECs(filtro);
            }
            else
            {
                // 25/1/2011: Se quitan los filtros de Rol
                //bool? bPendienteAprobar = string.IsNullOrEmpty(filtro.PendientesAprobar) ? new Nullable<bool>() : ((filtro.PendientesAprobar == "1") ? true : false);
                //filtro.PendientesAprobar = ObtenerFiltroRolAMECs(datosRoles, bPendienteAprobar);
                lNumero = MiRepositorioExpedientes.ObtenerNumeroAMECs(filtro);
            }
            return lNumero;
        }


        public ICollection<DVAmecCongresoConcatSolicitante> ObtenerAMECPorCongresoConcatSolicitante(int idcongreso, string AmecPorRoles, int amecReuniones, DVPeticionariosRoles roles, int? idconfempresa, bool includeOldAmecs = true, string idAmecsLike = null)
        {
            ICollection<DVAmecCongresoConcatSolicitante> myEnum = null;

            myEnum = MiRepositorioExpedientes.ObtenerAMECPorCongresoConcatSolicitanteProcedure(idcongreso, AmecPorRoles, amecReuniones, roles, idconfempresa, includeOldAmecs, idAmecsLike);
            return myEnum;
        }


        public ICollection<DAmec> ObtenerEntidadAMECs(FiltroAMECs filtro)
        {
            return MiRepositorioExpedientes.ObtenerEntidadAMECs(filtro);
        }

        public IEnumerable<DVServicioHotel> ObtenerServiciosHoteles(int idExpediente)
        {
            return MiRepositorioExpedientes.ObtenerServiciosHoteles(idExpediente);
        }

        public IEnumerable<DVServicioActividades> ObtenerOtrosServicios(int idExpediente)
        {
            return MiRepositorioExpedientes.ObtenerOtrosServicios(idExpediente);
        }

        public IEnumerable<DVServicioInscripciones> ObtenerServiciosInscripciones(int idExpediente)
        {
            return MiRepositorioExpedientes.ObtenerServiciosInscripciones(idExpediente);
        }

        public IEnumerable<DVServicioTransporte> ObtenerServiciosTransportes(int idExpediente)
        {
            return MiRepositorioExpedientes.ObtenerServiciosTransportes(idExpediente);
        }

        public ServicioTransporte ObtenerServicioTransporte(int idExpediente)
        {
            DVServicioTransporte dvserv = MiRepositorioExpedientes.ObtenerServicioTransporte(idExpediente);

            if (dvserv == null) return null;

            return MapeadorExpedientes.Copiar_DVServicioTransporte_a_ServicioTransporte(dvserv);
        }

        /*Pacifico 07012011*/

        public DVCongresos ObtenerCongreso(int idActividad)
        {
            return MiRepositorioExpedientes.ObtenerCongreso(idActividad);
        }

        public ICollection<DVServicioPassengerResumen> ObtenerResumenParticipantes(FiltroExpedientesAvanzado filtro)
        {
            return MiRepositorioExpedientes.ObtenerResumenParticipantes(filtro);
        }

        public long ObtenerNumeroResumenParticipantes(FiltroExpedientesAvanzado filtro)
        {
            return MiRepositorioExpedientes.ObtenerNumeroResumenParticipantes(filtro);
        }

        public ICollection<DVResumenEstados> ObtenerResumenEstados(DVPeticionariosRoles datosRoles)
        {
            bool newQuery;
            string query = ObtenerFiltroRolExpNew(datosRoles, "exp", "idexpediente", out newQuery);
            return MiRepositorioExpedientes.ObtenerResumenEstados(query, newQuery);
        }

        public int CambiaEstadoExpedienteEnviado(int nIDExpediente)
        {
            return MiRepositorioExpedientes.CambiaEstadoExpedienteEnviado(nIDExpediente);
        }

        public int CambiaEstadoExpedienteAprobado(int nIDExpediente, ref int nValidaEstadoAmec, ref int nValidaPresupAmec)
        {
            return MiRepositorioExpedientes.CambiaEstadoExpedienteAprobado(nIDExpediente, ref nValidaEstadoAmec, ref nValidaPresupAmec);
        }

        public int CambiaEstadoExpedienteCancelado(int nIDExpediente)
        {
            return MiRepositorioExpedientes.CambiaEstadoExpedienteCancelado(nIDExpediente);
        }

        public int CambiaEstadoExpedienteFinalizado (int idexpediente)
        {
            return MiRepositorioExpedientes.CambiaEstadoExpedienteFinalizado(idexpediente);
        }

        public DEmpGpPetAmecExp ObtenerEmpleadoGpPorPetAmecExp(int idPet, string idAmec, int idExp)
        {
            return MiRepositorioExpedientes.ObtenerEmpleadoGpPorPetAmecExp(idPet, idAmec, idExp);
        }

        public int CambiaEstadoServicioSinEnviar(int nIDReserva)
        {
            return MiRepositorioExpedientes.CambiaEstadoServicio(nIDReserva, "AB");
        }

        public int CambiaEstadoServicioAceptado(int nIDReserva)
        {
            return MiRepositorioExpedientes.CambiaEstadoServicio(nIDReserva, "CFP");
        }

        public int CambiaEstadoServicioCancelado(int nIDReserva)
        {
            return MiRepositorioExpedientes.CambiaEstadoServicioCancelado(nIDReserva);
        }

        public string ObtenerSiguienteNombreAMEC(string sCodAgencia)
        {
            return MiRepositorioExpedientes.ObtenerSiguienteNombreAMEC(sCodAgencia);
        }

        public string GetEstado(int idexpediente)
        {
            return MiRepositorioExpedientes.GetEstado(idexpediente);
        }

        public int AprobarAMEC(string nIDAmec, int nTipoAprobador, string sObservaciones)
        {
            FiltroAMECs flt = new FiltroAMECs();
            flt.IdAMEC = nIDAmec;
            int nGuardado = 0;

            DAmec miAmec = ObtenerEntidadAMECs(flt).FirstOrDefault();

            if (miAmec != null)
            {
                //switch (nTipoAprobador)
                //{
                //    case 2: //  Legal
                //        if (!miAmec.aprobadolegal.HasValue)
                //        {
                //            miAmec.aprobadolegal = true;
                //            miAmec.fechaaprobadolegal = System.DateTime.Now;
                //            miAmec.observacioneslegal = sObservaciones;

                //            nGuardado = NuevoAMEC(miAmec);
                //        }
                //        break;

                //    case 3: //  Compliance
                //        if (miAmec.aprobadolegal.HasValue)
                //        {
                //            if (miAmec.aprobadolegal.Value && !miAmec.aprobadocomplaice.HasValue)
                //            {
                //                miAmec.aprobadocomplaice = true;
                //                miAmec.fechaaprobadocomplaice = System.DateTime.Now;
                //                miAmec.observacionescomplaice = sObservaciones;

                //                nGuardado = NuevoAMEC(miAmec);
                //            }
                //        }
                //        break;

                //    case 4: //  DG
                //        if (miAmec.aprobadolegal.HasValue && miAmec.aprobadocomplaice.HasValue)
                //        {
                //            if (miAmec.aprobadolegal.Value && miAmec.aprobadocomplaice.Value && !miAmec.aprobadodg.HasValue)
                //            {
                //                miAmec.aprobadodg = true;
                //                miAmec.fechaaprobadodg = System.DateTime.Now;
                //                miAmec.observacionesdg = sObservaciones;
                //                miAmec.aprobado = true;

                //                nGuardado = NuevoAMEC(miAmec);
                //            }
                //        }
                //        break;
                //}
            }

            return nGuardado;
        }

        public bool PuedeAprobar(int nIDExpediente)
        {
            return MiRepositorioExpedientes.PuedeAprobar(nIDExpediente);
        }

        public bool EsCancelableAMEC(string nIDAmec)
        {
            return MiRepositorioExpedientes.EsCancelableAMEC(nIDAmec);
        }

        // Ismael Ameller 08-03-2011 Sacamos los Labels de la BBDD
        public string ObtenerLabelEstado(string idEstado)
        {
            return MiRepositorioExpedientes.ObtenerLabelEstado(idEstado);
        }

        //FIN Ismael Ameller 08-03-2011 Sacamos los Labels de la BBDD

        //Ismael Ameller 22-03-2011 Agrega productos al expediente
        public bool AgregaProductoExpedienteArea(ExpAreaProductoEmpresa entidad)
        {
            try
            {
                string consulta =
                    string.Format(
                        "INSERT INTO expedientes_areasproductosempresa (idexpediente, IdareaProductoempresa, porcentaje, locked) VALUES ({0}, {1}, {2}, {3})",
                        entidad.idExpediente,
                        entidad.idAreaProductoEmpresa,
                        entidad.porcentaje,
                        entidad.locked);

                Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void ClearProductosExpediente(int idExp)
        {
            MiRepositorioExpedientes.ClearProductosExpediente(idExp);
        }

        //FIN Ismael Ameller 22-03-2011 Agrega productos al expediente

        //Ismael Ameller 23-03-2011 Obtenemos productos del expediente
        public string ProductosExpediente(string IdExp)
        {
            return MiRepositorioExpedientes.ProductosExpediente(IdExp);
        }

        public bool CheckJustificanteInscripcion(int idExpediente, int idReserva, int idServicio, int idServicioAlojamiento, int idServicioTransporte)
        {
            return MiRepositorioExpedientes.CheckJustificanteInscripcion(idExpediente, idReserva, idServicio, idServicioAlojamiento, idServicioTransporte);
        }

        //FIN Ismael Ameller 23-03-2011 Obtenemos productos del expediente

        public ICollection<DProductoPorcentajeVista> ProductosExpedienteItems(string IdExp)
        {
            return MiRepositorioExpedientes.ProductosExpedienteItems(IdExp);
        }

        public bool GuardarHonorariosPassenger(int NuevoExpediente, DataTable honorariosCreados)
        {
            return MiRepositorioExpedientes.GuardarHonorariosPassenger(NuevoExpediente, honorariosCreados);

        }

        public bool EliminarHonorariosPassenger(int NuevoExpediente, List<int> participantesEliminados)
        {
            return MiRepositorioExpedientes.EliminarHonorariosPassenger(NuevoExpediente, participantesEliminados);
        }

        public bool EsEventoInternacional(int idevento)
        {
            return MiRepositorioExpedientes.EsEventoInternacional(idevento);
        }

        public string ObtenerMensajeAMostrar(string tipoActividad, string tipoAsistente, string riskLevel)
        {
            return MiRepositorioExpedientes.ObtenerMensajeAMostrar( tipoActividad, tipoAsistente, riskLevel);
        }
        
    }
}