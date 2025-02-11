using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class ByteNullableTypeConverter : ITypeConverter<byte?, int?>
    {
        public int? Convert(byte? source, int? destination, ResolutionContext context)
        {
            return source != null ? (int?)(byte)source : null;
        }
        public int? Convert(ResolutionContext context)
        {
            return context.SourceValue != null ? (int?)int.Parse(context.SourceValue.ToString()) : null;
        }
    }
}
