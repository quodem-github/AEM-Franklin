using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.DLL
{
    public class PeticionariosFilter
    {
        /// <summary>
        /// Current Page
        /// </summary>
        public int? CurrentPageIndex { get; set; }

        /// <summary>
        /// LastUpdateDateFrom
        /// </summary>
        public string LastUpdateDateFrom { get; set; }


        /// <summary>
        /// LastUpdateTo
        /// </summary>
        public string LastUpdateDateTo { get; set; }

    }
}
