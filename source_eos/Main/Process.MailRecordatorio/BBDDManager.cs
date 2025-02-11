using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using msd.MailRecordatorio.DLL;

namespace msd.MailRecordatorio
{
    public class BBDDManager
    {

        public DataTable GetAmecsRecordatorio()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Email", typeof(System.String));
            dt.Columns.Add("IdAmec", typeof(System.String));
            dt.Columns.Add("IdPeticionario", typeof(System.Int32));
            dt.NewRow();
            try
            {
                DataTable aux = GetEnvioMailMedico();
                foreach (DataRow row in aux.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["Email"] = Variables.EmailMedico;
                    dr["IdAmec"] = row["IdAmec"];
                    dr["IdPeticionario"] = "";
                    dt.Rows.Add(dr);
                }
                aux = GetEnvioMailLegal();
                foreach (DataRow row in aux.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["Email"] = Variables.EmailLegal;
                    dr["IdAmec"] = row["IdAmec"];
                    dr["IdPeticionario"] = "";
                    dt.Rows.Add(dr);
                }
                aux = GetEnvioMailSupJer();
                foreach (DataRow row in aux.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["Email"] = row["Email"];
                    dr["IdAmec"] = row["IdAmec"];
                    dr["IdPeticionario"] = row["IdPeticionario"];
                    dt.Rows.Add(dr);
                }
                aux = GetEnvioMailNegocio();
                foreach (DataRow row in aux.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["Email"] = row["Email"];
                    dr["IdAmec"] = row["IdAmec"];
                    dr["IdPeticionario"] = row["IdPeticionario"];
                    dt.Rows.Add(dr);
                }
                aux = GetMailTodosNegocio();
                foreach (DataRow row in aux.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["Email"] = row["Email"];
                    dr["IdAmec"] = row["IdAmec"];
                    dr["IdPeticionario"] = row["IdPeticionario"];
                    dt.Rows.Add(dr);
                }
                return dt;

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// Comprobamos si el en el historico hay dos registros a partir de la ultima vez que se sometió el amec. Si hay dos registros, es que
        /// el medico ha aprobado (pendiente de aprobar + aprobado) si solo hay uno, esta pendiente de aprobar
        /// </summary>
        /// <returns></returns>
        private DataTable GetEnvioMailMedico()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = string.Format(" SELECT am.idamecs as idamec" +
                                             " FROM amecs am " +
                                             " INNER JOIN histasocamecs his on am.idamecs = his.idamecs " +
                                             " WHERE DATEDIFF(day, " +
                                                           " (SELECT TOP 1 CAST(fechacreacion AS DATE) " +
                                                           " FROM histasocamecs where idamecs = am.idamecs order by 1 DESC), " +
                                                           " CAST(am.fechaamecs AS DATE)) = {0} " +
                                             " AND am.idestado NOT IN (1,2,3,4,5) " +
                                             " AND (his.idnivelaprobacion = am.idnivelaprobacion AND am.idnivelaprobacion = 2) " +
                                             " AND his.fechacreacion >= (SELECT top 1 fechacreacion " +
                                                                         "FROM histasocamecs " +
                                                                         "WHERE idamecs = am.idamecs and idestado = 29 order by 1 desc) " +
                                             " GROUP BY am.idamecs " +
                                             " HAVING COUNT(am.idamecs) < 2 ", Variables.RangoDias);
                ds.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(query));
                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Comprobamos si el en el historico hay dos registros a partir de la ultima vez que se sometió el amec. Si hay dos registros, es que
        /// legal ha aprobado (pendiente de aprobar + aprobado) si solo hay uno, esta pendiente de aprobar
        /// </summary>
        /// <returns></returns>
        private DataTable GetEnvioMailLegal()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = string.Format(" SELECT am.idamecs as idamec" +
                                             " FROM amecs am " +
                                             " INNER JOIN histasocamecs his on am.idamecs = his.idamecs " +
                                             " WHERE DATEDIFF(day, " +
                                                           " (SELECT TOP 1 CAST(fechacreacion AS DATE) " +
                                                           " FROM histasocamecs where idamecs = am.idamecs order by 1 DESC), " +
                                                           " CAST(am.fechaamecs AS DATE)) = {0} " +
                                             " AND am.idestado NOT IN (1,2,3,4,5) " +
                                             " AND (his.idnivelaprobacion = am.idnivelaprobacion AND am.idnivelaprobacion = 6) " +
                                             " AND his.fechacreacion >= (SELECT top 1 fechacreacion " +
                                                                         "FROM histasocamecs " +
                                                                         "WHERE idamecs = am.idamecs and idestado = 29 order by 1 desc) " +
                                             " GROUP BY am.idamecs " +
                                             " HAVING COUNT(am.idamecs) < 2 ", Variables.RangoDias);
                ds.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(query));
                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private DataTable GetEnvioMailSupJer()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = string.Format("select p.IdPeticionario,p.Email as email,apr.idamecs as idamec from aprobadoramec apr inner join " +
                                    " (select am.idamecs,his.idcreadopor,his.idestado,idestadoamechist " +
                                        " from amecs am " +
                                        " inner join histasocamecs his on am.idamecs = his.idamecs " +
                                        " where DATEDIFF(day," +
                                                      "(select TOP 1 CAST(fechacreacion AS DATE)" +
                                                      "from histasocamecs where idamecs = am.idamecs order by 1 DESC)," +
                                                      "CAST(am.fechaamecs AS DATE)) = {0} " +
                                            " AND am.idestado NOT IN (1,2,3,4,5)" +
                                            " AND (his.idnivelaprobacion = am.idnivelaprobacion AND  his.idnivelaprobacion = 3) " +
                                            " AND am.idestado <> his.idestado" +
                                            " AND his.fechacreacion >= (SELECT top 1 fechacreacion " +
                                                                       " from histasocamecs" +
                                                                       " where idamecs = am.idamecs and idestado = 29 order by 1 desc)" +
                                      " ) t ON t.idamecs = apr.idamecs AND t.idcreadopor <> apr.idaprobador" +
                                " inner join peticionarios p on p.IdPeticionario = apr.idaprobador", Variables.RangoDias);
                ds.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(query));
                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private DataTable GetEnvioMailNegocio()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = string.Format(" SELECT pet.IdPeticionario,pet.Email,T.idamecs FROM agency_user_approval_structure ag " +
                                            " INNER JOIN aprobadoramec apr ON ag.IdPeticionario = apr.idaprobador " +
                                            " INNER JOIN (SELECT am.idamecs,his.idcreadopor,his.idestado,idestadoamechist " +
                                            " FROM amecs am  " +
                                            " INNER JOIN histasocamecs his on am.idamecs = his.idamecs " +
                                            " WHERE DATEDIFF(day,(SELECT TOP 1 CAST(fechacreacion AS DATE) " +
                                                                " FROM histasocamecs " +
                                                                " WHERE idamecs = am.idamecs order by 1 DESC),CAST(am.fechaamecs AS DATE)) = {0} " +
                                            " AND am.idestado NOT IN (1,2,3,4,5) AND (his.idnivelaprobacion = am.idnivelaprobacion " +
                                            " AND  his.idnivelaprobacion = 4)  AND am.idestado <> his.idestado " +
                                            " AND his.fechacreacion >= (SELECT top 1 fechacreacion  " +
                                                                      " FROM histasocamecs " +
                                                                      " WHERE idamecs = am.idamecs and idestado = 29 ORDER BY 1 DESC) " +
                                            " ) T ON apr.idamecs = T.idamecs AND ag.IdPeticionarioManager <> T.idcreadopor " +
                                            " INNER JOIN peticionarios pet ON ag.IdPeticionarioManager = pet.IdPeticionario ", Variables.RangoDias);
                ds.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(query));
                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private DataTable CheckSiNadieAprobo()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = string.Format(" select am.idamecs IdAmec ,am.idnivelaprobacion as NivelAprobacion ,COUNT(idestadoamechist) as RecuentoHistorial,(SELECT COUNT(1) from aprobadoramec WHERE idamecs = am.idamecs) RecuentoAprobadores" +
                                            " from amecs am" +
                                            " left join histasocamecs his on am.idamecs = his.idamecs" +
                                            " where DATEDIFF(day," +
                                                          "(select TOP 1 CAST(fechacreacion AS DATE)" +
                                                          " from histasocamecs where idamecs = am.idamecs order by 1 DESC)," +
                                                          "CAST(am.fechaamecs AS DATE)) >= {0}" +
                                            " AND am.idestado NOT IN (1,2,3,4,5)" +
                                            " AND his.idnivelaprobacion = am.idnivelaprobacion" +
                                            " AND his.fechacreacion >= (SELECT top 1 fechacreacion" +
                                                                        " from histasocamecs" +
                                                                        " where idamecs = am.idamecs and idestado = 29 order by 1 desc)" +
                                            " GROUP BY am.idamecs,am.idnivelaprobacion", Variables.RangoDias);
                ds.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(query));
                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private DataTable GetMailTodosNegocio()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Email", typeof(System.String));
            dt.Columns.Add("IdAmec", typeof(System.String));
            dt.Columns.Add("IdPeticionario", typeof(System.Int32));
            string query = "";
            try
            {
                foreach (DataRow row in CheckSiNadieAprobo().Rows)
                {

                    if (int.Parse(row["RecuentoHistorial"].ToString()) == int.Parse(row["RecuentoAprobadores"].ToString()))
                    {
                        
                        if (row["NivelAprobacion"].ToString() == "3") query = string.Format("SELECT pet.IdPeticionario as IdPeticionario ,pet.Email as Email FROM peticionarios pet INNER JOIN aprobadoramec apr ON pet.IdPeticionario = apr.idaprobador WHERE apr.idamecs = {0}", row["IdAmec"]);
                        if (row["NivelAprobacion"].ToString() == "4") query = string.Format("SELECT IdPeticionario,Email FROM peticionarios WHERE IdPeticionario IN (SELECT ag.IdPeticionarioManager as Email FROM agency_user_approval_structure ag INNER JOIN aprobadoramec apr ON ag.IdPeticionario = apr.idaprobador WHERE apr.idamecs = {0})", row["IdAmec"]);
                        DataTable tempDt = Quodem.Sql.SqlServerClient.GetQuery(query);
                        foreach (DataRow tempRow in tempDt.Rows)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Email"] = tempRow["Email"];
                            dr["IdAmec"] = row["IdAmec"];
                            dr["IdPeticionario"] = tempRow["IdPeticionario"];
                            dt.Rows.Add(dr);
                        }

                    }

                }
                return dt;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int GetLogMailRecordatorio(string idamecs, int idPeticionario)
        {
            try
            {

                string query = string.Format("select count(*) from logmail_flujoamec log INNER JOIN amecs am ON log.idamecs = am.idamecs " +
                                             " where log.idamecs = '{0}'" +
                                             " and log.tipo_mail = 'MailRecordatorio' " +
                                             " and log.envio_correcto = 1 " +
                                             " and log.claim = '{1}' " +
                                             " and log.fecha_envio > am.fechaultimaactualizacion", idamecs, idPeticionario);

                return int.Parse(Quodem.Sql.SqlServerClient.GetValue(query));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int GuardaLogMail(int idamecs, string tipo_mail, string message_to, string message_subject, string message_body, string message_fileattach, bool envioCorrecto, string error, int idPeticionario)
        {
            try
            {
                int intError = envioCorrecto ? 1 : 0;
                string query = string.Empty;
                query = "INSERT INTO logmail_flujoamec (idamecs, tipo_mail, mail_to, mail_subject, mail_body, mail_fileattach, envio_correcto, error, fecha_envio,claim) " +
                        "VALUES(" + idamecs + ",'" + tipo_mail + "','" + message_to + "','" + message_subject + "','" + message_body + "','" + message_fileattach + "'," + intError + ",'" + error + "',GETDATE()," + idPeticionario + ")";
                return Quodem.Sql.SqlServerClient.ExecuteQuery(query);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public int ActualizarAprobacionJefe(int idamecs, int idPeticionario)
        {
            try
            {
                string query = $"UPDATE aprobadoramec SET aprobacionjefe = 1 WHERE idamecs = {idamecs} and idaprobador = {idPeticionario}";
                return Quodem.Sql.SqlServerClient.ExecuteQuery(query);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public List<DelegacionDto> ObtenerListaDelegaciones()
        {
            List<DelegacionDto> list = new List<DelegacionDto>();
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT (pet1.Nombre + ' ' + pet1.Apellido1 + ' ' + pet1.Apellido2) AS Usuario, ");
            sb.Append(" (pet2.Nombre + ' ' + pet2.Apellido1 + ' ' + pet2.Apellido2) AS Delegado, ");
            sb.Append(" del.fechadesde as FechaDesde,del.fechahasta as FechaHasta FROM delegaprobacion del INNER JOIN peticionarios pet1 ");
            sb.Append(" ON del.idusuariodel = pet1.IdPeticionario ");
            sb.Append(" INNER JOIN peticionarios pet2  ");
            sb.Append(" ON del.iddelegado = pet2.IdPeticionario ");
            sb.AppendFormat(" WHERE del.IdEstado = 1 AND fechahasta >= GETDATE() AND DATEDIFF(day,GETDATE(),del.fechahasta) <= {0} ",Variables.DiasAvisoDelegacion);
            DataTable dt = Quodem.Sql.SqlServerClient.GetQuery(sb.ToString());

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DelegacionDto()
                {
                    NombreUsuario = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Usuario"),
                    NombreDelegado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Delegado"),
                    FechaHasta = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaHasta"),
                    FechaDesde = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaDesde"),
                });
            }

            return list;
        }

    }
}
