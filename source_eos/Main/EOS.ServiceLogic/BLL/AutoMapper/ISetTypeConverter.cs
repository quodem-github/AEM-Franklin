using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class ISetTypeConverter<T> : ITypeConverter<ISet<T>, List<T>> 
    {
        public List<T> Convert(ISet<T> source, List<T> destination, ResolutionContext context)
        {
            List<T> list = new List<T>();
            foreach (var val in source)
            {
                list.Add(val);
            }
            return list;
        }
        public List<T> Convert(ResolutionContext context)
        {
            return (List<T>) context.SourceValue;
        }

       
    }
}
