using System;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class StringDateTimeTypeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime? destination, ResolutionContext context)
        {
            return string.IsNullOrEmpty(source) ? new DateTime() : DateTime.ParseExact(source, Variables.DATE_TIME_FORMAT, null);
        }

        public DateTime Convert(ResolutionContext context)
        {
            if (context.SourceValue != null)
            {
                return string.IsNullOrEmpty((string)context.SourceValue) ? new DateTime() : DateTime.ParseExact((string)context.SourceValue, Variables.DATE_TIME_FORMAT, null);
            }

            return new DateTime();
        }
    }
}
