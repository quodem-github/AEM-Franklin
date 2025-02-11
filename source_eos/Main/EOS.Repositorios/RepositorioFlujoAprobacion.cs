using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;

namespace EOS.Repositorios
{
    public class RepositorioFlujoAprobacion : IRepositorioFlujoAprobacion
    {
        public ICollection<DVFlujoAprobacion> ObtenerFlujosAprobacion(FiltroFlujoAprobacion filtroFlujoAprob, DVPeticionariosRoles datosRoles)
        {
            string consulta = string.Empty;
            if (filtroFlujoAprob.MaximumRows != null && filtroFlujoAprob.MaximumRows > 0)
            {
                consulta = string.Format("select top {0} * from cv_flujoaprobacion del {1}", filtroFlujoAprob.MaximumRows.ToString(), CrearSeccionWhereFiltroFlujo(filtroFlujoAprob));
            }
            else
            {
                consulta = string.Format("select * from cv_flujoaprobacion del {0}", CrearSeccionWhereFiltroFlujo(filtroFlujoAprob));
            }

            return DVFlujoAprobacion.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public long ObtenerNumeroFlujosAprobacion(FiltroFlujoAprobacion FiltroFlujoAprob)
        {
            string consulta = string.Format("select count(*) from cv_flujoaprobacion del {0}", CrearSeccionWhereFiltroFlujo(FiltroFlujoAprob, true));
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }

        public ICollection<DTipoFlujo> ObtenerTipoFlujo()
        {
            string consulta = string.Format("select * from tipoflujo");
            return DTipoFlujo.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }
        public ICollection<DTipoActividadFlujo> ObtenerTipoActividad(string idtipoactividad, bool veeva = false)
        {
            string consulta = "";
            if (idtipoactividad == "12")
                consulta = string.Format("select * from tipoactividad where veeva = " + (veeva ? "1" : "0"));
            else
                consulta = string.Format("select * from tipoactividad where idtipoactividad != 12");

            return DTipoActividadFlujo.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public ICollection<DTipoActividadFlujo> ObtenerTipoActividadPorTipoEvento(string idtipoevento)
        {
            string consulta = "";
            if (string.IsNullOrWhiteSpace(idtipoevento))
                consulta = string.Format("select * from tipoactividad");
            else
                consulta = string.Format("select * from tipoactividad where idtiporegistroactividad = " + idtipoevento);

            return DTipoActividadFlujo.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public ICollection<DTipoRegistroActividadFlujo> ObtenerTipoRegistroActividadNewCo(string idtipoactividad)
        {
            string consulta = "";
            if (!string.IsNullOrWhiteSpace(idtipoactividad))
                consulta = string.Format("select tipo.* from tiporegistroactividad tipo inner join tipoactividad act on tipo.idtiporegistroactividad = act.idtiporegistroactividad where act.idtipoactividad = " + idtipoactividad + " and tipo.newco = 1");
            else
                consulta = string.Format("select * from tiporegistroactividad where newco = 1");

            return DTipoRegistroActividadFlujo.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public ICollection<DTipoRegistroActividadFlujo> ObtenerTipoRegistroActividad(string idtipoactividad, bool veeva = false)
        {
            string consulta = "";
            if (!string.IsNullOrWhiteSpace(idtipoactividad))
                consulta = string.Format("select tipo.* from tiporegistroactividad tipo inner join tipoactividad act on tipo.idtiporegistroactividad = act.idtiporegistroactividad where act.idtipoactividad = " + idtipoactividad + " and tipo.veeva = " + (veeva ? "1" : "0"));
            else
                consulta = string.Format("select * from tiporegistroactividad");

            return DTipoRegistroActividadFlujo.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public DTipoActividadFlujo ObtenerTipoActividadXid(int idactividad)
        {
            string consulta = string.Format("select * from tipoactividad where idtipoactividad=" + idactividad);
            return DTipoActividadFlujo.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
        }

        public ICollection<DNivelesAprobacion> ObtenerNivelesAprobacion()
        {
            string consulta = string.Format("select * from nivelesaprobacion");
            return DNivelesAprobacion.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }
        
        public int AprobarFlujoAprobacion(FiltroFlujoAprobacion FiltroFlujoAprobacion)
        {
            string consulta = string.Empty;
            consulta = "INSERT INTO flujoaprobacion (idtipoflujo,idtipoactividad,fase,ordenfase, preaprobado, idcreadopor, fechacreacion, idnivelinicial, idnivelsiguiente, importepreaprobacion, condicionada) VALUES(" + FiltroFlujoAprobacion.idTipoFlujo.Value + "," + FiltroFlujoAprobacion.idTipoActividad.Value + ",'" + FiltroFlujoAprobacion.fase + "'," + FiltroFlujoAprobacion.orden.Value + ",'" + FiltroFlujoAprobacion.PreAprobado + "'," + FiltroFlujoAprobacion.idcreadopor.Value + ",CURRENT_TIMESTAMP," + FiltroFlujoAprobacion.idNivelInicial.Value + "," + FiltroFlujoAprobacion.idNivelSiguiente.Value + "," + FiltroFlujoAprobacion.importe + "," + FiltroFlujoAprobacion.condicionado + ")";
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

        }
        public int ActualizarFlujoAprobacion(FiltroFlujoAprobacion FiltroFlujoAprobacion, int idflujoaprobacion)
        {
            string consulta = string.Empty;
            consulta = string.Format("UPDATE flujoaprobacion SET idtipoflujo = '{1}', idtipoactividad = '{2}', fase = '{3}', ordenfase = '{4}', preaprobado= '{5}', idcreadopor = '{6}', fechacreacion = CURRENT_TIMESTAMP, idnivelinicial =  '{7}', idnivelsiguiente =  '{8}', importepreaprobacion =  '{9}',  condicionada =  '{10}'  WHERE idflujoaprobacion = {0}", idflujoaprobacion, FiltroFlujoAprobacion.idTipoFlujo.Value, FiltroFlujoAprobacion.idTipoActividad.Value, FiltroFlujoAprobacion.fase, FiltroFlujoAprobacion.orden.Value, FiltroFlujoAprobacion.PreAprobado, FiltroFlujoAprobacion.idcreadopor.Value, FiltroFlujoAprobacion.idNivelInicial.Value, FiltroFlujoAprobacion.idNivelSiguiente.Value, FiltroFlujoAprobacion.importe.Value, FiltroFlujoAprobacion.condicionado);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }
        public int EliminarFlujoAprobacion(int idflujoaprobacion)
        {
            string consulta = string.Format("DELETE from flujoaprobacion where idflujoaprobacion={0}", idflujoaprobacion);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        public DVFlujoAprobacion ObtenerFlujoAprobacionPorID(int idflujo)
        {
            string consulta = string.Format("select * from cv_flujoaprobacion where idflujoaprobacion={0}", idflujo);
            return DVFlujoAprobacion.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
        }
        private string CrearSeccionWhereFiltroFlujo(FiltroFlujoAprobacion FiltroFlujoAprob, bool count = false)
        {
            if (FiltroFlujoAprob == null) return null;

            StringBuilder db = new StringBuilder();
            bool primero = true;
            if (FiltroFlujoAprob.idTipoActividad != null && FiltroFlujoAprob.idTipoActividad != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" idtipoactividad = {0}", FiltroFlujoAprob.idTipoActividad.Value);
            }

            if (FiltroFlujoAprob.idTipoFlujo != null && FiltroFlujoAprob.idTipoFlujo != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" idtipoflujo = {0}", FiltroFlujoAprob.idTipoFlujo.Value);
            }

            if (!string.IsNullOrEmpty(FiltroFlujoAprob.SortParameter))
            {
                //Solo Ordenar Por Fecha

                db.AppendFormat(" ORDER BY {0}", FiltroFlujoAprob.SortParameter);
            }
            else
            {
                if (!count)
                {
                    db.AppendFormat(" ORDER BY fechacreacion Desc");
                }
            }
            /*
            if (FiltroFlujoAprob.MaximumRows != null && FiltroFlujoAprob.MaximumRows > 0)
            {
                db.AppendFormat(" LIMIT {0}", FiltroFlujoAprob.MaximumRows);
            }
            if (FiltroFlujoAprob.StartRowIndex != null && FiltroFlujoAprob.StartRowIndex > 0)
            {
                db.AppendFormat(" OFFSET {0}", FiltroFlujoAprob.StartRowIndex);
            }*/
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
