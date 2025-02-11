<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParticipantesSelect.ascx.cs"
  Inherits="EOS.Controls.ParticipantesSelect" %>

<asp:UpdatePanel ID="SelectPanel" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
    <table class="contentTableAsistents">
      <tr>
        <td colspan="5">
          &nbsp;
        </td>
      </tr>
      <tr>
        <td style="width: 33.3%">
          <b>Participantes disponibles</b>
        </td>
        <td style="width: 20px">
        </td>
        <td style="width: 33.3%">
          <b>Participantes seleccionados <span style="color: red">*</span></b>
        </td>
        <td style="width: 20px">
        </td>
        <td style="width: 33.3%">
          <b>Observaciones</b>
        </td>
      </tr>
      <tr>
        <td>
          <asp:ListBox ID="lstAvailable" runat="server"  SelectionMode="Multiple"
            CssClass="eosDisabledInputVacio style1"></asp:ListBox>
        </td>
        <td style="text-align: center">
          <asp:ImageButton ID="btnSelect" Width="20px" runat="server" ImageUrl="~/Styles/images/ic_participante_flecha.png"
            OnClick="btnSelect_Click" CssClass="contentIconSelect" />
        </td>
        <td>
          <asp:ListBox ID="lstSelected" runat="server" SelectionMode="Multiple"
            CssClass="eosDisabledInputVacio style1"></asp:ListBox>
        </td>
        <td style="text-align: center">
          <asp:ImageButton ID="btnDeselect" Width="20px" runat="server" ImageUrl="~/Styles/images/ic_participante_aspa.png"
            OnClick="btnDeselect_Click" CssClass="contentIconSelect" />
        </td>
        <td>
          <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" 
            CssClass="eosDisabledInputVacio style1"></asp:TextBox>
        </td>
      </tr>
    </table>
  </ContentTemplate>
  <Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnSelect" />
    <asp:AsyncPostBackTrigger ControlID="btnDeselect" />
  </Triggers>
</asp:UpdatePanel>
