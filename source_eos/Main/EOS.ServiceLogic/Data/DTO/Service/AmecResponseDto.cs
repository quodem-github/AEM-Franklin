using System.Collections.Generic;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class AmecResponseDto : ResponseBase
    {
        public List<AmecDto> AmecList { get; set; }
        public string Amec { get; set; }
        public int? IdAmec { get; set; }
    }
}
