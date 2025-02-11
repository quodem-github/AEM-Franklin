using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Entidades.Datos;
using EOS.Web;
using System.Text;

namespace EOS
{
    public partial class PlantillaExportarAmecExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
                DAmecInfo dAmecInfo = new DAmecInfo();
                AgenteAprobadorAmec agAprobAmec = new AgenteAprobadorAmec();
                AgentePeticionarioManager agPetManager = new AgentePeticionarioManager();
                

                string sIdDel = string.Empty;
                sIdDel = Request.QueryString["idamec"];
                dAmecInfo = agAmecInfo.CargarTodosValoresAmec(sIdDel);
                //dAmecInfo = agAmecInfo.CargarTodosValoresAmec(100000014);
                string prova = "ProVa";
                List<int> idAprobadores = agAprobAmec.ObernerIdsAprobadoresAmec(sIdDel);
                List<DPeticionarioManager> aprobadores = agPetManager.ObtenerPeticionarioDesdeLista(idAprobadores).ToList();

                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"DetalleAmec" + sIdDel + ".xls\"");
                //Response.ContentEncoding = Encoding.UTF8;

                Response.Charset = "iso-8859-1";
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.ContentType = "application/ms-excel";

                Response.Write("<html><body>");
                Response.Write("<TABLE width=\"100%\" align=\"center\" border=\"2\">");
                Response.Write("<caption><h1><u><b>" + Server.HtmlEncode(" SOLICITUD DE APROBACIÓN DE ACTIVIDADES MÉDICO CIENTÍFICAS") + " </b></u></h1></caption>");

                //TODO : TESTEAR
                AgenteUsuarios agenteUsu = new AgenteUsuarios();
                EOS.Entidades.Datos.DDatosPersonalesUsuario datosUsuarioSolicitante;
                datosUsuarioSolicitante = agenteUsu.ObtenerDatosPersonalesPorIDPeticionario(dAmecInfo.idsolicitante.ToString());
                string nombreSolicitante = datosUsuarioSolicitante.NombreCompleto;
                //Response.Write("<tr><td></td> <td width=120></td> <td width=120></td> <td width=120></td> <td></td> <td></td> <td></td> <td></td> <td width=150></td> <td></td> <td></td>  </tr>");
                Response.Write("<tr> <td  colspan=\"1\" width=\"100\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Solicitante: </b></FONT></td>");
                Response.Write("<td colspan=\"4\" bgcolor=\"#DDF5E3\">" + Server.HtmlEncode(nombreSolicitante) + "</td>");


                Response.Write("<td width=\"100\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> N. WEIN: </b></FONT></td>");
                Response.Write("<td bgcolor=\"#DDF5E3\"  colspan=\"1\" width=\"150\">" + dAmecInfo.nwein + "</td>");
                Response.Write("<td colspan=\"2\"></td>");
                Response.Write("<td colspan=\"1\" width=\"90\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>N. Amec: </b></FONT></td>");
                Response.Write("<td bgcolor=\"#A9BCF5\"  colspan=\"1\" width=\"150\">" + dAmecInfo.idamecs + "</td> </tr>");


                AgenteAMEC agSol_AMEC = new AgenteAMEC(datosUsuarioSolicitante.IdPeticionario);
                string cargo = agSol_AMEC.ObtenerCargo();
                Response.Write("<tr> <td  colspan=\"1\" width=\"80\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Cargo: </b></FONT></td>");
                Response.Write("<td colspan=\"3\" bgcolor=\"#B7E43A\">" + Server.HtmlEncode(cargo) + "</td>");

                Response.Write("<td  colspan=\"1\" width=\"100\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Fecha: </b></FONT></td>");
                Response.Write("<td colspan=\"2\">" + dAmecInfo.fechaamecs + "</td>");

                string paraguas;
                if (dAmecInfo.paraguas == true)
                {
                    paraguas = "SI";
                    Response.Write("<td  colspan=\"1\"  width=\"100\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode(" Paraguas:") + " </b></FONT></td>");
                    Response.Write("<td colspan=\"3\" bgcolor=\"#B7E43A\">" + paraguas + "</td></tr>");
                }
                else
                {
                    paraguas = "NO";
                    Response.Write("<td  colspan=\"1\"  width=\"100\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode(" Paraguas:") + " </b></FONT></td>");
                    Response.Write("<td colspan=\"3\" bgcolor=\"#B7E43A\">" + paraguas + "</td></tr>");
                }

                string preaprobadaamed;
                if (dAmecInfo.preaprobadaamed == true) { preaprobadaamed = "SI"; } else { preaprobadaamed = "NO"; }
                Response.Write("<tr> <td  colspan=\"4\" ><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode(" Preaprobada médico:") + " </b></FONT></td>");
                Response.Write("<td  colspan=\"1\" bgcolor=\"#B7E43A\">" + preaprobadaamed + "</td>");
                Response.Write("<td  colspan=\"6\" ></td></tr>");


                string preaprobadaneg;
                if (dAmecInfo.preaprobadaneg == true) { preaprobadaneg = "SI"; } else { preaprobadaneg = "NO"; }
                Response.Write("<tr> <td  colspan=\"4\" ><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Preaprobada negocio: </b></FONT></td>");
                Response.Write("<td  colspan=\"1\" bgcolor=\"#B7E43A\">" + preaprobadaneg + "</td>");
                Response.Write("<td  colspan=\"6\" ></td></tr>");


                string preaprobadaleg;
                if (dAmecInfo.preaprobadaleg == true) { preaprobadaleg = "SI"; } else { preaprobadaleg = "NO"; }
                Response.Write("<tr> <td  colspan=\"4\" ><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Preaprobada legal / compliance: </b></FONT></td>");
                Response.Write("<td  colspan=\"1\" bgcolor=\"#B7E43A\">" + preaprobadaleg + "</td>");
                Response.Write("<td  colspan=\"6\" ></td></tr>");


                string farmaindustria;
                if (dAmecInfo.farmaindustria == true) { farmaindustria = "SI"; } else { farmaindustria = "NO"; }
                Response.Write("<tr> <td  colspan=\"4\" ><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode("Comunicación a Farmaindustria:") + " </b></FONT></td>");
                Response.Write("<td  colspan=\"1\" bgcolor=\"#B7E43A\">" + farmaindustria + "</td>");
                Response.Write("<td  colspan=\"6\" ></td></tr>");


                string casosclinicos;
                if (dAmecInfo.casosclinicos == true) { casosclinicos = "SI"; } else { casosclinicos = "NO"; }
                Response.Write("<tr> <td  colspan=\"4\" ><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode(" ¿Conlleva la gestión de casos clínicos con productos MSD?: ") + " </b></FONT></td>");
                Response.Write("<td  colspan=\"1\" bgcolor=\"#B7E43A\">" + casosclinicos + "</td>");
                Response.Write("<td  colspan=\"6\" ></td></tr>");


                string politicaN20;
                if (dAmecInfo.politicaN20 == true) { politicaN20 = "SI"; } else { politicaN20 = "NO"; }
                //                Response.Write("<tr> <td  colspan=4 ><FONT FACE=Times New Roman SIZE=2><b>" + Server.HtmlEncode(" Actividad conforme con la política N. 20/FCPA:") + " </b></FONT></td>");
                Response.Write("<tr> <td  colspan=\"4\" ><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode(" Actividad conforme con la C.Pol 5/FCPA:") + " </b></FONT></td>");
                Response.Write("<td  colspan=\"1\" bgcolor=\"#B7E43A\">" + politicaN20 + "</td>");
                Response.Write("<td  colspan=\"6\" ></td></tr>");


                //TODO :obtener el nombre de la actividad de la tabla tipoactividad por el dAmecInfo.idtipoactividad.

                if (dAmecInfo.idtipoactividad != -1)
                {
                    AgenteFlujoAprobacion agenteFlujo = new AgenteFlujoAprobacion();
                    DTipoActividadFlujo datosActividad = agenteFlujo.ObtenerTipoActividadXid(Int32.Parse(dAmecInfo.idtipoactividad.ToString()));
                    Response.Write("<tr height=\"40\"> <td  colspan=\"2\" ><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Tipo de Actividad: </b></FONT></td>");
                    Response.Write("<td  colspan=\"9\" bgcolor=\"#B7E43A\">" + Server.HtmlEncode(datosActividad.tipoactividad.ToString()) + "</td></tr>");
                }
                //Response.Write("<tr> <td  colspan=2 ><FONT FACE=Times New Roman SIZE=2><b> Especificar Otras: </b></FONT></td>");
                //Response.Write("<td  colspan=9 bgcolor=#DDF5E3>" + prova + "</td></tr>");

                Response.Write("<tr height=\"70\"> <td colspan=\"2\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Nombre del Programa/Actividad: </b></FONT></td>");
                Response.Write("<td colspan=\"9\" bgcolor=\"#DDF5E3\">" + Server.HtmlEncode(dAmecInfo.descripcion) + "</td></tr>");

                Response.Write("<tr height=\"70\"> <td colspan=\"2\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode(" Descripción/Objetivos:") + "</b></FONT></td>");
                Response.Write("<td colspan=\"9\" bgcolor=\"#DDF5E3\">" + Server.HtmlEncode(dAmecInfo.descripcionobjetivo) + "</td></tr>");

                Response.Write("<tr height=\"40\"> <td colspan=\"2\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode("Lugar de realización, sede y categoría:") + " </b></FONT></td>");
                Response.Write("<td colspan=\"9\" bgcolor=\"#DDF5E3\">" + Server.HtmlEncode(dAmecInfo.lugarsede) + "</td></tr>");

                Response.Write("<tr> <td colspan=\"2\" bgcolor=\"#D1D1D1\"></td>");
                Response.Write("<td><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode(" Duración (horas):") + "</b></FONT></td>");
                Response.Write("<td bgcolor=\"#DDF5E3\">" + dAmecInfo.duracionhoras + "</td>");

                Response.Write("<td colspan=\"3\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode(" Núm. Total de participantes por parte de MSD:") + "</b></FONT></td>");
                Response.Write("<td bgcolor=\"#DDF5E3\">" + dAmecInfo.participantesmsd + "</td>");

                Response.Write("<td colspan=\"2\" align=\"right\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode(" Número de Ponentes:") + "</b></FONT></td>");
                Response.Write("<td bgcolor=\"#DDF5E3\">" + dAmecInfo.ponentespatrocinados + "</td></tr>");

                Response.Write("<tr> <td colspan=\"2\" bgcolor=\"#D1D1D1\"></td>");
                Response.Write("<td><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode("N. Profesionales Sanitarios percibiendo honorarios de MSD:") + "</b></FONT></td>");
                Response.Write("<td width=\"150\" bgcolor=\"#DDF5E3\">" + dAmecInfo.profesionalessanitarios + "</td>");

                Response.Write("<td colspan=\"2\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Fecha de Comienzo Prevista:</b></FONT></td>");
                Response.Write("<td bgcolor=\"#B7E43A\">" + dAmecInfo.fechacomienzo + "</td>");

                Response.Write("<td colspan=\"3\" align=\"right\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> " + Server.HtmlEncode("Fecha de Finalización Prevista:") + "</b></FONT></td>");
                Response.Write("<td bgcolor=\"#B7E43A\">" + dAmecInfo.fechafinalizacion + "</td></tr>");


                Response.Write("<tr height=\"40\"> <td colspan=\"2\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode("Detalles de los Criteros de Selección:") + "</b></FONT></td>");
                Response.Write("<td colspan=\"9\" bgcolor=\"#B7E43A\">" + Server.HtmlEncode(dAmecInfo.detallecriterios) + "</td></tr>");

                //TODO :FALTA CAMPO EN LA BD...aun nose como lo haremos.
                Response.Write("<tr> <td colspan=\"2\" align=\"right\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Otro (Especificar):</b></FONT></td>");
                Response.Write("<td colspan=\"9\" bgcolor=\"#DDF5E3\"></td></tr>");

                // AMS: abajo!

                string medicosfichero;
                if (dAmecInfo.medicosfichero == true) { medicosfichero = "SI"; } else { medicosfichero = "NO"; }
                Response.Write("<tr> <td colspan=\"7\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode("Todos los mC)dicos que han sido incluidos en la actividad estC!n dados de alta en el fichero APPIAN del delegado.") + "</b></FONT></td>");
                Response.Write("<td bgcolor=\"#B7E43A\">" + medicosfichero + "</td>");
                Response.Write("<td colspan=\"3\" bgcolor=\"#D1D1D1\"></td></tr>");

                Response.Write("<tr height=\"50\"> <td colspan=\"11\" bgcolor=\"#D1D1D1\" align=\"center\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b></b></FONT></td></tr>");
                Response.Write("<tr height=\"50\"> <td colspan=\"11\" bgcolor=\"#D1D1D1\" align=\"center\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b></b></FONT></td></tr>");





                Response.Write("<tr height=\"90\"> <td colspan=\"2\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Concepto de los gastos y desglose en euros: <br/> (honorarios, alojamiento, inscripciones, desplazamiento, hospitalidad)</b></FONT></td>");
                Response.Write("<td colspan=\"9\" bgcolor=\"#DDF5E3\">" + Server.HtmlEncode(dAmecInfo.conceptogastos) + "</td></tr>");


                Response.Write("<tr> ");
                Response.Write("<td colspan=\"2\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> IMPORTE CON CARGO A PRODUCTO:</b></FONT></td>");
                Response.Write("<td bgcolor=\"#B7E43A\">" + dAmecInfo.cargoadaxas.ToString() + "</td>");

                Response.Write("<td colspan=\"3\" align=\"right\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> " + Server.HtmlEncode("Importe total del gasto (aproximado Euros):") + "</b></FONT></td>");
                Response.Write("<td bgcolor=\"#DDF5E3\">" + dAmecInfo.importegasto + "</td>");

                Response.Write("<td colspan=\"3\" align=\"right\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> N. Cartas contrato que esperamos:</b></FONT></td>");
                Response.Write("<td bgcolor=\"#B7E43A\">" + dAmecInfo.cartascontrato + "</td></tr>");

                AgenteComentariosAMEC agComentAMEC = new AgenteComentariosAMEC();
                int numComentariosAmec = agComentAMEC.ObtenerNumeroComentariosAMEC(dAmecInfo.idamecs.ToString());
                ICollection<DComentariosAmec> comentariosAsociadosAmec = agComentAMEC.ObtenerComentariosAmec(dAmecInfo.idamecs.ToString(), "", 0, 50);
                if (numComentariosAmec > 0)
                {
                    Response.Write("<tr><td colspan=\"3\"  width=\"100\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Comentarios del AMEC</b></FONT></td></tr>");
                    Response.Write("<tr><td colspan=\"5\" bgcolor=\"#B7E43A\"><b> Comentario </b></td><td colspan=\"2\" bgcolor=\"#B7E43A\"><b> Usuario </b></td><td colspan=\"2\" bgcolor=\"#B7E43A\"><b>" + Server.HtmlEncode("Fecha CreaciC3n") + "</b></td></tr>");
                    foreach (DComentariosAmec comentAsocAmec in comentariosAsociadosAmec)
                    {
                        Response.Write("<tr><td colspan=\"5\">" + Server.HtmlEncode(comentAsocAmec.comentariosdoc) + "</td>");
                        Response.Write("<td colspan=\"2\">" + Server.HtmlEncode(comentAsocAmec.nombreusuario) + "</td>");
                        Response.Write("<td colspan=\"2\" align=\"left\">" + Server.HtmlEncode(comentAsocAmec.fechacreacion.ToString()) + "</td></tr>");
                    }
                }

                Response.Write("<tr height=\"50\"> <td colspan=\"11\" bgcolor=\"#D1D1D1\" align=center><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode("El listado definitivo de asistentes se subirá a APPIAN sin demora una vez concluida la actividad") + "  </b></FONT></td></tr>");

                Response.Write("<tr height=\"50\">  <td colspan=\"11\" bgcolor=\"#D1D1D1\" align=center><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode("Como norma general, se consideran preaprobados aquellos programas que cumplan los requisitos especificados en el punto 4 de la PolC-tica AMEC, ademC!s de estar dentro ficha tC)cnica.") + " </b></FONT></td></tr>");

                Response.Write("<tr height=\"50\">  <td colspan=\"11\" bgcolor=\"#D1D1D1\" align=center><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode("Cualquier desviaciC3n requerira la aprobaciC3n expresa y deberC! enviarse a la siguiente direcciC3n de correo electrC3nico para su aprobacion: invitations_spain@merck.com") + "</b></FONT></td></tr>");


                //Escribir aqui aprobadores
                Response.Write("<tr height=\"50\"><td colspan=\"11\" ></td></tr>");
                Response.Write("<tr> <td colspan=\"11\" ALIGN =\"center\" ><FONT FACE=\"Times New Roman\" SIZE=\"4\"><b> APROBADORES </b></FONT></td></tr>");
                Response.Write("<td colspan=\"11\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Nombre Completo </b></FONT></td>");
                foreach (var aprobador in aprobadores)
                {
                    Response.Write("<tr> <td colspan=\"11\" ALIGN =\"center\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"> " + aprobador.Nombre + " " + aprobador.Apellido1 + " " + aprobador.Apellido2 + " </FONT></td></tr>");
                }

                ICollection<DVCongresoAmec> DvCongresoAmec = agAmecInfo.ObtenerActividadesAsigAmec(dAmecInfo.idamecs, "", 0, 0);
                Response.Write("<tr height=\"50\"><td colspan=\"12\" ></td></tr>");
                Response.Write("<tr> <td colspan=\"12\" ><FONT FACE=\"Times New Roman\" SIZE=\"4\"><b> EVENTOS ASOCIADOS </b></FONT></td></tr>");
                Response.Write("<tr> <td colspan=\"3\" ALIGN =\"center\"  bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Id. </b></FONT></td>");
                Response.Write("<td colspan=\"3\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Nombre </b></FONT></td>");
                Response.Write("<td colspan=\"2\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Lugar </b></FONT></td>");
                Response.Write("<td colspan=\"2\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Desde </b></FONT></td>");
                Response.Write("<td colspan=\"2\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Hasta </b></FONT></td></tr>");
                if (DvCongresoAmec != null && DvCongresoAmec.Count > 0)
                {
                    foreach (DVCongresoAmec EventosCongreso in DvCongresoAmec)
                    {
                        Response.Write("<tr><td colspan=\"3\" bgcolor=\"#B7E43A\" ALIGN =\"center\">" + Server.HtmlEncode(EventosCongreso.IdCongreso.ToString()) + "</td>");
                        Response.Write("<tr><td colspan=\"3\" bgcolor=\"#DDF5E3\" ALIGN =\"center\">" + Server.HtmlEncode(EventosCongreso.Congreso) + "</td>");
                        Response.Write("<td colspan=\"2\" bgcolor=\"#B7E43A\" ALIGN =\"center\">" + Server.HtmlEncode(EventosCongreso.Poblacion) + "</td>");
                        Response.Write("<td colspan=\"2\" bgcolor=\"#DDF5E3\" ALIGN =\"center\">" + Server.HtmlEncode(EventosCongreso.Desde.ToString()) + "</td>");
                        Response.Write("<td colspan=\"2\" bgcolor=\"#B7E43A\" ALIGN =\"center\">" + Server.HtmlEncode(EventosCongreso.Hasta.ToString()) + "</td></tr>");

                    }
                }

                ICollection<DDocumentacionAmec> ListDocumentacionAmec = agAmecInfo.ObtenerDocumentacionAMEC(dAmecInfo.idamecs.ToString(), "", 0, 0);
                Response.Write("<tr height=\"50\"><td colspan=\"12\" ></td></tr>");
                Response.Write("<tr> <td colspan=\"12\" ><FONT FACE=\"Times New Roman\" SIZE=\"4\"><b> DOCUMENTOS ASOCIADOS </b></FONT></td></tr>");
                Response.Write("<tr> <td colspan=\"2\" ALIGN =\"center\"  bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Tipo </b></FONT></td>");
                Response.Write("<td colspan=\"2\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Documento </b></FONT></td>");
                Response.Write("<td colspan=\"2\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Fecha </b></FONT></td>");
                Response.Write("<td colspan=\"3\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Comentario </b></FONT></td>");
                Response.Write("<td colspan=\"3\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b>" + Server.HtmlEncode(" CategorC-a ") + " </b></FONT></td></tr>");
                if (ListDocumentacionAmec != null && ListDocumentacionAmec.Count > 0)
                {
                    foreach (DDocumentacionAmec DocAmec in ListDocumentacionAmec)
                    {
                        Response.Write("<tr><td colspan=\"2\" bgcolor=\"#B7E43A\" ALIGN =\"center\">" + Server.HtmlEncode(DocAmec.tipodoc) + "</td>");
                        Response.Write("<td colspan=\"2\" bgcolor=\"#DDF5E3\" ALIGN =\"center\">" + Server.HtmlEncode(DocAmec.nombredoc) + "</td>");
                        Response.Write("<td colspan=\"2\" bgcolor=\"#B7E43A\" ALIGN =\"center\">" + Server.HtmlEncode(DocAmec.fechacreacion.ToString()) + "</td>");
                        Response.Write("<td colspan=\"3\" bgcolor=\"#DDF5E3\" ALIGN =\"center\">" + Server.HtmlEncode(DocAmec.comentariosdoc) + "</td>");
                        Response.Write("<td colspan=\"3\" bgcolor=\"#DDF5E3\" ALIGN =\"center\">" + Server.HtmlEncode(DocAmec.categoriadocumento) + "</td></tr>");
                    }
                }

                AgenteComentariosAMEC agComentAmec = new AgenteComentariosAMEC();
                ICollection<DComentariosAmec> ListComentariosAmec = agComentAmec.ObtenerComentariosAmec(dAmecInfo.idamecs.ToString(), "", 0, 0);
                Response.Write("<tr height=\"50\"><td colspan=\"12\" ></td></tr>");
                Response.Write("<tr> <td colspan=\"12\" ><FONT FACE=\"Times New Roman\" SIZE=\"4\"><b> COMENTARIOS</b></FONT></td></tr>");
                Response.Write("<tr> <td colspan=\"3\" ALIGN =\"center\"  bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Usuario </b></FONT></td>");
                Response.Write("<td colspan=\"3\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Fecha </b></FONT></td>");
                Response.Write("<td colspan=\"6\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Comentarios </b></FONT></td></tr>");
                if (ListComentariosAmec != null && ListComentariosAmec.Count > 0)
                {
                    foreach (DComentariosAmec ComAmec in ListComentariosAmec)
                    {
                        Response.Write("<tr><td colspan=\"3\" bgcolor=\"#B7E43A\" ALIGN =\"center\">" + Server.HtmlEncode(ComAmec.nombreusuario) + "</td>");
                        Response.Write("<td colspan=\"3\" bgcolor=\"#DDF5E3\" ALIGN =\"center\">" + Server.HtmlEncode(ComAmec.fechacreacion.ToString()) + "</td>");
                        Response.Write("<td colspan=\"6\" bgcolor=\"#B7E43A\" ALIGN =\"center\">" + Server.HtmlEncode(ComAmec.comentariosdoc) + "</td></tr>");
                    }
                }


                ICollection<DHistEstadosAMEC> ListHistAmec = agAmecInfo.ObtenerHistorialEstadosAMEC(dAmecInfo.idamecs.ToString(), "", 0, 0);
                Response.Write("<tr height=\"50\"><td colspan=\"12\"></td></tr>");
                Response.Write("<tr> <td  ALIGN =\"center\" colspan=\"11\"><FONT FACE=\"Times New Roman\" SIZE=\"4\"><b>" + Server.HtmlEncode("FLUJO DE APROBACICN") + "</b></FONT></td></tr>");
                Response.Write("<tr> <td colspan=\"3\" ALIGN =\"center\"  bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Accion </b></FONT></td>");
                Response.Write("<td colspan=\"2\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Usuario </b></FONT></td>");
                Response.Write("<td colspan=\"2\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Cargo </b></FONT></td>");
                Response.Write("<td colspan=\"1\" style=\"width: 200px;\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Fecha </b></FONT></td>");
                Response.Write("<td colspan=\"2\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Estado </b></FONT></td>");
                Response.Write("<td colspan=\"1\" style=\"width: 200px;\" ALIGN =\"center\" bgcolor=\"#D1D1D1\"><FONT FACE=\"Times New Roman\" SIZE=\"2\"><b> Última accion </b></FONT></td></tr>");
                if (ListHistAmec != null && ListHistAmec.Count > 0)
                {
                    foreach (DHistEstadosAMEC HistAmec in ListHistAmec)
                    {
                        Response.Write("<tr><td colspan=\"3\" bgcolor=\"#B7E43A\" ALIGN =\"center\">" + Server.HtmlEncode(HistAmec.nivelaprobacion) + "</td>");
                        Response.Write("<td colspan=\"2\" bgcolor=\"#DDF5E3\" ALIGN =\"center\">" + Server.HtmlEncode(HistAmec.nombreusuario) + "</td>");
                        Response.Write("<td colspan=\"2\" bgcolor=\"#B7E43A\" ALIGN =\"center\">" + Server.HtmlEncode(HistAmec.cargo) + "</td>");
                        Response.Write("<td colspan=\"1\"  style=\"width: 200px;\" bgcolor =\"#DDF5E3\" ALIGN =\"center\">" + Server.HtmlEncode(HistAmec.fechacreacion.ToString()) + "</td>");
                        Response.Write("<td colspan=\"2\" bgcolor=\"#B7E43A\" ALIGN =\"center\">" + Server.HtmlEncode(HistAmec.estado) + "</td>");
                        Response.Write("<td colspan=\"1\" style=\"width: 200px;\" bgcolor=\"#B7E43A\" ALIGN =\"center\">" + Server.HtmlEncode(HistAmec.nombreusuariopendiente) + "</td>");

                    }
                }

                Response.Write("</TABLE>");
                Response.Write("</body></html>");
            }
        }
    }
}
