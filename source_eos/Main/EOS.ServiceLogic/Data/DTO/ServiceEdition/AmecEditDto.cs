using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    /// <summary>
    /// Esta clase es un poco especial. Dado que las diferenctes agencias no tienen el mismo formato de tabla se ha decidido podar los 
    /// artibutos para dejarlos identicos. Para ello hemos quitado el [DataMember] de los atributos que no se van a devolver.
    /// </summary>
    [DataContract]
    public class AmecEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "idamec no puede ser inferior a -1")]
        [Required(ErrorMessage = "idamec es obligatorio")]
        [DataMember]
        public int Idamec { get; set; }

        [Required(ErrorMessage = "amec es obligatorio")]
        [DataMember(Name = "amec")]
        public string Amec1 { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "IdEmpresa no puede ser inferior a 0")]
        [Required(ErrorMessage = "IdEmpresa es obligatorio")]
        [DataMember]
        public int IdEmpresa { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idpeticionactividad no puede ser inferior a 0")]
        [DataMember]
        public int? Idpeticionactividad { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "IdCongreso no puede ser inferior a 0")]
        [DataMember]
        public int? IdCongreso { get; set; }
        [Range(0, 1, ErrorMessage = "Inactivo no puede ser inferior a 0 ni superior a 1")]
        [DataMember]
        public short Inactivo { get; set; }

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

        public string Fechacomienzop { get; set; }

        public string Fechafinp { get; set; }

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

        public string Fechaaprobadodg { get; set; }

        public short Aprobadodg { get; set; }

        public string Observacionescomplaice { get; set; }

        public string Observacionesdg { get; set; }

        public string Fecha { get; set; }

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

        public string Idamecorigin { get; set; }


        public Congresos Congresos
        {
            get;
            set;
        }


        public Confempresa Confempresa
        {
            get;
            set;
        }


        public PeticionesActividad PeticionesActividad
        {
            get;
            set;
        }


        public List<Expediente> Expedientes
        {
            get;
            set;
        }
    }
}
