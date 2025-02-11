using System;
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    /// <summary>
    /// Esta clase es un poco especial. Dado que las diferenctes agencias no tienen el mismo formato de tabla se ha decidido podar los 
    /// artibutos para dejarlos identicos. Para ello hemos quitado el [DataMember] de los atributos que no se van a devolver.
    /// </summary>
    [DataContract]
    public class AmecDto
    {
        [DataMember]
        public int idamec { get; set; }
        [DataMember(Name = "amec")]
        public string Amec1 { get; set; }
        [DataMember]
        public int IdEmpresa { get; set; }
        [DataMember]
        public int? idpeticionactividad { get; set; }
        [DataMember]
        public int? IdCongreso { get; set; }
        [DataMember]
        public int inactivo { get; set; }
        
        public string Idestado { get; set; }
         
        public short Aprobado { get; set; }
         
        public int? Idpeticionario { get; set; }
         
        public string Fechaaprobadolegal { get; set; }
         
        public short Aprobadolegal { get; set; }
         
        public int? Idpeticionario2 { get; set; }
         
        public string Fechaaprobadocomplaice { get; set; }
         
        public short Aprobadocomplaice { get; set; }
         
        public int? Idarea { get; set; }
         
        public int? Idarea1 { get; set; }
         
        public int? Idarea2 { get; set; }
         
        public int? Idarea3 { get; set; }
         
        public int? Idunidad { get; set; }
         
        public int? Idtipoactividadcongreso { get; set; }
         
        public int? Iddistrito { get; set; }
         
        public short Paraguas { get; set; }
         
        public string Especificarotras { get; set; }
         
        public string Nombreprograma { get; set; }
         
        public string Codgenesis { get; set; }
         
        public string Objetivosprograma { get; set; }
         
        public string Contenido { get; set; }
         
        public string Lugar { get; set; }
         
        public string Ambitogeografico { get; set; }
         
        public string Duracion { get; set; }
         
        public int? Numparticipantes { get; set; }
         
        public DateTime Fechacomienzop { get; set; }
         
        public DateTime Fechafinp { get; set; }
         
        public string GastosdesAloj { get; set; }
         
        public int? Idcriterio { get; set; }
         
        public int? Idcriterio2 { get; set; }
         
        public int? Idcriterio3 { get; set; }
         
        public string Otros { get; set; }
         
        public string Relponentes { get; set; }
         
        public int? Numponentes { get; set; }
         
        public string Observacioneslegal { get; set; }
         
        public string Honorarios { get; set; }
         
        public string Conceptogastos { get; set; }
         
        public decimal Importetotal { get; set; }
         
        public int? Numpropuestas { get; set; }
         
        public int? Numcartas { get; set; }
         
        public int? Numhojasinscripcion { get; set; }
         
        public int? Idtipopatrocinio { get; set; }
         
        public string Mailautorizadofi { get; set; }
         
        public string Ficheroprograma { get; set; }
         
        public DateTime Fechaaprobadodg { get; set; }
         
        public short Aprobadodg { get; set; }
         
        public string Observacionescomplaice { get; set; }
         
        public string Observacionesdg { get; set; }
         
        public DateTime Fecha { get; set; }
         
        public int? Idpeticionario3 { get; set; }
         
        public short AmecComunicado { get; set; }
         
        public short ComunicarAmec { get; set; }
         
        public string Observaciones { get; set; }
         
        public int? Idregion { get; set; }
         
        public string Area { get; set; }
         
        public int? Numboletines { get; set; }
         
        public decimal Previsioninicial { get; set; }
         
        public int? IdpeticionarioResponsable { get; set; }
        public int? Idconfempresa { get; set; }
        [DataMember]
        public string Company { get { if (newco) return "ORGANON"; else return "MSD"; } set { } }
        public bool newco { get; set; }
    }
}
