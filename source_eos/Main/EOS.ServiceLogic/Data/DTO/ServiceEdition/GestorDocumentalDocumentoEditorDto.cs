using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceLogic.Data.DTO.Service;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class GestorDocumentalDocumentoEditorDto
    {
        [Range(0, int.MaxValue, ErrorMessage = "Idamec no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idamec es obligatorio")]
        [DataMember]
        public int IdAmec { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "IdExpediente no puede ser inferior a 1")]
        [Required(ErrorMessage = "IdExpediente es obligatorio")]
        [DataMember]
        public int IdExpediente { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "IdTipodoc no puede ser inferior a 1")]
        [Required(ErrorMessage = "IdTipodoc es obligatorio")]
        [DataMember]
        public int IdTipodoc { get; set; }
        [DataMember]
        public int? IdSubTipodoc { get; set; }
        [DataMember]
        public int? IdPassengerList { get; set; }
        [DataMember]
        public string CampoLibre { get; set; }
        [DataMember]
        public string NombreOriginal { get; set; }
        [DataMember]
        public bool NuevaVersion { get; set; }
        [DataMember]
        public int? IdDocumentoVersion { get; set; }

    }
}
