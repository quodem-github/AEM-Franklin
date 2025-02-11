using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class DoubleDecimalTypeConverter : ITypeConverter<double, decimal>
    {
        public decimal Convert(double source, decimal destination, ResolutionContext context)
        {
            return (decimal) source;
        }
        public decimal Convert(ResolutionContext context)
        {
            return (decimal) context.SourceValue;
        }
    }
}
