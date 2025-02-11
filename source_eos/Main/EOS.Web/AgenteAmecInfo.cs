using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Logica;
using System.Net.Mail;

using System.Data;
using EOS.Web.BLL.GestorPermisos.Permisos;


namespace EOS.Web
{
    public class AgenteAmecInfo : AgenteBase
    {
        private int CountObtenerListadoAmecs { get; set; }
        private DDatosPersonalesUsuario miUsuarioAmec = null;
        private DVPeticionariosRoles miUsuarioRoles = null;
        private DCabeceraActividad miDatosCongreso = null;

        private AgenteUsuarios agenteUsu = new AgenteUsuarios();
        private AgenteExpedientes agenteExp = new AgenteExpedientes();
        private AgenteMaestros agenteMaestro = new AgenteMaestros();
        public AgenteParticipantes agentePar = new AgenteParticipantes();

        public int nIDAmec = 0;
        public ICollection<DVParticipanteAmec> dParticipantesAmec = null;

        private LogicaAmecInfo MiLogicaAmecInfo;

        public AgenteAmecInfo()
        {
            MiLogicaAmecInfo = new LogicaAmecInfo();
        }

        public AgenteAmecInfo(int nIdPeticionario)
        {
            MiLogicaAmecInfo = new LogicaAmecInfo();
            miUsuarioAmec = agenteUsu.ObtenerDatosPersonalesPorIDPeticionario(nIdPeticionario.ToString());
            miUsuarioRoles = agenteUsu.ObtenerDatosRolesPorLogin(miUsuarioAmec.login);
        }

        public DataSet ObtenerInformeFCPA(string idamec, string nombrePrograma, string estado, string fechaInicio, string fechaFin, string unidad, string area,
string region, string distrito, string year, string mes, string iddistrict, string idsaleforce, string iddepartament)
        {
            return MiLogicaAmecInfo.ObtenerInformeFCPA(idamec, nombrePrograma, estado, fechaInicio, fechaFin, unidad,
                area, region, distrito, year, mes,iddistrict,idsaleforce,iddepartament);
        }

        public bool TieneExpedientesAsociados(int idamec)
        {
            return MiLogicaAmecInfo.TieneExpedientesAsociados(idamec);
        }

        public bool TieneExpedientesAsociadosPorIdAmecs(string idamecs)
        {
            return MiLogicaAmecInfo.TieneExpedientesAsociadosPorIdAmecs(idamecs);
        }

        public int guardarEnInformeFCPA(string idamecs, string idtipoactividad, string descripcion, DateTime? fechacomienzo,
            DateTime? fechafinalizacion, uint idestado, int idtiporiesgo, string nombrePax, string apellido1Pax, string msdid, string tiporeservacolectivo, string idexpediente)
        {
            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();

            string estado = ObtenerNombreEstado(idestado);
            string tiporiesgo = ObtenerNombreTipoRiesgo(idtiporiesgo.ToString());
            string tipoactividad = string.IsNullOrEmpty(idtipoactividad.ToString()) ? "" : ObtenerNombreTipoActividad(idtipoactividad);
            //Unidad, Area, Region y Distrito
            string idunidad = datosRoles.idunidad != null ? datosRoles.idunidad.ToString() : "null";
            string idarea = datosRoles.Idarea != null ? datosRoles.Idarea.ToString() : "null";
            string idregion = datosRoles.idregion != null ? datosRoles.idregion.ToString() : "null";
            string iddistrito = datosRoles.iddistrito != null ? datosRoles.iddistrito.ToString() : "null";
            string unidad;
            string area;
            string region;
            string distrito;

            IList<string> unidadOrganizativaUsuario = ObtenerNombreUnidadOrganizativa(idunidad, idarea, idregion,
                iddistrito);
            string usuario = datosRoles.Nombre + ' ' + datosRoles.Apellido1 + ' ' + datosRoles.Apellido2;
            string cargo = datosRoles.Cargo;
            if (idunidad != "null" && idarea == "null" && idregion != "null" && iddistrito == "null")
            {
                unidad = idunidad != "null" ? unidadOrganizativaUsuario[0] : "";
                area = "";
                region = idregion != "null" ? unidadOrganizativaUsuario[1] : "";
                distrito = "";
            }
            else if (idarea != "null" && idregion == "null" && iddistrito != "null")
            {
                unidad = idunidad != "null" ? unidadOrganizativaUsuario[0] : "";
                area = idarea != "null" ? unidadOrganizativaUsuario[1] : "";
                region = "";
                distrito = iddistrito != "null" ? unidadOrganizativaUsuario[2] : "";
            }
            else
            {
                unidad = idunidad != "null" ? unidadOrganizativaUsuario[0] : "";
                area = idarea != "null" ? unidadOrganizativaUsuario[1] : "";
                region = idregion != "null" ? unidadOrganizativaUsuario[2] : "";
                distrito = iddistrito != "null" ? unidadOrganizativaUsuario[3] : "";
            }
            /////////////////////////////

            return MiLogicaAmecInfo.guardarEnInformeFCPA(idamecs, tipoactividad, descripcion, fechacomienzo,
                fechafinalizacion, estado, usuario, cargo, unidad, area, region, distrito, tiporiesgo, idtiporiesgo,
                nombrePax, apellido1Pax, msdid, tiporeservacolectivo, idexpediente, datosRoles.IdDepartament, datosRoles.IdDistrict, datosRoles.IdSaleforce, datosRoles.IdPosition);
        }

        public bool EsGestorArchivo(int nIdPeticionario)
        {
            return MiLogicaAmecInfo.EsGestorArchivo(nIdPeticionario);
        }

        public IList<string> ObtenerNombreUnidadOrganizativa(string idunidad, string idarea, string idregion, string iddistrito)
        {
            return MiLogicaAmecInfo.ObtenerNombreUnidadOrganizativa(idunidad, idarea, idregion, iddistrito);
        }

        public bool EstaGuardadoAmec(string idamec)
        {
            return MiLogicaAmecInfo.EstaGuardadoAmec(idamec);
        }


        public bool ComprobarSiSeEnvioAFarma(string idamec)
        {
            return MiLogicaAmecInfo.ComprobarSiSeEnvioAFarma(idamec);
        }

        public bool ComprobarSiSeEnvioACasosClinicos(string idamec)
        {
            return MiLogicaAmecInfo.ComprobarSiSeEnvioACasosClinicos(idamec);
        }

        public DataSet ObtenerDocumentacionAdicionalAmec(string idamec)
        {
            return MiLogicaAmecInfo.ObtenerDocumentacionAdicionalAmec(idamec);
        }
        public int InsertarHistorialAmec(int idestado, string idamecs, DDatosPersonalesUsuario peticionario,  int nivelaprobacion, int? idAprobador)
        {
            return MiLogicaAmecInfo.InsertarHistorialAmec(idestado, idamecs, peticionario, nivelaprobacion, idAprobador);
        }
        public int CambiarEstadoAmec(string idestado, string idamecs,int nivelaprobacion = -1)
        {
            return MiLogicaAmecInfo.CambiarEstadoAmec(idestado, idamecs, nivelaprobacion);
        }
        public ICollection<ListadoAmecs> ObtenerListadoAMECs(string filtroAMEC, string filtroSolicitante, string filtroActividad, string filtroAprobacion,
             string filtroEstadoAmec, string filtroAnyo, string filtroMes, string filtroImporte, string filtroTipoImporte, string filtroParaguas,
            string filtroIdAreaUsuarioConectado, string filtroIdDistritoUsuarioConectado, string filtroIdUnidadUsuarioConectado, 
            string filtroIdRegionUsuarioConectado, string filtroIdArea, string filtroIdDistrito, string filtroIdUnidad,
            string filtroIdRegion, string filtroXecUnidadesOrganizativas, string filtroProducto, string filtroFechaInicio,
            string filtroFechaFin, string filtroIdDistrict, string filtroIdDepartament, string filtroIdSaleForce, int filtroIdPeticionarioSession,
            string sortParameter, int startRowIndex, int maximumRows)
        {
            try
            {

                DVPeticionariosRoles datosRoles = ObtenerDatosRoles();
                if (filtroIdUnidad == "0") filtroIdUnidad = "";
                if (filtroIdArea == "0") filtroIdArea = "";
                if (filtroIdRegion == "0") filtroIdRegion = "";
                if (filtroIdDistrito == "0") filtroIdDistrito = "";

                FiltroListadoAMECs filtro =
                    new FiltroListadoAMECs()
                    {
                        AMECLike = filtroAMEC.CambiarAsterisco(),
                        idsolicitante = string.IsNullOrEmpty(filtroSolicitante) ? new Nullable<int>() : int.Parse(filtroSolicitante),
                        estadoAprobacion = string.IsNullOrEmpty(filtroAprobacion) ? new Nullable<int>() : int.Parse(filtroAprobacion),
                        NombrePrograma = filtroActividad.CambiarAsterisco(),
                        idestadoamec = string.IsNullOrEmpty(filtroEstadoAmec) ? new Nullable<int>() : int.Parse(filtroEstadoAmec),
                        paraguas = string.IsNullOrEmpty(filtroEstadoAmec) ? new Nullable<int>() : int.Parse(filtroParaguas),
                        year = string.IsNullOrEmpty(filtroAnyo) ? new Nullable<int>() : int.Parse(filtroAnyo),
                        mes = string.IsNullOrEmpty(filtroMes) ? new Nullable<int>() : int.Parse(filtroMes),
                        Importe = string.IsNullOrEmpty(filtroImporte) ? new Nullable<decimal>() : decimal.Parse(filtroImporte),
                        TipoFiltroImporte = filtroTipoImporte,
                        IdPeticionarioUsuarioConectado = datosRoles.IdPeticionario,
                        IdCargoUsuarioConectado = datosRoles.IdCargo,
                        IdDistritoUsuarioConectado = string.IsNullOrEmpty(filtroIdDistritoUsuarioConectado) ? new Nullable<int>() : int.Parse(filtroIdDistritoUsuarioConectado),
                        IdAreaUsuarioConectado = string.IsNullOrEmpty(filtroIdAreaUsuarioConectado) ? new Nullable<int>() : int.Parse(filtroIdAreaUsuarioConectado),
                        IdRegionUsuarioConectado = string.IsNullOrEmpty(filtroIdRegionUsuarioConectado) ? new Nullable<int>() : int.Parse(filtroIdRegionUsuarioConectado),
                        IdUnidadUsuarioConectado = string.IsNullOrEmpty(filtroIdUnidadUsuarioConectado) ? new Nullable<int>() : int.Parse(filtroIdUnidadUsuarioConectado),
                        IdDistritoFiltro = string.IsNullOrEmpty(filtroIdDistrito) ? new Nullable<int>() : int.Parse(filtroIdDistrito),
                        IdAreaFiltro = string.IsNullOrEmpty(filtroIdArea) ? new Nullable<int>() : int.Parse(filtroIdArea),
                        IdRegionFiltro = string.IsNullOrEmpty(filtroIdRegion) ? new Nullable<int>() : int.Parse(filtroIdRegion),
                        IdUnidadFiltro = string.IsNullOrEmpty(filtroIdUnidad) ? new Nullable<int>() : int.Parse(filtroIdUnidad),
                        XecUnidades = string.IsNullOrEmpty(filtroEstadoAmec) ? new Nullable<bool>() : bool.Parse(filtroXecUnidadesOrganizativas),
                        ProductoLike = filtroProducto.CambiarAsterisco(),
                        FechaInicio = string.IsNullOrEmpty(filtroFechaInicio) ? "" : String.Format("{0:yyyyMMdd}", Convert.ToDateTime(filtroFechaInicio)),
                        FechaFin = string.IsNullOrEmpty(filtroFechaFin) ? "" : String.Format("{0:yyyyMMdd}", Convert.ToDateTime(filtroFechaFin)),
                        SortParameter = sortParameter,
                        StartRowIndex = startRowIndex,
                        MaximumRows = maximumRows,
                        IdDistrict = filtroIdDistrict,
                        IdDepartament = filtroIdDepartament,
                        IdSaleForce = filtroIdSaleForce,
                        IdPeticionarioSession = filtroIdPeticionarioSession
                    };

                int count = 0;
                ICollection<ListadoAmecs> CollectionListado = MiLogicaAmecInfo.ObtenerListadoAMECs(filtro, datosRoles, out count);
                CountObtenerListadoAmecs = count;
                if (filtro.MaximumRows != null && filtro.MaximumRows.Value != 0 && filtro.StartRowIndex != null &&
                  (filtro.idestadoamec == 6))// && !datosRoles.medico.Value && !datosRoles.legal.Value /*&& !datosRoles.administrador.Value*/))
                {
                    List < ListadoAmecs > listado =
                        CheckMeCorrespondeAprobar(CollectionListado.ToList(), datosRoles)
                            .Skip(filtro.StartRowIndex.Value)
                            .Take(filtro.MaximumRows.Value)
                            .ToList();
                    CountObtenerListadoAmecs = listado.Count;
                    return listado;
                    
                }
                return CollectionListado;
                
            }
            catch (Exception ex) { throw ex; }
        }

        public List<ListadoAmecs> CheckMeCorrespondeAprobar(List<ListadoAmecs> lista, DVPeticionariosRoles datosRoles)
        {
            List<ListadoAmecs> result = new List<ListadoAmecs>();
            GestorPermisos gPermisos = new GestorPermisos();

            foreach (var item in lista)
            {
                //if ((item.idamecs < 400000000 && item.idamecs > 500000000))
                if (!item.idamecs.StartsWith("4") && item.idamecs.Length == 9)
                {
                    result.Add(item);
                }
                else
                {
                    IPermiso permiso = gPermisos.GetPermisos(int.Parse(item.idestado.ToString()));
                    if (
                        (item.idestado == EOS.Web.Enums.EstadosAmec.PendienteDtoMedico.GetHashCode() && datosRoles.medico != null && datosRoles.medico.Value) ||
                        (item.idestado == EOS.Web.Enums.EstadosAmec.PendienteDtoLegal.GetHashCode() && datosRoles.legal != null && datosRoles.legal.Value) ||
                        (permiso.PuedoAprobarYRechazar(item.idamecs, datosRoles.IdPeticionario))
                        )
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        public int ObtenerNumeroListadoAMECs(string filtroAMEC, string filtroSolicitante, string filtroActividad, string filtroAprobacion,
               string filtroEstadoAmec, string filtroAnyo, string filtroMes, string filtroImporte, string filtroTipoImporte, string filtroParaguas,
              string filtroIdAreaUsuarioConectado, string filtroIdDistritoUsuarioConectado, string filtroIdUnidadUsuarioConectado, string filtroIdRegionUsuarioConectado, 
              string filtroIdArea, string filtroIdDistrito, string filtroIdUnidad, 
              string filtroIdRegion, string filtroXecUnidadesOrganizativas, string filtroProducto, string filtroFechaInicio, string filtroFechaFin,
              string filtroIdDistrict, string filtroIdDepartament, string filtroIdSaleForce, int filtroIdPeticionarioSession)
        {
            return CountObtenerListadoAmecs;
        }

        public DataSet MailsAEnviarCuandoAprobado(string IdAMEC)
        {
            return MiLogicaAmecInfo.MailsAEnviarCuandoAprobado(IdAMEC);
        }

        public DataSet BUDdelaUnidad(string IdAMEC)
        {
            return MiLogicaAmecInfo.BUDdelaUnidad(IdAMEC);
        }

        public int HayAmecRelacionExpediente(string idamec)
        {
            return MiLogicaAmecInfo.HayAmecRelacionExpediente(idamec);
        }
        //public string ObtenerAMEC()
        //{
        //    string sCodAgencia = "XX";

        //    IEnumerable<DVEmpresa> lEmpresas = agenteMaestro.ObtenerEmpresas(null);
        //    if (lEmpresas != null)
        //    {
        //        DVEmpresa miEmpresa = lEmpresas.FirstOrDefault();
        //        if (miEmpresa != null)
        //        {
        //            sCodAgencia = miEmpresa.CodigoAgencia;
        //        }
        //    }

        public int CambiarEstadoAmec(string idestado, string idamecs, string idpeticionario, string idunidad, string idarea, string idregion, string iddistrito)
        {
            return 1;
        }
        //public string ObtenerCargo()
        //{
        //    string sCargo = "";
        //    if (miUsuarioRoles != null)
        //    {
        //        sCargo = miUsuarioRoles.Cargo;
        //    }
        //    return sCargo;
        //}

        //public string ObtenerArea()
        //{
        //    string sArea = "";

        //    bool bEsAP = EsAtencionPrimaria();
        //    if (bEsAP)
        //    {
        //        DRegion miRegion = agenteMaestro.ObtenerRegiones(null).FirstOrDefault(f => (f.idregion == miUsuarioAmec.idregion));
        //        if (miRegion != null) { sArea = miRegion.region; }
        //    }
        //    else
        //    {
        //        DUnidad miUnidad = agenteMaestro.ObtenerUnidades(null).FirstOrDefault(f => (f.idunidad == miUsuarioAmec.idunidad));
        //        if (miUnidad != null) { sArea = miUnidad.unidad; }
        //    }
        //    return sArea;
        //}

        //public bool EsAtencionPrimaria()
        //{
        //    return (miUsuarioAmec.atencionprimaria.HasValue) ? miUsuarioAmec.atencionprimaria.Value : false;
        //}

        public int CambiarEstadoAmec(string idestado, string idamecs, string idpeticionario, string idaprobador)
        {
            return MiLogicaAmecInfo.CambiarEstadoAmec(idestado, idamecs, idpeticionario, idaprobador);
        }

        public string DameSiguienteIdAmecs()
        {
            return MiLogicaAmecInfo.DameSiguienteIdAmecs();
        }

        public int ObtenerNumeroDocumentacionAMEC(string filtroidamec)
        {
            return MiLogicaAmecInfo.ObtenerNumeroDocumentacionAMEC(filtroidamec);
        }

        public ICollection<DDocumentacionAmec> ObtenerDocumentacionAMEC(string filtroidamec, string sortParameter, int startRowIndex, int maximumRows)
        {
            return MiLogicaAmecInfo.ObtenerDocumentacionAMEC(filtroidamec, sortParameter, startRowIndex, maximumRows);
        }

        public DataSet ObtenerCategoriasDocumento()
        {
            return MiLogicaAmecInfo.ObtenerCategoriasDocumento();
        }
 
        public ICollection<DAmecInfo> ObtenerAMECsInfo(string filtroAMEC, string filtroSolicitante, int filtroEstado,
           string filtroActividad, string filtroPendientes, string filtroAnyo, string filtroMes, string filtroImporte, string filtroTipoImporte,
           string sortParameter, int startRowIndex, int maximumRows)
        {

            FiltroAmecInfo filtroAmecInfo =
                               new FiltroAmecInfo()
                               {

                                   IdAMEC = filtroAMEC,
                                   Solicitante = filtroSolicitante.CambiarAsterisco(),
                                   IdEstado = filtroEstado,
                                   //PendientesAprobar = filtroPendientes,
                                   ImporteGasto = int.Parse(filtroImporte),
                                   //TipoImporte = filtroTipoImporte


                                   SortParameter = sortParameter,
                                   StartRowIndex = startRowIndex,
                                   MaximumRows = maximumRows

                               };

            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
            ICollection<DAmecInfo> myEnumExp = MiLogicaAmecInfo.ObtenerAMECsInfo(filtroAmecInfo, datosRoles);

            return myEnumExp;

        }

        public int ObtenerNumeroAMECsInfo(string filtroAMEC, string filtroSolicitante, string filtroAprobado, string filtroActividad, string filtroPendientes, string filtroAnyo, string filtroMes, string filtroImporte, string filtroTipoImporte)
        {

            return 0;
        }

        public ICollection<DEstadoAmec> ObtenerEstadosAmec()
        {
            return MiLogicaAmecInfo.ObtenerEstadosAmec();
        }

        public uint ObtenerEstadoAmec(string idamecs)
        {
            return MiLogicaAmecInfo.ObtenerEstadoAmec(idamecs);
        }

        public int CrearRelacionAmecCongreso(int idcongres, string idamecs, int idpeticionario)
        {
            return MiLogicaAmecInfo.CrearRelacionAmecCongreso(idcongres, idamecs, idpeticionario);
        }

        public int EliminarRelacionAmecCongreso(int idameccongreso)
        {
            return MiLogicaAmecInfo.EliminarRelacionAmecCongreso(idameccongreso);
        }


        public int EventoYaEstaAsignadoAmec(string idamec, int idcongreso)
        {
            return MiLogicaAmecInfo.EventoYaEstaAsignadoAmec(idamec, idcongreso);

        }

        public int IdAmecAsignadoCongreso(string idamec, int idcongreso)
        {
            return MiLogicaAmecInfo.IdAmecAsignadoCongreso(idamec, idcongreso);

        }

        public DAmecInfo ObtenerProgramaAMEC(string filtroidamec, string sortParameter, int startRowIndex, int maximumRows)
        {
            DAmecInfo AMECInfo = new DAmecInfo();

            AMECInfo = MiLogicaAmecInfo.ObtenerProgramaAMEC(filtroidamec, sortParameter, startRowIndex, maximumRows);
            if (AMECInfo != null)
            {
                if (AMECInfo.urlprograma == "" && AMECInfo.programaamecs == "" && AMECInfo.descripcionobjetivo == "")
                {
                    AMECInfo = null;
                }
            }
            return AMECInfo;
        }
        public int ObtenerNumeroProgramaAMEC(string filtroidamec)
        {

            return MiLogicaAmecInfo.ObtenerNumeroProgramaAMEC(filtroidamec);

        }
        public DAmecInfo CargarTodosValoresAmec(string idamec)
        {
            return MiLogicaAmecInfo.CargarTodosValoresAmec(idamec);
        }

        public DataSet CargarTodosValoresAmecExcel(string idamec)
        {
            return MiLogicaAmecInfo.CargarTodosValoresAmecExcel(idamec);
        }

        public ICollection<DCongresos> CargarEventoActividadesRelacionadasAmec(string idamec)
        {
            return MiLogicaAmecInfo.CargarEventoActividadesRelacionadasAmec(idamec);
        }


        public ICollection<DCongresos> ObtenerEventosNoRelacionadosAMEC(string filtroIdAMEC, string filtroIdCongreso, string filtroNombre, string filtroPoblacion, string filtroAMEC, string filtroTipoActividad, string filtroFechaDesde, string filtroFechaHasta, string sortParameter, int maximumRows, int startRowIndex)
        {
            FiltroAMECCongreso filtroAMECCongreso =
              new FiltroAMECCongreso()
              {

                  IdAMEC = filtroIdAMEC,
                  AmecLike = filtroAMEC.CambiarAsterisco(),
                  IdCongreso = (string.IsNullOrEmpty(filtroIdCongreso)) ? new Nullable<int>() : int.Parse(filtroIdCongreso),
                  NombreEvento = filtroNombre.CambiarAsterisco(),
                  IdPoblacion = (string.IsNullOrEmpty(filtroPoblacion) || filtroPoblacion == "-1") ? new Nullable<int>() : int.Parse(filtroPoblacion),
                  TipoActividad = (string.IsNullOrEmpty(filtroTipoActividad) || filtroTipoActividad == "-1") ? new Nullable<int>() : int.Parse(filtroTipoActividad),
                  FechaDesde = string.IsNullOrEmpty(filtroFechaDesde) ? new Nullable<DateTime>() : DateTime.Parse(filtroFechaDesde),
                  FechaHasta = string.IsNullOrEmpty(filtroFechaHasta) ? new Nullable<DateTime>() : DateTime.Parse(filtroFechaHasta),
                  SortParameter = sortParameter,
                  StartRowIndex = startRowIndex,
                  MaximumRows = maximumRows,

              };

            //Utilitzarem la Logica Maestro perquè no hi ha cap LógicaAMEC declarada
            ICollection<DCongresos> collectionEventosAmecs = MiLogicaAmecInfo.ObtenerEventosNoRelacionadosAMEC(filtroAMECCongreso);
            return collectionEventosAmecs;

        }

        public int ObtenerNumeroEventosNoRelacionadosAMEC(string filtroIdAMEC, string filtroIdCongreso, string filtroNombre, string filtroPoblacion, string filtroAMEC, string filtroTipoActividad, string filtroFechaDesde, string filtroFechaHasta)
        {
            FiltroAMECCongreso filtroAMECCongreso =
              new FiltroAMECCongreso()
              {
                  IdAMEC = filtroIdAMEC,
                  AmecLike = filtroAMEC.CambiarAsterisco(),
                  IdCongreso = (string.IsNullOrEmpty(filtroIdCongreso)) ? new Nullable<int>() : int.Parse(filtroIdCongreso),
                  NombreEvento = filtroNombre.CambiarAsterisco(),
                  IdPoblacion = (string.IsNullOrEmpty(filtroPoblacion) || filtroPoblacion == "-1") ? new Nullable<int>() : int.Parse(filtroPoblacion),
                  TipoActividad = (string.IsNullOrEmpty(filtroTipoActividad) || filtroTipoActividad == "-1") ? new Nullable<int>() : int.Parse(filtroTipoActividad),
                  FechaDesde = string.IsNullOrEmpty(filtroFechaDesde) ? new Nullable<DateTime>() : DateTime.Parse(filtroFechaDesde),
                  FechaHasta = string.IsNullOrEmpty(filtroFechaHasta) ? new Nullable<DateTime>() : DateTime.Parse(filtroFechaHasta),
              };
            long numEventosNoRelacionados = MiLogicaAmecInfo.ObtenerNumeroEventosNoRelacionadosAMEC(filtroAMECCongreso);
            return Convert.ToInt32(numEventosNoRelacionados);
        }

        public DAmecInfo NuevoAMEC(DAmecInfo dAmec)
        {
            //insertar nuevo amec y devolver el idamec.
            DAmecInfo nuevoamec;
            //int nNuevoIDAMEC = dAmec.idamecs;
            nuevoamec = MiLogicaAmecInfo.NuevoAMEC(dAmec);
            return nuevoamec;
        }

        public DAmecInfo NuevoAMECNewCo(DAmecInfo dAmec)
        {
            //insertar nuevo amec y devolver el idamec.
            DAmecInfo nuevoamec;
            //int nNuevoIDAMEC = dAmec.idamecs;
            nuevoamec = MiLogicaAmecInfo.NuevoAMECNewCo(dAmec);
            return nuevoamec;
        }

        public DAmecInfo ClonarAMEC(DAmecInfo dAmec, string idAmecOriginal)
        {
            DAmecInfo amecClonado;
            amecClonado = MiLogicaAmecInfo.ClonarAMEC(dAmec, idAmecOriginal);
            return amecClonado;
        }

        public DataSet DataSetObtenerActividadesAsigAmec(string filtroIdAMEC, string sortParameter, int startRowIndex, int maximumRows)
        {

            DataSet myEnumExp = MiLogicaAmecInfo.DataSetObtenerActividadesAsigAmec(filtroIdAMEC, sortParameter, startRowIndex, maximumRows);
            if (myEnumExp == null) return null;
            if (myEnumExp.Tables[0].Rows.Count == 0) return null; //Per agilitzar la Càrrega
            else return myEnumExp;
        }

        public ICollection<DVCongresoAmec> ObtenerActividadesAsigAmec(string filtroIdAMEC, string sortParameter, int startRowIndex, int maximumRows)
        {
            ICollection<DVCongresoAmec> myEnumExp = MiLogicaAmecInfo.ObtenerActividadesAsigAmec(filtroIdAMEC, sortParameter, startRowIndex, maximumRows);
            if (myEnumExp == null || myEnumExp.Count == 0) return null; //Per agilitzar la Càrrega
            else return myEnumExp;
        }


        public int ObtenerNumeroActividadesAsigAmec(string filtroIdAMEC)
        {
            //ICollection<DCabeceraActividad> myEnumExp = null;

            long lNumeroActividadesAsigAmec = MiLogicaAmecInfo.ObtenerNumeroActividadesAsigAmec(filtroIdAMEC);

            return Convert.ToInt32(lNumeroActividadesAsigAmec);
        }

        public DAmecInfo ModificarAMEC(DAmecInfo dAmec, out bool cambioAgencia, bool updateFechaComienzoFinalizacion)
        {
            //Modificar nuevo amec.
            DAmecInfo amecModificado = new DAmecInfo();
            string nNuevoIDAMEC = dAmec.idamecs;

            amecModificado = MiLogicaAmecInfo.ModificarAMEC(dAmec, out cambioAgencia, updateFechaComienzoFinalizacion);

            return amecModificado;

        }
        public int EliminarDocumentacion(int iddocumentacion)
        {
            return MiLogicaAmecInfo.EliminarDocumentacion(iddocumentacion);
        }

        public int GuardarDocumentacion(string idamec, int idcreadopor, string nombreDocumento, string rutaDocumento, string comentarioDoc, bool AdjuntarAEmail, string Extension, int categoria)
        {
            DDocumentacionAmec DocAmec = new DDocumentacionAmec();
            DocAmec.idamecs = idamec;
            DocAmec.idcreadopor = idcreadopor;
            DocAmec.nombredoc = nombreDocumento;
            DocAmec.ubicaciondoc = rutaDocumento;
            DocAmec.comentariosdoc = comentarioDoc;
            DocAmec.adjuntaraemail = AdjuntarAEmail;
            DocAmec.idcategoriadocumento = categoria;
            switch (Extension)
            {
                case ".txt": DocAmec.tipodoc = "Texto"; break;
                case ".docx":
                case ".doc": DocAmec.tipodoc = "Word"; break;
                case ".xls":
                case ".xlsx": DocAmec.tipodoc = "Excel"; break;
                case ".ppt":
                case ".pptx": DocAmec.tipodoc = "PowerPoint"; break;
                case ".pdf": DocAmec.tipodoc = "PDF"; break;
                case ".png":
                case ".jpg":
                case ".gif":
                case ".bmp": DocAmec.tipodoc = "Imagen"; break;
                default: DocAmec.tipodoc = "Documento"; break;


            }
            return MiLogicaAmecInfo.GuardaDocumentacion(DocAmec);

        }

        public int ObtenerNumPendienteSometer(string idpeticionario)
        {
            return MiLogicaAmecInfo.ObtenerNumPendienteSometer(idpeticionario);
        }

        public string ObtenerNombreTipoRiesgo(string idtiporiesgo)
        {
            return MiLogicaAmecInfo.ObtenerNombreTipoRiesgo(idtiporiesgo);
        }

        public string ObtenerNombreTipoActividad(string idtipoactividad)
        {
            return MiLogicaAmecInfo.ObtenerNombreTipoActividad(idtipoactividad);
        }


        public string ObtenerNombreEstado(UInt32 idestado)
        {
            //Obtener el nombre del estado a partir del idestado del amec
            string nombreEstado = "";

            nombreEstado = MiLogicaAmecInfo.ObtenerNombreEstado(idestado);

            return nombreEstado;
        }

        public int GuardarEstadoAHistorialAMEC(string idamec, int idestado, int idcreadopor, DAmecInfo miAmec)
        {
            return MiLogicaAmecInfo.GuardarEstadoAHistorialAMEC(idamec, idestado, idcreadopor, miAmec);

        }
        
        public ICollection<DHistEstadosAMEC> ObtenerHistorialEstadosAMEC(string filtroidamec, string sortParameter, int startRowIndex, int maximumRows)
        {
            ICollection<DHistEstadosAMEC> HistEstadosAmec = MiLogicaAmecInfo.ObtenerHistorialEstadosAMEC(filtroidamec, sortParameter, startRowIndex, maximumRows);
            return HistEstadosAmec;
        }



        public int ObtenerNumeroHistorialEstadosAMEC(string filtroidamec)
        {
            long numeroHistEstadoAMEC = MiLogicaAmecInfo.ObtenerNumeroHistorialEstadosAMEC(filtroidamec);
            return Convert.ToInt32(numeroHistEstadoAMEC);
        }

        public ICollection<String> ObtenerCorreoParaEnviar(string idamecs, string connectionString)
        {
            return MiLogicaAmecInfo.ObtenerCorreoParaEnviar(idamecs, connectionString);
        }


        public DVCongresoAmec ObtenerCongreso(int idcongreso)
        {
            return MiLogicaAmecInfo.ObtenerCongreso(idcongreso);
        }

        public int GuardaLogMail(string idamecs, string tipo_mail, string message_to, string message_subject, string message_body, string message_fileattach, bool envioCorrecto, string error)
        {
            return MiLogicaAmecInfo.GuardaLogMail(idamecs, tipo_mail, message_to, message_subject, message_body, message_fileattach, envioCorrecto, error);
        }
        private DVPeticionariosRoles ObtenerDatosRoles()
        {
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            return agenteUsu.ObtenerDatosRolesPorLogin();
        }
        public int BorrarHistorialAmec(int idestado, string idamecs, int idPeticionario, int nivelaprobacion)
        {
            return MiLogicaAmecInfo.BorrarHistorialAmec(idestado, idamecs, idPeticionario, nivelaprobacion);
        }
    }


}
