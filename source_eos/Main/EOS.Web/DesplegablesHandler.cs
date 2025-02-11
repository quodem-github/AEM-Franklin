using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace EOS.Web
{
    public class DesplegablesHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();
            StreamWriter escritor = new StreamWriter(context.Response.OutputStream);

            escritor.Write("<div>hola</div><div>adios</div>");
            escritor.Close();
                
        }
    }
}
