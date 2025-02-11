<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contacto.aspx.cs" Inherits="EOS.Contacto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Repeater runat="server" ID="rptAgencias">
            <ItemTemplate>
                <div>
                    <strong>
                        <asp:Label ID="lblNombreAgencia" runat="server" Text='<%#Eval("NombreAgencia") %>'></asp:Label>
                    </strong>
                    <br />
                    <asp:Label ID="lblNombreContacto" runat="server" Text='<%#Eval("NombreContacto") %>'></asp:Label>
                    <br />
                    <asp:Label ID="lblDireccion" runat="server" Text='<%#Eval("Direccion") %>'></asp:Label>
                    <br />
                    <asp:Label ID="lblPoblacion" runat="server" Text='<%# string.Format("{0} {1}", Eval("CodPostal"), Eval("Poblacion") ) %>'></asp:Label>
                    <br />
                    <asp:Label ID="lblMailContacto" runat="server" Text='<%#Eval("MailContacto") %>'></asp:Label>
                    <br />
                    <asp:Label ID="lblTelefonoAgencia" runat="server" Text='<%#string.Format("Tel: {0}", Eval("TelefonoAgencia")) %>'></asp:Label>
                    <br />
                    <asp:Label ID="lblFaxAgencia" runat="server" Text='<%#string.Format("Tel: {0}", Eval("FaxAgencia")) %>'></asp:Label>
                    <br/>
                    <br/>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
