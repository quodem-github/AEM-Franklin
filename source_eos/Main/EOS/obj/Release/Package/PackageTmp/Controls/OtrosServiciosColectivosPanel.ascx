<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OtrosServiciosColectivosPanel.ascx.cs"
    Inherits="EOS.Controls.OtrosServiciosColectivosPanel" %>
<%@ Register TagPrefix="eosc" TagName="AlternativasListViewControl" Src="~/Controls/AlternativasListViewControl.ascx" %>
<!-- Xavier Morell Pantalla de espera para cambios de página y confirmaciones 02/06/2011-->
<script language="javascript" type="text/javascript">
    //<!--
   
    function maximo_largo() {
        var texto;
        texto = document.getElementById('eosContentResults_InscripcionTabContainer_otrosServiciosColectivosTabPanel_otrosServiciosColectivosPanel_txtObservaciones').value;
        if (texto.length > 1024) {
            document.getElementById('eosContentResults_InscripcionTabContainer_otrosServiciosColectivosTabPanel_otrosServiciosColectivosPanel_txtObservaciones').value = texto.substring(0, 1024);
        }
    }
    // -->
</script>



<div id="eosFilterHeaderEstado" style="position: absolute; top: -120px; left: 700px">
    <span style="float: left;">ESTADO:</span>
    <asp:Label ID="lblEstado" runat="server" Text="Label"></asp:Label>
</div>



<fieldset class="eos eosTablaFiltros">
    


    <div class="contentFormGeneric">


                <div class="contentForm eosTresColumna">
            <asp:Label ID="Label1" runat="server" AssociatedControlID="descripcionTextBox">Descripcion<span class="eosCampoObligatorio">*</span></asp:Label>
            <asp:TextBox ID="descripcionTextBox" runat="server"  CssClass="eosDisabledInputVacio"
                ></asp:TextBox>
        </div>

   
                    <div class="contentForm eosTresColumna">
            <asp:Label ID="Label2" runat="server" AssociatedControlID="otroTipoTextBox">Tipo</asp:Label>
            <asp:DropDownList ID="cboTipo" runat="server" OnSelectedIndexChanged="cboTipo_SelectedIndexChanged"
                AutoPostBack="True">
            </asp:DropDownList>
        </div>
                    <div class="contentForm eosTresColumna">

                        <label>&nbsp;</label>
            <asp:TextBox ID="otroTipoTextBox" runat="server" Enabled="false" CssClass="eosDisabledInputVacio"
                ></asp:TextBox>


        </div>

        </div>




        <div class="contentFormGeneric">
                            <div class="contentForm eosTresColumna">




            <asp:Label ID="Label6" runat="server" AssociatedControlID="personasTextBox">No. Personas</asp:Label>
            <asp:TextBox ID="personasTextBox" runat="server" CssClass="eosDisabledInputVacio"></asp:TextBox>

        </div>
                    <div class="contentForm eosTresColumna">


            <asp:Label ID="Label3" runat="server" AssociatedControlID="sedeTextBox">Sede</asp:Label>
            <asp:TextBox ID="sedeTextBox" runat="server" CssClass="eosDisabledInputVacio"></asp:TextBox>

        </div>
                    <div class="contentForm eosTresColumna">

            <asp:Label ID="Label4" runat="server" AssociatedControlID="importeTextBox">Importe Máximo</asp:Label>
            <asp:TextBox ID="importeTextBox" runat="server" CssClass="eosDisabledInputVacio"></asp:TextBox>


        </div>       

    </div>




             <div class="contentFormGeneric">
                            <div class="contentForm eosCuartoColumna calendarContentSpan">
                                
                                
                                <eos:InputDatePickerControl ID="fechaInicioInputDatePickerControl" runat="server"
                Text="Fecha inicio"/>
                                   </div>



                        <div class="contentForm eosCuartoColumna calendarContentSpan">
            <eos:TimeInputBox ID="horaInicioTimeInputBox" runat="server" Text="Hora"  />
                               </div>
                        <div class="contentForm eosCuartoColumna calendarContentSpan">
            <eos:InputDatePickerControl ID="fechaFinalInputDatePickerControl" runat="server"
                Text="Fecha final"   />
                               </div> 
                        <div class="contentForm eosCuartoColumna calendarContentSpan">
            <eos:TimeInputBox ID="horaFinalTimeInputBox" runat="server" Text="Hora" />
  

   </div>



   </div>







             <div class="contentFormGeneric">
                            <div class="contentForm eosUnoColumna ">
   
                        <asp:Label ID="Label9" runat="server" AssociatedControlID="txtObservaciones">Observaciones</asp:Label>
                        
            
                        <asp:TextBox ID="txtObservaciones" runat="server" MaxLength="128" Height="60px"
                            TextMode="MultiLine" Rows="2" CssClass="eosDisabledInputVacio contentTextArea" onKeyUp="maximo_largo()" ></asp:TextBox>
           </div>
        </div>


        <div class="contentTableGeneric contentDetalleGeneric ">

 <h2>
                        <asp:Label ID="Label7" runat="server" AssociatedControlID="lvServicioActividades">Resumen de Servicios Colectivos</asp:Label></h2>
          
                        <asp:ObjectDataSource ID="odsOtrosServicios" runat="server" SelectMethod="ObtenerOtrosServicios"
                            TypeName="EOS.Web.AgenteExpedientes" OnSelecting="odsOtrosServicios_Selecting">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="1" Name="idExpediente" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ListView ID="lvServicioActividades" runat="server" DataSourceID="odsOtrosServicios"
                            OnItemDataBound="lvServicioActividades_ItemDataBound">
                            <emptydatatemplate>
                                <table class="eosTablaResultados eosTarifasTablaResultados" width="500px">
                                    <thead>
                                        <tr>
                                            <th style="width: 80px">
                                                Servicio
                                            </th>
                                            <th style="width: 90px">
                                                Tipo
                                            </th>
                                            <th style="width: 80px">
                                                Inicio
                                            </th>
                                            <th style="width: 80px">
                                                Final
                                            </th>
                                            <th style="width: 80px">
                                                Sede
                                            </th>
                                            <th style="width: 60px">
                                                Pax
                                            </th>
                                            <th class="cotizado" style="width: 80px">
                                                Importe Máximo
                                            </th>
                                            <th class="dato" style="width: 80px">
                                                Observaciones
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td colspan="8">
                                                <span class="eosTituloRojo">No se han encontrado servicios en este expediente</span>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </emptydatatemplate>
                            <layouttemplate>
                                <table class="eosTablaResultados eosTarifasTablaResultados" width="500px">
                                    <thead>
                                        <tr>
                                            <th style="width: 95px">
                                                Servicio
                                            </th>
                                            <th style="width: 95px">
                                                Tipo
                                            </th>
                                            <th style="width: 80px">
                                                Inicio
                                            </th>
                                            <th style="width: 80px">
                                                Final
                                            </th>
                                            <th style="width: 80px">
                                                Sede
                                            </th>
                                            <th style="width: 60px">
                                                Pax
                                            </th>
                                            <th class="cotizado" style="width: 80px">
                                                Importe Máximo
                                            </th>
                                            <th style="width: 80px">
                                                Observaciones
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                    </tbody>
                                </table>
                            </layouttemplate>
                            <itemtemplate>
                                <tr>
                                    <td  >
                                        <asp:Label ID="DescripcionLabel" runat="server" Text='<%# Eval("descripcion") %>'
                                            Width="120px" />
                                    </td>
                                    <td class="par"  >
                                        <asp:Label ID="tipoLabel" runat="server" Text='<%# Eval("tipo") %>' Width="95px" />
                                    </td>
                                    <td>
                                        <asp:Label ID="fechainicioLabel" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",Eval("fechainicio")) %>'
                                            Width="80px" />
                                    </td>
                                    <td class="par"  >
                                        <asp:Label ID="fechafinLabel" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",Eval("fechafin")) %>'
                                            Width="80px" />
                                    </td>
                                    <td>
                                        <asp:Label ID="sedeLabel" runat="server" Text='<%# Eval("sede") %>' Width="80px" />
                                    </td>
                                     <td class="par">
                                        <asp:Label ID="paxLabel"    runat="server" Text='<%# String.Format("{0:0}",Eval("paxvis") ?? 0) %>' Width="60px" />
                                    </td>
                                    <td style="width: 80px;text-align:right;">
                                        <asp:Label ID="importeLabel" runat="server" Text='<%# String.Format("{0:0.00} €",Eval("pvp") ?? 0) %>'
                                            Width="80px" />
                                    </td>
                                     <td  class="par">
                                        <asp:Label ID="Label5" runat="server" Text='<%# Eval("observaciones") %>' Width="120px" />
                                    </td>
                                </tr>
                            </itemtemplate>
                        </asp:ListView>
                  </div>

     <div class="eosBotonera eosBotonFiltrado">
            <asp:Button ID="btnGuardar" runat="server"  Text="Guardar"
                 OnClick="btnGuardar_Click" OnClientClick="ConfirmaGuardar();"
                ClientIDMode="Static" Visible="False" />
            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" 
                 OnClick="btnNuevo_Click" Visible="False" />
            <asp:Button ID="btnEnviar" runat="server"  Text="Enviar" 
               OnClick="btnEnviar_Click" Visible="False" />
            <asp:Button ID="btnAprobar" runat="server"  Text="Aprobar"
                OnClick="btnAprobar_Click" OnClientClick="ConfirmaAprobar();"
                Visible="False" />
            <asp:Button ID="btnRechazar" runat="server"  Text="Rechazar"
                 OnClick="btnRechazar_Click" OnClientClick="ConfirmaRechazar();"
                Visible="False" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"  
                 OnClientClick="ConfirmaCancelacion();" Visible="False" />
        </div>

    <eosc:AlternativasListViewControl runat="server" ID="Alternativas" TipoServicio="3" />
    
</fieldset>
