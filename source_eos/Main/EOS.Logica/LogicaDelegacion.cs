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
    public class LogicaDelegacion : ILogicaDelegacion
    {
        private IRepositorioDelegacion MiRepositorioDelegacion { get; set; }
        
        public LogicaDelegacion()
        {
            MiRepositorioDelegacion = new RepositorioDelegacion();
        }

        public int AnularDelegacion(int iddelegaprobacion)
        {
            return MiRepositorioDelegacion.AnularDelegacion(iddelegaprobacion);
        }

        public int AprobarDelegacion(FiltroDelegacion filtroDeleg)
        {
            return MiRepositorioDelegacion.AprobarDelegacion(filtroDeleg);
        }

        public int ActualizarDelegacion(FiltroDelegacion filtroDeleg, int iddelegacion)
        {
            return MiRepositorioDelegacion.ActualizarDelegacion(filtroDeleg, iddelegacion);
        }

        public int ObtenerDelegacion(int idusuarioDel)
        {
            return MiRepositorioDelegacion.ObtenerDelegacion(idusuarioDel);
        }

        public DVDelegacion ObtenerDelegacionPorID(int iddelegacion)
        {
            return MiRepositorioDelegacion.ObtenerDelegacionPorID(iddelegacion);
        
        }
        public ICollection<DVDelegacion> ObtenerDelegaciones (FiltroDelegacion filtroDelegacion , DVPeticionariosRoles datosRoles)
        {
            //Aki haig de treure l'usuari que en aquest moment demana les seves delegacions
            return MiRepositorioDelegacion.ObtenerDelegaciones(filtroDelegacion);
        }

        public long ObtenerNumeroDelegaciones(FiltroDelegacion filtroDelegacion , DVPeticionariosRoles datosRoles)
        {
            //filtro.Roles = ObtenerFiltroRol(datosRoles, "exp");
            return MiRepositorioDelegacion.ObtenerNumeroDelegaciones(filtroDelegacion); 
        }

    }
}
