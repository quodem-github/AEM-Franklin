using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Logica;

namespace EOS.Web
{
    public class AgenteAprobadorAmec
    {
        private readonly LogicaAprobadorAmec _laa;

        public AgenteAprobadorAmec()
        {
            _laa = new LogicaAprobadorAmec();
        }

        public int InsertarAprobadorAmec(FiltroAprovadorAmec filtro)
        {
            return _laa.InsertarAprobador(filtro);
        }
        public int BorrarAprobador(string idamec, string idaprobador)
        {
            return _laa.BorrarAprobador(idamec, idaprobador);
        }
        public List<int> ObernerIdsAprobadoresAmec(string idamec)
        {
            return _laa.ObernerIdsAprobadoresAmec(idamec);
        }
        public bool ObtenerPermisosSupJerAmec(string idamec, int idPeticionario)
        {
            return _laa.ObtenerPermisosSupJerAmec(idamec, idPeticionario);
        }
        public bool ObtenerPermisoSometer(string idamec, int idPeticionario)
        {
            return _laa.ObtenerPermisoSometer(idamec, idPeticionario);
        }
        public bool ObtenerPermisosNegocio(string idamec, int idPeticionario)
        {
            return _laa.ObtenerPermisosNegocio(idamec, idPeticionario);
        }
        public bool ObtenerPermisoSometerCreador(string idamec, int idPeticionario)
        {
            return _laa.ObtenerPermisoSometerCreador(idamec, idPeticionario);
        }
        public bool ObtenerPermisosExecutive(string idamec, int idPeticionario)
        {
            return _laa.ObtenerPermisosExecutive(idamec, idPeticionario);
        }
        public bool HeAprobadoSupJer(string idamec, int idPeticionario)
        {
            return _laa.HeAprobadoSupJer(idamec, idPeticionario);
        }
        public bool TodosAprobaronSupJer(string idamec)
        {
            return _laa.TodosAprobaronSupJer(idamec);
        }
        public bool HeAprobadoNegocio(string idamec, int idPeticionario,List<int> idestados)
        {
            return _laa.HeAprobadoNegocio(idamec, idPeticionario,idestados);
        }

        public bool TodosAprobaronNegocio(string idamec, List<int> idestados)
        {
            return _laa.TodosAprobaronNegocio(idamec,idestados);
        }

        public bool ObtenerPermisosLegal(string idamec, int idPeticionario)
        {
            return _laa.ObtenerPermisosLegal(idamec, idPeticionario);
        }
        public bool HeAprobadoLegal(string idamec)
        {
            return _laa.HeAprobadoLegal(idamec);
        }
        public bool ObtenerPermisosMedico(string idamec, int idPeticionario)
        {
            return _laa.ObtenerPermisosMedico(idamec, idPeticionario);
        }

        public bool HeAprobadoMedico(string idamec)
        {
            return _laa.HeAprobadoMedico(idamec);
        }
        public bool ObtenerPermisosMultiAprobacion(string idamec, int idPeticionario)
        {
            return _laa.ObtenerPermisosMultiAprobacion(idamec, idPeticionario);
        }
        public bool ObtenerPermisosSupJerCreador(string idamec, int idPeticionario)
        {
            return _laa.ObtenerPermisosSupJerCreador(idamec, idPeticionario);
        }
        public bool ObtenerPermisosNegocioCreador(string idamec, int idPeticionario)
        {
            return _laa.ObtenerPermisosNegocioCreador(idamec, idPeticionario);
        }
        public ICollection<Int32> DestinatariosAprobadoresNegocio(string idamec)
        {
            return _laa.DestinatariosAprobadoresNegocio(idamec);
        }
        public ICollection<Int32> DestinatariosAprobadores(string idamec)
        {
            return _laa.DestinatariosAprobadores(idamec);
        }

        public Int32 DestinatariosSupJer(int idPeticionario)
        {
            return _laa.DestinatariosSupJer(idPeticionario);
        }

        public Int32 DestinatariosNegocio(int idPeticionario)
        {
            return _laa.DestinatariosNegocio(idPeticionario);
        }
        public ICollection<Int32> DestinatariosMedicos()
        {
            return _laa.DestinatariosMedicos();
        }
        public ICollection<Int32> DestinatariosLegal()
        {
            return _laa.DestinatariosLegal();
        }
        public bool ObtenerPermisoVerAmec(string idamec, int idPeticionario)
        {
            return _laa.ObtenerPermisoVerAmec(idamec, idPeticionario);
        }
        /*public int ObernerNumeroAprobadoresAmec(string idamec)
        {
            return _laa.ObernerNumeroAprobadoresAmec(idamec);
        }*/

        public List<DAprobadoresAmec> ObernerAprobadoresAmec(string idamec, int? idpeticionario)
        {
            return _laa.ObernerAprobadoresAmec(idamec, idpeticionario);
        }

        public bool SonTodosAprobadoresDirectores(string idamec)
        {
            return _laa.SonTodosAprobadoresDirectores(idamec);
        }
        public ICollection<Int32> DestinatariosAprobadoresNegocioSinExecutive(string idamec)
        {
            return _laa.DestinatariosAprobadoresNegocioSinExecutive(idamec);
        }
        public void ReestablecerAprobadores(string idamec)
        {
            _laa.ReestablecerAprobadores(idamec);
        }
        public int CheckPermisoAprobacionJefe(string idamec, int idPeticionario, int idAprobacionJefe)
        {
            return _laa.CheckPermisoAprobacionJefe(idamec, idPeticionario, idAprobacionJefe);
        }
        public void ActualizarAprobacionJefe(string idamec, int idPeticionario, int idAprobacionJefe)
        {
            _laa.ActualizarAprobacionJefe(idamec, idPeticionario, idAprobacionJefe);
        }
        public int DeCuantosSoyManager(string idamec, int idPeticionario)
        {
            return _laa.DeCuantosSoyManager(idamec, idPeticionario);
        }
        public List<int> ObtenerAmecsDeLosQueSoyAprobador(int idPeticionario)
        {
            return _laa.ObtenerAmecsDeLosQueSoyAprobador(idPeticionario);
        }
        public List<string> ObtenerCorreosTodosParticipantes(string idamecs)
        {
            return _laa.ObtenerCorreosTodosParticipantes(idamecs);
        }
    }
}
