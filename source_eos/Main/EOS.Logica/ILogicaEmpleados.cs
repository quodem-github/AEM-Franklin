using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;

namespace EOS.Logica
{
    public interface ILogicaEmpleados
    {
        ICollection<DEmpleadosGP> ObtenerDatosEmpleadosPorID(string sID);
        ICollection<DEmpleadosGP> ObtenerDatosEmpleadosPorID(int idpeticionario);
        DEmpleadosGP ObtenerDatosEmpleadoPorID(string sID);
    }
}