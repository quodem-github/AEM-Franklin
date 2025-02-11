using EOS.ServiceModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    class TarifasAlojEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idtarifaaloj no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idtarifaaloj es obligatorio")]
        [DataMember(Name = "Idtarifaaloj")]
        public int IdTarifaAloj { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Fkidcongreso no puede ser inferior a 0")]
        [Required(ErrorMessage = "FKIdCongreso es obligatorio.")]
        [DataMember(Name = "FKIdCongreso")]
        public int Fkidcongreso { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Idtipoaloj no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idtipoaloj es obligatorio.")]
        [DataMember(Name = "IdTipoAloj")]
        public int Idtipoaloj { get; set; }

        [Required(ErrorMessage = "Descripcion es obligatorio.")]
        [DataMember]
        public string Descripcion { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Idproveedor no puede ser inferior a 0")]
        [Required(ErrorMessage = "IdProveedor es obligatorio.")]
        [DataMember(Name = "IdProveedor")]
        public int Idproveedor { get; set; }

        [DataMember]
        public string Idproducto { get; set; }

        [DataMember(Name = "IdTipoHab")]
        public string Idtipohab { get; set; }

        [DataMember(Name = "IdServicio")]
        public string Idservicio { get; set; }

        [DataMember(Name = "PrecioNoche")]
        public double Precionoche { get; set; }

        [Range(0, 1, ErrorMessage = "Prepago ha de estar entre 0 y 1.")]
        [DataMember]
        public int? Prepago { get; set; }

        [Range(0, 1, ErrorMessage = "Socio ha de estar entre 0 y 1.")]
        [DataMember]
        public int? Socio { get; set; }

        [DataMember]
        public string Fechainicio { get; set; }

        [DataMember]
        public string Fechafin { get; set; }

        [DataMember]
        public string Cancelacion { get; set; }

        [Range(0, 1, ErrorMessage = "Locked ha de estar entre 0 y 1.")]
        [DataMember]
        public int? Locked { get; set; }

        [Range(0, 1, ErrorMessage = "Visible ha de estar entre 0 y 1.")]
        [DataMember(Name = "visible")]
        public int? Visible { get; set; }

        public Congresos Congresos
        {
            get { return new Congresos(); }
            set { }
        }

        public ISet<Serviciosreservashotel> Serviciosreservashotels
        {
            get { return new HashSet<Serviciosreservashotel>(); }
            set { }
        }

        public TiposHab TiposHab
        {
            get { return new TiposHab(); }
            set { }
        }

        public Tiposaloj Tiposaloj
        {
            get {
                return new Tiposaloj();
            }

            set { }
        }

        public Productos Productos
        {
            get { return new Productos(); }
            set { }
        }

        public virtual Proveedores Proveedores
        {
            get { return new Proveedores(); }
            set { }
        }

    }
}
