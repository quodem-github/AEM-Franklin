using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Entidades.Mapeadores;
using EOS.Repositorios;

namespace EOS.Logica
{
    public class LogicaFlujoAprobacion : ILogicaFlujoAprobacion
    {

        private IRepositorioFlujoAprobacion MiRepositorioFlujoAprobacion { get; set; }

        public LogicaFlujoAprobacion()
        {
            MiRepositorioFlujoAprobacion = new RepositorioFlujoAprobacion();
        }

        public ICollection<DVFlujoAprobacion> ObtenerFlujosAprobacion(FiltroFlujoAprobacion FiltroFlujoAprob, DVPeticionariosRoles datosRoles)
        {
            return MiRepositorioFlujoAprobacion.ObtenerFlujosAprobacion(FiltroFlujoAprob, datosRoles);
        }

        public long ObtenerNumeroDelegaciones(FiltroFlujoAprobacion FiltroFlujoAprob, DVPeticionariosRoles datosRoles)
        {
            return MiRepositorioFlujoAprobacion.ObtenerNumeroFlujosAprobacion(FiltroFlujoAprob);
        }

        public DVFlujoAprobacion ObtenerFlujoAprobacionPorID(int idflujo)
        {

            return MiRepositorioFlujoAprobacion.ObtenerFlujoAprobacionPorID(idflujo);
        }
        public int AprobarFlujoAprobacion(FiltroFlujoAprobacion FiltroFlujo)
        {
            return MiRepositorioFlujoAprobacion.AprobarFlujoAprobacion(FiltroFlujo);
        }

        public int ActualizarFlujoAprobacion(FiltroFlujoAprobacion FiltroFlujo, int idflujoaprobacion)
        {
            return MiRepositorioFlujoAprobacion.ActualizarFlujoAprobacion(FiltroFlujo, idflujoaprobacion);
        }

        public ICollection<DTipoFlujo> ObtenerTipoFlujo()
        {

            return MiRepositorioFlujoAprobacion.ObtenerTipoFlujo();
        }
        
        
        public int EliminarFlujoAprobacion(int idflujoaprobacion)
        { 
        return MiRepositorioFlujoAprobacion.EliminarFlujoAprobacion(idflujoaprobacion);
        
        }
        public ICollection<DTipoActividadFlujo> ObtenerTipoActividad(string idtipoactividad, bool veeva = false)
        {

            return MiRepositorioFlujoAprobacion.ObtenerTipoActividad(idtipoactividad, veeva);
        
        }

        public ICollection<DTipoActividadFlujo> ObtenerTipoActividadPorTipoEvento(string idtipoevento)
        {

            return MiRepositorioFlujoAprobacion.ObtenerTipoActividadPorTipoEvento(idtipoevento);

        }

        public ICollection<DTipoRegistroActividadFlujo> ObtenerTipoRegistroActividad(string idtipoactividad, bool veeva = false)
        {

            return MiRepositorioFlujoAprobacion.ObtenerTipoRegistroActividad(idtipoactividad, veeva);

        }

        public ICollection<DTipoRegistroActividadFlujo> ObtenerTipoRegistroActividadNewCo(string idtipoactividad)
        {

            return MiRepositorioFlujoAprobacion.ObtenerTipoRegistroActividadNewCo(idtipoactividad);

        }

        public DTipoActividadFlujo ObtenerTipoActividadXid(int idactividad)
        {
            return MiRepositorioFlujoAprobacion.ObtenerTipoActividadXid(idactividad);        
        }

        public ICollection<DNivelesAprobacion> ObtenerNivelesAprobacion()
        {
            return MiRepositorioFlujoAprobacion.ObtenerNivelesAprobacion();
        }
    }
}
