using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;
using Quodem.ContentManager.TemplateDataType;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class ServiciosReservasActividadesEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idservicioactividad no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idservicioactividad es obligatorio")]
        [DataMember]
        public int Idservicioactividad { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idtarifaactividad no puede ser inferior a 0")]
        [DataMember]
        public int? Idtarifaactividad { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember]
        public double? Pvp { get; set; }
        [Range(0, 1, ErrorMessage = "Locked tiene que tener un valor entre 0 y 1")]
        [DataMember]
        public int? Locked { get; set; }
        [DataMember]
        public string Sede { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public string Tipo { get; set; }
        [DataMember]
        public string Fechainicio { get; set; }
        [DataMember]
        public string Horainicio { get; set; }
        [DataMember]
        public string Minutosinicio { get; set; }
        [DataMember]
        public string Fechafin { get; set; }
        [DataMember]
        public string Horafin { get; set; }
        [DataMember(Name = "minutosfin")]
        public string Minutosfin { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Pax no puede ser inferior a 0")]
        [DataMember(Name = "pax")]
        public int? Pax { get; set; }
        //public int? Idconfempresa { get; set; }

        public  List<ActividadesPassengersList> ActividadesPassengersLists
        {
            get;
            set;
        }


        public  Tarifasactividad Tarifasactividad
        {
            get;
            set;
        }

        public  List<Serviciosreservasviajes> Serviciosreservasviajes
        {
            get;
            set;
        }
    }
}
