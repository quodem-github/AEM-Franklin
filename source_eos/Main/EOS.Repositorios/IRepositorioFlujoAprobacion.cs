using System.Collections.Generic;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;


namespace EOS.Repositorios
{
    public interface IRepositorioFlujoAprobacion
    {
        ICollection<DVFlujoAprobacion> ObtenerFlujosAprobacion(FiltroFlujoAprobacion FiltroFlujoAprob, DVPeticionariosRoles datosRoles);
        long ObtenerNumeroFlujosAprobacion(FiltroFlujoAprobacion FiltroFlujoAprob);
        ICollection<DTipoFlujo> ObtenerTipoFlujo();
        ICollection<DTipoActividadFlujo> ObtenerTipoActividad(string idtipoactividad, bool veeva = false);
        ICollection<DTipoActividadFlujo> ObtenerTipoActividadPorTipoEvento(string idtipoevento);
        ICollection<DTipoRegistroActividadFlujo> ObtenerTipoRegistroActividad(string idtipoactividad, bool veeva = false);
        ICollection<DTipoRegistroActividadFlujo> ObtenerTipoRegistroActividadNewCo(string idtipoactividad);
        DTipoActividadFlujo ObtenerTipoActividadXid(int idactividad);
        ICollection<DNivelesAprobacion> ObtenerNivelesAprobacion();
        DVFlujoAprobacion ObtenerFlujoAprobacionPorID(int idflujo);
        int AprobarFlujoAprobacion(FiltroFlujoAprobacion FlujoAprobacion);
        int ActualizarFlujoAprobacion(FiltroFlujoAprobacion FiltroFlujo, int idflujoaprobacion);
        int EliminarFlujoAprobacion(int idflujoaprobacion);
    }
}
