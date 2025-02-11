using System.Collections.Generic;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class TiposHabitacionResponseDto : ResponseBase
    {
        public List<TiposHabitacionDto> TiposHabitacionList { get; set; }
    }
}
