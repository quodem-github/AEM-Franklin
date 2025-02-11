using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.ServiceLogic;

namespace EOS.Repositorios
{
    public class RepositorioAprobadorAMEC : IRepositorioPeticionariosManager
    {
        public int InsertAprobadorAmec(FiltroAprovadorAmec filtro)
        {
            string consulta = "INSERT INTO aprobadoramec " +
                              "(idamecs,idaprobador, idcreadopor, fechacreacion) " +
                              "VALUES(" + filtro.IdAmecs + "," + filtro.IdAprobador + "," + filtro.IdCreador + ", '" +
                              DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "')";

            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        public int BorrarAprbadorAmec(string idamec, string idaprobador)
        {
            //Los hardcodeos estan en los enums en el proyecto EOS
            string consulta = string.Format("DELETE FROM aprobadoramec WHERE idamecs = '{0}' AND idaprobador={1}", idamec,
                idaprobador);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        public int ObernerNumeroAprobadoresAmec(string idamec)
        {
            //Los hardcodeos estan en los enums en el proyecto EOS
            string consulta = string.Format("SELECT count(idaprobador) FROM aprobadoramec WHERE idamecs='{0}'", idamec);
            return int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));

        }

        public List<DAprobadoresAmec> ObernerAprobadoresAmec(string idamec, int? idpeticionario)
        {
            //Los hardcodeos estan en los enums en el proyecto EOS
            string consulta = string.Empty;
            if (!string.IsNullOrWhiteSpace(idamec))
            {
                consulta =
                    string.Format(
                        "SELECT apr.idamecs as IdAmecs,apr.idaprobador as IdAprobador,st.IdPeticionarioManager as IdPeticionarioManager, st2.IdPeticionarioManager as IdPeticionarioManagerManager FROM aprobadoramec apr left join agency_user_approval_structure st on apr.idaprobador = st.IdPeticionario left join agency_user_approval_structure st2 on st.IdPeticionarioManager = st2.IdPeticionario WHERE idamecs = '{0}' group by apr.idamecs, apr.idaprobador, st.IdPeticionarioManager, st2.IdPeticionarioManager ",
                        idamec);
            }

            if (idpeticionario != null)
            {
                consulta =
                    string.Format(
                        "SELECT '' as IdAmecs, st.IdPeticionario as IdAprobador, st.IdPeticionarioManager, st2.IdPeticionarioManager as IdPeticionarioManagerManager from agency_user_approval_structure st left join agency_user_approval_structure st2 on	st.IdPeticionarioManager = st2.IdPeticionario WHERE st.IdPeticionario={0} group by st.IdPeticionario, st.IdPeticionarioManager, st2.IdPeticionarioManager ",
                        idpeticionario.Value);
            }
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));

            List<DAprobadoresAmec> result = new List<DAprobadoresAmec>();
            foreach (DataRow row in dt.Tables[0].Rows)
            {
                DAprobadoresAmec aprobador = new DAprobadoresAmec();
                aprobador.IdAmecs = row["IdAmecs"].ToString();
                aprobador.IdAprobador = string.IsNullOrWhiteSpace(row["IdAprobador"].ToString()) ? new int?() : int.Parse(row["IdAprobador"].ToString());
                aprobador.IdPeticionarioManager = string.IsNullOrWhiteSpace(row["IdPeticionarioManager"].ToString()) ? new int?() : int.Parse(row["IdPeticionarioManager"].ToString());
                aprobador.IdPeticionarioManagerManager = string.IsNullOrWhiteSpace(row["IdPeticionarioManagerManager"].ToString()) ? new int?() : int.Parse(row["IdPeticionarioManagerManager"].ToString());

                result.Add(aprobador);
            }
            return result;
        }

        public List<int> ObernerIdsAprobadoresAmec(string idamec)
        {
            string consulta = string.Format("SELECT idaprobador FROM aprobadoramec WHERE idamecs='{0}'", idamec);
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            List<int> ids = new List<int>();
            foreach (DataRow row in dt.Tables[0].Rows)
            {
                ids.Add(int.Parse(row[0].ToString()));
            }
            return ids;
        }

        public bool ObtenerPermisoSometer(string idamec, int idPeticionario)
        {
            try
            {
                //Los hardcodeos estan en los enums en el proyecto EOS
                string consulta = string.Format("SELECT idsolicitante FROM amecs WHERE idamecs = '{0}' AND idestado NOT IN (1,3,4)", idamec);
                Int32 result = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
                string consultaDelegacion = string.Format("SELECT iddelegado FROM delegaprobacion WHERE idusuariodel = {0} AND idEstado = 1 AND fechadesde <= GETDATE() AND fechahasta > GETDATE()", result);
                string sDelResult = Quodem.Sql.SqlServerClient.GetValue(consultaDelegacion);
                Int32 consultaDelegacionResult = -1;
                try
                {
                    consultaDelegacionResult = int.Parse(sDelResult);
                }
                catch (Exception)
                {
                    consultaDelegacionResult = -1;
                }
                return result == idPeticionario || consultaDelegacionResult == idPeticionario;
            }
            catch (Exception ex)
            {
                /*Entra aqui cuando no encuentra un idsolicitante con los filtros dados*/
                return false;
            }
        }

        public bool ObtenerPermisoSometerCreador(string idamec, int idPeticionario)
        {
            try
            {
                //Los hardcodeos estan en los enums en el proyecto EOS
                string consulta = string.Format("SELECT idcreadopor FROM amecs WHERE idamecs = '{0}' AND idestado NOT IN (1,3,4)", idamec);

                Int32 result = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
                return result == idPeticionario && false;
            }
            catch (Exception ex)
            {
                /*Entra aqui cuando no encuentra un idsolicitante con los filtros dados*/
                return false;
            }
        }


        public bool ObtenerPermisoSustitutoSupJer(string idamec, int idPeticionario)
        {
            string checkSustitucion = string.Format("SELECT iddelegado FROM delegaprobacion WHERE fechadesde <= GETDATE() AND fechahasta >= GETDATE() AND IdEstado = 1" +
                                                     "" +
                                        " AND idusuariodel IN( " +
                                        " SELECT idaprobador FROM aprobadoramec where idamecs = '{0}') ", idamec);
            DataTable dt = Quodem.Sql.SqlServerClient.GetQuery(checkSustitucion);
            List<int> sustitutos = new List<int>();
            foreach (DataRow row in dt.Rows)
            {
                sustitutos.Add(Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddelegado"));
            }
            //Comprobamos si es sustituto
            if (!sustitutos.Contains(idPeticionario)) return false;

            string checkActions =
                        "SELECT COUNT(*) FROM histasocamecs hist JOIN aprobadoramec aprob " +
                        " ON hist.idamecs = aprob.idamecs AND hist.idcreadopor = aprob.idaprobador " +
                        " JOIN amecs amecs ON aprob.idamecs = amecs.idamecs " +
                        " INNER JOIN delegaprobacion del ON aprob.idaprobador = del.idusuariodel " +
                    string.Format(" WHERE del.iddelegado = {0} AND aprob.idamecs = '{1}' ", idPeticionario, idamec) +
                    string.Format(" AND (amecs.idestado = {0} OR amecs.idestado = {1}) AND amecs.idnivelaprobacion = {2}  AND hist.idestado <> {3}", 8, 38, 3, 40) +
                    string.Format(" AND hist.fechacreacion > (SELECT TOP 1 fechacreacion FROM histasocamecs WHERE idamecs = '{0}' ", idamec) +
                    string.Format(" AND idestado = {0} ORDER BY fechacreacion DESC);", 29);
            //comprobamos si el sustituto ya ha firmado
            Int64 actions = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkActions));

            string checkAprobador =
                string.Format("SELECT COUNT(*) FROM aprobadoramec where idamecs = '{0}' AND idaprobador IN " +
                              "(SELECT idusuariodel FROM delegaprobacion WHERE iddelegado =  {1})", idamec,
                    idPeticionario);

            Int64 aprobadores = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));

            return actions < aprobadores && aprobadores > 0;
        }

        public bool ObtenerPermisosSupJerAmec(string idamec, int idPeticionario)
        {

            //Comprobamos si podemos aprobar el amec mirando el flag aprobacionjefe en aprobadoramec
            //Esto es un caso especial donde el manager puede aprobar en nombre del aprobador ya que se ha retrasado en su accion
            if (CheckPermisoAprobacionJefe(idamec, idPeticionario, 1) > 0) return true;


            //Los hardcodeos estan en los enums en el proyecto EOS
            string checkActions =
                "SELECT COUNT(*) FROM histasocamecs hist JOIN aprobadoramec aprob " +
                " ON hist.idamecs = aprob.idamecs AND hist.idcreadopor = aprob.idaprobador " +
                " JOIN amecs amecs ON aprob.idamecs = amecs.idamecs" +
  string.Format(" WHERE aprob.idaprobador = {0} AND aprob.idamecs = '{1}' ", idPeticionario, idamec) +
  string.Format(" AND (amecs.idestado = {0} OR amecs.idestado = {1}) AND amecs.idnivelaprobacion = {2}  AND hist.idestado <> {3}", 8, 38, 3, 40) +
  string.Format(" AND hist.fechacreacion > (SELECT TOP 1 fechacreacion FROM histasocamecs WHERE idamecs = '{0}' ", idamec) +
  string.Format(" AND idestado = {0} ORDER BY fechacreacion DESC);", 29);
            Int64 actions = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkActions));

            string checkAprobador =
                string.Format(
                    "SELECT count(*) FROM aprobadoramec inner join histasocamecs on  histasocamecs.idamecs = aprobadoramec.idamecs and  histasocamecs.idaprobador = aprobadoramec.idaprobador inner join amecs on  amecs.idamecs = histasocamecs.idamecs and  amecs.idnivelaprobacion = histasocamecs.idnivelaprobacion and amecs.idestado = histasocamecs.idestado WHERE  histasocamecs.fechacreacion >= (SELECT TOP 1 fechacreacion FROM histasocamecs WHERE idamecs = '{0}' AND idestado = 29 ORDER BY fechacreacion DESC) AND aprobadoramec.idamecs = '{0}' AND aprobadoramec.idaprobador = {1} ",
                    idamec, idPeticionario);

            // Soy aprobador yo  o soy superior jerárquico de un aprobador?
            // Si aprobador = 0 -> Soy jefe de aprobador
            Int64 aprobador = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));

            //Si el usuario no es un aprobador (Superior jerárquico) compruebo si es un aprobador de negocio y su inferior jerárquico ya ha aprobado
            Int64 aprobadorNegocio = 0;
            Int64 aprobacionesPreviasInferioresJerarquicos = 0;
            if (aprobador == 0)
            {
                string aprobadocionesTotalesSuperiorJerarquico = string.Format(
                "   select " +
                "       COUNT(*) " +
                "   from histasocamecs hist " +
                "   inner " +
                "   join agency_user_approval_structure aprob on " +
                "   hist.idaprobador = aprob.IdPeticionario " +
                "   where " +
                "       aprob.IdPeticionarioManager = {0} and " +
                "       idamecs = '{1}' and " +
                "       idestado = {2} and " +
                "       idnivelaprobacion = {3} and " +
                "       hist.fechacreacion >= (SELECT TOP 1 fechacreacion FROM histasocamecs WHERE idamecs = '{1}'  AND idestado = {4} ORDER BY fechacreacion DESC) " + 
                "       ", idPeticionario, idamec, 8, 3, 29);

                aprobacionesPreviasInferioresJerarquicos = int.Parse(Quodem.Sql.SqlServerClient.GetValue(aprobadocionesTotalesSuperiorJerarquico));

                string checkAprobadorNegocio = string.Format(
                "   select " +
                "       COUNT(*) " +
                "   from histasocamecs hist " +
                "   inner " +
                "   join agency_user_approval_structure aprob on " +
                "   hist.idaprobador = aprob.IdPeticionario " +
                "   where " +
                "       aprob.IdPeticionarioManager = {0} and " +
                "       idamecs = '{1}' and " +
                "       idestado = {2} and " +
                "       idnivelaprobacion = {3} AND " +
                "    " +
                "       hist.fechacreacion >= (SELECT TOP 1 fechacreacion FROM histasocamecs WHERE idamecs = '{4}'  AND idestado = {5} ORDER BY fechacreacion DESC) and " +
                "       exists " +
                "       ( " +
                        "   select " +
                "               * " +
                        "   from histasocamecs hista " +
                "           where " +
                            "   hista.idamecs = hist.idamecs and " +
                "               hista.idcreadopor = hist.idaprobador and " +
                "               hista.idestado = {6} and " +
                "               hista.idnivelaprobacion = {7} AND " +
                "               hista.fechacreacion > (SELECT TOP 1 fechacreacion FROM histasocamecs WHERE idamecs = '{8}'  AND idestado = {9} ORDER BY fechacreacion DESC) AND " +
                "               not exists " +
                "               (" +
                "                  select " +
                "                       * " +
                "                  from histasocamecs histaso " +
                "                  where " +
                "                      histaso.idamecs = hista.idamecs and " +
                "                      histaso.idcreadopor = {10} and " +
                "                      histaso.idestado = {11} and " +
                "                      histaso.idnivelaprobacion = {12} and " +
                "                      histaso.fechacreacion > (SELECT TOP 1 fechacreacion FROM histasocamecs WHERE idamecs = '{13}'  AND idestado = {14} ORDER BY fechacreacion DESC) " +
                "               )" +
                "   	) ", idPeticionario, idamec, 8, 3, idamec, 29, 21, 3, idamec, 29, idPeticionario, 22, 4, idamec, 29);

                aprobadorNegocio = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobadorNegocio));
            }

            return (actions < aprobador && aprobador > 0) || (aprobadorNegocio == aprobacionesPreviasInferioresJerarquicos && aprobadorNegocio != 0) || ObtenerPermisoSustitutoSupJer(idamec, idPeticionario);

        }

        public bool HeAprobadoSupJer(string idamec, int idPeticionario)
        {
            //Los hardcodeos estan en los enums en el proyecto EOS
            string checkAprobador = " SELECT COUNT(*) FROM histasocamecs " +
                      string.Format(" WHERE idamecs = '{0}' AND idcreadopor = {1} and idestado <> {2}", idamec, idPeticionario, 40) +
                                    " AND fechacreacion > " +
                                    " (SELECT TOP 1 fechacreacion " +
                                    " FROM histasocamecs " +
                      string.Format(" WHERE idamecs = '{0}' AND idestado = {1} ORDER BY fechacreacion DESC);", idamec, 29);
            Int64 result = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));

            //Los hardcodeos estan en los enums en el proyecto EOS
            //Si el manager aprobó ya no puede aprobar ni el manager ni el aprobador(Superior jerarquico)
            string checkAprobacionJefe =
                string.Format("SELECT COUNT(*) FROM aprobadoramec WHERE idamecs = '{0}' AND idaprobador = {1} AND aprobacionjefe = {2}", idamec, idPeticionario, 2);
            Int64 resultAprJefe = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobacionJefe));

            return result > 0 && resultAprJefe == 0;
        }

        public bool TodosAprobaronSupJer(string idamec)
        {
            //Los hardcodeos estan en los enums en el proyecto EOS
            string countAprobaciones = " SELECT COUNT(*) FROM histasocamecs " +
                         string.Format(" WHERE idamecs = '{0}' ", idamec) +
                         string.Format(" AND idnivelaprobacion = {0} ", 3) +
                         string.Format(" AND( idestado <> {0} AND idestado <> {1})", 8, 38) +
                                       " AND fechacreacion > " +
                                       " (SELECT TOP 1 fechacreacion " +
                                       " FROM histasocamecs " +
                         string.Format(" WHERE idamecs = '{0}' AND idestado = {1} ORDER BY fechacreacion DESC )", idamec, 29);
            Int64 aprobaciones = int.Parse(Quodem.Sql.SqlServerClient.GetValue(countAprobaciones));

            string countAprobadores = string.Format("SELECT COUNT(*) FROM aprobadoramec apro left join peticionarios pet on apro.idaprobador = pet.IdPeticionario WHERE apro.idamecs = '{0}' and pet.director <> 1 and pet.executive <> 1;", idamec);
            Int64 aprobadores = int.Parse(Quodem.Sql.SqlServerClient.GetValue(countAprobadores));

            return aprobaciones == aprobadores;
        }

        public bool ObtenerPermisoSustitutoNegocio(string idamec, int idPeticionario)
        {
            string checkSustitucion = string.Format("SELECT iddelegado FROM delegaprobacion WHERE fechadesde <= GETDATE() AND fechahasta >= GETDATE() AND IdEstado = 1" +
                                        " AND idusuariodel IN( " +
                                        " SELECT IdPeticionarioManager FROM agency_user_approval_structure WHERE IdPeticionario IN " +
                                        " (SELECT idaprobador FROM aprobadoramec where idamecs = '{0}')) ", idamec);
            DataTable dt = Quodem.Sql.SqlServerClient.GetQuery(checkSustitucion);
            List<int> sustitutos = new List<int>();
            foreach (DataRow row in dt.Rows)
            {
                sustitutos.Add(Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddelegado"));
            }
            //Comprobamos si es sustituto
            if (!sustitutos.Contains(idPeticionario)) return false;

            //Comprobamos si estando el amec en pendiente de negocio puedo aprobar siendo el sustituto
            string checkActions = "SELECT COUNT(*) FROM histasocamecs hist " +
                                  " JOIN aprobadoramec aprob ON hist.idamecs = aprob.idamecs " +
                                  " JOIN amecs amecs ON aprob.idamecs = amecs.idamecs " +
                                  " JOIN agency_user_approval_structure stru ON aprob.idaprobador = stru.IdPeticionario " +
                                  " INNER JOIN delegaprobacion del ON stru.IdPeticionarioManager = del.idusuariodel " +
                     string.Format(" WHERE aprob.idamecs = '{0}' and del.iddelegado = {1} ", idamec, idPeticionario) +
                     string.Format(" AND hist.idcreadopor = {0} AND amecs.idestado = {1} ", idPeticionario, 10) +
                                  " AND hist.fechacreacion > " +
                                  " (SELECT TOP 1 fechacreacion " +
                                  " FROM histasocamecs " +
                     string.Format(" WHERE idamecs = '{0}' AND (idestado = {1} OR idestado = {2}) ORDER BY fechacreacion DESC  ) ;", idamec, 21, 31);
            //comprobamos si el sustituto ya ha firmado
            Int64 actions = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkActions));

            string checkAprobador = " SELECT COUNT(*) FROM aprobadoramec aprob" +
                                    " JOIN agency_user_approval_structure stru ON aprob.idaprobador = stru.IdPeticionario" +
                                    string.Format(" where aprob.idamecs = '{0}' AND stru.IdPeticionarioManager IN (SELECT idusuariodel FROM delegaprobacion WHERE iddelegado =  {1})", idamec, idPeticionario);

            Int64 aprobadores = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));

            return actions < aprobadores && aprobadores > 0;
        }

        public bool HaRealizadoAprobacion(string idamecs, int idpeticionario)
        {
            string checkaprobaciones = " SELECT " +
                                       " 	COUNT(*) " +
                                       " FROM   histasocamecs hist  " +
                                       " JOIN amecs amecs ON  " +
                                       " 	hist.idamecs = amecs.idamecs  " +
                                       " JOIN agency_user_approval_structure stru ON " +
                                       "    hist.idcreadopor = stru.idpeticionario " +
                                       " WHERE   " +
                                       string.Format("  hist.idamecs = '{0}' AND ", idamecs) +
                                       string.Format(" 	stru.idpeticionariomanager = {0} and  ", idpeticionario) +
                                       "    hist.idcreadopor = stru.idpeticionario AND " +
                                       string.Format(" 	hist.idestado in ({0}, {1}) AND  ", 21, 31) +
                                       string.Format(" 	hist.fechacreacion >= (SELECT TOP 1 fechacreacion FROM   histasocamecs WHERE  idamecs = '{0}' AND idestado = {1} ORDER  BY fechacreacion DESC); ", idamecs, 29);

            return int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkaprobaciones)) > 0;
            
        }

        public bool ObtenerPermisosNegocio(string idamec, int idPeticionario)
        {
            //No le damos permiso a los executives sobre amec que los solicitantes no sean directores
            //string checkExecutive =
            //    string.Format("SELECT COUNT(*) FROM peticionarios where executive = 1 AND IdPeticionario = {0}",idPeticionario);
            //int executive = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkExecutive));
            //if (executive > 0) return false;

            //Comprobamos si estando el amec en pendiente de negocio puedo aprobar
            string checkActions = "SELECT COUNT(*) FROM histasocamecs hist " +
                                  " JOIN aprobadoramec aprob ON hist.idamecs = aprob.idamecs " +
                                  " JOIN amecs amecs ON aprob.idamecs = amecs.idamecs " +
                                  " JOIN agency_user_approval_structure stru ON aprob.idaprobador = stru.IdPeticionario " +
                     string.Format(" WHERE aprob.idamecs = '{0}' and stru.IdPeticionarioManager = {1} ", idamec, idPeticionario) +
                     string.Format(" AND hist.idcreadopor = {0} AND amecs.idestado = {1} ", idPeticionario, 10) +
                                  " AND hist.fechacreacion > " +
                                  " (SELECT TOP 1 fechacreacion " +
                                  " FROM histasocamecs " +
                     string.Format(" WHERE idamecs = '{0}' AND (idestado = {1} OR idestado = {2}) ORDER BY fechacreacion DESC  ) ;", idamec, 21, 31);
            int aprobaciones = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkActions));

            string checkAprobador = " SELECT COUNT(*) FROM aprobadoramec aprob" +
                                    " JOIN agency_user_approval_structure stru ON aprob.idaprobador = stru.IdPeticionario" +
                                    string.Format(" where aprob.idamecs = '{0}' AND (stru.IdPeticionarioManager = {1} OR aprob.idaprobador = {1})", idamec, idPeticionario);

            int aprobador = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));
            int aprobacionesUsuario = 0;
            if (aprobador > 0)
            {
                string checkActionsUser = string.Format(
                    " select " +
                    "     COUNT(*) " +
                    " FROM histasocamecs hist " +
                    " INNER JOIN histasocamecs hist2 on " +
                    "     hist.idamecs = hist2.idamecs and " +
                    "     hist.idaprobador = hist2.idcreadopor " +
                    " where " +
                    "     hist.idamecs = '{0}' and " +
                    "     hist.idaprobador = {1} and " +
                    "    ( " +
                    "        ( " +
                    "            hist.idestado = {2} and " +
                    "            hist.idnivelaprobacion = {3} and " +
                    "            hist2.idestado = {4} and " +
                    "            hist2.idnivelaprobacion = {5} " +
                    "        ) " +
                    "        OR " +
                    "        ( " +
                    "            hist.idestado = {8} and " +
                    "            hist.idnivelaprobacion = {9} and " +
                    "            hist2.idestado = {10} and " +
                    "            hist2.idnivelaprobacion = {11} " +
                    "        ) " +
                    "    ) AND " +
                    "     hist.fechacreacion >= (SELECT TOP 1 fechacreacion FROM histasocamecs WHERE idamecs = '{6}'  AND idestado = {7} ORDER BY fechacreacion DESC) and  hist2.fechacreacion >= (SELECT TOP 1 fechacreacion FROM histasocamecs WHERE idamecs = '{6}'  AND idestado = {7} ORDER BY fechacreacion DESC) ",
                    idamec, idPeticionario, 10, 4, 22, 4, idamec, 29, 8, 3, 21, 3);

                aprobacionesUsuario = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkActionsUser));
            }


            return aprobaciones < aprobador && aprobador > 0 && aprobacionesUsuario == 0 || ObtenerPermisoSustitutoNegocio(idamec, idPeticionario);
        }

        public bool ObtenerPermisoSustitutoExecutive(string idamec, int idPeticionario)
        {
            string checkSustitucion = string.Format("SELECT iddelegado FROM delegaprobacion WHERE fechadesde <= GETDATE() AND fechahasta >= GETDATE() AND IdEstado = 1" +
                                        " AND idusuariodel IN( SELECT idaprobador FROM aprobadoramec where idamecs = '{0}') ", idamec);
            DataTable dt = Quodem.Sql.SqlServerClient.GetQuery(checkSustitucion);
            List<int> sustitutos = new List<int>();
            foreach (DataRow row in dt.Rows)
            {
                sustitutos.Add(Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddelegado"));
            }
            //Comprobamos si es sustituto
            if (!sustitutos.Contains(idPeticionario)) return false;

            string checkActions = " SELECT COUNT(*) FROM histasocamecs hist " +
                                " JOIN aprobadoramec aprob ON hist.idamecs = aprob.idamecs " +
                                " JOIN amecs amecs ON aprob.idamecs = amecs.idamecs " +
                                " INNER JOIN delegaprobacion del ON aprob.idaprobador = del.idusuariodel " +
                  string.Format(" WHERE del.iddelegado = {0} AND aprob.idamecs = '{1}' ", idPeticionario, idamec) +
                  string.Format(" AND hist.idcreadopor = {0} AND amecs.idestado = {1} AND hist.idestado <> {2}", idPeticionario, 10, 40) +
                                " AND hist.fechacreacion > " +
                                " (SELECT TOP 1 fechacreacion " +
                                " FROM histasocamecs " + string.Format(" WHERE idamecs = '{0}' AND idestado = {1} ORDER BY fechacreacion DESC  ) ;", idamec, 29);


            //comprobamos si el sustituto ya ha firmado
            Int64 actions = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkActions));

            string checkAprobador =
                string.Format("SELECT COUNT(*) FROM aprobadoramec where idamecs = '{0}' AND idaprobador IN " +
                              "(SELECT idusuariodel FROM delegaprobacion WHERE iddelegado =  {1})", idamec,
                    idPeticionario);

            Int64 aprobadores = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));

            return actions < aprobadores && aprobadores > 0;
        }

        public bool ObtenerPermisosExecutive(string idamec, int idPeticionario)
        {
            //Los hardcodeos estan en los enums en el proyecto EOS
            string checkActions = " SELECT COUNT(*) FROM histasocamecs hist " +
                                  " JOIN aprobadoramec aprob ON hist.idamecs = aprob.idamecs " +
                                  " JOIN amecs amecs ON aprob.idamecs = amecs.idamecs " +
                    string.Format(" WHERE aprob.idamecs = '{0}' and aprob.idaprobador = {1} ", idamec, idPeticionario) +
                    string.Format(" AND hist.idcreadopor = {0} AND amecs.idestado = {1} AND hist.idestado <> {2}", idPeticionario, 10, 40) +
                                  " AND hist.fechacreacion > " +
                                  " (SELECT TOP 1 fechacreacion " +
                                  " FROM histasocamecs " + string.Format(" WHERE idamecs = '{0}' AND idestado = {1} ORDER BY fechacreacion DESC  ) ;", idamec, 29);

            Int64 aprobaciones = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkActions));

            string checkAprobador =
                string.Format("SELECT COUNT(*) FROM aprobadoramec where idamecs = '{0}' AND idaprobador = {1}", idamec, idPeticionario);
            Int64 aprobador = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));

            return aprobaciones < aprobador && aprobador > 0 || ObtenerPermisoSustitutoExecutive(idamec, idPeticionario);
        }

        //La lista de estados es necesaria para saber cual ha sido la ultima accion hecha por un superior jerarquico
        public bool HeAprobadoNegocio(string idamec, int idPeticionario, List<int> idestados)
        {
            //Los hardcodeos estan en los enums en el proyecto EOS
            string checkAprobador = " SELECT COUNT(*) FROM histasocamecs " +
                                    string.Format(" WHERE idamecs = '{0}' AND idcreadopor = {1} ", idamec, idPeticionario) +
                                    string.Format(" AND idestado <> {0}", 40) +
                                    " AND fechacreacion > " +
                                    " (SELECT TOP 1 fechacreacion " +
                                    " FROM histasocamecs " +
                                    string.Format(" WHERE idamecs = '{0}' AND (", idamec) +
                                    CrearCondicionEstados(idestados) +
                                    string.Format(
                                        ") and fechacreacion >= (SELECT TOP 1 fechacreacion  FROM histasocamecs  WHERE idamecs = '{0}' AND idestado = 29 ORDER BY fechacreacion DESC) ORDER BY fechacreacion DESC );",
                                        idamec);
            Int64 result = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));
            return result > 0;
        }

        //La lista de estados es necesaria para saber cual ha sido la ultima accion hecha por un superior jerarquico
        public bool TodosAprobaronNegocio(string idamec, List<int> idestados)
        {
            //Los hardcodeos estan en los enums en el proyecto EOS
            string countAprobaciones = " SELECT COUNT(*) FROM histasocamecs " +
                         string.Format(" WHERE idamecs = '{0}' ", idamec) +
                         string.Format(" AND idnivelaprobacion = 4  AND idestado in (22, 28, 32, 44) ", 4) +
                                       " AND fechacreacion >= " +
                                       " (SELECT TOP 1 fechacreacion " +
                                       " FROM histasocamecs " +
                         string.Format(" WHERE idamecs = '{0}' AND idestado = 29 ", idamec) +
                                       " ORDER BY fechacreacion DESC )";
            Int64 aprobaciones = int.Parse(Quodem.Sql.SqlServerClient.GetValue(countAprobaciones));

            string countAprobadores = " SELECT COUNT(*) FROM histasocamecs " +
                         string.Format(" WHERE idamecs = '{0}' ", idamec) +
                         string.Format(" AND idnivelaprobacion = 4  AND idestado in (10, 11, 27, 43) ", 4) +
                                       " AND fechacreacion >= " +
                                       " (SELECT TOP 1 fechacreacion " +
                                       " FROM histasocamecs " +
                         string.Format(" WHERE idamecs = '{0}' AND idestado = 29 ", idamec) +
                                       " ORDER BY fechacreacion DESC )";
            Int64 aprobadores = int.Parse(Quodem.Sql.SqlServerClient.GetValue(countAprobadores));

            return aprobaciones == aprobadores;
        }

        public bool ObtenerPermisosMultiAprobacion(string idamec, int idPeticionario)
        {
            try
            {
                //Los hardcodeos estan en los enums en el proyecto EOS
                string checkAprobador = " SELECT stru.idPeticionarioManager FROM amecs amec" +
                                        " JOIN agency_user_approval_structure stru ON amec.idsolicitante = stru.IdPeticionario" +
                                        string.Format(" where amec.idamecs = '{0}'", idamec);
                Int32 aprobador = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));

                //Sustituto del superior del creador del amec
                string checkSustituto = string.Format("SELECT iddelegado FROM delegaprobacion WHERE fechadesde <= GETDATE() AND fechahasta >= GETDATE() AND IdEstado = 1 AND idusuariodel ={0}", aprobador);
                DataTable dt = Quodem.Sql.SqlServerClient.GetQuery(checkSustituto);
                List<int> sustitutos = new List<int>();
                foreach (DataRow row in dt.Rows)
                {
                    sustitutos.Add(Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddelegado"));
                }

                return idPeticionario == aprobador || sustitutos.Contains(idPeticionario);
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool ObtenerPermisosSupJerCreador(string idamec, int idPeticionario)
        {
            try
            {
                //Los hardcodeos estan en los enums en el proyecto EOS
                string checkAprobador = " SELECT stru.idPeticionarioManager FROM amecs amec" +
                                        " JOIN agency_user_approval_structure stru ON amec.idcreadopor = stru.IdPeticionario" +
                                        string.Format(" where amec.idamecs = '{0}'", idamec);
                Int32 aprobador = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));

                //Sustituto del superior del creador del amec
                string checkSustituto = string.Format("SELECT iddelegado FROM delegaprobacion WHERE fechadesde <= GETDATE() AND fechahasta >= GETDATE() AND IdEstado = 1 AND idusuariodel ={0}", aprobador);
                DataTable dt = Quodem.Sql.SqlServerClient.GetQuery(checkSustituto);
                List<int> sustitutos = new List<int>();
                foreach (DataRow row in dt.Rows)
                {
                    sustitutos.Add(Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddelegado"));
                }

                return idPeticionario == aprobador || sustitutos.Contains(idPeticionario);
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool ObtenerPermisosNegocioCreador(string idamec, int idPeticionario)
        {
            try
            {
                //Los hardcodeos estan en los enums en el proyecto EOS
                string checkAprobador = " SELECT idPeticionarioManager " +
                                        "FROM agency_user_approval_structure " +
                                        "WHERE IdPeticionario = " +
                                        "(SELECT stru.idPeticionarioManager " +
                                        "FROM amecs amec " +
                                        "JOIN agency_user_approval_structure stru ON amec.idcreadopor = stru.IdPeticionario " +
                                        string.Format("WHERE amec.idamecs = '{0}')", idamec);

                Int32 aprobador = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));
                string checkSustituto = string.Format("SELECT iddelegado FROM delegaprobacion WHERE fechadesde <= GETDATE() AND fechahasta >= GETDATE() AND IdEstado = 1 AND idusuariodel ={0}", aprobador);
                DataTable dt = Quodem.Sql.SqlServerClient.GetQuery(checkSustituto);
                List<int> sustitutos = new List<int>();
                foreach (DataRow row in dt.Rows)
                {
                    sustitutos.Add(Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddelegado"));
                }

                return idPeticionario == aprobador || sustitutos.Contains(idPeticionario);
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool ObtenerPermisosLegal(string idamec, int idPeticionario)
        {
            //Los hardcodeos estan en los enums en el proyecto EOS
            string checkAprobador = string.Format("select count(*) from amecs where idamecs = '{0}' and idestado = {1} and idnivelaprobacion = {2} ", idamec, 14, 6);

            Int64 aprobador = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));

            string checkIsLegal =
                string.Format("SELECT COUNT(*) FROM peticionarios where IdPeticionario = {0} AND legal = 1;",
                    idPeticionario);
            Int64 legal = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkIsLegal));

            return aprobador > 0 && legal > 0;
        }

        public bool HeAprobadoLegal(string idamec)
        {
            //Los hardcodeos estan en los enums en el proyecto EOS
            string checkAprobador = string.Format("SELECT COUNT(*) FROM histasocamecs WHERE idamecs = '{0}' ", idamec) +
                                    string.Format(" AND idestado <> {0}", 14) +
                                    string.Format(" AND idnivelaprobacion = {0} ", 6) +
                                    " AND fechacreacion > " +
                                    " (SELECT TOP 1 fechacreacion " +
                                    " FROM histasocamecs " +
                                    string.Format(" WHERE idamecs = '{0}' AND (idestado = {1} OR idestado = {2}) ORDER BY fechacreacion DESC )", idamec, 29, 11);

            Int64 aprobador = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));
            return aprobador > 0;
        }

        public bool ObtenerPermisosMedico(string idamec, int idPeticionario)
        {
            //Los hardcodeos estan en los enums en el proyecto EOS
            string checkAprobador = string.Format("select count(*) from amecs where idamecs = '{0}' and idestado = {1} and idnivelaprobacion = {2} ", idamec, 6, 2);

            Int64 aprobador = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));

            string checkIsMedico =
                string.Format("SELECT COUNT(*) FROM peticionarios where IdPeticionario = {0} AND medico = 1;",
                    idPeticionario);
            Int64 medico = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkIsMedico));

            return aprobador > 0 && medico > 0;
        }

        public bool HeAprobadoMedico(string idamec)
        {
            //Los hardcodeos estan en los enums en el proyecto EOS
            string checkAprobador = string.Format("SELECT COUNT(*) FROM histasocamecs WHERE idamecs = '{0}' ", idamec) +
                                    string.Format(" AND idestado <> {0}", 6) +
                                    string.Format(" AND idnivelaprobacion = {0} ", 2) +
                                    " AND fechacreacion > " +
                                    " (SELECT TOP 1 fechacreacion " +
                                    " FROM histasocamecs " +
                                    string.Format(" WHERE idamecs = '{0}' AND (idestado = {1} OR idestado = {2}) ORDER BY fechacreacion DESC )", idamec, 29, 11);
            Int64 aprobador = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobador));
            return aprobador > 0;
        }

        private string CrearCondicionEstados(List<int> idestados)
        {
            string filtro = "";
            foreach (var idestado in idestados)
            {
                filtro += idestados.Count > idestados.IndexOf(idestado) + 1
                    ? "idestado = " + idestado + " OR "
                    : "idestado = " + idestado;
            }
            return filtro;
        }

        public ICollection<Int32> DestinatariosAprobadores(string idamec)
        {
            string query = "select idPeticionario from peticionarios where IdPeticionario in " +
                           string.Format("(SELECT idaprobador FROM aprobadoramec where idamecs = '{0}');", idamec);
            return Helper.ICollectionIntConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query));
        }

        public ICollection<Int32> DestinatariosAprobadoresNegocio(string idamec)
        {
            string query = " select idaprobador from histasocamecs " +
                           "   WHERE " +
                           string.Format("   idamecs = '{0}'  AND ", idamec) +
                           "   idnivelaprobacion = 4  AND " +
                           "   idestado = 10 AND " +
                           string.Format("   fechacreacion >= (SELECT TOP 1 fechacreacion FROM histasocamecs WHERE idamecs = '{0}'  AND idestado = 29  ORDER BY fechacreacion DESC ) ", idamec);
            return Helper.ICollectionIntConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query));
        }

        public ICollection<Int32> DestinatariosAprobadoresNegocioSinExecutive(string idamec)
        {
            string query = "SELECT idPeticionario from peticionarios where executive != 1 AND altocargo != 1 AND IdPeticionario IN " +
                           "(SELECT IdPeticionarioManager FROM agency_user_approval_structure where IdPeticionario in" +
                           string.Format(" (SELECT idaprobador FROM aprobadoramec where idamecs = '{0}'));", idamec);
            return Helper.ICollectionIntConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query));
        }

        public Int32 DestinatariosSupJer(int idPeticionario)
        {
            string query = "SELECT idPeticionario from peticionarios where IdPeticionario IN " +
                           string.Format("(SELECT IdPeticionarioManager FROM agency_user_approval_structure where IdPeticionario = {0});", idPeticionario);
            return int.Parse(Quodem.Sql.SqlServerClient.GetValue(query));
        }

        public Int32 DestinatariosNegocio(int idPeticionario)
        {
            string query = "SELECT idPeticionario from peticionarios where IdPeticionario " +
                           "in (SELECT IdPeticionarioManager FROM agency_user_approval_structure where IdPeticionario in " +
                           string.Format("(SELECT IdPeticionarioManager FROM agency_user_approval_structure where IdPeticionario = {0}));", idPeticionario);
            return int.Parse(Quodem.Sql.SqlServerClient.GetValue(query));
        }

        /*Los medicos tienen un correo en particular configurado en el web config*/
        public ICollection<Int32> DestinatariosMedicos()
        {
            /*string query = "SELECT idPeticionario from peticionarios WHERE medico = 1";
            return Helper.ICollectionIntConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query));*/
            return new List<int>();
        }
        /*Los de legal tienen un correo en particular configurado en el web config*/
        public ICollection<Int32> DestinatariosLegal()
        {
            /*string query = "SELECT idPeticionario from peticionarios WHERE legal = 1";
            return Helper.ICollectionIntConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query));*/
            return new List<int>();
        }

        public bool ObtenerPermisoVerAmec(string idamec, int idPeticionario)
        {
            /* AQUÍ ALGUIEN SE HA FUNADO ALGO
            string query = " (SELECT idPeticionario from peticionarios where medico =1) " +
                           " UNION " +
                           " (SELECT idPeticionario from peticionarios where legal =1) " +
                           " UNION " +
                           string.Format(" (SELECT idsolicitante from amecs where idamecs = {0}) ", idamec) +
                           " UNION " +
                            string.Format(" (SELECT IdPeticionario from peticionarios where IdPeticionario = {0} and administrador = 1) ", idPeticionario) +
                           " UNION " +
                           string.Format(" (SELECT idcreadopor from amecs where idamecs = {0}) ", idamec) +
                           " UNION " +
                           " (SELECT IdPeticionarioManager FROM agency_user_approval_structure where IdPeticionario = " +
                           string.Format(" (SELECT idcreadopor from amecs where idamecs = {0})) ", idamec) +
                           " UNION " +
                           " (SELECT IdPeticionarioManager FROM agency_user_approval_structure where IdPeticionario = " +
                           " (SELECT IdPeticionarioManager FROM agency_user_approval_structure where IdPeticionario = " +
                           string.Format(" (SELECT idcreadopor from amecs where idamecs = {0}))) ", idamec) +
                           " UNION " +
                           string.Format(" (SELECT idaprobador from aprobadoramec WHERE idamecs = {0}) ", idamec) +
                           " UNION " +
                           " (SELECT IdPeticionarioManager FROM agency_user_approval_structure where IdPeticionario in " +
                           string.Format(" (SELECT idaprobador from aprobadoramec WHERE idamecs = {0})) ", idamec) +
                           "UNION " +
                           " SELECT iddelegado FROM delegaprobacion WHERE fechadesde <= GETDATE() AND fechahasta >= GETDATE() AND IdEstado = 1" +
                           " AND idusuariodel IN (" +
                           string.Format(" SELECT idaprobador FROM aprobadoramec where idamecs = {0})", idamec) +
                           "UNION " +
                           " SELECT iddelegado FROM delegaprobacion WHERE fechadesde <= GETDATE() AND fechahasta >= GETDATE() AND IdEstado = 1" +
                           " AND idusuariodel IN ( SELECT IdPeticionarioManager FROM agency_user_approval_structure where IdPeticionario in" +
                           string.Format(" (SELECT idaprobador FROM aprobadoramec where idamecs = {0}))", idamec);

            */
            string query = string.Format("EXEC sp_obtener_amecs_puedo_ver @idPeticionario = {0}, @isCreateMode = 0;", idPeticionario);
            List<string> list = Helper.IListStringConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query)).ToList();
            return list.Contains(idamec);
        }

        public bool SonTodosAprobadoresDirectores(string idamec)
        {
            string queryAprobadores = string.Format("(SELECT count(*) FROM aprobadoramec where idamecs = '{0}');", idamec);

            string query = "select count(*) from peticionarios where director = 1 and IdPeticionario in " +
                           string.Format("(SELECT idaprobador FROM aprobadoramec where idamecs = '{0}');", idamec);
            return int.Parse(Quodem.Sql.SqlServerClient.GetValue(query)) == int.Parse(Quodem.Sql.SqlServerClient.GetValue(queryAprobadores));

        }

        public void ReestablecerAprobadores(string idamec)
        {
            string query = string.Format("UPDATE aprobadoramec SET aprobacionjefe = 0 WHERE idamecs ='{0}'", idamec);
            Quodem.Sql.SqlServerClient.ExecuteQuery(query);
        }

        public void ActualizarAprobacionJefe(string idamec, int idPeticionario, int idAprobacionJefe)
        {
            string query =
                string.Format("UPDATE aprobadoramec SET aprobacionjefe = {0} WHERE idamecs = '{1}' AND", idAprobacionJefe, idamec) +
                string.Format(" idaprobador IN (SELECT idPeticionario FROM agency_user_approval_structure WHERE IdPeticionarioManager = {0})", idPeticionario);

            Quodem.Sql.SqlServerClient.ExecuteQuery(query);
        }

        public int CheckPermisoAprobacionJefe(string idamec, int idPeticionario, int idAprobacionJefe)
        {
            //Comprobamos si podemos aprobar el amec mirando el flag aprobacionjefe en aprobadoramec
            //Esto es un caso especial donde el manager puede aprobar en nombre del aprobador ya que se ha retrasado en su accion
            string checkAprobacionJefe = "SELECT COUNT(*) FROM aprobadoramec apr INNER JOIN agency_user_approval_structure ag ON apr.idaprobador = ag.IdPeticionario " +
                           string.Format(" WHERE ag.IdPeticionarioManager = {0} AND apr.idamecs = '{1}' AND apr.aprobacionjefe = {2}", idPeticionario, idamec, idAprobacionJefe);
            return int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobacionJefe));
        }

        public int DeCuantosSoyManager(string idamec, int idPeticionario)
        {
            string query = "SELECT COUNT(*) FROM aprobadoramec apr INNER JOIN agency_user_approval_structure ag ON apr.idaprobador = ag.IdPeticionario " +
                           string.Format(" WHERE ag.IdPeticionarioManager = {0} AND apr.idamecs = '{1}' ", idPeticionario, idamec);
            return int.Parse(Quodem.Sql.SqlServerClient.GetValue(query));
        }

        public List<int> ObtenerAmecsDeLosQueSoyAprobador(int idPeticionario)
        {
            //Los hardcodeos estan en los enums en el proyecto EOS//Los hardcodeos estan en los enums en el proyecto EOS
            string checkSupJer = string.Format("SELECT am.idamecs,ap.idaprobador from amecs am " +
                                               "INNER JOIN aprobadoramec ap ON am.idamecs = ap.idamecs " +
                                               "where am.idestado = 8 AND ap.idaprobador = {0} ", idPeticionario);
            string checkNegocio = string.Format("SELECT am.idamecs,ap.idaprobador" +
                                                " FROM amecs am INNER JOIN aprobadoramec ap ON am.idamecs = ap.idamecs " +
                                                "  WHERE am.idestado = 10 AND ap.idaprobador IN" +
                                                "(SELECT idPeticionario FROM agency_user_approval_structure WHERE IdPeticionarioManager = {0})", idPeticionario);
            List<Int32> listSupJer = Helper.IListIntConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(checkSupJer)).ToList();
            List<Int32> listNegocio = Helper.IListIntConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(checkNegocio)).ToList();
            listSupJer.AddRange(listNegocio);
            return listSupJer;
        }

        public List<string> ObtenerCorreosTodosParticipantes(string idamec)
        {
            string query =
                string.Format(
                    " SELECT distinct email FROM peticionarios WHERE medico = 0 and legal = 0 AND idPeticionario IN (SELECT idcreadopor from aprobadoramec where idamecs = '{0}')",
                    idamec);
            return Helper.IListStringConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query)).ToList();
        }
    }
}
