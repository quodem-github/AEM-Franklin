
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class ServiciosReservasInscripcionEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idservicioinscripcion no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idservicioinscripcion es obligatorio")]
        [DataMember]
        public int Idservicioinscripcion { get; set; }
        [Required(ErrorMessage = "Inscripcion es obligatorio")]
        [DataMember]
        public string Inscripcion { get; set; }
        [Required(ErrorMessage = "Envioboletin es obligatorio")]
        [DataMember]
        public string Envioboletin { get; set; }
        [DataMember]
        public string Tipoinscripcion { get; set; }
        [DataMember]
        public string Otros { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember(Name = "observ_agencia")]
        public string ObservAgencia { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Iddatosentrega no puede ser inferior a 0")]
        [DataMember]
        public int? Iddatosentrega { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idtarifainscripcion no puede ser inferior a 0")]
        [DataMember(Name = "idtarifainscripcion")]
        public int? Idtarifainscripcion { get; set; }
        [DataMember]
        public double? Pvp { get; set; }
        [Range(0, 1, ErrorMessage = "locked tiene que tener un valor entre 0 y 1")]
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
        public int? Idconfempresa { get; set; }

        public  List<InsPassengersList> InsPassengersLists
        {
            get;
            set;
        }


        public  Tarifasinscripcion Tarifasinscripcion
        {
            get;
            set;
        }

        public  List<Serviciosreservasviajes> Serviciosreservasviajes
        {
            get;
            set;
        }
       

        public  Confempresa Confempresa
        {
            get;
            set;
        }
    }
}
