using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;

namespace EOS.Entidades
{
    public class DetalleExpediente
    {
        public int nIDExpediente {get; set;}
        public DAmec AMEC { get; set; }
        public DAmecInfo AMECNuevo { get; set; }
        public string idAmecNuevo { get; set; }
        public string idAmecViejo { get; set; }
        public int nIDCogreso {get; set; }
        public int esCongreso { get; set; }
        public string nIDAMEC { get; set; }
        public string sPedido { get; set; }
        public int? tipoPagoFee { get; set; }
        public string sCongreso { get; set; }
        public bool bUrgente { get; set; }
        public int nIDTipo { get; set; }

        public ICollection<DVParticipante> dParticipantes {get;set;}

        public string GetConjuntoIDs()
        {
            StringBuilder db = new StringBuilder();

            foreach (DVParticipante dvPar in dParticipantes)
            {
                db.AppendFormat("{0},", dvPar.IdPassengerlist);
            }

            return (db.Length > 0) ? db.ToString(0, db.Length - 1) : "-1";
        }
    }
}
