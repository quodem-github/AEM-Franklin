using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class ShortTypeConverter : ITypeConverter<short, int>
    {
        public int Convert(short source, int destination, ResolutionContext context)
        {
            return source;
        }
        public int Convert(ResolutionContext context)
        {
            return (short)context.SourceValue;
        }
    }
}
