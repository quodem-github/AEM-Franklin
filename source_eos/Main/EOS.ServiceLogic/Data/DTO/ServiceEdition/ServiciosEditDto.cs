using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    public class ServiciosEditDto
    {
        [Required(ErrorMessage = "Idservicio es obligatorio")]
        [DataMember]
        public string Idservicio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Idnumerico no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idnumerico es obligatorio")]
        [DataMember]
        public int? Idnumerico { get; set; }

        [Required(ErrorMessage = "Servicio es obligatorio")]
        [DataMember]
        public string Servicio { get; set; }

        [Range(0, 1, ErrorMessage = "Locked no puede ser inferior a 0 ni superior a 1")]
        [DataMember]
        public int? Locked { get; set; }
        
    }
}
