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
    /// Descripción breve de Distrito
    /// </summary>
    public class Distrito
    {
        protected DataTable dt;
        private BDManage extraccion;

        public Distrito()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
            idDistrito = "";
            idEmpresa = "";
            idRegion = "";
            nomDistrito = "";
        }

        public Distrito(string idDistrito)
        {
            dt = new BDManage(SQLSentence.GetInstancia().getDistrito(idDistrito), 1).getDataSet().Tables[0];
            if (dt.Rows.Count == 1) {
                idDistrito = dt.Rows[0].ItemArray[SQLSentence.ID_DISTRITO].ToString();
                idEmpresa = dt.Rows[0].ItemArray[SQLSentence.ID_EMPRESA_DISTRITO].ToString();
                idRegion = dt.Rows[0].ItemArray[SQLSentence.ID_REGION_DISTRITO].ToString();
                nomDistrito = dt.Rows[0].ItemArray[SQLSentence.NOMBRE_DISTRITO].ToString();
            }
        }

        private string idDistrito;
        public string IdDistrito
        {
            get { return idDistrito; }
            set { idDistrito = value; }
        }

        private string idEmpresa;
        public string IdEmpresa
        {
            get { return idEmpresa; }
            set { idEmpresa = value; }
        }

        private string idRegion;
        public string IdRegion
        {
            get { return idRegion; }
            set { idRegion = value; }
        }

        private string nomDistrito;
        public string NomDistrito
        {
            get { return nomDistrito; }
            set { nomDistrito = value; }
        }
    }
}
