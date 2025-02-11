using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Web.Enums;

namespace EOS.Web.BLL.GestorPermisos.Permisos
{
    public interface IPermiso
    {
        EstadosAmec Estado { get; }
        bool BuscarPermisos(int estado);
        bool PuedoAprobarYRechazar(string idamec, int idPeticionario);
        bool PuedoSometer(string idamec, int idPeticionario);
        bool Aprobar(string idamec, int idPeticionario, bool condicionado);
        bool Rechazar(string idamec, int idPeticionario);
        List<string> ObtenerListaDestinatarios(string idamec, int idPeticionario);
    }
}
