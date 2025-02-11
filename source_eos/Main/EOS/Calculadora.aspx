<%@ Page Title="" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true"
    CodeBehind="Calculadora.aspx.cs" Inherits="EOS.Calculadora" %>

<asp:Content ID="Content2" ContentPlaceHolderID="eosHeaderBotonera" runat="server">
    &nbsp;
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="eosContentFilter" runat="server">
    <script type="text/javascript" src="Scripts/calculadora.js"></script>

    <div id="idHonorariosMaximos" style="display: none; width: 800px; height: 780px; z-index: 1000"
        runat="server">
        <div class="eosTituloAMEC">
            <br />
            HONORARIOS MÁXIMOS SEGÚN POLÍTICA
        </div>
        <table class="eosTablaResultados honorarios" style="background: white; margin: auto; margin-top: 10px; width: 95%;">
            <thead>
                <tr>
                    <th>Actividad
                    </th>
                    <th>Importe máximo bruto
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptHonorariosMaximos">
                    <ItemTemplate>
                        <tr>
                            <td colspan="<%# Eval("Colspan")%>" <%# int.Parse(Eval("Colspan").ToString()) != 1 ? "align='center'" : "" %>>
                                <%# Eval("Text")%>
                            </td>
                            <asp:PlaceHolder runat="server" Visible='<%# int.Parse(Eval("Colspan").ToString()) == 1 %>'>
                                <td>
                                    <%# Server.HtmlDecode(Eval("Value").ToString()) %>
                                </td>
                            </asp:PlaceHolder>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <div class="eosTituloAMEC" style="font-size: 1.8em">
            <br />
            Casos especiales en los que se estima un importe estándar
        </div>
        <table class="eosTablaResultados honorarios" style="background: white; margin: auto; margin-top: 10px; width: 95%;">
            <thead>
                <tr>
                    <th>Actividad
                    </th>
                    <th>Importe estándar bruto
                    </th>
                    <th>Importe máximo bruto
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptHonorariosMaximosSpecialCase">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("Text")%>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(Eval("Value").ToString()) %>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(Eval("Value2").ToString()) %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr style="height: 80px;">
                    <td colspan="3">
                        <div class="eosDivCentrada">
                            <img onclick="$('#eosContentFilter_idHonorariosMaximos').hide();return false;" src="/Styles/images/bt_cerrar.png" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="idCountryToCountry" style="display: none; width: 800px; height: 780px; z-index: 1000"
        runat="server">
        <div class="eosTituloAMEC" style="font-size: 1.6em">
            <br />
            HONORARIOS PONENTES NACIONALES DENTRO DEL PROCESO COUNTRY TO COUNTRY
        </div>
        <table class="eosTablaResultados honorarios" style="background: white; margin: 10px 12px 10px 20px; width: 45%; float: left;">
            <thead>
                <tr>
                    <th colspan="2">Máximos
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x=>x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode()).Text %>
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptMaximosNonGlobal">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("TableText")%>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdMaxValue")).ToString("N2"))%>
                                <%# Server.HtmlDecode(Eval("IdMaxValueAppend").ToString())%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <table class="eosTablaResultados honorarios" style="background: white; margin: 10px 10px 10px 20px; width: 45%; float: left;">
            <thead>
                <tr>
                    <th colspan="2">Máximos
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode()).Text%>
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptMaximosGlobal">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("TableText")%>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdMaxValue")).ToString("N2"))%>
                                <%# Server.HtmlDecode(Eval("IdMaxValueAppend").ToString())%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <table class="eosTablaResultados honorarios" style="background: white; margin: 10px 10px 10px 20px; width: 93%;">
            <thead>
                <tr>
                    <th rowspan="2" class="hiddenCell"></th>
                    <th colspan="2">
                        <%= CountryToCountryObj.CountryToCountryNumSpeaksTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).Text%>
                    </th>
                    <th colspan="2">
                        <%= CountryToCountryObj.CountryToCountryNumSpeaksTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.SomeSpeaks.GetHashCode()).Text%>
                    </th>
                </tr>
                <tr>
                    <th>Estandar
                    </th>
                    <th>Máx.
                    </th>
                    <th>Estandar
                    </th>
                    <th>Máx.
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode()).Text%>
                    </td>
                    <asp:Repeater runat="server" ID="rptStandarCountryToCountryNonGlobal">
                        <ItemTemplate>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdStandarValue")).ToString("N2"))%>
                                <%# Server.HtmlDecode(Eval("IdStandarValueAppend").ToString())%>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdMaxValue")).ToString("N2"))%>
                                <%# Server.HtmlDecode(Eval("IdMaxValueAppend").ToString())%>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
                <tr>
                    <td>
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode()).Text%>
                    </td>
                    <asp:Repeater runat="server" ID="rptStandarCountryToCountryGlobal">
                        <ItemTemplate>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdStandarValue")).ToString("N2"))%>
                                &nbsp;<%# Server.HtmlDecode(Eval("IdStandarValueAppend").ToString())%>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdMaxValue")).ToString("N2"))%>
                                &nbsp;<%# Server.HtmlDecode(Eval("IdMaxValueAppend").ToString())%>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
            </tbody>
        </table>
        <table class="eosTablaResultados honorarios" style="background: white; margin: 10px 5px 10px 20px; width: 72%; float: left;">
            <thead>
                <tr>
                    <th colspan="3" class="hiddenCell">Travelling
                    </th>
                    <th>
                        <%= CountryToCountryObj.CountryToCountryNumSpeaksTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.SomeSpeaks.GetHashCode()).Text%>
                    </th>
                    <th>Día sin ponencia
                    </th>
                    <th>Adicional
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td rowspan="2">
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode()).Text%>
                    </td>
                    <td>1ª ciudad
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.FirstCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.FirstCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.FirstCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.SomeSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdDaysNoSpeak
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).Aditional
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).AditionalAppend
                        %>
                    </td>
                </tr>
                <tr>
                    <td>Ciudades consecutivas
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                                            x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.SomeSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdDaysNoSpeak
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).Aditional
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() && 
                                x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).AditionalAppend
                        %>
                    </td>
                </tr>
                <tr>
                    <td rowspan="2">
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode()).Text%>
                    </td>
                    <td>1ª ciudad
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.FirstCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.FirstCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.FirstCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.SomeSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdDaysNoSpeak
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).Aditional
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).AditionalAppend
                        %>
                    </td>
                </tr>
                <tr>
                    <td>Ciudades consecutivas
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                                            x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.SomeSpeaks.GetHashCode()).IdStandarValue
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdCitiesType == EOS.ServiceLogic.Enums.ECountryToCountryTypesCities.NextCities.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).IdDaysNoSpeak
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                    </td>
                    <td>
                        <%= ((long)CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).Aditional
                            ).ToString("N2")%>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x =>
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).IdStandarValueAppend
                        %>
                        <%= CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Find(
                            x => 
                                x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Travelling.GetHashCode() &&
                                                            x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() && 
                                x.IdNumSpeaksModeratorsType == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()
                            ).AditionalAppend
                        %>
                    </td>
                </tr>
            </tbody>
        </table>
        <table class="eosTablaResultados honorarios" style="background: white; margin: 10px 10px 10px 10px; width: 20%; float: left;">
            <thead>
                <tr>
                    <th>Motivos de excepción
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptExceptionReasons">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("Text") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <table class="eosTablaResultados honorarios" style="background: white; margin: 10px 10px 10px 20px; width: 93%;">
            <thead>
                <tr>
                    <th rowspan="2" class="hiddenCell">COP (Programa / actividad dentro<br />
                        del mismo centro de trabajo)
                    </th>
                    <th colspan="2">
                        <%= CountryToCountryObj.CountryToCountryNumSpeaksModerTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.UnicSpeaks.GetHashCode()).Text%>
                    </th>
                    <th colspan="2">
                        <%= CountryToCountryObj.CountryToCountryNumSpeaksModerTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.SomeSpeaks.GetHashCode()).Text%>
                    </th>
                </tr>
                <tr>
                    <th>Estandar
                    </th>
                    <th>Máx.
                    </th>
                    <th>Estandar
                    </th>
                    <th>Máx.
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode()).Text%>
                    </td>
                    <asp:Repeater runat="server" ID="rptCopCountryToCountryNonGlobal">
                        <ItemTemplate>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdStandarValue")).ToString("N2"))%>
                                <%# Server.HtmlDecode(Eval("IdStandarValueAppend").ToString())%>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdMaxValue")).ToString("N2"))%>
                                <%# Server.HtmlDecode(Eval("IdMaxValueAppend").ToString())%>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
                <tr>
                    <td>
                        <%= CountryToCountryObj.CountryToCountryGlobalTypeDtoList.Find(x => x.Id == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode()).Text%>
                    </td>
                    <asp:Repeater runat="server" ID="rptCopCountryToCountryGlobal">
                        <ItemTemplate>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdStandarValue")).ToString("N2"))%>
                                &nbsp;<%# Server.HtmlDecode(Eval("IdStandarValueAppend").ToString())%>
                            </td>
                            <td>
                                <%# Server.HtmlDecode(((long)Eval("IdMaxValue")).ToString("N2"))%>
                                &nbsp;<%# Server.HtmlDecode(Eval("IdMaxValueAppend").ToString())%>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
                <tr style="height: 80px;">
                    <td colspan="5">
                        <div class="eosDivCentrada">
                            <img onclick="$('#eosContentFilter_idCountryToCountry').hide();return false;" src="/Styles/images/bt_cerrar.png" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="idModuloParticipantes" style="width: 800px;" runat="server">
        <br />
        <div class="eosTituloAMEC">
            DATOS ADICIONALES PARTICIPANTE
        </div>
        <div id="eosContentAMECErrores">
            <asp:ValidationSummary ID="ContenHonorarioValidationSummary" runat="server" CssClass="failureNotification"
                ValidationGroup="ValidarHonorariosGroup" />
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
        </div>
        <input type="hidden" id="Participante" name="Participante" />
        <table class="eosTablaFiltros" style="background: white; margin: auto; margin-top: 2%; width: 95%;">
            <tr>
                <td style="padding-left: 20px; width: 200px">
                    <asp:Label runat="server"> Tipo de Participante</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTipoAsistente" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW220"
                        DataTextField="tipoasistente" DataValueField="idtipoasistente">
                    </asp:DropDownList>
                </td>
            </tr>
            <asp:ScriptManager ID="MainScriptManager" runat="server" EnableScriptGlobalization="True"
                EnablePartialRendering="true">
            </asp:ScriptManager>
            <!-- *************************************** Calculadora Speaker *************************************** -->

            <asp:HiddenField runat="server" ID="hiddenPonentesNivelPSList" ClientIDMode="Static" />

            <tr id="idtrCalculadora_Speaker_Chair" class="calculadora" style="display: none">
                <td colspan="2" style="padding: 6px 0 0 13px; background: #8abdbe;">
                    <span class="eosTituloNormal" style="color: #FFFFFF">FMV / CÁLCULO HONORARIOS - Ponentes
                        y Moderadores</span>
                </td>
            </tr>
            <tr id="idtrCalculadora_Speaker_Chair_Contenido" class="calculadora" style="display: none">
                <td colspan="2" style="background: #c3e0e0;">
                    <asp:UpdatePanel runat="server" ID="updatePanel">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td style="padding-left: 20px; width: 200px">
                                                    <asp:Label ID="lblSpeakerChairTipoReunion" runat="server">Tipo de Reunión</asp:Label><span
                                                        class="eosCampoObligatorio">*</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlSpeakerChairTipoReunion" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                                        OnSelectedIndexChanged="ddlSpeakerChairTipoReunion_Change" AutoPostBack="true"
                                                        DataTextField="Tipo" DataValueField="id" Style="word-break: break-word">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvddlSpeakerChairTipoReunion" runat="server" ErrorMessage="Campo Obligatorio: Tipo de Reunión"
                                                        ControlToValidate="ddlSpeakerChairTipoReunion" Enabled="False" ValidationGroup="ValidarHonorariosGroup"
                                                        SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                                <asp:PlaceHolder runat="server" ID="plhTipoPonente" Visible="true">
                                                    <td style="padding-left: 20px;" id="idPlhTipoPonente_1">
                                                        <asp:Label ID="Label22" runat="server">Tipo de Ponente</asp:Label><span class="eosCampoObligatorio">*</span>
                                                    </td>
                                                    <td id="idPlhTipoPonente_2">
                                                        <asp:RadioButtonList ID="rblTipoPonente" runat="server" RepeatDirection="Horizontal"
                                                            CssClass="floatLeft" DataTextField="Tipo" DataValueField="id" AutoPostBack="true"
                                                            OnSelectedIndexChanged="rblTipoPonente_change">
                                                        </asp:RadioButtonList>
                                                        <asp:RequiredFieldValidator ID="rfvRblTipoPonente" runat="server" ErrorMessage="Campo Obligatorio: Tipo de Ponente"
                                                            ControlToValidate="rblTipoPonente" Enabled="False" ValidationGroup="ValidarHonorariosGroup"
                                                            SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                                                    </td>
                                                </asp:PlaceHolder>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td style="padding-left: 20px; width: 200px">
                                                    <asp:Label ID="lblSpeakerChairTipoPS" runat="server">Tipo de P.S.</asp:Label><span
                                                        class="eosCampoObligatorio">*</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlSpeakerChairTipoPS" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                                        DataTextField="Text" DataValueField="id" Style="word-break: break-word" OnSelectedIndexChanged="ddlSpeakerChairTipoPS_Change"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvddlSpeakerChairTipoPS" runat="server" ErrorMessage="Campo Obligatorio: Tipo de P.S."
                                                        ControlToValidate="ddlSpeakerChairTipoPS" Enabled="False" ValidationGroup="ValidarHonorariosGroup"
                                                        SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                                                </td>
                                                <td style="padding-left: 20px;">
                                                    <asp:Label ID="lblSpeakerChairFeeHoraTitle" runat="server">Fee / hora</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSpeakerChairFeeHora" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW120"
                                                        ReadOnly="True"></asp:TextBox>€
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 20px;">
                                        <asp:Label ID="lblSpeakerChairDuracionActividad" runat="server">Duración actividad</asp:Label><span
                                            class="eosCampoObligatorio">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSpeakerChairDuracionActividad" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                            OnSelectedIndexChanged="ddlSpeakerChairDuracionActividad_Change" AutoPostBack="true"
                                            DataTextField="Text" DataValueField="id" Style="word-break: break-word">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlSpeakerChairDuracionActividad" runat="server"
                                            ErrorMessage="Campo Obligatorio: Duración actividad" ControlToValidate="ddlSpeakerChairDuracionActividad"
                                            Enabled="False" ValidationGroup="ValidarHonorariosGroup" SetFocusOnError="True"
                                            ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 20px;">
                                        <asp:Label ID="lblSpeakerChairTiempoPreparacion" runat="server">Tiempo preparación</asp:Label><span
                                            class="eosCampoObligatorio">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSpeakerChairTiempoPreparacion" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                            OnSelectedIndexChanged="ddlSpeakerChairTiempoPreparacion_Change" AutoPostBack="true"
                                            DataTextField="Text" DataValueField="id" Style="word-break: break-word">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlSpeakerChairTiempoPreparacion" runat="server"
                                            ErrorMessage="Campo Obligatorio: Tiempo preparación" ControlToValidate="ddlSpeakerChairTiempoPreparacion"
                                            Enabled="False" ValidationGroup="ValidarHonorariosGroup" SetFocusOnError="True"
                                            ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <!-- -->
                                <tr>
                                    <td colspan="2" style="padding-left: 20px;">
                                        <asp:CheckBox ID="chkBoxPonenciaCentroSalud" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                            OnCheckedChanged="chkBoxPonenciaCentroSalud_Change" AutoPostBack="true" Style="word-break: break-word"></asp:CheckBox>
                                        <asp:Label ID="Label19" runat="server">Ponencia en reunión en centro de salud, hospital, EM, ... (Duración aprox. 60 min.)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-left: 20px;">
                                        <asp:CheckBox ID="chkBoxTalleres" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                            OnCheckedChanged="chkBoxTalleres_Change" AutoPostBack="true" Style="word-break: break-word"></asp:CheckBox>
                                        <asp:Label ID="Label20" runat="server">Mas de una ponencia en un día con el mismo contenido (Talleres) (Media o una jornada de trabajo)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-left: 20px;">
                                        <asp:CheckBox ID="chkBoxVideoconferenciasRepetidas" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                            OnCheckedChanged="chkBoxVideoconferenciasRepetidas_Change" AutoPostBack="true"
                                            Style="word-break: break-word"></asp:CheckBox>
                                        <asp:Label ID="Label21" runat="server">Repetición de Videoconferencia. Para este tipo de reuniones se entenderá que el tiempo de preparación es 0.</asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="idtrCalculadora_Speaker_Chair_Cierre" class="calculadora" style="display: none">
                <td colspan="2" style="padding: 10px 0px 10px 13px; height: 0px; background: #8abdbe; color: #ffffff;">
                    <table>
                        <tr>
                            <td style="width: 550px; padding: 0px 0px 0px 25px; height: 0px;">
                                <asp:Label ID="Label13" runat="server">Cálculo Honorarios según FMV <a id="idtrCalculadora_Speaker_Chair_a_show" onclick="javascript:toggleCalculadoraSpeaker(this, '#idtrCalculadora_Speaker_Chair_a_hide', true);"><img src="/Styles/images/down_inactive.png"/></a>
                                        <a id="idtrCalculadora_Speaker_Chair_a_hide" onclick="javascript:toggleCalculadoraSpeaker(this, '#idtrCalculadora_Speaker_Chair_a_show', false);" style="display: none;"><img src="/Styles/images/up_inactive.png"/></a></asp:Label>
                            </td>
                            <td style="padding: 0px; height: 0px;">
                                <asp:UpdatePanel runat="server" ID="updPanelSpeakerHonorariosMaximos" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:PlaceHolder runat="server" ID="plhCalculadoraSemaforoVerde" Visible="false">
                                            <img src="Styles/images/ic_farma_verde.png" class="imgCalculadoraSemaforo" />
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" ID="plhCalculadoraSemaforoNaranja" Visible="false">
                                            <img src="Styles/images/ic_farma_naranja.png" class="imgCalculadoraSemaforo" />
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" ID="plhCalculadoraSemaforoRojo" Visible="false">
                                            <img src="Styles/images/ic_farma_rojo.png" class="imgCalculadoraSemaforo" />
                                        </asp:PlaceHolder>
                                        <asp:TextBox ID="txtSpeakerChairHonorariosMaximos" runat="server" CssClass="eosDisabledInputVacio eosInputVacio calculator eosSizeW120"
                                            ReadOnly="True"></asp:TextBox>€&nbsp;&nbsp;&nbsp;
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Speaker_Chair_Comment_1" style="display: none;">
                            <td colspan="2" style="padding: 0px; height: 0px;" align="center">
                                <span align="center">(*) Nota 1: Estos honorarios son estimados, pudiendo acordarse
                                    una cantidad menor si se estima adecuado</span>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Speaker_Chair_Comment_2" style="display: none;">
                            <td colspan="2" style="padding: 0px; height: 0px;" align="center">
                                <span align="center">(*) Nota 2: Según la política, los honorarios de los Moderadores
                                    aparecen ya calculados a un máximo del 80% del máximo permitido para ponentes</span>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Speaker_Chair_Comment_3" style="display: none;">
                            <td colspan="2" style="padding: 0px; height: 0px;" align="center">
                                <span align="center">(*) Nota 3: Para Ponentes / Moderadores dentro del proceso Country
                                    to Country emplear la pestaña correspondiente <a id="link_tabla_honorarios_country_to_country"
                                        href="#" style="color: #fd6817">LINK directo a la tabla</a> </span>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Speaker_Chair_Comment_4" style="display: none;">
                            <td colspan="2" style="padding: 5px 0px 5px 10px; height: 0px;" align="center">
                                <table border="2px" style="border-color: #fff;">
                                    <tr>
                                        <td rowspan="4" style="padding: 0px 25px 0px 25px;">
                                            <span align="center">Código de colores</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 0px 25px 0px 25px;">Verde: Importe por debajo del máximo permitido
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 0px 25px 0px 25px;">Naranja: Posiblemente por encima del máximo. Comprobar en tabla
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 0px 25px 0px 25px;">Rojo: Supera el máximo permitido en cualquier supuesto
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Speaker_Chair_Comment_5" style="display: none;">
                            <td colspan="2" style="padding: 0px; height: 0px;" align="center">
                                <asp:Label ID="Label14" runat="server">Antes de confirmar el Honorario, Comprobar con la tabla Honorarios Máximos <a id="link_tabla_honorarios_maximos_1" href="#" style="color: #fd6817">LINK directo a la tabla</a> </asp:Label>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Speaker_Chair_Comment_6" style="display: none;">
                            <td colspan="2" style="padding: 0px; height: 0px;" align="center">
                                <asp:Label ID="Label15" runat="server"><a href="http://ts1.merck.com/com/global_compliance_organization/Pages/Compliance-Standards-Page.aspx" target="_blank" style="color: #fd6817">Acceso a listado GEMS</a></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!-- *************************************** Fin Calculadora Speaker *************************************** -->
            <!-- *************************************** Calculadora Consultoria *************************************** -->
            <tr id="idtrCalculadora_Consultoria" class="calculadora" style="display: none">
                <td colspan="2" style="padding: 6px 0 0 13px; background: #8abdbe;">
                    <span class="eosTituloNormal" style="color: #FFFFFF">FMV / CÁLCULO HONORARIOS - Consultoría</span>
                </td>
            </tr>
            <tr id="idtrCalculadora_Consultoria_Contenido" class="calculadora" style="display: none">
                <td colspan="2" style="background: #c3e0e0;">
                    <asp:UpdatePanel runat="server" ID="updatePanelConsultoria">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td style="padding-left: 20px; width: 200px">
                                        <asp:Label ID="lblConsultoriaTipoReunion" runat="server">Tipo de contrato</asp:Label><span
                                            class="eosCampoObligatorio">*</span>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblConsultoriaTipoReunion" runat="server" RepeatDirection="Horizontal"
                                            OnSelectedIndexChanged="rblConsultoriaTipoReunion_Change" CssClass="floatLeft"
                                            AutoPostBack="true" DataTextField="Text" DataValueField="Value">
                                            <asp:ListItem Text="Día Consultor" Value="1" Selected="true" />
                                            <asp:ListItem Text="Contrato consultoría anual (contínua)" Value="0" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <asp:PlaceHolder runat="server" ID="plhTxtConsultoriaDiaConsultor" Visible="True">
                                    <tr id="idtrCalculadora_Consultoria_numdias">
                                        <td style="padding-left: 20px; width: 200px">
                                            <asp:Label ID="Label18" runat="server">Número de días</asp:Label><span class="eosCampoObligatorio">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtConsultoriaDiaConsultor" runat="server" CssClass="eosSizeW120"
                                                AutoPostBack="True" onkeyup="javascript:CalculateHonorariosConsultoria();"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationGroup="ValidarHonorariosGroup"
                                                ControlToValidate="txtConsultoriaDiaConsultor" ValidationExpression="[0-9]" ErrorMessage="El número de días no es numérico" />
                                        </td>
                                    </tr>
                                </asp:PlaceHolder>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="idtrCalculadora_Consultoria_Cierre" class="calculadora" style="display: none">
                <td colspan="2" style="padding: 10px 0px 10px 13px; height: 0px; background: #8abdbe; color: #ffffff;">
                    <table>
                        <tr>
                            <td style="width: 550px; padding: 0px 0px 0px 25px; height: 0px;">
                                <asp:Label ID="Label7" runat="server">Cálculo Honorarios según FMV <a id="idtrCalculadora_Consultoria_a_show" onclick="javascript:toggleConsultoriaInfo(this, '#idtrCalculadora_Consultoria_a_hide', true);"><img src="/Styles/images/down_inactive.png"/></a>
                                            <a id="idtrCalculadora_Consultoria_a_hide" onclick="javascript:toggleConsultoriaInfo(this, '#idtrCalculadora_Consultoria_a_show', false);" style="display: none;"><img src="/Styles/images/up_inactive.png"/></a></asp:Label>
                            </td>
                            <td style="padding: 0px; height: 0px;">
                                <div>
                                    <asp:UpdatePanel runat="server" ID="updPanelConsultoriaHonorariosMaximos" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtConsultoriaHonorariosMaximos" runat="server" CssClass="eosDisabledInputVacio eosInputVacio calculator eosSizeW120"
                                                ReadOnly="True"></asp:TextBox>€&nbsp;&nbsp;&nbsp;
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Consultoria_Comment_1" style="display: none;">
                            <td colspan="2" style="padding: 0px; height: 0px;" align="center">
                                <span align="center">(*) Nota 1: Estos honorarios son estimados, pudiendo acordarse
                                    una cantidad menor si se estima adecuado</span>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Consultoria_Comment_2" style="display: none;">
                            <td colspan="2" style="padding: 0px; height: 0px;" align="center">
                                <span align="center">(*) Nota 2: Según la política, los honorarios de los Moderadores
                                    aparecen ya calculados a un máximo del 80% del máximo permitido para ponentes</span>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Consultoria_Comment_3" style="display: none;">
                            <td colspan="2" style="padding: 0px; height: 0px;" align="center">
                                <span align="center">(*) Nota 3: Para Ponentes / Moderadores dentro del proceso Country
                                    to Country emplear la pestaña correspondiente <a id="link_tabla_honorarios_country_to_country_1"
                                        href="#" style="color: #fd6817">LINK directo a la tabla</a> </span>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Consultoria_Comment_4" style="display: none;">
                            <td colspan="2" style="padding: 5px 0px 5px 10px; height: 0px;" align="center">
                                <table border="2px" style="border-color: #fff;">
                                    <tr>
                                        <td rowspan="4" style="padding: 0px 25px 0px 25px;">
                                            <span align="center">Código de colores</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 0px 25px 0px 25px;">Verde: Importe por debajo del máximo permitido
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 0px 25px 0px 25px;">Naranja: Posiblemente por encima del máximo. Comprobar en tabla
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 0px 25px 0px 25px;">Rojo: Supera el máximo permitido en cualquier supuesto
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Consultoria_Comment_5" style="display: none;">
                            <td colspan="2" style="padding: 0px; height: 0px;" align="center">
                                <asp:Label ID="Label16" runat="server">Antes de confirmar el Honorario, Comprobar con la tabla Honorarios Máximos <a id="link_tabla_honorarios_maximos_3" href="#" style="color: #fd6817">LINK directo a la tabla</a> </asp:Label>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Consultoria_Comment_6" style="display: none;">
                            <td colspan="2" style="padding: 0px; height: 0px;" align="center">
                                <asp:Label ID="Label17" runat="server"><a href="http://ts1.merck.com/com/global_compliance_organization/Pages/Compliance-Standards-Page.aspx" target="_blank" style="color: #fd6817">Acceso a listado GEMS</a></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!-- *************************************** Fin Calculadora Consultoria *************************************** -->
            <!-- *************************************** Calculadora Eif & AB *************************************** -->
            <tr id="idtrCalculadora_Eif_AB" class="calculadora" style="display: none">
                <td colspan="2" style="padding: 6px 0 0 13px; background: #8abdbe;">
                    <span class="eosTituloNormal" style="color: #FFFFFF">FMV / CÁLCULO HONORARIOS - EIF
                        y Advisory boards</span>
                </td>
            </tr>
            <tr id="idtrCalculadora_Eif_AB_Contenido" class="calculadora" style="display: none">
                <td colspan="2" style="background: #c3e0e0;">
                    <asp:UpdatePanel runat="server" ID="updatePanel1">
                        <ContentTemplate>
                            <table>
                                <tr style="display: none">
                                    <td style="padding-left: 20px; width: 200px">
                                        <asp:Label ID="lblEifABTipoPS" runat="server">Tipo de P.S.</asp:Label><span class="eosCampoObligatorio">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEifABTipoPS" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                            DataTextField="Text" DataValueField="id" Style="word-break: break-word">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 20px; width: 200px">
                                        <asp:Label ID="lblEifABFeeHoraTitle" runat="server">Fee / hora</asp:Label><span class="eosCampoObligatorio">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEifABFeeHora" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW120"
                                            ReadOnly="True"></asp:TextBox>€
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 20px;">
                                        <asp:Label ID="Label8" runat="server">Duración actividad</asp:Label><span class="eosCampoObligatorio">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEifABDuracionActividad" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                            OnSelectedIndexChanged="ddlEifABDuracionActividad_Change" AutoPostBack="true"
                                            DataTextField="Text" DataValueField="id" Style="word-break: break-word">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlEifABDuracionActividad" runat="server" ErrorMessage="Campo Obligatorio: Duración actividad"
                                            ControlToValidate="ddlEifABDuracionActividad" Enabled="False" ValidationGroup="ValidarHonorariosGroup"
                                            SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 20px;">
                                        <asp:Label ID="Label9" runat="server">Tiempo preparación</asp:Label><span class="eosCampoObligatorio">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEifABTiempoPreparacion" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW530"
                                            OnSelectedIndexChanged="ddlEifABTiempoPreparacion_Change" AutoPostBack="true"
                                            DataTextField="Text" DataValueField="id" Style="word-break: break-word">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlEifABTiempoPreparacion" runat="server" ErrorMessage="Campo Obligatorio: Tiempo preparación"
                                            ControlToValidate="ddlEifABTiempoPreparacion" Enabled="False" ValidationGroup="ValidarHonorariosGroup"
                                            SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="idtrCalculadora_Eif_AB_Cierre" class="calculadora" style="display: none">
                <td colspan="2" style="padding: 3px 0px 3px 13px; height: 0px; background: #8abdbe; color: #ffffff;">
                    <table>
                        <tr>
                            <td style="width: 550px; padding: 0px 0px 0px 25px; height: 0px;">
                                <asp:Label ID="Label10" runat="server">Cálculo Honorarios según FMV                                         <a id="idtrCalculadora_Eif_AB_a_show" onclick="javascript:toggleCalculadoraEif_AB(this, '#idtrCalculadora_Eif_AB_a_hide', true);"><img src="/Styles/images/down_inactive.png"/></a>
                                        <a id="idtrCalculadora_Eif_AB_a_hide" onclick="javascript:toggleCalculadoraEif_AB(this, '#idtrCalculadora_Eif_AB_a_show', false);" style="display: none;"><img src="/Styles/images/up_inactive.png"/></a></asp:Label>
                            </td>
                            <td style="padding: 0px; height: 0px;">
                                <asp:UpdatePanel runat="server" ID="updPanelEifAbHonorariosMaximos" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtEifABHonorariosMaximos" runat="server" CssClass="eosDisabledInputVacio eosInputVacio calculator eosSizeW120"
                                            ReadOnly="True"></asp:TextBox>€&nbsp;&nbsp;&nbsp;
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <asp:Literal runat="server" ID="litChairmanAditionalEuros" Text="500" Visible="false"></asp:Literal>
                        </tr>
                        <tr id="idtrCalculadora_Eif_AB_Comment_1" style="display: none">
                            <td colspan="2" style="padding: 0px; height: 0px;" align="center">
                                <span align="center">(*) Nota: Estos honorarios son estimados, pudiendo acordarse una
                                    cantidad menor si se estima adecuado</span>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Eif_AB_Comment_2" style="display: none">
                            <td colspan="2" style="padding: 0px; height: 0px;" align="center">
                                <span align="center">(*) Nota: Se le añaden 500 euros al cálculo por su especial dedicación
                                    en la preparación del evento.</span>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Eif_AB_Comment_3" style="display: none">
                            <td colspan="2" style="padding: 0px; height: 0px;" align="center">
                                <asp:Label ID="Label11" runat="server">Antes de confirmar el Honorario, Comprobar con la tabla Honorarios Máximos <a id="link_tabla_honorarios_maximos_2" href="#" style="color: #fd6817">LINK directo a la tabla</a> </asp:Label>
                            </td>
                        </tr>
                        <tr id="idtrCalculadora_Eif_AB_Comment_4" style="display: none">
                            <td colspan="2" style="padding: 0px; height: 0px;" align="center">
                                <asp:Label ID="Label12" runat="server"><a href="http://ts1.merck.com/com/global_compliance_organization/Pages/Compliance-Standards-Page.aspx" target="_blank" style="color: #fd6817">Acceso a listado GEMS</a></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!-- *************************************** Fin Calculadora Eif & AB *************************************** -->
            <tr style="display: none">
                <td>
                    <asp:UpdatePanel runat="server" ID="updHonorarioMaximo" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField runat="server" ID="hidFieldIdValueHonorariosMaximos" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hidFieldValueHonorariosMaximos" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hidFieldIdValueHonorariosMaximosConsultoria"
                                ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hidFieldValueHonorariosMaximosConsultoria" ClientIDMode="Static" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <asp:ImageButton ID="btnCancelarDatosUsuario" Visible="True" runat="server" ImageUrl="~/Styles/images/bt_cancelar.png" />
                </td>
            </tr>
        </table>
        <center>
            <asp:Image CssClass="ajaxImageClass" Style="visibility: hidden" ID="ajaxImage" runat="server"
                ImageUrl="~/Styles/images/ajax-loader.gif" Title="Cargando..." />
            <br />
        </center>
    </div>
</asp:Content>
