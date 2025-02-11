<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlojamientoPanel.ascx.cs"
    Inherits="EOS.Controls.AlojamientoPanel" %>
<%@ Register TagPrefix="eosc" TagName="AlternativasListViewControl" Src="~/Controls/AlternativasListViewControl.ascx" %>
<%@ Register TagPrefix="eosc" TagName="ParticipantesSelect" Src="~/Controls/ParticipantesSelect.ascx" %>

<div id="eosFilterHeaderEstado" style="position: absolute; top: -120px; left: 700px">
    <span style="float: left;">ESTADO:&nbsp;</span>
    <asp:Label ID="lblEstado" runat="server" Text="Label"></asp:Label>
</div>





<fieldset class="eos eosTablaFiltros">
    <div class="contentFormGeneric">

         <div class="contentForm eosTresColumna calendarContentSpan">








           
                    <eos:InputDatePickerControl ID="llegadaInputDatePickerControl" runat="server" Text="Llegada&lt;span class=&quot;eosCampoObligatorio&quot;&gt;*&lt;/span&gt;</td><td>" />
     



         </div>

         <div class="contentForm eosTresColumna calendarContentSpan">

              <eos:InputDatePickerControl ID="salidaInputDatePickerControl" runat="server" Text="Salida&lt;span class=&quot;eosCampoObligatorio&quot;&gt;*&lt;/span&gt;</td><td>" />
         </div>


    </div>


        <div class="contentFormGeneric">

         <div class="contentForm eosTresColumna ">
                    <asp:Label ID="Label1" CssClass="eosCampoLabelSin" runat="server" AssociatedControlID="tipoDropDownList">Tipo</asp:Label>
     

                    <!-- Ismael Ameller 24/02/2011 Cambio de label en el combo, el valor 2 antes era "Solicitados para Evento"-->
                    <asp:DropDownList ID="tipoDropDownList" runat="server" OnSelectedIndexChanged="tipoDropDownList_SelectedIndexChanged"
                        AutoPostBack="True">
                        <asp:ListItem Value="0">Todos</asp:ListItem>
                        <asp:ListItem Value="1">Otros no listados</asp:ListItem>
                        <asp:ListItem Value="2">Hoteles secretaría</asp:ListItem>
                    </asp:DropDownList>
         </div>
         <div class="contentForm eosTresColumna ">
  
                    <asp:Label ID="Label2" CssClass="eosCampoLabelSin" runat="server" AssociatedControlID="poblacionDropDownList">Población</asp:Label>
       
                    <asp:DropDownList ID="poblacionDropDownList" runat="server"  AutoPostBack="True"
                        OnSelectedIndexChanged="poblacionDropDownList_SelectedIndexChanged">
                    </asp:DropDownList>
    
         </div>

         <div class="contentForm eosTresColumna ">

                    <asp:Label ID="Label3" CssClass="eosCampoLabelSin" runat="server" AssociatedControlID="hotelDropDownList">Hotel</asp:Label>
               
                    <asp:DropDownList ID="hotelDropDownList" runat="server"  AutoPostBack="True"
                        OnSelectedIndexChanged="hotelDropDownList_SelectedIndexChanged" >
                    </asp:DropDownList>
                    <asp:TextBox ID="hotelTextBox" runat="server" Visible="False" ></asp:TextBox>
        
         </div>
    </div>


        
   
                        
    
        <div class="contentFormGeneric">

         <div class="contentForm eosTresColumna ">
               


                    <asp:Label ID="Label5" CssClass="eosCampoLabelSin" runat="server" AssociatedControlID="tipoHabitacionDropDownList">Tipo habitación</asp:Label>
   
                    <asp:DropDownList ID="tipoHabitacionDropDownList" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="tipoHabitacionDropDownList_SelectedIndexChanged" >
                    </asp:DropDownList>
      
                      </div>
           <div class="contentForm eosTresColumna ">


                    <asp:Label ID="Label4" CssClass="eosCampoLabelSin" runat="server" AssociatedControlID="precioNocheMaximoTextBox">Precio por noche máximo</asp:Label>
         
                    <asp:TextBox ID="precioNocheMaximoTextBox" runat="server" ></asp:TextBox>
 

               
         </div>
    </div>








    <div class="contentSelectW">     <eosc:ParticipantesSelect ID="Test" runat="server" /></div>  
     <div class="eosBotonera eosBotonFiltrado" >
            <asp:Button ID="btnGuardar" runat="server"  Text="Guardar" 
                OnClick="btnGuardar_Click" Visible="False" OnClientClick="ConfirmaGuardar();"
                ClientIDMode="Static" />
            <asp:Button ID="btnEnviar" runat="server" Text="Enviar" 
                OnClick="btnEnviar_Click" ClientIDMode="Static" OnClientClick="ConfirmaEnvio();"
                Visible="False" />
            <asp:Button ID="btnAprobar" runat="server" Text="Aprobar"
                OnClick="btnAprobar_Click" ClientIDMode="Static"
                OnClientClick="ConfirmaAprobar();" Visible="False" />
            <asp:Button ID="btnRechazar" runat="server" Text="Rechazar" 
                OnClick="btnRechazar_Click" ClientIDMode="Static"
                OnClientClick="ConfirmaRechazar();" Visible="False" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
                OnClick="btnCancelar_Click" ClientIDMode="Static"
                OnClientClick="ConfirmaCancelacion();" Visible="False" />
        </div>
        <eosc:AlternativasListViewControl runat="server" ID="Alternativas" TipoServicio="1" />

</fieldset>
