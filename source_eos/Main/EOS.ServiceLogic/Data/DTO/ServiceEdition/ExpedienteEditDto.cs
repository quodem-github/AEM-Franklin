using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class ExpedienteEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idxpediente no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idxpediente es obligatorio")]
        [DataMember]
        public int Idxpediente { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idamec no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idamec es obligatorio")]
        [DataMember]
        public int Idamec { get; set; }
        [DataMember(Name = "Expediente")]
        public string Expediente1 { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "IdPeticionario no puede ser inferior a 0")]
        [Required(ErrorMessage = "IdPeticionario es obligatorio")]
        [DataMember(Name = "IdPeticionario")]
        public int Idpeticionario { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "IdtipoReserva no puede ser inferior a 0")]
        [Required(ErrorMessage = "IdTiporeserva es obligatorio")]
        [DataMember(Name = "IdTiporeserva")]
        public int IdtipoReserva { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idempresa no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idempresa es obligatorio")]
        [DataMember(Name = "Idempresa")]
        public int Idempresa { get; set; }
        [Required(ErrorMessage = "Idestado es obligatorio")]
        [DataMember]
        public string Idestado { get; set; }
        [Required(ErrorMessage = "Fechacreacion es obligatorio")]
        [DataMember]
        public string Fechacreacion { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idempleadogp no puede ser inferior a 0")]
        [DataMember]
        public int? Idempleadogp { get; set; }
        [Range(0, 1, ErrorMessage = "Locked tiene que tener un valor entre 0 y 1")]
        [DataMember]
        public int? Locked { get; set; }
        [DataMember]
        public string Codexpediente { get; set; }
        [Range(0, 1, ErrorMessage = "Urgente tiene que tener un valor entre 0 y 1")]
        [DataMember]
        public int? Urgente { get; set; }
        [DataMember(Name = "importetotal")]
        public double? Importetotal { get; set; }
        [Range(0, 1, ErrorMessage = "visible tiene que tener un valor entre 0 y 1")]
        [DataMember(Name = "visible")]
        public int? Visible { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "iddepartament no puede ser inferior a 0")]
        [DataMember(Name = "iddepartament")]
        public int? Iddepartament { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "idsaleforce no puede ser inferior a 0")]
        [DataMember(Name = "idsaleforce")]
        public int? Idsaleforce { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "iddistrict no puede ser inferior a 0")]
        [DataMember(Name = "iddistrict")]
        public int? Iddistrict { get; set; }


        [Range(0, int.MaxValue, ErrorMessage = "Tipopagofee no puede ser inferior a 0")]
        [DataMember(Name = "Tipopagofee")]
        public int? Tipopagofee { get; set; }

        public  Amec Amec
        {
            get;
            set;
        }

        public  Peticionarios Peticionarios
        {
            get;
            set;
        }

        public  Estadosreservas Estadosreservas
        {
            get;
            set;
        }

        public  TiposreservasWeb TiposreservasWeb
        {
            get;
            set;
        }

        public  Empleadosgp Empleadosgp
        {
            get;
            set;
        }

        public  List<ReservasPassengersList> ReservasPassengersLists
        {
            get;
            set;
        }

        public  List<Reservasviajes> Reservasviajes
        {
            get;
            set;
        }

        public List<PeticionGrupos> PeticionGrupos
        {
            get;
            set;
        }

        public  System.Nullable<int> Idarea
        {
            get;
            set;
        }

        public  System.Nullable<int> Idregion
        {
            get;
            set;
        }

        public  System.Nullable<int> Idunidad
        {
            get;
            set;
        }

        public  System.Nullable<int> Iddistrito
        {
            get;
            set;
        }
        public  Departaments Departaments
        {
            get;
            set;
        }

        public  Salesforce Salesforce
        {
            get;
            set;
        }

        public  Districts Districts
        {
            get;
            set;
        }

        public List<GestordocumentalDocumento> GestordocumentalDocumentos
        {
            get;
            set;
        }
    }
}
