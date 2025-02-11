<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransportePanel.ascx.cs"
    Inherits="EOS.Controls.TransportePanel" %>
<%@ Register TagPrefix="eosc" TagName="AlternativasListViewControl" Src="~/Controls/AlternativasListViewControl.ascx" %>
<%@ Register TagPrefix="eosc" TagName="ParticipantesSelect" Src="~/Controls/ParticipantesSelect.ascx" %>
<script type='text/javascript'>
    var optionsImageOpen = new Image();
    optionsImageOpen.src = "/Styles/images/bullet_toggle_plus.png";
    var optionsImageClose = new Image();
    optionsImageClose.src = "Styles/images/bullet_toggle_minus.png";



    function ToggleView(layer_ref) {
        var element = document.getElementById(layer_ref);
        var optionsElement = document.getElementById("optionsTitle");
        if (optionsElement.disabled) {
            return;
        }

        if (element.style.display == 'block') {
            element.style.display = 'none';
            document["optionsImg"].src = optionsImageOpen.src;

        }
        else {
            element.style.display = 'block';
            document["optionsImg"].src = optionsImageClose.src;

        }
    }
</script>

        <div id="eosFilterHeaderEstado" style="position:absolute; top:-120px; left:700px">
            <span style="float:left;">ESTADO:&nbsp;</span>
            <asp:Label ID="lblEstado" runat="server" Text="Label"></asp:Label>
        </div>
<fieldset class="eos eosTablaFiltros contentTransporte">







        <input id="hdTipoBono" type="hidden" runat="server" value="DSP" />
        <input id="hdCombinacionIda" type="hidden" runat="server" value="false" />
         <input id="hdCombinacionRegreso" type="hidden" runat="server" value="false" />




        <div class="contentTitleTransButton">

                    <asp:Button ID="horariosImageButton" runat="server" Text="Horarios"
                        AlternateText="Horarios" />
             <%--       <asp:Label ID="Label2" runat="server" Text="Label" AssociatedControlID="horariosImageButton">Horarios</asp:Label>--%>
     
            </div>


            <div class="contentTitleTrans">
            <asp:Label ID="Label9" runat="server" Text="IDA" Font-Bold="True"></asp:Label>
 </div>
    
     

       <div class="contentFormGeneric">

                <div class="contentForm eosCuartoColumna">

                         <asp:Label ID="Label3" runat="server" Text="Fecha de Salida" Font-Bold="True"></asp:Label>
                         <eos:InputDatePickerControl ID="fechaSalidaIdaInputDatePickerControl1" runat="server" CssClass="eosDisabledInputVacio style1 calendarContentSpan" />
                    </div> 
                    <div class="contentForm eosCuartoColumna">
                             <asp:Label ID="Label4" runat="server" Text="Label" Font-Bold="True">Locomoción</asp:Label>
                    <asp:DropDownList ID="locomocionIdaDropDownList1" runat="server" >
                        <asp:ListItem Value="AIR">Aéreo</asp:ListItem>
                        <asp:ListItem Value="CAR">Car Renting</asp:ListItem>
                        <asp:ListItem Value="REN">Renfe</asp:ListItem>
                        <asp:ListItem Value="SBI">Sin Billete</asp:ListItem>
                    </asp:DropDownList>
                              </div> 
                    <div class="contentForm eosCuartoColumna">
                    
    
             <asp:Label ID="Label5" runat="server" Text="Label" Font-Bold="True">Nº Vuelo/Tren</asp:Label>
                        <asp:TextBox ID="numeroVueloIdaTrenTextBox1" runat="server" CssClass="eosDisabledInputVacio style1" 
                     ></asp:TextBox>
                             </div> 
                               <div class="contentForm eosCuartoColumna">
                                   <asp:Label ID="Label6" runat="server" Text="Label" Font-Bold="True">Origen</asp:Label>
                    <asp:TextBox ID="origenIda1" runat="server" CssClass="eosDisabledInputVacio style1"></asp:TextBox>
          

                                        </div> 
 </div>

    
       <div class="contentFormGeneric">

      
                    <div class="contentForm eosCuartoColumna">
                    
             <asp:Label ID="Label1" runat="server" Text="Label" Font-Bold="True">Destino</asp:Label>
                         <asp:TextBox ID="destinoIda1" runat="server"  CssClass="eosDisabledInputVacio style1"></asp:TextBox>
                     </div> 
                    <div class="contentForm eosCuartoColumna">
                            <asp:Label ID="Label7" runat="server" Text="Label" Font-Bold="True">H. Salida</asp:Label>
                             <eos:TimeInputBox ID="horaSalidaIdaTimeInputBox1" runat="server" CssClass="eosDisabledInputVacio style1" />
           
                             </div> 
                 <div class="contentForm eosCuartoColumna">
                    <asp:Label ID="Label8" runat="server" Text="Label" Font-Bold="True">H. Llegada</asp:Label>
                         <eos:TimeInputBox ID="horaLlegadaIdaTimeInputBox1" runat="server"  CssClass="eosDisabledInputVacio style1" />
                                  </div> 
 </div>



        
    <div class="contentFormGeneric">

      
            <div class="contentForm eosCuartoColumna contentCheckInput">

                      <asp:CheckBox ID="chbAltIDA" runat="server" AutoPostBack="True" 
                                    oncheckedchanged="chbAltIDA_CheckedChanged" CssClass="radioCheck" />

                    <eos:InputDatePickerControl ID="fechaSalidaIdaInputDatePickerControl2" 
                        runat="server"  CssClass="eosDisabledInputVacio style1 calendarContentSpan" />
            </div>
        
            <div class="contentForm eosCuartoColumna">
                <asp:DropDownList ID="locomocionIdaDropDownList2" runat="server"  EnableTheming="True">
                        <asp:ListItem Value="AIR">Aéreo</asp:ListItem>
                        <asp:ListItem Value="CAR">Car Renting</asp:ListItem>
                        <asp:ListItem Value="REN">Renfe</asp:ListItem>
                        <asp:ListItem Value="SBI">Sin Billete</asp:ListItem>
                    </asp:DropDownList>
            </div> 

            <div class="contentForm eosCuartoColumna">
                 <asp:TextBox ID="numeroVueloIdaTrenTextBox2" runat="server"   CssClass="eosDisabledInputVacio style1"></asp:TextBox>
            </div> 

            <div class="contentForm eosCuartoColumna">
                <asp:TextBox ID="origenIda2" runat="server" 
                        CssClass="eosDisabledInputVacio style1"></asp:TextBox>
            </div> 

     </div>





    <div class="contentFormGeneric">

      
            <div class="contentForm eosCuartoColumna">
                 <asp:TextBox ID="destinoIda2" runat="server"  CssClass="eosDisabledInputVacio style1"></asp:TextBox>

      </div> 
                    <div class="contentForm eosCuartoColumna">
                         <eos:TimeInputBox ID="horaSalidaIdaTimeInputBox2" runat="server"  CssClass="eosDisabledInputVacio style1" />

      </div> 

                    <div class="contentForm eosCuartoColumna">
                         <eos:TimeInputBox ID="horaLlegadaIdaTimeInputBox2" runat="server" CssClass="eosDisabledInputVacio style1" />

      </div> 

                    <div class="contentForm eosCuartoColumna">


      </div> 
     </div>



    <div class="contentFormGeneric">
    
            <div class="contentForm eosCuartoColumna contentTextAreaElement">
                 <a id="optionsTitle" onclick="ToggleView('ComentArea1');" href="#">
                        <img alt="" name="optionsImg" align="middle" src="Styles/images/bullet_toggle_plus.png" />
                        <b style="vertical-align: middle">Observaciones Ida</b> </a>
                    <div id="ComentArea1" class="" style="display: none">
                        <asp:TextBox ID="txtObservacionIda" runat="server" TextMode="MultiLine"
                            Rows="3" Width="100%" CssClass="eosDisabledInputVacio style1 contentTextArea"></asp:TextBox>
                    </div>
      </div> 
     </div>




    

            <div class="contentTitleTrans marginTop">
                      <asp:Label ID="Label18" runat="server" Text="REGRESO" Font-Bold="True"></asp:Label>
     </div>





            
    <div class="contentFormGeneric">
      
            <div class="contentForm eosCuartoColumna ">
                      <asp:Label ID="Label11" runat="server" Text="Fecha de Salida" Font-Bold="True"></asp:Label>
                  <eos:InputDatePickerControl ID="fechaSalidaRegresoInputDatePickerControl1" runat="server" CssClass="eosDisabledInputVacio style1 calendarContentSpan" />
            </div> 

            <div class="contentForm eosCuartoColumna">
                    <asp:Label ID="Label12" runat="server" Text="Label" Font-Bold="True">Locomoción</asp:Label>
                          <asp:DropDownList ID="locomocionRegresoDropDownList1" runat="server" >
                        <asp:ListItem Value="AIR">Aéreo</asp:ListItem>
                        <asp:ListItem Value="CAR">Car Renting</asp:ListItem>
                        <asp:ListItem Value="REN">Renfe</asp:ListItem>
                        <asp:ListItem Value="SBI">Sin Billete</asp:ListItem>
                    </asp:DropDownList>
            </div> 
      
            <div class="contentForm eosCuartoColumna">
                <asp:Label ID="Label13" runat="server" Text="Label" Font-Bold="True">Nº Vuelo/Tren</asp:Label>
                                    <asp:TextBox ID="numeroVueloTrenRegresoTextBox1" runat="server" CssClass="eosDisabledInputVacio style1"></asp:TextBox>
            </div> 

            <div class="contentForm eosCuartoColumna">
                     <asp:Label ID="Label14" runat="server" Text="Label" Font-Bold="True">Origen</asp:Label>
                                    <asp:TextBox ID="origenRegreso1" runat="server"  CssClass="eosDisabledInputVacio style1"></asp:TextBox>
            </div> 
     </div>
        <div class="contentFormGeneric">
      
            <div class="contentForm eosCuartoColumna">
                                    <asp:Label ID="Label17" runat="server" Text="Label" Font-Bold="True">Destino</asp:Label>
                                    <asp:TextBox ID="destinoRegreso1" runat="server"  CssClass="eosDisabledInputVacio style1"></asp:TextBox>
            </div> 

            <div class="contentForm eosCuartoColumna">
                         <asp:Label ID="Label15" runat="server" Text="Label" Font-Bold="True">H. Salida</asp:Label>
                            <eos:TimeInputBox ID="horaSalidaRegresoTimeInputBox1" runat="server"  CssClass="eosDisabledInputVacio style1" />
            </div> 
      
            <div class="contentForm eosCuartoColumna">
           
                    <asp:Label ID="Label16" runat="server" Text="Label" Font-Bold="True">H. Llegada</asp:Label>
                         <eos:TimeInputBox ID="horaLlegadaRegresoTimeInputBox1" runat="server"  CssClass="eosDisabledInputVacio style1" />
            </div> 

            <div class="contentForm eosCuartoColumna">

            </div> 
     </div>




            <div class="contentFormGeneric">
                <div class="contentForm eosCuartoColumna contentCheckInput">
  <asp:CheckBox ID="chbAltRegreso" runat="server" AutoPostBack="True" oncheckedchanged="chbAltREGRESO_CheckedChanged" CssClass="eosDisabledInputVacio style1 radioCheck" />
                    <eos:InputDatePickerControl ID="fechaSalidaRegresoInputDatePickerControl2" runat="server" CssClass="eosDisabledInputVacio style1 calendarContentSpan" />
            </div> 

                <div class="contentForm eosCuartoColumna">
        <asp:DropDownList ID="locomocionRegresoDropDownList2" runat="server">
                        <asp:ListItem Value="AIR">Aéreo</asp:ListItem>
                        <asp:ListItem Value="CAR">Car Renting</asp:ListItem>
                        <asp:ListItem Value="REN">Renfe</asp:ListItem>
                        <asp:ListItem Value="SBI">Sin Billete</asp:ListItem>
                    </asp:DropDownList>
            </div> 

                <div class="contentForm eosCuartoColumna">
                     <asp:TextBox ID="numeroVueloTrenRegresoTextBox2" runat="server"  CssClass="eosDisabledInputVacio style1"></asp:TextBox>
            </div> 

                <div class="contentForm eosCuartoColumna">
 <asp:TextBox ID="origenRegreso2" runat="server"  CssClass="eosDisabledInputVacio style1"></asp:TextBox>
            </div> 
     </div>






            <div class="contentFormGeneric">
                <div class="contentForm eosCuartoColumna">
                     <asp:TextBox ID="destinoRegreso2" runat="server"  CssClass="eosDisabledInputVacio style1"></asp:TextBox>
            </div> 
                <div class="contentForm eosCuartoColumna">
                      <eos:TimeInputBox ID="horaSalidaRegresoTimeInputBox2" runat="server"  CssClass="eosDisabledInputVacio style1" />

            </div> 
                <div class="contentForm eosCuartoColumna">
                     <eos:TimeInputBox ID="horaLlegadaRegresoTimeInputBox2" runat="server"  CssClass="eosDisabledInputVacio style1" />
            </div> 
                <div class="contentForm eosCuartoColumna">
            </div> 
     </div>




        <div class="contentFormGeneric">
    

            <div class="contentForm eosCuartoColumna contentTextAreaElement">
                   <a id="A1" onclick="ToggleView('ComentArea2');" href="#">
                        <img name="optionsImg" align="middle" src="Styles/images/bullet_toggle_plus.png" />
                        <b style="vertical-align: middle">Observaciones Regreso</b> </a>
                    <div id="ComentArea2" style="display: none">
                        <asp:TextBox ID="txtObservacionRegreso" runat="server"  TextMode="MultiLine"
                            Rows="3" Width="100%" CssClass="eosDisabledInputVacio style1 contentTextArea"></asp:TextBox>
                    </div>
            </div> 
     </div>



     <div class="contentFormGeneric">
                <div class="contentForm eosCuartoColumna">
                      <asp:Label ID="Label10" runat="server" Text="Label" AssociatedControlID="importeMaximoTextBox">Importe máximo</asp:Label>
                        <asp:TextBox ID="importeMaximoTextBox" runat="server" ToolTip="Importe máximo" 
                          CausesValidation="True"  CssClass="eosDisabledInputVacio style1">0</asp:TextBox> <div class="contentEuro">€</div>
                        
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="El importe máximo debe ser un valor numérico."
                            ControlToValidate="importeMaximoTextBox" MaximumValue="99999" MinimumValue="0"
                            Type="Currency" SetFocusOnError="True" Text="El importe máximo debe ser un valor numérico."></asp:RangeValidator>
            </div> 
     </div>


 <div class="contentSelectW">  

        <eosc:ParticipantesSelect ID="Test" runat="server" />

          </div>

     <div class="eosBotonera eosBotonFiltrado">
      <asp:Button ID="btnGuardar" runat="server"  Text="Guardar" 
        OnClick="btnGuardar_Click" Visible="False" OnClientClick="ConfirmaGuardar();" ClientIDMode="Static" />
      <asp:Button ID="btnEnviar" runat="server"  Text="Enviar"
        OnClick="btnEnviar_Click" ClientIDMode="Static"
                OnClientClick="ConfirmaEnvio();" Visible="False" />
      <asp:Button ID="btnAprobar" runat="server"  Text="Aprobar" 
        OnClick="btnAprobar_Click" 
            OnClientClick="ConfirmaAprobar();" Visible="False" ClientIDMode="Static"  />
      <asp:Button ID="btnRechazar" runat="server"  Text="Rechazar" 
        OnClick="btnRechazar_Click" ClientIDMode="Static"
            OnClientClick="ConfirmaRechazar();" Visible="False"  />
      <asp:Button ID="btnCancelar" runat="server"  Text="Cancelar" 
        OnClick="btnCancelar_Click" ClientIDMode="Static"
                OnClientClick="ConfirmaCancelacion();" Visible="False" />
<%--            <asp:ImageButton ID="btnNueva" runat="server" ImageUrl="~/Styles/images/bt_nueva_combinacion.png"
                Style="margin-right: 15px; margin-left: 15px;"  
                OnClick="btnNueva_Click" />--%>
        </div>
        <eosc:AlternativasListViewControl runat="server" ID="Alternativas" TipoServicio="2" />

</fieldset>
