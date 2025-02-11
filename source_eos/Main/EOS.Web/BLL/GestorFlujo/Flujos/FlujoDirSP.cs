using System;
using System.Collections.Generic;
using System.Linq;
using EOS.Web.Enums;
using EOS.Entidades.Datos;
using EOS.Web;

namespace EOS.Web.BLL.GestorFlujo.Flujo
{
    /// <summary>
    /// Director Sin Paraguas
    /// </summary>
    public class FlujoDirSP : IFlujo
    {

        public bool EsDifSolicitante {get{return false;}}
        public bool ConParaguas {get{return false;}}
        public bool EsAsistente {get{return false;}}
        public bool EsDelegado {get{return false;}}
        public bool EsDirector {get{return true;}}
        public bool EstaSometido {get{return true;}}
        public bool SolicitanteDirector { get { return true; } }

        public bool BuscarFlujo(CondicionesFlujo condiciones)
        {
            return ConParaguas == condiciones.ConParaguas &&
                EsDifSolicitante == condiciones.EsDifSolicitante &&
                EsDelegado == condiciones.EsDelegado &&
                EsAsistente == condiciones.EsAsistente &&
                EsDirector == condiciones.EsDirector ||
                (ConParaguas == condiciones.ConParaguas &&
                EstaSometido == condiciones.EstaSometido &&
                SolicitanteDirector == condiciones.SolicitanteDirector);
        }
        public bool SometerAmec(CondicionesFlujo condiciones, string idamec, int idPeticionario, int idCreador)
        {
            AgenteAmecInfo agAmec = new AgenteAmecInfo();
            EstadosAmec estadoMedico = condiciones.PreMedico ? EstadosAmec.PreaprobadoDtoMedico : EstadosAmec.PendienteDtoMedico;
            EstadosAmec estadoLegal = condiciones.PreLegal ? EstadosAmec.PreaprobadoDtoLegal : EstadosAmec.PendienteDtoLegal;
            try
            {
                AgenteUsuarios agUsuarios = new AgenteUsuarios();
                DDatosPersonalesUsuario usuario = agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(idPeticionario.ToString());

                AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
                List<DAprobadoresAmec> aprobadores = agAprobador.ObernerAprobadoresAmec(idamec.ToString(), null);

                agAmec.InsertarHistorialAmec(EstadosAmec.AmecSometido.GetHashCode(), idamec, usuario, NivelesAprobacion.Someter.GetHashCode(), null);
                //Estado pendiente superior jerarquico
                agAmec.CambiarEstadoAmec(EstadosAmec.PendienteNegocio.GetHashCode().ToString(), idamec.ToString(), NivelesAprobacion.Negocio.GetHashCode());
                agAmec.InsertarHistorialAmec(EstadosAmec.PendienteNegocio.GetHashCode(), idamec, usuario, NivelesAprobacion.Negocio.GetHashCode(), aprobadores.First().IdAprobador);

                //Estados dependiendo de las preaprobaciones en el historial del amec
                
                //Inserciones en el historial
                agAmec.InsertarHistorialAmec(estadoMedico.GetHashCode(), idamec, usuario, NivelesAprobacion.DepartamentoMedico.GetHashCode(), null);
                agAmec.InsertarHistorialAmec(estadoLegal.GetHashCode(), idamec, usuario, NivelesAprobacion.DepartamentoLegal.GetHashCode(), null);

                return true;
            }
            catch (Exception ex)
            {
                agAmec.BorrarHistorialAmec(EstadosAmec.AmecSometido.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.Someter.GetHashCode());
                agAmec.BorrarHistorialAmec(EstadosAmec.PendienteNegocio.GetHashCode(), idamec, idPeticionario, NivelesAprobacion.Negocio.GetHashCode());
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