using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Logica;

namespace EOS.Web
{
   public class AgenteUnidadesOrgAMEC :AgenteBase
    {
        public LogicaUnidadesOrgAMEC MiLogicaUnidadesOrgAMEC { get; set; }
        
        public AgenteUnidadesOrgAMEC()
        {
            MiLogicaUnidadesOrgAMEC = new LogicaUnidadesOrgAMEC();
            
        }


        public bool ComprobarDuplicidadUnidadOrgAMEC(string idamec, string idunidad, string idarea, string idregion, string iddistrito)
        {
 
            int? Idamec = string.IsNullOrEmpty(idamec) ? new Nullable<int>() : int.Parse(idamec);

            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();
            bool duplicado = MiLogicaUnidadesOrgAMEC.ComprobarDuplicidadUnidadOrgAMEC(Idamec, idunidad, idarea, idregion, iddistrito);
            return duplicado;
        }


        public int InsertUnidadOrgAMEC(string filtroidamec, string filtroidcreadopor, string filtroidunidad, string filtroidarea, string filtroidregion, string filtroiddistrito)
        {
            FiltroUnidadesOrgAMEC filtroUnidad =
                new FiltroUnidadesOrgAMEC()
                {
                    IdAmec = filtroidamec,
                    IdUsuario = string.IsNullOrEmpty(filtroidcreadopor) ? new Nullable<int>() : int.Parse(filtroidcreadopor),
                    IdUnidad = string.IsNullOrEmpty(filtroidunidad) ? new Nullable<int>() : int.Parse(filtroidunidad),
                    IdArea = string.IsNullOrEmpty(filtroidarea) ? new Nullable<int>() : int.Parse(filtroidarea),
                    IdRegion = string.IsNullOrEmpty(filtroidregion) ? new Nullable<int>() : int.Parse(filtroidregion),
                    IdDistrito = string.IsNullOrEmpty(filtroiddistrito) ? new Nullable<int>() : int.Parse(filtroiddistrito),
                    
                };

            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();
            int insertat = MiLogicaUnidadesOrgAMEC.InsertUnidadOrgAMEC(filtroUnidad,datosRoles);
            return insertat;
        }

        public ICollection<DUnidadesOrganizativasAmec> ObtenerUnidadesOrgAmec(string filtroidamec, string filtroidsolicitante,string filtroidUnidad, string filtroidArea,string filtroidRegion,string  filtroidDistrito,string filtroAccion, string filtroUnidad, string filtroArea,string filtroRegion,string  filtroDistrito, string sortParameter, int startRowIndex, int maximumRows)
        {
            //Carregar dades
            //Sino entra per primera vegada voldrà dir que estem fent un filtre      
            ICollection<DUnidadesOrganizativasAmec> collectionUnidadesOrgAMEC;

            FiltroUnidadesOrgAMEC UnidadOrganizativa =
               new FiltroUnidadesOrgAMEC()
               {
                   IdAmec = filtroidamec
               };

            collectionUnidadesOrgAMEC = MiLogicaUnidadesOrgAMEC.ObtenerUnidadesOrgAMEC(UnidadOrganizativa);

            if (filtroidUnidad != null || filtroidRegion != null || filtroidDistrito != null || filtroidArea != null)
            {
                if (filtroAccion == "True") //Han seleccionado un solicitante.
                {

                    //Si no obtiene unidades sacamos la de por defecto del usuario/solicitante.
                    if (collectionUnidadesOrgAMEC.Count == 0)
                    {
                        FiltroUnidadesOrgAMEC filtroUnidadUsu =
                        new FiltroUnidadesOrgAMEC()
                        {
                            IdAmec = filtroidamec,
                            IdUsuario = string.IsNullOrEmpty(filtroidsolicitante) ? new Nullable<int>() : int.Parse(filtroidsolicitante),
                        };

                        collectionUnidadesOrgAMEC = MiLogicaUnidadesOrgAMEC.ObtenerUnidadesOrgUsu(filtroUnidadUsu);
                    }


                }
                else //Han seleccionado alguno de los combos de la UO  x defecto.
                {
                    //Si no obtiene unidades sacamos la de por defecto la actual UOx defecto de los combos.
                    if (collectionUnidadesOrgAMEC.Count == 0)
                    {


                        DUnidadesOrganizativasAmec uniOrgXDefecto = new DUnidadesOrganizativasAmec();



                        if (!string.IsNullOrEmpty(filtroidsolicitante.ToString())) uniOrgXDefecto.idcreadopor = Int32.Parse(filtroidsolicitante.ToString());
                        if (!string.IsNullOrEmpty(filtroidamec.ToString())) uniOrgXDefecto.idamecs = filtroidamec.ToString();

                        if (!string.IsNullOrEmpty(filtroidUnidad)) uniOrgXDefecto.idunidad = Int32.Parse(filtroidUnidad.ToString());
                        if (!string.IsNullOrEmpty(filtroidArea)) uniOrgXDefecto.idarea = Int32.Parse(filtroidArea.ToString());
                        if (!string.IsNullOrEmpty(filtroidRegion)) uniOrgXDefecto.idregion = Int32.Parse(filtroidRegion.ToString());
                        if (!string.IsNullOrEmpty(filtroidDistrito)) uniOrgXDefecto.iddistrito = Int32.Parse(filtroidDistrito.ToString());


                        //OBTENER NOMBRE DE unidad, area, region, distrito.

                        if (!string.IsNullOrEmpty(filtroidUnidad)) uniOrgXDefecto.unidad = ObtenerNombreUnidadxId(Int32.Parse(filtroidUnidad.ToString()));
                        if (!string.IsNullOrEmpty(filtroidArea)) uniOrgXDefecto.area = ObtenerNombreAreaxId(Int32.Parse(filtroidArea.ToString()));
                        if (!string.IsNullOrEmpty(filtroidRegion)) uniOrgXDefecto.region = ObtenerNombreRegionxId(Int32.Parse(filtroidRegion.ToString()));
                        if (!string.IsNullOrEmpty(filtroidDistrito)) uniOrgXDefecto.distrito = ObtenerNombreDistritoxId(Int32.Parse(filtroidDistrito.ToString()));

                        collectionUnidadesOrgAMEC.Add(uniOrgXDefecto);

                    }
                }
            }

            return collectionUnidadesOrgAMEC;
            

        }

        public int ObtenerNumeroUnidadesOrgAMEC(string filtroidamec, string filtroidsolicitante, string filtroidUnidad, string filtroidArea, string filtroidRegion, string filtroidDistrito, string filtroAccion, string filtroUnidad, string filtroArea, string filtroRegion, string  filtroDistrito)
        {
            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();
            FiltroUnidadesOrgAMEC UnidadOrganizativa =
                new FiltroUnidadesOrgAMEC()
                {
                    IdAmec = filtroidamec,
                };
            long numUnidadesOrgAMEC = MiLogicaUnidadesOrgAMEC.ObtenerNumeroUnidadesOrgAMEC(UnidadOrganizativa);

            if ((numUnidadesOrgAMEC == 0 || filtroidUnidad != null || filtroidRegion != null || filtroidDistrito != null || filtroidArea != null) && (numUnidadesOrgAMEC == 0 && filtroidUnidad != "" && filtroidRegion != "" && filtroidDistrito != "" && filtroidArea != ""))
            {
                numUnidadesOrgAMEC = 1; //solo una la de por defecto.
            }

            return Convert.ToInt32(numUnidadesOrgAMEC);
        }

        public int EliminarUnidadOrganizativaAMEC(long idunidadamec)
        {
            
            DVPeticionariosRoles datosRoles = ObtenerDatosRoles();

            int resultat = MiLogicaUnidadesOrgAMEC.EliminarUnidadOrganizativaAMEC(idunidadamec, datosRoles);
            return 1;

        }

        private DVPeticionariosRoles ObtenerDatosRoles()
        {
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            return agenteUsu.ObtenerDatosRolesPorLogin();
        }



       public void ModificarUnidadOrgId(DUnidadesOrganizativasAmec unidadxdefecto)
       {
           MiLogicaUnidadesOrgAMEC.ModificarUnidadOrgId(unidadxdefecto);
       }

       public string ObtenerNombreUnidadxId(int idunidad)
       {
           string NombreUnidad = MiLogicaUnidadesOrgAMEC.ObtenerNombreUnidadxId(idunidad);
           return NombreUnidad;
       }


       public string ObtenerNombreAreaxId(int idarea)
       {
           string NombreArea = MiLogicaUnidadesOrgAMEC.ObtenerNombreAreaxId(idarea);
           return NombreArea;
       }

       public string ObtenerNombreRegionxId(int idregion)
       {
           string NombreRegion = MiLogicaUnidadesOrgAMEC.ObtenerNombreRegionxId(idregion);
           return NombreRegion;
       }

       public string ObtenerNombreDistritoxId(int iddistrito)
       {
           string NombreDistrito = MiLogicaUnidadesOrgAMEC.ObtenerNombreDistritoxId(iddistrito);
           return NombreDistrito;
       }

    }
}
