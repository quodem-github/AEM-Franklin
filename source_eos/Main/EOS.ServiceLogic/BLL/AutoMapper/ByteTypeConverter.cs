using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class ByteTypeConverter : ITypeConverter<byte, int>
    {
        public int Convert(byte source, int destination, ResolutionContext context)
        {
            return source;
        }
        public int Convert(ResolutionContext context)
        {
            return (byte)context.SourceValue;
        }
    }
}
