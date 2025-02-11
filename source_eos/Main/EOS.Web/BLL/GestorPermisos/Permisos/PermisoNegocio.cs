using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Web.Enums;
using EOS.Entidades.Datos;
using EOS.Web;

namespace EOS.Web.BLL.GestorPermisos.Permisos
{
    public class PermisoNegocio : IPermiso
    {
        public EstadosAmec Estado { get { return EstadosAmec.PendienteNegocio; } } 
        private readonly AgenteAprobadorAmec _agAprobador;

        public PermisoNegocio(AgenteAprobadorAmec agAprobador)
        {
            _agAprobador = agAprobador;
        }

        public bool BuscarPermisos(int estado)
        {
            return estado == Estado.GetHashCode();
        }

        public bool PuedoAprobarYRechazar(string idamec, int idAprobador)
        {
            bool solicitanteDirector = ComunPermisos.EsSolicitanteDirector(idamec);
            AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
            AgenteAmecInfo agAmec = new AgenteAmecInfo();
            DAmecInfo amec = agAmec.CargarTodosValoresAmec(idamec);
            List<DAprobadoresAmec> aprobadores = agAprobador.ObernerAprobadoresAmec(idamec.ToString(), idAprobador);
            if (aprobadores.Count > 0)
            {
                AgenteUsuarios agUsuarios = new AgenteUsuarios();
                DDatosPersonalesUsuario usuario = agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(idAprobador.ToString());
                bool usuarioDirector = ((usuario.director.HasValue && usuario.director.Value) || (usuario.executive.HasValue && usuario.executive.Value));
                if (!usuarioDirector && aprobadores[0].IdAprobador != idAprobador && aprobadores[0].IdPeticionarioManager != idAprobador && aprobadores[0].IdPeticionarioManagerManager != idAprobador)
                {
                    solicitanteDirector = false;
                }
            }
            return solicitanteDirector ?
                  _agAprobador.ObtenerPermisosExecutive(idamec, idAprobador)
                : _agAprobador.ObtenerPermisosNegocio(idamec, idAprobador);
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
                agAmec.InsertarHistorialAmec(EstadosAmec.Rechazado.GetHashCode(), idamec, usuario, NivelesAprobacion.Negocio.GetHashCode(), null);
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
            EstadosAmec tipoEstado = condicionado ? EstadosAmec.AprobadoCondicionadoNegocio : EstadosAmec.AprobadoNegocio;
            try
            {
                AgenteUsuarios agUsuarios = new AgenteUsuarios();
                AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
                List<int> estados = new List<int>();

                bool solicitanteDirector = ComunPermisos.EsSolicitanteDirector(idamec);
                int numInserciones;
                if (solicitanteDirector)
                {
                    // En el caso del director solo son necesarias las aprobaciones de nivel uno (Sup Jerarquico) Pero para ellos es negocio.
                    //Dependiendo de si es dir o no, comparamos con los estados previos correspondientes
                    estados.Add(EstadosAmec.AmecSometido.GetHashCode());
                    numInserciones = 1;
                }
                else
                {
                    estados.Add(EstadosAmec.AprobadoSupJerarquico.GetHashCode());
                    estados.Add(EstadosAmec.AprobadoCondicionadoSupJerarquico.GetHashCode());
                    estados.Add(EstadosAmec.PreaprobadoNegocio.GetHashCode());
                    numInserciones = agAprobador.DeCuantosSoyManager(idamec, idPeticionario);
                }

                if (HeAprobado(idamec, idPeticionario, estados)) return false;

                DDatosPersonalesUsuario usuario = agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(idPeticionario.ToString());

                
                int i = 0;
                while (i < numInserciones && i < 1)
                {
                    agAmec.InsertarHistorialAmec(tipoEstado.GetHashCode(), idamec, usuario, NivelesAprobacion.Negocio.GetHashCode(), null);
                    i++;
                }

                if(numInserciones == 0 && ( (usuario.director.HasValue && usuario.director.Value) || (usuario.executive.HasValue && usuario.executive.Value)) )
                {
                    agAmec.InsertarHistorialAmec(tipoEstado.GetHashCode(), idamec, usuario, NivelesAprobacion.Negocio.GetHashCode(), null);
                }

                if (TodosAprobaron(idamec, estados))
                {
                    if (solicitanteDirector)
                    {
                        if (amec.preaprobadaamed.HasValue && amec.preaprobadaamed.Value)
                        {
                            if (amec.preaprobadaleg.HasValue && amec.preaprobadaleg.Value)
                            {
                                ComunPermisos.CambiarEstadoAmec(idamec, agAmec, EstadosAmec.Aprobado, NivelesAprobacion.Aprobado);
                                agAmec.InsertarHistorialAmec(EstadosAmec.Aprobado.GetHashCode(), idamec, usuario, NivelesAprobacion.Aprobado.GetHashCode(), null);
                            }
                            else
                            {
                                agAmec.CambiarEstadoAmec(EstadosAmec.PendienteDtoLegal.GetHashCode().ToString(), idamec.ToString(), NivelesAprobacion.DepartamentoLegal.GetHashCode());
                            }
                        }
                        else
                        {
                            agAmec.CambiarEstadoAmec(EstadosAmec.PendienteDtoMedico.GetHashCode().ToString(), idamec.ToString(), NivelesAprobacion.DepartamentoMedico.GetHashCode());
                        }
                    }
                    else
                    {
                        if (amec.preaprobadaamed.HasValue && amec.preaprobadaamed.Value)
                        {
                            if (amec.preaprobadaleg.HasValue && amec.preaprobadaleg.Value)
                            {
                                ComunPermisos.CambiarEstadoAmec(idamec, agAmec, EstadosAmec.Aprobado, NivelesAprobacion.Aprobado);
                                agAmec.InsertarHistorialAmec(EstadosAmec.Aprobado.GetHashCode(), idamec, usuario, NivelesAprobacion.Aprobado.GetHashCode(), null);
                            }
                            else
                            {
                                agAmec.CambiarEstadoAmec(EstadosAmec.PendienteDtoLegal.GetHashCode().ToString(), idamec.ToString(), NivelesAprobacion.DepartamentoLegal.GetHashCode());
                            }
                        }
                        else
                        {
                            agAmec.CambiarEstadoAmec(EstadosAmec.PendienteDtoMedico.GetHashCode().ToString(), idamec.ToString(), NivelesAprobacion.DepartamentoMedico.GetHashCode());
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                agAmec.BorrarHistorialAmec(tipoEstado.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.Negocio.GetHashCode());
                agAmec.CambiarEstadoAmec(amec.idestado.ToString(), idamec.ToString(), amec.idnivelaprobacion);
                throw;
            }
        }

        private bool HeAprobado(string idamec, int idPeticionario, List<int> estados)
        {
            return _agAprobador.HeAprobadoNegocio(idamec, idPeticionario, estados);
        }

        private bool TodosAprobaron(string idamec, List<int> estados)
        {
            return _agAprobador.TodosAprobaronNegocio(idamec, estados);
        }

      
        public List<string> ObtenerListaDestinatarios(string idamec, int idPeticionario)
        {
            try
            {
                   AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
                return agAprobador.DestinatariosAprobadoresNegocio(idamec).Select(x => x.ToString()).ToList();

                //Esta validacion comprobaba si el solicitante era director y si no lo fuera, no enviaba correos a executive
                //bool solicitanteDirector = ComunPermisos.EsSolicitanteDirector(idamec);
                /*return solicitanteDirector ? agAprobador.DestinatariosAprobadoresNegocio(idamec).Select(x => x.ToString()).ToList() 
                    : agAprobador.DestinatariosAprobadoresNegocioSinExecutive(idamec).Select(x => x.ToString()).ToList();*/
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
