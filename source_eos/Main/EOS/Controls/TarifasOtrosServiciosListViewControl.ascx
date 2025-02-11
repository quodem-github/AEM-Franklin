<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TarifasOtrosServiciosListViewControl.ascx.cs" Inherits="EOS.Controls.TarifasOtrosServiciosListViewControl" %>

<asp:ListView ID="TarifasListView" runat="server" DataKeyNames="IdTarifaActividad">
    <EmptyDataTemplate>
        <table class="eosTablaResultados eosTarifasTablaResultados" style="margin-bottom: 16px;">
        <thead>
            <tr>
                <th></th>
                <th>PROVEEDOR</th>
                <th>DESCRIPCIÓN</th>
                <th>TIPO</th>
                <th>PRECIO</th>
                <th>CANCELACIÓN</th>
                <th>INICIO</th>
                <th>FIN</th>
            </tr>
        </thead>
          <tbody>
            <tr>
              <td colspan="8" style="text-align: center; color:Gray" >
              No existen otros servicios precargados.
              </td>
            </tr>
          </tbody>
    </table>
    </EmptyDataTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:ImageButton ID="imgSeleccionarTarifa" runat="server" CommandName="Selected" ToolTip='<%# String.Format("Seleccionar Tarifa {0}", Eval("IdTarifaActividad")) %>' ImageUrl="~/Styles/images/ic_amec.png" OnCommand="ImageButton_Command" CommandArgument='<%#Eval("IdTarifaActividad")%>'/>
            </td>
            <td>
                <asp:Label ID="ProveedorLabel" runat="server" Text='<%# Eval("proveedor") %>' />
            </td>
            <td>
                <asp:Label ID="DescripcionLabel" runat="server" Text='<%# Eval("actividad") %>' />
            </td>
            <td class="par">
                <asp:Label ID="TipoLabel" runat="server" 
                    Text='<%# Eval("tipoactividadcongreso") %>' />
            </td>
            <td>
                <asp:Label ID="PrecioLabel" runat="server" Text='<%# Eval("PVP") %>' />
            </td>
             <td>
                <asp:Label ID="CancelacionLabel" runat="server" Text='<%# Eval("cancelacion") %>' />
            </td>
            <td>
                <!-- Ismael Ameller 25/02/2011 Añadidos los campos de la grid-->
                <asp:Label ID="InicioLabel" runat="server" Text='<%# Eval("fechainicio").ToString().Contains("0:00:00") ? String.Format("{0:dd/MM/yyyy}",Eval("fechainicio")) : String.Format("{0:dd/MM/yyyy HH:mm:ss}",Eval("fechainicio")) %>' />
            </td>
            <td>
                <!-- Ismael Ameller 25/02/2011 Añadidos los campos de la grid-->
                <asp:Label ID="FinLabel" runat="server" Text='<%# Eval("fechafin").ToString().Contains("0:00:00") ? String.Format("{0:dd/MM/yyyy}",Eval("fechafin")) : String.Format("{0:dd/MM/yyyy HH:mm:ss}",Eval("fechafin")) %>' />
            </td>
        </tr>
    </ItemTemplate>
    <LayoutTemplate>
        <table class="eosTablaResultados eosTarifasTablaResultados">
            <thead>
                <tr>
                    <th></th>
                    <th>PROVEEDOR</th>
                    <th>DESCRIPCIÓN</th>
                    <th>TIPO</th>
                    <th>PRECIO</th>
                    <th>CANCELACIÓN</th>
                    <th>INICIO</th>
                    <th>FIN</th>
                </tr>
            </thead>
            <tbody>
                <tr ID="itemPlaceholder" runat="server">
                </tr>
            </tbody>
        </table>
    </LayoutTemplate>
</asp:ListView>
<span class="space-up"><asp:LinkButton ID="lnkBtnOtraInscripcion" runat="server" 
    onclick="LinkButton1_Click" CssClass="linkServiciosNoListados">Indicar otro evento no listada</asp:LinkButton> (deberá rellenar los siguientes campos)</span>
