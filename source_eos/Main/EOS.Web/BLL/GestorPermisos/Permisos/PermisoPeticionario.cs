using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Web.Enums;
using EOS.Web;

namespace EOS.Web.BLL.GestorPermisos.Permisos
{
    public class PermisoPeticionario : IPermiso
    {
        public EstadosAmec Estado { get { return EstadosAmec.PendienteSupJerarquico; } }
        private readonly List<int> _estados;
        private readonly AgenteAprobadorAmec _agAprobador;

        public PermisoPeticionario(AgenteAprobadorAmec agAprobador)
        {
            _agAprobador = agAprobador;
        }

        public PermisoPeticionario()
        {
            _estados = new List<int>
            {
                EstadosAmec.Aprobado.GetHashCode(),
                EstadosAmec.Cancelado.GetHashCode(),
                EstadosAmec.Rechazado.GetHashCode()
            };
        }

        public bool BuscarPermisos(int estado)
        {
            return !_estados.Contains(estado);
        }

        public bool PuedoAprobarYRechazar(string idamec, int idAprobador)
        {
            AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
            return agAprobador.ObtenerPermisosSupJerAmec(idamec, idAprobador);
        }
        public bool Rechazar(string idamec, int idPeticionario)
        {
            return false;
        }
        public bool PuedoSometer(string idamec, int idPeticionario)
        {
            throw new NotImplementedException();
        }
        public bool Aprobar(string idamec, int idPeticionario, bool condicionado)
        {
            throw new NotImplementedException();
        }
        public List<string> ObtenerListaDestinatarios(string idamec, int idPeticionario)
        {
            return new List<string>();
        }
    }
}
