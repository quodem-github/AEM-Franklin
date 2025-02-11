using System;
using System.Collections.Generic;
using System.Linq;
using EOS.Entidades.Filtros;
using EOS.Web;
using EOS.Web.Enums;
using EOS.Entidades.Datos;

namespace EOS.Web.BLL.GestorFlujo.Flujo
{
    /// <summary>
    /// Delegado Sin Paraguas
    /// </summary>
    public class FlujoDSP : IFlujo
    {

        public bool EsDifSolicitante { get{return false;} }
        public bool ConParaguas { get{return false;} }
        public bool EsAsistente{ get{return false;} }
        public bool EsDelegado { get{return true;} }
        public bool EsDirector { get{return false;} }
        public bool EstaSometido { get{return true;} }
        public bool SolicitanteDirector { get { return false; } }

        public bool BuscarFlujo(CondicionesFlujo condiciones)
        {
            return (ConParaguas == condiciones.ConParaguas &&
                EsDifSolicitante == condiciones.EsDifSolicitante &&
                EsDelegado == condiciones.EsDelegado &&
                EsAsistente == condiciones.EsAsistente &&
                EsDirector == condiciones.EsDirector) ||
                (ConParaguas == condiciones.ConParaguas &&
                EstaSometido == condiciones.EstaSometido &&
                SolicitanteDirector == condiciones.SolicitanteDirector);
        }

        /// <summary>
        /// Insertamos el superior del aprobador en la tabla aprobadores. Modificamos el estado del amec a pendiente de sup jerarquico o pendiente negocio si esta preaprobado.
        /// </summary>
        /// <param name="condiciones">necesario para saber si esta preaprobado por algun departamento</param>
        /// <param name="idamec"></param>
        /// <param name="idPeticionario">id del creador</param>
        public bool SometerAmec(CondicionesFlujo condiciones, string idamec, int idPeticionario, int idCreador)
        {
            AgenteAmecInfo agAmec = new AgenteAmecInfo();
            EstadosAmec estadoNegocio = EstadosAmec.PendienteNegocio;
            EstadosAmec estadoMedico = condiciones.PreMedico ? EstadosAmec.PreaprobadoDtoMedico : EstadosAmec.PendienteDtoMedico;
            EstadosAmec estadoLegal = condiciones.PreLegal ? EstadosAmec.PreaprobadoDtoLegal : EstadosAmec.PendienteDtoLegal;
            EstadosAmec estadoSupJer = condiciones.PreNegocio ? EstadosAmec.PreaprobadoNegocio : EstadosAmec.PendienteSupJerarquico;
            try
            {
                AgenteUsuarios agUsuarios = new AgenteUsuarios();
                DDatosPersonalesUsuario usuario = agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(idPeticionario.ToString());

                AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
                List<DAprobadoresAmec> aprobadores = agAprobador.ObernerAprobadoresAmec(idamec.ToString(), null);

                agAmec.InsertarHistorialAmec(EstadosAmec.AmecSometido.GetHashCode(), idamec, usuario, NivelesAprobacion.Someter.GetHashCode(), null);

                //Inserciones en el historial
                var i = 0;
                bool allDirector = true;
                List<int?> lstAprobadoresNegocioInserted = new List<int?>();
                while (i < aprobadores.Count)
                {
                    DDatosPersonalesUsuario usuarioAprobador = agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(aprobadores[i].IdAprobador.ToString());
                    if ((usuarioAprobador.director.HasValue && usuarioAprobador.director.Value) || (usuarioAprobador.executive.HasValue && usuarioAprobador.executive.Value))
                    {
                        agAmec.InsertarHistorialAmec(estadoNegocio.GetHashCode(), idamec, usuario, NivelesAprobacion.Negocio.GetHashCode(), aprobadores[i].IdAprobador);
                        lstAprobadoresNegocioInserted.Add(aprobadores[i].IdAprobador);
                    }
                    else
                    {
                        allDirector = false;
                        agAmec.InsertarHistorialAmec(EstadosAmec.PendienteSupJerarquico.GetHashCode(), idamec, usuario, NivelesAprobacion.SuperiorJerarquico.GetHashCode(), aprobadores[i].IdAprobador);
                        if (!lstAprobadoresNegocioInserted.Contains(aprobadores[i].IdPeticionarioManager) || !aprobadores[i].IdPeticionarioManager.HasValue)
                        {
                            agAmec.InsertarHistorialAmec(EstadosAmec.PendienteNegocio.GetHashCode(), idamec, usuario, NivelesAprobacion.Negocio.GetHashCode(), aprobadores[i].IdPeticionarioManager);
                            lstAprobadoresNegocioInserted.Add(aprobadores[i].IdPeticionarioManager);
                        }
                    }
                    i++;
                }
                if (allDirector)
                {
                    agAmec.CambiarEstadoAmec(EstadosAmec.PendienteNegocio.GetHashCode().ToString(), idamec.ToString(), NivelesAprobacion.Negocio.GetHashCode());
                }
                else
                {
                    agAmec.CambiarEstadoAmec(EstadosAmec.PendienteSupJerarquico.GetHashCode().ToString(), idamec.ToString(), NivelesAprobacion.SuperiorJerarquico.GetHashCode());
                }

                agAmec.InsertarHistorialAmec(estadoMedico.GetHashCode(), idamec, usuario, NivelesAprobacion.DepartamentoMedico.GetHashCode(), null);
                agAmec.InsertarHistorialAmec(estadoLegal.GetHashCode(), idamec, usuario, NivelesAprobacion.DepartamentoLegal.GetHashCode(), null);

                return true;
            }
            catch (Exception ex)
            {
                agAmec.BorrarHistorialAmec(EstadosAmec.AmecSometido.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.Someter.GetHashCode());
                agAmec.BorrarHistorialAmec(EstadosAmec.PendienteSupJerarquico.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.SuperiorJerarquico.GetHashCode());
                agAmec.BorrarHistorialAmec(estadoNegocio.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.Negocio.GetHashCode());
                agAmec.BorrarHistorialAmec(estadoMedico.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.DepartamentoMedico.GetHashCode());
                agAmec.BorrarHistorialAmec(estadoLegal.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.DepartamentoLegal.GetHashCode());
                agAmec.CambiarEstadoAmec(EstadosAmec.PendienteSometer.GetHashCode().ToString(), idamec.ToString(), NivelesAprobacion.Someter.GetHashCode());
                throw;
            }

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