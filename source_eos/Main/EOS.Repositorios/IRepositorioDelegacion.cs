using System.Collections.Generic;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;

namespace EOS.Repositorios
{
    public interface IRepositorioDelegacion
    {

        ICollection<DVDelegacion> ObtenerDelegaciones(FiltroDelegacion filtroDelegacion);
        
        long ObtenerNumeroDelegaciones(FiltroDelegacion filtroDelegacion);
        int EliminarDelegacion(int iddelegaprobacion);
        int AprobarDelegacion(FiltroDelegacion filtroDelegacion);
        int ActualizarDelegacion(FiltroDelegacion filtroDelegacion, int iddelegacion);
        int AnularDelegacion(int iddelegaprobacion);
        DVDelegacion ObtenerDelegacionPorID(int iddelegacion);
        int ObtenerDelegacion(int idusuarioDel);
    }
}
