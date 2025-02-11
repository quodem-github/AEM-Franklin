using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;

namespace EOS.Logica
{
    public  interface ILogicaComentariosAMEC
    {
        int InsertComentarioAMEC(FiltroComentariosAMEC filtroComentario, DVPeticionariosRoles datosRoles);
        ICollection<DComentariosAmec> ObtenerComentariosAMEC(FiltroComentariosAMEC filtroComentario, DVPeticionariosRoles datosRoles);
        long ObtenerNumeroComentariosAMEC(FiltroComentariosAMEC filtroComentario, DVPeticionariosRoles datosRoles);
        int EliminarComentarioAMEC(long idunidadamec, DVPeticionariosRoles datosRoles);
        int ComprobarComentarioEsTuyo(int idcomentario, int idpeticionario);
    }
}
