using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class DecimalTypeConverter : ITypeConverter<decimal, double>
    {
        public double Convert(decimal source, double destination, ResolutionContext context)
        {
            return (double) source;
        }
        public double Convert(ResolutionContext context)
        {
            return (double) context.SourceValue;
        }
    }
}
