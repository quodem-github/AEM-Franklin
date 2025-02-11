using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EOS.Entidades.Datos;
using System.Web;
using EOS.Logica;


namespace EOS.Web
{
    public class AgenteUsuarios
    {
        public LogicaUsuarios MiLogicaUsuarios { get; set; }

        public AgenteUsuarios()
        {
            MiLogicaUsuarios = new LogicaUsuarios();
        }

        private static DDatosPersonalesUsuario DatosUsuarioEnSession
        {
            get
            {
                if (HttpContext.Current.Session == null) return null;

                return HttpContext.Current.Session["DatosUsuario"] as DDatosPersonalesUsuario;
                
               
            }
            set
            {
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["DatosUsuario"] = value;
                }
            }
        }


        private static DVPeticionariosRoles DatosRolEnSession
        {
            get
            {
                if (HttpContext.Current.Session == null) return null;

                return HttpContext.Current.Session["DatosRol"] as DVPeticionariosRoles;
            }
            set
            {
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["DatosRol"] = value;
                }
            }
        }



        public int EsUsurioDelegado(int IdPeticionario)
        {
            return MiLogicaUsuarios.EsUsurioDelegado(IdPeticionario);
        }

        public DDatosPersonalesUsuario ObtenerDatosPersonalesPorIDPeticionario(string sIDPeticionario)
        {
            return MiLogicaUsuarios.ObtenerDatosPersonalesPorIDPeticionario(sIDPeticionario);
        }

        public DataSet ObtenerUsuariosComplianceAprobador()
        {
            return MiLogicaUsuarios.ObtenerUsuariosComplianceAprobador();
        }

        public DataSet ObtenerUsuariosPorCargo(int IdCargo)
        {
            return MiLogicaUsuarios.ObtenerUsuariosPorCargo(IdCargo);
        }
        public DataSet ObtenerTodosUsuarios()
        {
            return MiLogicaUsuarios.ObtenerTodosUsuarios();
        }

        public DataSet ObtenerTodosUsuariosCombo(string idamecs)
        {
            return MiLogicaUsuarios.ObtenerTodosUsuariosCombo(idamecs);
        }

        public ICollection<DDatosPersonalesUsuario> ObtenerUsuariosPuedenSerDelegados(int idpeticionario)
        {
            return MiLogicaUsuarios.ObtenerUsuariosPuedenSerDelegados(idpeticionario);
        }

        public DataSet ObtenerReportesDirectosAssistant(int IdUnidad, int IdCargo)
        {

            return MiLogicaUsuarios.ObtenerReportesDirectosAssistant(IdUnidad, IdCargo);
        }

        public DDatosPersonalesUsuario ObtenerDatosPersonalesPorLogin()
        {           
            string usuLogin = HttpContext.Current.User.Identity.Name;
            return ObtenerDatosPersonalesPorLogin(usuLogin);
        }

        public DDatosPersonalesUsuario ObtenerDatosPersonalesPorLogin(string usuLogin)
        {
            ILogicaUsuarios logica = new LogicaUsuarios();
            DDatosPersonalesUsuario datosSalida = DatosUsuarioEnSession;
            if ((datosSalida == null) || (datosSalida.login != usuLogin))
            {

                datosSalida = logica.ObtenerDatosPersonalesPorLogin(usuLogin);
                DatosUsuarioEnSession = datosSalida;
            }
            return datosSalida;
        }

        public DVPeticionariosRoles ObtenerDatosRolesPorLogin()
        {
            string usuLogin = HttpContext.Current.User.Identity.Name;
            return ObtenerDatosRolesPorLogin(usuLogin);
        }

        public DVPeticionariosRoles ObtenerDatosRolesPorLogin(string usuLogin)
        {
            ILogicaUsuarios logica = new LogicaUsuarios();
            DVPeticionariosRoles datosSalida = DatosRolEnSession;
            if ((datosSalida == null) || (datosSalida.login != usuLogin))
            {
                datosSalida = logica.ObtenerDatosRolesPorLogin(usuLogin);
                DatosRolEnSession = datosSalida;
            }
            return datosSalida;
        }

        public bool IsAdmin()
        {
            bool result = false;
            DVPeticionariosRoles pet = ObtenerDatosRolesPorLogin(HttpContext.Current.User.Identity.Name);
            if (pet != null && pet.administrador == true)
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarDatosPersonalesPrimerLogin(DDatosPersonalesUsuario datos)
        {
            ILogicaUsuarios logica = new LogicaUsuarios();
            return logica.ActualizarDatosPersonalesPrimerLogin(datos);
        }

        public bool EsPrimerLogin(string usuario)
        {
            DDatosPersonalesUsuario datos = ObtenerDatosPersonalesPorLogin(usuario);
            return ! datos.fechaprimeraccesoportal.HasValue;
        }

        public string NavegacionDesdeLogin()
        {
            DVPeticionariosRoles datosRoles = ObtenerDatosRolesPorLogin();
            string sNavegacion = "Expedientes.aspx";

            if (datosRoles.aprobador.HasValue)
            {
                if (datosRoles.aprobador.Value)
                    sNavegacion = "ListadoAMECs.aspx";
            }
            return sNavegacion;                    
        }

        public DPosition ObtenerCargoPorIdPeticionario(int idPeticionario)
        {
            return MiLogicaUsuarios.ObtenerCargoPorIdPeticionario(idPeticionario);
        }

    }
}
