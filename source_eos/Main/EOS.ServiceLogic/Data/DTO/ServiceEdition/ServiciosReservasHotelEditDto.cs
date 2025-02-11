using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class ServiciosReservasHotelEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idserviciohotel no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idserviciohotel es obligatorio")]
        [DataMember]
        public int? Idserviciohotel { get; set; }
        [Required(ErrorMessage = "Fechahorallegada es obligatorio")]
        [DataMember]
        public string Fechahorallegada { get; set; }
        [Required(ErrorMessage = "Fechahorasalida es obligatorio")]
        [DataMember]
        public string Fechahorasalida { get; set; }
        [DataMember(Name = "IdPais")]
        public string Idpais { get; set; }
        [DataMember]
        public string Pais { get; set; }
        [DataMember(Name = "IdProvincia")]
        public string Idprovincia { get; set; }
        [DataMember]
        public string Provincia { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idpoblacion no puede ser inferior a 0")]
        [DataMember(Name = "IdPoblacion")]
        public int? Idpoblacion { get; set; }
        [DataMember]
        public string Poblacion { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idproveedor no puede ser inferior a 0")]
        [DataMember]
        public int? Idproveedor { get; set; }
        [DataMember]
        public string Hotel { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idtipoaloj no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idtipoaloj es obligatorio")]
        [DataMember(Name = "IdTipoAloj")]
        public int Idtipoaloj { get; set; }
        [DataMember]
        public string Categoria { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember]
        public string Idtipohab { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "NumHabitaciones no puede ser inferior a 0")]
        [DataMember(Name= "num_habitaciones")]
        public int? NumHabitaciones { get; set; }
        [DataMember(Name = "desc_tipoalojamiento")]
        public string DescTipoalojamiento { get; set; }
        [DataMember(Name = "desc_idtipo_habitacion")]
        public string DescIdtipoHabitacion { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idtarifaaloj no puede ser inferior a 0")]
        [DataMember]
        public int? Idtarifaaloj { get; set; }
        [DataMember]
        public double? Pvp { get; set; }
        [Range(0, 1, ErrorMessage = "locked tiene que tener un valor entre 0 y 1")]
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
        public  int? Idconfempresa{ get; set; }

        public List<HotelPassengersList> HotelPassengersLists
        {
            get;
            set;
        }



        public  Tarifasaloj Tarifasaloj
        {
            get;
            set;
        }



        public  Tiposaloj Tiposaloj
        {
            get;
            set;
        }


        public  TiposHab TiposHab
        {
            get;
            set;
        }


        public  Poblaciones Poblaciones
        {
            get;
            set;
        }


        public  Proveedores Proveedores
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
