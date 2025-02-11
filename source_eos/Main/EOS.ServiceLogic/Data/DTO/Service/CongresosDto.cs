using System;
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{

    [DataContract]
    public class CongresosDto
    {
        [DataMember]
        public int Idcongreso { get; set; }
        [DataMember]
        public string Congreso { get; set; }
        [DataMember(Name = "IdTipoCongreso")]
        public int? Idtipocongreso { get; set; }
        [DataMember(Name = "IdPoblacion")]
        public int Idpoblacion { get; set; }
        [DataMember]
        public string Desde { get; set; }
        [DataMember]
        public string Hasta { get; set; }
        [DataMember (Name = "IdEmpresa")]
        public int? Idempresa { get; set; }
        [DataMember(Name = "FCierre")]
        public string Fcierre { get; set; }
        [DataMember(Name = "CodInterno")]
        public string Codinterno { get; set; }
        [DataMember(Name = "IdProveedor")]
        public int? IdProveedor { get; set; }
        [DataMember]
        public int? Idespecialidad { get; set; }
        [DataMember]
        public string Urlcongreso { get; set; }
        [DataMember]
        public string Telemergencias { get; set; }
        [DataMember(Name = "farmaindustria_valoracion")]
        public string  FarmaindustriaValoracion{ get; set; }
        [DataMember]
        public int? Locked { get; set; }
        [DataMember]
        public int? Internacional { get; set; }
        [DataMember]
        public string Emailsecretaria { get; set; }
        [DataMember(Name = "IdPeticionario")]
        public int? Idpeticionario { get; set; }
        [DataMember]
        public string Fechacreacion { get; set; }
        [DataMember]
        public int? Comunicar { get; set; }
        [DataMember]
        public int? Idvaloracionfi { get; set; }
        [DataMember(Name = "publicar")]
        public int? Publicar { get; set; }
        [DataMember]
        public int? AutorizadoMSDI { get; set; }
    }
}