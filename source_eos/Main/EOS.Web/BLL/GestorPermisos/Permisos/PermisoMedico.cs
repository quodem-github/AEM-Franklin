using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Web.Enums;
using EOS.Entidades.Datos;
using EOS.Web;

namespace EOS.Web.BLL.GestorPermisos.Permisos
{
    public class PermisoMedico : IPermiso
    {
        public EstadosAmec Estado { get { return EstadosAmec.PendienteDtoMedico; } }
        private readonly AgenteAprobadorAmec _agAprobador;

        public PermisoMedico(AgenteAprobadorAmec agAprobador)
        {
            _agAprobador = agAprobador;
        }

        public bool BuscarPermisos(int estado)
        {
            return estado == Estado.GetHashCode();
        }

        public bool PuedoAprobarYRechazar(string idamec, int idAprobador)
        {
            AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
            return agAprobador.ObtenerPermisosMedico(idamec, idAprobador);
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
                
                DDatosPersonalesUsuario usuario =
                    agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(idPeticionario.ToString());
                agAmec.InsertarHistorialAmec(EstadosAmec.Rechazado.GetHashCode(), idamec, usuario,
                    NivelesAprobacion.DepartamentoMedico.GetHashCode(), null);
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
            EstadosAmec tipoEstado = condicionado
                    ? EstadosAmec.AprobadoCondicionadoDtoMedico
                    : EstadosAmec.AprobadoDtoMedico;
            try
            {
                if (HeAprobado(idamec)) return false;
                AgenteUsuarios agUsuarios = new AgenteUsuarios();
                
                DDatosPersonalesUsuario usuario =
                    agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(idPeticionario.ToString());
                
                agAmec.InsertarHistorialAmec(tipoEstado.GetHashCode(), idamec, usuario,
                    NivelesAprobacion.DepartamentoMedico.GetHashCode(), null);
                ComunPermisos.CambiarEstadoAmec(idamec, agAmec, EstadosAmec.PendienteDtoLegal,
                    NivelesAprobacion.DepartamentoLegal);
                return true;
            }
            catch (Exception ex)
            {
                agAmec.BorrarHistorialAmec(tipoEstado.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.DepartamentoMedico.GetHashCode());
                agAmec.CambiarEstadoAmec(amec.idestado.ToString(), idamec.ToString(), amec.idnivelaprobacion);
                throw;
            }
        }

        private bool HeAprobado(string idamec)
        {
            return _agAprobador.HeAprobadoMedico(idamec);
        }

       
        
        public List<string> ObtenerListaDestinatarios(string idamec, int idPeticionario)
        {
            try
            {
                AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
                return agAprobador.DestinatariosMedicos().Select(x => x.ToString()).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
