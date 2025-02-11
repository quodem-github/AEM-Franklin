using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class IntByteNullableTypeConverter : ITypeConverter<int?, byte?>
    {
        public byte? Convert(int? source, byte? destination, ResolutionContext context)
        {
            return source != null ? (byte?)(byte)(int)source : null;
        }
        public byte? Convert(ResolutionContext context)
        {
            return context.SourceValue != null ? (byte?)byte.Parse(context.SourceValue.ToString()) : null;
        }
    }
}
