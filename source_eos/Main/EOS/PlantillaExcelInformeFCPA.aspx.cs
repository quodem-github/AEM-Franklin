using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Entidades.Datos;
using EOS.Web;
using System.Data;

namespace EOS
{
    public partial class PlantillaExcelInformeFCPA : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
                DAmecInfo dAmecInfo = new DAmecInfo();

                DataSet dt = (DataSet)Session["datasetFCPA"];

                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";

                Response.AddHeader("Content-Disposition", "attachment;filename=\"InformeFCPA.xls\"");

                Response.Write("<html><body>");
                Response.Write("<TABLE width=100% align=center border='1'>");
                Response.Write("<tr style='color:#FFFFFD' align=center><td bgcolor=#963634 colspan= '6'> DATOS AMEC </td><td bgcolor=#31869B colspan= '10'> DATOS USUARIO EOS </td><td bgcolor=#76933C colspan= '6'> DATOS FCPA </td>");
                Response.Write("<tr style='color:#FFFFFD'><td bgcolor=#963634> " + Server.HtmlEncode("Número amec") + "</td><td bgcolor=#963634> Nombre Programa Actividad" + "</td><td bgcolor=#963634> Tipo Actividad </td><td bgcolor=#963634>" +
                     " Fecha Inicio </td><td bgcolor=#963634>" + "Fecha Final </td><td bgcolor=#963634> " + "Estado Amec </td><td bgcolor=#31869B> " + "Usuario </td><td bgcolor=#31869B> " +
                     "Cargo </td><td bgcolor=#31869B> " + "Position </td><td bgcolor=#31869B> " + "Unidad </td><td bgcolor=#31869B> " + "Area </td><td bgcolor=#31869B> " + Server.HtmlEncode("Región") + "</td><td bgcolor=#31869B>" + "Distrito* </td><td bgcolor=#31869B>" + "Departamento </td><td bgcolor=#31869B>" + "Fuerza de Ventas </td><td bgcolor=#31869B>" + "Distrito </td><td bgcolor=#76933C> " +
                     Server.HtmlEncode("Fecha Certificación FCPA") + "</td><td bgcolor=#76933C>" + Server.HtmlEncode("Dónde se certifica") +
                     "</td><td bgcolor=#76933C> Tipo Reserva Colectivo </td><td bgcolor=#76933C> Nombre Pax </td><td bgcolor=#76933C> Apellido1 Pax </td><td bgcolor=#76933C> Msdid </td></tr>");
                    
                for (int i = 0; i <dt.Tables[0].Rows.Count; i++)
                {
                    Response.Write("<tr>");
                    //Datos Amec
                    Response.Write("<td bgcolor=#F2DCDB align=left>" + dt.Tables[0].Rows[i]["idamecs"].ToString() + "</td>");
                    Response.Write("<td bgcolor=#F2DCDB align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["nombreprogramaactividad"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#F2DCDB align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["tipoactividad"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#F2DCDB align=left>" + dt.Tables[0].Rows[i]["fechainicio"].ToString() + "</td>");
                    Response.Write("<td bgcolor=#F2DCDB align=left>" + dt.Tables[0].Rows[i]["fechafin"].ToString() + "</td>");
                    Response.Write("<td bgcolor=#F2DCDB align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["estadoamec"].ToString()) + "</td>");

                    //Datos Usuario
                    Response.Write("<td bgcolor=#DAEEF3 align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["usuario"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#DAEEF3 align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["cargo"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#DAEEF3 align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["position"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#DAEEF3 align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["unidad"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#DAEEF3 align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["area"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#DAEEF3 align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["region"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#DAEEF3 align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["distrito"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#DAEEF3 align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["departament"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#DAEEF3 align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["saleforce"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#DAEEF3 align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["district"].ToString()) + "</td>");

                    //DatosFCPA
                    Response.Write("<td bgcolor=#EBF1DE align=left>" + dt.Tables[0].Rows[i]["fechacertificacionFCPA"].ToString() + "</td>");
                    Response.Write("<td bgcolor=#EBF1DE align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["tiporiesgo"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#EBF1DE align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["tiporeservacolectivo"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#EBF1DE align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["nombrepax"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#EBF1DE align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[i]["apellido1pax"].ToString()) + "</td>");
                    Response.Write("<td bgcolor=#EBF1DE align=left>" + dt.Tables[0].Rows[i]["msdid"].ToString() + "</td>");
                    Response.Write("</tr>");
                }
                Response.Write("</TABLE>");
                Response.Write("</body></html>");
            }
        }

    }
}