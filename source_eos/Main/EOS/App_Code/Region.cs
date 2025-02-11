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
    /// Descripción breve de Region
    /// </summary>
    public class Region
    {
        private DataTable dt;
        //private DataSet ds;
        private BDManage extraccion;
       
        public Region()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
            idRegion = "";
            fkIdEmpresa = "";
            nomRegion = "";
            locked = "";
            codRegion = "";
        }

        public Region(string idReg)
        {
            dt = new BDManage(SQLSentence.GetInstancia().getRegion(idReg), 1).getDataSet().Tables[0];

            if (dt.Rows.Count == 1) {
                idRegion = idReg;
                fkIdEmpresa = dt.Rows[0].ItemArray[SQLSentence.ID_EMPRESA_REGION].ToString();
                nomRegion = dt.Rows[0].ItemArray[SQLSentence.NOMBRE_REGION].ToString();
                locked = dt.Rows[0].ItemArray[SQLSentence.LOCKED_REGION].ToString();
                codRegion = dt.Rows[0].ItemArray[SQLSentence.CODIGO_REGION].ToString();
            }
        }

        private string idRegion;
        public string IdRegion
        {
            get { return idRegion; }
            set { idRegion = value; }
        }

        private string fkIdEmpresa;
        public string FkIdEmpresa
        {
            get { return fkIdEmpresa; }
            set { fkIdEmpresa = value; }
        }

        private string nomRegion;
        public string NomRegion
        {
            get { return nomRegion; }
            set { nomRegion = value; }
        }

        private string locked;
        public string Locked
        {
            get { return locked; }
            set { locked = value; }
        }

        private string codRegion;
        public string CodRegion
        {
            get { return codRegion; }
            set { codRegion = value; }
        }
    }
}
