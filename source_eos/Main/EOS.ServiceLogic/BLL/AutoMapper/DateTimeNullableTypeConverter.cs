using System;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class DateTimeNullableTypeConverter : ITypeConverter<DateTime?, string>
    {
        public string Convert(DateTime? source, string destination, ResolutionContext context)
        {
            return source != null ? source.Value.ToString(Variables.DATE_TIME_FORMAT) : string.Empty;
        }

        public string Convert(ResolutionContext context)
        {
            if (context.SourceValue != null)
            {
                return ((DateTime)context.SourceValue).ToString(Variables.DATE_TIME_FORMAT);
            }

            return string.Empty;
        }
    }
}
