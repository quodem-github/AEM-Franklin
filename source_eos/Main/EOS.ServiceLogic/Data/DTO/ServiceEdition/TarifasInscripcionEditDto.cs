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
    class TarifasInscripcionEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdTarifaInscripcion no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdTarifaInscripcion es obligatorio")]
        [DataMember(Name = "IdTarifaInscripcion")]
        public int Idtarifainscripcion { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Fkidcongreso no puede ser inferior a 0")]
        [DataMember(Name = "FKIdCongreso")]
        public int Fkidcongreso { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Idtipoinscripcion no puede ser inferior a 0")]
        [DataMember(Name = "IdTipoInscripcion")]
        public int Idtipoinscripcion { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Idproveedor no puede ser inferior a 0")]
        [DataMember(Name = "IdProveedor")]
        public int Idproveedor { get; set; }

        [DataMember(Name = "PVP")]
        public double Pvp { get; set; }

        [DataMember(Name = "NotasCli")]
        public string Notascli { get; set; }

        [DataMember(Name ="idproducto")]
        public string Idproducto { get; set; }

        [Range(0, 1, ErrorMessage = "Prepago ha de estar entre 0 y 1.")]
        [DataMember]
        public int? Prepago { get; set; }

        [Range(0, 1, ErrorMessage = "Socio ha de estar entre 0 y 1.")]
        [DataMember]
        public int? Socio { get; set; }

        [DataMember(Name="cancelacion")]
        public string Cancelacion { get; set; }

        [Range(0, 1, ErrorMessage = "Locked ha de estar entre 0 y 1.")]
        [DataMember(Name ="locked")]
        public int? Locked { get; set; }

        [DataMember(Name = "fecha_validez")]
        public string FechaValidez { get; set; }

        [DataMember(Name ="fechainicio")]
        public string Fechainicio { get; set; }

        [DataMember(Name="fechafin")]
        public string Fechafin { get; set; }

        [DataMember(Name = "descripcion")]
        public string Descripcion { get; set; }

        public Congresos Congresos { get; set; }

        public ISet<Serviciosreservasinscripciones> Serviciosreservasinscripciones
        {
            get { return new HashSet<Serviciosreservasinscripciones>(); }
            set { }
        }

        public Tiposinscripcion Tiposinscripcion{ get; set; }

        public Productos Productos{ get; set; }

        public Proveedores Proveedores { get; set; }

    }
}
