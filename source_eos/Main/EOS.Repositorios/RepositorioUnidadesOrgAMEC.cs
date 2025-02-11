using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Policy;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Common;

namespace EOS.Repositorios
{
    public class RepositorioUnidadesOrgAMEC : IRepositorioUnidadesOrgAMEC
    {

        public ICollection<DUnidadesOrganizativasAmec> ObtenerUnidadesOrgAMEC(FiltroUnidadesOrgAMEC filtroUnidades)
        {
            string consulta = string.Format("SELECT uo.idunidadamec,uo.idamecs,uo.idunidad,uo.idarea,uo.idregion, uo.iddistrito, dis.distrito as distrito, reg.region as region, " +
                                             "area.area as area, uni.unidad as unidad, uo.fechacreacion,uo.idcreadopor from unidorganizamec uo " +
                                            "LEFT JOIN distritos dis on dis.iddistrito=uo.iddistrito " +
                                            "LEFT JOIN regiones reg on reg.idregion=uo.idregion " +
                                            "LEFT JOIN areas area on area.idarea=uo.idarea " +
                                            "LEFT JOIN unidades uni on uni.idunidad=uo.idunidad " +
                                            "{0}", CrearSeccionWhereFiltroUnidades(filtroUnidades));

            return DUnidadesOrganizativasAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public ICollection<DUnidadesOrganizativasAmec> ObtenerUnidadesOrgUsu(FiltroUnidadesOrgAMEC filtroUnidades)
        {
            string consulta = string.Format("SELECT 0 as idunidadamec,0 as idamecs, pet.idunidad as idunidad,pet.idarea as idarea,pet.idregion as idregion,pet.iddistrito as iddistrito, " +
                                            "dis.distrito as distrito, reg.region as region, area.area as area, uni.unidad as unidad, CURRENT_TIMESTAMP as fechacreacion, " +
                                            "pet.idpeticionario as idcreadopor from peticionarios pet "+
                                            "LEFT JOIN distritos dis on dis.iddistrito=pet.iddistrito " +
                                            "LEFT JOIN regiones reg on reg.idregion=pet.idregion " +
                                            "LEFT JOIN areas area on area.idarea=pet.idarea " +
                                            "LEFT JOIN unidades uni on uni.idunidad=pet.idunidad " +
                                            "where pet.idPeticionario=" + filtroUnidades.IdUsuario);


            return DUnidadesOrganizativasAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public bool ExisteAprobador(int unidad, int area, int region, int distrito)
        {
            bool result = false;
            if (region == 0 && distrito == 0 && area == 0 && unidad == 0)
            {
                result = false;
            }
            else if (region == 0 && distrito == 0 && area == 0)
            {
                string consulta = string.Format(string.Format("SELECT IDPETICIONARIO FROM DIRECTORES_UNIDADES WHERE IDUNIDAD =  {0}", unidad));
                IList<int> list = Helper.IListIntConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
                result = list != null && list.Count > 0 ? true : false;
            }
            else if (region == 0 && distrito == 0)
            {
                string consulta = string.Format(string.Format("SELECT IDPETICIONARIO FROM GERENTES_AREAS WHERE IDAREA = {0}", area));
                IList<int> list = Helper.IListIntConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
                result = list != null && list.Count > 0 ? true : false;
            }

            else if (distrito == 0)
            {
                string consulta = string.Format(string.Format("SELECT IDPETICIONARIO FROM DIRECTORES_REGIONES WHERE IDREGION = {0}", region));
                IList<int> list = Helper.IListIntConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)); 
                result = list != null && list.Count > 0 ? true : false;
            }
            else
            {
                string consulta = string.Format(string.Format("SELECT IDPETICIONARIO FROM GERENTES_DISTRITOS WHERE IDDISTRITO =   {0}", distrito));
                IList<int> list = Helper.IListIntConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
                result = list != null && list.Count > 0 ? true : false;
            }

            return result;

        }

        public long ObtenerNumeroUnidadesOrgAMEC(FiltroUnidadesOrgAMEC filtroUnidades)
        { 
            
            string consulta = string.Format("select count(*) from unidorganizamec uni {0}", CrearSeccionWhereFiltroUnidades(filtroUnidades));
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }


        public int EliminarUnidadOrganizativaAMEC(long idunidadamec) 
        {            
            string consulta = string.Format("DELETE from unidorganizamec  where idunidadamec=" + idunidadamec);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        public bool ComprobarDuplicidadUnidadOrgAMEC(int? idamec, string idunidad, string idarea, string idregion, string iddistrito)
        {
            if (idunidad == null) idunidad = "null";
            if (idarea == null) idarea = "null";
            if (idregion == null) idregion = "null";
            if (iddistrito == null) iddistrito = "null";
            string consulta = string.Format("select amecControlDuplicidadUnidadesOrg(" + idamec + "," + idunidad + "," + idarea + "," + idregion + "," + iddistrito + ")");
            return Boolean.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }

        public int InsertUnidadOrgAMEC(FiltroUnidadesOrgAMEC filtroUnidades) 
        {
            string idarea = null;
            string idregion = null;
            string iddistrito = null;

            if (filtroUnidades.IdArea != null) { idarea = filtroUnidades.IdArea.ToString(); } else idarea = "null";
            if (filtroUnidades.IdRegion != null) { idregion =  filtroUnidades.IdRegion.ToString(); } else idregion = "null";
            if (filtroUnidades.IdDistrito != null) { iddistrito = filtroUnidades.IdDistrito.ToString(); } else iddistrito = "null";
            if (filtroUnidades.IdArea == null && filtroUnidades.IdRegion == null && filtroUnidades.IdDistrito == null && filtroUnidades.IdUnidad == null) return 1;

            else
            {
                string consulta = "INSERT INTO unidorganizamec (idamecs, idunidad, idarea, idregion, iddistrito, idcreadopor, fechacreacion) VALUES('" + filtroUnidades.IdAmec + "'," + filtroUnidades.IdUnidad + "," + idarea + "," + idregion + "," + iddistrito + "," + filtroUnidades.IdUsuario.Value + ", '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "')";
                return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
            }
        }

        public void ModificarUnidadOrgId(DUnidadesOrganizativasAmec idunidadorgamec)
        {

            string idarea = null;
            string idregion = null;
            string iddistrito = null;

            if (idunidadorgamec.idarea != null) { idarea = " idarea=" + idunidadorgamec.idarea; } else idarea = " idarea=null";
            if (idunidadorgamec.idregion != null) { idregion = " idregion=" + idunidadorgamec.idregion; } else idregion = " idregion=null";
            if (idunidadorgamec.iddistrito != null) { iddistrito = " iddistrito=" + idunidadorgamec.iddistrito; } else iddistrito = " iddistrito=null";
            
         
            string consulta = "UPDATE unidorganizamec SET idunidad =" + idunidadorgamec.idunidad + "," + idarea + "," + idregion + ","  + iddistrito + "," +
                        " idcreadopor=" + idunidadorgamec.idcreadopor + " where idunidadamec=" + idunidadorgamec.idunidadamec ;

            Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }



        public string ObtenerNombreUnidadxId(int idunidad)
        {
            string consulta = "select unidad from unidades where idunidad=" + idunidad;
            return Quodem.Sql.SqlServerClient.GetValue(consulta);
        }


        public string ObtenerNombreAreaxId(int idarea)
        {
            string consulta = "select area from areas where idarea="+ idarea;
            return Quodem.Sql.SqlServerClient.GetValue(consulta);
        }

        public string ObtenerNombreRegionxId(int idregion)
        {
            string consulta = "select region from regiones where idregion=" + idregion;
            return Quodem.Sql.SqlServerClient.GetValue(consulta);
        }

        public string ObtenerNombreDistritoxId(int iddistrito)
        {
            string consulta = "select distrito from distritos where iddistrito=" + iddistrito;
            return Quodem.Sql.SqlServerClient.GetValue(consulta);
        }

        
        private string CrearSeccionWhereFiltroUnidades(FiltroUnidadesOrgAMEC filtroUnidades)
        {
            if (filtroUnidades == null) return null;

            StringBuilder db = new StringBuilder();
            bool primero = true;
            if (filtroUnidades.IdAmec != null && !string.IsNullOrWhiteSpace(filtroUnidades.IdAmec))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" idamecs ='{0}'", filtroUnidades.IdAmec);
            }

            if (filtroUnidades.IdUsuario != null && filtroUnidades.IdUsuario != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" idcreadopor ={0}", filtroUnidades.IdUsuario.Value);
            }

            if (filtroUnidades.IdUnidad != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" idunidad ={0}", filtroUnidades.IdUnidad.Value);
            }

            if (filtroUnidades.IdArea != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" idarea ={0}", filtroUnidades.IdArea.Value);
            }

            if (filtroUnidades.IdRegion != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" idregion ={0}", filtroUnidades.IdRegion.Value);
            }

            if (filtroUnidades.IdDistrito != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" iddistrito ={0}", filtroUnidades.IdDistrito.Value);
            }
            
            if (filtroUnidades.MaximumRows != null && filtroUnidades.MaximumRows > 0)
            {
                db.AppendFormat(" LIMIT {0}", filtroUnidades.MaximumRows);
            }
            if (filtroUnidades.StartRowIndex != null && filtroUnidades.StartRowIndex > 0)
            {
                db.AppendFormat(" OFFSET {0}", filtroUnidades.StartRowIndex);
            }
            return db.ToString();
        }



        private void AgregarAndSiProcede(StringBuilder sb, ref bool primero)
        {
            if (primero == true)
            {
                primero = false;
                sb.Append(" WHERE ");
            }
            else sb.Append(" AND ");
        }


    }
}
