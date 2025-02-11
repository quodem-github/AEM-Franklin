using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class PeticionariosEmpleadosGPEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "id no puede ser inferior a -1")]
        [Required(ErrorMessage = "id es obligatorio")]
        [DataMember(Name = "id")]
        public int id { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "idpeticionario no puede ser inferior a 0")]
        [Required(ErrorMessage = "idpeticionario es obligatorio")]
        [DataMember(Name = "idpeticionario")]
        public int idpeticionario { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "idempleadogp no puede ser inferior a 0")]
        [Required(ErrorMessage = "idempleadogp es obligatorio")]
        [DataMember(Name = "idempleadogp")]
        public int idempleadogp { get; set; }

        [Range(0, 1, ErrorMessage = "locked ha de estar entre 0 y 1.")]
        [DataMember(Name = "locked")]
        public int locked { get; set; }

        /// <summary>
        /// There are no comments for Empleadosgp in the schema.
        /// </summary>
        public Empleadosgp Empleadosgp
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for Peticionarios in the schema.
        /// </summary>
        public Peticionarios Peticionarios
        {
            get;
            set;
        }
    }
}

