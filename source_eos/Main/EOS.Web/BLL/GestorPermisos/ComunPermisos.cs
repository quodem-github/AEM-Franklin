using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Web.Enums;
using EOS.Entidades.Datos;
using EOS.Web;

namespace EOS.Web.BLL.GestorPermisos.Permisos
{
    public static class ComunPermisos
    {
        public static void CambiarEstadoAmec(string idamec, AgenteAmecInfo agenteAmec, EstadosAmec estadoAmec,
            NivelesAprobacion nivelAprobacion)
        {
            try
            {
                DAmecInfo amec = agenteAmec.CargarTodosValoresAmec(idamec);

                bool preaprobadoLeg = amec.preaprobadaleg != null && amec.preaprobadaleg.Value;
                bool preaprobadoMed = amec.preaprobadaamed != null && amec.preaprobadaamed.Value
                                      && amec.idtipoactividad != 17 && amec.idtipoactividad != 18 &&
                                      amec.idtipoactividad != 9;

                if (preaprobadoMed && amec.idestado == EstadosAmec.PendienteNegocio.GetHashCode() &&
                    estadoAmec != EstadosAmec.Rechazado)
                {
                    //Si el amec es tipo 9 17 18 me lo salto
                    estadoAmec = EstadosAmec.PendienteDtoLegal;
                    nivelAprobacion = NivelesAprobacion.DepartamentoLegal;
                }

                if (preaprobadoLeg && estadoAmec != EstadosAmec.Rechazado)
                {
                    if (preaprobadoMed && amec.idestado == EstadosAmec.PendienteNegocio.GetHashCode())
                    {
                        estadoAmec = EstadosAmec.Aprobado;
                        nivelAprobacion = NivelesAprobacion.Aprobado;
                    }
                    if (amec.idestado == EstadosAmec.PendienteDtoMedico.GetHashCode())
                    {
                        estadoAmec = EstadosAmec.Aprobado;
                        nivelAprobacion = NivelesAprobacion.Aprobado;
                    }
                }

                agenteAmec.CambiarEstadoAmec(estadoAmec.GetHashCode().ToString(), idamec.ToString(),
                    nivelAprobacion.GetHashCode());

            }
            catch (Exception)
            {

                throw;
            }

        }

        public static bool EsSolicitanteDirector(string idamec)
        {
            try
            {
                AgenteUsuarios agUsuarios = new AgenteUsuarios();
                AgenteAmecInfo agAmec = new AgenteAmecInfo();
                DAmecInfo amec = agAmec.CargarTodosValoresAmec(idamec);
                if (amec.idsolicitante != null)
                {
                    int idSolicitante = amec.idsolicitante.Value;
                    DDatosPersonalesUsuario cargoSolicitante =
                        agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(idSolicitante.ToString());
                    bool executive = cargoSolicitante.executive != null ? cargoSolicitante.executive.Value : false;
                    bool director = cargoSolicitante.director != null ? cargoSolicitante.director.Value : false;
                    return executive || director;

                }
                return false;

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
