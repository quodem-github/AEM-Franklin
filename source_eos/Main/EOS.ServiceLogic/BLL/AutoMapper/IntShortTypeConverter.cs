using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class IntShortTypeConverter : ITypeConverter<int, short>
    {
        public short Convert(int source, short destination, ResolutionContext context)
        {
            return (short) source;
        }
        public short Convert(ResolutionContext context)
        {
            return (short)context.SourceValue;
        }
    }
}
