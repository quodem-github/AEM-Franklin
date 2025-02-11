using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Logica;

namespace EOS.Web
{
    public class AgentePeticionarioManager
    {
        private readonly LogicaPeticionariosManager _lpm;
        public   ICollection<DPeticionarioManager> _managerList;

        public AgentePeticionarioManager()
        {
            _lpm = new LogicaPeticionariosManager();
            _managerList = new List<DPeticionarioManager>();
        }

        public ICollection<DPeticionarioManager> ObtenerPeticionariosPorIdManager(int idManager)
        {
            return _lpm.ObtenerPeticionariosPorIdManager(idManager);
        }
        public ICollection<DPeticionarioManager> ObtenerPeticionariosPorWeinManager(int weinManager)
        {
            return _lpm.ObtenerPeticionariosPorWeinManager(weinManager);
        }
        public ICollection<DPeticionarioManager> ObtenerTodosPeticionariosYManager()
        {
            return _lpm.ObtenerTodosPeticionariosYManager();
        }

        public DataSet ObtenerUbicacionDepartamental(int idPeticionario)
        {
            return _lpm.ObtenerUbicacionDepartamental(idPeticionario);
        }
        public DataSet ObtenerUbicacionDepartamentalPorWein(int wein)
        {
            return _lpm.ObtenerUbicacionDepartamentalPorWein(wein);
        }
        public DPeticionarioManager ObtenerPeticionarioManager(int idPeticionario)
        {
            return _lpm.ObtenerPeticionarioManager(idPeticionario);
        }
        public ICollection<DPeticionarioManager> ObtenerManagers(int idPeticionario, string sortParameter, int startRowIndex, int maximumRows)
        {
            return _lpm.ObtenerManagers(idPeticionario);
        }
        public ICollection<DPeticionarioManager> ObtenerManagers(int idPeticionario)
        {
            return _lpm.ObtenerManagers(idPeticionario);
        }

        public DPeticionarioManager ObtenerManagerDePeticionario(int idPeticionario)
        {
            return _lpm.ObtenerManagerDePeticionario(idPeticionario);
        }

        public DataSet ObtenerManagersDesdeLista(List<int> idsPeticionario)
        {
            return _lpm.ObtenerManagersDesdeLista(idsPeticionario);
        }

        public ICollection<DPeticionarioManager> ObtenerPeticionarioDesdeLista(List<int> idsPeticionario)
        {
            return _lpm.ObtenerPeticionarioDesdeLista(idsPeticionario);
        }

        public DataSet ObtenerManagersFullName(int idPeticionario)
        {
            return _lpm.ObtenerManagersFullName(idPeticionario);
        }
        public DataSet ObtenerManagersFullName()
        {
            return _lpm.ObtenerManagersFullName();
        }

        public DataSet ObtenerManagersFullNameNoSolicitanteNoResponsable(int idPeticionario)
        {
            return _lpm.ObtenerManagersFullNameNoSolicitanteNoResponsable(idPeticionario);
        }

        public ICollection<DPeticionarioManager> ObtenerListaManagersFullName()
        {
            return _lpm.ObtenerListaManagersFullName();
        }
        public ICollection<DPeticionarioManager> ObtenerManagersList(string sortParameter, int startRowIndex, int maximumRows)
        {
            return _managerList;
        }

        public void InsertarUsuarioEnLista(DPeticionarioManager usuario)
        {
            _managerList.Add(usuario);
        }

        public ICollection<DPeticionarioManager> EliminarUsuarioEnLista(ICollection<DPeticionarioManager> lista,
            DPeticionarioManager usuario)
        {
            lista.Remove(usuario);
            return lista;
        }
    }
}
