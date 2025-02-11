using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EOS.Repositorios;
using EOS.Entidades.Datos;
using System.Data.Common;

namespace EOS.Logica
{
    public class LogicaUsuarios : ILogicaUsuarios
    {
        IRepositorioUsuarios MiRepositorioUsuarios { get; set; }

        public LogicaUsuarios()
        {
            this.MiRepositorioUsuarios = new RepositorioUsuarios();
        }

        public bool CambiarPassword(string nombreUsuario, string claveNueva)
        {
            return (this.MiRepositorioUsuarios.CambiarPassword(nombreUsuario, claveNueva) > 0);
        }

        public bool ValidarUsuario(string login, string clave, string claveHash)
        {
            return MiRepositorioUsuarios.ValidarUsuario(login, clave, claveHash);
        }

        public DataSet ObtenerUsuariosPorCargo(int IdCargo)
        { 
        
        return MiRepositorioUsuarios.ObtenerUsuariosPorCargo(IdCargo);
        }

        public DataSet ObtenerUsuariosComplianceAprobador()
        {
            return MiRepositorioUsuarios.ObtenerUsuariosComplianceAprobador();
        }

        public int EsUsurioDelegado(int IdPeticionario)
        {
            return MiRepositorioUsuarios.EsUsurioDelegado(IdPeticionario);
        }

        public DDatosPersonalesUsuario ObtenerDatosPersonalesPorLogin(string login)
        {
            return MiRepositorioUsuarios.ObtenerDatosPersonalesPorLogin(login);
        }
        public DDatosPersonalesUsuario ObtenerDatosPersonalesPorIDPeticionario(string sIDPeticionario)
        {
            return MiRepositorioUsuarios.ObtenerDatosPersonalesPorIDPeticionario(sIDPeticionario);
        }

        public DataSet ObtenerTodosUsuarios()
        {
            return MiRepositorioUsuarios.ObtenerTodosUsuarios();
        }

        public DataSet ObtenerTodosUsuariosCombo(string idamecs)
        {
            return MiRepositorioUsuarios.ObtenerTodosUsuariosCombo(idamecs);
        }

        public DataSet ObtenerReportesDirectosAssistant(int IdUnidad, int IdCargo)
        {

            return MiRepositorioUsuarios.ObtenerReportesDirectosAssistant(IdUnidad, IdCargo);
        }

        public ICollection<DDatosPersonalesUsuario> ObtenerUsuariosPuedenSerDelegados(int idpeticionario)
        {
            return MiRepositorioUsuarios.ObtenerUsuariosPuedenSerDelegados(idpeticionario);
        }

        public DVPeticionariosRoles ObtenerDatosRolesPorLogin(string login)
        {
            return MiRepositorioUsuarios.ObtenerDatosRolesPorLogin(login);
        }

        public DPosition ObtenerCargoPorIdPeticionario(int idPeticionario)
        {
            return MiRepositorioUsuarios.ObtenerCargoPorIdPeticionario(idPeticionario);
        }

        public bool ActualizarDatosPersonalesPrimerLogin(DDatosPersonalesUsuario datos)
        {
            DDatosPersonalesUsuario datosActuales = MiRepositorioUsuarios.ObtenerDatosPersonalesPorLogin(datos.login);
            DDatosPersonalesActualizarUsuarios DatosActulizados = new DDatosPersonalesActualizarUsuarios();


            /////////////////////////////////////////////////
            datosActuales.login = datos.login;
            datosActuales.Apellido1 = datos.Apellido1;
            datosActuales.Apellido2 = datos.Apellido2;
            datosActuales.CodPostal = datos.CodPostal;
            datosActuales.Telefono = datos.Telefono;
            datosActuales.extension = datos.extension;
            datosActuales.Direccion = datos.Direccion;
            datosActuales.Email = datos.Email;
            if (!datosActuales.fechaprimeraccesoportal.HasValue) { datosActuales.fechaprimeraccesoportal = System.DateTime.Now; }
            datosActuales.Movil = datos.Movil;
            datosActuales.Nombre = datos.Nombre;
            datosActuales.password = datos.password;
            datosActuales.FKIdEmpresa = datos.FKIdEmpresa;
            datosActuales.IdPoblacion = datos.IdPoblacion;
            //Ismael Ameller 20/04/2011 Añadimos AMEX al peticionario
            datosActuales.amex = datos.amex;
            datosActuales.fechacaducidad = datos.fechacaducidad;
            //FIN Ismael Ameller 20/04/2011 Añadimos AMEX al peticionario
            ////////////////////////////////////////////////////


            DatosActulizados.login = datosActuales.login;
            DatosActulizados.Apellido1 = datosActuales.Apellido1;
            DatosActulizados.Apellido2 = datosActuales.Apellido2;
            DatosActulizados.CodPostal = datosActuales.CodPostal;
            DatosActulizados.Telefono = datosActuales.Telefono;
            DatosActulizados.extension = datosActuales.extension;
            DatosActulizados.Direccion = datosActuales.Direccion;
            DatosActulizados.Email = datosActuales.Email;
            DatosActulizados.fechaprimeraccesoportal = datosActuales.fechaprimeraccesoportal;
            DatosActulizados.Movil = datosActuales.Movil;
            DatosActulizados.Nombre = datosActuales.Nombre;
            DatosActulizados.password = datosActuales.password;
            DatosActulizados.FKIdEmpresa = datosActuales.FKIdEmpresa;
            DatosActulizados.IdPoblacion = datosActuales.IdPoblacion;
            DatosActulizados.amex = datosActuales.amex;
            DatosActulizados.fechacaducidad = datosActuales.fechacaducidad;
            DatosActulizados.IdPeticionario = datosActuales.IdPeticionario;


            bool salida = false;
            try
            {
                salida = (MiRepositorioUsuarios.ActualizarDatosPersonales(DatosActulizados) > 0);
            }
            catch (Exception)
            {
                throw;
            }

            return salida;
        }


        public bool EsPrimerLogin(string login)
        {
            DDatosPersonalesUsuario datos = this.MiRepositorioUsuarios.ObtenerDatosPersonalesPorLogin(login);
            return !datos.fechaprimeraccesoportal.HasValue;
        }

        public string ObtenerElLoginMedianteEmail(string email)
        {
            return MiRepositorioUsuarios.ObtenerElLoginMedianteEmail(email);
        }

    }
}
