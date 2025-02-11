using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    public class NivelesAprobacionEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idnivelaprobacion no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idnivelaprobacion es obligatorio")]
        [DataMember]
        public long Idnivelaprobacion { get; set; }
        [Required(ErrorMessage = "Nivelaprobacion es obligatorio")]
        [DataMember]
        public string Nivelaprobacion { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idcargo no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idcargo es obligatorio")]
        [DataMember]
        public long Idcargo { get; set; }


        public  List<Amecs> Amecs
        {
            get;
            set;
        }


       
        public List<Peticionarios> Peticionarios
        {
            get;
            set;
        }
    }
}