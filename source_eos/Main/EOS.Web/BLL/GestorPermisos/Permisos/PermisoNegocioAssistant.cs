using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Web.Enums;
using EOS.Entidades.Datos;
using EOS.Web;

namespace EOS.Web.BLL.GestorPermisos.Permisos
{
    public class PermisoNegocioAssistant : IPermiso
    {
        public EstadosAmec Estado { get { return EstadosAmec.PendienteNegocioAssistant; } }
        private readonly AgenteAprobadorAmec _agAprobador;

        public PermisoNegocioAssistant(AgenteAprobadorAmec agAprobador)
        {
            _agAprobador = agAprobador;
        }

        public bool BuscarPermisos(int estado)
        {
            return estado == Estado.GetHashCode();
        }

        public bool PuedoAprobarYRechazar(string idamec, int idPeticionario)
        {

            return _agAprobador.ObtenerPermisosNegocioCreador(idamec, idPeticionario);
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
                agAmec.InsertarHistorialAmec(EstadosAmec.Rechazado.GetHashCode(), idamec, usuario, NivelesAprobacion.NegocioAssitant.GetHashCode(), null);
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
                
                bool solicitanteDirector = ComunPermisos.EsSolicitanteDirector(idamec);
                DDatosPersonalesUsuario usuario = agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(idPeticionario.ToString());
                agAmec.InsertarHistorialAmec(EstadosAmec.AprobadoNegocioAssistant.GetHashCode(), idamec, usuario, NivelesAprobacion.NegocioAssitant.GetHashCode(), null);
                if (solicitanteDirector)
                {
                    agAmec.CambiarEstadoAmec(EstadosAmec.PendienteNegocio.GetHashCode().ToString(), idamec.ToString(), NivelesAprobacion.Negocio.GetHashCode());
                }
                else
                {
                    if (amec.preaprobadaneg.HasValue && amec.preaprobadaneg.Value)
                    {
                        agAmec.CambiarEstadoAmec(EstadosAmec.PendienteNegocio.GetHashCode().ToString(), idamec.ToString(), NivelesAprobacion.Negocio.GetHashCode());
                    }
                    else
                    {
                        agAmec.CambiarEstadoAmec(EstadosAmec.PendienteSupJerarquico.GetHashCode().ToString(), idamec.ToString(), NivelesAprobacion.SuperiorJerarquico.GetHashCode());
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                if (amec.preaprobadaneg.HasValue && amec.preaprobadaneg.Value)
                    agAmec.BorrarHistorialAmec(EstadosAmec.PreaprobadoNegocio.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.SuperiorJerarquico.GetHashCode());
                agAmec.BorrarHistorialAmec(EstadosAmec.AprobadoNegocioAssistant.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.NegocioAssitant.GetHashCode());
                agAmec.CambiarEstadoAmec(amec.idestado.ToString(), idamec.ToString(), amec.idnivelaprobacion);
                throw;
            }
        }
        
        public List<string> ObtenerListaDestinatarios(string idamec, int idPeticionario)
        {
            try
            {
                AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
                return new List<string> { agAprobador.DestinatariosSupJer(idPeticionario).ToString() };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
