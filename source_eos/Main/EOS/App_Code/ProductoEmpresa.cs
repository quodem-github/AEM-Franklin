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
    /// Descripción breve de Producto
    /// </summary>
    public class ProductoEmpresa
    {
        public ProductoEmpresa()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
            idProductoEmpresa = "";
            fkIdEmpresa = "";
            producto = "";
            inactivo = "";
            locked = "";
        }

        private string idProductoEmpresa;
        public string IdProductoEmpresa
        {
            get { return idProductoEmpresa; }
            set { idProductoEmpresa = value; }
        }

        private string fkIdEmpresa;
        public string FkIdEmpresa
        {
            get { return fkIdEmpresa; }
            set { fkIdEmpresa = value; }
        }

        private string producto;
        public string Producto
        {
            get { return producto; }
            set { producto = value; }
        }

        private string inactivo;
        public string Inactivo
        {
            get { return inactivo; }
            set { inactivo = value; }
        }

        private string locked;
        public string Locked
        {
            get { return locked; }
            set { locked = value; }
        }
    }
}
