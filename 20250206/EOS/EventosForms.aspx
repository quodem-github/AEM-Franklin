<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventosForms.aspx.cs" Inherits="EOS.EventosForms" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/ecmascript" language="javascript">
        function redirect() {
            var frm = document.forms['eventosForm'];

            if (!frm) {
                frm = document.eventosForm;
            }

            frm.submit();
        }
    </script>
</head>
<body onload="redirect()">
    <form runat="server" visible="false">
    <div>
        <asp:HiddenField ID="IdEvento" runat="server" />
        <asp:HiddenField ID="IdUser" runat="server" />
        <asp:HiddenField ID="EosToken" runat="server" />
    </div>
    </form>
    <form id="eventosForm" method="<%=FormMethod %>" action="<%=FormAction %>">
        <input type="hidden" id="IdEvento" name="IdEvento" value="<%=IdEvento.Value %>" />
        <input type="hidden" id="IdUser" name="IdUser" value="<%=IdUser.Value %>" />
        <input type="hidden" id="EosToken" name="EosToken" value="<%=EosToken.Value %>" />
        <%--<input type="submit" value="Continuar" />--%>
    </form>
</body>
</html>
