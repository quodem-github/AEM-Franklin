using EOS.ServiceModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    class TiposAlojamientoEditDto 
    {

        [Range(-1, int.MaxValue, ErrorMessage = "IdTipoAloj es campo obligatorio. Si se desea dar de alta el registro poner -1. Si se desea actualizar el registro indicar el id a actualizar.")]
        [Required(ErrorMessage = "IdTipoAloj es obligatorio")]
        [DataMember]
        public int Idtipoaloj { get; set; }

        [Required(ErrorMessage = "TipoAloj es obligatorio")]
        [DataMember]
        public string Tipoaloj { get; set; }

        [Range(0, 1, ErrorMessage = "Locked ha de estar entre 0 y 1.")]
        [DataMember]
        public int? Locked { get; set; }

        public ISet<Serviciosreservashotel> Serviciosreservashotels
        {
            get{ return new HashSet<Serviciosreservashotel>(); }
            set{ }
        }

        public ISet<Tarifasaloj> Tarifasalojs
        {
            get{ return new HashSet<Tarifasaloj>(); }
            set{ }
        }
    }
}