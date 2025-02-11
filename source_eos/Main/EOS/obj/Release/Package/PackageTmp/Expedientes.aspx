<%@ Page Title="Listado de Expedientes" Language="C#" MasterPageFile="~/Styles/EOS.Master"
    AutoEventWireup="true" CodeBehind="Expedientes.aspx.cs" Inherits="EOS.Expedientes"
    EnableEventValidation="false" ValidateRequest="false" Culture="es-ES" UICulture="es" %>
<%@ Import Namespace="System.ComponentModel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script type='text/javascript'>

        function ToggleAdvancedInfo(celda) {

            var filaOculta = celda.parentNode.parentNode.nextSibling;
            if (filaOculta.innerHTML == null) // Esto es porque en firefox encuentra otro elemento antes que la siguiente TR
                filaOculta = filaOculta.nextSibling;

            var sActual = celda.innerHTML;

            if (sActual == '+') {
                filaOculta.className = 'datoVisible';
                celda.innerHTML = '-';
            } else {
                filaOculta.className = 'datoOculto';
                celda.innerHTML = '+';
            }
        }

        function ToggleAdvancedFilter() {

            //var tabla = document.getElementById("eosFiltroAvanzado");
            var tabla = document.getElementById('<%= eosFiltroAvanzado.ClientID %>');

            if (tabla.style.display == "none") {
                mostrarFiltroAvanzado(true);
            }
            else {
                mostrarFiltroAvanzado(false);
            }

        }

        function mostrarFiltroAvanzado(blnMostrar) {

            var tabla = document.getElementById('<%= eosFiltroAvanzado.ClientID %>');
            var valor = document.getElementById('<%= txtFiltroAvanzadoEstado.ClientID %>');

            if (blnMostrar) {

                tabla.style.display = "inline";
                valor.value = "1";


            }
            else {

                tabla.style.display = "none";
                valor.value = "0";

            }

        }

        function ConfirmaCopia(idExpediente) {
            return confirm('¿Desea copiar el expediente ' + idExpediente + '?');
        }

        function AbreCarpeta(expediente) {
            window.open('Informes/' + expediente);
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="eosContentStatus" runat="server">
    <div id="eosContentStatus">
        <asp:Label ID="lblSinEnviar" runat="server" Text="" CssClass="eosCSta"></asp:Label>
        <asp:Label ID="lblEnviado" runat="server" Text="" CssClass="eosCSta"></asp:Label>
        <asp:Label ID="lblCotizando" runat="server" Text="" CssClass="eosCSta"></asp:Label>
        <asp:Label ID="lblCotizado" runat="server" Text="" CssClass="eosCSta"></asp:Label>
        <asp:Label ID="lblAceptado" runat="server" Text="" CssClass="eosCSta"></asp:Label>
        <asp:Label ID="lblTramitando" runat="server" Text="" CssClass="eosCSta"></asp:Label>
        <asp:Label ID="lblTramitado" runat="server" Text="" CssClass="eosCSta"></asp:Label>
        <asp:Label ID="lblRechazado" runat="server" Text="" CssClass="eosCSta"></asp:Label>
        <asp:Label ID="lblCancelado" runat="server" Text="" CssClass="eosCSta"></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="eosContentFilter" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
    <div id="eosContentStatusTitulo" class="eosTituloWizard" runat="server" visible="false">
        Seleccione el Expediente a copiar
    </div>
    <div id="eosContentFilter">
        <h1>Listado de Expedientes</h1>
        
        <div class="eosTablaFiltros" id="eosFiltroBasico">

            <h2>Filtro de expediente</h2>
            <p>Rellene los campos necesarios y pulse el botón FILTRAR</p> 


            <div class="contentFormGeneric">
                <div class="contentForm eosMitadColumna">
                    <label class="eosCampoLabel" for="txtActividad">Filtro Nombre Actividad</label>
                    <!--<input type="text" onkeypress="changeOnKeyPress(this);" onmousedown="this.focus();" onblur="changeOnBlur(this, 'Nombre');" onfocus="changeOnFocus(this, 'Nombre');" id="xxxTextNombre" name="xxxNombre" maxlength="80" class="eosInputVacio eosSizeW120" value="Nombre" gtbfieldid="1">-->
                    <asp:TextBox ID="txtActividad" runat="server" autocomplete="off" MaxLength="80" CssClass="eosInputVacio eosSizeW190"></asp:TextBox>
                </div>
                <div class="contentForm eosMitadColumna">
                    <label class="eosCampoLabel" for="xxxFiltroAsistente">Filtro Asistente</label>
                    <asp:TextBox ID="txtAsistente" runat="server" autocomplete="off" MaxLength="80" CssClass="eosInputVacio eosSizeW170"></asp:TextBox>
                </div>


                <div>
                    <asp:Label runat="server" CssClass="eosCampoLabel" for="xxxFiltroEstadoExpediente"
                    Visible="false">&nbsp;&nbsp;Filtro Estado Expediente</asp:Label>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="eosInputVacio eosSizeW190"
                        Visible="false">
                        <asp:ListItem Value="">(Todos)</asp:ListItem>
                        <asp:ListItem Value="AB">Sin Enviar</asp:ListItem>
                        <asp:ListItem Value="NC">En Curso</asp:ListItem>
                        <asp:ListItem Value="FZ">Finalizado</asp:ListItem>
                        <asp:ListItem Value="AN,CN">Cancelado</asp:ListItem>
                    </asp:DropDownList>
                </div>

                       </div>
                        <div class="contentFormGeneric">
                <div class="contentForm eosCuartoColumna">
                    <label class="eosCampoLabel" for="xxxFiltroAMEC">
                    Filtro <%= System.Configuration.ConfigurationManager.AppSettings["codigoPresupuesto"].ToString() %> (Número EM)</label>
                    <asp:TextBox ID="txtAMEC" runat="server" autocomplete="off" MaxLength="80" CssClass="eosInputVacio eosSizeW135"></asp:TextBox>
                </div>
                <div class="contentForm eosCuartoColumna">
                    <label class="eosCampoLabel" for="xxxFiltroAMEC">
                        Filtro Estado Petición
                    </label>
                    <asp:DropDownList ID="ddlEstadoReserva" runat="server" CssClass="eosInputVacio eosSizeW120">
                        <%--Ismael Ameller 08-03-2011 Carga el combo de estado reserva de BBDD--%>
                        <%--                  <asp:ListItem Value="">(Todos)</asp:ListItem>
                                <asp:ListItem Value="AB">Sin Enviar</asp:ListItem>
                                <asp:ListItem Value="CR">Enviado</asp:ListItem>
                                <asp:ListItem Value="CTZD">Cotizando</asp:ListItem>
                                <asp:ListItem Value="CFP">Aceptado</asp:ListItem>
                                <asp:ListItem Value="PTR">Tramitando</asp:ListItem>
                                <asp:ListItem Value="TR">Tramitado</asp:ListItem>
                                <asp:ListItem Value="CN">Cancelado</asp:ListItem>
                                <asp:ListItem Value="AN">Rechazado</asp:ListItem>--%>
                    </asp:DropDownList>
                </div>
                <div class="contentForm eosCuartoColumna">
                    <label class="eosCampoLabel" for="xxxFiltroAMEC">
                    Tipo</label>
                    <asp:DropDownList ID="ddlTipo" runat="server" CssClass="eosInputVacio eosSizeW135">
                        <asp:ListItem Value="0">(Todos)</asp:ListItem>
                        <asp:ListItem Value="1">Individual</asp:ListItem>
                        <asp:ListItem Value="2">Colectivo</asp:ListItem>
                        <asp:ListItem Value="3">Calculadora</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div>
                     <asp:Label runat="server" CssClass="eosCampoLabel" for="xxxFiltroAMEC" Visible="false">Año</asp:Label>
                    <asp:DropDownList ID="ddlAnyo" runat="server" CssClass="eosInputVacio eosSizeW50"
                        Visible="false">
                        <asp:ListItem Value="0">(Todos)</asp:ListItem>
                        <asp:ListItem Value="2011">2011</asp:ListItem>
                        <asp:ListItem Value="2010">2010</asp:ListItem>
                        <asp:ListItem Value="2009">2009</asp:ListItem>
                        <asp:ListItem Value="2008">2008</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div>
                    <asp:Label runat="server" CssClass="eosCampoLabel" for="xxxFiltroAMEC" Visible="false">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Mes</asp:Label>
                    <asp:DropDownList ID="ddlMes" runat="server" CssClass="eosInputVacio eosSizeW90"
                        Visible="false">
                        <asp:ListItem Value="0">(Todos)</asp:ListItem>
                        <asp:ListItem Value="1">Enero</asp:ListItem>
                        <asp:ListItem Value="2">Febrero</asp:ListItem>
                        <asp:ListItem Value="3">Marzo</asp:ListItem>
                        <asp:ListItem Value="4">Abril</asp:ListItem>
                        <asp:ListItem Value="5">Mayo</asp:ListItem>
                        <asp:ListItem Value="6">Junio</asp:ListItem>
                        <asp:ListItem Value="7">Julio</asp:ListItem>
                        <asp:ListItem Value="8">Agosto</asp:ListItem>
                        <asp:ListItem Value="9">Septiembre</asp:ListItem>
                        <asp:ListItem Value="10">Octubre</asp:ListItem>
                        <asp:ListItem Value="11">Noviembre</asp:ListItem>
                        <asp:ListItem Value="12">Diciembre</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="eosCuartoColumna">
                    <asp:Label ID="lblExpediente" runat="server" Text="Nº Expediente" CssClass="eosCampoLabel"></asp:Label>
                    <asp:TextBox ID="txtNumExpediente" runat="server" MaxLength="10" CssClass="eosInputVacio eosSizeW190"></asp:TextBox>
                    <asp:RangeValidator ID="rvNumExpediente" runat="server" ErrorMessage="*" ControlToValidate="txtNumExpediente"
                        MinimumValue="0" MaximumValue="9999999999"></asp:RangeValidator>
                </div>
             </div>
        </div>
        <div id="eosFiltroAvanzado" runat="server" style="display: none;" class="contentFiltroAvanzado">
            <input id="txtFiltroAvanzadoEstado" type="hidden" runat="server" value="0" />
            <div class="eosTablaFiltros">
           
                  <h2>Filtro avanzado</h2>
            
                <p>Fechas de creación</p>
          
                
                <div class="contentFormGeneric">

                <div class="eosCuartoColumna">
                    <label class="eosCampoLabel" for="">
                        Desde</label>
                    <asp:TextBox ID="txtFechaDesde" runat="server" autocomplete="off" MaxLength="80"
                        CssClass="eosInputVacio eosSizeW90"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Styles/images/iconCalculadora.png"  CssClass="iconCalendar"/>

                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFechaDesde" runat="server"
                        TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday" PopupButtonID="ImageButton1" />
                                        </div>
                                    <div class="eosCuartoColumna">

                                             <label class="eosCampoLabel" for="">
                        Hasta</label>
                    <asp:TextBox ID="txtFechaHasta" runat="server" autocomplete="off" MaxLength="80"
                        CssClass="eosInputVacio eosSizeW90"></asp:TextBox>
                                       <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Styles/images/iconCalculadora.png" CssClass="iconCalendar"/>
                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtFechaHasta" runat="server"
                        TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday" PopupButtonID="ImageButton3" />

                                                            </div>
                                    <div class="eosMitadColumna">

                                         <label class="eosCampoLabel" for="">
                        Peticionario</label>
                    <asp:TextBox ID="txtPeticionario" runat="server" autocomplete="off" MaxLength="80"
                        CssClass="eosInputVacio eosSizeW170"></asp:TextBox>

        </div>                
                </div>


                <div class="contentFormGeneric">
                
                <div class="eosCuartoColumna">
               
                    <label class="eosCampoLabel" for="">
                        Área Negocio</label>
                    <asp:TextBox ID="txtNegocio" runat="server" autocomplete="off" MaxLength="80" CssClass="eosInputVacio eosSizeW170"></asp:TextBox>     




                                        </div>
                                    <div class="eosCuartoColumna">
                    <asp:Label ID="Label1" runat="server" CssClass="eosCampoLabel" for="" Text="Producto"></asp:Label>
                    <asp:DropDownList ID="ddltiposDeProductos" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW120"
                        DataTextField="Nombre" DataValueField="IdTipoProducto">
                    </asp:DropDownList>
                                                                                </div>
                                    <div class="eosCuartoColumna">
                    <label class="eosCampoLabel" for="">
                        Región</label>
                    <asp:TextBox ID="txtRegion" runat="server" autocomplete="off" MaxLength="80" CssClass="eosInputVacio eosSizeW170"></asp:TextBox>        </div>

                               <div class="eosCuartoColumna">
              <label class="eosCampoLabel" for="">
                        Departamento:</label>
                    <asp:DropDownList ID="ddlDepartament" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartament_OnSelectedIndexChanged" CssClass="eosInputVacio eosSizeW50"></asp:DropDownList>
                 
 </div>

                </div>
                   <div class="contentFormGeneric">

                <div class="eosCuartoColumna">
                  <label class="eosCampoLabel" for="">
                        Proveedor servicio</label>
                    <asp:TextBox ID="txtProveedorServicio" runat="server" autocomplete="off" MaxLength="80"
                        CssClass="eosInputVacio eosSizeW170"></asp:TextBox>
                    </div>

                                    <div class="eosMitadColumna">
                    <label class="eosCampoLabel" for="">
                        Distrito (Antes de 01/01/2017)</label>
                    <asp:TextBox ID="txtDistrito" runat="server" autocomplete="off" MaxLength="80" CssClass="eosInputVacio eosSizeW170"></asp:TextBox>

                  </div>

                                    <div class="eosCuartoColumna">

 <label class="eosCampoLabel" for="">
                        Fuerda de venta:</label>
                    <asp:DropDownList ID="ddlSaleForce" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSaleForce_OnSelectedIndexChanged" CssClass="eosInputVacio eosSizeW50"></asp:DropDownList>
                  
          </div>

                </div>


                <div class="contentFormGeneric">
                              <div class="eosCuartoColumna contentImporte">


                     <label class="eosCampoLabel" for="">
                        Importe</label>
                    <asp:DropDownList ID="ddlTipoImporte" runat="server" CssClass="eosInputVacio eosSizeW50">
                        <asp:ListItem Value="0">(Todos)</asp:ListItem>
                        <asp:ListItem Value="1"><</asp:ListItem>
                        <asp:ListItem Value="2"><=</asp:ListItem>
                        <asp:ListItem Value="3">=</asp:ListItem>
                        <asp:ListItem Value="4">>=</asp:ListItem>
                        <asp:ListItem Value="5">></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtImporte" runat="server" autocomplete="off" MaxLength="80" CssClass="eosInputVacio eosSizeW115"></asp:TextBox>

          </div><div class="eosCuartoColumna">

                    <label class="eosCampoLabel" for="">
                        Unidad</label>
                    <asp:TextBox ID="txtUnidad" runat="server" autocomplete="off" MaxLength="80" CssClass="eosInputVacio eosSizeW120"></asp:TextBox>

                     </div>


                                <div class="eosMitadColumna">

                    <label class="eosCampoLabel" for="">
                        Distrito:</label>
                    <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="eosInputVacio eosSizeW50"></asp:DropDownList>   </div>


                </div>
            </div>
        </div>
        <div class="eosTablaFiltros" id="eosFiltroBotones">
            <div class="eosBotonera eosBotonFiltradoAvanzado">
                <a title="Mostrar más opciones de Filtrado" href="javascript:;" onclick="javascript:ToggleAdvancedFilter();return false;"
                    onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Mostrar más opciones de Filtrado';return true">Filtro Avanzado </a>
            </div>
            <div class="eosBotonera eosBotonFiltrado">
                <asp:Button ID="ImageButton2" runat="server" OnClick="ImageButton2_Click" Text="Filtrar"/>

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="eosContentResults" runat="server">
    <div id="eosContentResults" class="contentTableGeneric">


        <asp:ListView ID="lvExpedientes" runat="server" DataSourceID="odsExpediente"
            OnSorting="lvExpedientes_Sorting" DataKeyNames="IDEXPEDIENTE" EnablePersistedSelection="True" OnLoad="lvExpedientes_Load"
            OnSelectedIndexChanged="lvExpedientes_SelectedIndexChanged">
            <%-- ESTOS PARAMETROS ESTABAN DENTRO DEL TAG LISTVIEW Y SE QUITARON PARA QUE NO CARGUE EL LISTVIEW AL CARGAR LA
                 PAGINA, SINO CUANDO LE DES A FILTRAR --%>
            <%--DataSourceID="odsExpediente"
            OnSorting="lvExpedientes_Sorting" DataKeyNames="IDEXPEDIENTE" EnablePersistedSelection="True"
            OnSelectedIndexChanged="lvExpedientes_SelectedIndexChanged"--%>
            <EmptyDataTemplate>
                <table class="eosTablaResultados">
                    <thead>
                        <tr>
                            <th></th>
                            <th>Nº Exp
                            </th>
                            <th>amec
                            </th>
                            <th>fecha
                            </th>
                            <th width="40%">Evento
                            </th>
                            <th></th>
                            <th colspan="2">Tipo
                            </th>
                            <th>Total
                            </th>
                            <th>!
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="10">
                                <asp:PlaceHolder runat="server" ID="plcNoexp">
                                    <span class="eosTituloRojo">No se han encontrado expedientes que cumplan los criterios
                                    de búsqueda seleccionados</span>
                                </asp:PlaceHolder>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table class="eosTablaResultados">
                    <thead>
                        <tr>
                            <th width="7%" style="text-align: left;"></th>
                            <th style="text-align: left;">
                                <asp:LinkButton ID="lbNExp" runat="server" Text="Nº Exp" CommandName="Sort" CommandArgument="IDEXPEDIENTE"></asp:LinkButton>
                            </th>
                            <th style="text-align: left;">
                                <asp:LinkButton ID="lbAmec" runat="server" CommandName="Sort" CommandArgument="AMEC"><%= System.Configuration.ConfigurationManager.AppSettings["codigoPresupuesto"].ToString() %></asp:LinkButton>
                            </th>
                            <th style="text-align: left;">
                                <asp:LinkButton ID="lbFecha" runat="server" Text="Fecha Exp" CommandName="Sort"
                                    CommandArgument="FECHACREACION"></asp:LinkButton>
                            </th>
                            <th width="40%" style="text-align: left;">
                                <asp:LinkButton ID="lbActividad" runat="server" Text="Evento" CommandName="Sort"
                                    CommandArgument="ACTIVIDAD"></asp:LinkButton>
                            </th>
                            <th style="text-align: left;"></th>
                            <th colspan="2" style="text-align: left;">Tipo
                            </th>
                            <th style="text-align: left;">
                                <asp:LinkButton ID="lbTotal" runat="server" Text="Total" CommandName="Sort" CommandArgument="IMPORTE"></asp:LinkButton>
                            </th>
                            <th style="text-align: left;">!
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody>
    
                </table>
                <div class="contentPagination">
                                <asp:DataPager ID="DataPager3" runat="server" PageSize="10">
                                    <Fields>
                                        <asp:NextPreviousPagerField ButtonType="Image" ShowFirstPageButton="True" ShowNextPageButton="False"
                                            ShowPreviousPageButton="True" FirstPageText=" " PreviousPageText=" " PreviousPageImageUrl="Styles/images/ic_pagina_anterior.png"
                                            FirstPageImageUrl="Styles/images/ic_pagina_primera.png" RenderDisabledButtonsAsLabels="True" />
                                        <asp:NumericPagerField CurrentPageLabelCssClass="pageSel" />
                                        <asp:NextPreviousPagerField ButtonType="Image" ShowLastPageButton="True" ShowNextPageButton="True"
                                            ShowPreviousPageButton="False" LastPageText=" " NextPageText=" " LastPageImageUrl="Styles/images/ic_pagina_ultima.png"
                                            NextPageImageUrl="Styles/images/ic_pagina_siguiente.png" RenderDisabledButtonsAsLabels="True" />
                                    </Fields>
                                </asp:DataPager>
                    </div>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td class="table-icon-container">
                        <%if (!m_bSelect)
                            {%>
                        <asp:ImageButton ID="imgExpedienteDetalle" runat="server" CommandArgument='<%# Eval("idexpediente") %>'
                            ToolTip='<%# String.Format("Ver detalles del expediente {0}", Eval("idexpediente")) %>'
                            ImageUrl="~/Styles/images/icolupa.png" PostBackUrl='<%# String.Format("DetalleExpediente.aspx?idexp={0}", Eval("idexpediente")) %>' />
                        <%--                            <asp:ImageButton ID="imgExpedienteInformes" runat="server" ToolTip='<%# String.Format("Ver documentación del expediente {0}", Eval("idexpediente")) %>' ImageUrl="~/Styles/images/ic_listado.png" OnClientClick='<%# string.Format("javascript:AbreCarpeta({0});", Eval("idexpediente"))%>' />
                        --%>
                        <asp:ImageButton ID="imgExpedienteInformes" runat="server" ToolTip='<%# String.Format("Ver documentación del expediente {0}", Eval("idexpediente")) %>'
                            ImageUrl="~/Styles/images/ic_listado.png" CommandName='<%# Eval("idexpediente") %>'
                            OnCommand="imgExpedienteInformes_Command" />
                        <%}
                            else
                            { %>
                        <asp:ImageButton ID="imgActividadDetalle" runat="server" CommandName="Select" ToolTip='<%# String.Format("Seleccionar el Expediente {0} para copiar", Eval("idexpediente")) %>'
                            ImageUrl="~/Styles/images/ic_amec.png" OnClientClick='<%# String.Format("return ConfirmaCopia({0});", Eval("idexpediente")) %>' />
                        <%} %>
                    </td>
                    <td class="par">
                        <asp:Label ID="idexpedienteLabel" runat="server" Text='<%# Eval("idexpediente") %>' />
                    </td>
                    <td>
                        <asp:Label ID="lblAmec" runat="server" Text='<%# Eval("amec") %>' />
                    </td>
                    <td class="par">
                        <asp:Label ID="fechacreacionLabel" runat="server" Text='<%# String.Format("{0:dd/MM/yy}",Eval("fechacreacion")) %>' />
                    </td>
                    <td>
                        <asp:Label ID="expedienteLabel" runat="server" Text='<%# Eval("actividad") %>' />
                        <a href="javascript:;" class="contentMoreInfo" title="Ampliar Información del expediente <%# Eval("idexpediente") %>"
                            onclick="javascript:ToggleAdvancedInfo(this);return false;" onmouseout="javascript:window.status='';return true;"
                            onmouseover="javascript:window.status='Ampliar Información del expediente <%# Eval("idexpediente") %>';return true">+</a>
                    </td>
                    <td>
                        
                    </td>
                    <td class="par">
                        <span class='eosValoracionFinanciera <%# String.Format("eosValoracionFin{0}", Eval("idvaloracionfi")!=null ? Eval("idvaloracionfi") : 6) %>'>&nbsp;</span>
                    </td>
                    <td class="par">
                        <!--Ismael Ameller 24/02/2011 aparición del tooltip en la imagen de tipo de reserva-->
                        <asp:Image ID="imgTipoReserva" runat="server" ToolTip='<%# String.Format("Expediente {0}",Eval("idtiporeserva")!=null? ((int)Eval("idtiporeserva")==1 ? (int?)Eval("TipoPagoFee") == 3 ? "calculadora" : "individual" : "colectivo") : "colectivo") %>'
                            ImageUrl='<%# String.Format("~/Styles/images/{0}", Eval("idtiporeserva")!=null ? ((int)Eval("idtiporeserva") == 1 ? (int?)Eval("TipoPagoFee") == 3 ? "ic_reu_calculadora.png" : "ic_reu_individual.png" : "ic_reu_colectivo.png") : "ic_reu_colectivo.png") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="pvpLabel" runat="server" Text='<%# ((double)Eval("importe")).ToString("N2") + " €" %>' />
                    </td>
                    <td class="par">
                        <asp:Image ID="imgAviso" runat="server" CssClass="imagenTable" ImageUrl='<%# String.Format("~/Styles/images/{0}", Eval("urgente")!=null ? ((bool)Eval("urgente") ? "ic_aviso_naranja.png" : "ic_aviso_gris.png") : "ic_aviso_gris.png") %>' />
                    </td>
                </tr>
                <tr class="datoOculto">
                    <td colspan="10">
                        <%# Eval("amec").ToString().ToCharArray()[0] != '4' ?
                            String.Format("<div class=\"eosExpedienteDetalleAmpliado\"><span class=\"eosExpedienteDetalleAmpliadoLabel\">Unidad:</span> {0} &nbsp;&nbsp;|<span class=\"eosExpedienteDetalleAmpliadoLabel\">Región</span>: {1} &nbsp;&nbsp;|<span class=\"eosExpedienteDetalleAmpliadoLabel\">Distrito:</span> {2} &nbsp;&nbsp;|<span class=\"eosExpedienteDetalleAmpliadoLabel\">Área:</span> {3} &nbsp;&nbsp;|<br/><span class=\"eosExpedienteDetalleAmpliadoLabel\">Peticionario:</span> {4} &nbsp;&nbsp;| <span class=\"eosExpedienteDetalleAmpliadoLabel\">Cargo del Peticionario:</span> {5}</div>", Eval("unidad"), Eval("region"), Eval("distrito"), Eval("area"), Eval("Peticionario"), Eval("cargo")) :
                            String.Format("<div class=\"eosExpedienteDetalleAmpliado\"><span class=\"eosExpedienteDetalleAmpliadoLabel\">Departamento:</span> {0} &nbsp;&nbsp;|<span class=\"eosExpedienteDetalleAmpliadoLabel\">Fuerza de Venta</span>: {1} &nbsp;&nbsp;|<span class=\"eosExpedienteDetalleAmpliadoLabel\">Distrito:</span> {2} &nbsp;&nbsp;|<br/><span class=\"eosExpedienteDetalleAmpliadoLabel\">Peticionario:</span> {3} &nbsp;&nbsp;| <span class=\"eosExpedienteDetalleAmpliadoLabel\">Cargo del Peticionario:</span> {4}</div>", Eval("departament") ?? "Sin Departamento", Eval("saleforce") ?? "Sin Fuerza de Venta", Eval("district") ?? "Sin Distrito", Eval("Peticionario"), Eval("cargo"))
                        %>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsExpediente" runat="server" SelectMethod="ObtenerExpedientesTotal"
            TypeName="EOS.Web.AgenteExpedientes" SelectCountMethod="ObtenerNumeroExpedientes"
            EnablePaging="True" OnSelecting="odsExpediente_Selecting" SortParameterName="sortParameter">
            <SelectParameters>
                <asp:Parameter Name="filtroActividad" Type="String" />
                <asp:Parameter Name="filtroAsistente" Type="String" />
                <asp:Parameter Name="filtroEstadoExpediente" Type="String" />
                <asp:Parameter Name="filtroAmec" Type="String" />
                <asp:Parameter Name="filtroEstadoReserva" Type="String" />
                <asp:Parameter Name="filtroTipo" Type="String" />
                <asp:Parameter Name="filtroAnyo" Type="String" />
                <asp:Parameter Name="filtroMes" Type="String" />
                <asp:Parameter Name="filtroFechaDesde" Type="String" />
                <asp:Parameter Name="filtroFechaHasta" Type="String" />
                <asp:Parameter Name="filtroUnidad" Type="String" />
                <asp:Parameter Name="filtroProducto" Type="String" />
                <asp:Parameter Name="filtroAreaNegocio" Type="String" />
                <asp:Parameter Name="filtroRegion" Type="String" />
                <asp:Parameter Name="filtroDistrito" Type="String" />
                <asp:Parameter Name="filtroPeticionario" Type="String" />
                <asp:Parameter Name="filtroProveedor" Type="String" />
                <asp:Parameter Name="filtroImporte" Type="String" />
                <asp:Parameter Name="filtroTipoImporte" Type="String" />
                <asp:Parameter Name="filtroNumExpediente" Type="String" />
                <asp:Parameter Name="filtroIdDistrict" Type="String" />
                <asp:Parameter Name="filtroIdDepartament" Type="String" />
                <asp:Parameter Name="filtroIdSaleForce" Type="String" />
                <asp:Parameter Name="filtroIdPeticionarioSession" Type="Int32" />
                <asp:Parameter Name="filtroTipoActividad" Type="String" />
                <asp:Parameter Name="disableBinding" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    <p />
    <div class="eosDivCentrada">
    </div>
    <%if (blnMostrarFiltroAvanzado)
        {%>
    <script language="javascript" type="text/javascript">

        mostrarFiltroAvanzado(true);

    
                        </script>
    <% }%>
</asp:Content>
<asp:Content ID="cphScripts" ContentPlaceHolderID="ContentPlaceHolderScripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("ul#eosHPBotones li").removeClass("active");
            $("ul#eosHPBotones li#liListadoExpedientes").addClass("active");
        });
    </script>
</asp:Content>