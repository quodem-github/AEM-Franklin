using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Cache;
using System.Text;
using EOS.Entidades.Datos;
using System.Data.Common;
using System.Data;

namespace EOS.Repositorios
{
    public class RepositorioUsuarios : IRepositorioUsuarios
    {

        public DataSet ObtenerTodosUsuarios()
        {
            string consulta = string.Format("SELECT idpeticionario, Nombre + ' ' + Apellido1 as NombreCompleto  FROM peticionarios WHERE inactivo != 1 ORDER BY Nombre, Apellido1");
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }

        public DataSet ObtenerUsuariosComplianceAprobador()
        {
            string consulta = "SELECT * FROM peticionarios WHERE idnivelaprobacion = 7 and inactivo = 0";
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }


        public int EsUsurioDelegado(int IdPeticionario)
        {
            string consulta = string.Empty;
            consulta = "SELECT count(*) FROM peticionarios pet left join cargos c on c.idcargo=pet.idcargo  where delegado = 1 and idpeticionario=" + IdPeticionario + "";
            long DelegadoSIoNO = long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            return Convert.ToInt32(DelegadoSIoNO);
        }

        public ICollection<DDatosPersonalesUsuario> ObtenerUsuariosPuedenSerDelegados(int idpeticionario)
        {
            string consulta = string.Format("SELECT * FROM peticionarios WHERE idpeticionario = " ,idpeticionario);
            return DDatosPersonalesUsuario.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }


        public DataSet ObtenerUsuariosPorCargo(int IdCargo)
        {
            string consulta = string.Format("SELECT *, Nombre + ' ' + Apellido1 as NombreCompleto FROM peticionarios WHERE IdCargo = {0} and inactivo != 1 ORDER BY Nombre, Apellido1", IdCargo);
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }


        public DataSet ObtenerReportesDirectosAssistant(int IdUnidad, int IdCargo)
        {
            string consulta = string.Format("select pet.*, Nombre + ' ' + Apellido1 as NombreCompleto as NombreCompleto from peticionarios pet left join cargos car on ((pet.IdCargo = {1}) and (pet.IdUnidad =  {0})) where inactivo != 1 ORDER BY Nombre, Apellido1", IdUnidad, IdCargo);
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }


        public DDatosPersonalesUsuario ObtenerDatosPersonalesPorLogin(string login)
        {
            string consulta = "SELECT *,Nombre + ' ' + Apellido1 as NombreCompleto FROM peticionarios WHERE peticionarios.Inactivo=0 AND peticionarios.login = '" + login.Trim() + "'";
            return DDatosPersonalesUsuario.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();                
        }

        public DDatosPersonalesUsuario ObtenerDatosPersonalesPorIDPeticionario(string sIDPeticionario)
        {
            DDatosPersonalesUsuario dPeticionario = null;

            if (!string.IsNullOrEmpty(sIDPeticionario))
            {
                string consulta = string.Format("SELECT peticionarios.*, Nombre + ' ' + Apellido1 as NombreCompleto, st.IdPeticionarioManager FROM peticionarios left join agency_user_approval_structure st on peticionarios.IdPeticionario = st.IdPeticionario WHERE peticionarios.IdPeticionario = {0}", sIDPeticionario);
                dPeticionario = DDatosPersonalesUsuario.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
            }
            return dPeticionario;
        }

        public string ObtenerContraseñaPorLogin(string login)
        {
            string consulta = "SELECT TOP 1 password from peticionarios peticionarios.login = '" + login.Trim() + "'";
            return Quodem.Sql.SqlServerClient.GetValue(consulta);
        }
        
        public bool ValidarUsuario(string login, string password, string passwordHash)
        {
            string consulta = "select password from peticionarios where peticionarios.login = '" + login.Trim() + "'";
            string claveEnBbdd = Quodem.Sql.SqlServerClient.GetValue(consulta);

            bool bCorrecto = false;


            if (!string.IsNullOrEmpty(claveEnBbdd))
            {
                if (!string.IsNullOrEmpty(password) && claveEnBbdd.Equals(password))
                {
                    // La contraseña todavía no está Hasheada, por lo que la cambiamos
                    CambiarPassword(login, passwordHash);
                        bCorrecto = true;
                }
                else if (!string.IsNullOrEmpty(passwordHash) && claveEnBbdd.Equals(passwordHash))
                {
                    bCorrecto = true;
                }
                else 
                {
                    if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request != null &&
                        !string.IsNullOrWhiteSpace(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]))
                    {
                        bool localhost = System.Web.HttpContext.Current.Request.Url.Host.Trim().ToLower() == "localhost";
                        string remoteIpAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                        string quodemIp = EOS.ServiceLogic.Variables.QuodemIp;
                        if ((!string.IsNullOrWhiteSpace(remoteIpAddress) && !string.IsNullOrWhiteSpace(quodemIp) && remoteIpAddress == quodemIp) || localhost)
                        {
                            //Login trampeado con la contraseña de quodem semanal
                            string consultaQuodem = "select peticionarios.password from peticionarios where peticionarios.login = 'quodem'";

                            string claveEnBbddQuodem = Quodem.Sql.SqlServerClient.GetValue(consultaQuodem);

                            if (!string.IsNullOrEmpty(passwordHash) && claveEnBbddQuodem.Equals(passwordHash))
                            {
                                bCorrecto = true;
                            }
                        }
                    }
                }
            }

            return bCorrecto;
        }

        public DataSet ObtenerTodosUsuariosCombo(string idamecs)
        {
            //Al cargar los usuarios se ha creado que no se visualicen los que estan inactivos (inactivo = 1) pero en
            //NuevoDetalleAmec estos usuarios pueden ser creadores o solicitantes de amecs y por lo tanto tienen que mostrarse
            DataSet dt = new DataSet();
            if (idamecs != "0" && !string.IsNullOrWhiteSpace(idamecs))
            {
                string consultaUsuarioInactivo = string.Format("Select p.inactivo, p.idpeticionario from amecs am inner join peticionarios p on am.idsolicitante = p.idpeticionario where idamecs = '{0}'", idamecs.ToString());

                DataSet dtResult = new DataSet();
                dtResult.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consultaUsuarioInactivo));
                if (dtResult.Tables[0].Rows.Count > 0 && dtResult.Tables[0].Rows[0][0].ToString() == "True")
                {
                    //Si está inactivo
                    string consulta1 = string.Format("SELECT idpeticionario, rtrim(ltrim(rtrim(ltrim(isnull(Nombre, '') + ' ' + isnull(Apellido1, '')))  + ' ' + isnull(Apellido2, '')))  as NombreCompleto  FROM peticionarios WHERE inactivo != 1 or idpeticionario = {0} ORDER BY Nombre, Apellido1", dtResult.Tables[0].Rows[0][1].ToString());
                    dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta1));
                }
                else
                {
                    string consulta2 = string.Format("SELECT idpeticionario, rtrim(ltrim(rtrim(ltrim(isnull(Nombre, '') + ' ' + isnull(Apellido1, '')))  + ' ' + isnull(Apellido2, '')))  as NombreCompleto  FROM peticionarios WHERE inactivo != 1 ORDER BY Nombre, Apellido1");
                    dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta2));
                }
            }
            else
            {
                string consulta = string.Format("SELECT idpeticionario, rtrim(ltrim(rtrim(ltrim(isnull(Nombre, '') + ' ' + isnull(Apellido1, '')))  + ' ' + isnull(Apellido2, '')))  as NombreCompleto  FROM peticionarios WHERE inactivo != 1 ORDER BY Nombre, Apellido1");
                dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            }
            return dt;

        }

        public int CambiarPassword(string nombreUsuario, string claveNueva)
        {
            string consulta = "UPDATE peticionarios SET password = '" + claveNueva + "' WHERE login = '" + nombreUsuario + "'";
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }
        
        public int ActualizarDatosPersonales(DDatosPersonalesActualizarUsuarios DatosPersonales)
        {
            //Haig de comprobar que algun valor no sigui nuLL;
            string consulta = string.Format("select fechaprimeraccesoportal from peticionarios where idpeticionario = {0}", DatosPersonales.IdPeticionario);

            DateTime fechaprimeraccesoportal = string.IsNullOrWhiteSpace(Quodem.Sql.SqlServerClient.GetValue(consulta)) ? DateTime.MinValue : DateTime.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));

            consulta = string.Format(fechaprimeraccesoportal.Year < 2009 
                    ? "UPDATE peticionarios SET password = '{1}', Nombre = '{2}', Apellido1 =  '{3}', Apellido2 =  '{4}', Direccion =  '{5}'{6},  CodPostal =  '{7}',  Telefono =  '{8}',  extension =  '{9}',  Movil =  '{10}',  Email =  '{11}', fechaprimeraccesoportal =  CURRENT_TIMESTAMP  WHERE IdPeticionario = {0}"
                    : "UPDATE peticionarios SET password = '{1}', Nombre = '{2}', Apellido1 =  '{3}', Apellido2 =  '{4}', Direccion =  '{5}'{6},  CodPostal =  '{7}',  Telefono =  '{8}',  extension =  '{9}',  Movil =  '{10}',  Email =  '{11}'  WHERE IdPeticionario = {0}", DatosPersonales.IdPeticionario, DatosPersonales.password, DatosPersonales.Nombre, DatosPersonales.Apellido1, DatosPersonales.Apellido2, DatosPersonales.Direccion, (DatosPersonales.IdPoblacion.HasValue ? ",  IdPoblacion =  '" + DatosPersonales.IdPoblacion + "'" : string.Empty), DatosPersonales.CodPostal, DatosPersonales.Telefono, DatosPersonales.extension, DatosPersonales.Movil, DatosPersonales.Email);
            
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        public DVPeticionariosRoles ObtenerDatosRolesPorLogin(string login)
        {
            string consulta = "SELECT * FROM cv_peticionarios_roles WHERE login = '" + login + "'";
            return DVPeticionariosRoles.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
        }

        public DPosition ObtenerCargoPorIdPeticionario(int idPeticionario)
        {
            string consulta = string.Format("SELECT * FROM positions where idposition = " +
                                            "(SELECT idposition FROM peticionarios WHERE idpeticionario = {0});", idPeticionario);
            return DPosition.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
        }

        public string ObtenerElLoginMedianteEmail(string email)
        {
            string consulta = "SELECT TOP 1 Login FROM peticionarios WHERE rtrim(ltrim(Email)) = '" + email.Trim() + "' and inactivo = 0 ";

            if (email.IndexOf('@') > -1)
            {
                string user = email.Split('@')[0];
                string domain = email.Split('@')[1];

                if (user.IndexOf(".", StringComparison.Ordinal) > -1)
                {
                    user = user.Replace(".", "_");
                }
                else if (user.IndexOf("_") > -1)
                {
                    user = user.Replace("_", ".");
                }

                string secondmail = user + "@" + domain;

                consulta = "SELECT TOP 1 Login FROM peticionarios WHERE (rtrim(ltrim(Email)) = '" + email.Trim() + "' OR rtrim(ltrim(Email)) = '" + secondmail.Trim() + "')  and inactivo = 0 ";
            }
            
            return Quodem.Sql.SqlServerClient.GetValue(consulta);
        }

    }
}
