using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class IntByteTypeConverter : ITypeConverter<int, byte>
    {
        public byte Convert(int source, byte destination, ResolutionContext context)
        {
            return (byte)source;
        }
        public byte Convert(ResolutionContext context)
        {
            return (byte)(int)context.SourceValue;
        }
    }
}
