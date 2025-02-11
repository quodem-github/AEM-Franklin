using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;


namespace EOS.Logica
{
    public interface ILogicaFlujoAprobacion
    {

        ICollection<DVFlujoAprobacion> ObtenerFlujosAprobacion(FiltroFlujoAprobacion FiltroFlujoAprob,DVPeticionariosRoles datosRoles);
        long ObtenerNumeroDelegaciones(FiltroFlujoAprobacion FiltroFlujoAprob, DVPeticionariosRoles datosRoles);
        ICollection<DTipoFlujo> ObtenerTipoFlujo();
        ICollection<DTipoActividadFlujo> ObtenerTipoActividad(string idtipoactividad, bool veeva = false);
        ICollection<DTipoRegistroActividadFlujo> ObtenerTipoRegistroActividad(string idtipoactividad, bool veeva = false);        
        DTipoActividadFlujo ObtenerTipoActividadXid(int idactividad);
        ICollection<DNivelesAprobacion> ObtenerNivelesAprobacion();
        DVFlujoAprobacion ObtenerFlujoAprobacionPorID(int idflujo);
        int AprobarFlujoAprobacion(FiltroFlujoAprobacion FiltroFlujo);
        int ActualizarFlujoAprobacion(FiltroFlujoAprobacion FiltroFlujo, int idflujoaprobacion);
        int EliminarFlujoAprobacion(int idflujoaprobacion);
    }
}
