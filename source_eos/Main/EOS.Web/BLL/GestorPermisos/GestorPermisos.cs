using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Web.Enums;
using EOS.Web;

namespace EOS.Web.BLL.GestorPermisos.Permisos
{
    public class GestorPermisos
    {
        private readonly List<IPermiso> _permisos;

        public GestorPermisos()
        {
            AgenteAprobadorAmec agenteAprobadorAmec = new AgenteAprobadorAmec();
            _permisos = new List<IPermiso>
            {
                new PermisoBorrador(agenteAprobadorAmec),
                new PermisoLegal(agenteAprobadorAmec),
                new PermisoNegocio(agenteAprobadorAmec),
                new PermisoSupJer(agenteAprobadorAmec),
                new PermisoMedico(agenteAprobadorAmec),
                new PermisosAprobado(agenteAprobadorAmec),
                new PermisoMultiaprobacion(agenteAprobadorAmec),
                new PermisoSupJerCreador(agenteAprobadorAmec),
                new PermisoSupJerAssistant(agenteAprobadorAmec),
                new PermisoNegocioAssistant(agenteAprobadorAmec),
                new PermisoPendienteSometer(agenteAprobadorAmec),
                new PermisoRechazado(agenteAprobadorAmec),
                new PermisoCancelado(agenteAprobadorAmec)
            };
        }

        public IPermiso GetPermisos(int estado)
        {
            return _permisos.FirstOrDefault(x => x.BuscarPermisos(estado));
        }
    }
}
