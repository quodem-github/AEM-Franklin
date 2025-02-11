using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Repositorios;

namespace EOS.Logica
{
    public class LogicaAprobadorAmec
    {
        private readonly RepositorioAprobadorAMEC _raa;

        public LogicaAprobadorAmec()
        {
            _raa = new RepositorioAprobadorAMEC();
        }

        public int InsertarAprobador(FiltroAprovadorAmec filtro)
        {
            return _raa.InsertAprobadorAmec(filtro);
        }

        public int BorrarAprobador(string idamec,string idaprobador)
        {
            return _raa.BorrarAprbadorAmec(idamec,idaprobador);
        }

        public List<int> ObernerIdsAprobadoresAmec(string idamec)
        {
            return _raa.ObernerIdsAprobadoresAmec(idamec);
        }

        public bool ObtenerPermisosSupJerAmec(string idamec, int idPeticionario)
        {
            return _raa.ObtenerPermisosSupJerAmec(idamec, idPeticionario);
        }

        public bool ObtenerPermisoSometer(string idamec, int idPeticionario)
        {
            return _raa.ObtenerPermisoSometer(idamec, idPeticionario);
        }
        public bool ObtenerPermisoSometerCreador(string idamec, int idPeticionario)
        {
            return _raa.ObtenerPermisoSometerCreador(idamec, idPeticionario);
        }
        public bool ObtenerPermisosNegocio(string idamec, int idPeticionario)
        {
            return _raa.ObtenerPermisosNegocio(idamec, idPeticionario);
        }

        public bool ObtenerPermisosExecutive(string idamec, int idPeticionario)
        {
            return _raa.ObtenerPermisosExecutive(idamec, idPeticionario);
        }

        public bool HeAprobadoSupJer(string idamec, int idPeticionario)
        {
            return _raa.HeAprobadoSupJer(idamec, idPeticionario);
        }

        public bool TodosAprobaronSupJer(string idamec)
        {
            return _raa.TodosAprobaronSupJer(idamec);
        }

        public bool HeAprobadoNegocio(string idamec, int idPeticionario, List<int> idestados)
        {
            return _raa.HeAprobadoNegocio(idamec, idPeticionario,idestados);
        }

        public bool TodosAprobaronNegocio(string idamec,List<int> idestados)
        {
            return _raa.TodosAprobaronNegocio(idamec,idestados);
        }

        public bool ObtenerPermisosLegal(string idamec, int idPeticionario)
        {
            return _raa.ObtenerPermisosLegal(idamec, idPeticionario);
        }

        public bool HeAprobadoLegal(string idamec)
        {
            return _raa.HeAprobadoLegal(idamec);
        }

        public bool ObtenerPermisosMedico(string idamec, int idPeticionario)
        {
            return _raa.ObtenerPermisosMedico(idamec, idPeticionario);
        }

        public bool HeAprobadoMedico(string idamec)
        {
            return _raa.HeAprobadoMedico(idamec);
        }

        public bool ObtenerPermisosMultiAprobacion(string idamec, int idPeticionario)
        {
            return _raa.ObtenerPermisosMultiAprobacion(idamec, idPeticionario);
        }

        public bool ObtenerPermisosSupJerCreador(string idamec, int idPeticionario)
        {
            return _raa.ObtenerPermisosSupJerCreador(idamec, idPeticionario);
        }

        public bool ObtenerPermisosNegocioCreador(string idamec, int idPeticionario)
        {
            return _raa.ObtenerPermisosNegocioCreador(idamec, idPeticionario);
        }

        public ICollection<Int32> DestinatariosAprobadoresNegocio(string idamec)
        {
            return _raa.DestinatariosAprobadoresNegocio(idamec);
        }

        public ICollection<Int32> DestinatariosAprobadores(string idamec)
        {
            return _raa.DestinatariosAprobadores(idamec);
        }

        public Int32 DestinatariosSupJer(int idPeticionario)
        {
            return _raa.DestinatariosSupJer(idPeticionario);
        }

        public Int32 DestinatariosNegocio(int idPeticionario)
        {
            return _raa.DestinatariosNegocio(idPeticionario);
        }

        public ICollection<Int32> DestinatariosMedicos()
        {
            return _raa.DestinatariosMedicos();
        }
        public ICollection<Int32> DestinatariosLegal()
        {
            return _raa.DestinatariosLegal();
        }

        public bool ObtenerPermisoVerAmec(string idamec, int idPeticionario)
        {
            return _raa.ObtenerPermisoVerAmec(idamec, idPeticionario);
        }

        public int ObernerNumeroAprobadoresAmec(string idamec)
        {
            return _raa.ObernerNumeroAprobadoresAmec(idamec);
        }

        public List<DAprobadoresAmec> ObernerAprobadoresAmec(string idamec, int? idpeticionario)
        {
            return _raa.ObernerAprobadoresAmec(idamec, idpeticionario);
        }

        public bool SonTodosAprobadoresDirectores(string idamec)
        {
            return _raa.SonTodosAprobadoresDirectores(idamec);
        }

        public ICollection<Int32> DestinatariosAprobadoresNegocioSinExecutive(string idamec)
        {
            return _raa.DestinatariosAprobadoresNegocioSinExecutive(idamec);
        }

        public void ReestablecerAprobadores(string idamec)
        {
            _raa.ReestablecerAprobadores(idamec);
        }

        public int CheckPermisoAprobacionJefe(string idamec, int idPeticionario, int idAprobacionJefe)
        {
            return _raa.CheckPermisoAprobacionJefe(idamec, idPeticionario, idAprobacionJefe);
        }

        public void ActualizarAprobacionJefe(string idamec, int idPeticionario, int idAprobacionJefe)
        {
            _raa.ActualizarAprobacionJefe(idamec, idPeticionario, idAprobacionJefe);
        }

        public int DeCuantosSoyManager(string idamec, int idPeticionario)
        {
            return _raa.DeCuantosSoyManager(idamec, idPeticionario);
        }
        
        public List<int> ObtenerAmecsDeLosQueSoyAprobador(int idPeticionario)
        {
            return _raa.ObtenerAmecsDeLosQueSoyAprobador(idPeticionario);
        }
        public List<string> ObtenerCorreosTodosParticipantes(string idamecs)
        {
            return _raa.ObtenerCorreosTodosParticipantes(idamecs);
        }
    }

}
