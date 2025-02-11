using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Entidades.Mapeadores;
using EOS.Repositorios;
using EOS.Logica;
using System.Data;

namespace EOS.Logica
{
   public class LogicaAmecInfo : ILogicaAmecInfo
    {
       private IRepositorioAmecInfo MiRepositorioAmecInfo { get; set; }

       public LogicaAmecInfo()
        {
            MiRepositorioAmecInfo = new RepositorioAmecInfo();
        }

       public DataSet ObtenerInformeFCPA(string idamec, string nombrePrograma, string estado, string fechaInicio, string fechaFin, string unidad, string area,
string region, string distrito, string year, string mes, string iddistrict, string idsaleforce, string iddepartament)
       {
           return MiRepositorioAmecInfo.ObtenerInformeFCPA(idamec, nombrePrograma, estado, fechaInicio, fechaFin, unidad,
               area, region, distrito, year, mes,iddistrict,idsaleforce,iddepartament);
       }

       public int guardarEnInformeFCPA(string idamecs, string tipoactividad, string descripcion, DateTime? fechacomienzo,
           DateTime? fechafinalizacion, string idestado, string usuario, string cargo, string idunidad, string idarea, string idregion,
           string iddistrito, string tiporiesgo, int idtiporiesgo, string nombrePax, string apellido1Pax, string msdid, string tiporeservacolectivo, string idexpediente,int? iddepartament,int?iddistrict,int?idsaleforce, int? idposition)
       {
           return MiRepositorioAmecInfo.guardarEnInformeFCPA(idamecs, tipoactividad, descripcion, fechacomienzo, fechafinalizacion,
                idestado, usuario, cargo, idunidad, idarea, idregion, iddistrito, tiporiesgo, idtiporiesgo, nombrePax, apellido1Pax, msdid, tiporeservacolectivo, idexpediente,iddepartament,iddistrict,idsaleforce, idposition);
       }

       public IList<string> ObtenerNombreUnidadOrganizativa(string idunidad, string idarea, string idregion, string iddistrito)
       {
           return MiRepositorioAmecInfo.ObtenerNombreUnidadOrganizativa(idunidad, idarea, idregion, iddistrito);
       }
       public int ObtenerNumPendienteSometer(string idpeticionario)
       {
           return MiRepositorioAmecInfo.ObtenerNumPendienteSometer(idpeticionario);
       }

       public int GuardaDocumentacion(DDocumentacionAmec DocAmec)
       {
           return MiRepositorioAmecInfo.GuardaDocumentacion(DocAmec);
       }

       public int EliminarDocumentacion(int iddocumentacion)
       {
           return MiRepositorioAmecInfo.EliminarDocumentacion(iddocumentacion);
       }

       public bool EstaGuardadoAmec(string idamec)
       {
           return MiRepositorioAmecInfo.EstaGuardadoAmec(idamec);
       }

        public int CambiarEstadoAmec(string idestado, string idamecs, string idpeticionario, string idaprobador)
        {
            return MiRepositorioAmecInfo.CambiarEstadoAmec(idestado, idamecs, idpeticionario, idaprobador);
        }

        public int CambiarEstadoAmec(string idestado, string idamecs, int nivelaprobacion = -1)
        {
            return MiRepositorioAmecInfo.CambiarEstadoAmec(idestado, idamecs, nivelaprobacion);
        }

       public int InsertarHistorialAmec(int idestado, string idamecs, DDatosPersonalesUsuario peticionario,  int nivelaprobacion, int? idAprobador)
       {
            return MiRepositorioAmecInfo.InsertarHistorialAmec(idestado,idamecs, peticionario, nivelaprobacion, idAprobador);
       }

       public int ObtenerNumeroDocumentacionAMEC(string idamec)
       {
           return MiRepositorioAmecInfo.ObtenerNumeroDocumentacionAMEC(idamec);
       }

       public DataSet ObtenerDocumentacionAdicionalAmec(string idamec)
       {
           return MiRepositorioAmecInfo.ObtenerDocumentacionAdicionalAmec(idamec);
       }

       public ICollection<DDocumentacionAmec> ObtenerDocumentacionAMEC(string idamec, string sortParameter, int startRowIndex, int maximumRows)
       {
           return MiRepositorioAmecInfo.ObtenerDocumentacionAMEC(idamec, sortParameter, startRowIndex, maximumRows);
       }

        public bool TieneExpedientesAsociados(int idamec)
        {
            return MiRepositorioAmecInfo.TieneExpedientesAsociados(idamec);
        }

        public bool TieneExpedientesAsociadosPorIdAmecs(string idamecs)
        {
            return MiRepositorioAmecInfo.TieneExpedientesAsociadosPorIdAmecs(idamecs);
        }

        public DataSet ObtenerCategoriasDocumento()
       {
           return MiRepositorioAmecInfo.ObtenerCategoriasDocumento();
       }

       public bool EsGestorArchivo(int nIdPeticionario)
       {
           return MiRepositorioAmecInfo.EsGestorArchivo(nIdPeticionario);
       }

       public DAmecInfo ObtenerProgramaAMEC(string idamec, string sortParameter, int startRowIndex, int maximumRows)
       {
           return MiRepositorioAmecInfo.ObtenerProgramaAMEC(idamec, sortParameter, startRowIndex, maximumRows);
       }
       public int ObtenerNumeroProgramaAMEC(string idamec)
       {
           return MiRepositorioAmecInfo.ObtenerNumeroProgramaAMEC(idamec);
       }



       public bool ComprobarSiSeEnvioAFarma(string idamec)
       {
           return MiRepositorioAmecInfo.ComprobarSiSeEnvioAFarma(idamec);
       }

       public bool ComprobarSiSeEnvioACasosClinicos(string idamec)
       {
           return MiRepositorioAmecInfo.ComprobarSiSeEnvioACasosClinicos(idamec);
       }

       public ICollection<DEstadoAmec> ObtenerEstadosAmec()
       {
           return MiRepositorioAmecInfo.ObtenerEstadosAmec();
       }

       public uint ObtenerEstadoAmec(string idamecs)
       {
           return MiRepositorioAmecInfo.ObtenerEstadoAmec(idamecs);
       }
       public int HayAmecRelacionExpediente(string idamec)
       {
           return MiRepositorioAmecInfo.HayAmecRelacionExpediente(idamec);
       }

       public int GuardaLogMail(string idamecs, string tipo_mail, string message_to, string message_subject, string message_body, string message_fileattach, bool envioCorrecto, string error)
       {
           return MiRepositorioAmecInfo.GuardaLogMail(idamecs, tipo_mail, message_to, message_subject, message_body, message_fileattach, envioCorrecto, error);
       }


       public string ObtenerFiltroRolAmec(DVPeticionariosRoles datosRoles)
       {
           string sFiltroRolAmec = null;
           if (datosRoles == null) return sFiltroRolAmec;

           bool bAprobador = (datosRoles.aprobador.HasValue) ? datosRoles.aprobador.Value : false;
           bool marketingDelegado = false;
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
               if (datosRoles.marketing.HasValue && !marketingDelegado)
               {
                   if (datosRoles.marketing.Value)
                   {
                       // En caso de que el usuario tenga para su ID en la tabla iw_peticionarios el campo marketing a TRUE, solo verá los AMEC cuyo distrito y área coincidan con los del peticionario, por tanto solo filtraremos por estos campos.

                       // OJO: Puede ocurrir que tenga el campo marketing a true, pero no tenga configurado area o distrito, por eso hacemos estas validaciones:
                       string sWhere = "";
                       if (datosRoles.Idarea.HasValue)
                           //sWhere = string.Format(" WHERE ( IdArea = {0} or Idarea1 = {0} or Idarea2 = {0} or Idarea3 = {0} )", datosRoles.Idarea.ToString());
                           sWhere = string.Format(" WHERE ( unidams.IdArea = {0} )", datosRoles.Idarea.ToString());
                       if (datosRoles.iddistrito.HasValue)
                           sWhere += (!string.IsNullOrEmpty(sWhere) ? " AND " : " WHERE ") + " unidams.iddistrito = " + datosRoles.iddistrito.ToString();

                       sFiltroRolAmec = string.Format(" inner join (select distinct (idamecs) from unidorganizamec unidams {0}) rol on ams.idamecs = rol.idamecs ", sWhere);
                       //sFiltroRol = string.Format(" inner join (select distinct (idamec), (idxpediente) from expediente {0}) rol on {1}.IDEXPEDIENTE = rol.idxpediente ", sWhere, sDBName);

                       //Jose: 22-03-2011 si entra en alguna de las condiciones, se debe saltar el siguiente filtro
                       marketingDelegado = true;
                   }
               }
               if (datosRoles.delegado.HasValue && !marketingDelegado)
               {
                   if (datosRoles.delegado.Value)
                   {
                       if (string.IsNullOrEmpty(sFiltroRolAmec))
                       {
                           StringBuilder db = new StringBuilder();

                           // Si el usuario tiene el campo delegado a true, solo verá sus datos, es decir, los que ha creado el.
                           db.Append(" WHERE ");
                           db.AppendFormat("(ams.idsolicitante = {0} or ams.idcreadopor = {0})", datosRoles.IdPeticionario);
                           //Jose: 22-03-2011 si entra en alguna de las condiciones, se debe saltar el siguiente filtro
                           marketingDelegado = true;
                           sFiltroRolAmec = db.ToString();
                       }
                   }
               }
               //if (datosRoles.IdProductoempresa.HasValue && string.IsNullOrEmpty(sFiltroRolAmec) && !marketingDelegado)TODO
               //{
               //    // Si un usuario es jefe de producto. Es porque está dado de alta en la tabla iw_jefes_productoempresa, el modo de filtrar los expedientes y los AMEC será solo filtrando por aquellos expedientes o por aquellos amec en los que se incluye el IdProductoempresa del jefe de producto.
               //    sFiltroRol = string.Format(" inner join (select distinct (idamec) from cv_filtro_producto_amec where IdProductoempresa = {0} ) rol on {1}.idamec = rol.idamec ", datosRoles.IdProductoempresa, sDBName);
               //    //Jose: 22-03-2011 si entra en alguna de las condiciones, se debe saltar el siguiente filtro
               //    marketingDelegado = true;
               //}
           }
           if (string.IsNullOrEmpty(sFiltroRolAmec))
           {
               // Si el usuario logueado no cumple ninguna de las indicaciones anteriores, filtraremos por la Unidad, área, región y distrito del usuario logueado y en este orden.Si alguno de estos campos, en la tabla de iw_peticionarios está en blanco, significará que no es necesario filtrar por ese campo pues tiene acceso a todo ese nivel.
               // unidad, area, región y distrito

               try
               {
                   StringBuilder db = new StringBuilder();
                   StringBuilder dbcreadopor = new StringBuilder();
                   if ((datosRoles.administrador.HasValue && datosRoles.administrador.Value) || datosRoles.IdCargo == 554 || datosRoles.IdCargo == 555 || datosRoles.IdCargo == 547 || datosRoles.IdCargo == 534)
                   {
                       return sFiltroRolAmec;
                   }
                   else
                   {

                       //Código que te permite ver los amecs que puede ver aquel usuario: ProcedureSergio
                       //EL Usuario podrá ver, todos los amecs que sea el solicitante y Creador y todos los amecs que esten en su Unidad, región, area, distrito

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
                       if (db.Length > 0) db.Append(")");
                       //if (db.Length > 0) db.Append(") OR "); else db.Append(" WHERE ");
                       //db.AppendFormat(" IdPeticionario = {0} ", datosRoles.IdPeticionario);
                       //if (datosRoles.idunidad.HasValue)
                       //{
                       //    db.AppendFormat("(select distinct (idamecs) as idamecs from unidorganizamec unidams where unidams.idunidad = {0} and unidams.Idarea= null and unidams.idregion=null and unidams.iddistrito = null)", datosRoles.idunidad);
                       //}
                       //if (datosRoles.Idarea.HasValue)
                       //{
                       //    if (db.Length > 0) db.Append(" Union ");
                       //    db.AppendFormat("(select distinct (idamecs) as idamecs from unidorganizamec unidams where unidams.idunidad = {0} and unidams.Idarea= {1} and unidams.idregion=null and unidams.iddistrito = null)", datosRoles.idunidad, datosRoles.Idarea);
                       //}

                       //if (datosRoles.idregion.HasValue)
                       //{
                       //    if (db.Length > 0) db.Append(" Union ");
                       //    db.AppendFormat("(select distinct (idamecs) as idamecs from unidorganizamec unidams where unidams.idunidad = {0} and unidams.Idarea= {1} and unidams.idregion={2} and unidams.iddistrito = null)", datosRoles.idunidad, datosRoles.Idarea, datosRoles.idregion);
                       //}
                       //if (datosRoles.iddistrito.HasValue)
                       //{
                       //    if (db.Length > 0) db.Append(" Union ");
                       //    db.AppendFormat("(select distinct (idamecs) as idamecs from unidorganizamec unidams where unidams.idunidad = {0} and unidams.Idarea= {1} and unidams.idregion={2} and unidams.iddistrito = {3})", datosRoles.idunidad, datosRoles.Idarea, datosRoles.idregion, datosRoles.iddistrito);
                       //}

                       //db.AppendFormat("(select distinct (idamecs) as idamecs from unidorganizamec unidams where unidams.idunidad = {0} and unidams.Idarea= {1} and unidams.idregion={2} and unidams.iddistrito = {3})", datosRoles.idunidad, datosRoles.Idarea, datosRoles.idregion, datosRoles.iddistrito);
                       
                       //dbcreadopor.AppendFormat(" inner join amecs amse On amse.idcreadopor = {0} Or amse.idsolicitante = {0}", datosRoles.IdPeticionario);
             
                    }
                   //if (db.Length > 0)
                   //{
                   //    sFiltroRolAmec = string.Format(" inner join  ({0}) rol on ams.idamecs = rol.idamecs ", db.ToString());
                   //}
               }
               catch
               {
               }
           }
           return sFiltroRolAmec;

       }


       public ICollection<ListadoAmecs> ObtenerListadoAMECs(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles, out int count)
       {
           ICollection<ListadoAmecs> listaAmecs = MiRepositorioAmecInfo.ObtenerListadoAMECsProcedure(filtro, datosRoles, out count);
           return listaAmecs;
       }

       public DataSet BUDdelaUnidad(string IdAMEC)
       {
           return MiRepositorioAmecInfo.BUDdelaUnidad(IdAMEC);
       }

       public List<ListadoAmecs> ObtenerNumeroListadoAMECsProcedureAux(FiltroListadoAMECs filtro,DVPeticionariosRoles datosRoles)
       {
            return MiRepositorioAmecInfo.ObtenerNumeroListadoAMECsProcedureAux(filtro, datosRoles);
        }

       public int ObtenerNumeroListadoAMECs(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles)
       {
           //filtro.Roles = ObtenerFiltroRolAmecCliente(datosRoles);
           return MiRepositorioAmecInfo.ObtenerNumeroListadoAMECsProcedure(filtro, datosRoles);
       }

       public string DameSiguienteIdAmecs()
       {
           return MiRepositorioAmecInfo.DameSiguienteIdAmecs();
       }

       public ICollection<DAmecInfo> ObtenerAMECsInfo(FiltroAmecInfo filtroaMEC, DVPeticionariosRoles datosRoles)
       {

           return MiRepositorioAmecInfo.ObtenerAMECsInfo(filtroaMEC, datosRoles);
       }

       public long ObtenerNumeroAMECsInfo(FiltroAmecInfo filtroaMEC, DVPeticionariosRoles datosRoles)
       {

           return MiRepositorioAmecInfo.ObtenerNumeroAMECsInfo(filtroaMEC, datosRoles);
       }

       public string ObtenerFiltroRolAmecCliente(DVPeticionariosRoles datosRoles)
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

               if (db.Length > 0 || dbcreadopor.Length > 0)
               {
                   sFiltroRolAmec = string.Format(" inner join ({0}) union ({1})) rol on ams.idamecs = rol.idamecs", db.ToString(), dbcreadopor.ToString());
               }
               return sFiltroRolAmec;
           }
           return sFiltroRolAmec;
       }

       public DAmecInfo NuevoAMEC(DAmecInfo amec)
       {
           DAmecInfo nuevoamec;
                //amec.idamec = MiRepositorioAmecInfo.ObtenerSiguienteIdAMEC();
                // 25/1/2011: Comentamos esta llamada, ya que ahora nos "fiamos" del nombre que ponga el usuario para el AMEC
                //amec.amec = MiRepositorioExpedientes.ObtenerSiguienteNombreAMEC(amec.amec.Substring(0, 2));

                nuevoamec = MiRepositorioAmecInfo.NuevoAmec(amec);                  

                return nuevoamec;
       }

        public DAmecInfo NuevoAMECNewCo(DAmecInfo amec)
        {
            DAmecInfo nuevoamec;            
            nuevoamec = MiRepositorioAmecInfo.NuevoAmecNewCo(amec);
            return nuevoamec;
        }

        public DAmecInfo ClonarAMEC(DAmecInfo amec, string idAmecOriginal)
        {
            DAmecInfo amecClonado;
            amecClonado = MiRepositorioAmecInfo.ClonarAmec(amec, idAmecOriginal);
            return amecClonado;
        }

        public DAmecInfo CargarTodosValoresAmec(string idamec)
       {
           return MiRepositorioAmecInfo.CargarTodosValoresAmec(idamec);
       }

       public DataSet CargarTodosValoresAmecExcel(string idamec)
       {
           return MiRepositorioAmecInfo.CargarTodosValoresAmecExcel(idamec);
       }

       public DataSet MailsAEnviarCuandoAprobado(string IdAMEC)
       {
           return MiRepositorioAmecInfo.MailsAEnviarCuandoAprobado(IdAMEC);
       }

       public DAmecInfo ModificarAMEC(DAmecInfo amec, out bool cambioAgencia, bool updateFechaComienzoFinalizacion)
       {
           DAmecInfo amecModificado;
           //amec.idamec = MiRepositorioAmecInfo.ObtenerSiguienteIdAMEC();
           // 25/1/2011: Comentamos esta llamada, ya que ahora nos "fiamos" del nombre que ponga el usuario para el AMEC
           //amec.amec = MiRepositorioExpedientes.ObtenerSiguienteNombreAMEC(amec.amec.Substring(0, 2));

           amecModificado = MiRepositorioAmecInfo.ModificarAMEC(amec, out cambioAgencia, updateFechaComienzoFinalizacion);

           return amecModificado;
       }


       public int CrearRelacionAmecCongreso(int idcongres, string idamecs, int idpeticionario)
       {
           return MiRepositorioAmecInfo.CrearRelacionAmecCongreso(idcongres, idamecs, idpeticionario);
       }

       public int EliminarRelacionAmecCongreso(int idameccongreso)
       {
           return MiRepositorioAmecInfo.EliminarRelacionAmecCongreso(idameccongreso);
       }

       public ICollection<DCongresos> CargarEventoActividadesRelacionadasAmec(string idamec)
       {
           return MiRepositorioAmecInfo.CargarEventoActividadesRelacionadasAmec(idamec);
       }


       public int EventoYaEstaAsignadoAmec(string idamec, int idcongreso)
       {
           return MiRepositorioAmecInfo.EventoYaEstaAsignadoAmec(idamec, idcongreso);

       }

       public int IdAmecAsignadoCongreso(string idamec, int idcongreso)
       {
           return MiRepositorioAmecInfo.IdAmecAsignadoCongreso(idamec, idcongreso);

       }
       public ICollection<DCongresos> ObtenerEventosNoRelacionadosAMEC(FiltroAMECCongreso filtroAMECCongreso)
       {
           return MiRepositorioAmecInfo.ObtenerEventosNoRelacionadosAMEC(filtroAMECCongreso);
       }

       public long ObtenerNumeroEventosNoRelacionadosAMEC(FiltroAMECCongreso filtroAMECCongreso)
       {
           return MiRepositorioAmecInfo.ObtenerNumeroEventosNoRelacionadosAMEC(filtroAMECCongreso);
       }


       public DataSet DataSetObtenerActividadesAsigAmec(string IdAMEC, string sortParameter, int startRowIndex, int maximumRows)
       {
           return MiRepositorioAmecInfo.DataSetObtenerActividadesAsigAmec(IdAMEC, sortParameter, startRowIndex, maximumRows);
       }

       public ICollection<DVCongresoAmec> ObtenerActividadesAsigAmec(string IdAMEC, string sortParameter, int startRowIndex, int maximumRows)
       {
           return MiRepositorioAmecInfo.ObtenerActividadesAsigAmec(IdAMEC, sortParameter, startRowIndex, maximumRows);
       }

       public long ObtenerNumeroActividadesAsigAmec(string IdAMEC)
       {
           return MiRepositorioAmecInfo.ObtenerNumeroActividadesAsigAmec(IdAMEC);
       }


       public string ObtenerNombreEstado(UInt32 idestado)
       {
           return MiRepositorioAmecInfo.ObtenerNombreEstado(idestado);
       }

       public string ObtenerNombreTipoRiesgo(string idtiporiesgo)
       {
           return MiRepositorioAmecInfo.ObtenerNombreTipoRiesgo(idtiporiesgo);
       }

       public string ObtenerNombreTipoActividad(string idtipoactividad)
       {
           return MiRepositorioAmecInfo.ObtenerNombreTipoActividad(idtipoactividad);
       }

       public int GuardarEstadoAHistorialAMEC(string idamec, int idestado, int idcreadopor, DAmecInfo miAmec)
       {
           return MiRepositorioAmecInfo.GuardarEstadoAHistorialAMEC(idamec, idestado, idcreadopor, miAmec);
       }


       public ICollection<DHistEstadosAMEC> ObtenerHistorialEstadosAMEC(string idamec, string sortParameter, int startRowIndex, int maximumRows)
       {
           return MiRepositorioAmecInfo.ObtenerHistorialEstadosAMEC(idamec, sortParameter, startRowIndex, maximumRows);
       }

       public long ObtenerNumeroHistorialEstadosAMEC(string idamec)
       {
           long numeroHistEstadoAMEC = MiRepositorioAmecInfo.ObtenerNumeroHistorialEstadosAMEC(idamec);
           return numeroHistEstadoAMEC;
       }

       public ICollection<String> ObtenerCorreoParaEnviar(string idamecs, string connectionString)
       {
           return MiRepositorioAmecInfo.ObtenerCorreoParaEnviar(idamecs, connectionString);
       }

       public DVCongresoAmec ObtenerCongreso(int idcongreso)
       {
           return MiRepositorioAmecInfo.ObtenerCongreso(idcongreso);
       }
        public int BorrarHistorialAmec(int idestado, string idamecs, int idPeticionario, int nivelaprobacion)
        {
            return MiRepositorioAmecInfo.BorrarHistorialAmec(idestado, idamecs, idPeticionario, nivelaprobacion);
        }

    }
}
