using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;

namespace EOS.Repositorios
{
    public interface IRepositorioParticipantes
    {
        ICollection<DVParticipante> ObtenerParticipantes(FiltroParticipantes filtro);
        ICollection<DVParticipanteVeeva> ObtenerParticipantesVeeva(FiltroParticipantesVeeva filtro);
        ICollection<DVParticipante> ObtenerParticipantesExpediente(FiltroParticipantes filtro);
        long ObtenerNumeroParticipantes(FiltroParticipantes filtro);
        long ObtenerNumeroParticipantesVeeva(FiltroParticipantesVeeva filtro);
        ICollection<DPassengerList> getInscripcionList(int idexp);
        ICollection<DPassengerList> getAlojamientoList(int idexp);
        ICollection<DPassengerList> getTransporteList(int idexp);
        ICollection<DPassengerList> getActividadesList(int idexp);
        ICollection<DVParticipante> ObtenerTodosParticipantesExpediente(int idexpediente);
        ICollection<DVParticipante> ObtenerPonentesParticipantesExpediente(int idexpediente);

        int InsertaEntidad(DReservasPassengersList reservasPassenger);
        
        ICollection<DVParticipanteAmec> ObtenerParticipantesAMEC(FiltroParticipantes filtro);
        long ObtenerNumeroParticipantesAMEC(FiltroParticipantes filtro);
        long ObtenerNumeroServiciosParticipante(int nIdExpediente, int nIdPassengerList);

        int ObtenerSiguienteIdPassenger();
        int ObtenerSiguienteIdReservaPassenger();
        int ObtenerSiguienteIdReservaAMECPassenger();

        ICollection<DReservasPassengersList> ObtenerReservasParticipantes(int nIdPassengerList, int nIdExpediente);
        ICollection<DAmecPassengerList> ObtenerReservasAMECParticipantes(int nIdPassengerList, int nIdAMEC);

        int EliminarReservasExpediente(int nIDExpediente);
        int EliminarReservasExpediente(int participantesEliminado, int nIDExpediente);
        int EliminarReservasServiciosParticipante(int nIDExpediente);
        int EliminarReservasAMEC(int nIDAmec);

        ICollection<DVServicioPassengerResumen> ObtenerParticipantesServicio(int nIdExpediente, int? nIdServicio, int nTipoSer);

        //Ismael Ameller 22/02/2011 Control de duplicados a la hora de dar de alta un nuevo participante
        long ExisteDuplicadoDNI(string dni);
        long ExisteDuplicadoMSDID(string msdid);
        bool ExisteReserva(int idpassengerlist, int idExpediente);
        //FIN Ismael Ameller 22/02/2011 Control de duplicados a la hora de dar de alta un nuevo participante

    }

}
