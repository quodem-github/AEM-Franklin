
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class ProveedorEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdProveedor no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdProveedor es obligatorio")]
        [DataMember (Name = "Idproveedor")]
        public int IdProveedor { get; set; }
        [Required(ErrorMessage = "IdTipoPrv es obligatorio")]
        [DataMember (Name = "Idtipoprv")]
        public string IdTipoPrv { get; set; }
        [DataMember(Name = "Codcia")]
        public string CodCia { get; set; }
        [DataMember(Name = "Codamadeus")]
        public string CodAmadeus { get; set; }
        [DataMember]
        public string Enlace { get; set; }
        [DataMember]
        public string Proveedor { get; set; }
        [Required(ErrorMessage = "Direccion es obligatorio")]
        [DataMember]
        public string Direccion { get; set; }
        [DataMember]
        public string Nro { get; set; }
        [DataMember]
        public string Piso { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idpoblacion no puede ser inferior a 0")]
        [DataMember(Name = "IdPoblacion")]
        public int? Idpoblacion { get; set; }
        [DataMember(Name = "CodPostal")]
        public string Codpostal { get; set; }
        [DataMember]
        public string Telefono { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember(Name = "http")]
        public string Http { get; set; }
        [Range(0,1, ErrorMessage = "Locked tiene que tener un valor entre 0 y 1")]
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }

        public System.Nullable<int> Idconfempresa
        {
            get;
            set;
        }

        public  List<Congresos> Congresos
        {
            get;
            set;
        }


        public  List<Serviciosreservashotel> Serviciosreservashotels
        {
            get;
            set;
        }

        public List<Tarifasaloj> Tarifasalojs
        {
            get;
            set;
        }

        public  List<Tarifasinscripcion> Tarifasinscripcions
        {
            get;
            set;
        }

        public  List<Tramitacionesserviciosreservas> Tramitacionesserviciosreservas_IdProveedor1
        {
            get;
            set;
        }

        public  List<Tramitacionesserviciosreservas> Tramitacionesserviciosreservas_IdProveedor2
        {
            get;
            set;
        }

        public List<Tramitacionesserviciosreservas> Tramitacionesserviciosreservas_IdProveedor3
        {
            get;
            set;
        }

        public List<Tramitacionesserviciosreservas> Tramitacionesserviciosreservas_IdProveedor4
        {
            get;
            set;
        }

        public List<Tramitacionesserviciosreservas> Tramitacionesserviciosreservas_IdProveedor5
        {
            get;
            set;
        }

        public List<Tramitacionesserviciosreservas> Tramitacionesserviciosreservas_IdProveedor6
        {
            get;
            set;
        }

        public  Poblaciones Poblaciones
        {
            get;
            set;
        }

        public  TiposPrv TiposPrv
        {
            get;
            set;
        }

        public  List<ProductosPrv> ProductosPrvs
        {
            get;
            set;
        }
    }
}