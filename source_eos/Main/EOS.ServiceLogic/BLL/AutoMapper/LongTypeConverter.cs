using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    class LongTypeConverter : ITypeConverter<long, int>
    {
        public int Convert(long source, int destination, ResolutionContext context)
        {
            return (int) source;
        }
        public int Convert(ResolutionContext context)
        {
            return int.Parse(context.SourceValue.ToString()) ;
        }
    }
}
