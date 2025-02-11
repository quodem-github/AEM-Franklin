<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pruebas.aspx.cs" Inherits="EOS.public.Pruebas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="float: left; width: 40%;">
            <div>
                <label>Token</label>
                <br />
                <asp:TextBox runat="server" ID="txtHash"></asp:TextBox>
            </div>
            <div>
                <label>Data</label>
                <br />
                <asp:TextBox runat="server" TextMode="MultiLine" Width="50%" Rows="5" ID="txtData"></asp:TextBox>
            </div>
            <br />
            <br />
            <asp:Button runat="server" ID="btnEncrypt" Text="Encriptar" OnClick="btnEncrypt_OnClick" />
            <asp:Button runat="server" ID="btnDecrypt" Text="Desencriptar" OnClick="btnDecrypt_OnClick" />
            <br />
            <br />
            <div>
                <label>Resultado</label>
                <br />
                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtResult" Width="95%" Rows="30"></asp:TextBox>
            </div>
        </div>

        <div style="float: right; width: 55%;">
            <p>Identificadores de las empresas</p>
            <ul>
                <li>
                    <p>AMEX: c4626375-01fe-43df-a329-5379f69a69cb</p>
                </li>
                <li>
                    <p>MT: 9ac3c3af-34f5-4400-8ef7-b9e51f0f91dd</p>
                </li>
                <li>
                    <p>GP: 06c0bf42-f19e-4747-bf64-95729f497e49</p>
                </li>
            </ul>



            <p>Datos de actualización:</p>
            <ul>
                <li>
                    <p>Actualización de ActividadesPassengerList:</p>
                    <pre>{ "idactividadpassengerlist": 100000001, "idservicioactividad": 100000000,"idpassengerlist": 212857,"locked": 0}</pre>
                </li>
                <li>
                    <p>Actualización de amec:</p>
                    <pre>{"CurrentPageIndex": 1,"idamec": 1,"amec": "test","IdEmpresa": 1,"idpeticionactividad": 1,"IdCongreso": 1,"inactivo": 1}</pre>
                </li>
                <li>
                    <p>Actualización de Congresos:</p>
                    <pre>{"IdCongreso": -1,"Congreso": "V JORNADAS CARDIOVASCULARES SEMERGEN","IdTipoCongreso": 8,"IdPoblacion": 300034361,"Desde": "2017-01-25T18:00:00","Hasta": "2017-01-25T22:30:00","IdEmpresa": 1257,"FCierre": "2017-01-25T22:30:00","Codinterno": 7394,"IdProveedor": 2545,"Idespecialidad": 139,"Urlcongreso": "www.asociacionemdr.org/es/categorias.php","Telemergencias": "-","farmaindustria_valoracion": "PENDIENTE PROGRAMA CIENTIFICO","Locked": 0,"internacional": 1,"Emailsecretaria": "email@secretaria.com","IdPeticionario": 1564,"Fechacreacion": "2017-01-25T22:30:00","Comunicar": 0,"Idvaloracionfi": 3,"publicar": 1,"AutorizadoMSDI": 0}</pre>
                </li>
                <li>
                    <p>Actualización de EmpleadosGp:</p>
                    <pre>{ "Id": 1604, "Idempleadogp": "1","locked": 0,"Apellido": "testApellido","Email": "testEmail@test.com","Nombre": "testNombre"}</pre>
                </li>
                <li>
                    <p>Actualización de estadosreservas:</p>
                    <pre>{ "idestado": "1604", "estado": "3","locked": 0}</pre>
                </li>
                <li>
                    <p>Actualización de estados_reservas:</p>
                    <pre>{"idregistre": -1,"Idestadoinicial": "AB","Idestadofinal": "CR","Enviado": 0,"Fechacambioestado": "2016-12-30T15:15:30","Locked": 1,"Tipo": "INS","Idreserva": 300006022,"Idexpediente": 14650,"wait_ack": 3,"Datasync": "2016-12-30T15:15:30","Sync": 3,"transaction_key": "e6a608345f07456f8ddc6238d8468350","Idservicio": 9,"xml_data": "test","enviado_agencia": 0,"idtramitacion": 1}</pre>
                </li>
                <li>
                    <p>Actualización de Expediente:</p>
                    <pre>{"Idxpediente": 300004178,"Idamec": "100000018","Expediente": "prueba1","IdPeticionario": 10783,"IdTipoReserva": 1,"IdEmpresa": 1257,"Idestado": "AB","Fechacreacion": "2016-05-25T15:10:25","empleadogp": "ABE","Locked": 1,"Codexpediente": "3756 993410 72003","Urgente": 0,"importeTotal": 398.51,	"visible": 1,"iddepartament": 3,"idsaleforce": 5,"iddistrict": 15}</pre>
                </li>
                <li>
                    <p>Actualización de Gestorinvitados:</p>
                    <pre>{"IdGestorInvitados": -1,"IdEventoFormulario": 425,"DescripcionGestor": "REUNION MSD","FechaInicio": "2016-05-23T00:00:00","FechaFin": "2016-05-25T00:00:00","Poblacion": "Madrid","LinkGestorInvitados": "https: //forms.com/index.aspx?idEvento=1488","Amec": "100002202","TipoGestorInvitados": "FV","Inactivo": 0,"locked": 1}</pre>
                </li>
                <li>
                    <p>Actualización de hotelpassengerlist:</p>
                    <pre>{ "idhotelpassengerlist": 1, "idserviciohotel": 1111,"idpassengerlist": 1111,"locked": 0}</pre>
                </li>
                <li>
                    <p>Actualización de Iatas:</p>
                    <pre>{"IdIata": "ACC","IdPoblacion": 100057703,"Iata": "test Quodem change","Locked": 1}</pre>
                </li>
                <li>
                    <p>Actualización de NivelesAprobacion:</p>
                    <pre>{"Idtarifaactividad": 100000008,"Fkidcongreso": 100003177,"Idtipoactividadcongreso": 1,"IdProveedor": 100008370,"Idproducto": "AUD","Actividad": "TALLER","Pvp": 130,"Notascli": "taller de urología","Prepago": 1,"Socio": 0,"Cancelacion": "gggggg","Locked": 0,"Fechainicio": "2014-01-29T00:00:00","Fechafin": "2014-02-05T00:00:00","Descripcion": "TALLER2"}</pre>
                </li>
                <li>
                    <p>Actualización de InscripcionPassengersList:</p>
                    <pre>{ "idinspassengerlist": -1, "idpassengerlist": 218764,"idservicioinscripcion": 300001255,"locked": 0}</pre>
                </li>

                <li>
                    <p>Actualización de Población:</p>
                    <pre>{"IdPoblacion": 416,"Poblacion": "ADEJE CASCO","IdProvincia": "TF","IdPais": "E","CodPostal": "38670","locked": 1,"IdPaisABC": 407,"inactivo": 1}</pre>
                </li>
                <li>
                    <p>Actualización de Productos:</p>
                    <pre>{"idproducto": "TST","producto": "Test Quodem","inactivo": 1,"locked": 1}</pre>
                </li>
                <li>
                    <p>Actualización de productos_prv:</p>
                    <pre>{"IdProductoPrv": 101211546,"FKIdProveedor": 100034617,"DesProducto": "test Quodem edit","inactivo": 0,"idproducto": "HOC","locked": 0}</pre>
                </li>

                <li>
                    <p>Actualización de PeticionActividades:</p>
                    <pre>{"idpeticionactividad": 100008542,"nombre": "PQR","desde": "2016-02-05T00:00:00","hasta": "2016-02-05T00:00:00","IdPoblacion": 100015421,"IdCongreso": 100105786,"idespecialidad": 14,"sede": "test quodem","IdTipoCongreso": 3,"locked": 0,"internacional": 1,"url_web": "test quodem","email_secretaria": "test quodem","especialidad": "test quodem","fechacreacion": "2016-02-05T00:00:00","IdPeticionario": 1,"comunicar": 1,"valoracion_farmaindustria": "test quodem"}</pre>
                </li>

                <li>
                    <p>Actualización de PeticionGrupo:</p>
                    <pre>{"Idpeticiongrupo": -1,"Idevento": 555,"Idasistente": 26356,"Idexpediente": 300003427,"Idamec": "100005225","Evento": "QUODEM edit","fecha_inicio_evento": "2013-04-25T00:00:00","tipo_gasto": "Hotel","importe_peticion": 67.5,"fecha_peticion": "2013-04-25T00:00:00","estado_peticion": "Finalizado","Idpeticionario": 300002026,"Fkidcongreso": 8465,"Numpedido": "8100883868","ultima_actualizacion": "2013-04-25T00:00:00","Codespecialidad": "ESP","estado_expediente": "Cerrado","fecha_expediente": "2013-04-25T00:00:00","Producto": "Ninguno","porcentaje_prodcuto": "75","Idvalfi": 0,"fecha_fin_evento": "2013-04-25T00:00:00","Idtipoactividadcongreso": 0,"Idestadoreserva": "CN","justificaciones_datosadicionales": "Contratado por MSD para desarrollar a nivel local una session.","ficherogenesis_datosadicionales": "NO","iddepartament": 100,"idsaleforce": null,"iddistrict": null,"idposition": null,"idtipoactividad_datosadicionales": null,"idtipoasistente_datosadicionales": null,"idnivelriesgo_datosadicionales": null}</pre>
                </li>

                <li>
                    <p>Actualización de proveedores:</p>
                    <pre>{ "IdProveedor": 300000003,"IdTipoPrv": "CIA","CodCia":  "IB","CodAmadeus": "75","Enlace": "40000000816","Proveedor": "quodem  edit","Direccion": "Velazquez 130","Nro": "130","Piso": "3º","IdPoblacion": "28605","CodPostal": "28042","Telefono": "913298100","Fax": "987 218821","Email": "test@test.com","http": "www.test.com","Locked": 1}</pre>
                </li>
                <li>
                    <p>Actualización de provincias:</p>
                    <pre>{"IdProvincia": "TST","Provincia": "QUODEM","IdPais": "E","IdComunidad": 14,"locked": 1}</pre>
                </li>
                <li>
                    <p>Actualización de ReservaPassengerList:</p>
                    <pre>{"Idxpediente": 300004176,"Idreservapassengerlist": -1,"Idpassengerlist": 218764,"Locked": 1}</pre>
                </li>
                <li>
                    <p>Actualización de ReservasViajes:</p>
                    <pre>{"idreserva": -1,"reserva": "Nombre reserva1","fechapeticion": "2011-03-28T20:31:56","idestado": "CNTR","LastUpd": "2011 - 03 - 28 T20: 31: 56","LastLog": "Valor prueba","IdPeticionario": 10783,"Observaciones": "LA INSCRIPCION ES DE SOCIO","observ_agencia": "GASTOS DE CANCELACION COBRADOS","mainreserva": "asdf","fkidexpediente": 300004176,"Locked": 1}</pre>
                </li>
                <li>
                    <p>Actualización de Servicios:</p>
                    <pre>{ "IdServicio": "test1", "IdNumerico": 1,"Servicio": "test1","locked": 1}</pre>
                </li>
                <li>
                    <p>Actualización de ServiciosReservasActividades:</p>
                    <pre>{"idservicioactividad": 300002608,"idtarifaactividad": null,"observaciones": "test quodem2","pvp": 150,"locked": 0,"sede": "PUERTO DE SAGUNTO","Descripcion": "DESAYUNO","tipo": "Catering","fechainicio": "2011-05-24","horainicio": "08","minutosinicio": "30","fechafin": "2011-05-24T00:00:00","horafin": "12","minutosfin": "15","pax": 10}</pre>
                </li>
                <li>
                    <p>Actualización de ServiciosReservasHotel:</p>
                    <pre>{"idserviciohotel": 100000001,"fechahorallegada": "2011-03-25T00:00:00","fechahorasalida": "2011-03-29T00:00:00","IdPais": "E","pais": "ESPAÑA","IdProvincia": "AB","provincia": "ALBACETE","IdPoblacion": 24157,"poblacion": "pedro HELLIN","Idproveedor": 100000830,"hotel": "HOTEL NH ATLANTICO","IdTipoAloj": 3,"categoria": "","observaciones": "hotel cerca hospital","idtipohab": "DUI","num_habitaciones": 1,"desc_tipoalojamiento": "","desc_idtipo_habitacion": "","idtarifaaloj": 100000095,"pvp": 100.50,"Locked": 1}</pre>
                </li>
                <li>
                    <p>Actualización de ServicioReservasInscripcion:</p>
                    <pre>{"idservicioinscripcion": -1,"inscripcion": "Edit test","envioboletin": "testq","tipoinscripcion": "Cupo MSD","Otros": "Cupo MSD Cupo MSD","observaciones": "Cuota reducida especial MSD","observ_agencia": "Cuota reducida","iddatosentrega": null,"idtarifainscripcion": null,"pvp": 696.2,"locked": 1}</pre>
                </li>
                <li>
                    <p>Actualización de ServiciosReservasViajes:</p>
                    <pre>{"Idservicio": 300006037,"idreserva": 300006038,"IdTipoBono": "ACT","fechapeticion": "2015-11-07T00:00:00","resumenservicio": "tested","Cotizado": 0,"idservicioactividad": null,"locked": 0,"importeReserva": 181.5}</pre>
                </li>
                <li>
                    <p>Actualización de ServicioReservasTransporte:</p>
                    <pre>{"Idserviciotransporte": 100000072,"IdTipoBono_ida1": "AIR","ida1_fechasalida": "2011-09-28T00:00:00","ida1_origen": "QuodemOrigen","ida1_destino": "QuodemDestino","ida1_numvuelo_tren": "IB8977","ida1_horasalida": "19:55","ida1_horallegada": "19:55","IdTipoBono_ida2": "AIR","ida2_fechasalida": "2011-09-28T00:00:00","ida2_origen": "MADRID","ida2_destino": "OVIEDO","ida2_numvuelo_tren": "IB8648","ida2_horasalida": "19:55","ida2_horallegada": "19:55","IdTipoBono_reg1": "AIR","reg1_fechasalida": "2011-09-28T00:00:00","reg1_origen": "OVIEDO","reg1_destino": "MADRID","reg1_numvuelo_tren": "IB475","reg1_horasalida": "19:55","reg1_horallegada": "19:55","IdTipoBono_reg2": "AIR","reg2_fechasalida": "2011-09-28T00:00:00","reg2_origen": "MADRID","reg2_destino": "VALENCIA","reg2_numvuelo_tren": "IB8976","reg2_horasalida": "19:55","reg2_horallegada": "19:55","importe_max": 750,"observaciones_ida": "Primera hora de la mañana","observaciones_reg": "Ultima hora de la tarde","observaciones": "","gastos_cancelacion": "","ida": "","regreso": "","locked": 0}</pre>
                </li>
                <li>
                    <p>Actualización de TarifasActividad:</p>
                    <pre>{"Idtarifaactividad": 100000008,"Fkidcongreso": 100003177,"Idtipoactividadcongreso": 1,"IdProveedor": 100008370,"Idproducto": "AUD","Actividad": "TALLER","Pvp": 130,"Notascli": "taller de urología","Prepago": 1,"Socio": 0,"Cancelacion": "gggggg","Locked": 0,"Fechainicio": "2014-01-29T00:00:00","Fechafin": "2014-02-05T00:00:00","Descripcion": "TALLER2"}</pre>
                </li>
                <li>
                    <p>Actualización de TarifasAloj:</p>
                    <pre>{"IdTarifaAloj": 200000432,"FKIdCongreso": 200005549,"IdTipoAloj": 3,"Descripcion": "test - HOTEL AYRE ALFONSO II","IdProveedor": 200001904,"Idproducto": "HON","IdTipoHab": "DUI","IdServicio": null,"PrecioNoche": 140,"Prepago": 1,"Socio": 0,"Fechainicio": "2011-05-03T00:00:00","Fechafin": "2011-05-04T00:00:00","Cancelación": "100 % de gastos si se cancela despues del 6 de feb","Locked": 0,"visible": 1}</pre>
                </li>

                <li>
                    <p>Actualización de TarofasInscripcion:</p>
                    <pre>{"IdTarifaInscripcion": -1,"FKIdCongreso": 100004218,"IdTipoInscripcion": 1,"IdProveedor": 100003466,"PVP": 750,"NotasCli": "PRECIO EN DOLARES","Idproducto": "SEC","Prepago": 1,"Socio": 0,"Cancelación": "VALORADO EN DOLARES!!!","Locked": 0,"fecha_validez": "2016-09-16T00:00:00","Fechainicio": "2016-07-16T00:00:00","Fechafin": "2016-09-16T00:00:00","descripcion": "INDUSTRIA"}</pre>
                </li>
                <li>
                    <p>Actualización de TiposAloj:</p>
                    <pre>{"IdTipoAloj": 4,"TipoAloj": "TEST1","locked": 1}</pre>
                </li>
                <li>
                    <p>Actualización de TipoBonos:</p>
                    <pre>{ "IdTipoBono": "te1", "TipoBono": "test1","locked": 0}</pre>
                </li>
                <li>
                    <p>Actualización de TiposPRV:</p>
                    <pre>{ "IdTipoPrv": "AAA","TipoProveedor": "test2","IdTipoBono":  "VAR","Locked": 1}</pre>
                </li>

                <li>
                    <p>Actualización de TramitacionesServiciosReservas:</p>
                    <pre>{"Idtramitacion": 300004551,"Fkidservicio": 300005947,"Línea": 1,"Requerimientos": "test: 62 congreso nacional de la SEORL","alternativa1": "Inscrpción ESPECIALISTA","okalternativa1": "AC","alternativa2": "Inscripción RESIDENTE,menor 35 años ","okalternativa2": "NA","Idestado": "ACP","pvp1": 149.25,"pvp2": 170.01,"requerimientos2": "test","IdProveedor1": null,"IdProveedor2": null,"IdProductoPrv1": 89872,"IdProductoPrv2": 39093,"Validez1": "11/07/2011","Validez2": "11/08/2011","gastoscancelacion1": "VUELOS EMITIDOS.","gastoscancelacion2": "caduca 30 jun 13: 00","locked": 0}</pre>
                </li>
                <li>
                    <p>Actualización de TransportePassengersList:</p>
                    <pre>{ "idtransportepassengerlist": -1, "idserviciotransporte": 100000001,"idpassengerlist": 212857,"locked": 1}</pre>
                </li>


            </ul>


            <p>Filtros:</p>
            <ul>
                <li>
                    <p>Filtro Expedientes:</p>
                    <pre>{"CurrentPageIndex": 1,"CreationDateFrom": "20161020","CreationDateTo": "20161231", "IdExpediente":-1,"Amec":"300002194" }</pre>
                </li>
                <li>
                    <p>Filtro ReservaPassengerList:</p>
                    <pre>{"CurrentPageIndex": 1,"IdExpediente":300003819,"IdReservaPassengersList":-1,"IdPassengerlist":-1 }</pre>
                </li>
                <li>
                    <p>Filtro TarifasActividad:</p>
                    <pre>{"CurrentPageIndex": 1,"IdTarifaActividad":-1,"IdCongreso":100066583 }</pre>
                    <pre>{"CurrentPageIndex": 1,"IdTarifaActividad":100000004,"IdCongreso":-1 }</pre>
                </li>
                <li>
                    <p>Filtro ActividadesPassengersList:</p>
                    <pre>{"CurrentPageIndex": 1,"IdActividadPassengerList":100000001,"IdExpediente":-1, "IdPassengerlist": -1 }</pre>
                    <pre>{"CurrentPageIndex": 1,"IdActividadPassengerList":-1,"IdExpediente":300000155, "IdPassengerlist": -1 }</pre>
                </li>
                <li>
                    <p>Filtro Congresos:</p>
                    <pre>{"CurrentPageIndex": 1,"CongresoDateFrom": "20161020","CongresoDateTo": "20161231", "IdCongreso":-1,"Congreso":"" }</pre>
                    <pre>{"CurrentPageIndex": 1,"CongresoDateFrom": "","CongresoDateTo": "", "IdCongreso":-1,"Congreso":"As" }</pre>
                </li>
                <li>
                    <p>Filtro PassengerList:</p>
                    <pre>{"CurrentPageIndex": 1,"LastUpdateDateFrom": "20161116","LastUpdateDateTo": "20161117"}</pre>
                </li>
                <li>
                    <p>Filtro Peticionarios:</p>
                    <pre>{"CurrentPageIndex": 1,"LastUpdateDateFrom": "20161116","LastUpdateDateTo": "20161117"}</pre>
                </li>
                <li>
                    <p>Filtro TarifasInscripcion:</p>
                    <pre>{"CurrentPageIndex": 1, "IdTarifaInscripcion": 1053,"IdCongreso": 17548}</pre>
                </li>
                <li>
                    <p>Filtro TarifasAloj:</p>
                    <pre>{"CurrentPageIndex":1, "IdTarifaAloj":100000005,"IdCongreso":100006536}</pre>
                </li>
                <li>
                    <p>Filtro Amec:</p>
                    <pre>{"idamec": 355555861,"amec": "test","IdEmpresa": 1257,"idpeticionactividad": 300106019,"IdCongreso": 100110887,"inactivo": 1}</pre>
                </li>
                <li>
                    <p>Filtro InscripcionPassengersList:</p>
                    <pre>{"CurrentPageIndex": 1,"IdInsPassengerlist":300000281,"IdExpediente":-1, "IdPassengerlist": -1 }</pre>
                </li>
                <li>
                    <p>Filtro TransportePassengersList:</p>
                    <pre>{ "CurrentPageIndex": 1,"IdTransportePassengersList": -1,"IdExpediente":-1,"IdPassengerlist": -1}</pre>
                    <pre>{ "CurrentPageIndex": 1,"IdTransportePassengersList": 300000047,"IdExpediente":-1,"IdPassengerlist": -1}</pre>
                </li>
                <li>
                    <p>Filtro PeticionActividad:</p>
                    <pre>{ "CurrentPageIndex": 1,"IdPeticionActividad": -1}</pre>
                    <pre>{ "CurrentPageIndex": 1,"PeticionActividadDateFrom": "20160101",  "PeticionActividadDateTo": "20161201","IdPeticionActividad": 1053,"PeticionActividad": "SIMPOSIUM INTERNACIONAL"}</pre>
                </li>
                <li>
                    <p>Filtro Poblacion:</p>
                    <pre>{ "CurrentPageIndex": 1, "IdPoblacion": -1, "Poblacion": "Madrid"}</pre>
                    <pre>{ "CurrentPageIndex": 1, "IdPoblacion": 42867, "Poblacion": "Madrid"}</pre>
                </li>
                <li>
                    <p>Filtro Proveedor:</p>
                    <pre>{ "CurrentPageIndex": 1, "IdProveedor": -1,"Proveedor": "AN"}</pre>
                    <pre>{ "CurrentPageIndex": 1, "IdProveedor": 1,"Proveedor": "AN"}</pre>
                </li>
                <li>
                    <p>Filtro ReservasViajes:</p>
                    <pre>{"Idreserva": 300006034,"reserva": "Nombre reserva222","fechapeticion": "2011-03-28T20:31:56","idestado": "AB","LastUpd": "2011-03-28T20:31:56","LastLog": "Valor prueba","IdPeticionario": "10783","Observaciones": "LA INSCRIPCION ES DE SOCIO","observ_agencia": "GASTOS DE CANCELACION COBRADOS","mainreserva": "asdf","fkidexpediente": "300004172","Locked": 1}</pre>
                </li>
                <li>
                    <p>Filtro ServiciosReservasActividades:</p>
                    <pre>{ "CurrentPageIndex": 1, "IdExpediente": -1,"IdServicio": -1,"IdReserva": -1,"IdServicioActividad": -1}</pre>
                </li>
                <li>
                    <p>Filtro ServiciosReservasHotel:</p>
                    <pre>{ "CurrentPageIndex": 1, "IdExpediente": -1,"IdServicio": -1,"IdReserva": -1,"IdServicioHotel": -1}</pre>
                </li>
                <li>
                    <p>Filtro ServiciosReservasInscripcion:</p>
                    <pre>{ "CurrentPageIndex": 1, "IdExpediente": -1,"IdServicio": -1,"IdReserva": -1,"IdServicioInscripcion": -1}</pre>
                </li>
                <li>
                    <p>Filtro ServiciosReservasTransporte:</p>
                    <pre>{ "CurrentPageIndex": 1, "IdExpediente": -1,"IdServicio":-1,"IdReserva": -1,"IdServicioTransporte": -1}</pre>
                </li>
                <li>
                    <p>Filtro ServiciosReservasViajes:</p>
                    <pre>{ "CurrentPageIndex": 1,"IdExpediente": 300003819,"IdServicio": -1,"IdReserva": -1}</pre>
                </li>
                <li>
                    <p>Filtro Amecs:</p>
                    <pre>{"CurrentPageIndex": 1,"IdAmecs": "100018663"}</pre>
                </li>
                <li>
                    <p>Filtro Comunidad:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro ConfEmpresa:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro CriteriosSelecion:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro DatosAdicionalesReservasPassenger:</p>
                    <pre>{ "CurrentPageIndex": 1,"IdExpediente": 100015900,"IdPassengerlist": 268659}</pre>
                </li>
                <li>
                    <p>Filtro Districts:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro Departaments:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro EmpleadosGP:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro Especialidades:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro Estados_Reservas:</p>
                    <pre>{ "CurrentPageIndex": 1, "IdRegistre": 100000017,"IdEstadoInicial": "CR","IdEstadoFinal": "AN","IdReserva": 100000017,"IdExpediente": 16,"Sync": 3,"TransactionKey": "cdf4cbf900204b189eb9c91d09e938ff","IdServicio": 17 }</pre>
                </li>
                <li>
                    <p>Filtro HotelPassengerList:</p>
                    <pre>{ "CurrentPageIndex": 1,"IdHotelPassengerlist": -1,"IdEstado": "NC","IdExpediente": -1,"IdPassengerlist": -1}</pre>
                </li>
                <li>
                    <p>Filtro Iatas:</p>
                    <pre>{ "CurrentPageIndex": 1,"IdIata": "","Iata": "LANZAROTE"}</pre>
                </li>
                <li>
                    <p>Filtro JustificacionesDatosAdicionales:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro MensajeReservasPassengerDatosAdicionales:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro NivelesRiesgoHCP:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro NivelesRiesgoHCPDatosAdicionales:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro NivelesAprobacion:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro Productos:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro ProductosPRV:</p>
                    <pre>{ "CurrentPageIndex": 1,"IdProductoPrv": -1,"FKIdProveedor": -1,"IdProducto": "AZA" }</pre>
                </li>
                <li>
                    <p>Filtro SalesForce:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro Servicios:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro TipoServicio:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro TipoActividad:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro TipoActividadCongreso:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro TiposPRV:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro TiposPatrocinio:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro TiposReservaWeb:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro TramitacionesServiciosReservas:</p>
                    <pre>{ "CurrentPageIndex": 1, "IdExpediente": -1,"IdServicio": -1,"IdReserva": -1,"IdTramitacion": 100000085}</pre>
                </li>
                <li>
                    <p>Filtro ValoracionFi:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro TipoActividadPaxDatosAdicionales:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro TipoAsistenteDatosAdicionales:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro TiposBonos:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro Tratamientos:</p>
                    <pre>Sin Filtro</pre>
                </li>
                <li>
                    <p>Filtro Gestor de invitados:</p>
                    <pre>{"CurrentPageIndex": 1, "CongresoDateFrom": "20150101", "CongresoDateTo": "20160101","IdGestorInvitados": 300000001, "IdCongreso": 300000001, "IdEventoFormulario" : 361 }</pre>
                </li>
                <li>
                    <p>Filtro AprobadorAmec:</p>
                    <pre>{"CurrentPageIndex": 1,"IdAprobador":10685,"Idamecs":400000006 }</pre>
                </li>
                <li>
                    <p>Filtro EstructuraOrganizativa:</p>
                    <pre>{"CurrentPageIndex": 1,"UpdateDateFrom":"" ,"UpdateDateTo":"" , IdPeticionario: -1, IdPeticionarioManager: 1159, Wein: "", WeinManager: "25610460" }</pre>
                </li>
            </ul>

        </div>

    </form>
</body>
</html>
