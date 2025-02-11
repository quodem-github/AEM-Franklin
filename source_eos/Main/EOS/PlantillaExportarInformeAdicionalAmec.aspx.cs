using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Web;
using EOS.Entidades.Datos;
using System.Data;

namespace EOS
{
    public partial class PlantillaExportarInformeAdicionalAmec : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
                DAmecInfo dAmecInfo = new DAmecInfo();

                string sIdDel = string.Empty;
                sIdDel = Request.QueryString["idamec"];
                string rutaDescargaDocumentacion = ConfigUtil.GetAppSetting("RutaDescargaDocumentacion");
                DataSet dt = agAmecInfo.ObtenerDocumentacionAdicionalAmec(sIdDel);

                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"ListadoAmec" + sIdDel + ".xls\"");

                Response.Write("<html><body>");
                Response.Write("<TABLE width=100% align=center border=1>");
                Response.Write("<caption><h1><u><b>" + Server.HtmlEncode(" AMEC: " + sIdDel) + " </b></u></h1></caption>");

                Response.Write("<tr bgcolor=#86B404><td> " + Server.HtmlEncode("ESTADO") + "</td><td> FECHA AMECS </td><td> USUARIO </td><td> SOLICITANTE </td><td> NOMBRE PROGRAMA </td><td> TIPO DE ACTIVIDAD </td><td> FECHA COMIENZO </td><td> " + Server.HtmlEncode("FECHA FINALIZACIÓN") + " </td><td> " + Server.HtmlEncode("NÚMERO DE PONENTES") + " </td><td> " + Server.HtmlEncode("PARTICIPANTES MSD") + " </td><td> " + Server.HtmlEncode("FECHA DOCUMENTO") + " </td><td> " + Server.HtmlEncode("CATEGORÍA DOCUMENTO") + " </td><td> " + Server.HtmlEncode("NOMBRE DE DOCUMENTO") + " </td><td> " + Server.HtmlEncode("COMENTÁRIOS DOCUMENTO") + " </td><td> " + Server.HtmlEncode("PARAGUAS") + " </td><td> " + Server.HtmlEncode("FARMAINDUSTRIA") + " </td><td> " + Server.HtmlEncode("ADJUNTADO A FI") + " </td><td> " + Server.HtmlEncode("LINK DESCARGA") + " </td></tr>");

                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    Response.Write("<tr>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["estado"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[i]["fechaamecs"] + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["Usuario"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["SolicitanteAmec"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["NombreProgramaActividad"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["tipoactividad"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[i]["fechacomienzo"].ToString() + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[i]["fechafinalizacion"].ToString() + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["NumPonentes"] != null ? dt.Tables[0].Rows[i]["NumPonentes"].ToString() : "") + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["participantesmsd"] != null ? dt.Tables[0].Rows[i]["participantesmsd"].ToString() : "") + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[0]["FechaDocumento"].ToString() + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["categoriadocumento"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["nombredoc"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["comentariosdoc"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[i]["Paraguas"].ToString() + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[i]["FarmaIndustria"].ToString() + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[i]["AdjuntadoaFI"].ToString() + "</td>");
                    
                    string nombredoc = dt.Tables[0].Rows[i]["nombredoc"].ToString();
                    nombredoc = nombredoc.Replace(" ", "%20");
                    Response.Write("<td align=left><a href = " + string.Format("{0}&idamec={1}&file={2}", rutaDescargaDocumentacion, sIdDel, Server.HtmlEncode(nombredoc)) + ">" + Server.HtmlEncode(dt.Tables[0].Rows[i]["nombredoc"].ToString()) + "</a></td>");
                    Response.Write("</tr>");
                }
            }
        }
    }
}