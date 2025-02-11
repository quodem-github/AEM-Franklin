using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Logica;

namespace EOS.Web
{
    public class AgenteExpedientes : AgenteBase
    {
        public int ObtenerExpedientesTotalCount { get; set; }

        public LogicaExpedientes MiLogicaExpedientes { get; set; }

        public AgenteExpedientes()
        {
            MiLogicaExpedientes = new LogicaExpedientes();
        }

        #region "Expedientes"

        public string ObtenerMensajeAMostrar(string tipoActividad, string tipoAsistente, string riskLevel)
        {
            return MiLogicaExpedientes.ObtenerMensajeAMostrar(tipoActividad, tipoAsistente, riskLevel);
        }

        public DataSet ObtenerRiskLevel()
        {
            return MiLogicaExpedientes.ObtenerRiskLevel();
        }

        public DataSet ObtenerTiposActividadPax()
        {
            return MiLogicaExpedientes.ObtenerTiposActividadPax();
        }

        public DataSet ObtenerTiposAsistente()
        {
            return MiLogicaExpedientes.ObtenerTiposAsistente();
        }

        public DataSet ObtenerTiposAsistenteConCalculadora()
        {
            return MiLogicaExpedientes.ObtenerTiposAsistenteConCalculadora();
        }

        public DAmecExpediente ObtenerRelacionAmecExpediente(int idExpediente)
        {
            return MiLogicaExpedientes.ObtenerRelacionAmecExpediente(idExpediente);
        }
        public List<DDocumentoVersion> ObtenerDatosDocumentos(string rutaDir)
        {
            return MiLogicaExpedientes.ObtenerDatosDocumentos(rutaDir);
        }
        public DataSet ObtenerJustificaciones()
        {
            return MiLogicaExpedientes.ObtenerJustificaciones();
        }

        public List<DPassenger> ObtenerPassengers(int idExpediente)
        {
            return MiLogicaExpedientes.ObtenerPassengers(idExpediente);
        }
        public List<DDocumentNameTypeSubType> ObtenerRelacionTipoVersion(int idExpediente)
        {
            return MiLogicaExpedientes.ObtenerRelacionTipoVersion(idExpediente);
        }
        public List<string> ObtenerNombresOriginalesDoc(int idExpediente)
        {
            return MiLogicaExpedientes.ObtenerNombresOriginalesDoc(idExpediente);
        }
        //public ICollection<DCabeceraExpedienteAmpliado> ObtenerExpedientes(string filtroActividad, string filtroAsistente, string filtroEstadoExpediente,
        //    string filtroAmec, string filtroEstadoReserva, string filtroTipo, string filtroAnyo, string filtroMes, string filtroFechaDesde,
        //    string filtroFechaHasta, string filtroUnidad, string filtroProducto, string filtroAreaNegocio, string filtroRegion,
        //    string filtroDistrito, string filtroPeticionario, string filtroProveedor, string filtroImporte, string filtroTipoImporte, string filtroNumExpediente,
        //    string sortParameter, int startRowIndex, string filtroIdDistrict, string filtroIdDepartament, string filtroIdSaleForce,int filtroIdPeticionarioSession, string filtroTipoActividad, int maximumRows)
        //{
        //    DVPeticionariosRoles datosRoles = ObtenerDatosRoles();

        //    FiltroExpedientesAvanzado filtro =
        //        ObtenerFiltroExpedientesAvanzado(filtroActividad, filtroAsistente, filtroEstadoExpediente, filtroAmec,
        //    filtroEstadoReserva, filtroTipo, filtroAnyo, filtroMes, filtroFechaDesde, filtroFechaHasta, filtroUnidad,
        //    filtroProducto, filtroAreaNegocio, filtroRegion, filtroDistrito, filtroPeticionario, filtroProveedor,
        //    filtroImporte, filtroTipoImporte, filtroNumExpediente, sortParameter, startRowIndex,
        //    maximumRows, filtroIdDistrict, filtroIdDepartament, filtroIdSaleForce,filtroIdPeticionarioSession, filtroTipoActividad);

        //    ICollection<DCabeceraExpedienteAmpliado> myEnumExp = MiLogicaExpedientes.ObtenerExpedientes(filtro, datosRoles);
        //    return myEnumExp;
        //}

        public ICollection<DCabeceraExpedienteAmpliado> ObtenerExpedientesTotal(string filtroActividad, string filtroAsistente, string filtroEstadoExpediente,
            string filtroAmec, string filtroEstadoReserva, string filtroTipo, string filtroAnyo, string filtroMes, string filtroFechaDesde,
            string filtroFechaHasta, string filtroUnidad, string filtroProducto, string filtroAreaNegocio, string filtroRegion,
            string filtroDistrito, string filtroPeticionario, string filtroProveedor, string filtroImporte, string filtroTipoImporte, string filtroNumExpediente,
            string sortParameter, int startRowIndex, int maximumRows, string filtroIdDistrict, string filtroIdDepartament, 
            string filtroIdSaleForce, int filtroIdPeticionarioSession, string filtroTipoActividad, bool disableBinding)
        {
            if (disableBinding)
                return new Collection<DCabeceraExpedienteAmpliado>();

            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();

            FiltroExpedientesAvanzado filtro =
                ObtenerFiltroExpedientesAvanzado(filtroActividad, filtroAsistente, filtroEstadoExpediente, filtroAmec,
            filtroEstadoReserva, filtroTipo, filtroAnyo, filtroMes, filtroFechaDesde, filtroFechaHasta, filtroUnidad,
            filtroProducto, filtroAreaNegocio, filtroRegion, filtroDistrito, filtroPeticionario, filtroProveedor,
            filtroImporte, filtroTipoImporte, filtroNumExpediente, sortParameter, startRowIndex, maximumRows,
            filtroIdDistrict,filtroIdDepartament,filtroIdSaleForce,filtroIdPeticionarioSession, filtroTipoActividad);

            int count = 0;
            ICollection<DCabeceraExpedienteAmpliado> myEnumExp = MiLogicaExpedientes.ObtenerExpedientesTotal(filtro, datosRoles, out count);
            ObtenerExpedientesTotalCount = count;

            return myEnumExp;
        }

        public int ObtenerNumeroExpedientes(string filtroActividad, string filtroAsistente, string filtroEstadoExpediente,
            string filtroAmec, string filtroEstadoReserva, string filtroTipo, string filtroAnyo, string filtroMes,
            string filtroFechaDesde, string filtroFechaHasta, string filtroUnidad, string filtroProducto, string filtroAreaNegocio,
            string filtroRegion, string filtroDistrito, string filtroPeticionario, string filtroProveedor, string filtroImporte, 
            string filtroTipoImporte, string filtroNumExpediente, string filtroIdDistrict, string filtroIdDepartament,
            string filtroIdSaleForce,int filtroIdPeticionarioSession, string filtroTipoActividad, bool disableBinding)
        {
            return ObtenerExpedientesTotalCount;
        }

        private DVPeticionariosRoles ObtenerDatosRoles()
        {
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            return agenteUsu.ObtenerDatosRolesPorLogin();
        }

        public FiltroExpedientesAvanzado ObtenerFiltroExpedientesAvanzado(string filtroActividad, string filtroAsistente, string filtroEstadoExpediente,
            string filtroAmec, string filtroEstadoReserva, string filtroTipo, string filtroAnyo, string filtroMes,
            string filtroFechaDesde, string filtroFechaHasta, string filtroUnidad, string filtroProducto, string filtroAreaNegocio,
            string filtroRegion, string filtroDistrito, string filtroPeticionario, string filtroProveedor, string filtroImporte, string filtroTipoImporte, string filtroNumExpediente,
            string sortParameter, int startRowIndex, int maximumRows, string filtroIdDistrict, string filtroIdDepartament, string filtroIdSaleForce, int filtroIdPeticionarioSession,string filtroTipoActividad)
        {
            FiltroExpedientesAvanzado filtro =
                new FiltroExpedientesAvanzado()
                {
                    Actividad = filtroActividad.CambiarAsterisco(),
                    Asistente = filtroAsistente.CambiarAsterisco(),
                    EstadoExpediente = filtroEstadoExpediente,
                    Amec = filtroAmec.CambiarAsterisco(),
                    EstadoReserva = filtroEstadoReserva,
                    Tipo = string.IsNullOrEmpty(filtroTipo) ? new Nullable<int>() : int.Parse(filtroTipo),
                    Año = string.IsNullOrEmpty(filtroAnyo) ? new Nullable<int>() : int.Parse(filtroAnyo),
                    Mes = string.IsNullOrEmpty(filtroMes) ? new Nullable<int>() : int.Parse(filtroMes),
                    Desde = string.IsNullOrEmpty(filtroFechaDesde) ? new Nullable<DateTime>() : DateTime.Parse(filtroFechaDesde),
                    Hasta = string.IsNullOrEmpty(filtroFechaHasta) ? new Nullable<DateTime>() : DateTime.Parse(filtroFechaHasta),
                    Unidad = filtroUnidad.CambiarAsterisco(),
                    Producto = filtroProducto.CambiarAsterisco(),
                    AreaNegocio = filtroAreaNegocio.CambiarAsterisco(),
                    Region = filtroRegion.CambiarAsterisco(),
                    Distrito = filtroDistrito.CambiarAsterisco(),
                    Peticionario = filtroPeticionario.CambiarAsterisco(),
                    ProveedorServicio = filtroProveedor.CambiarAsterisco(),
                    Importe = string.IsNullOrEmpty(filtroImporte) ? new Nullable<decimal>() : decimal.Parse(filtroImporte),
                    TipoFiltroImporte = filtroTipoImporte,
                    IdExpediente = string.IsNullOrEmpty(filtroNumExpediente) ? new long?() : long.Parse(filtroNumExpediente),
                    SortParameter = sortParameter,
                    StartRowIndex = startRowIndex,
                    MaximumRows = maximumRows,
                    IdDistrict = filtroIdDistrict,
                    IdDepartament = filtroIdDepartament,
                    IdSaleForce = filtroIdSaleForce,
                    IdPeticionarioSession = filtroIdPeticionarioSession,
                    TipoActividad = filtroTipoActividad
                };
            return filtro;
        }

        public DCabeceraExpedienteAmpliado ObtenerExpedientePorID(string filtroIdExp)
        {
            DCabeceraExpedienteAmpliado miExp = null;
            if (!string.IsNullOrEmpty(filtroIdExp))
            {
                FiltroExpedientesAvanzado flt = new FiltroExpedientesAvanzado();
                flt.IdExpediente = (string.IsNullOrEmpty(filtroIdExp)) ? -1 : int.Parse(filtroIdExp);
                miExp = MiLogicaExpedientes.ObtenerExpedientes(flt, null).FirstOrDefault();
            }

            return miExp;
        }

        public DCabeceraExpedienteAmpliado ObtenerExpedientePorID(string filtroIdExp, DVPeticionariosRoles datosRoles)
        {
            DCabeceraExpedienteAmpliado miExp = null;
            if (!string.IsNullOrEmpty(filtroIdExp))
            {
                FiltroExpedientesAvanzado flt = new FiltroExpedientesAvanzado();
                flt.IdExpediente = (string.IsNullOrEmpty(filtroIdExp)) ? -1 : int.Parse(filtroIdExp);
                miExp = MiLogicaExpedientes.ObtenerExpedientes(flt, datosRoles).FirstOrDefault();
            }
            return miExp;
        }

        #endregion "Expedientes"

        #region "AMECS"

        public ICollection<DVAmecCongreso> ObtenerAMECs(string filtroAMEC, string filtroSolicitante, string filtroAprobado,
            string filtroActividad, string filtroPendientes, string filtroAnyo, string filtroMes, string filtroImporte, string filtroTipoImporte,
            string sortParameter, int startRowIndex, int maximumRows)
        {
            FiltroAMECs filtro =
                   new FiltroAMECs()
                   {
                       AMECLike = filtroAMEC.CambiarAsterisco(),
                       Solicitante = filtroSolicitante.CambiarAsterisco(),
                       Aprobado = filtroAprobado,
                       CongresoLike = filtroActividad.CambiarAsterisco(),
                       PendientesAprobar = filtroPendientes,
                       Año = string.IsNullOrEmpty(filtroAnyo) ? new Nullable<int>() : int.Parse(filtroAnyo),
                       Mes = string.IsNullOrEmpty(filtroMes) ? new Nullable<int>() : int.Parse(filtroMes),
                       Importe = string.IsNullOrEmpty(filtroImporte) ? new Nullable<decimal>() : decimal.Parse(filtroImporte),
                       TipoFiltroImporte = filtroTipoImporte,
                       SortParameter = sortParameter,
                       StartRowIndex = startRowIndex,
                       MaximumRows = maximumRows
                   };

            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
            ICollection<DVAmecCongreso> myEnumExp = MiLogicaExpedientes.ObtenerAMECs(filtro, datosRoles);

            return myEnumExp;
        }

        public int ObtenerNumeroAMECs(string filtroAMEC, string filtroSolicitante, string filtroAprobado,
            string filtroActividad, string filtroPendientes, string filtroAnyo, string filtroMes, string filtroImporte, string filtroTipoImporte)
        {
            FiltroAMECs filtro =
                   new FiltroAMECs()
                   {
                       AMECLike = filtroAMEC.CambiarAsterisco(),
                       Solicitante = filtroSolicitante.CambiarAsterisco(),
                       Aprobado = filtroAprobado,
                       CongresoLike = filtroActividad.CambiarAsterisco(),
                       PendientesAprobar = filtroPendientes,
                       Año = string.IsNullOrEmpty(filtroAnyo) ? new Nullable<int>() : int.Parse(filtroAnyo),
                       Mes = string.IsNullOrEmpty(filtroMes) ? new Nullable<int>() : int.Parse(filtroMes),
                       Importe = string.IsNullOrEmpty(filtroImporte) ? new Nullable<decimal>() : decimal.Parse(filtroImporte),
                       TipoFiltroImporte = filtroTipoImporte
                   };

            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
            long ltemp = MiLogicaExpedientes.ObtenerNumeroAMECs(filtro, datosRoles);
            return Convert.ToInt32(ltemp);
        }

        public DVAmecCongreso ObtenerAMECporID(string filtroIdAMEC)
        {
            DVAmecCongreso miAMEC = null;
            if (!string.IsNullOrEmpty(filtroIdAMEC))
            {
                FiltroAMECs flt = new FiltroAMECs();
                flt.IdAMEC = filtroIdAMEC;
                miAMEC = MiLogicaExpedientes.ObtenerAMECs(flt, null).FirstOrDefault();
            }

            return miAMEC;
        }

        public DAmec ObtenerEntidadAMECporID(string filtroIdAMEC)
        {
            DAmec miAMEC = null;
            if (!string.IsNullOrEmpty(filtroIdAMEC))
            {
                FiltroAMECs flt = new FiltroAMECs();
                flt.IdAMEC = filtroIdAMEC;
                miAMEC = MiLogicaExpedientes.ObtenerEntidadAMECs(flt).FirstOrDefault();
            }

            return miAMEC;
        }

        /*
        public ICollection<DVAmecCongreso> ObtenerAMECPorActividad()
        {
            ICollection<DVAmecCongreso> myEnumExp = null;
            if (!string.IsNullOrEmpty("1"))
            {
                FiltroAMECs flt = new FiltroAMECs();
                flt.IdCongreso = (string.IsNullOrEmpty("1")) ? -1 : int.Parse("1");
                myEnumExp = MiLogicaExpedientes.ObtenerAMECs(flt);
            }
            return myEnumExp;
        }*/


        public ICollection<DVAmecCongresoConcatSolicitante> ObtenerAMECPorCongresoConcatSolicitante(int idcongreso, string AmecReunionesInternas, int? idconfempresa, bool includeOldAmecs = true, string idAmecsLike = null)
        {
            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();
            LogicaAmecInfo logAmecInfo = new LogicaAmecInfo();
            string AmecPorRoles = logAmecInfo.ObtenerFiltroRolAmecCliente(datosRoles);
            ICollection<DVAmecCongresoConcatSolicitante> myEnum = null;
            int amecReuniones = Convert.ToInt32(AmecReunionesInternas);
            myEnum = MiLogicaExpedientes.ObtenerAMECPorCongresoConcatSolicitante(idcongreso, AmecPorRoles, amecReuniones, datosRoles, idconfempresa, includeOldAmecs, idAmecsLike);
            myEnum = myEnum.OrderBy(x => x.AmecConcatSolicitante).ToList();
            return myEnum;
        }


        public ICollection<DVAmecCongreso> ObtenerAMECPorCongreso(int idcongreso)
        {
            ICollection<DVAmecCongreso> myEnumExp = null;
            FiltroAMECs flt = new FiltroAMECs();
            flt.IdCongreso = idcongreso;
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
            myEnumExp = MiLogicaExpedientes.ObtenerAMECs(flt, datosRoles);
            return myEnumExp;
        }

        public ICollection<DVAmecCongreso> ObtenerAMECPorActividad(string filtroIdCongreso)
        {
            ICollection<DVAmecCongreso> myEnumExp = null;
            if (!string.IsNullOrEmpty(filtroIdCongreso))
            {
                FiltroAMECs flt = new FiltroAMECs();
                flt.IdCongreso = (string.IsNullOrEmpty(filtroIdCongreso)) ? -1 : int.Parse(filtroIdCongreso);
                AgenteUsuarios agenteUsu = new AgenteUsuarios();
                DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                myEnumExp = MiLogicaExpedientes.ObtenerAMECs(flt, datosRoles);
            }
            return myEnumExp;
        }

        public Boolean intentaObtenerAmec(ref DVAmecCongreso oAmecCongreso, string filtroIdCongreso, string Amec)
        {
            List<DVAmecCongreso> lstAmecCongreso;

            lstAmecCongreso = ObtenerAMECPorActividad(filtroIdCongreso).ToList();

            foreach (DVAmecCongreso oAmecCongresoIt in lstAmecCongreso)
            {
                if (oAmecCongresoIt.AMEC.Equals(Amec))
                {
                    oAmecCongreso = oAmecCongresoIt;
                    return true;
                }
            }

            return false;
        }

        #endregion "AMECS"

        #region "Actividades"


        public ICollection<DCabeceraActividad> ObtenerActividades1(string filtroID, string filtroNombre1, string filtroPoblacion, string filtroAMEC,
            string filtroTipoActividad, string filtroFechaDesde, string filtroFechaHasta, bool isGestorInvitados,
            string sortParameter, int startRowIndex, int maximumRows)
        {
            return ObtenerActividades(filtroID, filtroNombre1, filtroPoblacion, filtroAMEC, filtroTipoActividad, filtroFechaDesde, filtroFechaHasta, isGestorInvitados, sortParameter, startRowIndex, maximumRows);
        }

        public ICollection<DCabeceraActividad> ObtenerActividades2(string filtroID, string filtroNombre2, string filtroPoblacion, string filtroAMEC,
            string filtroTipoActividad, string filtroFechaDesde, string filtroFechaHasta, bool isGestorInvitados,
            string sortParameter, int startRowIndex, int maximumRows)
        {
            return ObtenerActividades(filtroID, filtroNombre2, filtroPoblacion, filtroAMEC, filtroTipoActividad, filtroFechaDesde, filtroFechaHasta, isGestorInvitados, sortParameter, startRowIndex, maximumRows);
        }

        public ICollection<DCabeceraActividad> ObtenerActividades(string filtroID, string filtroNombre, string filtroPoblacion, string filtroAMEC,
            string filtroTipoActividad, string filtroFechaDesde, string filtroFechaHasta, bool isGestorInvitados,
            string sortParameter, int startRowIndex, int maximumRows)
        {
            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();

            FiltroActividades filtro =
                new FiltroActividades()
                {
                    IdCongreso = (string.IsNullOrEmpty(filtroID)) ? new Nullable<int>() : int.Parse(filtroID),
                    NombreLike = filtroNombre.CambiarAsterisco(),
                    IDPoblacion = (string.IsNullOrEmpty(filtroPoblacion) || filtroPoblacion == "-1") ? new Nullable<int>() : int.Parse(filtroPoblacion),
                    AmecLike = filtroAMEC.CambiarAsterisco(),
                    TipoActividad = (string.IsNullOrEmpty(filtroTipoActividad) || filtroTipoActividad == "-1") ? new Nullable<int>() : int.Parse(filtroTipoActividad),
                    Desde = string.IsNullOrEmpty(filtroFechaDesde) ? new Nullable<DateTime>() : DateTime.Parse(filtroFechaDesde),
                    Hasta = string.IsNullOrEmpty(filtroFechaHasta) ? new Nullable<DateTime>() : DateTime.Parse(filtroFechaHasta),
                    SortParameter = sortParameter,
                    StartRowIndex = startRowIndex,
                    MaximumRows = maximumRows,
                    Publicar = true,
                    IdConfEmpresa = datosRoles.IdConfEmpresa,
                    isAdmin = datosRoles.administrador,
                    newco = datosRoles.newco,
                    isGestorInvitados = isGestorInvitados
                };

            ICollection<DCabeceraActividad> myEnumExp = MiLogicaExpedientes.ObtenerActividades(filtro);
            //ICollection<DCabeceraActividad> myEnumExp = null;

            return myEnumExp;
        }

        public ICollection<DCabeceraEForm> ObtenerDatosEventosFormulario(string filtroID, string sortParameter, int startRowIndex, int maximumRows)
        {
            FiltroEForm filtro =
                new FiltroEForm()
                {
                    IdCongreso = (string.IsNullOrEmpty(filtroID)) ? new Nullable<int>() : int.Parse(filtroID),
                    SortParameter = sortParameter,
                    StartRowIndex = startRowIndex,
                    MaximumRows = maximumRows,
                    Publicar = true
                };

            return MiLogicaExpedientes.ObtenerDatosEventosFormulario(filtro);
        }

        public int ObtenerNumDatosEventosFormulario(string filtroID, string filtroDescripcion, string filtroFechaDesde, string filtroFechaHasta, string filtroPoblacion)
        {
            FiltroEForm filtro =
                new FiltroEForm()
                {
                    IdCongreso = (string.IsNullOrEmpty(filtroID)) ? new Nullable<int>() : int.Parse(filtroID),
                    Publicar = true
                };

            return Convert.ToInt32(MiLogicaExpedientes.ObtenerNumDatosEventosFormulario(filtro));
        }

        public int ObtenerNumeroActividades1(string filtroID, string filtroNombre1, string filtroPoblacion, string filtroAMEC,
            string filtroTipoActividad, string filtroFechaDesde, string filtroFechaHasta, bool isGestorInvitados)
        {
            return ObtenerNumeroActividades(filtroID, filtroNombre1, filtroPoblacion, filtroAMEC, filtroTipoActividad, filtroFechaDesde, filtroFechaHasta, isGestorInvitados);
        }

        public int ObtenerNumeroActividades2(string filtroID, string filtroNombre2, string filtroPoblacion, string filtroAMEC,
            string filtroTipoActividad, string filtroFechaDesde, string filtroFechaHasta, bool isGestorInvitados)
        {
            return ObtenerNumeroActividades(filtroID, filtroNombre2, filtroPoblacion, filtroAMEC, filtroTipoActividad, filtroFechaDesde, filtroFechaHasta, isGestorInvitados);
        }

        public int ObtenerNumeroActividades(string filtroID, string filtroNombre, string filtroPoblacion, string filtroAMEC,
            string filtroTipoActividad, string filtroFechaDesde, string filtroFechaHasta,bool isGestorInvitados)
        {
            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();
            FiltroActividades filtro =
                new FiltroActividades()
                {
                    IdCongreso = (string.IsNullOrEmpty(filtroID)) ? new Nullable<int>() : int.Parse(filtroID),
                    NombreLike = filtroNombre.CambiarAsterisco(),
                    IDPoblacion = (string.IsNullOrEmpty(filtroPoblacion) || filtroPoblacion == "-1") ? new Nullable<int>() : int.Parse(filtroPoblacion),
                    AmecLike = filtroAMEC.CambiarAsterisco(),
                    TipoActividad = (string.IsNullOrEmpty(filtroTipoActividad) || filtroTipoActividad == "-1") ? new Nullable<int>() : int.Parse(filtroTipoActividad),
                    Desde = string.IsNullOrEmpty(filtroFechaDesde) ? new Nullable<DateTime>() : DateTime.Parse(filtroFechaDesde),
                    Hasta = string.IsNullOrEmpty(filtroFechaHasta) ? new Nullable<DateTime>() : DateTime.Parse(filtroFechaHasta),
                    Publicar = true,
                    IdConfEmpresa = datosRoles.IdConfEmpresa,
                    isAdmin = datosRoles.administrador,
                    newco = datosRoles.newco,
                    isGestorInvitados = isGestorInvitados
                };

            long lNumeroActividades = MiLogicaExpedientes.ObtenerNumeroActividades(filtro);

            return Convert.ToInt32(lNumeroActividades);
        }        

        public DCabeceraActividad ObtenerActividadPorID(string filtroIdAct)
        {
            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();
            DCabeceraActividad miActividad = null;
            if (!string.IsNullOrEmpty(filtroIdAct))
            {
                FiltroActividades flt = new FiltroActividades();
                flt.IdCongreso = (string.IsNullOrEmpty(filtroIdAct)) ? -1 : int.Parse(filtroIdAct);
                flt.isAdmin = datosRoles.administrador;
                flt.newco = datosRoles.newco;
                miActividad = MiLogicaExpedientes.ObtenerActividades(flt).FirstOrDefault();
            }

            return miActividad;
        }

        #endregion "Actividades"

        public int CambiaEstadoExpedienteEnviado(int nIDExpediente)
        {
            return MiLogicaExpedientes.CambiaEstadoExpedienteEnviado(nIDExpediente);
        }

        public int CambiaEstadoExpedienteAprobado(int nIDExpediente, ref int nValidaEstadoAmec, ref int nValidaPresupAmec)
        {
            return MiLogicaExpedientes.CambiaEstadoExpedienteAprobado(nIDExpediente, ref nValidaEstadoAmec, ref nValidaPresupAmec);
        }

        public int CambiaEstadoExpedienteCancelado(int nIDExpediente)
        {
            return MiLogicaExpedientes.CambiaEstadoExpedienteCancelado(nIDExpediente);
        }

        public int CambiaEstadoExpedienteFinalizado(int idexpediente)
        {
            return MiLogicaExpedientes.CambiaEstadoExpedienteFinalizado(idexpediente);
        }

        public DEmpGpPetAmecExp ObtenerEmpleadoGpPorPetAmecExp(int idPet, string idAmec, int idExp)
        {
            return MiLogicaExpedientes.ObtenerEmpleadoGpPorPetAmecExp(idPet, idAmec, idExp);
        }

        public int CambiaEstadoServicioSinEnviar(int nIDReserva)
        {
            return MiLogicaExpedientes.CambiaEstadoServicioSinEnviar(nIDReserva);
        }

        public int CambiaEstadoServicioAceptado(int nIDReserva)
        {
            return MiLogicaExpedientes.CambiaEstadoServicioAceptado(nIDReserva);
        }

        public int CambiaEstadoServicioCancelado(int nIDReserva)
        {
            return MiLogicaExpedientes.CambiaEstadoServicioCancelado(nIDReserva);
        }

        public int CrearCopiaExpediente(int nIDExpediente, DDatosPersonalesUsuario datosUsuario)
        {
            return MiLogicaExpedientes.CrearCopiaExpediente(nIDExpediente, datosUsuario);
        }

        public bool PuedeAprobar(int nIDExpediente)
        {
            return MiLogicaExpedientes.PuedeAprobar(nIDExpediente);
        }

        public int NuevoExpediente(int? nIDRegion, int nIDAmec, int? nIDUnidad, int? nIDArea, int nIDPeticionario, int nIDTipoReserva, int nIDEmpresa, string sEstado, int? nIDDistrito, DateTime dFechaCreacion, string sIDEmpleadoGP, string sPedido, int? tipoPagoFee, bool bUrgente, ICollection<DVParticipante> dTodosParticipantes, ICollection<int> dParticipantesEliminados, DataTable dtparticipantesNuevos, int nIDExp, int? nIDDepartament, int? nIDSalesForce, int? nIDDistrict)
        {
            DExpediente expediente = new DExpediente();

            expediente.idregion = nIDRegion;
            expediente.idamec = nIDAmec;
            expediente.idunidad = nIDUnidad;
            expediente.Idarea = nIDArea;
            expediente.IdPeticionario = nIDPeticionario;
            expediente.IdTipoReserva = nIDTipoReserva;
            expediente.IdEmpresa = nIDEmpresa;
            expediente.idestado = sEstado;
            expediente.iddistrito = nIDDistrito;
            expediente.fechacreacion = dFechaCreacion;
            expediente.idempleadogp = sIDEmpleadoGP;
            expediente.codexpediente = sPedido;
            expediente.tipoPagoFee = tipoPagoFee;
            expediente.urgente = bUrgente;
            expediente.idxpediente = nIDExp;
            expediente.iddepartament = nIDDepartament;
            expediente.idsaleforce = nIDSalesForce;
            expediente.iddistrict = nIDDistrict;

            AgenteParticipantes agentePar = new AgenteParticipantes();
            int nNuevoIdExpediente = nIDExp;

            if (nIDExp <= 0)
            {
                nNuevoIdExpediente = MiLogicaExpedientes.NuevoExpediente(expediente);
            }
            else
            {
                MiLogicaExpedientes.ActualizarExpediente(expediente);
                foreach (int participanteEliminado in dParticipantesEliminados)
                {
                    agentePar.EliminarReservasExpediente(participanteEliminado, expediente.idxpediente);
                }

            }


            for (int i = 0; i < dtparticipantesNuevos.Rows.Count; i++)
            {
                if (dtparticipantesNuevos.Rows[i]["idpassengerlist"].ToString() != "")
                {
                    agentePar.NuevaReservaParticipante(int.Parse(dtparticipantesNuevos.Rows[i]["idpassengerlist"].ToString()), nNuevoIdExpediente);
                }
            }

            if (nIDExp > 0)
            {
                // Actualiza los servicios asociados
                agentePar.EliminarReservasServiciosParticipante(expediente.idxpediente);
            }

            return nNuevoIdExpediente;
        }

        public int NuevaPeticionActividad(DateTime inicio, DateTime finalizacion, DateTime creacion, int idPeticionario, int IDPoblacion, string nombre, int tipoActividad, int idEspecialidad, bool comunicar, bool internacional, string sede, string urlWeb, string email, int idconfempresa)
        {
            DPeticionActividad peticion = new DPeticionActividad();
            peticion.desde = inicio;
            peticion.hasta = finalizacion;
            peticion.fechacreacion = creacion;
            peticion.IDPoblacion = IDPoblacion;
            peticion.sede = sede;
            peticion.nombre = nombre;
            peticion.idpeticionario = idPeticionario;
            peticion.IdTipoCongreso = tipoActividad;
            peticion.idespecialidad = idEspecialidad;
            peticion.url_web = urlWeb;
            peticion.email_secretaria = email;
            peticion.idespecialidad = idEspecialidad;
            peticion.comunicar = comunicar;
            peticion.internacional = internacional;
            peticion.idconfempresa = idconfempresa;

            return MiLogicaExpedientes.NuevaPeticionActividad(peticion);
        }

        public int NuevoAMEC(DAmec dNuevoAmec)
        {
            AgenteParticipantes agentePar = new AgenteParticipantes();
            int nNuevoIDAMEC = dNuevoAmec.idamec;

            if (nNuevoIDAMEC <= 0)
            {
                nNuevoIDAMEC = MiLogicaExpedientes.NuevoAMEC(dNuevoAmec);
            }
            else
            {
                nNuevoIDAMEC = MiLogicaExpedientes.NuevoAMEC(dNuevoAmec);
                agentePar.EliminarReservasAMEC(dNuevoAmec.idamec);
            }

            return nNuevoIDAMEC;
        }

        public int NuevoAMEC(int nIDAmec, string sAMEC, int nIDEmpresa, int nIDCogreso, int? nNumBoletines,
            string sEstado, bool? bAprobado, int? nIdPeticionario, DateTime? dFechaAprobadoLegal, bool? bAprobadoLegal,
            int? nIdPeticionario2, DateTime? dFechaAprobadoComplaice, bool? bAprobadoComplaice, int? nIdPeticionActividad,
            int? nIdArea, int? nIdArea1, int? nIdArea2, int? nIdArea3, int? nIdUnidad, int? nIdTipoActividadCongreso,
            int? nIdDistrito, bool? bParaguas, string sEspecificarOtras, string sNombrePrograma, string sCodGenesis,
            string sObjetivosPrograma, string sContenido, string sLugar, string sAmbitoGeografico, string sDuracion,
            string sNumParticipantes, DateTime? dFechaComiendoP, DateTime? dFechaFinP

            )
        {
            DAmec miAmec = new DAmec();
            miAmec.idamec = nIDAmec;
            miAmec.amec = sAMEC;
            miAmec.IdEmpresa = nIDEmpresa;
            miAmec.IdCongreso = nIDCogreso;
            miAmec.numboletines = nNumBoletines;

            miAmec.idestado = sEstado;
            miAmec.aprobado = bAprobado;
            miAmec.idpeticionario = nIdPeticionario.HasValue ? nIdPeticionario : null;
            miAmec.fechaaprobadolegal = dFechaAprobadoLegal.HasValue ? dFechaAprobadoLegal : null;
            miAmec.aprobadolegal = bAprobadoLegal.HasValue ? bAprobadoLegal : null;
            miAmec.idpeticionario2 = nIdPeticionario2.HasValue ? nIdPeticionario2 : null;
            miAmec.fechaaprobadocomplaice = dFechaAprobadoComplaice.HasValue ? dFechaAprobadoComplaice : null;
            miAmec.aprobadocomplaice = bAprobadoComplaice.HasValue ? bAprobadoComplaice : null;
            miAmec.idpeticionactividad = nIdPeticionActividad.HasValue ? nIdPeticionActividad : null;
            miAmec.Idarea = nIdArea.HasValue ? nIdArea : null;

            int nNuevoIDAMEC = nIDAmec;

            if (nIDAmec <= 0)
            {
                nNuevoIDAMEC = MiLogicaExpedientes.NuevoAMEC(miAmec);
            }

            return nNuevoIDAMEC;
        }

        public DVCongresos ObtenerCongreso(int idActividad)
        {
            return MiLogicaExpedientes.ObtenerCongreso(idActividad);
        }

        public IEnumerable<DVServicioHotel> ObtenerServiciosHoteles(int idExpediente)
        {
            return MiLogicaExpedientes.ObtenerServiciosHoteles(idExpediente);
        }

        public IEnumerable<DVServicioActividades> ObtenerOtrosServicios(int idExpediente)
        {
            return MiLogicaExpedientes.ObtenerOtrosServicios(idExpediente);
        }

        public IEnumerable<DVServicioInscripciones> ObtenerServiciosInscripciones(int idExpediente)
        {
            return MiLogicaExpedientes.ObtenerServiciosInscripciones(idExpediente);
        }

        public IEnumerable<DVServicioTransporte> ObtenerServiciosTransportes(int idExpediente)
        {
            return MiLogicaExpedientes.ObtenerServiciosTransportes(idExpediente);
        }

        public ICollection<DVServicioPassengerResumen> ObtenerResumenParticipantes(int filtroIDExp,
            string sortParameter, string groupParameter, int startRowIndex, int maximumRows)
        {
            FiltroExpedientesAvanzado filtro =
                new FiltroExpedientesAvanzado()
                {
                    IdExpediente = filtroIDExp,
                    SortParameter = sortParameter,
                    GroupParameter = groupParameter,
                    StartRowIndex = startRowIndex,
                    MaximumRows = maximumRows
                };

            ICollection<DVServicioPassengerResumen> myEnumExp = MiLogicaExpedientes.ObtenerResumenParticipantes(filtro);

            return myEnumExp;
        }

        public int ObtenerNumeroResumenParticipantes(int filtroIDExp, string sortParameter, string groupParameter)
        {
            FiltroExpedientesAvanzado filtro =
                new FiltroExpedientesAvanzado()
                {
                    IdExpediente = filtroIDExp,
                };
            long lNumeroExpedientes = MiLogicaExpedientes.ObtenerNumeroResumenParticipantes(filtro);

            return Convert.ToInt32(lNumeroExpedientes);
        }


        public int NumeroExpedientesPorAmec(string NumAmec)
        {
            return MiLogicaExpedientes.NumeroExpedientesPorAmec(NumAmec);
        }

        public ICollection<DExpediente> ExpedientesPorAmec(string NumAmec)
        {
            return MiLogicaExpedientes.ExpedientesPorAmec(NumAmec);
        }

        public string GetEstado(int idexpediente)
        {
            return MiLogicaExpedientes.GetEstado(idexpediente);
        }

        public ICollection<DVResumenEstados> ObtenerResumenEstados()
        {
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();

            ICollection<DVResumenEstados> myEnumEst = MiLogicaExpedientes.ObtenerResumenEstados(datosRoles);

            return myEnumEst;
        }

        public long GetTotalEstado(ICollection<DVResumenEstados> lResEstados, string sEstado)
        {
            DVResumenEstados dvTemp = lResEstados.FirstOrDefault(f => f.IdEstado == sEstado);
            return (dvTemp != null) ? dvTemp.Total : 0;
        }

        public string ObtenerSiguienteNombreAMEC(string sCodAgencia)
        {
            return MiLogicaExpedientes.ObtenerSiguienteNombreAMEC(sCodAgencia);
        }

        public int AprobarAMECs(string filtroAMEC, string filtroSolicitante, string filtroAprobado,
            string filtroActividad, string filtroPendientes, string filtroAnyo, string filtroMes, string filtroImporte, string filtroTipoImporte)
        {
            int nAprobados = 0;

            ICollection<DVAmecCongreso> myListAMEC = ObtenerAMECs(filtroAMEC, filtroSolicitante, filtroAprobado, filtroActividad, filtroPendientes, filtroAnyo, filtroMes, filtroImporte, filtroTipoImporte, "", 0, 10000);

            if (myListAMEC != null)
            {
                AgenteUsuarios agenteUsu = new AgenteUsuarios();
                DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                foreach (DVAmecCongreso amec in myListAMEC)
                {
                    nAprobados += MiLogicaExpedientes.AprobarAMEC(amec.IdAmec, datosRoles.idtipoaprobador, null);
                }
            }

            return nAprobados;
        }

        #region "Obtener Mensajes"

        public const string MSG_JS_NAVEGACION = "DetalleExpediente.aspx?idexp={0}";
        public const string MSG_JS_YACANCELADO = "javascript:alert('El expediente {0} no puede ser modificado porque está Cancelado');return false;";
        public const string MSG_JS_YACERRADO = "javascript:alert('El expediente {0} no puede ser modificado porque está Cerrado');return false;";
        public const string MSG_JS_YAENVIADO = "javascript:alert('El expediente {0} no puede ser enviado');return false;";
        public const string MSG_JS_YAAPROBADO = "javascript:alert('El expediente {0} no puede ser aprobado sin tener servicios cotizados');return false;";

        public const string MSG_JS_SEMODIFICADO = "javascript:alert('Este servicio no puede ser modificado en el estado actual');return false;";
        public const string MSG_JS_SEAPROBADO = "javascript:alert('Este servicio no puede ser aprobado en el estado actual');return false;";
        public const string MSG_JS_SECANCELADO = "javascript:alert('Este servicio no puede ser cancelado en el estado actual');return false;";
        public const string MSG_JS_RESOMETER = "javascript:confirm('Este Amec ya esta Sometido. ¿Quiere Resometerlo?');";

        public string MsgNavegacion(int nIdExpediente)
        {
            return MsgGenerico(nIdExpediente, MSG_JS_NAVEGACION);
        }

        public string MsgResometer()
        {
            return MsgGenerico(0, MSG_JS_NAVEGACION);
        }

        public string MsgYaCancelado(int nIdExpediente)
        {
            return MsgGenerico(nIdExpediente, MSG_JS_YACANCELADO);
        }

        public string MsgYaCerrado(int nIdExpediente)
        {
            return MsgGenerico(nIdExpediente, MSG_JS_YACERRADO);
        }

        public string MsgYaEnviado(int nIdExpediente)
        {
            return MsgGenerico(nIdExpediente, MSG_JS_YAENVIADO);
        }

        public string MsgYaAprobado(int nIdExpediente)
        {
            return MsgGenerico(nIdExpediente, MSG_JS_YAAPROBADO);
        }

        public string MsgSeModificado()
        {
            return MsgGenerico(0, MSG_JS_SEMODIFICADO);
        }

        public string MsgSeAprobado()
        {
            return MsgGenerico(0, MSG_JS_SEAPROBADO);
        }

        public string MsgSeCancelado()
        {
            return MsgGenerico(0, MSG_JS_SECANCELADO);
        }

        public string MsgGenerico(int nIdExpediente, string sMsgBase)
        {
            return string.Format(sMsgBase, nIdExpediente);
        }

        #endregion "Obtener Mensajes"

        public bool EsCancelableAMEC(string nIDAmec)
        {
            return MiLogicaExpedientes.EsCancelableAMEC(nIDAmec);
        }

        //Ismael Ameller 08-03-2011 Sacamos los Labels de la BBDD
        public string ObtenerLabelEstado(string idEstado)
        {
            return MiLogicaExpedientes.ObtenerLabelEstado(idEstado);
        }

        //FIN Ismael Ameller 08-03-2011 Sacamos los Labels de la BBDD

        public void AgregaProductoExpedienteArea(ExpAreaProductoEmpresa entidad)
        {
            bool a = MiLogicaExpedientes.AgregaProductoExpedienteArea(entidad);
        }


        //Ismael Ameller 23-03-2011 productos por expediente
        public string ProductosExpediente(string IdExp)
        {
            return MiLogicaExpedientes.ProductosExpediente(IdExp);
        }

        public bool CheckJustificanteInscripcion(int idExpediente, int idReserva, int idServicio, int idServicioAlojamiento, int idServicioTransporte)
        {
            return MiLogicaExpedientes.CheckJustificanteInscripcion(idExpediente, idReserva, idServicio, idServicioAlojamiento, idServicioTransporte);
        }

        //FIN Ismael Ameller 23-03-2011 productos por expediente


        public ICollection<DProductoPorcentajeVista> ProductosExpedienteItems(string IdExp)
        {
            return MiLogicaExpedientes.ProductosExpedienteItems(IdExp);
        }


        public void ClearProductosExpediente(int idExp)
        {
            MiLogicaExpedientes.ClearProductosExpediente(idExp);
        }

        public bool EstaIdAreaProductoEmpresaInactivo(string idAreaProductoEmpresa)
        {
            return MiLogicaExpedientes.EstaIdAreaProductoEmpresaInactivo(idAreaProductoEmpresa);
        }

        #region honorarios

        public bool GuardarHonorariosPassenger(int NuevoExpediente, DataTable honorariosCreados)
        {
            return MiLogicaExpedientes.GuardarHonorariosPassenger(NuevoExpediente, honorariosCreados);
        }

        public bool EliminarHonorariosPassenger(int NuevoExpediente, List<int> participantesEliminados)
        {
            return MiLogicaExpedientes.EliminarHonorariosPassenger(NuevoExpediente, participantesEliminados);
        }

        public bool EsEventoInternacional(int idevento)
        {
            return MiLogicaExpedientes.EsEventoInternacional(idevento);
        }

        #endregion

    }
}