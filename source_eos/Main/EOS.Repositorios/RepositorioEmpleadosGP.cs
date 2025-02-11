using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using System.Data.Common;

namespace EOS.Repositorios
{
    public class RepositorioEmpleadosGP : IRepositorioEmpleadosGP
    {
        public ICollection<DEmpleadosGP> ObtenerDatosEmpleadoPorID(string sID)
        {
            string consulta = "select idempleadogp, nombre, apellido, email from empleadosgp";
            string where = string.Format(" WHERE idempleadogp = '{0}'", sID);
            consulta += where;
            return DEmpleadosGP.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public ICollection<DEmpleadosGP> ObtenerDatosEmpleadoPorID(int idPeticionario)
        {
            string consulta = "select emp.id, emp.idconfempresa, conf.logosuperiorpantalla, conf.nombreagencia, emp.idempleadogp, emp.nombre, emp.apellido, emp.email from empleadosgp emp inner join peticionarios_empleadosgp pet_emp on emp.id = pet_emp.idempleadogp inner join confempresa conf on emp.idconfempresa = conf.idconfempresa ";
            string where = string.Format(" WHERE pet_emp.idpeticionario = '{0}' and conf.inactive=0 ", idPeticionario);
            consulta += where;
                        
            return DEmpleadosGP.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }
    }
}
