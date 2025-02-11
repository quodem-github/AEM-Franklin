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
    public class LogicaComentariosAMEC : ILogicaComentariosAMEC
    {
        private IRepositorioComentariosAMEC MiRepositorioComentarios { get; set; }

        public LogicaComentariosAMEC()
        {
            MiRepositorioComentarios = new RepositorioComentariosAMEC();
        }

        public int InsertComentarioAMEC(FiltroComentariosAMEC filtroComentarios, DVPeticionariosRoles datosRoles)
        {
            return MiRepositorioComentarios.InsertComentarioAMEC(filtroComentarios);
        }

        public ICollection<DComentariosAmec> ObtenerComentariosAMEC(FiltroComentariosAMEC filtroComentarios, DVPeticionariosRoles datosRoles)
        {

            return MiRepositorioComentarios.ObtenerComentariosAMEC(filtroComentarios);
        }

        public int ComprobarComentarioEsTuyo(int idcomentario, int idpeticionario)
        {
            int i = MiRepositorioComentarios.ComprobarComentarioEsTuyo(idcomentario, idpeticionario);

            return i;
        }

        public long ObtenerNumeroComentariosAMEC(FiltroComentariosAMEC filtroComentarios, DVPeticionariosRoles datosRoles)
        {
            return MiRepositorioComentarios.ObtenerNumeroComentariosAMEC(filtroComentarios); 
        }

        public int EliminarComentarioAMEC(long idcomentarioamec, DVPeticionariosRoles datosRoles)
        {
            return MiRepositorioComentarios.EliminarComentarioAMEC(idcomentarioamec);
        }       

        

    }
}
