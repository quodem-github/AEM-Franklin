using System.Collections.Generic;
using System.Data;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;

namespace EOS.Repositorios
{
    public interface IRepositorioExpedientes
    {
        DExpediente ObtenerExpediente(string filtroIDExp);

        ICollection<DCabeceraExpedienteAmpliado> ObtenerExpedientes(FiltroExpedientesAvanzado filtro);

        ICollection<DCabeceraExpedienteAmpliado> ObtenerExpedientesTotal(FiltroExpedientesAvanzado filtro, out int count);

        long ObtenerNumeroExpedientes(FiltroExpedientesAvanzado filtro);

        //DCabeceraExpedienteAmpliado ObtenerExpedientePorID(string filtroIdExp);

        ICollection<DCabeceraActividad> ObtenerActividades(FiltroActividades filtro);

        long ObtenerNumeroActividades(FiltroActividades filtro);
        List<DDocumentoVersion> ObtenerDatosDocumentos(string rutaDir);
        ICollection<DCabeceraEForm> ObtenerDatosEventosFormulario(FiltroEForm filtro);

        long ObtenerNumDatosEventosFormulario(FiltroEForm filtro);
        DAmecExpediente ObtenerRelacionAmecExpediente(int idExpediente);
        ICollection<DVAmecCongreso> ObtenerAMECs(FiltroAMECs filtro);

        long ObtenerNumeroAMECs(FiltroAMECs filtro);

        ICollection<DAmec> ObtenerEntidadAMECs(FiltroAMECs filtro);

        int ObtenerSiguienteIdExpediente();

        int ObtenerSiguienteIdPeticionActividad();

        int ObtenerSiguienteIdAMEC();

        string ObtenerSiguienteNombreAMEC(string sCodAgencia);

        /*Pacifico 07012011*/

        List<DDocumentNameTypeSubType> ObtenerRelacionTipoVersion(int idExpediente);
        DVCongresos ObtenerCongreso(int idActividad);

        List<DPassenger> ObtenerPassengers(int idExpediente);

        IEnumerable<DVServicioHotel> ObtenerServiciosHoteles(int idExpediente);

        IEnumerable<DVServicioActividades> ObtenerOtrosServicios(int idExpediente);

        IEnumerable<DVServicioInscripciones> ObtenerServiciosInscripciones(int idExpediente);

        List<string> ObtenerNombresOriginalesDoc(int idExpediente);

        IEnumerable<DVServicioTransporte> ObtenerServiciosTransportes(int idExpediente);

        DVServicioTransporte ObtenerServicioTransporte(int idExpediente);

        ICollection<DVServicioPassengerResumen> ObtenerResumenParticipantes(FiltroExpedientesAvanzado filtro);

        long ObtenerNumeroResumenParticipantes(FiltroExpedientesAvanzado filtro);

        ICollection<DVResumenEstados> ObtenerResumenEstados(string sFiltroRol, bool newQuery);

        int CambiaEstadoExpedienteEnviado(int nIDExpediente);

        int CambiaEstadoExpedienteAprobado(int nIDExpediente, ref int nValidaEstadoAmec, ref int nValidaPresupAmec);

        int CambiaEstadoExpedienteCancelado(int nIDExpediente);

        int CambiaEstadoExpedienteFinalizado(int idexpediente);

        int CambiaEstadoServicio(int nIDReserva, string sIDServicio);

        int CambiaEstadoServicioCancelado(int nIDReserva);

        DEmpGpPetAmecExp ObtenerEmpleadoGpPorPetAmecExp(int idPet, string idAmec, int idExp);

        bool PuedeAprobar(int nIDExpediente);

        string GetEstado(int idexpediente);

        bool EsCancelableAMEC(string nIDAmec);

        int CrearCopiaExpediente(int nIDExpediente, DDatosPersonalesUsuario datosUsuario);

        //Ismael Ameller 08-03-2011 Sacamos los Labels de la BBDD
        string ObtenerLabelEstado(string idEstado);

        //Ismael Ameller 23-03-2011 Recupera productos por expediente
        string ProductosExpediente(string IdExp);

        bool CheckJustificanteInscripcion(int idExpediente, int idReserva, int idServicio, int idServicioAlojamiento, int idServicioTransporte);

        /// <summary>
        /// Obtiene el listado de Productos del expediente.
        /// </summary>
        /// <param name="IdExp">Identificador del expediente.</param>
        /// <returns>Listado de Productos.</returns>
        ICollection<DProductoPorcentajeVista> ProductosExpedienteItems(string IdExp);

        /// <summary>
        /// Elimina los productos contenidos en el expediente.
        /// </summary>
        /// <param name="idExp">Identificador del expediente.</param>
        void ClearProductosExpediente(int idExp);

        ICollection<DVAmecCongresoConcatSolicitante> ObtenerAMECPorCongresoConcatSolicitanteProcedure(int idcongreso, string AmecPorRoles, int amecReuniones, DVPeticionariosRoles roles, int? idconfempresa, bool includeOldAmecs = true, string idAmecsLike = null);

        int NumeroExpedientesPorAmec(string NumAmec);
        ICollection<DExpediente> ExpedientesPorAmec(string NumAmec);

        bool GuardarHonorariosPassenger(int idExpediente, DataTable honorariosCreados);
        bool EliminarHonorariosPassenger(int idExpediente, List<int> participantesEliminados);
        DataSet ObtenerRiskLevel();
        DataSet ObtenerTiposActividadPax();
        DataSet ObtenerTiposAsistente();
        DataSet ObtenerTiposAsistenteConCalculadora();
        DataSet ObtenerJustificaciones();
        bool EsEventoInternacional(int idevento);

        string ObtenerMensajeAMostrar(string tipoActividad, string tipoAsistente, string riskLevel);

        bool EstaIdAreaProductoEmpresaInactivo(string idAreaProductoEmpresa);

    }
}