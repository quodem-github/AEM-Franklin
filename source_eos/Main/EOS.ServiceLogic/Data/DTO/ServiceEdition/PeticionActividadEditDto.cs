using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class PeticionActividadEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idpeticionactividad no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idpeticionactividad es obligatorio")]
        [DataMember]
        public int Idpeticionactividad { get; set; }
        [Required(ErrorMessage = "Nombre")]
        [DataMember]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Desde")]
        [DataMember]
        public string Desde { get; set; }
        [Required(ErrorMessage = "Hasta")]
        [DataMember]
        public string Hasta { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "IdPoblacion no puede ser inferior a 0")]
        [Required(ErrorMessage = "IdPoblacion")]
        [DataMember(Name = "IdPoblacion")]
        public int Idpoblacion { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idcongreso no puede ser inferior a 0")]
        [DataMember(Name = "IdCongreso")]
        public int? Idcongreso { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idespecialidad no puede ser inferior a 0")]
        [DataMember]
        public int? Idespecialidad { get; set; }
        [DataMember]
        public string Sede { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idtipocongreso no puede ser inferior a 0")]
        [DataMember(Name = "IdTipoCongreso")]
        public int? Idtipocongreso { get; set; }
        [Range(0, 1, ErrorMessage = "Locked no puede ser inferior a 0 ni superior a 1")]
        [DataMember]
        public int? Locked { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Internacional no puede ser inferior a 0")]
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
        [Range(0, int.MaxValue, ErrorMessage = "Idpeticionario no puede ser inferior a 0")]
        [DataMember(Name = "IdPeticionario")]
        public int? Idpeticionario { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Comunicar no puede ser inferior a 0")]
        [DataMember]
        public int? Comunicar { get; set; }
        [DataMember(Name = "valoracion_farmaindustria")]
        public string ValoracionFarmaindustria { get; set; }


        public  List<Amec> Amecs
        {
            get;
            set;
        }

        public  Congresos Congresos
        {
            get;
            set;
        }

        public  Peticionarios Peticionarios
        {
            get;
            set;
        }

        public  Tiposcongreso Tiposcongreso
        {
            get;
            set;
        }
        
        public  Poblaciones Poblaciones
        {
            get;
            set;
        }
           
        public Especialidades Especialidades
               {
                   get;
                   set;
               }

        public int? Idconfempresa { get; set; }

        public Confempresa Confempresa
        {
            get;
            set;
        }

    }
}
