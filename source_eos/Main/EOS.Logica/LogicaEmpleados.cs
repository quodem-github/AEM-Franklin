using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Repositorios;


namespace EOS.Logica
{
    public class LogicaEmpleados : ILogicaEmpleados
    {
        IRepositorioEmpleadosGP MiRepositorioEmpleados { get; set; }

        public LogicaEmpleados()
        {
            this.MiRepositorioEmpleados = new RepositorioEmpleadosGP();
        }

        public ICollection<DEmpleadosGP> ObtenerDatosEmpleadosPorID(string sID)
        {
            return MiRepositorioEmpleados.ObtenerDatosEmpleadoPorID(sID);
        }

        public DEmpleadosGP ObtenerDatosEmpleadoPorID(string sID)
        {
            return MiRepositorioEmpleados.ObtenerDatosEmpleadoPorID(sID).FirstOrDefault();
        }

        public ICollection<DEmpleadosGP> ObtenerDatosEmpleadosPorID(int idpeticionario)
        {
            return MiRepositorioEmpleados.ObtenerDatosEmpleadoPorID(idpeticionario);
        }
    }
}
