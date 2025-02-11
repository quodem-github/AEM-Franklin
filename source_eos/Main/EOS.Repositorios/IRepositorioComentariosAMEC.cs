using System.Collections.Generic;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;

namespace EOS.Repositorios
{
    public interface IRepositorioComentariosAMEC
    {
        
        ICollection<DComentariosAmec> ObtenerComentariosAMEC(FiltroComentariosAMEC filtroUnidades);
        int InsertComentarioAMEC(FiltroComentariosAMEC filtroUnidades);
        long ObtenerNumeroComentariosAMEC(FiltroComentariosAMEC filtroUnidades);
        int EliminarComentarioAMEC(long idunidadamec);
        int ComprobarComentarioEsTuyo(int idcomentario, int idpeticionario);
    }
}
