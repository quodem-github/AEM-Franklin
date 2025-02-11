using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    public class ProvinciasEditDto
    {
        [Required(ErrorMessage = "Idprovincia es obligatorio")]
        [DataMember]
        public string Idprovincia { get; set; }
        [Required(ErrorMessage = "Idpais es obligatorio")]
        [DataMember]
        public string Idpais { get; set; }
        [Required(ErrorMessage = "Provincia es obligatorio")]
        [DataMember]
        public string Provincia { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idcomunidad no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idcomunidad es obligatorio")]
        [DataMember]
        public int? Idcomunidad { get; set; }
        [Range(0, 1, ErrorMessage = "Locked tiene que tener un valor entre 0 y 1")]
        [DataMember]
        public int? Locked { get; set; }

        public Comunidad Comunidad
        {
            get;
            set;
        }
    }
}
