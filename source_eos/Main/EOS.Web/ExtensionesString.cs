using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class ExtensionesString
    {
        public static string CambiarAsterisco(this string original)
        {
            /*
            if (string.IsNullOrEmpty(original))
            {
                return original;
            }
            return original.Replace("*", "%");
             * */
            // 12/1/2010: ya no existe el *. Ahora todas las búsquedas de literales llevan %%
            // De todas formas dejamos la posibilidad de que el usuario ponga un * para indicar que quiere que haya algo escrito
            if (original == "%*%")
            {
                return "%";
            }

            return original;
        }
    }
}
