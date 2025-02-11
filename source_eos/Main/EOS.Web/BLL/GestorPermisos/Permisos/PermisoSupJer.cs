using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Web.Enums;
using EOS.Web;

namespace EOS.Web.BLL.GestorPermisos.Permisos
{
    public class PermisoSupJer : IPermiso
    {
        public EstadosAmec Estado { get { return EstadosAmec.PendienteSupJerarquico; } }
        private readonly AgenteAprobadorAmec _agAprobador;

        public PermisoSupJer(AgenteAprobadorAmec agAprobador)
        {
            _agAprobador = agAprobador;
        }

        public bool BuscarPermisos(int estado)
        {
            return estado == Estado.GetHashCode();
        }

        public bool PuedoAprobarYRechazar(string idamec, int idPeticionario)
        {
            return _agAprobador.ObtenerPermisosSupJerAmec(idamec, idPeticionario);
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
                agAmec.InsertarHistorialAmec(EstadosAmec.Rechazado.GetHashCode(), idamec, usuario, NivelesAprobacion.SuperiorJerarquico.GetHashCode(), null);
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
            EstadosAmec tipoEstado = condicionado ? EstadosAmec.AprobadoCondicionadoSupJerarquico : EstadosAmec.AprobadoSupJerarquico;
            EstadosAmec tipoEstadoNeg = condicionado ? EstadosAmec.AprobadoCondicionadoNegocio : EstadosAmec.AprobadoNegocio;

            try
            {
                if (HeAprobado(idamec, idPeticionario)) return false;
                AgenteUsuarios agUsuarios = new AgenteUsuarios();
                AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
                DDatosPersonalesUsuario usuario = agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(idPeticionario.ToString());
                bool insertDirector = false;
                //Si es mayor que 0 es que el aprobador esta siendo el manager del superior jerarquico
                if (usuario.director != null && !usuario.director.Value &&
                    _agAprobador.CheckPermisoAprobacionJefe(idamec, idPeticionario, 1) > 0)
                {
                    int numInserciones = agAprobador.DeCuantosSoyManager(idamec, idPeticionario);
                    int i = 0;
                    while (i < numInserciones)
                    {
                        agAmec.InsertarHistorialAmec(tipoEstadoNeg.GetHashCode(), idamec, usuario, NivelesAprobacion.Negocio.GetHashCode(), null);
                        agAmec.InsertarHistorialAmec(tipoEstado.GetHashCode(), idamec, usuario, NivelesAprobacion.SuperiorJerarquico.GetHashCode(), null);
                        i++;
                    }
                    agAprobador.ActualizarAprobacionJefe(idamec, idPeticionario, AprobacionJefe.JefeAprobo.GetHashCode());
                }
                else
                {
                    if ((usuario.executive.HasValue && usuario.executive.Value) || (usuario.director.HasValue && usuario.director.Value))
                    {
                        agAmec.InsertarHistorialAmec(EstadosAmec.AprobadoNegocio.GetHashCode(), idamec, usuario, NivelesAprobacion.Negocio.GetHashCode(), null);
                        if (usuario.director.HasValue && usuario.director.Value)
                        {
                            insertDirector = true;
                        }
                    }
                    else
                    {
                        agAmec.InsertarHistorialAmec(tipoEstado.GetHashCode(), idamec, usuario, NivelesAprobacion.SuperiorJerarquico.GetHashCode(), null);
                    }
                }

                //Si es director solo insertamos negocio
                if (!insertDirector && usuario.director != null && usuario.director.Value)
                {
                    int numInserciones = agAprobador.DeCuantosSoyManager(idamec, idPeticionario);
                    int i = 0;
                    while (i < numInserciones)
                    {
                        agAmec.InsertarHistorialAmec(tipoEstadoNeg.GetHashCode(), idamec, usuario, NivelesAprobacion.Negocio.GetHashCode(), null);
                        i++;
                    }
                }
                if (TodosAprobaron(idamec))
                {
                    if (agAprobador.SonTodosAprobadoresDirectores(idamec))
                        ComunPermisos.CambiarEstadoAmec(idamec, agAmec, EstadosAmec.PendienteDtoMedico,NivelesAprobacion.DepartamentoMedico);
                    else
                    {
                        if(TodosAprobaronNegocio(idamec))
                            ComunPermisos.CambiarEstadoAmec(idamec, agAmec, EstadosAmec.PendienteDtoMedico, NivelesAprobacion.DepartamentoMedico);
                        else
                            ComunPermisos.CambiarEstadoAmec(idamec, agAmec, EstadosAmec.PendienteNegocio,NivelesAprobacion.Negocio);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                agAmec.BorrarHistorialAmec(tipoEstado.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.SuperiorJerarquico.GetHashCode());
                agAmec.BorrarHistorialAmec(tipoEstadoNeg.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.Negocio.GetHashCode());
                agAmec.CambiarEstadoAmec(amec.idestado.ToString(), idamec.ToString(), amec.idnivelaprobacion);
                throw;
            }
        }

        private bool HeAprobado(string idamec, int idPeticionario)
        {
            return _agAprobador.HeAprobadoSupJer(idamec, idPeticionario);
        }

        private bool TodosAprobaron(string idamec)
        {
            return _agAprobador.TodosAprobaronSupJer(idamec);
        }

        private bool TodosAprobaronNegocio(string idamec)
        {
            List<int> estados = new List<int>();
            estados.Add(EstadosAmec.AprobadoSupJerarquico.GetHashCode());
            estados.Add(EstadosAmec.AprobadoCondicionadoSupJerarquico.GetHashCode());
            estados.Add(EstadosAmec.PreaprobadoNegocio.GetHashCode());
            return _agAprobador.TodosAprobaronNegocio(idamec, estados);
        }

        public List<string> ObtenerListaDestinatarios(string idamec, int idPeticionario)
        {
            try
            {
                AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
                return agAprobador.DestinatariosAprobadores(idamec).Select(x => x.ToString()).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
