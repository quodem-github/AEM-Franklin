using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Entidades.Mapeadores;
using EOS.Repositorios;


namespace EOS.Logica
{
    public class LogicaUnidadesOrgAMEC : ILogicaUnidadesOrgAMEC
    {
        private IRepositorioUnidadesOrgAMEC MiRepositorioUnidades { get; set; }

        public LogicaUnidadesOrgAMEC()
        {
            MiRepositorioUnidades = new RepositorioUnidadesOrgAMEC();
        }

        public bool ExisteAprobador(int unidad, int area, int region, int distrito)
        {
            return MiRepositorioUnidades.ExisteAprobador(unidad, area, region, distrito);
        }

        public int InsertUnidadOrgAMEC(FiltroUnidadesOrgAMEC filtroUnidades, DVPeticionariosRoles datosRoles)
        {
            return MiRepositorioUnidades.InsertUnidadOrgAMEC(filtroUnidades);
        }

        public bool ComprobarDuplicidadUnidadOrgAMEC(int? idamec, string idunidad, string idarea, string idregion, string iddistrito)
        {
            return MiRepositorioUnidades.ComprobarDuplicidadUnidadOrgAMEC( idamec, idunidad, idarea, idregion, iddistrito);
        }

        public ICollection<DUnidadesOrganizativasAmec> ObtenerUnidadesOrgAMEC(FiltroUnidadesOrgAMEC filtroUnidades)
        {
            
            return MiRepositorioUnidades.ObtenerUnidadesOrgAMEC(filtroUnidades);
        }

        public ICollection<DUnidadesOrganizativasAmec> ObtenerUnidadesOrgUsu(FiltroUnidadesOrgAMEC filtroUnidades)
        {

            return MiRepositorioUnidades.ObtenerUnidadesOrgUsu(filtroUnidades);
        }

        public long ObtenerNumeroUnidadesOrgAMEC(FiltroUnidadesOrgAMEC filtroUnidades)
        {
            //filtro.Roles = ObtenerFiltroRol(datosRoles, "exp");
            return MiRepositorioUnidades.ObtenerNumeroUnidadesOrgAMEC(filtroUnidades); 
        }

        public int EliminarUnidadOrganizativaAMEC(long idunidadamec, DVPeticionariosRoles datosRoles)
        {
            return MiRepositorioUnidades.EliminarUnidadOrganizativaAMEC(idunidadamec);
        }



        public void ModificarUnidadOrgId(DUnidadesOrganizativasAmec idunidadorgAMEC)
        {
            MiRepositorioUnidades.ModificarUnidadOrgId(idunidadorgAMEC);
        }




        public string ObtenerNombreUnidadxId(int idunidad)
        {
            string NombreUnidad = MiRepositorioUnidades.ObtenerNombreUnidadxId(idunidad);
            return NombreUnidad;
        }


       public string ObtenerNombreAreaxId(int idarea)
        {
            string NombreArea = MiRepositorioUnidades.ObtenerNombreAreaxId(idarea);
            return NombreArea;
        }

        public string ObtenerNombreRegionxId(int idregion)
        {
            string NombreRegion = MiRepositorioUnidades.ObtenerNombreRegionxId(idregion);
            return NombreRegion;
        }

       public string ObtenerNombreDistritoxId(int iddistrito)
        {
            string NombreDistrito = MiRepositorioUnidades.ObtenerNombreDistritoxId(iddistrito);
            return NombreDistrito;
        }


    }
}
