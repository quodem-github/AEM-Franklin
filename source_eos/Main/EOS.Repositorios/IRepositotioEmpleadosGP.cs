using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;


namespace EOS.Repositorios
{
    public interface IRepositorioEmpleadosGP
    {
        ICollection<DEmpleadosGP> ObtenerDatosEmpleadoPorID(string sID);
        ICollection<DEmpleadosGP> ObtenerDatosEmpleadoPorID(int idpeticionario);
    }
}