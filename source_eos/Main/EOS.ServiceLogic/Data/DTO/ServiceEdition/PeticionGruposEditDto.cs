using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class PeticionGruposEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idpeticiongrupo no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idpeticiongrupo es obligatorio")]
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
        [Range(0, int.MaxValue, ErrorMessage = "Idvalfi no puede ser inferior a 0")]
        [DataMember]
        public int? Idvalfi { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idunidad no puede ser inferior a 0")]
        [DataMember]
        public int? Idunidad { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "IdtipoasistenteDatosadicionales no puede ser inferior a 0")]
        [DataMember (Name = "idtipoasistente_datosadicionales")]
        public int? IdtipoasistenteDatosadicionales { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "IdtipoactividadDatosadicionales no puede ser inferior a 0")]
        [DataMember (Name = "idtipoactividad_datosadicionales")]
        public int? IdtipoactividadDatosadicionales { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idtipoactividadcongreso no puede ser inferior a 0")]
        [DataMember]
        public int? Idtipoactividadcongreso { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idregion no puede ser inferior a 0")]
        [DataMember]
        public int? Idregion { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "IdnivelriesgoDatosadicionales no puede ser inferior a 0")]
        [DataMember (Name = "idnivelriesgo_datosadicionales")]
        public int? IdnivelriesgoDatosadicionales { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idevento no puede ser inferior a 0")]
        [DataMember]
        public int? Idevento { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Iddistrito no puede ser inferior a 0")]
        [DataMember]
        public int? Iddistrito { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idcargo no puede ser inferior a 0")]
        [DataMember]
        public int? Idcargo { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idarea no puede ser inferior a 0")]
        [DataMember]
        public int? Idarea { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Fkidcongreso no puede ser inferior a 0")]
        [DataMember]
        public int? Fkidcongreso { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idsaleforce no puede ser inferior a 0")]
        [DataMember(Name = "idsaleforce")]
        public int? Idsaleforce { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idposition no puede ser inferior a 0")]
        [DataMember(Name = "idposition")]
        public int? Idposition { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idpeticionario no puede ser inferior a 0")]
        [DataMember]
        public int? Idpeticionario { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idexpediente no puede ser inferior a 0")]
        [DataMember]
        public int? Idexpediente { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Iddistrict no puede ser inferior a 0")]
        [DataMember (Name = "iddistrict")]
        public int? Iddistrict { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Iddepartament no puede ser inferior a 0")]
        [DataMember(Name = "iddepartament")]
        public int? Iddepartament { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idasistente no puede ser inferior a 0")]
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


        public  Expediente Expediente
        {
            get;
            set;
        }

        public  PassengersList PassengersList
        {
            get;
            set;
        }

        public  Peticionarios Peticionarios
        {
            get;
            set;
        }

        public  NivelRiesgoHcpDatosadicionales NivelRiesgoHcpDatosadicionales
        {
            get;
            set;
        }

        public  TipoActividadPaxDatosadicionales TipoActividadPaxDatosadicionales
        {
            get;
            set;
        }
    }
}
