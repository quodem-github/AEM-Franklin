using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Logica;

namespace EOS.Web
{
    public class AgenteFlujoAprobacion : AgenteBase
    {
        public LogicaFlujoAprobacion MiLogicaFlujoAprobacion { get; set; }
        public AgenteFlujoAprobacion()
        {
            MiLogicaFlujoAprobacion = new LogicaFlujoAprobacion();
        }

        public ICollection<DVFlujoAprobacion> ObtenerFlujosAprobacion(string filtroidactividad, string filtroidtipoflujo, string sortParameter, int startRowIndex, int maximumRows)
        {
            //Crear un filtre amb les dades de moment només he creat un filtreflujoaprobacion amb les dades que necessito per fer el filtre 
            FiltroFlujoAprobacion FiltroFlujoAprob = new FiltroFlujoAprobacion 
            {
                idTipoActividad = string.IsNullOrEmpty(filtroidactividad) ? new Nullable<int>() : int.Parse(filtroidactividad),
                idTipoFlujo = string.IsNullOrEmpty(filtroidtipoflujo) ? new Nullable<int>() : int.Parse(filtroidtipoflujo),
                MaximumRows = maximumRows,
                SortParameter = sortParameter,
                StartRowIndex = startRowIndex
            };
            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();
            return MiLogicaFlujoAprobacion.ObtenerFlujosAprobacion(FiltroFlujoAprob, datosRoles);

        }

        public int ObtenerNumeroFlujosAprobacion(string filtroidactividad, string filtroidtipoflujo)
        {

            FiltroFlujoAprobacion FiltroFlujoAprob = new FiltroFlujoAprobacion
            {
                idTipoActividad = string.IsNullOrEmpty(filtroidactividad) ? new Nullable<int>() : int.Parse(filtroidactividad),
                idTipoFlujo = string.IsNullOrEmpty(filtroidtipoflujo) ? new Nullable<int>() : int.Parse(filtroidtipoflujo),
            };
        DVPeticionariosRoles datosRoles = ObtenerDatosRoles();
        long numFlujoAprobacion = MiLogicaFlujoAprobacion.ObtenerNumeroDelegaciones(FiltroFlujoAprob, datosRoles);
        return Convert.ToInt32(numFlujoAprobacion);
        }

        public DVFlujoAprobacion ObtenerFlujoAprobacionPorID(int idflujo)
        {

            DVFlujoAprobacion DelegacionPorID = MiLogicaFlujoAprobacion.ObtenerFlujoAprobacionPorID(idflujo);
            return DelegacionPorID;
        }

        public int AprobarFlujoAprobacion(string idtipoFlujo, string idtipoActi, string orden, string fase, string importe, string idNivelInicial, string idNivelSiguiente, string PreAprobado, string idCondicionado, int idcreadopor)
        {
            if (PreAprobado == "Selecciona Tipo de PreAprobación") PreAprobado = "";
            //Comprovar quin tipus de preaprobat ha 
            FiltroFlujoAprobacion FiltroFlujo = new FiltroFlujoAprobacion()
            {
                idTipoActividad = string.IsNullOrEmpty(idtipoActi) ? new Nullable<int>() : int.Parse(idtipoActi),
                idTipoFlujo = string.IsNullOrEmpty(idtipoFlujo) ? new Nullable<int>() : int.Parse(idtipoFlujo),
                orden = string.IsNullOrEmpty(orden) ? new Nullable<int>() : int.Parse(orden),
                fase = string.IsNullOrEmpty(fase) ? "" : fase,
                importe = string.IsNullOrEmpty(importe) ? 0 : double.Parse(importe),
                idNivelInicial = string.IsNullOrEmpty(idNivelInicial) ? new Nullable<int>() : int.Parse(idNivelInicial),
                idNivelSiguiente = string.IsNullOrEmpty(idNivelSiguiente) ? new Nullable<int>() : int.Parse(idNivelSiguiente),
                PreAprobado = string.IsNullOrEmpty(PreAprobado) ? "" : PreAprobado,
                condicionado = string.IsNullOrEmpty(idCondicionado) ? new Nullable<int>() : int.Parse(idCondicionado),
                idcreadopor = idcreadopor
            };

            int aprobat = MiLogicaFlujoAprobacion.AprobarFlujoAprobacion(FiltroFlujo);
            return aprobat;
        }


        public int ActualizarFlujoAprobacion(string idtipoFlujo, string idtipoActi, string orden, string fase, string importe, string idNivelInicial, string idNivelSiguiente, string PreAprobado, string idCondicionado, int idflujoaprobacion, int idcreadopor)
        {

            //Comprovar quin tipus de preaprobat ha 
            FiltroFlujoAprobacion FiltroFlujo = new FiltroFlujoAprobacion()
            {
                idTipoActividad = string.IsNullOrEmpty(idtipoFlujo) ? new Nullable<int>() : int.Parse(idtipoActi),
                idTipoFlujo = string.IsNullOrEmpty(idtipoFlujo) ? new Nullable<int>() : int.Parse(idtipoFlujo),
                orden = string.IsNullOrEmpty(orden) ? new Nullable<int>() : int.Parse(orden),
                fase = string.IsNullOrEmpty(fase) ? "" : fase,
                importe = string.IsNullOrEmpty(importe) ? new Nullable<double>() : double.Parse(importe),
                idNivelInicial = string.IsNullOrEmpty(idNivelInicial) ? new Nullable<int>() : int.Parse(idNivelInicial),
                idNivelSiguiente = string.IsNullOrEmpty(idNivelSiguiente) ? new Nullable<int>() : int.Parse(idNivelSiguiente),
                PreAprobado = string.IsNullOrEmpty(PreAprobado) ? "" : PreAprobado,
                condicionado = string.IsNullOrEmpty(idCondicionado) ? new Nullable<int>() : int.Parse(idCondicionado),
                idcreadopor = idcreadopor
            };

            int aprobat = MiLogicaFlujoAprobacion.ActualizarFlujoAprobacion(FiltroFlujo, idflujoaprobacion);
            return aprobat;
        }
        public int EliminarFlujoAprobacion(int idflujoaprobacion)
        {
            return MiLogicaFlujoAprobacion.EliminarFlujoAprobacion(idflujoaprobacion);
        }
        private DVPeticionariosRoles ObtenerDatosRoles()
        {
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            return agenteUsu.ObtenerDatosRolesPorLogin();
        }

        public ICollection<DTipoFlujo> ObtenerTipoFlujo()
        {
            return MiLogicaFlujoAprobacion.ObtenerTipoFlujo();
        }
                     
        public ICollection<DTipoActividadFlujo> ObtenerTipoActividadPorTipoEvento(string idtipoevento)
        {
            return MiLogicaFlujoAprobacion.ObtenerTipoActividadPorTipoEvento(idtipoevento);
        }

        public ICollection<DTipoActividadFlujo> ObtenerTipoActividad(string idtipoactividad, bool veeva = false)
        {
            return MiLogicaFlujoAprobacion.ObtenerTipoActividad(idtipoactividad, veeva);
        }

        public ICollection<DTipoRegistroActividadFlujo> ObtenerTipoRegistroActividad(string idtipoactividad, bool veeva = false)
        {
            return MiLogicaFlujoAprobacion.ObtenerTipoRegistroActividad(idtipoactividad, veeva);
        }

        public ICollection<DTipoRegistroActividadFlujo> ObtenerTipoRegistroActividadNewCo(string idtipoactividad)
        {
            return MiLogicaFlujoAprobacion.ObtenerTipoRegistroActividadNewCo(idtipoactividad);
        }

        public DTipoActividadFlujo ObtenerTipoActividadXid(int idactividad)
        {
            return MiLogicaFlujoAprobacion.ObtenerTipoActividadXid(idactividad);
        }

        public ICollection<DNivelesAprobacion> ObtenerNivelesAprobacion()
        {
            return MiLogicaFlujoAprobacion.ObtenerNivelesAprobacion();
        }

    }
}
