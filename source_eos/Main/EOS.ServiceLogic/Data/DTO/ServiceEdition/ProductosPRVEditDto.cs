using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    public class ProductosPRVEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idproductoprv no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idproductoprv es obligatorio")]
        [DataMember]
        public int Idproductoprv { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Fkidproveedor no puede ser inferior a 0")]
        [Required(ErrorMessage = "Fkidproveedor es obligatorio")]
        [DataMember]
        public int Fkidproveedor { get; set; }
        [Range(0, 1, ErrorMessage = "Inactivo tiene que tener un valor entre 0 y 1")]
        [Required(ErrorMessage = "Inactivo es obligatorio")]
        [DataMember]
        public short Inactivo { get; set; }
        [Required(ErrorMessage = "Desproducto es obligatorio")]
        [DataMember]
        public string Desproducto { get; set; }
        [Required(ErrorMessage = "Idproducto es obligatorio")]
        [DataMember]
        public string Idproducto { get; set; }
        [Range(0, 1, ErrorMessage = "Locked tiene que tener un valor entre 0 y 1")]
        [DataMember]
        public int? Locked { get; set; }
        
        public  Productos Productos
        {
            get;
            set;
        }

        public  Proveedores Proveedores
        {
            get;
            set;
        }
    }
}
