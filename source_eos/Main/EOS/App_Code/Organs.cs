using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace EOS
{
    /// <summary>
    /// Descripción breve de Organ
    /// </summary>
    public class Organs
    {
        public Organs()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        private Area[] area;
        public Area[] Area
        {
            get { return area; }
            set { area = value; }
        }
    }
}