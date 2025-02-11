using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;

namespace EOS.Logica
{
    public interface ILogicaDelegacion
    {
        long ObtenerNumeroDelegaciones(FiltroDelegacion filtroDelegacion, DVPeticionariosRoles datosRoles);
        ICollection<DVDelegacion> ObtenerDelegaciones(FiltroDelegacion filtroDelegacion, DVPeticionariosRoles datosRoles);
        int AprobarDelegacion(FiltroDelegacion filtroDelegacion);
        int ActualizarDelegacion(FiltroDelegacion filtroDelegacion, int iddelegacion);
        int AnularDelegacion(int iddelegaprobacion);
        DVDelegacion ObtenerDelegacionPorID(int iddelegacion);
    }
}
