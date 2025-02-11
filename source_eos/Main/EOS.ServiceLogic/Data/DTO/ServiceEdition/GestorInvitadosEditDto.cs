using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class GestorInvitadosEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdGestorInvitados no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdGestorInvitados es obligatorio")]
        [DataMember(Name = "IdGestorInvitados")]
        public long IdGestorInvitados { get; set; }
        [Required(ErrorMessage = "IdEventoFormulario es obligatorio")]
        [DataMember(Name = "IdEventoFormulario")]
        public long Ideventoformulario { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idcongreso no puede ser inferior a 0")]
        [DataMember(Name = "IdCongreso")]
        public int? Idcongreso { get; set; }
        [Required(ErrorMessage = "DescripcionGestor es obligatorio")]
        [DataMember(Name = "DescripcionGestor")]
        public string Descripciongestor { get; set; }
        [Required(ErrorMessage = "FechaInicio es obligatorio")]
        [DataMember(Name = "FechaInicio")]
        public string Fechainicio { get; set; }
        [Required(ErrorMessage = "FechaFin es obligatorio")]
        [DataMember(Name = "FechaFin")]
        public string Fechafin { get; set; }
        [Required(ErrorMessage = "Poblacion es obligatorio")]
        [DataMember(Name = "Poblacion")]
        public string Poblacion { get; set; }
        [Required(ErrorMessage = "LinkGestorInvitados es obligatorio")]
        [DataMember(Name = "LinkGestorInvitados")]
        public string Linkgestorinvitados { get; set; }
        [DataMember(Name = "Linkprograma")]
        public string Linkprograma { get; set; }
        [DataMember]
        public string Amec { get; set; }
        [Required(ErrorMessage = "TipoGestorInvitados es obligatorio")]
        [DataMember(Name = "TipoGestorInvitados")]
        public string Tipogestorinvitados { get; set; }
        [Range(0, 1, ErrorMessage = "Inactivo no puede ser inferior a 0 ni superior a 1")]
        [Required(ErrorMessage = "Inactivo es obligatorio")]
        [DataMember(Name = "Inactivo")]
        public int? Inactivo { get; set; }
        [Range(0, 1, ErrorMessage = "locked no puede ser inferior a 0 ni superior a 1")]
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idconfempresa no puede ser inferior a 0")]
        [DataMember(Name = "idconfempresa")]
        public int? Idconfempresa { get; set; }


        public Confempresa Confempresa
        {
            get;
            set;
        }

        public  Congresos Congresos
        {
            get;
            set;
        }
    }
}
