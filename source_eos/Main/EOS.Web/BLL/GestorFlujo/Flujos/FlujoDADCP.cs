using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EOS.Web.Enums;
using EOS.Entidades.Datos;
using EOS.Web;

namespace EOS.Web.BLL.GestorFlujo.Flujo
{
    /// <summary>
    /// Delegado A Delegado Con Paraguas
    /// </summary>
    public class FlujoDADCP : IFlujo
    {

        public bool EsDifSolicitante {get{return true;}}
        public bool ConParaguas {get{return true;}}
        public bool EsAsistente {get{return false;}}
        public bool EsDelegado {get{return true;}}
        public bool EsDirector {get{return false;}}
        public bool EstaSometido { get { return false; } }

        public bool BuscarFlujo(CondicionesFlujo condiciones)
        {
            return ConParaguas == condiciones.ConParaguas &&
                   EsDifSolicitante == condiciones.EsDifSolicitante &&
                   EsDelegado == condiciones.EsDelegado &&
                   EsAsistente == condiciones.EsAsistente &&
                   EsDirector == condiciones.EsDirector &&
                   EstaSometido == condiciones.EstaSometido;
        }
        public bool SometerAmec(CondicionesFlujo condiciones, string idamec, int idPeticionario, int idCreador)
        {
            AgenteAmecInfo agAmec = new AgenteAmecInfo();
            EstadosAmec estadoNegocio =  EstadosAmec.PendienteNegocio;
            EstadosAmec estadoMedico = condiciones.PreMedico ? EstadosAmec.PreaprobadoDtoMedico : EstadosAmec.PendienteDtoMedico;
            EstadosAmec estadoLegal = condiciones.PreLegal ? EstadosAmec.PreaprobadoDtoLegal : EstadosAmec.PendienteDtoLegal;
            EstadosAmec estadoSupJer = condiciones.PreNegocio ? EstadosAmec.PreaprobadoNegocio : EstadosAmec.PendienteSupJerarquico;
            try
            {
                AgenteUsuarios agUsuarios = new AgenteUsuarios();
                DDatosPersonalesUsuario usuario = agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(idPeticionario.ToString());
                AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
                List<DAprobadoresAmec> aprobadores = agAprobador.ObernerAprobadoresAmec(idamec.ToString(), null);
                List<DAprobadoresAmec> aprobadoresAssistant = agAprobador.ObernerAprobadoresAmec(string.Empty, idCreador);

                agAmec.InsertarHistorialAmec(EstadosAmec.AmecSometido.GetHashCode(), idamec, usuario, NivelesAprobacion.Someter.GetHashCode(), null);
                //Estado pendiente superior jerarquico
                agAmec.CambiarEstadoAmec(EstadosAmec.PendienteSupJerarquicoCreador.GetHashCode().ToString(), idamec.ToString(), NivelesAprobacion.SupJerarquicoCreador.GetHashCode());
                agAmec.InsertarHistorialAmec(EstadosAmec.PendienteSupJerarquicoCreador.GetHashCode(), idamec, usuario, NivelesAprobacion.SupJerarquicoCreador.GetHashCode(), aprobadoresAssistant.First().IdPeticionarioManager);

                //Estados dependiendo de las preaprobaciones en el historial del amec
                

                //Inserciones en el historial
                var i = 0;
                List<int?> lstAprobadoresNegocioInserted = new List<int?>();
                while (i < aprobadores.Count)
                {
                    agAmec.InsertarHistorialAmec(estadoSupJer.GetHashCode(), idamec, usuario, NivelesAprobacion.SuperiorJerarquico.GetHashCode(), aprobadores[i].IdAprobador);
                    if (!lstAprobadoresNegocioInserted.Contains(aprobadores[i].IdPeticionarioManager) || !aprobadores[i].IdPeticionarioManager.HasValue)
                    {
                        agAmec.InsertarHistorialAmec(EstadosAmec.PendienteNegocio.GetHashCode(), idamec, usuario, NivelesAprobacion.Negocio.GetHashCode(), aprobadores[i].IdPeticionarioManager);
                        lstAprobadoresNegocioInserted.Add(aprobadores[i].IdPeticionarioManager);
                    }
                    i++;
                }
                agAmec.InsertarHistorialAmec(estadoMedico.GetHashCode(), idamec, usuario, NivelesAprobacion.DepartamentoMedico.GetHashCode(), null);
                agAmec.InsertarHistorialAmec(estadoLegal.GetHashCode(), idamec, usuario, NivelesAprobacion.DepartamentoLegal.GetHashCode(), null);

                return true;
            }
            catch (Exception ex)
            {
                agAmec.BorrarHistorialAmec(EstadosAmec.AmecSometido.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.Someter.GetHashCode());
                agAmec.BorrarHistorialAmec(EstadosAmec.PendienteSupJerarquicoCreador.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.SupJerarquicoCreador.GetHashCode());
                agAmec.BorrarHistorialAmec(estadoSupJer.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.SuperiorJerarquico.GetHashCode());
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
                return new List<string> { agAprobador.DestinatariosSupJer(idPeticionario).ToString() };
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}