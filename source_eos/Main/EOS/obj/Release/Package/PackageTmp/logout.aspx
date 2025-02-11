<%@ Page Title="" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="EOS.Logout" %>



<asp:Content ID="closePlanel" ContentPlaceHolderID="eosContentFilter" runat="server">
    <script type="text/javascript">
        window.onload = function () {
            var close = document.getElementById('eosHeaderPersonal');
                if (close) {
                    close.style.display = 'none';
            }};
    </script>
    <div class="eosTituloWizard">
        <h2>Fin sesión</h2>
        <p>Si tiene alguna pregunta, por favor contacte al administrador del sitio. </p>
    </div>
</asp:Content>

