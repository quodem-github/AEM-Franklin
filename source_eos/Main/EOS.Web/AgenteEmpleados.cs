using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Logica;

namespace EOS.Web
{
    public class AgenteEmpleados : AgenteBase
    {
        public LogicaEmpleados MiLogicaEmpleados { get; set; }

        public AgenteEmpleados()
        {
            MiLogicaEmpleados = new LogicaEmpleados();
        }

        public ICollection<DEmpleadosGP> ObtenerDatosEmpleadosPorID(int sID)
        {
            AgenteEmpleados agenteEmp = new AgenteEmpleados();

            return MiLogicaEmpleados.ObtenerDatosEmpleadosPorID(sID);
        }

        public ICollection<DEmpleadosGP> ObtenerDatosEmpleadosPorID(string sID)
        {
            AgenteEmpleados agenteEmp = new AgenteEmpleados();

            return MiLogicaEmpleados.ObtenerDatosEmpleadosPorID(sID);
        }

        public DEmpleadosGP ObtenerDatosEmpleadoPorID(string sID)
        {
            AgenteEmpleados agenteEmp = new AgenteEmpleados();

            return MiLogicaEmpleados.ObtenerDatosEmpleadoPorID(sID);
        }
    }
}
