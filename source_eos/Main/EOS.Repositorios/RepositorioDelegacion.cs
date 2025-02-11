using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using NHibernate;

namespace EOS.Repositorios
{
    public class RepositorioDelegacion : IRepositorioDelegacion
    {

        #region Definitions
        private ISession _sessVariable;
        public ISession _session
        {
            get
            {
                if (!_sessVariable.IsOpen)
                {
                    _sessVariable = Quodem.ORM.NHibernate.Helper.GetCurrentSession(ServiceLogic.Enums.EDbConnection.Default.GetHashCode());
                }
                return _sessVariable;
            }
            set { _sessVariable = value; }
        }
        #endregion

        #region Constructor
        public RepositorioDelegacion()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(EOS.ServiceLogic.Enums.EDbConnection.Default.GetHashCode());
        }
        #endregion

        public long ObtenerNumeroDelegaciones(FiltroDelegacion filtroDelegacion)
        {
            string consulta = string.Format("select count(*) from cv_delegaprobacion del {0}", CrearSeccionWhereFiltroDelegacion(filtroDelegacion, true));
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));

        }
        public ICollection<DVDelegacion> ObtenerDelegaciones(FiltroDelegacion filtroDelegacion)
        {
            string consulta = string.Format("select * from cv_delegaprobacion del {0}", CrearSeccionWhereFiltroDelegacion(filtroDelegacion));

            if (filtroDelegacion.MaximumRows != null && filtroDelegacion.StartRowIndex != null)
            {
                return _session.CreateSQLQuery(consulta).SetFirstResult(filtroDelegacion.StartRowIndex.Value).SetMaxResults(filtroDelegacion.MaximumRows.Value).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DVDelegacion))).List<DVDelegacion>();
            }

            return _session.CreateSQLQuery(consulta).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DVDelegacion))).List<DVDelegacion>();

        }

        public DVDelegacion ObtenerDelegacionPorID(int iddelegacion)
        {
            string consulta = string.Format("select * from cv_delegaprobacion where iddelegaprobacion={0}", iddelegacion);
            return DVDelegacion.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();

        }
        public int AnularDelegacion(int iddelegaprobacion)
        {
            string consulta = string.Empty;
            consulta = string.Format("UPDATE delegaprobacion SET IdEstado = 2 WHERE iddelegaprobacion = {0}", iddelegaprobacion);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

        }
        public int AprobarDelegacion(FiltroDelegacion filtroDelegacion)
        {
            string consulta = "INSERT INTO delegaprobacion (idusuariodel,iddelegado,fechadesde,fechahasta, fechacreacion, idcreadopor,IdEstado) VALUES(" + filtroDelegacion.IdUsuario.Value + "," + filtroDelegacion.IdDelegado.Value + ",'" + filtroDelegacion.FechaDesde.Value.ToString("yyyy/MM/dd HH:mm:ss") + "','" + filtroDelegacion.FechaHasta.Value.ToString("yyyy/MM/dd HH:mm:ss") + "',CURRENT_TIMESTAMP," + filtroDelegacion.idcreadopor.Value + ",1)";
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        public int ActualizarDelegacion(FiltroDelegacion filtroDelegacion, int iddelegacion)
        {
            string consulta = string.Format("UPDATE delegaprobacion SET idusuariodel = '{1}', iddelegado = '{2}', fechadesde = '{3}', fechahasta = '{4}', fechacreacion= CURRENT_TIMESTAMP, IdEstado = 1, idcreadopor =  '{5}'  WHERE iddelegaprobacion = {0}", iddelegacion, filtroDelegacion.IdUsuario.Value, filtroDelegacion.IdDelegado.Value, filtroDelegacion.FechaDesde.Value.ToString("yyyy/MM/dd HH:mm:ss"), filtroDelegacion.FechaHasta.Value.ToString("yyyy/MM/dd HH:mm:ss"), filtroDelegacion.idcreadopor.Value);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

        }
        public int EliminarDelegacion(int iddelegaprobacion)
        {
            string consulta = string.Format("UPDATE delegaprobacion SET IdEstado=3 where iddelegaprobacion={0}", iddelegaprobacion);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        public int ObtenerDelegacion(int idusuarioDel)
        {
            string consultaDelegacion = string.Format("SELECT iddelegado FROM delegaprobacion WHERE idusuariodel = {0} AND idEstado = 1 AND fechadesde <= GETDATE() AND fechahasta > GETDATE()", idusuarioDel);
            try
            {
                return int.Parse(Quodem.Sql.SqlServerClient.GetValue(consultaDelegacion));
            }
            catch (Exception)
            {

                return -1;
            }
        }

        private string CrearSeccionWhereFiltroDelegacion(FiltroDelegacion filtroDelegacion, bool count = false)
        {
            if (filtroDelegacion == null) return null;

            StringBuilder db = new StringBuilder();
            bool primero = true;
            if (filtroDelegacion.IdUsuario != null && filtroDelegacion.IdUsuario != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" idusuariodel = {0}", filtroDelegacion.IdUsuario.Value);
            }

            if (filtroDelegacion.IdDelegado != null && filtroDelegacion.IdDelegado != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" iddelegado = {0}", filtroDelegacion.IdDelegado.Value);
            }

            if (filtroDelegacion.Estado != null && filtroDelegacion.Estado != "-1")
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" IdEstado = '{0}'", filtroDelegacion.Estado);
            }

            if (filtroDelegacion.FechaDesde != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" fechadesde >= '{0}'", filtroDelegacion.FechaDesde.Value.ToString("yyyy/MM/dd"));
            }

            if (filtroDelegacion.FechaHasta != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" fechahasta <= '{0}'", filtroDelegacion.FechaHasta.Value.ToString("yyyy/MM/dd"));
            }
            if (!string.IsNullOrEmpty(filtroDelegacion.SortParameter))
            {
                //Solo Ordenar Por Fecha

                db.AppendFormat(" ORDER BY {0}", filtroDelegacion.SortParameter);
            }
            else
            {
                if (!count)
                {
                    db.AppendFormat(" ORDER BY fechacreacion Desc");
                }
            }
            /*
            if (filtroDelegacion.MaximumRows != null && filtroDelegacion.MaximumRows > 0)
            {
                db.AppendFormat(" LIMIT {0}", filtroDelegacion.MaximumRows);
            }
            if (filtroDelegacion.StartRowIndex != null && filtroDelegacion.StartRowIndex > 0)
            {
                db.AppendFormat(" OFFSET {0}", filtroDelegacion.StartRowIndex);
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
