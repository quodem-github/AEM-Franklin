using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Repositorios;
using EOS.Entidades.Filtros;
using System.Data.Common;
using EOS.Entidades.Mapeadores;

namespace EOS.Logica
{
    public class LogicaParticipantes : ILogicaParticipantes
    {
        IRepositorioParticipantes MiRepositorioParticipantes { get; set; }

        public LogicaParticipantes()
        {
            MiRepositorioParticipantes = new RepositorioParticipantes();
        }

        public bool ParticipantesPonentes (int idexpediente)
        {
            bool result = false;
            ICollection<DVParticipante> todos = MiRepositorioParticipantes.ObtenerTodosParticipantesExpediente(idexpediente);
            ICollection<DVParticipante> ponentes = MiRepositorioParticipantes.ObtenerPonentesParticipantesExpediente(idexpediente);
            if (todos.Count == ponentes.Count)
            {
                result = true;
            }
            return result;
        }

        public ICollection<DVParticipante> ObtenerParticipantes(FiltroParticipantes filtro)
        {
            return MiRepositorioParticipantes.ObtenerParticipantes(filtro);
        }

        public ICollection<DVParticipanteVeeva> ObtenerParticipantesVeeva(FiltroParticipantesVeeva filtro)
        {
            return MiRepositorioParticipantes.ObtenerParticipantesVeeva(filtro);
        }

        public ICollection<DVParticipante> ObtenerParticipantesExpediente(FiltroParticipantes filtro)
        {
            return MiRepositorioParticipantes.ObtenerParticipantesExpediente(filtro);
        }

        public ICollection<DPassengerList> getInscripcionList(int idexp)
        {
            return MiRepositorioParticipantes.getInscripcionList(idexp);
        }

        public ICollection<DPassengerList> getAlojamientoList(int idexp)
        {
            return MiRepositorioParticipantes.getInscripcionList(idexp);
        }

        public ICollection<DPassengerList> getTransporteList(int idexp)
        {
            return MiRepositorioParticipantes.getInscripcionList(idexp);
        }

        public ICollection<DPassengerList> getActividadesList(int idexp)
        {
            return MiRepositorioParticipantes.getInscripcionList(idexp);
        }


        public long ObtenerNumeroParticipantes(FiltroParticipantes filtro)
        {
            return MiRepositorioParticipantes.ObtenerNumeroParticipantes(filtro);
        }

        public long ObtenerNumeroParticipantesVeeva(FiltroParticipantesVeeva filtro)
        {
            return MiRepositorioParticipantes.ObtenerNumeroParticipantesVeeva(filtro);
        }

        public ICollection<DVParticipanteAmec> ObtenerParticipantesAMEC(FiltroParticipantes filtro)
        {
            return MiRepositorioParticipantes.ObtenerParticipantesAMEC(filtro);
        }
        public long ObtenerNumeroParticipantesAMEC(FiltroParticipantes filtro)
        {
            return MiRepositorioParticipantes.ObtenerNumeroParticipantesAMEC(filtro);
        }

        public int EliminarReservasExpediente(int nIDExpediente)
        {
            return MiRepositorioParticipantes.EliminarReservasExpediente(nIDExpediente);
        }

        public int EliminarReservasExpediente(int participanteEliminado, int nIDExpediente)
        {
            return MiRepositorioParticipantes.EliminarReservasExpediente(participanteEliminado, nIDExpediente);
        }

        public int EliminarReservasServiciosParticipante(int nIDExpediente)
        {
            return MiRepositorioParticipantes.EliminarReservasServiciosParticipante(nIDExpediente);
        }

        public int NuevaReservaPassengerList(DReservasPassengersList reserva)
        {
            if (!MiRepositorioParticipantes.ExisteReserva(reserva.idpassengerlist, reserva.idxpediente))
            {
                reserva.idreservapassengerlist = MiRepositorioParticipantes.ObtenerSiguienteIdReservaPassenger();
                int nRet1 = MiRepositorioParticipantes.InsertaEntidad(reserva);
                return reserva.idreservapassengerlist;

            }
            else return 0;
        }

        //Ismael Ameller 22/02/2011 Control de duplicados a la hora de dar de alta un nuevo participante
        public bool Existe_Duplicado_DNI(string dni)
        {
            long numero = MiRepositorioParticipantes.ExisteDuplicadoDNI(dni);

            if (numero == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool Existe_Duplicado_Msdid(string msdid)
        {

            long numero = MiRepositorioParticipantes.ExisteDuplicadoMSDID(msdid);
            if (numero == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int EliminarReservasAMEC(int nIDAmec)
        {
            return MiRepositorioParticipantes.EliminarReservasAMEC(nIDAmec);
        }

        public ICollection<DVServicioPassengerResumen> ObtenerParticipantesServicio(int nIdExpediente, int? nIdServicio, int nTipoSer)
        {
            return MiRepositorioParticipantes.ObtenerParticipantesServicio(nIdExpediente, nIdServicio, nTipoSer);
        }

        public long ObtenerNumeroServiciosParticipante(int nIdExpediente, int nIdPassengerList)
        {
            return MiRepositorioParticipantes.ObtenerNumeroServiciosParticipante(nIdExpediente, nIdPassengerList);
        }
    }
}
