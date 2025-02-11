using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;

namespace EOS.Logica
{
    public interface ILogicaExpedientes
    {
        ICollection<DCabeceraExpedienteAmpliado> ObtenerExpedientes(FiltroExpedientesAvanzado filtro, DVPeticionariosRoles datosRoles);
        long ObtenerNumeroExpedientes(FiltroExpedientesAvanzado filtro, DVPeticionariosRoles datosRoles);

        ICollection<DCabeceraActividad> ObtenerActividades(FiltroActividades filtro);
        long ObtenerNumeroActividades(FiltroActividades filtro);

        ICollection<DCabeceraEForm> ObtenerDatosEventosFormulario(FiltroEForm filtro);
        long ObtenerNumDatosEventosFormulario(FiltroEForm filtro);

        int NuevoExpediente(DExpediente expediente);
        int NuevaPeticionActividad(DPeticionActividad peticion);
        int NuevoAMEC(DAmec amec);

        ICollection<DVAmecCongreso> ObtenerAMECs(FiltroAMECs filtro, DVPeticionariosRoles datosRoles);
        long ObtenerNumeroAMECs(FiltroAMECs filtro, DVPeticionariosRoles datosRoles);

        ICollection<DAmec> ObtenerEntidadAMECs(FiltroAMECs filtro);

               
        /*Pacifico 07012011*/
        DVCongresos ObtenerCongreso(int idActividad);
        IEnumerable<DVServicioHotel> ObtenerServiciosHoteles(int idExpediente);
        IEnumerable<DVServicioActividades> ObtenerOtrosServicios(int idExpediente);
        IEnumerable<DVServicioInscripciones> ObtenerServiciosInscripciones(int idExpediente);
        IEnumerable<DVServicioTransporte> ObtenerServiciosTransportes(int idExpediente);
        ServicioTransporte ObtenerServicioTransporte(int idExpediente);

        long ObtenerNumeroResumenParticipantes(FiltroExpedientesAvanzado filtro);
        ICollection<DVResumenEstados> ObtenerResumenEstados(DVPeticionariosRoles datosRoles);

        int CambiaEstadoExpedienteEnviado(int nIDExpediente);
        int CambiaEstadoExpedienteAprobado(int nIDExpediente, ref int nValidaEstadoAmec, ref int nValidaPresupAmec);
        int CambiaEstadoExpedienteCancelado(int nIDExpediente);
        int CambiaEstadoExpedienteFinalizado(int idexpediente);
        int CambiaEstadoServicioSinEnviar(int nIDReserva);
        int CambiaEstadoServicioAceptado(int nIDReserva);
        int CambiaEstadoServicioCancelado(int nIDReserva);
        bool PuedeAprobar(int nIDExpediente);
        string GetEstado(int idexpediente);
        DEmpGpPetAmecExp ObtenerEmpleadoGpPorPetAmecExp(int idPet, string idAmec, int idExp);

        int CrearCopiaExpediente(int nIDExpediente, DDatosPersonalesUsuario datosUsuario);

        string ObtenerSiguienteNombreAMEC(string sCodAgencia);
        int AprobarAMEC(string nIDAmec, int nTipoAprobador, string sObservaciones);
        bool EsCancelableAMEC(string nIDAmec);

        ICollection<DVAmecCongresoConcatSolicitante> ObtenerAMECPorCongresoConcatSolicitante(int idcongreso, string AmecPorRoles, int amecReuniones,  DVPeticionariosRoles roles, int? idconfempresa, bool includeOldAmecs = true, string idAmecsLike = null);


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

        bool CheckJustificanteInscripcion(int idExpediente, int idReserva, int idServicio, int idServicioAlojamiento, int idServicioTransporte);
    }
}
