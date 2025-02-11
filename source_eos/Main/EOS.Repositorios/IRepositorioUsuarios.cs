using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EOS.Entidades.Datos;


namespace EOS.Repositorios
{
    public interface IRepositorioUsuarios
    {
        DDatosPersonalesUsuario ObtenerDatosPersonalesPorLogin(string login);
        DDatosPersonalesUsuario ObtenerDatosPersonalesPorIDPeticionario(string sIDPeticionario);

        DataSet ObtenerTodosUsuarios();
        DataSet ObtenerTodosUsuariosCombo(string idamecs);
        ICollection<DDatosPersonalesUsuario> ObtenerUsuariosPuedenSerDelegados(int idpeticionario);
        string ObtenerContraseñaPorLogin(string login);
        bool ValidarUsuario(string login, string password, string passwordHash);

        int CambiarPassword(string nombreUsuario, string claveNueva);

        DVPeticionariosRoles ObtenerDatosRolesPorLogin(string login);

        DataSet ObtenerUsuariosPorCargo(int IdCargo);
        DataSet ObtenerReportesDirectosAssistant(int IdUnidad, int IdCargo);

        int ActualizarDatosPersonales(DDatosPersonalesActualizarUsuarios DatosPersonales);

        DataSet ObtenerUsuariosComplianceAprobador();

        int EsUsurioDelegado(int IdPeticionario);

        DPosition ObtenerCargoPorIdPeticionario(int idPeticionario);

        string ObtenerElLoginMedianteEmail(string email);
    }
}
