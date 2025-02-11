using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class EmpleadosGpEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Id no puede ser inferior a -1")]
        [Required(ErrorMessage = "Id es obligatorio")]
        [DataMember]
        public int? Id { get; set; }
        public int? Idconfempresa { get; set; }
        [Required(ErrorMessage = "Idempleadogp es obligatorio")]
        [DataMember]
        public string Idempleadogp { get; set; }
        [Required(ErrorMessage = "Apellido es obligatorio")]
        [DataMember]
        public string Apellido { get; set; }
        [DataMember]
        public string Email { get; set; }
        [Required(ErrorMessage = "Nombre es obligatorio")]
        [DataMember]
        public string Nombre { get; set; }
        [Range(0, 1, ErrorMessage = "Locked no puede ser inferior a 0 ni superior a 1")]
        [DataMember]
        public int? Locked { get; set; }


        public Confempresa Confempresa
        {
            get;
            set;
        }

        
        public List<Expediente> Expedientes
        {
            get;
            set;
        }

        public List<PeticionariosEmpleadosgp> PeticionariosEmpleadosgps
        {
            get;
            set;
        }

    }
}
