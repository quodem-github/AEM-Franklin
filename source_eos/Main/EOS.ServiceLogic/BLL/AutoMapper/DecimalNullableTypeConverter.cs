using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class DecimalNullableTypeConverter : ITypeConverter<decimal?, double?>
    {
        public double? Convert(decimal? source, double? destination, ResolutionContext context)
        {
            return source != null ? (double?)(double)(decimal)source : null;
        }
        public double? Convert(ResolutionContext context)
        {
            return context.SourceValue != null ? (double?)double.Parse(context.SourceValue.ToString()) : null;
        }
    }
}
