using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class TipoAsistenteDatosAdicionalesDto
    {
        [DataMember (Name = "idtipoasistente")]
        public int Idtipoasistente { get; set; }
        [DataMember (Name = "tipoasistente")]
        public string Tipoasistente{get;set;}
        [DataMember (Name = "idtipoABC")]
        public int Idtipoabc{get;set;}
    }
}
