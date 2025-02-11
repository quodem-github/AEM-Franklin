using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class TipoBonosEditDto
    {
        [Required(ErrorMessage = "Idtipobono es obligatorio")]
        [DataMember(Name = "IdTipoBono")]
        public string Idtipobono{get;set;}

        [Required(ErrorMessage = "Tipobono es obligatorio")]
        [DataMember(Name = "TipoBono")]
        public string Tipobono{get;set;}

        [Range(0, 1, ErrorMessage = "Locked ha de estar entre 0 y 1.")]
        [DataMember(Name = "locked")]
        public int? Locked{get;set;}
    }
}
