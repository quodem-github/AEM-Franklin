using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Web.Enums;
using EOS.Entidades.Datos;
using EOS.Web;

namespace EOS.Web.BLL.GestorPermisos.Permisos
{
    public class PermisoSupJerAssistant : IPermiso
    {
        
        public EstadosAmec Estado { get { return EstadosAmec.PendienteSupJerAssistant; } }
        private readonly AgenteAprobadorAmec _agAprobador;

        public PermisoSupJerAssistant(AgenteAprobadorAmec agAprobador)
        {
            _agAprobador = agAprobador;
        }

        public bool BuscarPermisos(int estado)
        {
            return estado == Estado.GetHashCode();
        }

        public bool PuedoAprobarYRechazar(string idamec, int idPeticionario)
        {

            return _agAprobador.ObtenerPermisosSupJerCreador(idamec, idPeticionario);
        }

        public bool PuedoSometer(string idamec, int idPeticionario)
        {
            return _agAprobador.ObtenerPermisoSometer(idamec, idPeticionario);
        }
        public bool Rechazar(string idamec, int idPeticionario)
        {
            AgenteAmecInfo agAmec = new AgenteAmecInfo();
            DAmecInfo amec = agAmec.CargarTodosValoresAmec(idamec);
            try
            {
                AgenteUsuarios agUsuarios = new AgenteUsuarios();
                
                DDatosPersonalesUsuario usuario = agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(idPeticionario.ToString());
                agAmec.InsertarHistorialAmec(EstadosAmec.Rechazado.GetHashCode(), idamec, usuario, NivelesAprobacion.SupJerAssistant.GetHashCode(), null);
                ComunPermisos.CambiarEstadoAmec(idamec, agAmec, EstadosAmec.Rechazado, NivelesAprobacion.No);
                return true;
            }
            catch (Exception)
            {
                agAmec.BorrarHistorialAmec(EstadosAmec.Rechazado.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.DepartamentoLegal.GetHashCode());
                agAmec.CambiarEstadoAmec(amec.idestado.ToString(), idamec.ToString(), amec.idnivelaprobacion);
                throw;
            }
        }
        public bool Aprobar(string idamec, int idPeticionario, bool condicionado)
        {
            AgenteAmecInfo agAmec = new AgenteAmecInfo();
            DAmecInfo amec = agAmec.CargarTodosValoresAmec(idamec);
            try
            {
                AgenteUsuarios agUsuarios = new AgenteUsuarios();
                
                DDatosPersonalesUsuario usuario = agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(idPeticionario.ToString());
                agAmec.InsertarHistorialAmec(EstadosAmec.AprobadoSupJerAssistant.GetHashCode(), idamec, usuario, NivelesAprobacion.SupJerAssistant.GetHashCode(), null);
                agAmec.CambiarEstadoAmec(EstadosAmec.PendienteNegocioAssistant.GetHashCode().ToString(), idamec.ToString(), NivelesAprobacion.NegocioAssitant.GetHashCode());
                return true;
            }
            catch (Exception ex)
            {
                agAmec.BorrarHistorialAmec(EstadosAmec.AprobadoSupJerAssistant.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.SupJerAssistant.GetHashCode());
                agAmec.CambiarEstadoAmec(amec.idestado.ToString(), idamec.ToString(), amec.idnivelaprobacion);
                throw;
            }
        }
        public List<string> ObtenerListaDestinatarios(string idamec, int idPeticionario)
        {
            return new List<string>();
        }
    }
}
