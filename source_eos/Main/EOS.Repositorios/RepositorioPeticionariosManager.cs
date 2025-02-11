using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using EOS.Entidades.Datos;

namespace EOS.Repositorios
{
    public class RepositorioPeticionariosManager
    {
        public ICollection<DPeticionarioManager> ObtenerTodosPeticionariosYManager()
        {
            string consulta = "SELECT * FROM cv_peticionario_manager";
            return DPeticionarioManager.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public ICollection<DPeticionarioManager> ObtenerPeticionariosPorIdManager(int idManager)
        {
            string consulta = string.Format("SELECT * FROM cv_peticionario_manager WHERE IdManager = {0}", idManager);
            return DPeticionarioManager.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public ICollection<DPeticionarioManager> ObtenerPeticionariosPorWeinManager(int weinManager)
        {
            string consulta = string.Format("SELECT * FROM cv_peticionario_manager WHERE WeinManager = {0}", weinManager);
            return DPeticionarioManager.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public DataSet ObtenerUbicacionDepartamental(int idPeticionario)
        {
            string consulta =
                string.Format(
                    "SELECT rtrim(ltrim(rtrim(ltrim(isnull(Nombre, '') + ' ' + isnull(Apellido1, '')))  + ' ' + isnull(Apellido2, ''))),Departamento,Distrito,FuerzaVentas FROM cv_peticionario_manager WHERE idPeticionario = {0}",
                    idPeticionario);
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }
        public DataSet ObtenerUbicacionDepartamentalPorWein(int wein)
        {
            string consulta =
                string.Format(
                    "SELECT rtrim(ltrim(rtrim(ltrim(isnull(Nombre, '') + ' ' + isnull(Apellido1, '')))  + ' ' + isnull(Apellido2, ''))),Departamento,Distrito,FuerzaVentas FROM cv_peticionario_manager WHERE Wein = {0}",
                    wein);
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }

        public DPeticionarioManager ObtenerPeticionarioManager(int idPeticionario)
        {
            string consulta = string.Format("SELECT * FROM cv_peticionario_manager WHERE IdPeticionario = {0}",
                idPeticionario);

            return DPeticionarioManager.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
        }

        public ICollection<DPeticionarioManager> ObtenerManagers(int idPeticionario)
        {
            string consulta =
                string.Format(
                    "SELECT * FROM cv_peticionario_manager WHERE IdPeticionario IN (SELECT IdManager FROM cv_peticionario_manager WHERE IdPeticionario = {0})",
                    idPeticionario);
            return DPeticionarioManager.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public DPeticionarioManager ObtenerManagerDePeticionario(int idPeticionario)
        {
            string consulta =
                string.Format(
                    "SELECT * FROM cv_peticionario_manager WHERE IdPeticionario IN (SELECT IdManager FROM cv_peticionario_manager WHERE IdPeticionario = {0})",
                    idPeticionario);
            return DPeticionarioManager.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
        }

        public DataSet ObtenerManagersDesdeLista(List<int> idsPeticionario)
        {
            string consulta =
                string.Format(
                    "SELECT *,rtrim(ltrim(rtrim(ltrim(isnull(Nombre, '') + ' ' + isnull(Apellido1, '')))  + ' ' + isnull(Apellido2, ''))) as Nombre_completo FROM cv_peticionario_manager WHERE IdPeticionario IN (SELECT IdManager FROM cv_peticionario_manager WHERE IdPeticionario IN {0})",
                    ConvertirEnIn(idsPeticionario));
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }

        public ICollection<DPeticionarioManager> ObtenerPeticionarioDesdeLista(List<int> idsPeticionario)
        {
            if (idsPeticionario.Count == 0) return new List<DPeticionarioManager>();

            string consulta = string.Format("SELECT * FROM cv_peticionario_manager WHERE IdPeticionario IN {0}",
                ConvertirEnIn(idsPeticionario));
            return DPeticionarioManager.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public DataSet ObtenerManagersFullName(int idPeticionario)
        {
            string consulta =
                string.Format(
                    "SELECT DISTINCT idPeticionario,rtrim(ltrim(rtrim(ltrim(isnull(Nombre, '') + ' ' + isnull(Apellido1, '')))  + ' ' + isnull(Apellido2, ''))) as Nombre_completo,Departamento,Distrito,FuerzaVentas FROM cv_peticionario_manager WHERE Wein IN (SELECT WeinManager FROM cv_peticionario_manager WHERE IdPeticionario = {0}) ORDER BY 2",
                    idPeticionario);
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }

        public DataSet ObtenerManagersFullName()
        {
            string consulta = "SELECT -1 AS idPeticionario, '- Seleccione un aprobador -' as Nombre_completo, null as District, null as Departament, null as SaleForce UNION SELECT DISTINCT idPeticionario,rtrim(ltrim(rtrim(ltrim(isnull(Nombre, '') + ' ' + isnull(Apellido1, '')))  + ' ' + isnull(Apellido2, ''))) as Nombre_completo,District,Departament,SaleForce FROM cv_peticionario_manager WHERE Wein IN (SELECT WeinManager FROM cv_peticionario_manager WHERE WeinManager is not null) ORDER BY 2;";
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }

        public DataSet ObtenerManagersFullNameNoSolicitanteNoResponsable(int idpeticionario)
        {
            string consulta = string.Format("SELECT -1 AS idPeticionario, '- Seleccione un aprobador -' as Nombre_completo, null as District, null as Departament, null as SaleForce UNION SELECT DISTINCT idPeticionario,rtrim(ltrim(rtrim(ltrim(isnull(Nombre, '') + ' ' + isnull(Apellido1, '')))  + ' ' + isnull(Apellido2, ''))) as Nombre_completo,District,Departament,SaleForce FROM cv_peticionario_manager WHERE idpeticionario not in (select idpeticionario from agency_user_approval_structure where IdPeticionario = {0} UNION select IdPeticionarioManager from agency_user_approval_structure where IdPeticionario = {0}) and Wein IN (SELECT WeinManager FROM cv_peticionario_manager WHERE WeinManager is not null) ORDER BY 2;", idpeticionario);
            DataSet dt = new DataSet();
            dt.Tables.Add(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            return dt;
        }
        public ICollection<DPeticionarioManager> ObtenerListaManagersFullName()
        {
            string consulta = "SELECT DISTINCT idPeticionario,rtrim(ltrim(rtrim(ltrim(isnull(Nombre, '') + ' ' + isnull(Apellido1, '')))  + ' ' + isnull(Apellido2, ''))) as Nombre_completo,Departamento,Distrito,FuerzaVentas FROM cv_peticionario_manager WHERE Wein IN (SELECT WeinManager FROM cv_peticionario_manager WHERE WeinManager is not null) ORDER BY 2;";
            return DPeticionarioManager.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        

        private string ConvertirEnIn(List<int> lista)
        {
            string filtro = "(";
            foreach (var i in lista)
            {
                if (lista.IndexOf(i) < lista.Count - 1)
                {
                    filtro += i + ",";
                }
                else
                {
                    filtro += i + ")";
                }
            }
            return filtro;
        }

    }
}
