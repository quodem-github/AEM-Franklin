<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlternativasListViewControl.ascx.cs" Inherits="EOS.Controls.AlternativasListViewControl" %>

<asp:UpdatePanel ID="alternativasUpdatePanel" runat="server" class="contentTableGeneric space-under">
  <ContentTemplate>
    <asp:ListView ID="AlternativasListView" runat="server" DataKeyNames="IdTramitacion">
      <EmptyDataTemplate>
        <table class="eosTablaResultadosShort eosTablaResultados">
          <thead>
            <tr>
              <th>
              </th>
              <th>
              </th>
              <th>
                IMPORTE
              </th>
              <th>
                PROVEEDOR
              </th>
              <th>
                VALIDEZ
              </th>
              <th>
                GASTOS
              </th>
              <th>
                DESCRIPCIÓN DEL SERVICIO
              </th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td colspan="7" style="text-align: center; color:Gray">
              No existe alternativas para este servicio.
              </td>
            </tr>
          </tbody>
        </table>
      </EmptyDataTemplate>
      <ItemTemplate>
        <tr>
          <td style="width: 20px">
            <asp:ImageButton ID="imgSeleccionarTarifa" runat="server" CommandName="Selected"
              ToolTip='<%# String.Format("Seleccionar Alternativa {0}", Eval("alternativa")) %>'
              ImageUrl="~/Styles/images/ic_amec.png" OnCommand="ImageButton_Command" CommandArgument='<%#Eval("alternativa")%>'  />
          </td>
          <td style="width: 0px">
            <asp:Label ID="ID" runat="server" Text='<%# Eval("idtramitacion") %>' Visible="false" />
          </td>
          <td style="width: 30px">
            <asp:Label ID="ImporteLabel" runat="server" Text='<%# Eval("pvp") %>' />
          </td>
          <td style="width: 150px">
            <asp:Label ID="ProveedorLabel" runat="server" Text='<%# Eval("proveedor") %>' />
          </td>
          <td style="width: 30px">
            <asp:Label ID="ValidezLabel" runat="server" Text='<%# Eval("validez") %>' />
          </td>
          <td style="width: 100px">
            <asp:Label ID="GastosLabel" runat="server" Text='<%# Eval("gastos") %>' />
          </td>
          <td style="width: 360px">
            <asp:Label ID="ObservacionesLabel" runat="server" Text='<%# Eval("observaciones") %>' />
          </td>
        </tr>
      </ItemTemplate>
      <LayoutTemplate>
        <table class="eosTablaResultadosShort">
          <thead>
            <tr>
              <th>
              </th>
              <th>
              </th>
              <th>
                IMPORTE
              </th>
              <th>
                PROVEEDOR
              </th>
              <th>
                VALIDEZ
              </th>
              <th>
                GASTOS
              </th>
              <th>
                DESCRIPCIÓN DEL SERVICIO
              </th>
            </tr>
          </thead>
          <tbody>
            <tr id="itemPlaceholder" runat="server">
            </tr>
          </tbody>
        </table>
      </LayoutTemplate>
    </asp:ListView>
  </ContentTemplate>
</asp:UpdatePanel>
