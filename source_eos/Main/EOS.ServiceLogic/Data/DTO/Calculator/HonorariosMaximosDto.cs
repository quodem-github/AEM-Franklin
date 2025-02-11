
using System;

namespace EOS.ServiceLogic.Data.DTO.Calculator
{
    [Serializable]
    public class HonorariosMaximosDto
    {
        public int id { get; set; } 
        public DateTime FechaInicioVigencia { get; set; }
        public DateTime FechaFinVigencia { get; set; }
        public string Colspan { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string Value2 { get; set; }
        public double FloatValue { get; set; }
        public bool SpecialCase { get; set; }
        public bool Visible { get; set; }
    }
}
