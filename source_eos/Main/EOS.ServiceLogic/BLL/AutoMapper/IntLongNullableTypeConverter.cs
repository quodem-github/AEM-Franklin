using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    class IntLongNullableTypeConverter : ITypeConverter<int?, long?>
    {
        public long? Convert(int? source, long? destination, ResolutionContext context)
        {
            
            return source != null ? (long?)(int) source : null;
        }

        public long? Convert(ResolutionContext context)
        {
            return context.SourceValue != null ? (long?)(long) int.Parse(context.SourceValue.ToString()) : null;
            
        }
    }
}
