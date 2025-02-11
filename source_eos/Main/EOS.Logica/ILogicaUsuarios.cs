using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EOS.Entidades.Datos;

namespace EOS.Logica
{
    public interface ILogicaUsuarios
    {
        bool CambiarPassword(string nombreUsuario, string claveNueva);

        bool ValidarUsuario(string login, string clave, string claveHash);

        DDatosPersonalesUsuario ObtenerDatosPersonalesPorLogin(string login);
        DDatosPersonalesUsuario ObtenerDatosPersonalesPorIDPeticionario(string sIDPeticionario);

        DataSet ObtenerTodosUsuarios();
        DataSet ObtenerTodosUsuariosCombo(string idamecs);
        ICollection<DDatosPersonalesUsuario> ObtenerUsuariosPuedenSerDelegados(int idpeticionario);

        bool ActualizarDatosPersonalesPrimerLogin(DDatosPersonalesUsuario datos);

        bool EsPrimerLogin(string login);

        DVPeticionariosRoles ObtenerDatosRolesPorLogin(string login);

        DataSet ObtenerUsuariosPorCargo(int IdCargo);
        DataSet ObtenerReportesDirectosAssistant(int IdUnidad, int IdCargo);

        DataSet ObtenerUsuariosComplianceAprobador();

        int EsUsurioDelegado(int IdPeticionario);

        DPosition ObtenerCargoPorIdPeticionario(int idPeticionario);

        string ObtenerElLoginMedianteEmail(string email);

    }
}
