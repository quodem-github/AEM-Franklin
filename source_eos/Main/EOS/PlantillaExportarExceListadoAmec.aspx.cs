using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Entidades.Datos;
using EOS.Web;



namespace EOS
{
    public partial class PlantillaExportarExceListadoAmec : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
                DAmecInfo dAmecInfo = new DAmecInfo();

                string sIdDel = string.Empty;
                sIdDel = Request.QueryString["idamec"];
                dAmecInfo = agAmecInfo.CargarTodosValoresAmec(sIdDel);
                AgenteUsuarios agUsua = new AgenteUsuarios();
                DDatosPersonalesUsuario datosCreador = agUsua.ObtenerDatosPersonalesPorIDPeticionario(dAmecInfo.idcreadopor.ToString());
                DDatosPersonalesUsuario datosSolicitante = agUsua.ObtenerDatosPersonalesPorIDPeticionario(dAmecInfo.idsolicitante.ToString());
                 
                //dAmecInfo = agAmecInfo.CargarTodosValoresAmec(100000035);
                ICollection<DCongresos> CollectiondCongresoAmec = agAmecInfo.CargarEventoActividadesRelacionadasAmec(sIdDel);
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"ListadoAmec"+sIdDel+".xls\"");

                Response.Write("<html><body>");
                Response.Write("<TABLE width=100% align=center>");
                Response.Write("<caption><h1><u><b>" + Server.HtmlEncode(" LISTADO AMEC/CONGRESOS ") + " </b></u></h1></caption>");
                    

                Response.Write("<tr><td colspan=6><h2>" + Server.HtmlEncode(" AMEC: ") + " </h2></td></tr>");
                Response.Write("<tr bgcolor=#86B404><td> " + Server.HtmlEncode("NÚMERO AMEC") + "</td><td> NWEIN </td><td> CREADO POR </td><td> SOLICITANTE </td><td>" + Server.HtmlEncode("FECHA CREACIÓN") + " </td><td>" + Server.HtmlEncode("IMPORTE GASTO") + " </td><td>" + Server.HtmlEncode("ESTADO") + " </td><td> " + Server.HtmlEncode("TIPO DE ACTIVIDAD") + " </td><td> " + Server.HtmlEncode("DESCRIPCIÓN") + " </td></tr>");
                Response.Write("<tr>");
                Response.Write("<td align=left>" + dAmecInfo.idamecs + "</td>");
                Response.Write("<td align=left>" + dAmecInfo.nwein + "</td>");
                Response.Write("<td align=left>" + Server.HtmlEncode(datosCreador.NombreCompleto) + "</td>");
                Response.Write("<td align=left>" + Server.HtmlEncode(datosSolicitante.NombreCompleto) + "</td>");
                Response.Write("<td align=left>" + dAmecInfo.fechaamecs + "</td>");
                Response.Write("<td align=left>" + dAmecInfo.importegasto + "</td>");
                Response.Write("<td align=left>" + dAmecInfo.idestado + "</td>");
                Response.Write("<td align=left>" + dAmecInfo.idtipoactividad + "</td>");
                Response.Write("<td align=left>" + Server.HtmlEncode(dAmecInfo.descripcion) + "</td>");
                Response.Write("</tr>");


                if (CollectiondCongresoAmec.Count() != 0)
                    {
                        Response.Write("<tr height=70><td colspan=4></td></tr>");
                        Response.Write("<tr><td colspan=4><h2>" + Server.HtmlEncode(" CONGRESO: ") + " </h2></td></tr>");
                        Response.Write("<tr bgcolor=#A9F5BC><td> CONGRESO </td><td>" + Server.HtmlEncode("POBLACIÓN DEL CONGRESO") + "</td><td> DESDE </td><td> HASTA </td></tr>");
                        foreach (DCongresos dCongresoAmec in CollectiondCongresoAmec)
                        {
                            Response.Write("<tr>");
                            //Inclourem la informació del Amec 

                            //Inclourem al Congrés
                            Response.Write("<td align=left>" + Server.HtmlEncode(dCongresoAmec.Congreso) + "</td>");
                            Response.Write("<td align=left>" + Server.HtmlEncode(dCongresoAmec.Poblacion) + "</td>");
                            Response.Write("<td align=left>" + dCongresoAmec.Desde + "</td>");
                            Response.Write("<td align=left>" + dCongresoAmec.Hasta + "</td>");

                            Response.Write("</tr>");
                        }
                    
                    }

                Response.Write("</TABLE>");
                Response.Write("</body></html>");
            }
        }

    }
}