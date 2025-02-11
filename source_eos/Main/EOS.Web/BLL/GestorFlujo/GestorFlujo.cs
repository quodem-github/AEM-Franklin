using System;
using System.Collections.Generic;
using System.Linq;
using EOS.Web.BLL.GestorFlujo.Flujo;
namespace EOS.Web.BLL.GestorFlujo
{
    public class GestorFlujo
    {
        private readonly List<IFlujo> _flujos;

        public GestorFlujo()
        {
            _flujos = new List<IFlujo>
            {
                new FlujoDSP(),
                new FlujoDCP(),
                new FlujoACP(),
                new FlujoASP(),
                new FlujoDADCP(),
                new FlujoDADSP(),
                new FlujoDirCP(),
                new FlujoDirSP(),
            };
        }

        public IFlujo ObtenerFlujo(CondicionesFlujo condiciones)
        {
            return _flujos.FirstOrDefault(x => x.BuscarFlujo(condiciones));
        }
    }
}