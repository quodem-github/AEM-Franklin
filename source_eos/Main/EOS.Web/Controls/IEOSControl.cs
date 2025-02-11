using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace EOS.Web.Controls
{
    /**
     * Clase Abstracta para los controles de EOS propios */
    public interface IEOSControl
    {
        /** Indica que el control tiene Cambios Pendientes. */
        bool hasPendingChanges
        {
            get;
        }
        /**
         * Establece que hay cambios pendientes */
        void setHasPendingChanges();
    }
}
