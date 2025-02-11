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
    /// Descripción breve de Area
    /// </summary>
    public class Area
    {
        protected DataTable dt;
        private BDManage extraccion;

        public Area()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
            idArea = "";
            fkIdEmpresa = "";
            idUnidad = "";
            codArea = "";
            nomArea = "";
            inactivo = "";
            locked = "";
        }

        public Area(string area)
        {
            dt = new BDManage(SQLSentence.GetInstancia().getArea(area), 1).getDataSet().Tables[0];

            if (dt.Rows.Count == 1)
            {
                idArea = area;
                fkIdEmpresa = dt.Rows[0].ItemArray[SQLSentence.ID_EMPRESA_AREA].ToString();
                idUnidad = dt.Rows[0].ItemArray[SQLSentence.ID_UNIDAD_AREA].ToString();
                codArea = dt.Rows[0].ItemArray[SQLSentence.COD_AREA].ToString();
                nomArea = dt.Rows[0].ItemArray[SQLSentence.NOMBRE_AREA].ToString();
                inactivo = dt.Rows[0].ItemArray[SQLSentence.INACTIVO_AREA].ToString();
                locked = dt.Rows[0].ItemArray[SQLSentence.LOCKED_AREA].ToString();
            }
        }

        private string idArea;
        public string IdArea
        {
            get { return idArea; }
            set { idArea = value; }
        }

        private string fkIdEmpresa;
        public string FkIdEmpresa
        {
            get { return fkIdEmpresa; }
            set { fkIdEmpresa = value; }
        }

        private string idUnidad;
        public string IdUnidad
        {
            get { return idUnidad; }
            set { idUnidad = value; }
        }

        private string codArea;
        public string CodArea
        {
            get { return codArea; }
            set { codArea = value; }
        }

        private string nomArea;
        public string NomArea
        {
            get { return nomArea; }
            set { nomArea = value; }
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
