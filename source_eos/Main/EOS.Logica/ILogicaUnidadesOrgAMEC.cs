using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;

namespace EOS.Logica
{
    public interface ILogicaUnidadesOrgAMEC
    {

        bool ExisteAprobador(int unidad, int area, int region, int distrito);
        int InsertUnidadOrgAMEC(FiltroUnidadesOrgAMEC filtroUnidades, DVPeticionariosRoles datosRoles);
        bool ComprobarDuplicidadUnidadOrgAMEC(int? idamec, string idunidad, string idarea, string idregion, string iddistrito);
        ICollection<DUnidadesOrganizativasAmec> ObtenerUnidadesOrgAMEC(FiltroUnidadesOrgAMEC filtroUnidades);
        ICollection<DUnidadesOrganizativasAmec> ObtenerUnidadesOrgUsu(FiltroUnidadesOrgAMEC filtroUnidades);
        long ObtenerNumeroUnidadesOrgAMEC(FiltroUnidadesOrgAMEC filtroUnidades);
        int EliminarUnidadOrganizativaAMEC(long idunidadamec, DVPeticionariosRoles datosRoles);
        void ModificarUnidadOrgId(DUnidadesOrganizativasAmec idunidadorgAMEC);


        string ObtenerNombreUnidadxId(int idunidad);
        string ObtenerNombreAreaxId(int idarea);
        string ObtenerNombreRegionxId(int idregion);
        string ObtenerNombreDistritoxId(int iddistrito);
    }
}
