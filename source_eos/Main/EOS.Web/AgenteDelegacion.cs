using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Logica;

namespace EOS.Web
{
    public class AgenteDelegacion : AgenteBase
    {
        public LogicaDelegacion MiLogicaDelegacion { get; set; }
        public AgenteDelegacion()
        {
            MiLogicaDelegacion = new LogicaDelegacion();
        }

        
        public ICollection<DVDelegacion> ObtenerDelegacionesTotal()
        {

            return null;
        }

        public int ObtenerDelegacion(int idusuarioDel)
        {
            return MiLogicaDelegacion.ObtenerDelegacion(idusuarioDel);
        }

        public int AnularDelegacion(int iddelegaprobacion)
        {

            return MiLogicaDelegacion.AnularDelegacion(iddelegaprobacion);
 

        }


        public int ActualizarDelegacion(string filtroiddelegado, string filtroidusuario, string filtrofechadesde, string filtrofechahasta, int iddelegacion, int idcreadopor)
        {

            FiltroDelegacion filtroDelegacion =
                new FiltroDelegacion()
                {

                    IdUsuario = string.IsNullOrEmpty(filtroidusuario) ? new Nullable<int>() : int.Parse(filtroidusuario),
                    IdDelegado = string.IsNullOrEmpty(filtroiddelegado) ? new Nullable<int>() : int.Parse(filtroiddelegado),
                    FechaDesde = string.IsNullOrEmpty(filtrofechadesde) ? new Nullable<DateTime>() : DateTime.Parse(filtrofechadesde),
                    FechaHasta = string.IsNullOrEmpty(filtrofechahasta) ? new Nullable<DateTime>() : DateTime.Parse(filtrofechahasta),
                    idcreadopor = idcreadopor
                    //TODO ---> Falta Poner el Estado
                };

            int aprobat = MiLogicaDelegacion.ActualizarDelegacion(filtroDelegacion, iddelegacion);
            return aprobat;
        }


        public int AprobarDelegacion(string filtroiddelegado, string filtroidusuario, string filtrofechadesde, string filtrofechahasta, int idcreadopor)
        {

            FiltroDelegacion filtroDelegacion =
                new FiltroDelegacion()
                {
                    
                    IdUsuario = string.IsNullOrEmpty(filtroidusuario) ? new Nullable<int>() : int.Parse(filtroidusuario),
                    IdDelegado = string.IsNullOrEmpty(filtroiddelegado) ? new Nullable<int>() : int.Parse(filtroiddelegado),
                    FechaDesde = string.IsNullOrEmpty(filtrofechadesde) ? new Nullable<DateTime>() : DateTime.Parse(filtrofechadesde),
                    FechaHasta = string.IsNullOrEmpty(filtrofechahasta) ? new Nullable<DateTime>() : DateTime.Parse(filtrofechahasta),
                    idcreadopor = idcreadopor
                    //TODO ---> Falta Poner el Estado
                };

            int aprobat = MiLogicaDelegacion.AprobarDelegacion(filtroDelegacion);
            return aprobat;
        }
        public DVDelegacion ObtenerDelegacionPorID(int iddelegacion)
        {

            DVDelegacion DelegacionPorID = MiLogicaDelegacion.ObtenerDelegacionPorID(iddelegacion);
        return DelegacionPorID;
        }
        public ICollection<DVDelegacion> ObtenerDelegacionesTotal(string filtroiddelegado, string filtroidusuario, string filtrofechadesde, string filtrofechahasta,string filtroestado, string sortParameter, int startRowIndex, int maximumRows)
        {

            FiltroDelegacion filtroDelegacion =
                new FiltroDelegacion()
                {

                    IdUsuario = string.IsNullOrEmpty(filtroidusuario) ? new Nullable<int>() : int.Parse(filtroidusuario),
                    IdDelegado = string.IsNullOrEmpty(filtroiddelegado) ? new Nullable<int>() : int.Parse(filtroiddelegado),
                    FechaDesde = string.IsNullOrEmpty(filtrofechadesde) ? new Nullable<DateTime>() : DateTime.Parse(filtrofechadesde),
                    FechaHasta = string.IsNullOrEmpty(filtrofechahasta) ? new Nullable<DateTime>() : DateTime.Parse(filtrofechahasta),
                    Estado = string.IsNullOrEmpty(filtroestado) ? "" : filtroestado,
                    SortParameter = sortParameter,
                    StartRowIndex = startRowIndex,
                    MaximumRows = maximumRows

                };

            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();
            ICollection<DVDelegacion> collectionDelegacion = MiLogicaDelegacion.ObtenerDelegaciones(filtroDelegacion, datosRoles);
            return collectionDelegacion;
        
        }

        public int ObtenerNumeroDelegaciones(string filtroiddelegado, string filtroidusuario, string filtrofechadesde, string filtrofechahasta, string filtroestado)
        {
            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();
            FiltroDelegacion filtroDelegacion =
               new FiltroDelegacion()
               {

                   IdUsuario = string.IsNullOrEmpty(filtroidusuario) ? new Nullable<int>() : int.Parse(filtroidusuario),
                   IdDelegado = string.IsNullOrEmpty(filtroiddelegado) ? new Nullable<int>() : int.Parse(filtroiddelegado),
                   FechaDesde = string.IsNullOrEmpty(filtrofechadesde) ? new Nullable<DateTime>() : DateTime.Parse(filtrofechadesde),
                   FechaHasta = string.IsNullOrEmpty(filtrofechahasta) ? new Nullable<DateTime>() : DateTime.Parse(filtrofechahasta),
                   Estado = string.IsNullOrEmpty(filtroestado) ? "" : filtroestado
               };
            long numDelegaciones = MiLogicaDelegacion.ObtenerNumeroDelegaciones(filtroDelegacion, datosRoles);
            return Convert.ToInt32(numDelegaciones);
        }

        private DVPeticionariosRoles ObtenerDatosRoles()
        {
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            return agenteUsu.ObtenerDatosRolesPorLogin();
        }
    }
}
