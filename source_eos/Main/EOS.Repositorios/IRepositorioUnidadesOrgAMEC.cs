using System.Collections.Generic;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;

namespace EOS.Repositorios
{
    public interface IRepositorioUnidadesOrgAMEC
    {

        ICollection<DUnidadesOrganizativasAmec> ObtenerUnidadesOrgAMEC(FiltroUnidadesOrgAMEC filtroUnidades);
        ICollection<DUnidadesOrganizativasAmec> ObtenerUnidadesOrgUsu(FiltroUnidadesOrgAMEC filtroUnidades);
        bool ExisteAprobador(int unidad, int area, int region, int distrito);
        long ObtenerNumeroUnidadesOrgAMEC(FiltroUnidadesOrgAMEC filtroUnidades);
        int EliminarUnidadOrganizativaAMEC(long idunidadamec);
        int InsertUnidadOrgAMEC(FiltroUnidadesOrgAMEC filtroUnidades);
        bool ComprobarDuplicidadUnidadOrgAMEC(int? idamec, string idunidad, string idarea, string idregion, string iddistrito);
        void ModificarUnidadOrgId(DUnidadesOrganizativasAmec idunidorgAMEC);


        string ObtenerNombreUnidadxId(int idunidad);
        string ObtenerNombreAreaxId(int idarea);
        string ObtenerNombreRegionxId(int idregion);
        string ObtenerNombreDistritoxId(int iddistrito);
    }
}
