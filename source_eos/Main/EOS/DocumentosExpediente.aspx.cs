using System;
using System.Collections.Generic;
using System.Web;
using EOS.Entidades.Datos;
using EOS.Web;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;

namespace EOS
{
    public partial class DocumentosExpediente : System.Web.UI.Page
    {
        public List<DGestorDocumentos> DocumentosExpedientes { get; set; }
        public List<DGestorDocumentos> DocumentosAmecs { get; set; }
        public bool IsExpIndividual { get; set; }

        public DCabeceraExpedienteAmpliado expediente
        {
            get { return (DCabeceraExpedienteAmpliado)HttpContext.Current.Session["currentExpediente"]; }

            set
            {
                HttpContext.Current.Session["currentExpediente"] = value;
                HttpContext.Current.Session["currentFKIdCongreso"] = value.Idactividad;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["key"] == null)
                    throw new ArgumentNullException("key",
                        "El argumento key con la clave de documento no ha sido facilitado");

                AgenteExpedientes agenteExp = new AgenteExpedientes();
                string key = Request.QueryString["key"];
                string idexp = EOS.Web.Encriptacion.Decrypt(key.Substring(0, key.Length - 4),
                    "Documents" + key.Substring(key.Length - 4));
                expediente = agenteExp.ObtenerExpedientePorID(idexp);

                //btnSalir.HRef = "Expedientes.aspx";

                // Información de cabecera
                InitInfoPanel();
                lblFechaDesde.Text = string.Format("{0:dd/MM/yyyy }", expediente.FechaDesde);
                LblFechaHasta.Text = string.Format("{0:dd/MM/yyyy }", expediente.FechaHasta);
                DAmec damec = agenteExp.ObtenerEntidadAMECporID(expediente.Idamec.ToString());
                AgenteMaestros am = new AgenteMaestros();
                if (damec.idconfempresa != null)
                {
                    var empresaConf = am.ObtenerEmpresaConf(damec.idconfempresa.Value);
                    lblAgencia.Text = empresaConf.nombreagencia;
                }
                LblPoblacion.Text = expediente.Poblacion;
                CalculaEstados();
                DocumentosExpedientes = ObtenerFicherosYCarpetasExpediente();
                DocumentosAmecs = ObtenerFicherosYCarpetasAmecs();
                IsExpIndividual = expediente.Idtiporeserva != 2;



            }
        }

        private void InitInfoPanel()
        {
            this.pedidoLabelValue.Text = this.expediente.Pedido;
            this.pedidoLabelExpediente.Text = this.expediente.Idexpediente.ToString();
            this.fechaExpedienteLabelValue.Text = this.expediente.Fechacreacion.ToString();
            this.amecLabelValue.Text = this.expediente.Amec;
            this.congresoLabelValue.Text = this.expediente.Actividad;
            this.imgExpedienteDetalle.ToolTip = String.Format("Ver detalles del AMEC {0}", this.expediente.Idamec);
            this.imgExpedienteDetalle.PostBackUrl =
                Page.ResolveUrl(String.Format("DetalleAMEC.aspx?idamec={0}", this.expediente.Idamec));
        }

        private void CalculaEstados()
        {
            AgenteExpedientes agExp = new AgenteExpedientes();
            lblEstado.CssClass = string.Format("eosImagenEstado eosImagenLeyenda{0}", expediente.Idestado);
            if (expediente.Idestado == "AB")
            {
                lblEstado.Text = agExp.ObtenerLabelEstado("AB");
            }
            else if (expediente.Idestado == "NC")
            {
                lblEstado.Text = agExp.ObtenerLabelEstado("NC");
            }
            else if (expediente.Idestado == "FZ")
            {
                lblEstado.Text = agExp.ObtenerLabelEstado("FZ");
            }
            else if (expediente.Idestado == "CR")
            {
                lblEstado.Text = agExp.ObtenerLabelEstado("CR");
            }
            else if (expediente.Idestado == "CN")
            {
                lblEstado.Text = agExp.ObtenerLabelEstado("CN");
            }
            else if (expediente.Idestado == "CNTR")
            {
                lblEstado.Text = agExp.ObtenerLabelEstado("CNTR");
            }
            else if (expediente.Idestado == "CFP")
            {
                lblEstado.Text = agExp.ObtenerLabelEstado("CFP");
            }
            else if (expediente.Idestado == "ACP")
            {
                lblEstado.Text = agExp.ObtenerLabelEstado("ACP");
            }
            else if (expediente.Idestado == "AC")
            {
                lblEstado.Text = agExp.ObtenerLabelEstado("AC");
            }
            else if (expediente.Idestado == "TR")
            {
                lblEstado.Text = agExp.ObtenerLabelEstado("TR");
            }
            else
            {
                lblEstado.Text = agExp.ObtenerLabelEstado("CN");
            }
        }


        private List<DGestorDocumentos> ObtenerFicherosYCarpetasAmecs()
        {
            List<DGestorDocumentos>  documentos = new List<DGestorDocumentos>();
            string amecsPath = ConfigurationManager.AppSettings["AmecDocumentation"];
            string amecsDocumentacion = amecsPath + expediente.Amec + "\\Documentacion";
            string amecsPrograma = amecsPath + expediente.Amec+ "\\Programa" ;

            if (System.IO.Directory.Exists(amecsPrograma))
            {
                documentos.Add(new DGestorDocumentos()
                {
                    Ruta = amecsPrograma,
                    Ficheros = OrganizarFicheros(amecsPrograma)
                });
            }
            if (System.IO.Directory.Exists(amecsDocumentacion))
            {
                DGestorDocumentos gestDoc = new DGestorDocumentos()
                {
                    Ruta = amecsDocumentacion,
                    Ficheros = OrganizarFicheros(amecsDocumentacion)
                };

                documentos.Add(gestDoc);


                List<string> dirsDocumentacion = System.IO.Directory.GetDirectories(amecsDocumentacion).ToList();
                List<DGestorDocumentos> subTiposDocumentacion = new List<DGestorDocumentos>();
           
                foreach (var dirs in dirsDocumentacion)
                {
                    subTiposDocumentacion.Add(new DGestorDocumentos()
                    {
                        Ruta = dirs,
                        Ficheros = OrganizarFicheros(dirs),
                    });
                }

                DGestorDocumentos documentacion = new DGestorDocumentos()
                {
                    Ruta = amecsDocumentacion,
                    SubTipo = subTiposDocumentacion
                };
                if(documentacion.Ficheros != null || documentacion.SubTipo.Count > 0)
                {
                    documentos.Add(documentacion);
                }

            }

            return documentos;
        }

        private List<DGestorDocumentos> ObtenerFicherosYCarpetasExpediente()
        {
            List<DGestorDocumentos> documentos = new List<DGestorDocumentos>();
            AgenteExpedientes agenteExp = new AgenteExpedientes();
            DAmecExpediente relacion = agenteExp.ObtenerRelacionAmecExpediente(expediente.Idexpediente);
            //string amecsPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AmecDocumentation"]);
            string amecsPath = ConfigurationManager.AppSettings["AmecDocumentation"];
            string expPath = amecsPath + relacion.IdAmecs + "\\" + relacion.IdExpediente + "\\";

            if (!System.IO.Directory.Exists(expPath))
            {
                System.IO.Directory.CreateDirectory(expPath);
            }

            string[] dirsTipo = System.IO.Directory.GetDirectories(expPath);

            foreach (var tipo in dirsTipo)
            {
                documentos.Add(new DGestorDocumentos()
                {
                    Ruta = tipo,
                    Ficheros = OrganizarFicheros(tipo),
                    SubTipo = ObtenerSubTipoDocs(tipo)
                });
            }
            return documentos;
        }

        private List<DGestorDocumentos> ObtenerSubTipoDocs(string rutaPadre)
        {
            List<DGestorDocumentos> documentos = new List<DGestorDocumentos>();
            string[] dirs = System.IO.Directory.GetDirectories(rutaPadre);

            foreach (var dir in dirs)
            {
                documentos.Add(new DGestorDocumentos()
                {
                    Ruta = dir,
                    Ficheros = OrganizarFicheros(dir),
                    SubTipo = ObtenerSubTipoDocs(dir)
                });
            }

            return documentos;
        }
        
        private List<DGestorFicheros> OrganizarFicheros(string rutaFicheros)
        {
            AgenteExpedientes agenteExp = new AgenteExpedientes();
            List<DGestorFicheros> lista = new List<DGestorFicheros>();
            List<DDocumentoVersion> datosDoc =
                agenteExp.ObtenerDatosDocumentos(rutaFicheros).OrderBy(x => x.Version).ToList();
            List<int> idsDocumento = datosDoc.Select(x => x.IdDocumento).Distinct().ToList();
            foreach (var iddoc in idsDocumento)
            {
                lista.Add(new DGestorFicheros()
                {
                    NombreOrignal = datosDoc.FirstOrDefault(x => x.IdDocumento == iddoc).NombreOriginal,
                    Versiones = datosDoc.Where(x => x.IdDocumento == iddoc).Select(x => new DFicheroFecha()
                    {
                        FechaFichero = x.FechaDocVersion,
                        NombreFichero = x.RutaFichero.Replace("\\", "/"),
                        NombreFicheroEncriptado = Quodem.Utility.Cryptography.SimpleEncryption.Encrypt(x.RutaFichero.Replace("\\", "/")),
                        DocumentoNoValido = x.DocumentoNoValido

                    }).ToList()
                });
            }

            return lista;

        }


    }
}