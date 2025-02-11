using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;


namespace EOS.Logica
{
    public interface ILogicaParticipantes
    {
        ICollection<DVParticipante> ObtenerParticipantes(FiltroParticipantes filtro);
        ICollection<DVParticipante> ObtenerParticipantesExpediente(FiltroParticipantes filtro);
        long ObtenerNumeroParticipantes(FiltroParticipantes filtro);
        ICollection<DPassengerList> getInscripcionList(int idexp);
        ICollection<DPassengerList> getAlojamientoList(int idexp);
        ICollection<DPassengerList> getTransporteList(int idexp);
        ICollection<DPassengerList> getActividadesList(int idexp);
        bool ParticipantesPonentes(int idexpediente);

        ICollection<DVParticipanteAmec> ObtenerParticipantesAMEC(FiltroParticipantes filtro);
        long ObtenerNumeroParticipantesAMEC(FiltroParticipantes filtro);
        long ObtenerNumeroServiciosParticipante(int nIdExpediente, int nIdPassengerList);

        int EliminarReservasExpediente(int nIDExpediente);
        int EliminarReservasExpediente(int participanteEliminado, int nIDExpediente);
        int EliminarReservasServiciosParticipante(int nIDExpediente);

        int EliminarReservasAMEC(int nIDAmec);

        ICollection<DVServicioPassengerResumen> ObtenerParticipantesServicio(int nIdExpediente, int? nIdServicio, int nTipoSer);
    }
}
