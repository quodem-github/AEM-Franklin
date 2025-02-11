using System.Collections.Generic;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using System.Data;
using System;

namespace EOS.Repositorios
{
    public interface IRepositorioAmecInfo
    {
        int ObtenerNumPendienteSometer(string idpeticionario);
        // int ObtenerSiguienteIdAMEC();
        string DameSiguienteIdAmecs();
        // string ObtenerSiguienteNombreAMEC(string sCodAgencia);

        string ObtenerNombreTipoRiesgo(string idtiporiesgo);
        string ObtenerNombreTipoActividad(string idtipoactividad);

        DataSet ObtenerInformeFCPA(string idamec, string nombrePrograma, string estado, string fechaInicio, string fechaFin, string unidad, string area,
                                     string region, string distrito, string year, string mes, string iddistrict, string idsaleforce, string iddepartament);

        int guardarEnInformeFCPA(string idamecs, string tipoactividad, string descripcion, DateTime? fechacomienzo,
                            DateTime? fechafinalizacion, string idestado, string usuario, string cargo, string idunidad, string idarea, string idregion,
                            string iddistrito, string tiporiesgo, int idtiporiesgo, string nombrePax, string apellido1Pax, string msdid, string tiporeservacolectivo, string idexpediente, int? iddepartament, int? iddistrict, int? idsaleforce, int? idposition);

        IList<string> ObtenerNombreUnidadOrganizativa(string idunidad, string idarea, string idregion, string iddistrito);

        DAmecInfo NuevoAmec(DAmecInfo amec);
        DAmecInfo NuevoAmecNewCo(DAmecInfo amec);
        DAmecInfo ClonarAmec(DAmecInfo amec, string idAmecOriginal);
        int CambiarEstadoAmec(string idestado, string idamecs, string idpeticionario, string idaprobador);
        int CambiarEstadoAmec(string idestado, string idamecs, int nivelaprobacion = -1);

        int CrearRelacionAmecCongreso(int idcongres, string idamecs, int idpeticionario);
        int EliminarRelacionAmecCongreso(int idameccongreso);

        DataSet ObtenerDocumentacionAdicionalAmec(string idamec);

        int InsertarHistorialAmec(int idestado, string idamecs, DDatosPersonalesUsuario peticionario, int nivelaprobacion, int? idAprobador);
        int BorrarHistorialAmec(int idestado, string idamecs, int idPeticionario, int nivelaprobacion);
        ICollection<DCongresos> ObtenerEventosNoRelacionadosAMEC(FiltroAMECCongreso filtroAMECCongreso);
        long ObtenerNumeroEventosNoRelacionadosAMEC(FiltroAMECCongreso filtroAMECCongreso);
        List<ListadoAmecs> ObtenerNumeroListadoAMECsProcedureAux(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles);
        bool TieneExpedientesAsociados(int idamec);
        bool TieneExpedientesAsociadosPorIdAmecs(string idamecs);

        DataSet DataSetObtenerActividadesAsigAmec(string IdAMEC, string sortParameter, int startRowIndex, int maximumRows);
        ICollection<DVCongresoAmec> ObtenerActividadesAsigAmec(string IdAMEC, string sortParameter, int startRowIndex, int maximumRows);
        long ObtenerNumeroActividadesAsigAmec(string IdAMEC);

        int EventoYaEstaAsignadoAmec(string idamec, int idcongreso);
        int IdAmecAsignadoCongreso(string idamec, int idcongreso);
        DAmecInfo ModificarAMEC(DAmecInfo amec, out bool cambioAgencia, bool updateFechaComienzoFinalizacion);

        //Imprimir
        DAmecInfo CargarTodosValoresAmec(string idamec);
        DataSet CargarTodosValoresAmecExcel(string idamec);
        ICollection<DCongresos> CargarEventoActividadesRelacionadasAmec(string idamec);
        //

        string ObtenerNombreEstado(uint idestado);

        ICollection<DEstadoAmec> ObtenerEstadosAmec();
        uint ObtenerEstadoAmec(string idamec);
        ICollection<DAmecInfo> ObtenerAMECsInfo(FiltroAmecInfo filtroaMEC, DVPeticionariosRoles datosRoles);
        long ObtenerNumeroAMECsInfo(FiltroAmecInfo filtroaMEC, DVPeticionariosRoles datosRoles);


        ICollection<DHistEstadosAMEC> ObtenerHistorialEstadosAMEC(string idamec, string sortParameter, int startRowIndex, int maximumRows);
        long ObtenerNumeroHistorialEstadosAMEC(string idamec);
        int GuardarEstadoAHistorialAMEC(string idamec, int idestado, int idcreadopor, DAmecInfo miAmec);

        int GuardaDocumentacion(DDocumentacionAmec DocAmec);
        int ObtenerNumeroDocumentacionAMEC(string iddocumentacion);
        ICollection<DDocumentacionAmec> ObtenerDocumentacionAMEC(string idamec, string sortParameter, int startRowIndex, int maximumRows);
        int EliminarDocumentacion(int iddocumentacion);
        DataSet ObtenerCategoriasDocumento();

        ICollection<ListadoAmecs> ObtenerListadoAMECs(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles);
        int ObtenerNumeroListadoAMECs(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles);

        ICollection<ListadoAmecs> ObtenerListadoAMECsProcedure(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles, out int count);
        int ObtenerNumeroListadoAMECsProcedure(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles);

        DAmecInfo ObtenerProgramaAMEC(string idamec, string sortParameter, int startRowIndex, int maximumRows);
        int ObtenerNumeroProgramaAMEC(string idamec);

        bool EstaGuardadoAmec(string idamec);

        DataSet MailsAEnviarCuandoAprobado(string IdAMEC);
        DataSet BUDdelaUnidad(string IdAMEC);

        int HayAmecRelacionExpediente(string idamec);

        ICollection<string> ObtenerCorreoParaEnviar(string idAMEC, string connectionString);

        DVCongresoAmec ObtenerCongreso(int idcongreso);

        bool EsGestorArchivo(int nIdPeticionario);


        int GuardaLogMail(string idamecs, string tipo_mail, string message_to, string message_subject, string message_body, string message_fileattach, bool envioCorrecto, string error);

        bool ComprobarSiSeEnvioAFarma(string idamec);
        bool ComprobarSiSeEnvioACasosClinicos(string idamec);

    }
}
