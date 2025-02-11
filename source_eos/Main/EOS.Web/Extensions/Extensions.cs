using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.Web.Extensions
{

    public static class Extensions
    {
        /// <summary>
        /// Checks if a collection is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pCollection"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptyCollection<T>(this IEnumerable<T> pCollection)
        {
            return (pCollection == null || !pCollection.Any());
        }
    }
}
