using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class AmecsDto
    {
        [DataMember]
        public string Fechafinalizacion { get; set; }
        [DataMember]
        public string Fechacomienzo { get; set; }
        [DataMember]
        public int? Preaprobadaamed { get; set; }
        [DataMember(Name = "politicaN20")]
        public int? PoliticaN20 { get; set; }
        [DataMember]
        public int? Medicosfichero { get; set; }
        [DataMember(Name = "profesionalessanitarios")]
        public int? Profesionalessanitarios { get; set; }
        [DataMember]
        public int? Ponentespatrocinados { get; set; }
        [DataMember]
        public int? Participantesmsd { get; set; }
        [DataMember]
        public int? Idnivelaprobacion { get; set; }
        [DataMember]
        public int? Cartascontrato { get; set; }
        [DataMember]
        public int? Idtipoactividad { get; set; }
        [DataMember]
        public int? Idcriterioseleccion { get; set; }
        [DataMember]
        public int? Idconfempresa { get; set; }
        [DataMember]
        public double? Importegastoacumulado { get; set; }
        [DataMember]
        public decimal? Importegasto { get; set; }
        [DataMember]
        public string Fechaamecs { get; set; }
        [DataMember]
        public string Urlprograma { get; set; }
        [DataMember]
        public string Programaamecs { get; set; }
        [DataMember(Name = "nwein")]
        public string Nwein { get; set; }
        [DataMember]
        public string Lugarsede { get; set; }
        [DataMember]
        public string Duracionhoras { get; set; }
        [DataMember]
        public string Detallecriterios { get; set; }
        [DataMember]
        public string Descripcionobjetivo { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public string Criterioespecificado { get; set; }
        [DataMember]
        public string Conceptogastos { get; set; }
        [DataMember]
        public string Cargoadaxas { get; set; }
        [DataMember]
        public int Preaprobadaneg { get; set; }
        [DataMember]
        public int Preaprobadaleg { get; set; }
        [DataMember]
        public int Paraguas { get; set; }
        [DataMember]
        public int Farmaindustria { get; set; }
        [DataMember]
        public int Casosclinicos { get; set; }
        [DataMember]
        public int Idestado { get; set; }
        [DataMember]
        public int Idsolicitante { get; set; }
        [DataMember]
        public int Idcreadopor { get; set; }
        [DataMember]
        public int? Idcargo { get; set; }
        [DataMember]
        public int? Idposition { get; set; }
        [DataMember]
        public string Idamecs { get; set; }
        [DataMember]
        public string Company { get { if (newco) return "ORGANON"; else return "MSD"; } set { } }
        public bool newco { get; set; }
    }
}
