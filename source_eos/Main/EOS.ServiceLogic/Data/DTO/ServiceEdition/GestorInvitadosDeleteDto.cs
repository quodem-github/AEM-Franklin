using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class GestorInvitadosDeleteDto
    {
        [Required(ErrorMessage = "ListIdGestorInvitados es obligatorio")]
        [DataMember(Name = "ListIdGestorInvitados")]
        public List<long> ListIdGestorInvitados { get; set; }
    }
}
