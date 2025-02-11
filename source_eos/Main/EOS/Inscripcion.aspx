<%@ Page Title="Inscripción" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true"
    CodeBehind="Inscripcion.aspx.cs" Inherits="EOS.Inscripcion" EnableEventValidation="false"
    Culture="es-ES" UICulture="es" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script language="javascript" type="text/javascript">
        function GoBack() {
            document.location = 'DetalleExpediente.aspx?idexp=<%= Request.QueryString["idexp"] %>';
        }
    </script>
</asp:Content>

<%--<asp:Content ID="ContentBotonera" ContentPlaceHolderID="eosHeaderBotonera" runat="server">
    <div id="eosHPBotones">
        <div class="eosBotonera eosBotonInformes">
            <a title="Acceder a Informes y Listados" onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Acceder a Informes y Listados';return true;" href="javascript:document.location.href = '/Informes/FiltroInforme.aspx';">
                Acceso a Informes y Listados</a></div>
        <div class="eosBotonera eosBotonDatosPer">
            <a title="Acceder a Datos Personales" onmouseout="javascript:window.status='';return true;"
                onmouseover="javascript:window.status='Cambiar datos personales';return true;"
                href="javascript:document.location.href = '/CambiarDatosPersonales.aspx';">Acceso
                a Datos Personales</a></div>
        <div class="eosBotonera eosBotonNuevoExp">
            <a title="Crear Nuevo Expediente" onmouseout="javascript:window.status='';return true;"
                onmouseover="javascript:window.status='Crear un Nuevo Expediente';return true;"
                href="javascript:document.location.href = '/NuevoExpedientePasoA.aspx?idexp=0';">
                Nuevo Expediente</a></div>
        <div class="eosBotonera eosBotonAMEC">
            <a title="Acceder a Listados AMEC" onmouseout="javascript:window.status='';return true;"
                onmouseover="javascript:window.status='Litados de AMECs';return true;" href="javascript:document.location.href = '/ListadoAMECs.aspx';">
                Listados de AMECs</a></div>
        <!--<div class="eosBotonera eosBotonActividades">
            <a title="Acceder a Actividades" href="javascript:NoImplementado();">Acceso a Eventos</a></div>-->
        <div class="eosBotonera eosBotonSalir">

            <a title="Volver al Detalle de Expediente" id="btnSalir" runat="server" href="/DetalleExpediente.aspx">
                Volver al detalle de Expediente</a>


        </div>
    </div>
</asp:Content>--%>

<asp:Content ID="ContentDetalles" ContentPlaceHolderID="eosHeaderContent" runat="server">
    
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="eosContentResults" runat="server">

    
      <h1>Inscripción</h1>


      <div id="eosFilterHeaderDetalle" class="contentDetalleGeneric">
          <h2>Información</h2>
        <div id="infoPanel">



                <div class="contentInfoPanel">
                        <div class="contentInfoPanelElement">

                        <span class="lblTitle">Evento:</span><asp:Label ID="congresoLabelValue" runat="server" Text="" CssClass="eosCampoLabelDato"></asp:Label>
                  <span class="separateElement">|</span> 
                        <span class="lblTitle" >Lugar:</span>
                        <asp:Label ID="LblPoblacion"  runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                 </div>

                        <div class="contentInfoPanelElement">


                        <span class="lblTitle" >Inicio:</span>
                        <asp:Label ID="lblFechaDesde"  runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                  <span class="separateElement">|</span> 
                        <span class="lblTitle">Fin:</span>
                        <asp:Label ID="LblFechaHasta"  runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label> 
                  <span class="separateElement">|</span> 
                            <span class="lblTitle">Agencia:</span><asp:Label ID="lblAgencia"
                            runat="server" Text="" CssClass="eosCampoLabelDato"></asp:Label>
       
     </div>
                            <div class="contentInfoPanelElement">


                        <span class="lblTitle">Fecha de expediente:</span><asp:Label ID="fechaExpedienteLabelValue"
                            runat="server" Text="" CssClass="eosCampoLabelDato"></asp:Label>
                  <span class="separateElement">|</span> 
                       <span class="lblTitle">Pedido:</span><asp:Label ID="pedidoLabelValue"
                            runat="server" Text="" CssClass="eosCampoLabelDato"></asp:Label>
                  <span class="separateElement">|</span> 
                        <span class="lblTitle">Nº Expediente:</span><asp:Label ID="pedidoLabelExpediente"
                            runat="server" Text="" CssClass="eosCampoLabelDato"></asp:Label>
          
     </div>

   


                            <div class="contentInfoPanelElement">


                        <asp:ImageButton ID="imgExpedienteDetalle" runat="server" ImageUrl="~/Styles/images/ic_lupa.png" Visible="false" />
                        <span class="lblTitle">AMEC:</span><asp:Label ID="amecLabelValue" runat="server" Text="" CssClass="eosCampoLabelDato"></asp:Label>
                       </div>


       
     </div>

           <div class="eosHPBotonesDetalle" id="">

            
                              <a title="Volver al Detalle de Expediente" id="btnSalir" runat="server" href="/DetalleExpediente.aspx">
                Volver</a>

           </div>
         
             
      

    


        </div>
    </div>














    <script language="javascript" type="text/javascript">
        function ActiveTabChanged(sender, e) {
            __doPostBack('<%= InscripcionTabContainer.ClientID %>', sender.get_activeTab().get_headerText());
        }
    </script>
    <asp:ScriptManager ID="MainScriptManager" runat="server" EnableScriptGlobalization="True"
        EnablePartialRendering="true">
    </asp:ScriptManager>
<%--    <act:TabContainer ID="InscripcionTabContainer" runat="server" ActiveTabIndex="0"
        OnActiveTabChanged="InscripcionTabContainer_ActiveTabChanged" OnClientActiveTabChanged="ActiveTabChanged"
        CssClass="ajax__tab_technorati-theme">
--%>
    <act:TabContainer ID="InscripcionTabContainer" runat="server" ActiveTabIndex="0"
        OnActiveTabChanged="InscripcionTabContainer_ActiveTabChanged" OnClientActiveTabChanged="ActiveTabChanged"
        >



        <act:TabPanel runat="server" HeaderText="Inscripción" ID="inscripcionTabPanel" CssClass="inscripcionTabPanel">
            <ContentTemplate>
                <asp:UpdatePanel ID="inscripcionUpdatePanel" runat="server">
                    <ContentTemplate>
                        <eosc:TarifasInscripcionPanel runat="server" ID="tarifasInscripcionPanel" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </act:TabPanel>


        <act:TabPanel runat="server" HeaderText="Alojamiento" ID="alojamientoTabPanel">
            <ContentTemplate>
                <asp:UpdatePanel ID="alojamientoUpdatePanel" runat="server">
                    <contenttemplate>
                        <eosc:AlojamientoPanel runat="server" ID="alojamientoPanel" />
                    </contenttemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </act:TabPanel>


        <act:TabPanel runat="server" HeaderText="Transporte" ID="transporteTabPanel">
            <ContentTemplate>
                <asp:UpdatePanel ID="transporteUpdatePanel" runat="server">
                    <ContentTemplate>
                        <eosc:TransportePanel runat="server" ID="transportePanel" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </act:TabPanel>


        <act:TabPanel runat="server" HeaderText="Otros servicios" ID="otrosServiciosTabPanel">
            <ContentTemplate>
                <asp:UpdatePanel ID="otrosserviciosUpdatePanel" runat="server">
                    <ContentTemplate>
                        <eosc:OtrosServiciosPanel runat="server" ID="otrosServiciosPanel" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </act:TabPanel>


        <act:TabPanel runat="server" HeaderText="Otros servicios" ID="otrosServiciosColectivosTabPanel">
            <ContentTemplate>
                <asp:UpdatePanel ID="otrosserviciosColectivosUpdatePanel" runat="server">
                    <ContentTemplate>
                        <eosc:OtrosServiciosColectivosPanel runat="server" ID="otrosServiciosColectivosPanel" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </act:TabPanel>


    </act:TabContainer>
</asp:Content>
