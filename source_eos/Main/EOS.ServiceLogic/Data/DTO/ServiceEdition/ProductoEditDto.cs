
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class ProductoEditDto
    {
        [Required(ErrorMessage = "Idproducto es obligatorio")]
        [DataMember]
        public string Idproducto { get; set; }
        [Required(ErrorMessage = "Producto es obligatorio")]
        [DataMember]
        public string Producto { get; set; }
        [Range(0, 1, ErrorMessage = "Inactivo tiene que tener un valor entre 0 y 1")]
        [DataMember]
        public int? Inactivo { get; set; }
        [Range(0, 1, ErrorMessage = "Locked tiene que tener un valor entre 0 y 1")]
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }

        public  List<Tarifasactividad> Tarifasactividads
        {
            get;
            set;
        }



        public List<Tarifasaloj> Tarifasalojs
        {
            get;
            set;
        }


 
        public List<Tarifasinscripcion> Tarifasinscripcions
        {
            get;
            set;
        }



        public List<ProductosPrv> ProductosPrvs
        {
            get;
            set;
        }
    }
}
