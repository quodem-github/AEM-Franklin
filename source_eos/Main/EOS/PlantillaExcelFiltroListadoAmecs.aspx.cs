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
    public partial class PlantillaExcelFiltroListadoAmecs : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
                DAmecInfo dAmecInfo = new DAmecInfo();

                string sIdDel = string.Empty;
                IEnumerable<ListadoAmecs> listAmec = (IEnumerable<ListadoAmecs>)Session["IEnumAmec"];

                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";

                Response.AddHeader("Content-Disposition", "attachment;filename=\"ListadoAmecs.xls\"");

                Response.Write("<html><body>");
                Response.Write("<TABLE width=100% align=center>");
                Response.Write("<tr bgcolor=#86B404><td> " + Server.HtmlEncode("Número amec") + "</td><td> Estado </td><td>"+
                     Server.HtmlEncode("Fecha Creación") + "</td><td> Solicitante </td><td> Creador </td><td> Unidad </td><td> Area " +
                     "</td><td>"+ Server.HtmlEncode("Región")+ " </td><td> Distrito* </td><td> Departamento </td><td> Fuerza de ventas </td><td> Distrito </td><td> Nombre del Programa/Actividad </td><td>" +
                     "Tipo de Actividad </td><td> Fecha Comienzo </td><td>" + Server.HtmlEncode("Fecha Finalización") + "</td><td>" + Server.HtmlEncode("Año Inicio Actividad") +
                     "</td><td>" + Server.HtmlEncode("Año Fin Actividad") + "</td><td> Productos </td><td> Sede </td><td>" + Server.HtmlEncode("Núm. Ponentes") +
                     "</td><td> Participantes MSD </td><td> ProfSanitario_HonorariosMsd</td><td>Cartas Contrato</td><td> Concepto Gastos </td><td>" + Server.HtmlEncode("Criterio de Selección") +
                     "</td><td> Importe Gasto </td><td> Paraguas  </td><td> N20_FCPA </td><td> Farma_Industria </td><td> Casos_Clinicos </td><td> Preaprobado_Medico </td><td>" +
                     "Preaprobado_Negocio </td><td> Preaprobado_Legal </td></tr>");
                    
                foreach (ListadoAmecs lista in listAmec)
                {

                    DataSet dt  = agAmecInfo.CargarTodosValoresAmecExcel(lista.idamecs);
                    //AgenteUsuarios agUsua = new AgenteUsuarios();
                    //DDatosPersonalesUsuario datosSolicitante = agUsua.ObtenerDatosPersonalesPorIDPeticionario(dt.Tables[0].Rows[0]["idsolicitante"].ToString());
                    ////dAmecInfo = agAmecInfo.CargarTodosValoresAmec(100000035);

                    Response.Write("<tr>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[0]["idamecs"].ToString() + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["estado"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[0]["fechaamecs"] + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["Solicitante"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["Creador"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["unidad"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["area"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["region"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["Distrito*"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["Departamento"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["Fuerza de Ventas"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["Distrito"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["NombreProgramaActividad"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["tipoactividad"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[0]["fechacomienzo"].ToString() + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[0]["fechaFinalizacion"].ToString() + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[0][14].ToString() + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[0][15].ToString() + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["Productos"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["lugarsede"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["NumPonentes"] != null ? dt.Tables[0].Rows[0]["NumPonentes"].ToString() : "") + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["participantesmsd"] != null ? dt.Tables[0].Rows[0]["participantesmsd"].ToString() : "") + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["ProfSanitario_HonorariosMsd"] != null ? dt.Tables[0].Rows[0]["ProfSanitario_HonorariosMsd"].ToString() : "") + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["cartascontrato"] != null ? dt.Tables[0].Rows[0]["cartascontrato"].ToString() : "") + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["conceptogastos"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["criterioseleccion"].ToString()) + "</td>");
                    Response.Write("<td align=left>" + Server.HtmlEncode(dt.Tables[0].Rows[0]["importegasto"] != null ? dt.Tables[0].Rows[0]["importegasto"].ToString() : "") + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[0]["Paraguas"].ToString() + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[0]["N20_FCPA"].ToString() + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[0]["Farma_Industria"].ToString() + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[0]["Casos_Clinicos"].ToString() + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[0]["Preaprobado_Medico"].ToString() + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[0]["Preaprobado_Negocio"].ToString() + "</td>");
                    Response.Write("<td align=left>" + dt.Tables[0].Rows[0]["Preaprobado_Legal"].ToString() + "</td>");
                    Response.Write("</tr>");


                    //if (CollectiondCongresoAmec.Count() != 0)
                    //{
                    //    Response.Write("<tr><td colspan=4></td></tr>");
                    //    Response.Write("<tr bgcolor=#A9F5BC><td> CONGRESO </td><td>" + Server.HtmlEncode("POBLACIÓN DEL CONGRESO") + "</td><td> DESDE </td><td> HASTA </td></tr>");
                    //    foreach (DCongresos dCongresoAmec in CollectiondCongresoAmec)
                    //    {
                    //        Response.Write("<tr>");
                    //        //Inclourem la informació del Amec 

                    //        //Inclourem al Congrés
                    //        Response.Write("<td align=left>" + Server.HtmlEncode(dCongresoAmec.Congreso) + "</td>");
                    //        Response.Write("<td align=left>" + Server.HtmlEncode(dCongresoAmec.Poblacion) + "</td>");
                    //        Response.Write("<td align=left>" + dCongresoAmec.Desde + "</td>");
                    //        Response.Write("<td align=left>" + dCongresoAmec.Hasta + "</td>");

                    //        Response.Write("</tr>");
                    //    }
                    //    Response.Write("<tr height=30><td colspan=4></td></tr>");
                    //}


                }
                Response.Write("</TABLE>");
                Response.Write("</body></html>");
            }
        }

    }
}