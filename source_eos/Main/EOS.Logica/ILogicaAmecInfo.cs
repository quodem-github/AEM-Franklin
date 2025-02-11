using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using System.Data;

namespace EOS.Logica
{
    public interface ILogicaAmecInfo
    {
       //ICollection<DAmecInfo> ObtenerAMECs(FiltroAmecInfo filtro, DVPeticionariosRoles datosRoles);
       //long ObtenerNumeroAMECs(FiltroAmecInfo filtro, DVPeticionariosRoles datosRoles);
        int ObtenerNumPendienteSometer(string idpeticionario);

        string ObtenerNombreTipoRiesgo(string idtiporiesgo);
        string ObtenerNombreTipoActividad(string idtipoactividad);

        DataSet ObtenerInformeFCPA(string idamec, string nombrePrograma, string estado, string fechaInicio, string fechaFin, string unidad, string area,
                                    string region, string distrito, string year, string mes, string iddistrict, string idsaleforce, string iddepartament);
        int guardarEnInformeFCPA(string idamecs, string tipoactividad, string descripcion, DateTime? fechacomienzo,
           DateTime? fechafinalizacion, string idestado, string usuario, string cargo, string idunidad, string idarea, string idregion,
           string iddistrito, string tiporiesgo, int idtiporiesgo, string nombrePax, string apellido1Pax, string msdid, string tiporeservacolectivo, string idexpediente, int? iddepartament, int? iddistrict, int? idsaleforce, int? idposition);

        IList<string> ObtenerNombreUnidadOrganizativa(string idunidad, string idarea, string idregion, string iddistrito);

        DAmecInfo NuevoAMEC(DAmecInfo amec);
        DAmecInfo NuevoAMECNewCo(DAmecInfo amec);

        DAmecInfo ModificarAMEC(DAmecInfo amec, out bool cambioAgencia, bool updateFechaComienzoFinalizacion);
        int CambiarEstadoAmec(string idestado, string idamecs, string idpeticionario, string idaprobador);
        int CambiarEstadoAmec(string idestado, string idamecs, int nivelaprobacion = -1);
        int BorrarHistorialAmec(int idestado, string idamecs, int idPeticionario, int nivelaprobacion);

        int CrearRelacionAmecCongreso(int idcongres, string idamecs, int idpeticionario);
        int EliminarRelacionAmecCongreso(int idameccongreso);

        int InsertarHistorialAmec(int idestado, string idamecs, DDatosPersonalesUsuario peticionario, int nivelaprobacion, int? idAprobador);
        DataSet ObtenerDocumentacionAdicionalAmec(string idamec);

        ICollection<DCongresos> ObtenerEventosNoRelacionadosAMEC(FiltroAMECCongreso filtroAMECCongreso);
        long ObtenerNumeroEventosNoRelacionadosAMEC(FiltroAMECCongreso filtroAMECCongreso);

        DataSet DataSetObtenerActividadesAsigAmec(string IdAMEC, string sortParameter, int startRowIndex, int maximumRows);
        ICollection<DVCongresoAmec> ObtenerActividadesAsigAmec(string IdAMEC, string sortParameter, int startRowIndex, int maximumRows);
        long ObtenerNumeroActividadesAsigAmec(string IdAMEC);

        List<ListadoAmecs> ObtenerNumeroListadoAMECsProcedureAux(FiltroListadoAMECs filtro,DVPeticionariosRoles datosRoles);

        int EventoYaEstaAsignadoAmec(string idamec, int idcongreso);
        int IdAmecAsignadoCongreso(string idamec, int idcongreso);
        //Imprimir
        DAmecInfo CargarTodosValoresAmec(string idamec);
        DataSet CargarTodosValoresAmecExcel(string idamec);
        ICollection<DCongresos> CargarEventoActividadesRelacionadasAmec(string idamec);
        //
        string ObtenerNombreEstado(UInt32 idestado);

        ICollection<DEstadoAmec> ObtenerEstadosAmec();
        uint ObtenerEstadoAmec(string idamec);
        ICollection<DAmecInfo> ObtenerAMECsInfo(FiltroAmecInfo filtroaMEC, DVPeticionariosRoles datosRoles);
        long ObtenerNumeroAMECsInfo(FiltroAmecInfo filtroaMEC, DVPeticionariosRoles datosRoles);

        ICollection<DHistEstadosAMEC> ObtenerHistorialEstadosAMEC(string idamec, string sortParameter, int startRowIndex, int maximumRows);
        long ObtenerNumeroHistorialEstadosAMEC(string idamec);
        int GuardarEstadoAHistorialAMEC(string idamec, int idestado, int idcreadopor, DAmecInfo miAmec);

        string DameSiguienteIdAmecs();

        int GuardaDocumentacion(DDocumentacionAmec DocAmec);
        int ObtenerNumeroDocumentacionAMEC(string idamec);
        ICollection<DDocumentacionAmec> ObtenerDocumentacionAMEC(string idamec, string sortParameter, int startRowIndex, int maximumRows);
        int EliminarDocumentacion(int iddocumentacion);
        DataSet ObtenerCategoriasDocumento();

        ICollection<ListadoAmecs> ObtenerListadoAMECs(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles, out int count);
        int ObtenerNumeroListadoAMECs(FiltroListadoAMECs filtro, DVPeticionariosRoles datosRoles);

        DAmecInfo ObtenerProgramaAMEC(string idamec, string sortParameter,int startRowIndex,int maximumRows);
        int ObtenerNumeroProgramaAMEC(string idamec);

        bool EstaGuardadoAmec(string idamec);
        int HayAmecRelacionExpediente(string idamec);

        ICollection<String> ObtenerCorreoParaEnviar(string idamecs, string connectionString);

        DataSet MailsAEnviarCuandoAprobado(string IdAMEC);
        DataSet BUDdelaUnidad(string IdAMEC);

        DVCongresoAmec ObtenerCongreso(int idcongreso);

        bool EsGestorArchivo(int nIdPeticionario);

        int GuardaLogMail(string idamecs, string tipo_mail, string message_to, string message_subject, string message_body, string message_fileattach, bool envioCorrecto, string error);
        
        bool ComprobarSiSeEnvioAFarma(string idamec);
        bool ComprobarSiSeEnvioACasosClinicos(string idamec);

    }
}
