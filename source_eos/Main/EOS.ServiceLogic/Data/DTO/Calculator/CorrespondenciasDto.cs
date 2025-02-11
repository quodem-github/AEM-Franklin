
using System;
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Calculator
{
    [Serializable]
    [DataContract]
    public class CorrespondenciasDto
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long? IdTipoAsistente { get; set; }
        [DataMember]
        public long? IdTipoReunion { get; set; }
        [DataMember]
        public long? IdTipoPsPonentes { get; set; }
        [DataMember]
        public long? IdTipoNacionalLocal { get; set; }
        [DataMember]
        public long? IdDuracionActividadPonentes { get; set; }
        [DataMember]
        public long? IdTiempoPreparacionPonentes { get; set; }
        [DataMember]
        public long? IdTipoPsAbEif { get; set; }
        [DataMember]
        public long? IdDuracionActividadAbEif { get; set; }
        [DataMember]
        public long? IdTiempoPreparacionAbEif { get; set; }
        [DataMember]
        public bool? PonenciaCentroSalud { get; set; }
        [DataMember]
        public bool? TallerOCurso { get; set; }
        [DataMember]
        public bool? VideoconferenciaRepetida { get; set; }
        [DataMember]
        public long IdCalcHonorariosMaximos { get; set; }
        [DataMember]
        public long? IdTipoContratoConsultoria { get; set; }
    }
}
