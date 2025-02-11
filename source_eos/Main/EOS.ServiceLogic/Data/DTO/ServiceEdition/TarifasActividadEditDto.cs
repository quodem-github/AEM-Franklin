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
    class TarifasActividadEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idtarifaactividad no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idtarifaactividad es obligatorio")]
        [DataMember]
        public int Idtarifaactividad { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Fkidcongreso no puede ser inferior a 0")]
        [Required(ErrorMessage = "FKIdCongreso es obligatorio.")]
        [DataMember]
        public int Fkidcongreso { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Idtipoactividadcongreso no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idtipoactividadcongreso es obligatorio.")]
        [DataMember]
        public int Idtipoactividadcongreso { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "IdProveedor no puede ser inferior a 0")]
        [Required(ErrorMessage = "IdProveedor es obligatorio.")]
        [DataMember]
        public int IdProveedor { get; set; }

        [DataMember]
        public string Idproducto { get; set; }

        [DataMember]
        public string Actividad { get; set; }

        [DataMember]
        public double? Pvp { get; set; }

        [DataMember]
        public string Notascli { get; set; }

        [Range(0, 1, ErrorMessage = "Prepago ha de estar entre 0 y 1.")]
        [DataMember]
        public int? Prepago { get; set; }

        [Range(0, 1, ErrorMessage = "Socio ha de estar entre 0 y 1.")]
        [DataMember]
        public int? Socio { get; set; }

        [DataMember]
        public string Cancelacion { get; set; }

        [Range(0, 1, ErrorMessage = "Locked ha de estar entre 0 y 1.")]
        [DataMember]
        public int? Locked { get; set; }

        [DataMember]
        public string Fechainicio { get; set; }

        [DataMember]
        public string Fechafin { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        //---

        public  Congresos Congresos
        {
            get;
            set;
        }
        public  List<Serviciosreservasactividades> Serviciosreservasactividades
        {
            get;
            set;
        }

        public  TiposActividadCongreso TiposActividadCongreso
        {
            get;
            set;
        }

        public  Productos Productos
        {
            get;
            set;
        }

    }
}
