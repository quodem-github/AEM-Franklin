using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;

namespace EOS.Entidades
{
    public class DetalleAMEC
    {
        public int nIDAMEC { get; set; }
        public DAmec miAmec = null;
        public ICollection<DVParticipanteAmec> dParticipantesAmec { get; set; }
        public ICollection<DVParticipanteAmec> dParticipantesBack { get; set; }

        public DetalleAMEC(int _nIDAMEC)
        {
            nIDAMEC = _nIDAMEC;
            miAmec = new DAmec();
            miAmec.idamec = nIDAMEC;
           // dParticipantesAmec = new List<DVParticipanteAmec>();
           // dParticipantesBack = new List<DVParticipanteAmec>();
        }

        public string GetConjuntoIDs()
        {
            StringBuilder db = new StringBuilder();

            foreach (DVParticipanteAmec dvPar in dParticipantesAmec)
            {
                db.AppendFormat("{0},", dvPar.IdPassengerList);
            }

            return (db.Length > 0) ? db.ToString(0, db.Length - 1) : "-1";
        }
    }
}
