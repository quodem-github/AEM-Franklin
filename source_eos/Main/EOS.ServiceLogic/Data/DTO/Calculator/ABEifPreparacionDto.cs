using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Calculator
{
    [Serializable]
    public class ABEifPreparacionDto
    {
        public int id { get; set; }
        public string Text { get; set; }
        public double Value { get; set; }
    }
}
