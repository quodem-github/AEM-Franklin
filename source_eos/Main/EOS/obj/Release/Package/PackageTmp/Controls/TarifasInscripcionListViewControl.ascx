<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TarifasInscripcionListViewControl.ascx.cs" Inherits="EOS.Controls.TarifasInscripcionListViewControl" %>

<asp:ListView ID="TarifasListView" runat="server" DataKeyNames="IdTarifaInscripcion">


    <EmptyDataTemplate>



        <div class="contentTableGeneric">
        <table class="eosTablaResultados eosTarifasTablaResultados" >
        <thead>
            <tr>
                <th></th>
                <th>Tipo</th>
                <th>Precio</th>
            </tr>
        </thead>
          <tbody>
            <tr>
              <td colspan="3" style="text-align: center; color:Gray">
              No existen inscripciones precargadas.
              </td>
            </tr>
          </tbody>
    </table>

</div>

    </EmptyDataTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:ImageButton ID="imgSeleccionarTarifa" runat="server" CommandName="Selected" ToolTip='<%# String.Format("Seleccionar Tarifa {0}", Eval("IdTarifaInscripcion")) %>' ImageUrl="~/Styles/images/ic_amec.png" OnCommand="ImageButton_Command" CommandArgument='<%#Eval("IdTarifaInscripcion")%>'/>
            </td>
            <td class="par">
                <asp:Label ID="TipoLabel" runat="server" 
                    Text='<%# Eval("Descripcion") %>' />
            </td>
            <td>
                <asp:Label ID="PrecioLabel" runat="server" Text='<%# Eval("PVP") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <LayoutTemplate>
        <div class="contentTableGeneric">
        <table class="eosTablaResultados eosTarifasTablaResultados" >
            <thead>
                <tr>
                <th></th>
                <th>Tipo</th>
                <th>Precio</th>
                </tr>
            </thead>
            <tbody>
                <tr ID="itemPlaceholder" runat="server">
                </tr>
            </tbody>
        </table>
        </div>
    </LayoutTemplate>
</asp:ListView>

<p class="marginTop">
<asp:LinkButton ID="lnkBtnOtraInscripcion" onclick="LinkButton1_Click" runat="server" CssClass="linkServiciosNoListados">Indicar otra inscripción no listada</asp:LinkButton> (deberá rellenar los siguientes campos)</p>