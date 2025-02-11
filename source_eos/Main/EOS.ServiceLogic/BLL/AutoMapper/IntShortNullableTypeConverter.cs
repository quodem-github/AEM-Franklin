using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    class IntShortNullableTypeConverter : ITypeConverter<int?, short?>
    {
        public short? Convert(int? source, short? destination, ResolutionContext context)
        {
            return source != null ? (short?) (short) (int) source : null;
        }

        public short? Convert(ResolutionContext context)
        {
            return context.SourceValue != null ? (short?) (short) int.Parse(context.SourceValue.ToString()) : null;

        }
    }
}
