using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using EOS;

namespace EOS
{
    /// <summary>
    /// Descripción breve de PeticionarioEOS
    /// </summary>
    public class Peticionario
    {
        protected DataTable dt;
        //protected DataTable dt2;
        private DataSet ds;
        protected BDManage extraccion;
        protected SqlDataAdapter da;
        protected DataView dv;

        public Peticionario()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
            idPeticionario = string.Empty;
            fkIdEmpresa = string.Empty;
            login = string.Empty;
            password = string.Empty;
            nombre = string.Empty;
            apellidos = string.Empty;
            wein = string.Empty;
            idCargo = string.Empty;
            rol = string.Empty;
            idTratamiento = string.Empty;
            email = string.Empty;
            movil = string.Empty;
            idEmpresa = string.Empty;
            idEmpleadoGp = string.Empty;
            idUnidad = string.Empty;
            nomUnidad = string.Empty;
            idArea = string.Empty;
            nomArea = string.Empty;
            area = new Area();
            idProducto = string.Empty;
            producto = new ProductoEmpresa();
            idRegion = string.Empty;
            nomRegion = string.Empty;
            region = new Region();
            idDistrito = string.Empty;
            nomDistrito = string.Empty;
            distrito = new Distrito();
            observaciones = string.Empty;
            delegado = string.Empty;
            marketing = string.Empty;
            IdDepartament = string.Empty;
            NomDepartament = string.Empty;
            IdFuerzaVentas = string.Empty;
            NomFuerzaVentas = string.Empty;
            IdDistrict = string.Empty;
            NomDistrict = string.Empty;
        }

        public Peticionario(string idpeticionario)
        {
            Initialize(idpeticionario, false);
        }

        public Peticionario(string idpeticionario, bool checkAdministrador)
        {
            Initialize(idpeticionario, checkAdministrador);
        }

        public void Initialize(string idpeticionario, bool checkAdministrador)
        {
            /*
            dt = new BDManage(MySQLSentence.GetInstancia().getRol(idpeticionario), 1).getDataSet().Tables[0];
            if (dt.Rows.Count == 1)
            {
                if (dt.Rows[0].ItemArray[4].ToString() == "Regional Bussines Director")

                    dt = new BDManage(MySQLSentence.GetInstancia().getPeticionarioGerente(idpeticionario), 1).getDataSet().Tables[0];

                else dt = new BDManage(MySQLSentence.GetInstancia().getPeticionario(idpeticionario), 1).getDataSet().Tables[0];
            }
            */
            dt = new BDManage(SQLSentence.GetInstancia().getPeticionario(idpeticionario), 1).getDataSet().Tables[0];

            if (dt.Rows.Count == 1) {
                idPeticionario = idpeticionario;
                fkIdEmpresa = dt.Rows[0].ItemArray[SQLSentence.FK_ID_EMPRESA].ToString();
                //login = string.Empty;
                //password = string.Empty;
                nombre = dt.Rows[0].ItemArray[SQLSentence.NOMBRE_PETICIONARIO].ToString();
                apellidos = dt.Rows[0].ItemArray[SQLSentence.APELLIDO_PETICIONARIO].ToString();
                idCargo = dt.Rows[0].ItemArray[SQLSentence.ID_CARGO_PETICIONARIO].ToString();
                rol = dt.Rows[0].ItemArray[SQLSentence.NOMBRE_CARGO_PETICIONARIO].ToString();
                //idTratamiento = string.Empty;
                email = dt.Rows[0].ItemArray[SQLSentence.EMAIL_PETICIONARIO].ToString();
                movil = dt.Rows[0].ItemArray[SQLSentence.MOVIL_PETICIONARIO].ToString();

                delegado = dt.Rows[0].ItemArray[SQLSentence.ES_DELEGADO].ToString();
                marketing = dt.Rows[0].ItemArray[SQLSentence.ES_MARKETING].ToString();

                //idEmpresa = dt.Rows[0].ItemArray[MySQLSentence.FK_ID_EMPRESA].ToString();
                //idEmpleadoGp = string.Empty;
                //observaciones = string.Empty;
                bool esAdministador = false;
                if (checkAdministrador)
                    esAdministador = isAdministrador();
                if (!esAdministador) {
                    idUnidad = dt.Rows[0].ItemArray[SQLSentence.ID_UNIDAD_PETICIONARIO].ToString();
                    idArea = dt.Rows[0].ItemArray[SQLSentence.ID_AREA_PETICIONARIO].ToString();
                    idRegion = dt.Rows[0].ItemArray[SQLSentence.ID_REGION_PETICIONARIO].ToString();
                    idDistrito = dt.Rows[0].ItemArray[SQLSentence.ID_DISTRITO_PETICIONARIO].ToString();
                }

                IdDepartament = dt.Rows[0].ItemArray[SQLSentence.ID_DEPARTAMENT].ToString();
                NomDepartament = dt.Rows[0].ItemArray[SQLSentence.DEPARTAMENT].ToString();
                NomFuerzaVentas = dt.Rows[0].ItemArray[SQLSentence.SALESFORCE].ToString();
                IdFuerzaVentas = dt.Rows[0].ItemArray[SQLSentence.ID_SALESFORCE].ToString();
                IdDistrict = dt.Rows[0].ItemArray[SQLSentence.ID_DISTRICT].ToString();
                NomDistrict = dt.Rows[0].ItemArray[SQLSentence.DISTRICT].ToString();


                getVectUnidad();

                getVectArea();
                getVectRegion();
                getVectDistrito();
                getVectPeticionario(string.Empty);

                if (unidad != null && !string.IsNullOrEmpty(unidad.NomUnidad))
                    nomUnidad = unidad.NomUnidad;
                if (area != null && !string.IsNullOrEmpty(area.NomArea))
                    nomArea = area.NomArea;
                if (region != null && !string.IsNullOrEmpty(region.NomRegion))
                    nomRegion = region.NomRegion;
                if (distrito != null && !string.IsNullOrEmpty(distrito.NomDistrito))
                    nomDistrito = distrito.NomDistrito;

                //Segun Rol
                #region oldSwitch
                /*
            switch (idCargo)
            {
                //Delegado
                case "1":
                case "501":

                    getVectUnidad();
                    getVectArea("");
                    getVectRegion("");
                    getVectDistrito("");
                    getVectPeticionario("");

                    break;
                //Gerente de distrito
                case "2":
                case "502":
                    getVectUnidad();
                    getVectArea("");
                    getVectRegion("");
                    getVectDistrito("");
                    getVectPeticionario("");

                    break;

                //Director Regional
                case "3":
                case "503":
                case "57":
                case "557":
                    getVectUnidad();
                    getVectArea("");
                    getVectRegion("");
                    getVectDistrito("");
                    getVectPeticionario("");

                    break;
                //Jefe de producto
                case "4":
                case "504":
                case "17":
                case "517":
                    vectUnidad = new BDManage(MySQLSentence.GetInstancia().getAllUnidades(), 1);
                    vectArea = new BDManage(MySQLSentence.GetInstancia().getAllAreas(), 1);
                    vectRegion = new BDManage(MySQLSentence.GetInstancia().getAllRegiones(), 1);
                    vectDistrito = new BDManage(MySQLSentence.GetInstancia().getAllDistritos(), 1);
                    vectPeticionarios = new BDManage(MySQLSentence.GetInstancia().getAllPeticionarios(), 1);

                    break;

                //Compras - D.General
                case "64":
                case "564":
                case "35":
                case "535":
                    getVectUnidad();
                    getVectArea("");
                    getVectRegion("");
                    getVectDistrito("");
                    getVectPeticionario("");

                    break;
            }
            */
                #endregion

            }
        }

        private bool isAdministrador()
        {
            BDManage adminextract = new BDManage(SQLSentence.GetInstancia().getAdministratorSentencia(idPeticionario), 1);
            if (adminextract.getDataSet() != null && adminextract.getDataSet().Tables[0] != null)
            {
                DataTable dt = adminextract.getDataSet().Tables[0];
                if (dt.Rows.Count == 1)
                {
                    DataRow row = dt.Rows[0];
                    if (row["administrador"].ToString() == "1" || row["administrador"].ToString().ToLower() == "true")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void getVectUnidad()
        {
            if (!string.IsNullOrEmpty(idUnidad))
            {
                unidad = new Unidad(idUnidad);
                vectUnidad = new BDManage(SQLSentence.GetInstancia().getUnidad(idUnidad), 1);
            }
            else
            {
                vectUnidad = new BDManage(SQLSentence.GetInstancia().getAllUnidades(), 1);
            }
        }

        public void getVectArea()
        {
            if (!string.IsNullOrEmpty(idArea))
            {
                area = new Area(idArea);
                //vectArea = new BDManage(MySQLSentence.GetInstancia().getAreaDistrito(idArea), 1);
                vectArea = new BDManage(SQLSentence.GetInstancia().getArea(idArea), 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(idUnidad)) vectArea = new BDManage(SQLSentence.GetInstancia().getAreasUnidad(idUnidad), 1);
                else vectArea = new BDManage(SQLSentence.GetInstancia().getAllAreas(), 1);
            }

        }
        public void getVectAreaUnidad(string unidad)
        {
            vectArea = new BDManage(SQLSentence.GetInstancia().getAreasUnidad(unidad), 1);
        }

        public void getVectRegion()
        {
            if (!string.IsNullOrEmpty(idRegion))
            {
                region = new Region(idRegion);
                /*
                if ("Regional Bussines Director" == rol.ToString()) vectRegion = new BDManage(MySQLSentence.GetInstancia().getRegion(idRegion), 1);
                else vectRegion = new BDManage(MySQLSentence.GetInstancia().getRegionesDistrito(idDistrito), 1);
                */
                //Los distritos por encima de delegado

                //vectDistrito = new BDManage(MySQLSentence.GetInstancia().getDistritoRegion(idRegion, idArea), 1);
                vectRegion = new BDManage(SQLSentence.GetInstancia().getRegion(idRegion), 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(idUnidad)) vectRegion = new BDManage(SQLSentence.GetInstancia().getRegionesUnidades(idUnidad), 1);
                else vectRegion = new BDManage(SQLSentence.GetInstancia().getAllRegiones(), 1);
            }
        }

        public void getVectRegionUnidad(string unidad)
        {
            vectRegion = new BDManage(SQLSentence.GetInstancia().getRegionesUnidades(unidad), 1);
        }

        public void getVectDistrito()
        {
            if (!string.IsNullOrEmpty(idDistrito))
            {
                distrito = new Distrito(idDistrito);
                vectDistrito = new BDManage(SQLSentence.GetInstancia().getDistrito(idDistrito), 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(idUnidad)) vectDistrito = new BDManage(SQLSentence.GetInstancia().getDistritoUnidad(idUnidad), 1);
                else vectDistrito = new BDManage(SQLSentence.GetInstancia().getAllDistritos(), 1);

                if (!string.IsNullOrEmpty(idRegion) && !string.IsNullOrEmpty(idArea))
                {
                    vectDistrito = new BDManage(SQLSentence.GetInstancia().getDistritoRegionArea(idRegion, idArea), 1);
                }
                else
                {
                    if (!string.IsNullOrEmpty(idRegion) && string.IsNullOrEmpty(idArea))
                        vectDistrito = new BDManage(SQLSentence.GetInstancia().getDistritoRegion(idRegion), 1);
                    if (string.IsNullOrEmpty(idRegion) && !string.IsNullOrEmpty(idArea))
                        vectDistrito = new BDManage(SQLSentence.GetInstancia().getDistritoArea(idArea), 1);

                }
            }
            //Si cambia ddlregion afecta al ddl de distritos
            //if (idreg != "") vectDistrito = new BDManage(MySQLSentence.GetInstancia().getDistritoRegion(idreg, idArea), 1);

        }
        public void getVectDistritoRegionUnidad(string region, string area)
        {

            if (!string.IsNullOrEmpty(region) && !string.IsNullOrEmpty(area))
            {
                vectDistrito = new BDManage(SQLSentence.GetInstancia().getDistritoRegionArea(region, area), 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(region) && string.IsNullOrEmpty(area))
                    vectDistrito = new BDManage(SQLSentence.GetInstancia().getDistritoRegion(region), 1);
                if (string.IsNullOrEmpty(region) && !string.IsNullOrEmpty(area))
                    vectDistrito = new BDManage(SQLSentence.GetInstancia().getDistritoArea(area), 1);
            }
        }


        public void getVectPeticionario(string iddistr)
        {
            if (delegado != "True")
            {
                //if (!string.IsNullOrEmpty(idDistrito))
                //    vectPeticionarios = new BDManage(MySQLSentence.GetInstancia().getPeticionarioDistrito(idDistrito), 1);
                //else
                //    if (!string.IsNullOrEmpty(idRegion)) VectPeticionarios = new BDManage(MySQLSentence.GetInstancia().getPeticionarioRegion(idRegion), 1);
                //    else vectPeticionarios = new BDManage(MySQLSentence.GetInstancia().getAllPeticionarios(), 1);

                vectPeticionarios = new BDManage(SQLSentence.GetInstancia().getPeticionarios(idDistrito, idRegion, idArea, idUnidad), 1);
            }
            else
            {
                vectPeticionarios = new BDManage(SQLSentence.GetInstancia().getThePeticionario(idPeticionario), 1);
            }

            if (!string.IsNullOrEmpty(iddistr)) vectPeticionarios = new BDManage(SQLSentence.GetInstancia().getPeticionarioDistrito(iddistr), 1);
            if (iddistr == "0") vectPeticionarios = new BDManage(SQLSentence.GetInstancia().getAllPeticionarios(), 1);
        }

        public void getVectPeticionarioChangeDISTR_REG(string iddistr, string idreg)
        {
            if (!string.IsNullOrEmpty(iddistr)) vectPeticionarios = new BDManage(SQLSentence.GetInstancia().getPeticionarioDistrito(iddistr), 1);
            if (idreg != "") vectPeticionarios = new BDManage(SQLSentence.GetInstancia().getPeticionarioRegion(idreg), 1);
        }
        public void getVectDistritoUnidad(string unidad)
        {
            vectDistrito = new BDManage(SQLSentence.GetInstancia().getDistritoUnidad(unidad), 1);
        }

        //public string Id_jefeRegional;


        private string idPeticionario;
        public string IdPeticionario
        {
            get { return idPeticionario; }
            set { idPeticionario = value; }
        }

        private string fkIdEmpresa;
        public string FkIdEmpresa
        {
            get { return fkIdEmpresa; }
            set { fkIdEmpresa = value; }
        }


        private string login;
        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }



        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string apellidos;
        public string Apellidos
        {
            get { return apellidos; }
            set { apellidos = value; }
        }



        private string wein;
        public string Wein
        {
            get { return wein; }
            set { wein = value; }
        }

        private string idCargo;
        public string IdCargo
        {
            get { return idCargo; }
            set { idCargo = value; }
        }

        private string rol;
        public string Rol
        {
            get { return rol; }
            set { rol = value; }
        }

        private string idTratamiento;
        public string IdTratamiento
        {
            get { return idTratamiento; }
            set { idTratamiento = value; }
        }


        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private string movil;
        public string Movil
        {
            get { return movil; }
            set { movil = value; }
        }

        private string idEmpresa;
        public string IdEmpresa
        {
            get { return idEmpresa; }
            set { idEmpresa = value; }
        }

        private string idEmpleadoGp;
        public string IdEmpleadoGp
        {
            get { return idEmpleadoGp; }
            set { idEmpleadoGp = value; }
        }


        private string idUnidad;
        public string IdUnidad
        {
            get { return idUnidad; }
            set { idUnidad = value; }
        }

        private string nomUnidad;
        public string NomUnidad
        {
            get { return nomUnidad; }
            set { nomUnidad = value; }
        }

        private Unidad unidad;
        public Unidad Unidad
        {
            get { return unidad; }
            set { unidad = value; }
        }

        private string idArea;
        public string IdArea
        {
            get { return idArea; }
            set { idArea = value; }
        }

        private string nomArea;
        public string NomArea
        {
            get { return nomArea; }
            set { nomArea = value; }
        }

        private Area area;
        public Area Area
        {
            get { return area; }
            set { area = value; }
        }

        private string idProducto;
        public string IdProducto
        {
            get { return idProducto; }
            set { idProducto = value; }
        }

        private ProductoEmpresa producto;
        public ProductoEmpresa Producto
        {
            get { return producto; }
            set { producto = value; }
        }

        private string idRegion;
        public string IdRegion
        {
            get { return idRegion; }
            set { idRegion = value; }
        }

        private string nomRegion;
        public string NomRegion
        {
            get { return nomRegion; }
            set { nomRegion = value; }
        }

        private Region region;
        public Region Region
        {
            get { return region; }
            set { region = value; }
        }

        private string idDistrito;
        public string IdDistrito
        {
            get { return idDistrito; }
            set { idDistrito = value; }
        }

        private string nomDistrito;
        public string NomDistrito
        {
            get { return nomDistrito; }
            set { nomDistrito = value; }
        }

        private Distrito distrito;
        public Distrito Distrito
        {
            get { return distrito; }
            set { distrito = value; }
        }

        private string observaciones;
        public string Observaciones
        {
            get { return observaciones; }
            set { observaciones = value; }
        }
        private string delegado;
        public string Delegado
        {
            get { return delegado; }
            set { delegado = value; }
        }
        private string marketing;
        public string Marketing
        {
            get { return marketing; }
            set { marketing = value; }
        }


        private BDManage vectUnidad;
        public BDManage VectUnidad
        {
            get { return vectUnidad; }
            set { vectUnidad = value; }
        }

        private BDManage vectArea;
        public BDManage VectArea
        {
            get { return vectArea; }
            set { vectArea = value; }
        }

        private BDManage vectRegion;
        public BDManage VectRegion
        {
            get { return vectRegion; }
            set { vectRegion = value; }
        }
        private BDManage vectDistrito;
        public BDManage VectDistrito
        {
            get { return vectDistrito; }
            set { vectDistrito = value; }
        }
        private BDManage vectPeticionarios;
        public BDManage VectPeticionarios
        {
            get { return vectPeticionarios; }
            set { vectPeticionarios = value; }
        }

        public string IdDepartament { get; set; }
        public string NomDepartament { get; set; }
        public string IdFuerzaVentas { get; set; }
        public string NomFuerzaVentas { get; set; }
        public string IdDistrict { get; set; }
        public string NomDistrict { get; set; }
    }
}
