using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class TipoActividadPaxDatosAdicionalesDto
    {
        [DataMember(Name = "idtipoactividadpax")]
        public int Idtipoactividadpax{get;set;}
        [DataMember(Name = "tipoactividadpax")]
        public string Tipoactividadpax{get;set;}
    }
}
