using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Web.Enums;
using EOS.Web;

namespace EOS.Web.BLL.GestorPermisos.Permisos
{
    public class PermisoBorrador: IPermiso
    {
        public EstadosAmec Estado { get { return EstadosAmec.Borrador; } }
        private readonly AgenteAprobadorAmec _agAprobador;

        public PermisoBorrador(AgenteAprobadorAmec agAprobador)
        {
            _agAprobador = agAprobador;
        }

        public bool BuscarPermisos(int estado)
        {
            return estado == Estado.GetHashCode();
        }

        public bool PuedoAprobarYRechazar(string idamec, int idPeticionario)
        {
            return false;
        }

        public bool PuedoSometer(string idamec, int idPeticionario)
        {
            return _agAprobador.ObtenerPermisoSometer(idamec, idPeticionario) || _agAprobador.ObtenerPermisoSometerCreador(idamec, idPeticionario);
        }

        public bool Aprobar(string idamec, int idPeticionario, bool condicionado)
        {
            return false;
        }

        public bool Rechazar(string idamec, int idPeticionario)
        {
            return false;
        }
        public List<string> ObtenerListaDestinatarios(string idamec, int idPeticionario)
        {
            return new List<string>();
        }
    }
}
