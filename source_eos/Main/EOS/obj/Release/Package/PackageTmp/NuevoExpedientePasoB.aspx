<%@ Page Title="" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true"
    CodeBehind="NuevoExpedientePasoB.aspx.cs" Inherits="EOS.NuevoExpedientePasoB" %>

<asp:Content ID="Content5" ContentPlaceHolderID="eosContentFilter" runat="server">
    <script type="text/javascript" src="Scripts/nuevoExpedientePasoB.js">
       
    </script>
    <script type="text/javascript">
        ValidatorEnable('<%=rfvddlTipoActividadPax.ClientID %>', false);
        $(document).ready(function () {
            //           $("[id$=idimgparticipantes]").click(function (event) {
            //               event.preventDefault();
            //               $("#idModuloParticipantes").show(3000);
            //           });
            //           ValidatorEnable('<%= rfvddlTipoActividadPax.ClientID %>', false);
        });
    </script>



    <asp:Panel ID="plTituloInd" runat="server">
        <h1>
            Expediente Individual
        </h1>
    </asp:Panel>


    <h2> Listado de asistentes</h2>


    <p></p>  <p></p>


    <asp:Panel ID="plPaso3_Ind" runat="server" DefaultButton="btnFiltrar">
        <div id="Div1" class="contentTableGeneric">


            <asp:ListView ID="lvServicioParticipantes" runat="server" DataSourceID="odsParticipantesExpediente"
                DataKeyNames="IdPassengerlist" EnablePersistedSelection="True" Visible="true"
                OnSelectedIndexChanged="lvServicioParticipantes_SelectedIndexChanged">
                <EmptyDataTemplate>
                    <table class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th>
                                    Nombre Completo
                                </th>
                                <th>
                                    Tipo Asistente
                                </th>
                                <th width="20%">
                                    Tipo Actividad Pax
                                </th>
                                <th>
                                    Nivel de riesgo HCP en APPIAN
                                </th>
                                <th width="10%">
                                    <%= ConfigurationManager.AppSettings["codigoInvitado"] %>
                                </th>
                                <th width="20%">
                                    Honorarios
                                </th>
                                <th width="4%">
                                </th>
                                <th width="4%">
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="8">
                                    <span class="eosTituloRojo">&nbsp;</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </EmptyDataTemplate>
                <LayoutTemplate>
                     <table class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th>
                                    <asp:LinkButton ID="lbNombre" runat="server" Text="Nombre Completo" CommandName="Sort"
                                        CommandArgument="NOMBRE"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Tipo Asistente" CommandName="Sort"
                                        CommandArgument="TIPOASISTENTE"></asp:LinkButton>
                                </th>
                                <th width="30%">
                                    <asp:LinkButton ID="lbHospital" runat="server" Text="Tipo Actividad" CommandName="Sort"
                                        CommandArgument="TIPOACTIVIDADPAX"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lbNivelRiesgo" runat="server" Text="Nivel de Riesgo" CommandName="Sort"
                                        CommandArgument="NIVELRIESGO"></asp:LinkButton>
                                </th>
                                <th width="20%">
                                    <asp:LinkButton ID="lbMSDID" runat="server" CommandName="Sort" CommandArgument="MSDID"><%= ConfigurationManager.AppSettings["codigoInvitado"]%></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lbHonorarios" runat="server" Text="Honorarios" CommandName="Sort"
                                        CommandArgument="HONORARIOS"></asp:LinkButton>
                                </th>
                                <th width="4%">
                                </th>
                                <th width="4%">
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
                                                ShowPreviousPageButton="True" FirstPageText="" PreviousPageText="" PreviousPageImageUrl="Styles/images/ic_pagina_anterior.png"
                                                FirstPageImageUrl="Styles/images/ic_pagina_primera.png" RenderDisabledButtonsAsLabels="True" />
                                            <asp:NumericPagerField CurrentPageLabelCssClass="pageSel" />
                                            <asp:NextPreviousPagerField ButtonType="Image" ShowLastPageButton="True" ShowNextPageButton="True"
                                                ShowPreviousPageButton="False" LastPageText="" NextPageText="" LastPageImageUrl="Styles/images/ic_pagina_ultima.png"
                                                NextPageImageUrl="Styles/images/ic_pagina_siguiente.png" RenderDisabledButtonsAsLabels="True" />
                                        </Fields>
                                    </asp:DataPager>
                         </div>
           
             
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="par">
                            <asp:Label ID="nombreLabel" runat="server" Text='<%#  Eval("nombre")+ " " + Eval("apel1") %>' />
                        </td>
                        <td>
                            <asp:Label ID="tipoasistenteLabel" runat="server" Text='<%# Eval("tipoasistente") %>' />
                        </td>
                        <td class="par">
                            <asp:Label ID="tipoactividadpaxLabel" runat="server" Text='<%# Eval("tipoactividadpax") %>' />
                        </td>
                        <td>
                            <asp:Label ID="nivelriesgolabel" runat="server" Text='<%# Eval("nivelriesgo") %>' />
                        </td>
                        <td class="par">
                            <asp:Label ID="msdidLabel" runat="server" Text='<%# Eval("msdid") %>' />
                        </td>
                        <td>
                            <asp:Label ID="honorariosLabel" runat="server" Text='<%# Eval("honorarios") %>' />&nbsp;€
                        </td>
                        <td class="par">
                            <img id="imgParticipanteView" src="/Styles/images/ic_lupa.png" style="cursor: pointer;" title='<%# String.Format("Ver detalle del Participante {0}", Eval("IdPassengerlist")) %>' alt='<%#Eval("IdPassengerlist") %>' />
                            <input type="hidden" id='hiddenParticipanteObj_<%#Eval("IdPassengerlist") %>' value='<%#GetJsonObject(Container.DataItem) %>'/>
                        </td>
                        <td>
                            <asp:ImageButton ID="imgParticipanteDetalle" runat="server" CommandName="Select"
                                ToolTip='<%# String.Format("Eliminar el Participante {0}", Eval("IdPassengerlist")) %>'
                                ImageUrl="~/Styles/images/ic_participante_aspa.png" AlternateText="Eliminar" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>




            <asp:ObjectDataSource ID="odsParticipantesExpediente" runat="server" SelectMethod="ObtenerParticipantesExpediente"
                TypeName="EOS.Web.AgenteParticipantes" SelectCountMethod="ObtenerNumeroParticipantesExpediente"
                EnablePaging="True" SortParameterName="sortParameter" OnSelecting="odsParticipantesExpediente_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="filtroIDExpediente" Type="String" DefaultValue="-1" />
                </SelectParameters>
            </asp:ObjectDataSource>



        </div>

                <div class="eosTablaFiltros" id="eosFiltroBasicoVeeva">
                    <h2>Buscador asistentes Veeva al evento</h2>
         <p> Defina los filtros del listado si es necesario y haga click en 'FILTRAR'</p>

                   
        <div class="contentFormGeneric">


                <div class="contentForm eosMitadColumna">
                          <label class="eosCampoLabel" for="txtNombre">
                            Nombre</label>
                     <asp:TextBox ID="txtNombreVeeva" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"></asp:TextBox>
                           </div>
                            <div class="contentForm eosMitadColumna">
                                       <label class="eosCampoLabel" for="txtCiudad">
                            Apellidos</label> <asp:TextBox ID="txtApel1Veeva" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"></asp:TextBox>
                           </div>
                <%-- <td>
                        <label class="eosCampoLabel" for="txtCiudad">
                            2º Apellido</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtApel2" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"></asp:TextBox>
                    </td>--%>
                    <%--<td>
                        <label class="eosCampoLabel" for="xxxFiltroAMEC">
                            Hospital</label>&nbsp;
                    </td>--%>
                    <%--<td>
                        <asp:TextBox ID="txtHospitalVeeva" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"></asp:TextBox>
                    </td>--%>
       </div>
<div class="eosBotonera eosBotonFiltrado btnInline">

                 <asp:Button ID="btnFiltrarVeeva" runat="server" Text="Filtrar" 
                            OnClick="btnFiltrarVeeva_Click" />
    
  </div>
  </div>
















        <div id="eosContentResultsVeeva" class="contentTableGeneric">



            <asp:ListView ID="lvParticipantesVeeva" runat="server" DataSourceID="odsParticipantesVeeva"
                DataKeyNames="IdPassengerlist" EnablePersistedSelection="True" Visible="false">
                <EmptyDataTemplate>
                     <table class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th>
                                    Nombre
                                </th>
                                <th>
                                    Apellido1
                                </th>
                                <th width="30%">
                                    Tipo
                                </th>
                                <th width="20%">
                                    Estado
                                </th>
                                <th width="4%">
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="6">
                                    <span class="eosTituloRojo">No se han encontrado participantes que cumplan los criterios
                                        de búsqueda seleccionados</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </EmptyDataTemplate>
                <LayoutTemplate>
                     <table class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lbNombre" runat="server" Text="Nombre" CommandName="Sort" CommandArgument="NOMBRE"></asp:LinkButton>
                                </th>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lbApel1" runat="server" Text="Apellidos" CommandName="Sort" CommandArgument="APEL1"></asp:LinkButton>
                                </th>
                                <th width="35%" style="text-align: left;">
                                    <asp:LinkButton ID="lbTipo" runat="server" Text="Tipo" CommandName="Sort"
                                        CommandArgument="AttendeeType"></asp:LinkButton>
                                </th>
                                <th width="10%" style="text-align: left;">
                                    <asp:LinkButton ID="lbEstado" runat="server" CommandName="Sort" CommandArgument="Status" Text="Estado"></asp:LinkButton>
                                </th>
                                <th width="4%">
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
                                                ShowPreviousPageButton="True" FirstPageText="" PreviousPageText="" PreviousPageImageUrl="Styles/images/ic_pagina_anterior.png"
                                                FirstPageImageUrl="Styles/images/ic_pagina_primera.png" RenderDisabledButtonsAsLabels="True" />
                                            <asp:NumericPagerField CurrentPageLabelCssClass="pageSel" />
                                            <asp:NextPreviousPagerField ButtonType="Image" ShowLastPageButton="True" ShowNextPageButton="True"
                                                ShowPreviousPageButton="False" LastPageText="" NextPageText="" LastPageImageUrl="Styles/images/ic_pagina_ultima.png"
                                                NextPageImageUrl="Styles/images/ic_pagina_siguiente.png" RenderDisabledButtonsAsLabels="True" />
                                        </Fields>
                                    </asp:DataPager>
                      
              </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Label ID="nombreLabel" runat="server" Text='<%# Eval("nombre") %>' />
                        </td>
                        <td class="par">
                            <asp:Label ID="apel1Label" runat="server" Text='<%# Eval("apel1")+ " " + Eval("apel2") %>' />
                        </td>
                        <td class="par">
                            <asp:Label ID="tipoLabel" runat="server" Text='<%# Eval("AttendeeType") %>' />
                        </td>
                        <td>
                            <asp:Label ID="estadoLabel" runat="server" Text='<%# Eval("Status") %>' />
                        </td>
                        <td class="par">                            
                            <asp:ImageButton ID="idimgparticipantesVeeva" runat="server" CommandName="Select" ToolTip='<%# String.Format("Añadir el Participante {0} y nivel de riesgo {1}", Eval("IdPassengerlist"),Eval("nivelriesgo")) %>'
                                ImageUrl="~/Styles/images/ic_participante_flecha.png" AlternateText="Añadir" style='<%#Eval("Status").ToString().Trim().ToLower() == "rejected" ? "display: none" : "display: block" %>'/>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>







            <asp:ObjectDataSource ID="odsParticipantesVeeva" runat="server" SelectMethod="ObtenerParticipantesVeeva"
                TypeName="EOS.Web.AgenteParticipantes" SelectCountMethod="ObtenerNumeroParticipantesVeeva"
                EnablePaging="True" SortParameterName="sortParameter" OnSelecting="odsParticipantesVeeva_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="filtroID" Type="String" DefaultValue="-1" />
                    <asp:Parameter Name="filtroNombre" Type="String" />
                    <asp:Parameter Name="filtroApel1" Type="String" />
                    <asp:Parameter Name="filtroApel2" Type="String" />
                    <asp:Parameter Name="filtroHospital" Type="String" />
                    <asp:Parameter Name="filtroMSDID" Type="String" />
                    <asp:Parameter Name="filtroIDDistrito" Type="String" />
                    <asp:Parameter Name="filtroIDRegion" Type="String" />
                    <asp:Parameter Name="filtroIDEmpresa" Type="String" />
                    <asp:Parameter Name="filtroIDExpediente" Type="String" />
                    <asp:Parameter Name="filtroIDAmecsString" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>


                      <div class="eosTablaFiltros" id="eosFiltroBasico">
                          <h2 class="marginTop">Buscador asistentes no cargados</h2>
         <p >Defina los filtros del listado si es necesario y haga click en 'FILTRAR'</p>

                   
        <div class="contentFormGeneric">


                <div class="contentForm eosTresColumna">
                       <label class="eosCampoLabel" for="txtNombre">
                            Nombre</label>
                     <asp:TextBox ID="txtNombre" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"></asp:TextBox>
                  </div>

                      <div class="contentForm eosTresColumna">
                            <label class="eosCampoLabel" for="txtCiudad">
                            Apellidos</label>
                          <asp:TextBox ID="txtApel1" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"></asp:TextBox>
  </div>
               
                                  <div class="contentForm eosTresColumna">
                                       <label class="eosCampoLabel" for="xxxFiltroAMEC">
                            Hospital</label>  <asp:TextBox ID="txtHospital" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"></asp:TextBox>
  </div>
               
                  <%-- <td>
                        <label class="eosCampoLabel" for="txtCiudad">
                            2º Apellido</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtApel2" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"></asp:TextBox>
                    </td>--%>


  

                  
       </div>
<div class="eosBotonera eosBotonFiltrado btnInline">

                        <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar"  
                            OnClick="btnFiltrar_Click" />
                    </div>
                                 </div>

        <div id="eosContentResults" class="contentTableGeneric">

            <asp:ListView ID="lvParticipantes" runat="server" DataSourceID="odsParticipantes"
                DataKeyNames="IdPassengerlist" EnablePersistedSelection="True" Visible="false">
                <EmptyDataTemplate>
                    <table class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th>
                                    Nombre
                                </th>
                                <th>
                                    Apellido1
                                </th>
                                <th width="30%">
                                    Hospital
                                </th>
                                <th width="20%">
                                    <%= ConfigurationManager.AppSettings["codigoInvitado"] %>
                                </th>
                                <th width="4%">
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="6">
                                    <span class="eosTituloRojo">No se han encontrado participantes que cumplan los criterios
                                        de búsqueda seleccionados</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lbNombre" runat="server" Text="Nombre" CommandName="Sort" CommandArgument="NOMBRE"></asp:LinkButton>
                                </th>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lbApel1" runat="server" Text="Apellidos" CommandName="Sort" CommandArgument="APEL1"></asp:LinkButton>
                                </th>
                                <th width="35%" style="text-align: left;">
                                    <asp:LinkButton ID="lbHospital" runat="server" Text="Hospital" CommandName="Sort"
                                        CommandArgument="HOSPITAL"></asp:LinkButton>
                                </th>
                                <th width="10%" style="text-align: left;">
                                    <asp:LinkButton ID="lbMSDID" runat="server" CommandName="Sort" CommandArgument="MSDID"><%= ConfigurationManager.AppSettings["codigoInvitado"] %></asp:LinkButton>
                                </th>
                                <th width="4%">
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
                                                ShowPreviousPageButton="True" FirstPageText="" PreviousPageText="" PreviousPageImageUrl="Styles/images/ic_pagina_anterior.png"
                                                FirstPageImageUrl="Styles/images/ic_pagina_primera.png" RenderDisabledButtonsAsLabels="True" />
                                            <asp:NumericPagerField CurrentPageLabelCssClass="pageSel" />
                                            <asp:NextPreviousPagerField ButtonType="Image" ShowLastPageButton="True" ShowNextPageButton="True"
                                                ShowPreviousPageButton="False" LastPageText="" NextPageText="" LastPageImageUrl="Styles/images/ic_pagina_ultima.png"
                                                NextPageImageUrl="Styles/images/ic_pagina_siguiente.png" RenderDisabledButtonsAsLabels="True" />
                                        </Fields>
                                    </asp:DataPager>
                      
                </div>
                </LayoutTemplate>
                <ItemTemplate>
                     <tr>
                        <td>
                            <asp:Label ID="nombreLabel" runat="server" Text='<%# Eval("nombre") %>' />
                        </td>
                        <td class="par">
                            <asp:Label ID="apel1Label" runat="server" Text='<%# Eval("apel1")+ " " + Eval("apel2") %>' />
                        </td>
                        <td class="par">
                            <asp:Label ID="hospitalLabel" runat="server" Text='<%# Eval("hospital") %>' />
                        </td>
                        <td>
                            <asp:Label ID="msdidLabel" runat="server" Text='<%# Eval("msdid") %>' />
                        </td>
                        <td class="par">
                            <asp:ImageButton ID="idimgparticipantes" runat="server" CommandName="Select" ToolTip='<%# String.Format("Añadir el Participante {0} y nivel de riesgo {1}", Eval("IdPassengerlist"),Eval("nivelriesgo")) %>'
                                ImageUrl="~/Styles/images/ic_participante_flecha.png" AlternateText="Añadir" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
            <asp:ObjectDataSource ID="odsParticipantes" runat="server" SelectMethod="ObtenerParticipantes"
                TypeName="EOS.Web.AgenteParticipantes" SelectCountMethod="ObtenerNumeroParticipantes"
                EnablePaging="True" SortParameterName="sortParameter" OnSelecting="odsParticipantes_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="filtroID" Type="String" DefaultValue="-1" />
                    <asp:Parameter Name="filtroNombre" Type="String" />
                    <asp:Parameter Name="filtroApel1" Type="String" />
                    <asp:Parameter Name="filtroApel2" Type="String" />
                    <asp:Parameter Name="filtroHospital" Type="String" />
                    <asp:Parameter Name="filtroMSDID" Type="String" />
                    <asp:Parameter Name="filtroIDDistrito" Type="String" />
                    <asp:Parameter Name="filtroIDRegion" Type="String" />
                    <asp:Parameter Name="filtroIDEmpresa" Type="String" />
                    <asp:Parameter Name="filtroIDExpediente" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </asp:Panel>
    <div id="idHonorariosMaximos" style="display: none; width: 800px; height: 780px;
        z-index: 1000" runat="server">
        <div class="eosTituloAMEC">
            <br />
            HONORARIOS MÁXIMOS SEGÚN POLÍTICA
        </div>
        <table class="eosTablaResultados honorarios" style="background: white; margin: auto;
            margin-top: 10px; width: 95%;">
            <thead>
                <tr>
                    <th>
                        Actividad
                    </th>
                    <th>
                        Importe máximo bruto
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptHonorariosMaximos">
                    <ItemTemplate>
                        <tr>
                            <td colspan="<%# Eval("Colspan")%>" <%# int.Parse(Eval("Colspan").ToString()) != 1 ? "align='center'" : "" %>>
                                <%# Eval("Text")%>
                            </td>
                            <asp:PlaceHolder runat="server" Visible='<%# int.Parse(Eval("Colspan").ToString()) == 1 %>'>
                                <td>
                                    <%# Server.HtmlDecode(Eval("Value").ToString()) %>
                                </td>
                            </asp:PlaceHolder>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <div class="eosTituloAMEC" style="font-size: 1.8em">
            <br />
            Casos especiales en los que se estima un importe estándar
        </div>
        <table class="eosTablaResultados honorarios" style="background: white; margin: auto;
            margin-top: 10px; width: 95%;">
            <thead>
                <tr>
                    <th>
                        Actividad
                    </th>
                    <th>
                        Importe estándar bruto
                    </th>
                    <th>
                        Importe máximo bruto
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptHonorariosMaximosSpecialCase">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("Text")%>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(Eval("Value").ToString()) %>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(Eval("Value2").ToString()) %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr style="height: 80px;">
                    <td colspan="3">
                        <div class="eosDivCentrada">
                            <img onclick="$('#eosContentFilter_idHonorariosMaximos').hide();return false;" src="/Styles/images/bt_cerrar.png" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="idCountryToCountry" style="display: none; width: 800px; height: 780px; z-index: 1000"
        runat="server">
        <div class="eosTituloAMEC" style="font-size: 1.6em">
            <br />
            HONORARIOS PONENTES NACIONALES DENTRO DEL PROCESO COUNTRY TO COUNTRY
        </div>
      <table class="eosTablaResultados honorarios" style="background: white; margin: 10px 12px 10px 20px;
            width: 45%; float: left;">
            <thead>
                <tr>
                    <th colspan="2">
                        Máximos
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x=>x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode()).Text %>
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptMaximosNonGlobal">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("TableText")%>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdMaxValue")).ToString("N2"))%>
                                <%# Server.HtmlDecode(Eval("IdMaxValueAppend").ToString())%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <table class="eosTablaResultados honorarios" style="background: white; margin: 10px 10px 10px 20px;
            width: 45%; float: left;">
            <thead>
                <tr>
                    <th colspan="2">
                        Máximos
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode()).Text%>
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptMaximosGlobal">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("TableText")%>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdMaxValue")).ToString("N2"))%>
                                <%# Server.HtmlDecode(Eval("IdMaxValueAppend").ToString())%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <table class="eosTablaResultados honorarios" style="background: white; margin: 10px 10px 10px 20px;
            width: 93%;">
            <thead>
                <tr>
                    <th rowspan="2" class="hiddenCell">
                    </th>
                    <th colspan="2">
                        <%= CountryToCountryObj.CountryToCountryNumSpeaksTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).Text%>
                    </th>
                    <th colspan="2">
                        <%= CountryToCountryObj.CountryToCountryNumSpeaksTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.SomeSpeaks.GetHashCode()).Text%>
                    </th>
                </tr>
                <tr>
                    <th>
                        Estandar
                    </th>
                    <th>
                        Máx.
                    </th>
                    <th>
                        Estandar
                    </th>
                    <th>
                        Máx.
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode()).Text%>
                    </td>
                    <asp:Repeater runat="server" ID="rptStandarCountryToCountryNonGlobal">
                        <ItemTemplate>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdStandarValue")).ToString("N2"))%>
                                <%# Server.HtmlDecode(Eval("IdStandarValueAppend").ToString())%>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdMaxValue")).ToString("N2"))%>
                                <%# Server.HtmlDecode(Eval("IdMaxValueAppend").ToString())%>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
                <tr>
                    <td>
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode()).Text%>
                    </td>
                    <asp:Repeater runat="server" ID="rptStandarCountryToCountryGlobal">
                        <ItemTemplate>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdStandarValue")).ToString("N2"))%>
                                &nbsp;<%# Server.HtmlDecode(Eval("IdStandarValueAppend").ToString())%>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdMaxValue")).ToString("N2"))%>
                                &nbsp;<%# Server.HtmlDecode(Eval("IdMaxValueAppend").ToString())%>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
            </tbody>
        </table>
        <table class="eosTablaResultados honorarios" style="background: white; margin: 10px 5px 10px 20px;
            width: 72%; float: left;">
            <thead>
                <tr>
                    <th colspan="3" class="hiddenCell">
                        Travelling
                    </th>
                    <th>
                        <%= CountryToCountryObj.CountryToCountryNumSpeaksTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.SomeSpeaks.GetHashCode()).Text%>
                    </th>
                    <th>
                        Día sin ponencia
                    </th>
                    <th>
                        Adicional
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td rowspan="2">
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode()).Text%>
                    </td>
                    <td>
                        1ª ciudad
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.FirstCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.FirstCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.FirstCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.SomeSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdDaysNoSpeak
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).Aditional
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).AditionalAppend
                        %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Ciudades consecutivas
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                                            x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.SomeSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdDaysNoSpeak
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).Aditional
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).AditionalAppend
                        %>
                    </td>
                </tr>
                <tr>
                    <td rowspan="2">
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode()).Text%>
                    </td>
                    <td>
                        1ª ciudad
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.FirstCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.FirstCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.FirstCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.SomeSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdDaysNoSpeak
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).Aditional
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).AditionalAppend
                        %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Ciudades consecutivas
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                                            x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.SomeSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdDaysNoSpeak
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).Aditional
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).AditionalAppend
                        %>
                    </td>
                </tr>
            </tbody>
        </table>
        <table class="eosTablaResultados honorarios" style="background: white; margin: 10px 10px 10px 10px;
            width: 20%; float: left;">
            <thead>
                <tr>
                    <th>
                        Motivos de excepción
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptExceptionReasons">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("Text") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <table class="eosTablaResultados honorarios" style="background: white; margin: 10px 10px 10px 20px;
            width: 93%;">
            <thead>
                <tr>
                    <th rowspan="2" class="hiddenCell">
                        COP (Programa / actividad dentro<br />
                        del mismo centro de trabajo)
                    </th>
                    <th colspan="2">
                        <%= CountryToCountryObj.CountryToCountryNumSpeaksModerTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).Text%>
                    </th>
                    <th colspan="2">
                        <%= CountryToCountryObj.CountryToCountryNumSpeaksModerTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.SomeSpeaks.GetHashCode()).Text%>
                    </th>
                </tr>
                <tr>
                    <th>
                        Estandar
                    </th>
                    <th>
                        Máx.
                    </th>
                    <th>
                        Estandar
                    </th>
                    <th>
                        Máx.
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode()).Text%>
                    </td>
                    <asp:Repeater runat="server" ID="rptCopCountryToCountryNonGlobal">
                        <ItemTemplate>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdStandarValue")).ToString("N2"))%>
                                <%# Server.HtmlDecode(Eval("IdStandarValueAppend").ToString())%>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdMaxValue")).ToString("N2"))%>
                                <%# Server.HtmlDecode(Eval("IdMaxValueAppend").ToString())%>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
                <tr>
                    <td>
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode()).Text%>
                    </td>
                    <asp:Repeater runat="server" ID="rptCopCountryToCountryGlobal">
                        <ItemTemplate>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdStandarValue")).ToString("N2"))%>
                                &nbsp;<%# Server.HtmlDecode(Eval("IdStandarValueAppend").ToString())%>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdMaxValue")).ToString("N2"))%>
                                &nbsp;<%# Server.HtmlDecode(Eval("IdMaxValueAppend").ToString())%>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
                <tr style="height: 80px;">
                    <td colspan="5">
                        <div class="eosDivCentrada">
                            <img onclick="$('#eosContentFilter_idCountryToCountry').hide();return false;" src="/Styles/images/bt_cerrar.png" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="idModuloParticipantes" style="display: none; width: 800px;" runat="server">
    
        <div class="eosTituloAMEC">Datos adicionales participante</div>


        <div id="eosContentAMECErrores">
            <asp:ValidationSummary ID="ContenHonorarioValidationSummary" runat="server" CssClass="failureNotification"
                ValidationGroup="ValidarHonorariosGroup" />
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
        </div>

        <input type="hidden" id="Participante" name="Participante" />


       <div class="eosTablaFiltros" >

           <div class="contentFormGeneric">


                <div class="contentForm eosMitadColumna">
                     <asp:Label runat="server" CssClass="labelForm"> Tipo de Participante  <span class="eosCampoObligatorio">*</span> </asp:Label>
                     <asp:DropDownList ID="ddlTipoAsistente" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW220"
                        DataTextField="tipoasistente" DataValueField="idtipoasistente">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfdddlTipoAsistente" runat="server" ErrorMessage="Campo Obligatorio: Tipo Asistente"
                        ControlToValidate="ddlTipoAsistente" ValidationGroup="ValidarHonorariosGroup"
                        SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                </div>

              <div class="contentForm eosMitadColumna" id="idtrpertenece">

                   <asp:Label CssClass="labelForm" ID="lblPagoDirecto" runat="server">Pertenece a mi fichero de APPIAN? <span class="eosCampoObligatorio floatLeft">*</span> </asp:Label>
                    <asp:RadioButtonList ID="rbficheroGenesis" runat="server" RepeatDirection="Horizontal"
                        CssClass="floatLeft radioCheck">
                        <asp:ListItem Text="Si" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rfdtxtficheroGenesis" runat="server" ErrorMessage="Campo Obligatorio: Fichero APPIAN"
                        ControlToValidate="rbficheroGenesis" ValidationGroup="ValidarHonorariosGroup"
                        SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                </div>



            </div>



           
           <div class="contentFormGeneric">


                <div class="contentForm eosMitadColumna">
                      <asp:Label ID="Label2" runat="server" CssClass="labelForm">  Nivel de riesgo HCP en APPIAN <span
                        class="eosCampoObligatorio">*</span></asp:Label>
                    <input type="hidden" id="inputRiskLevel" runat="server" />
                    <asp:TextBox ID="txtRiskLevel" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW220"
                        ReadOnly="True"></asp:TextBox>
                    <asp:DropDownList ID="ddlRiskLevel" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW220"
                        DataTextField="nivelriesgo" DataValueField="idnivelriesgo" Style="display: none">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfdddlRiskLevel" runat="server" ControlToValidate="txtRiskLevel"
                        ValidationGroup="ValidarHonorariosGroup" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                </div>

             <div class="contentForm eosMitadColumna" id="idtrtipoactividad">
                       <asp:Label ID="Label3" runat="server" CssClass="labelForm">  Tipo Actividad FCPA/Matriz Riesgo<span
                        class="eosCampoObligatorio">*</span></asp:Label>
                  <asp:DropDownList ID="ddlTipoActividadPax" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW450"
                        DataTextField="tipoactividadpax" DataValueField="idtipoactividadpax">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvddlTipoActividadPax" runat="server" ErrorMessage="Campo Obligatorio: Tipo Actividad FCPA/Matriz Riesgo"
                        ControlToValidate="ddlTipoActividadPax" ValidationGroup="ValidarHonorariosGroup"
                        SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                </div>

            </div>

                      
           <div class="contentFormGeneric" id="idtrjustificaciones" style="display: none">

                <input type="hidden" id="hiddenInternacional" runat="server" />
                <div class="contentForm eosUnoColumna">
                    <asp:Label ID="Label1" runat="server">Criterios para esponsorizar lideres científicos en Eventos celebrados fuera de España</asp:Label><span
                        class="eosCampoObligatorio">*</span>
        <asp:DropDownList ID="ddljustificaciones" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                        DataTextField="justificaciones" DataValueField="idjustificaciones" Style="word-break: break-word">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfdtxtjustificaciones" runat="server" ErrorMessage="Campo Obligatorio: Justificaciones"
                        ControlToValidate="ddljustificaciones" Enabled="False" ValidationGroup="ValidarHonorariosGroup"
                        SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                </div>

            </div>




  
    


            <asp:ScriptManager ID="MainScriptManager" runat="server" EnableScriptGlobalization="True"
                EnablePartialRendering="true">
            </asp:ScriptManager>



            <!-- *************************************** Calculadora Speaker *************************************** -->
             
           <div class="contentSpeaker contentCalculadoras contentFiltroAvanzado "  id="idtrCalculadora_Speaker_Chair" style="display: none">

            <asp:HiddenField runat="server" ID="hiddenPonentesNivelPSList" ClientIDMode="Static"  />



            <div id="idtrCalculadora_Speaker_Chair" class="calculadora" style="display: none">
              
                    <span class="eosTituloNormal" style="color: #FFFFFF">FMV / CÁLCULO HONORARIOS - Ponentes
                        y Moderadores</span>
            </div>


            <div id="idtrCalculadora_Speaker_Chair_Contenido" class="calculadora" style="display: none">
    
                    <asp:UpdatePanel runat="server" ID="updatePanel" class="eosTablaFiltros">
                        <ContentTemplate>
                                       
                               <div class="contentFormGeneric">


                                    <div class="contentForm eosMitadColumna">
                                         <asp:Label ID="lblSpeakerChairTipoReunion" runat="server" CssClass="labelForm"> Tipo de Reunión<span
                                                        class="eosCampoObligatorio">*</span></asp:Label>
                                          <asp:DropDownList ID="ddlSpeakerChairTipoReunion" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                                        OnSelectedIndexChanged="ddlSpeakerChairTipoReunion_Change" AutoPostBack="true"
                                                        DataTextField="Tipo" DataValueField="id" Style="word-break: break-word">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvddlSpeakerChairTipoReunion" runat="server" ErrorMessage="Campo Obligatorio: Tipo de Reunión"
                                                        ControlToValidate="ddlSpeakerChairTipoReunion" Enabled="False" ValidationGroup="ValidarHonorariosGroup"
                                                        SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                                     </div>
                                    <div class="contentForm eosMitadColumna">
                                              <asp:PlaceHolder runat="server" ID="plhTipoPonente" Visible="true">
                                                    <div  id="idPlhTipoPonente_1">
                                                        <asp:Label ID="Label22" runat="server" CssClass="labelForm">Tipo de Ponente<span class="eosCampoObligatorio">*</span></asp:Label>
                                                    </div>
                                                    <div id="idPlhTipoPonente_2">
                                                        <asp:RadioButtonList ID="rblTipoPonente" runat="server" RepeatDirection="Horizontal"
                                                            CssClass="floatLeft radioCheck" DataTextField="Tipo" DataValueField="id" AutoPostBack="true"
                                                            OnSelectedIndexChanged="rblTipoPonente_change">
                                                        </asp:RadioButtonList>
                                                        <asp:RequiredFieldValidator ID="rfvRblTipoPonente" runat="server" ErrorMessage="Campo Obligatorio: Tipo de Ponente"
                                                            ControlToValidate="rblTipoPonente" Enabled="False" ValidationGroup="ValidarHonorariosGroup"
                                                            SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                                                    </div>
                                                </asp:PlaceHolder>
                                     </div>

                               </div>

                               <div class="contentFormGeneric">


                                    <div class="contentForm eosMitadColumna">
                                          <asp:Label ID="lblSpeakerChairTipoPS" runat="server" CssClass="labelForm">Tipo de P.S.<span
                                                        class="eosCampoObligatorio">*</span></asp:Label>
                                        <asp:DropDownList ID="ddlSpeakerChairTipoPS" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                                        DataTextField="Text" DataValueField="id" Style="word-break: break-word" OnSelectedIndexChanged="ddlSpeakerChairTipoPS_Change"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvddlSpeakerChairTipoPS" runat="server" ErrorMessage="Campo Obligatorio: Tipo de P.S."
                                                        ControlToValidate="ddlSpeakerChairTipoPS" Enabled="False" ValidationGroup="ValidarHonorariosGroup"
                                                        SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                                                 </div>
                                                         <div class="contentForm eosMitadColumna">

                                                             <asp:Label ID="lblSpeakerChairFeeHoraTitle" runat="server" CssClass="labelForm">Fee / hora</asp:Label>
                                                                <div>
                                                                   <asp:TextBox ID="txtSpeakerChairFeeHora" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW120"
ReadOnly="True"></asp:TextBox>                                     <span class="bold-label not-top">€</span>
                                                                </div>
                                                                         
                                                 </div>
                               </div>


                       <div class="contentFormGeneric">


                                    <div class="contentForm eosMitadColumna">
                                                <asp:Label ID="lblSpeakerChairDuracionActividad" runat="server" CssClass="labelForm">Duración actividad<span
                                            class="eosCampoObligatorio">*</span></asp:Label>
                                        <asp:DropDownList ID="ddlSpeakerChairDuracionActividad" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                            OnSelectedIndexChanged="ddlSpeakerChairDuracionActividad_Change" AutoPostBack="true"
                                            DataTextField="Text" DataValueField="id" Style="word-break: break-word">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlSpeakerChairDuracionActividad" runat="server"
                                            ErrorMessage="Campo Obligatorio: Duración actividad" ControlToValidate="ddlSpeakerChairDuracionActividad"
                                            Enabled="False" ValidationGroup="ValidarHonorariosGroup" SetFocusOnError="True"
                                            ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                            </div>

                                             <div class="contentForm eosMitadColumna">
                                                  <asp:Label ID="lblSpeakerChairTiempoPreparacion" runat="server" CssClass="labelForm">Tiempo preparación<span
                                            class="eosCampoObligatorio">*</span></asp:Label>
                                                  <asp:DropDownList ID="ddlSpeakerChairTiempoPreparacion" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                            OnSelectedIndexChanged="ddlSpeakerChairTiempoPreparacion_Change" AutoPostBack="true"
                                            DataTextField="Text" DataValueField="id" Style="word-break: break-word">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlSpeakerChairTiempoPreparacion" runat="server"
                                            ErrorMessage="Campo Obligatorio: Tiempo preparación" ControlToValidate="ddlSpeakerChairTiempoPreparacion"
                                            Enabled="False" ValidationGroup="ValidarHonorariosGroup" SetFocusOnError="True"
                                            ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                            </div>


                               </div>


                            <table>


                     
                                <!-- -->
                                <tr>
                                    <td colspan="2" style="padding-left: 20px;" class="contentFormGeneric data-space">
                                        <asp:CheckBox ID="chkBoxPonenciaCentroSalud" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530 radioCheck"
                                            OnCheckedChanged="chkBoxPonenciaCentroSalud_Change" AutoPostBack="true" Style="word-break: break-word">
                                        </asp:CheckBox>
                                        <asp:Label ID="Label19" runat="server" CssClass="bold-label">Ponencia en reunión en centro de salud, hospital, EM, ... (Duración aprox. 60 min.)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-left: 20px;" class="contentFormGeneric data-space">
                                        <asp:CheckBox ID="chkBoxTalleres" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530 radioCheck"
                                            OnCheckedChanged="chkBoxTalleres_Change" AutoPostBack="true" Style="word-break: break-word">
                                        </asp:CheckBox>
                                        <asp:Label ID="Label20" runat="server" CssClass="bold-label">Mas de una ponencia en un día con el mismo contenido (Talleres) (Media o una jornada de trabajo)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-left: 20px;" class="contentFormGeneric data-space">
                                        <asp:CheckBox ID="chkBoxVideoconferenciasRepetidas" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530 radioCheck"
                                            OnCheckedChanged="chkBoxVideoconferenciasRepetidas_Change" AutoPostBack="true"
                                            Style="word-break: break-word"></asp:CheckBox>
                                        <asp:Label ID="Label21" runat="server" CssClass="bold-label">Repetición de Videoconferencia. Para este tipo de reuniones se entenderá que el tiempo de preparación es 0.</asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
            
            </div>


            <div id="idtrCalculadora_Speaker_Chair_Cierre" class="calculadora" style="display: none">
              

                    <div>
                        <div class="calc-col">
                            <div  class="center-elements">
                                <asp:Label ID="Label13" runat="server">Cálculo Honorarios según FMV <a id="idtrCalculadora_Speaker_Chair_a_show" onclick="javascript:toggleCalculadoraSpeaker(this, '#idtrCalculadora_Speaker_Chair_a_hide', true);" class="contentMoreInfo no-padd">+</a>
                                        <a id="idtrCalculadora_Speaker_Chair_a_hide" onclick="javascript:toggleCalculadoraSpeaker(this, '#idtrCalculadora_Speaker_Chair_a_show', false);" style="display: none;" class="contentMoreInfo no-padd">-</a></asp:Label>
                            </div>
                            <div class="center-elements">
                                <asp:UpdatePanel runat="server" ID="updPanelSpeakerHonorariosMaximos" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:PlaceHolder runat="server" ID="plhCalculadoraSemaforoVerde" Visible="false">
                                            <img src="Styles/images/ic_farma_verde.png" class="imgCalculadoraSemaforo" />
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" ID="plhCalculadoraSemaforoNaranja" Visible="false">
                                            <img src="Styles/images/ic_farma_naranja.png" class="imgCalculadoraSemaforo" />
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" ID="plhCalculadoraSemaforoRojo" Visible="false">
                                            <img src="Styles/images/ic_farma_rojo.png" class="imgCalculadoraSemaforo" />
                                        </asp:PlaceHolder>
                                        <asp:TextBox ID="txtSpeakerChairHonorariosMaximos" runat="server" CssClass="eosDisabledInputVacio eosInputVacio calculator eosSizeW120"
                                            ReadOnly="True"></asp:TextBox><span class="absolute-money">€&nbsp;&nbsp;&nbsp;</span>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Speaker_Chair_Comment_1" style="display: none;" class="notes-text">
                            <div>
                                <span>(*) Nota 1: Estos honorarios son estimados, pudiendo acordarse
                                    una cantidad menor si se estima adecuado</span>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Speaker_Chair_Comment_2" style="display: none;"  class="notes-text">
                            <div>
                                <span>(*) Nota 2: Según la política, los honorarios de los Moderadores
                                    aparecen ya calculados a un máximo del 80% del máximo permitido para ponentes</span>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Speaker_Chair_Comment_3" style="display: none;"  class="notes-text">
                            <div>
                                <span>(*) Nota 3: Para Ponentes / Moderadores dentro del proceso Country
                                    to Country emplear la pestaña correspondiente <a id="link_tabla_honorarios_country_to_country"
                                        href="#" style="color: #00CFB2">LINK directo a la tabla</a> </span>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Speaker_Chair_Comment_4" style="display: none;"  class="notes-text">
                            <div>
                                <div>
                                    <div>
                                        <div rowspan="4" style="padding: 0px 25px 0px 25px;">
                                            <span>Código de colores</span>
                                        </div>
                                    </div>
                                    <div>
                                        <div style="padding: 0px 25px 0px 25px;">
                                            Verde: Importe por debajo del máximo permitido
                                        </div>
                                    </div>
                                    <div>
                                        <div style="padding: 0px 25px 0px 25px;">
                                            Naranja: Posiblemente por encima del máximo. Comprobar en tabla
                                        </div>
                                    </div>
                                    <div>
                                        <div style="padding: 0px 25px 0px 25px;">
                                            Rojo: Supera el máximo permitido en cualquier supuesto
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Speaker_Chair_Comment_5" style="display: none;"  class="notes-text">
                            <div>
                                <asp:Label ID="Label14" runat="server">Antes de confirmar el Honorario, Comprobar con la tabla Honorarios Máximos <a id="link_tabla_honorarios_maximos_1" href="#" style="color: #00CFB2">LINK directo a la tabla</a> </asp:Label>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Speaker_Chair_Comment_6" style="display: none;"  class="notes-text">
                            <div>
                                <asp:Label ID="Label15" runat="server"><a href="http://ts1.merck.com/com/global_compliance_organization/Pages/Compliance-Standards-Page.aspx" target="_blank" style="color: #00CFB2">Acceso a listado GEMS</a></asp:Label>
                            </div>
                        </div>
                    </div>
         
            </div>

</div>






                      <div>



            <!-- *************************************** Fin Calculadora Speaker *************************************** -->
            <!-- *************************************** Calculadora Consultoria *************************************** -->
            
            <div class="contentFiltroAvanzado" id="idtrCalculadora_Consultoria_Container" style="display: none">
                <div id="idtrCalculadora_Consultoria" class="calculadora" style="display: none">
                    <div>
                        <span class="eosTituloNormal bold-label not-top">FMV / CÁLCULO HONORARIOS - Consultoría</span>
                    </div>
                </div>
                <div id="idtrCalculadora_Consultoria_Contenido" class="calculadora" style="display: none">
                    <div class="full-width">
                        <asp:UpdatePanel runat="server" ID="updatePanel2">
                            <ContentTemplate>
                                <div class="contentFormGeneric">
                                    <div class="flex-full-width radioCheck">
                                        <asp:Label ID="Label24" runat="server" CssClass="labelForm">Tipo de contrato <span class="eosCampoObligatorio"> *</span></asp:Label>
                                        <asp:RadioButtonList ID="rblConsultoriaTipoReunion" runat="server" RepeatDirection="Horizontal"
                                            OnSelectedIndexChanged="rblConsultoriaTipoReunion_Change" CssClass="floatLeft"
                                            AutoPostBack="true" DataTextField="Text" DataValueField="Value">
                                            <asp:ListItem Text="Día Consultor" Value="1" Selected="true" />
                                            <asp:ListItem Text="Contrato consultoría anual (contínua)" Value="0" />
                                        </asp:RadioButtonList>
                                        <div>
                                            
                                            
                                        </div>
                                    </div>
                                    <asp:PlaceHolder runat="server" ID="plhTxtConsultoriaDiaConsultor" Visible="True">
                                        <div id="idtrCalculadora_Consultoria_numdias">
                                            <div class="top-space">
                                                <asp:Label ID="Label25" runat="server" CssClass="labelForm">Número de días</asp:Label><span class="eosCampoObligatorio">*</span>
                                            </div>
                                            <div>
                                                <asp:TextBox ID="txtConsultoriaDiaConsultor" runat="server" CssClass="eosSizeW120"
                                                    AutoPostBack="True" onkeyup="javascript:CalculateHonorariosConsultoria();"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationGroup="ValidarHonorariosGroup"
                                                    ControlToValidate="txtConsultoriaDiaConsultor" ValidationExpression="[0-9]" ErrorMessage="El número de días no es numérico" />
                                            </div>
                                        </div>
                                    </asp:PlaceHolder>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div id="idtrCalculadora_Consultoria_Cierre" class="calculadora" style="display: none">
                <div>
                    <div>
                        <div class="calc-col center-content">
                            <div class="top-space">
                                <asp:Label ID="Label7" runat="server">Cálculo Honorarios según FMV <a class="contentMoreInfo no-padd" id="idtrCalculadora_Consultoria_a_show" onclick="javascript:toggleConsultoriaInfo(this, '#idtrCalculadora_Consultoria_a_hide', true);">+</a>
                                            <a class="contentMoreInfo no-padd" id="idtrCalculadora_Consultoria_a_hide" onclick="javascript:toggleConsultoriaInfo(this, '#idtrCalculadora_Consultoria_a_show', false);" style="display: none;">-</a></asp:Label>
                            </div>
                            <div>
                                <div>
                                    <asp:UpdatePanel runat="server" ID="updPanelConsultoriaHonorariosMaximos" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtConsultoriaHonorariosMaximos" runat="server" CssClass="eosDisabledInputVacio eosInputVacio calculator eosSizeW120"
                                                ReadOnly="True"></asp:TextBox><span class="white-bold">€&nbsp;&nbsp;&nbsp;</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Consultoria_Comment_1" style="display: none;" class="notes-text">
                            <div>
                                <span>(*) Nota 1: Estos honorarios son estimados, pudiendo acordarse
                                    una cantidad menor si se estima adecuado</span>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Consultoria_Comment_2" style="display: none;" class="notes-text">
                            <div>
                                <span>(*) Nota 2: Según la política, los honorarios de los Moderadores
                                    aparecen ya calculados a un máximo del 80% del máximo permitido para ponentes</span>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Consultoria_Comment_3" style="display: none;" class="notes-text">
                            <div>
                                <span>(*) Nota 3: Para Ponentes / Moderadores dentro del proceso Country
                                    to Country emplear la pestaña correspondiente <a id="link_tabla_honorarios_country_to_country_1"
                                        href="#" style="color: #00CFB2">LINK directo a la tabla</a> </span>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Consultoria_Comment_4" style="display: none;" class="notes-text">
                            <div>
                                <div>
                                    <div>
                                        <div rowspan="4" style="padding: 0px 25px 0px 25px;">
                                            <span>Código de colores</span>
                                        </div>
                                    </div>
                                    <div>
                                        <div style="padding: 0px 25px 0px 25px;">
                                            Verde: Importe por debajo del máximo permitido
                                        </div>
                                    </div>
                                    <div>
                                        <div style="padding: 0px 25px 0px 25px;">
                                            Naranja: Posiblemente por encima del máximo. Comprobar en tabla
                                        </div>
                                    </div>
                                    <div>
                                        <div style="padding: 0px 25px 0px 25px;">
                                            Rojo: Supera el máximo permitido en cualquier supuesto
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Consultoria_Comment_5" style="display: none;" class="notes-text">
                            <div>
                                <asp:Label ID="Label16" runat="server">Antes de confirmar el Honorario, Comprobar con la tabla Honorarios Máximos <a id="link_tabla_honorarios_maximos_3" href="#" style="color: #00CFB2">LINK directo a la tabla</a> </asp:Label>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Consultoria_Comment_6" style="display: none;" class="notes-text">
                            <div>
                                <asp:Label ID="Label17" runat="server"><a href="http://ts1.merck.com/com/global_compliance_organization/Pages/Compliance-Standards-Page.aspx" target="_blank" style="color: #00CFB2">Acceso a listado GEMS</a></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- *************************************** Fin Calculadora Consultoria *************************************** -->
            <!-- *************************************** Calculadora Eif & AB *************************************** -->
            <div class="contentFiltroAvanzado" id="idtrCalculadora_Eif_AB_container" style="display: none;">
                <div id="idtrCalculadora_Eif_AB" class="calculadora" style="display: none;">
                    <div>
                        <span class="eosTituloNormal bold-label not-top">FMV / CÁLCULO HONORARIOS - EIF
                            y Advisory boards</span>
                    </div>
                </div>
                <div id="idtrCalculadora_Eif_AB_Contenido" class="calculadora" style="display: none">
                    <asp:UpdatePanel runat="server" ID="updatePanel1">
                        <ContentTemplate>
                            <div class="contentFormGeneric">
                                <div style="display: none">
                                    <div class="contentForm eosMitadColumna">
                                        <asp:Label ID="lblEifABTipoPS" runat="server" class="labelForm">Tipo de P.S.<span class="eosCampoObligatorio"> *</span></asp:Label>
                                        <asp:DropDownList ID="ddlEifABTipoPS" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                            DataTextField="Text" DataValueField="id" Style="word-break: break-word">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                
                                    <div class="contentForm eosMitadColumna">
                                        <asp:Label ID="lblEifABFeeHoraTitle" runat="server" class="labelForm">Fee / hora<span class="eosCampoObligatorio"> *</span></asp:Label>
                                        <div>
                                            <asp:TextBox ID="txtEifABFeeHora" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW120" ReadOnly="True"></asp:TextBox>
                                            <span class="bold-label not-top">€</span>
                                        </div>
                                    </div>
                                
                                
                                    <div class="contentForm eosMitadColumna">
                                        <asp:Label ID="Label8" runat="server" class="labelForm">Duración actividad<span class="eosCampoObligatorio"> *</span></asp:Label>
                                        <asp:DropDownList ID="ddlEifABDuracionActividad" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                            OnSelectedIndexChanged="ddlEifABDuracionActividad_Change" AutoPostBack="true"
                                            DataTextField="Text" DataValueField="id" Style="word-break: break-word">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlEifABDuracionActividad" runat="server" ErrorMessage="Campo Obligatorio: Duración actividad"
                                            ControlToValidate="ddlEifABDuracionActividad" Enabled="False" ValidationGroup="ValidarHonorariosGroup"
                                            SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                                    </div>
                                
                                
                                    <div class="contentForm eosMitadColumna">
                                        <asp:Label ID="Label9" runat="server" class="labelForm">Tiempo preparación<span class="eosCampoObligatorio"> *</span></asp:Label>
                                        <asp:DropDownList ID="ddlEifABTiempoPreparacion" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                            OnSelectedIndexChanged="ddlEifABTiempoPreparacion_Change" AutoPostBack="true"
                                            DataTextField="Text" DataValueField="id" Style="word-break: break-word">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlEifABTiempoPreparacion" runat="server" ErrorMessage="Campo Obligatorio: Tiempo preparación"
                                            ControlToValidate="ddlEifABTiempoPreparacion" Enabled="False" ValidationGroup="ValidarHonorariosGroup"
                                            SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                                    </div>
                                
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
            </div>
            </div>
            <div id="idtrCalculadora_Eif_AB_Cierre" class="calculadora" style="display: none">
                <div>
                    <div>
                        <div class="calc-col center">
                            <div class="center-elements">
                                <asp:Label ID="Label10" runat="server">Cálculo Honorarios según FMV <a class="contentMoreInfo no-padd" id="idtrCalculadora_Eif_AB_a_show" onclick="javascript:toggleCalculadoraEif_AB(this, '#idtrCalculadora_Eif_AB_a_hide', true);">+</a>
                                        <a id="idtrCalculadora_Eif_AB_a_hide" onclick="javascript:toggleCalculadoraEif_AB(this, '#idtrCalculadora_Eif_AB_a_show', false);" style="display: none;" class="contentMoreInfo no-padd">-</a></asp:Label>
                            </div>
                            <div class="center-elements">
                                <asp:UpdatePanel runat="server" ID="updPanelEifAbHonorariosMaximos" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtEifABHonorariosMaximos" runat="server" CssClass="eosDisabledInputVacio eosInputVacio calculator eosSizeW120"
                                            ReadOnly="True"></asp:TextBox><span class="absolute-money">€&nbsp;&nbsp;&nbsp;</span>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <asp:Literal runat="server" ID="litChairmanAditionalEuros" Text="500" Visible="false"></asp:Literal>
                        </div>
                        <div id="idtrCalculadora_Eif_AB_Comment_1" style="display: none" class="notes-text">
                            <div>
                                <span>(*) Nota: Estos honorarios son estimados, pudiendo acordarse una
                                    cantidad menor si se estima adecuado</span>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Eif_AB_Comment_2" style="display: none" class="notes-text">
                            <div>
                                <span>(*) Nota: Se le añaden 500 euros al cálculo por su especial dedicación
                                    en la preparación del evento.</span>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Eif_AB_Comment_3" style="display: none" class="notes-text">
                            <div>
                                <asp:Label ID="Label11" runat="server">Antes de confirmar el Honorario, Comprobar con la tabla Honorarios Máximos <a id="link_tabla_honorarios_maximos_2" href="#" style="color: #00CFB2">LINK directo a la tabla</a> </asp:Label>
                            </div>
                        </div>
                        <div id="idtrCalculadora_Eif_AB_Comment_4" style="display: none" class="notes-text">
                            <div>
                                <asp:Label ID="Label12" runat="server"><a href="http://ts1.merck.com/com/global_compliance_organization/Pages/Compliance-Standards-Page.aspx" target="_blank" style="color: #00CFB2">Acceso a listado GEMS</a></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- *************************************** Fin Calculadora Eif & AB *************************************** -->
            <div class="contentFormGeneric">
                <div colspan="2">
                    <div>
                        <div class="line-content">
                            <div id="idtrhonorarios_1" style="display: none;">
                                
                            </div>
                            <div id="idtrhonorarios_2" style="display: none;" class="contentForm eosMitadColumna honorario-input">
                                <asp:Label ID="Label4" runat="server" CssClass="bold-label data-space">Honorarios</asp:Label>
                                <asp:UpdatePanel runat="server" ID="updHonorarioMaximo" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txthonorarios" runat="server" CssClass="eosSizeW120"></asp:TextBox>€
                                        <asp:HiddenField runat="server" ID="hidFieldIdValueHonorariosMaximos" ClientIDMode="Static" />
                                        <asp:HiddenField runat="server" ID="hidFieldValueHonorariosMaximos" ClientIDMode="Static" />
                                        <asp:HiddenField runat="server" ID="hidFieldIdValueHonorariosMaximosConsultoria"
                                            ClientIDMode="Static" />
                                        <asp:HiddenField runat="server" ID="hidFieldValueHonorariosMaximosConsultoria" ClientIDMode="Static" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="idtrpagodirecto_1" style="display: none;">
                                
                            </div>
                            <div id="idtrpagodirecto_2" style="display: none;" class="contentForm eosMitadColumna radioCheck">
                                <asp:Label ID="Label5" runat="server" CssClass="bold-label data-space">Pago a través de Sociedad/Fundación</asp:Label>
                                <asp:RadioButtonList ID="rblpagodirecto" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Si&nbsp;&nbsp;&nbsp;" Value="1" />
                                    <asp:ListItem Text="No" Value="0" />
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="idtrhonorarios_3" style="display: none;" class="col-content full-width">
                <div style="padding-left: 20px;">
                    <asp:Label ID="Label23" runat="server" class="bold-label data-space">Justificación</asp:Label>
                </div>
                <div>
                    <asp:TextBox ID="txtJustificacion" runat="server" TextMode="MultiLine"
                        MaxLength="200" CssClass="eosSizeW450 large-text-area"></asp:TextBox>
                </div>
            </div>
            <div id="errorHonorarioJustificacion" style="display: none">
                <div colspan="2">
                    <div id="eosContentAMECErroresJustificacion" class="failureNotification">
                        <ul>
                            <li>El campo justificación es obligatorio siempre que Honorarios introducidos < Honorarios
                                calculados</li></ul>
                    </div>
                </div>
            </div>
            <div id="idtrpagofundacionotros" style="display: none">
                <asp:Label ID="Label6" runat="server">Nombre de Sociedad/Fundación/Otros</asp:Label>
                <asp:TextBox ID="txtPagoSociedadOtros" runat="server" TextMode="MultiLine"
                    MaxLength="200" CssClass="eosSizeW450"></asp:TextBox>
            </div>
            <div id="errorMaximoHonorario" style="display: none">
                <div>
                    <div id="eosContentAMECErrores" class="failureNotification">
                        <ul>
                            <li>Ha sobrepasado el importe máximo marcado por el Cálculo Honorarios según FMV</li></ul>
                    </div>
                </div>
            </div>
            <div>
                <div colspan="2">
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="ValidarHonorariosGroup"
                        ControlToValidate="txthonorarios" ValidationExpression="[0-9]+(\.[0-9][0-9]?)?"
                        ErrorMessage="Honorarios no es decimal (debe poner una '.' y no ','). Solo Acepta 2 decimales" />
                </div>
            </div>

                      </div>
          <div class="eosBotonera eosBotonFiltrado">
                    <asp:Button ID="btnGuardarDatosUsuario" ValidationGroup="ValidarHonorariosGroup"
                        OnClick="btnSeleccionarParticipante" runat="server" Text="Guardar" />
                    <asp:Button ID="btnGuardarDatosUsuarioConEura" ValidationGroup="ValidarHonorariosGroup"
                        CssClass="eurahonorarios" OnClick="btnSeleccionarParticipante" runat="server" Text="Guardar" 
                         />
                    <asp:Button ID="btnCancelarDatosUsuario" Visible="True" runat="server"  Text="Cancelar" />
      
        </div>
        
        </div>
        <center>
            <asp:Image CssClass="ajaxImageClass" Style="visibility: hidden" ID="ajaxImage" runat="server"
                ImageUrl="~/Styles/images/ajax-loader.gif" Title="Cargando..." />
            <br />
        </center>
    </div>
<div class="eosBotonera eosBotonFiltrado">
        <asp:Button ID="btnGuardar" runat="server" ClientIDMode="Static"  Text="Guardar" 
            OnClientClick="confirma();" OnClick="btnGuardar_Click" />
        <!--<asp:Button ID="btnCancelar" runat="server" ClientIDMode="Static" Text="Volver" 
            OnClientClick="javascript:document.location = 'NuevoExpedientePasoA.aspx?idback=1&tipo=0';return false;" />-->
    </div>
</asp:Content>
<asp:Content ID="cphScripts" ContentPlaceHolderID="ContentPlaceHolderScripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("ul#eosHPBotones li").removeClass("active");
            var tipoPago = "<%=dExpediente.tipoPagoFee%>";
            if (tipoPago == "3") {
                $("ul#eosHPBotones li#liCalculadoraHonorarios").addClass("active");
            }
            else {
                $("ul#eosHPBotones li#liCreacionExpedienteIndividual").addClass("active");
            }
        });
    </script>
</asp:Content>