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
    public class RepositorioComentariosAMEC : IRepositorioComentariosAMEC
    {
        public ICollection<DComentariosAmec> ObtenerComentariosAMEC(FiltroComentariosAMEC filtroComentario)
        {
            string consulta = string.Format("SELECT co.idcomentarios,co.idamecs, co.comentariosdoc, co.idcreadopor, co.fechacreacion, "+
                                            "(rtrim(ltrim(isnull(pet.Nombre, '') + ' ' + isnull(pet.Apellido1, '')))) as nombreusuario "+
                                            "from comentasocamecs co " +
                                            "INNER JOIN peticionarios pet on pet.idPeticionario=co.idcreadopor " +
                                            "{0}", CrearSeccionWhereFiltroComentarios(filtroComentario));


            if (filtroComentario.MaximumRows > 0 && filtroComentario.StartRowIndex > 0)
            {
                return DComentariosAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).Skip(filtroComentario.StartRowIndex.Value).Take(filtroComentario.MaximumRows.Value).ToList();
            }
            if (filtroComentario.StartRowIndex > 0)
            {
                return DComentariosAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).Skip(filtroComentario.StartRowIndex.Value).ToList();
            }

            return DComentariosAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public int ComprobarComentarioEsTuyo(int idcomentario, int idpeticionario)
        {
            string consulta = string.Format("select count(*) from comentasocamecs where idcomentarios={0} and idcreadopor={1}",idcomentario, idpeticionario);
            return Convert.ToInt32(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }

        public long ObtenerNumeroComentariosAMEC(FiltroComentariosAMEC filtroComentario)
        {
            string consulta = string.Format("select * from comentasocamecs  {0}", CrearSeccionWhereFiltroComentarios(filtroComentario));
            ICollection<DComentariosAmec> list;

            if (filtroComentario.MaximumRows > 0 && filtroComentario.StartRowIndex > 0)
            {
                list= DComentariosAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).Skip(filtroComentario.StartRowIndex.Value).Take(filtroComentario.MaximumRows.Value).ToList();
                return long.Parse(list.Count.ToString());
            }
            if (filtroComentario.StartRowIndex > 0)
            {
                list = DComentariosAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).Skip(filtroComentario.StartRowIndex.Value).ToList();
                return long.Parse(list.Count.ToString());
            }

            list = DComentariosAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return long.Parse(list.Count.ToString());
        }

        public int EliminarComentarioAMEC(long idcomentarioamec)
        {
            string consulta = string.Format("DELETE from comentasocamecs  where idcomentarios=" + idcomentarioamec);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        public int InsertComentarioAMEC(FiltroComentariosAMEC filtroComentario)
        {
            string consulta = string.Empty;

            if (filtroComentario.Comentario != null) 
                filtroComentario.Comentario = filtroComentario.Comentario.Replace("'", "´");
            
            consulta = "INSERT INTO comentasocamecs (idamecs, comentariosdoc, idcreadopor, fechacreacion) VALUES('" + filtroComentario.IdAmec + "','" + filtroComentario.Comentario + "'," + filtroComentario.IdUsuario.Value + ", CURRENT_TIMESTAMP " + ")";

            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }


        private string CrearSeccionWhereFiltroComentarios(FiltroComentariosAMEC filtroComentarios)
        {
            if (filtroComentarios == null) return null;

            StringBuilder db = new StringBuilder();
            bool primero = true;
            if (!String.IsNullOrWhiteSpace(filtroComentarios.IdAmec))
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" idamecs ='{0}'", filtroComentarios.IdAmec);
            }

            if (filtroComentarios.IdUsuario != null && filtroComentarios.IdUsuario != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" idcreadopor ={0}", filtroComentarios.IdUsuario.Value);
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
