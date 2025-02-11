using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Logica;

namespace EOS.Web
{
    public class AgenteComentariosAMEC : AgenteBase
    {

        public LogicaComentariosAMEC MiLogicaComentariosAMEC { get; set; }
        
        public AgenteComentariosAMEC()
        {
            MiLogicaComentariosAMEC = new LogicaComentariosAMEC();
            
        }            

        public int InsertComentarioAMEC(string filtroidamec, string filtroidcreadopor, string filtrocomentario)
        {
            FiltroComentariosAMEC filtroComentarios =
                new FiltroComentariosAMEC()
                {
                    IdAmec = filtroidamec,
                    IdUsuario = string.IsNullOrEmpty(filtroidcreadopor) ? new Nullable<int>() : int.Parse(filtroidcreadopor),
                    Comentario = string.IsNullOrEmpty(filtrocomentario) ? "" : filtrocomentario, 
                };

            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();
            int insertat = MiLogicaComentariosAMEC.InsertComentarioAMEC(filtroComentarios,datosRoles);
            return insertat;
        }


        public int ComprobarComentarioEsTuyo(int idcomentario, string idpeticionario)
        {

            return MiLogicaComentariosAMEC.ComprobarComentarioEsTuyo(idcomentario, Int32.Parse(idpeticionario)); 
        }
        public ICollection<DComentariosAmec> ObtenerComentariosAmec(string filtroidamec, string sortParameter, int startRowIndex, int maximumRows)
        {

            FiltroComentariosAMEC filtroComentarios =
                new FiltroComentariosAMEC()
                {
                    IdAmec = filtroidamec, 
                    SortParameter = string.IsNullOrEmpty(sortParameter) ? "null" : sortParameter,
                    StartRowIndex = startRowIndex == null ? new Nullable<int>() : startRowIndex,
                    MaximumRows = maximumRows == null ? new Nullable<int>() : maximumRows
                };

            DVPeticionariosRoles datosRoles = ObtenerDatosRoles(); //??
            ICollection<DComentariosAmec> collectionComentariosAMEC = MiLogicaComentariosAMEC.ObtenerComentariosAMEC(filtroComentarios, datosRoles); //??
       
            return collectionComentariosAMEC;

        }

        public int ObtenerNumeroComentariosAMEC(string filtroidamec)
        {
            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();
            FiltroComentariosAMEC filtroComentarios =
                new FiltroComentariosAMEC()
                {
                    IdAmec = filtroidamec
                };
            long numComentariosAMEC = MiLogicaComentariosAMEC.ObtenerNumeroComentariosAMEC(filtroComentarios, datosRoles);
            return Convert.ToInt32(numComentariosAMEC);
        }

        public int EliminarComentarioAMEC(long idcomentarioamec)
        {
            
            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();

            int resultat = MiLogicaComentariosAMEC.EliminarComentarioAMEC(idcomentarioamec, datosRoles);
            return 1;

        }

        private DVPeticionariosRoles ObtenerDatosRoles()
        {
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            return agenteUsu.ObtenerDatosRolesPorLogin();
        }
    }
        
}
