using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class PoblacionEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdPoblacion no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdPoblacion es obligatorio")]
        [DataMember(Name = "IdPoblacion")]
        public int Idpoblacion { get; set; }
        [Required(ErrorMessage = "Poblacion es obligatorio")]
        [DataMember]
        public string Poblacion { get; set; }
        [DataMember(Name = "IdProvincia")]
        public string Idprovincia { get; set; }
        [Required(ErrorMessage = "IdPais es obligatorio")]
        [DataMember(Name = "IdPais")]
        public string Idpais { get; set; }
        [DataMember(Name = "CodPostal")]
        public string Codpostal { get; set; }
        [Range(0, 1, ErrorMessage = "Locked tiene que tener un valor entre 0 y 1")]
        [DataMember]
        public int? Locked { get; set; }
        [DataMember (Name = "IdPaisABC")]
        public int? Idpaisabc { get; set; }
        [Range(0, 1, ErrorMessage = "inactivo tiene que tener un valor entre 0 y 1")]
        [DataMember(Name = "inactivo")]
        public int? Inactivo { get; set; }

        /// </summary>
        public System.Nullable<int> Idconfempresa
        {
            get;
            set;
        }
        public  List<Congresos> Congresos
        {
            get;
            set;
        }



        public List<Peticionarios> Peticionarios
        {
            get;
            set;
        }



        public List<Serviciosreservashotel> Serviciosreservashotels
        {
            get;
            set;
        }



        public List<PeticionesActividad> PeticionesActividads
        {
            get;
            set;
        }


        public List<Proveedores> Proveedores
        {
            get;
            set;
        }
    }
}