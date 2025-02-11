using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class DoubleDecimalNullableTypeConverter : ITypeConverter<double?, decimal?>
    {
        public decimal? Convert(double? source, decimal? destination, ResolutionContext context)
        {
            return source != null ? (decimal?)(decimal)(double)source : null;
        }
        public decimal? Convert(ResolutionContext context)
        {
            return context.SourceValue != null ? (decimal?)decimal.Parse(context.SourceValue.ToString()) : null;
        }
    }
}
