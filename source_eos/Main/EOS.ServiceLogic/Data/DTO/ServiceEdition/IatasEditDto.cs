using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class IatasEditDto
    {
        public int Id { get; set; }
        public int Idconfempresa { get; set; }
        [DataMember]
        public string Iata { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "IdPoblacion no puede ser inferior a 0")]
        [DataMember(Name = "Idpoblacion")]
        public int? IdPoblacion { get; set; }
        [Range(0, 1, ErrorMessage = "Locked tiene que tener un valor entre 0 y 1")]
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
        [Required(ErrorMessage = "Idiata es obligatorio")]
        [DataMember]
        public string Idiata { get; set; }
    }
}
