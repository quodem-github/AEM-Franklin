using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{

    [DataContract]
    public class CongresosEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idcongreso no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idcongreso es obligatorio")]
        [DataMember]
        public int Idcongreso { get; set; }
        [Required(ErrorMessage = "Congreso es obligatorio")]
        [DataMember]
        public string Congreso { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "IdTipoCongreso no puede ser inferior a 0")]
        [Required(ErrorMessage = "IdTipoCongreso es obligatorio")]
        [DataMember(Name = "IdTipoCongreso")]
        public int? Idtipocongreso { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "IdPoblacion no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idpoblacion es obligatorio")]
        [DataMember(Name = "IdPoblacion")]
        public int Idpoblacion { get; set; }
        [Required(ErrorMessage = "Desde es obligatorio")]
        [DataMember]
        public string Desde { get; set; }
        [Required(ErrorMessage = "Hasta es obligatorio")]
        [DataMember]
        public string Hasta { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idempresa no puede ser inferior a 0")]
        [Required(ErrorMessage = "IdEmpresa es obligatorio")]
        [DataMember (Name = "IdEmpresa")]
        public int? Idempresa { get; set; }
        [DataMember(Name = "FCierre")]
        public string Fcierre { get; set; }
        [DataMember(Name = "CodInterno")]
        public string Codinterno { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idproveedor no puede ser inferior a 0")]
        [DataMember(Name = "IdProveedor")]
        public int? Idproveedor { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idespecialidad no puede ser inferior a 0")]
        [DataMember]
        public int? Idespecialidad { get; set; }
        [DataMember]
        public string Urlcongreso { get; set; }
        [DataMember]
        public string Telemergencias { get; set; }
        [DataMember(Name = "farmaindustria_valoracion")]
        public string  FarmaindustriaValoracion{ get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Locked no puede ser inferior a 0")]
        [DataMember]
        public int? Locked { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Internacional no puede ser inferior a 0")]
        [DataMember]
        public int? Internacional { get; set; }
        [DataMember]
        public string Emailsecretaria { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idpeticionario no puede ser inferior a 0")]
        [DataMember(Name = "IdPeticionario")]
        public int? Idpeticionario { get; set; }
        [DataMember]
        public string Fechacreacion { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Comunicar no puede ser inferior a 0")]
        [DataMember]
        public int? Comunicar { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idvaloracionfi no puede ser inferior a 0")]
        [DataMember]
        public int? Idvaloracionfi { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Publicar no puede ser inferior a 0")]
        [DataMember(Name = "publicar")]
        public int? Publicar { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Autorizadomsdi no puede ser inferior a 0")]
        [DataMember(Name = "AutorizadoMSDI")]
        public int? Autorizadomsdi { get; set; }
        public int? Idconfempresa { get; set; }



        public  List<Amec> Amecs
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

        public  ValoracionFi ValoracionFi
        {
            get;
            set;
        }

        public  Poblaciones Poblaciones
        {
            get;
            set;
        }

        public  Proveedores Proveedores
        {
            get;
            set;
        }

        public  Especialidades Especialidades
        {
            get;
            set;
        }

        public List<Gestorinvitados> Gestorinvitados
        {
            get;
            set;
        }

        public List<Tarifasactividad> Tarifasactividads
        {
            get;
            set;
        }

        public List<Tarifasaloj> Tarifasalojs
        {
            get;
            set;
        }

        public List<Tarifasinscripcion> Tarifasinscripcions
        {
            get;
            set;
        }

        public List<PeticionesActividad> PeticionesActividads
        {
            get;
            set;
        }
        public  Confempresa Confempresa
        {
            get;
            set;
        }
    }
}