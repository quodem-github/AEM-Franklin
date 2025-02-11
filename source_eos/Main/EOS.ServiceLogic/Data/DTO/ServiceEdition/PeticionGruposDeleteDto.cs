using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class PeticionGruposDeleteDto
    {
        [Required(ErrorMessage = "ListIdpeticiongrupo es obligatorio")]
        [DataMember]
        public List<int> ListIdpeticiongrupo { get; set; }
    }
}
