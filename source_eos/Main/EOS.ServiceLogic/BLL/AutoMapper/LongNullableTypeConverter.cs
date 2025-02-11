using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    class LongNullableTypeConverter : ITypeConverter<long?, int?>
    {
        public int? Convert(long? source, int? destination, ResolutionContext context)
        {
            return source != null ? (int?) (int) (long) source : null;
        }

        public int? Convert(ResolutionContext context)
        {
            return context.SourceValue != null ? (int?)(int) long.Parse(context.SourceValue.ToString()) : null;
            
        }
    }
}
