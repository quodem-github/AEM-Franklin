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
    /// Descripción breve de Unidad
    /// </summary>
    public class Unidad
    {
        private DataTable dt;
        //private DataSet ds;
        private BDManage extraccion;

        public Unidad()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
            idUnidad = "";
            idEmpresa = "";
            codigo = "";
            nomUnidad = "";
            locked = "";
        }

        public Unidad(string unidad)
        {
            dt = new BDManage(SQLSentence.GetInstancia().getUnidad(unidad), 1).getDataSet().Tables[0];

            if (dt.Rows.Count == 1) {
                idUnidad = unidad;
                idEmpresa = dt.Rows[0].ItemArray[SQLSentence.ID_EMPRESA_UNIDAD].ToString();
                codigo = dt.Rows[0].ItemArray[SQLSentence.CODIGO_UNIDAD].ToString();
                nomUnidad = dt.Rows[0].ItemArray[SQLSentence.NOMBRE_UNIDAD].ToString();
                locked = dt.Rows[0].ItemArray[SQLSentence.LOCKED_UNIDAD].ToString();
            }
        }




        private string idUnidad;
        public string IdUnidad
        {
            get { return idUnidad; }
            set { idUnidad = value; }
        }


        private string idEmpresa;
        public string IdEmpresa
        {
            get { return idEmpresa; }
            set { idEmpresa = value; }
        }

        private string codigo;
        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        private string nomUnidad;
        public string NomUnidad
        {
            get { return nomUnidad; }
            set { nomUnidad = value; }
        }

        private string locked;
        public string Locked
        {
            get { return locked; }
            set { locked = value; }
        }
    }
}
