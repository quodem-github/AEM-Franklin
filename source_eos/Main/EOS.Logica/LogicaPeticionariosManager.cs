using System.Collections.Generic;
using System.Data;
using EOS.Entidades.Datos;
using EOS.Repositorios;

namespace EOS.Logica
{
    public class LogicaPeticionariosManager
    {
        private readonly RepositorioPeticionariosManager _rpm;

        public LogicaPeticionariosManager()
        {
            _rpm = new RepositorioPeticionariosManager();
        }

        public ICollection<DPeticionarioManager> ObtenerPeticionariosPorIdManager(int idManager)
        {
            return _rpm.ObtenerPeticionariosPorIdManager(idManager);
        }
        public ICollection<DPeticionarioManager> ObtenerPeticionariosPorWeinManager(int weinManager)
        {
            return _rpm.ObtenerPeticionariosPorWeinManager(weinManager);
        }
        public ICollection<DPeticionarioManager> ObtenerTodosPeticionariosYManager()
        {
            return _rpm.ObtenerTodosPeticionariosYManager();
        }

        public DataSet ObtenerUbicacionDepartamental(int idPeticionario)
        {
            return _rpm.ObtenerUbicacionDepartamental(idPeticionario);
        }
        public DataSet ObtenerUbicacionDepartamentalPorWein(int wein)
        {
            return _rpm.ObtenerUbicacionDepartamentalPorWein(wein);
        }

        public DPeticionarioManager ObtenerPeticionarioManager(int idPeticionario)
        {
            return _rpm.ObtenerPeticionarioManager(idPeticionario);
        }
        
        public ICollection<DPeticionarioManager> ObtenerManagers(int idPeticionario)
        {
            return _rpm.ObtenerManagers(idPeticionario);
        }

        public DPeticionarioManager ObtenerManagerDePeticionario(int idPeticionario)
        {
            return _rpm.ObtenerManagerDePeticionario(idPeticionario);
        }

        public DataSet ObtenerManagersDesdeLista(List<int> idsPeticionario)
        {
            return _rpm.ObtenerManagersDesdeLista(idsPeticionario);
        }
        public ICollection<DPeticionarioManager> ObtenerPeticionarioDesdeLista(List<int> idsPeticionario)
        {
            return _rpm.ObtenerPeticionarioDesdeLista(idsPeticionario);
        }
        public DataSet ObtenerManagersFullName(int idPeticionario)
        {
            return _rpm.ObtenerManagersFullName(idPeticionario);
        }
        public DataSet ObtenerManagersFullName()
        {
            return _rpm.ObtenerManagersFullName();
        }
        public DataSet ObtenerManagersFullNameNoSolicitanteNoResponsable(int idPeticionario)
        {
            return _rpm.ObtenerManagersFullNameNoSolicitanteNoResponsable(idPeticionario);
        }

        public ICollection<DPeticionarioManager> ObtenerListaManagersFullName()
        {
            return _rpm.ObtenerListaManagersFullName();
        }


    }
}
