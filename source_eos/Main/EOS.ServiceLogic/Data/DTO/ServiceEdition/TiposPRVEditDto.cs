using EOS.ServiceModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    class TiposPRVEditDto
    {

        
        [Required(ErrorMessage = "Idtipoprv es obligatorio")]
        [DataMember]
        public string Idtipoprv { get; set; }

        [Required(ErrorMessage = "TipoProveedor es obligatorio.")]
        [DataMember]
        public string Tipoproveedor { get; set; }

        [Required(ErrorMessage = "IdTipoBono es obligatorio.")]
        [DataMember]
        public string Idtipobono { get; set; }

        [Range(0, 1, ErrorMessage = "Locked ha de estar entre 0 y 1.")]
        [DataMember]
        public int? Locked { get; set; }

        public ISet<Proveedores> Proveedores
        {
            get { return new HashSet<Proveedores>(); }
            set { }
        }

    }
}
