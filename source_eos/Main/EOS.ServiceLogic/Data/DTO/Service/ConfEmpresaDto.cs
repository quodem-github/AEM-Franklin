using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class ConfEmpresaDto
    {
        [DataMember]
        public int Idempresa { get; set; }
        [DataMember]
        public string Asuntomailcomunicfi { get; set; }
        [DataMember(Name = "codigo_agencia")]
        public string CodigoAgencia { get; set; }
        [DataMember]
        public string Codpostal { get; set; }
        [DataMember]
        public string Cuerpomailcomunicfi { get; set; }
        [DataMember]
        public string Direccion { get; set; }
        [DataMember(Name = "faxagencia")]
        public string Faxagencia { get; set; }
        [DataMember]
        public string Formatoprgmailcomunicfi { get; set; }
        [DataMember]
        public string Gestordearchivo { get; set; }
        [DataMember]
        public string Idagencia { get; set; }
        [DataMember]
        public string Linkhorariosservicios { get; set; }
        [DataMember]
        public string Logoinferiorpantalla { get; set; }
        [DataMember]
        public string Logosuperiorpantalla { get; set; }
        [DataMember]
        public string Mailcccomunicfi { get; set; }
        [DataMember]
        public string Mailcontacto { get; set; }
        [DataMember]
        public string Mailfromcomunicfi { get; set; }
        [DataMember]
        public string Mailtocomunicfi { get; set; }
        [DataMember]
        public string Mailtorespuesta { get; set; }
        [DataMember]
        public string Nombreagencia { get; set; }
        [DataMember]
        public string Nombrecontacto { get; set; }
        [DataMember]
        public string Poblacion { get; set; }
        [DataMember]
        public string Rutaformulariogrupos { get; set; }
        [DataMember]
        public string Telefonoagencia { get; set; }
        [DataMember]
        public int? Locked { get; set; }
        [DataMember]
        public int Idconfempresa { get; set; }


    }
}
