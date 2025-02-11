using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    class IntLongTypeConverter : ITypeConverter<int, long>
    {
        public long Convert(int source, long destination, ResolutionContext context)
        {
            return (long) source;
        }
        public long Convert(ResolutionContext context)
        {
            return long.Parse(context.SourceValue.ToString()) ;
        }
    }
}
