using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class PeticionGruposDto
    {
        [DataMember]
        public int Idpeticiongrupo { get; set; }
        [DataMember(Name = "ultima_actualizacion")]
        public string UltimaActualizacion { get; set; }
        [DataMember(Name = "fecha_peticion")]
        public string FechaPeticion { get; set; }
        [DataMember(Name = "fecha_inicio_evento")]
        public string FechaInicioEvento { get; set; }
        [DataMember(Name = "fecha_fin_evento")]
        public string FechaFinEvento { get; set; }
        [DataMember(Name = "fecha_expediente")]
        public string FechaExpediente { get; set; }
        [DataMember]
        public int? Idvalfi { get; set; }
        [DataMember]
        public int? Idunidad { get; set; }
        [DataMember (Name = "idtipoasistente_datosadicionales")]
        public int? IdtipoasistenteDatosadicionales { get; set; }
        [DataMember (Name = "idtipoactividad_datosadicionales")]
        public int? IdtipoactividadDatosadicionales { get; set; }
        [DataMember]
        public int? Idtipoactividadcongreso { get; set; }
        [DataMember]
        public int? Idregion { get; set; }
        [DataMember (Name = "idnivelriesgo_datosadicionales")]
        public int? IdnivelriesgoDatosadicionales { get; set; }
        [DataMember]
        public int? Idevento { get; set; }
        [DataMember]
        public int? Iddistrito { get; set; }
        [DataMember]
        public int? Idcargo { get; set; }
        [DataMember]
        public int? Idarea { get; set; }
        [DataMember]
        public int? Fkidcongreso { get; set; }
        [DataMember(Name = "idsaleforce")]
        public int? Idsaleforce { get; set; }
        [DataMember(Name = "idposition")]
        public int? Idposition { get; set; }
        [DataMember]
        public int? Idpeticionario { get; set; }
        [DataMember]
        public int? Idexpediente { get; set; }
        [DataMember (Name = "iddistrict")]
        public int? Iddistrict { get; set; }
        [DataMember(Name = "iddepartament")]
        public int? Iddepartament { get; set; }
        [DataMember]
        public int? Idasistente { get; set; }
        [DataMember(Name = "importe_peticion")]
        public float? ImportePeticion { get; set; }
        [DataMember]
        public string Unidad { get; set; }
        [DataMember(Name = "tipo_gasto")]
        public string TipoGasto { get; set; }
        [DataMember (Name = "tipoasistente_datosadicionales")]
        public string TipoasistenteDatosadicionales { get; set; }
        [DataMember(Name = "tipoactividad_datosadicionales")]
        public string TipoactividadDatosadicionales { get; set; }
        [DataMember]
        public string Region { get; set; }
        [DataMember]
        public string Producto { get; set; }
        [DataMember(Name = "porcentaje_prodcuto")]
        public string PorcentajeProdcuto { get; set; }
        [DataMember]
        public string Numpedido { get; set; }
        [DataMember(Name = "nombre_peticionario")]
        public string NombrePeticionario { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember(Name= "nivelriesgo_datosadicionales")]
        public string NivelriesgoDatosadicionales { get; set; }
        [DataMember]
        public string Msdid { get; set; }
        [DataMember(Name = "justificaciones_datosadicionales")]
        public string JustificacionesDatosadicionales { get; set; }
        [DataMember]
        public string Idestadoreserva { get; set; }
        [DataMember]
        public string Idamec { get; set; }
        [DataMember(Name= "ficherogenesis_datosadicionales")]
        public string FicherogenesisDatosadicionales { get; set; }
        [DataMember]
        public string Evento { get; set; }
        [DataMember(Name = "estado_peticion")]
        public string EstadoPeticion { get; set; }
        [DataMember(Name = "estado_expediente")]
        public string EstadoExpediente { get; set; }
        [DataMember(Name = "distrito")]
        public string Distrito { get; set; }
        [DataMember]
        public string Codespecialidad { get; set; }
        [DataMember(Name = "cargo_peticionario")]
        public string CargoPeticionario { get; set; }
        [DataMember]
        public string Area { get; set; }
        [DataMember(Name = "apellido_peticionario")]
        public string ApellidoPeticionario { get; set; }
        [DataMember(Name= "apellido2")]
        public string Apellido2 { get; set; }
        [DataMember (Name = "apellido1")]
        public string Apellido1 { get; set; }



    }
}
