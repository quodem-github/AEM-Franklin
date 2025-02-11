using System;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class DateTimeTypeConverter : ITypeConverter<DateTime, string>
    {
        public string Convert(DateTime source, string destination, ResolutionContext context)
        {
            return source.ToString(Variables.DATE_TIME_FORMAT);
        }

        public string Convert(ResolutionContext context)
        {
            return ((DateTime)context.SourceValue).ToString(Variables.DATE_TIME_FORMAT);
        }
    }

}
