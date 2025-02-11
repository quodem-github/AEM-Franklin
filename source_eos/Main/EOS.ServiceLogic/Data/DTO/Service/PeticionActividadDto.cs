using System;
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class PeticionActividadDto
    {
        [DataMember]
        public int Idpeticionactividad { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Desde { get; set; }
        [DataMember]
        public string Hasta { get; set; }
        [DataMember(Name = "IdPoblacion")]
        public int Idpoblacion { get; set; }
        [DataMember(Name = "IdCongreso")]
        public int? Idcongreso { get; set; }
        [DataMember]
        public int? Idespecialidad { get; set; }
        [DataMember]
        public string Sede { get; set; }
        [DataMember(Name = "IdTipoCongreso")]
        public int? Idtipocongreso { get; set; }
        [DataMember]
        public int? Locked { get; set; }
        [DataMember]
        public int? Internacional { get; set; }
        [DataMember(Name = "url_web")]
        public string UrlWeb { get; set; }
        [DataMember(Name = "email_secretaria")]
        public string EmailSecretaria { get; set; }
        [DataMember]
        public string Especialidad { get; set; }
        [DataMember]
        public string Fechacreacion { get; set; }
        [DataMember(Name = "IdPeticionario")]
        public int? Idpeticionario { get; set; }
        [DataMember]
        public int? Comunicar { get; set; }
        [DataMember(Name = "valoracion_farmaindustria")]
        public string ValoracionFarmaindustria { get; set; }

    }
}
