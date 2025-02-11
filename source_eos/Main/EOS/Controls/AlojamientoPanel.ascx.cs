using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using EOS.Entidades.Datos;
using EOS.Web;


namespace EOS.Controls
{
    public partial class AlojamientoPanel : UserControl
    {
        #region Variables

        private string idExpediente;
        private string idServicioReserva;
        private string idReserva;
        private string idServicio;
        private string idCongreso;
        private string idUsuario;
        private string idPoblacion;
        private string idPoblacionAux;
        private string idTab;
        private string state;
        private string statename;
        private bool showAlternative;

        private class poblacion
        {
            public int idPoblacion { get; set; }
            public string nombre { get; set; }
        }

        #endregion Variables

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            //1702/2011 EAS.
            //TipoActividad tipoactividad = (TipoActividad)Session["tipoactividad"];
            idTab = Request.QueryString.Get("tab");
            if (idTab != "1")
                return;

            showAlternative = (Request.QueryString["view"] != null && Request.QueryString["view"] == "alternativa");
            idExpediente = Request.QueryString.Get("idexp");
            idServicioReserva = Request.QueryString.Get("idres");
            //Ismael Ameller 21-03-2011 Cambio a la hora de obtener el IdReservaServicio
            if (Request.QueryString.Get("idres") == null)
            {
                idServicioReserva = Convert.ToString(0);
            }
            //FIN Ismael Ameller 21-03-2011 Cambio a la hora de obtener el IdReservaServicio
            idReserva = GetReservaByServicio();
            idServicio = idServicioReserva;
            idCongreso = HttpContext.Current.Session["currentFKIdCongreso"].ToString();
            DVCongresos congreso = (new AgenteExpedientes()).ObtenerCongreso(Int32.Parse(idCongreso));
            idUsuario = Convert.ToString(((DDatosPersonalesUsuario)HttpContext.Current.Session["DatosUsuario"]).IdPeticionario);

            btnEnviar.Visible = !string.IsNullOrEmpty(idReserva);
            Alternativas.IdServicio = idServicio;

            Test.IdExpediente = idExpediente;
            Test.IdServicio = idServicio;
            

            idPoblacionAux = Convert.ToString(congreso.IdPoblacion);

            if (!IsPostBack)
            {
                idPoblacion = Convert.ToString(congreso.IdPoblacion);  // GetEventCity();
                llegadaInputDatePickerControl.Value = congreso.Desde.ToString().Substring(0, 10);
               salidaInputDatePickerControl.Value = congreso.Hasta.ToString().Substring(0, 10);

                SqlConnection connIdconf = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
                connIdconf.Open();
                string consultaIdConfEmpresa = string.Format("select idconfempresa from amec where idamec = (select top 1 idamec from expediente where idxpediente = {0})", idExpediente);
                SqlCommand commandIdConfEmpresa = new SqlCommand(consultaIdConfEmpresa, connIdconf);
                string idConfEmpresa = commandIdConfEmpresa.ExecuteScalar().ToString();
                connIdconf.Close();

                // Recupera las poblaciones
                GetCities(idConfEmpresa);

                // Recupera la ciudad del congreso
                if (idPoblacion != "")
                    poblacionDropDownList.SelectedValue = idPoblacion;

                //Ismael Ameller 21-03-2011 Ponemos en el combo por defecto 'Hoteles secretaría siempre y cuando haya almenos un hotel'
                tipoDropDownList.SelectedIndex = 2;
                // Filtro inicial de hoteles
                //GetHotels();
                if (hotelDropDownList.Items.Count == 0)
                {
                    tipoDropDownList.SelectedIndex = 0;
                    //GetHotels();
                }
                else
                {
                    MofificaTipo();
                }
                //FIN Ismael Ameller 21-03-2011 Ponemos en el combo por defecto 'Hoteles secretaría siempre y cuando haya almenos un hotel'
                // Recupera tipos de habitación
                GetRoomTypes();

                // Muestra alternativas
                Alternativas.Visible = showAlternative;
                btnEnviar.Visible = !showAlternative;
                btnGuardar.Visible = !showAlternative;

                llegadaInputDatePickerControl.Enabled = !showAlternative;
                tipoDropDownList.Enabled = !showAlternative;
                salidaInputDatePickerControl.Enabled = !showAlternative;
                poblacionDropDownList.Enabled = !showAlternative;
                hotelDropDownList.Enabled = !showAlternative;
                hotelTextBox.Enabled = !showAlternative;
                tipoHabitacionDropDownList.Enabled = !showAlternative;
                Test.Enabled = !showAlternative;
                SetEditTarifa();
                GetHotels(idConfEmpresa);
            }

            //Actualizar Boton Segun corresponda
            GetRecord();
            SetButtons();

            // Muestra el estado
            lblEstado.Text = statename;
            lblEstado.CssClass = string.Format("eosImagenEstado eosImagenLeyenda{0}", state);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        public void Guardar()
        {
            // Control de errores y validación
            if (DateTime.Parse(llegadaInputDatePickerControl.Value) > DateTime.Parse(salidaInputDatePickerControl.Value))
            {
                ShowMessage("La fecha de salida no puede ser anterior a la fecha de llegada.", null);
                return;
            }

            if (precioNocheMaximoTextBox.Text != "")
                try
                {
                    Double d;
                    Double.TryParse(precioNocheMaximoTextBox.Text, out d);
                }
                catch (Exception ex)
                {
                    //Ismael Ameller 09-03-2011 Envio de Mail
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                    //FIN Ismael Ameller 09-03-2011 Envio de Mail
                    ShowMessage("El precio por noche máximo debe ser un valor numérico.", null); return;
                }

            if (llegadaInputDatePickerControl.Value != "" &&
                salidaInputDatePickerControl.Value != "" &&
                Test.NumPassengers > 0)
            {
                // Comprueba si es una creación o una modificación
                if (string.IsNullOrEmpty(idReserva))
                {
                    idReserva = Convert.ToString(GetNextId("reservasviajes", "idReserva"));
                    CreateRecord();

                    // Pau Ferrer  09-06-2011 #No se debe mostrar el boton enviar
                    //btnEnviar.Visible = true;
                    //FIN Pau

                }
                else
                {
                    UpdateRecord();
                }
            }
            else
            {
                // Mostrar mensaje campos obligatorios
                ShowMessage("Debe introducir todos los campos obligatorios indicados con un asterisco de color rojo. No se pudo guardar el nuevo servicio.", null);
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                //Jose Laguna 11-05-2012 Añadimos las validaciones del estado y del presupuesto del AMEC para saber si la reserva se puede ENVIAR/APROBAR
                AgenteMaestros agente = new AgenteMaestros();
                if (!agente.ValidarReservaEstadoAmec(int.Parse(GetServicioByReserva())))
                    ShowMessage("El estado del AMEC no permite realizar el envío del servicio", "");
                // LJM 20180124 - Eliminar restricción  
                //else if (!agente.ValidarReservaImporteAmec(int.Parse(GetServicioByReserva())))
                //    ShowMessage("El servicio no se ha podido enviar ya que el importe superaría el presupuesto máximo del AMEC.", "");
                else
                {
                    Guardar();
                    SetState("CR");

                    //Inicio Generación de reserva asociada a los fees
                    AgenteUsuarios agenteUsuFee = new AgenteUsuarios();
                    AgenteExpedientes agenteExpFee = new AgenteExpedientes();
                    DVPeticionariosRoles datosRolesFee = agenteUsuFee.ObtenerDatosRolesPorLogin();
                    var datosExpFee = agenteExpFee.ObtenerExpedientePorID(idExpediente.ToString());
                    DAmec damecFee = agenteExpFee.ObtenerEntidadAMECporID(datosExpFee.Idamec.ToString());
                    SqlConnection connFees = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
                    connFees.Open();
                    string idconfEmpresa = damecFee.idconfempresa == null ? "0" : damecFee.idconfempresa.Value.ToString();
                    string consulta = " exec dbo.sp_fee_generar_reserva " + idExpediente.ToString() + ", " + datosRolesFee.IdPeticionario + ", " + idconfEmpresa + ";";
                    SqlCommand commFees = new SqlCommand(consulta, connFees);
                    commFees = new SqlCommand(consulta, connFees);
                    commFees.ExecuteNonQuery();
                    connFees.Close();
                    //Fin Generación de reserva asociada a los fees

                    ShowMessage("Se ha enviado el servicio correctamente.", "DetalleExpediente.aspx?idexp=" + idExpediente);

                    //IAV ENVIAR MAIL
                    AgenteExpedientes agenteExp = new AgenteExpedientes();
                    DCabeceraExpedienteAmpliado expediente = agenteExp.ObtenerExpedientePorID(idExpediente);

                    Mail mail = new Mail();
                    string body = "";

                    switch (expediente.Tiporeserva)
                    {
                        case "IND":
                            body = "EXPEDIENTE INDIVIDUAL número: " + expediente.Idexpediente + "\n\n";
                            break;
                        case "COL":
                            body = "EXPEDIENTE COLECTIVO número: " + expediente.Idexpediente + "\n\n";
                            break;
                    }
                    body = body +
                                  "Tipo de Reserva: ALOJAMIENTO \n" +
                                  "Id de servicio: " + idServicio + "\n" +
                                  "LLegada: " + llegadaInputDatePickerControl.Value + "\n" +
                                  "Salida: " + salidaInputDatePickerControl.Value + "\n" +
                                  "Hotel: " + hotelDropDownList.SelectedItem.Text + "\n" +
                                  "Tipo Habitacion: " + tipoHabitacionDropDownList.SelectedItem.Text + "\n" +
                                  "Poblacion: " + poblacionDropDownList.SelectedItem.Text + "\n" +
                                  "Precio Máximo por noche: " + precioNocheMaximoTextBox.Text + " euros";

                    if (Test.NumPassengers > 0)
                    {
                        string pasajeros = Test.ListaPasajeros;
                        body = body + "\n" + "Pasajeros: " + pasajeros + "\n";
                    }
                    else
                    {
                        body = body + "\n" + "Pasajeros: No hay ninguno seleccionado";
                    }
                    if (!string.IsNullOrEmpty(Test.Observaciones))
                    {
                        body = body + "Observaciones pasajeros: " + Test.Observaciones + "\n";
                    }
                    AgenteUsuarios agenteUsu = new AgenteUsuarios();
                    DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                    var datosExp = new AgenteExpedientes().ObtenerExpedientePorID(idExpediente);

                    datosRoles.idempleadogp = agenteExp.ObtenerEmpleadoGpPorPetAmecExp(datosRoles.IdPeticionario, datosExp.Amec, datosExp.Idexpediente).IdEmpleadoGp;
                    DAmec damec = agenteExp.ObtenerEntidadAMECporID(datosExp.Idamec.ToString());
                    mail.EnvioReservaMail(body, idExpediente, datosRoles.idempleadogp, true, ConfigUtil.GetAppSetting(Constantes.AppParams.CopiaContacto), damec.idconfempresa == null ? 0 : damec.idconfempresa.Value);
                }


            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
            }


        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            SetState("CN");
            ShowMessage("Se ha cancelado el servicio correctamente.", "DetalleExpediente.aspx?idexp=" + idExpediente);
        }

        protected void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                //Jose Laguna 11-05-2012 Añadimos las validaciones del estado y del presupuesto del AMEC para saber si la reserva se puede ENVIAR/APROBAR
                AgenteMaestros agente = new AgenteMaestros();
                if (!agente.ValidarReservaEstadoAmec(int.Parse(GetServicioByReserva())))
                {
                    ShowMessage("El estado del AMEC no permite la aprobación del servicio", "");
                }
                //LJM 12/01/2018 - Eliminar restricción
                //else if (!agente.ValidarReservaImporteAmec(int.Parse(GetServicioByReserva())))
                //{
                //    ShowMessage("El servicio no se ha podido aprobar ya que el importe supereraría el presupuesto máximo del AMEC.", "");
                //}
                else
                {
                    SetState("CFP");
                    ShowMessage("Se ha aprobado el servicio correctamente.", "DetalleExpediente.aspx?idexp=" + idExpediente);
                    //IAV ENVIAR MAIL
                    AgenteExpedientes agenteExp = new AgenteExpedientes();
                    DCabeceraExpedienteAmpliado expediente = agenteExp.ObtenerExpedientePorID(idExpediente);
                    Mail mail = new Mail();
                    string body = "";

                    if (expediente.Tiporeserva == "IND")
                    {
                        body = "EXPEDIENTE INDIVIDUAL número: " + expediente.Idexpediente + "\n\n";
                    }
                    else if (expediente.Tiporeserva == "COL")
                    {
                        body = "EXPEDIENTE COLECTIVO número: " + expediente.Idexpediente + "\n\n";
                    }

                    body = body +
                                  "Tipo de Reserva: ALOJAMIENTO \n" +
                                  "Id de servicio: " + idServicio + "\n" +
                                  "LLegada: " + llegadaInputDatePickerControl.Value + "\n" +
                                  "Salida: " + salidaInputDatePickerControl.Value + "\n" +
                                  "Hotel: " + (hotelDropDownList.SelectedItem != null ? hotelDropDownList.SelectedItem.Text : string.Empty) + "\n" +
                                  "Tipo Habitacion: " + (tipoHabitacionDropDownList.SelectedItem != null ? tipoHabitacionDropDownList.SelectedItem.Text : string.Empty) + "\n" +
                                  "Poblacion: " + (poblacionDropDownList.SelectedItem != null ? poblacionDropDownList.SelectedItem.Text : string.Empty) + "\n" +
                                  "Precio Máximo por noche: " + precioNocheMaximoTextBox.Text + " euros";

                    if (Test.NumPassengers > 0)
                    {
                        string pasajeros = Test.ListaPasajeros;
                        body = body + "\n" + "Pasajeros: " + pasajeros + "\n";
                    }
                    else
                    {
                        body = body + "\n" + "Pasajeros: No hay ninguno seleccionado";
                    }

                    if (!string.IsNullOrEmpty(Test.Observaciones))
                    {
                        body = body + "Observaciones pasajeros: " + Test.Observaciones + "\n";
                    }

                    AgenteUsuarios agenteUsu = new AgenteUsuarios();
                    DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                    var datosExp = new AgenteExpedientes().ObtenerExpedientePorID(idExpediente);

                    datosRoles.idempleadogp = agenteExp.ObtenerEmpleadoGpPorPetAmecExp(datosRoles.IdPeticionario, datosExp.Amec, datosExp.Idexpediente).IdEmpleadoGp;
                    DAmec damec = agenteExp.ObtenerEntidadAMECporID(datosExp.Idamec.ToString());
                    mail.EnvioReservaMail(body, idExpediente, datosRoles.idempleadogp, false, ConfigUtil.GetAppSetting(Constantes.AppParams.CopiaContacto), damec.idconfempresa == null ? 0 : damec.idconfempresa.Value);
                }
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
            }
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            SetState("AN");
            ShowMessage("Se ha rechazado el servicio correctamente.", "DetalleExpediente.aspx?idexp=" + idExpediente);
        }

        protected void tipoDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            MofificaTipo();
        }

        //Ismael Ameller 21-03-2011 Cambio para que cargue la información correctamente al iniciar pantalla si el tipo hotel es Hoteles secretaría
        public void MofificaTipo()
        {
            // Muestra u oculta introducción manual de Hotel segun el tipo seleccionado (si Otros no listados muestra texto, en otros casos muestra combo)
            ShowOtherHotel();
            SqlConnection connIdconf = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            connIdconf.Open();
            string consultaIdConfEmpresa = string.Format("select idconfempresa from amec where idamec = (select top 1 idamec from expediente where idxpediente = {0})", idExpediente);
            SqlCommand commandIdConfEmpresa = new SqlCommand(consultaIdConfEmpresa, connIdconf);
            string idConfEmpresa = commandIdConfEmpresa.ExecuteScalar().ToString();
            connIdconf.Close();
            // Recupera ciudades
            GetCities(idConfEmpresa);

            if (idPoblacionAux != "")
            {
                try
                {
                    poblacionDropDownList.SelectedValue = idPoblacionAux;
                }
                catch
                {
                    if (poblacionDropDownList.Items.Count != 0)
                    {
                        poblacionDropDownList.SelectedValue = poblacionDropDownList.Items[0].Value;
                        idPoblacionAux = poblacionDropDownList.Items[0].Value;
                    }
                    else
                    {
                        poblacionDropDownList.Items.Clear();
                        idPoblacionAux = string.Empty;
                    }
                }
            }

            // Recupera los hoteles
            //GetHotels();

            // Recupera los tipos de habitación
            GetRoomTypes();

            // Recupera el precio máximo
            precioNocheMaximoTextBox.Text = GetHotelFee();

            SetEditTarifa();

            if (idPoblacionAux != "")
            {
                poblacionDropDownList.SelectedValue = idPoblacionAux;
            }
        }

        //FIN Ismael Ameller 21-03-2011 Cambio para que cargue la información correctamente al iniciar pantalla si el tipo hotel es Hoteles secretaría

        protected void poblacionDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection connIdconf = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            connIdconf.Open();
            string consultaIdConfEmpresa = string.Format("select idconfempresa from amec where idamec = (select top 1 idamec from expediente where idxpediente = {0})", idExpediente);
            SqlCommand commandIdConfEmpresa = new SqlCommand(consultaIdConfEmpresa, connIdconf);
            string idConfEmpresa = commandIdConfEmpresa.ExecuteScalar().ToString();
            connIdconf.Close();
            // Recupera los hoteles
            GetHotels(idConfEmpresa);

            if (tipoDropDownList.SelectedValue == "2")
            {
                // Recupera los tipos de habitación
                GetRoomTypes();

                // Recupera el precio máximo
                precioNocheMaximoTextBox.Text = GetHotelFee();
            }
        }

        protected void hotelDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tipoDropDownList.SelectedValue == "2")
            {
                // Recupera los tipos de habitación
                GetRoomTypes();

                // Recupera el precio máximo
                precioNocheMaximoTextBox.Text = GetHotelFee();
            }
        }

        protected void tipoHabitacionDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tipoDropDownList.SelectedValue == "2")
            {
                // Recupera el precio máximo
                precioNocheMaximoTextBox.Text = GetHotelFee();
            }
        }

        #endregion Events

        #region Functions

        protected void ShowOtherHotel()
        {
            // Muestra u oculta introducción manual de Hotel segun el tipo seleccionado (si Otros no listados muestra texto, en otros casos muestra combo)
            hotelTextBox.Text = "";
            hotelTextBox.Visible = tipoDropDownList.SelectedValue.Equals("1");
            hotelDropDownList.Visible = !hotelTextBox.Visible;
        }

        protected void SetEditTarifa()
        {
            precioNocheMaximoTextBox.Enabled = !showAlternative && tipoDropDownList.SelectedValue != "2";
        }

        private void GetCities(string idConfEmpresa)
        {
            System.Collections.Generic.List<poblacion> lstPoblaciones = new System.Collections.Generic.List<poblacion>();
            const int idPoblacionOtros = 1;

            // ********** Substituir por método de capas de acceso a datos
            // Consulta las poblaciones
            string consulta;
            if (tipoDropDownList.SelectedValue == "2")
                consulta = "SELECT DISTINCT PB.IdPoblacion, PB.Poblacion " +
                           "FROM tarifasaloj AL " +
                           "LEFT JOIN proveedores P " +
                           "ON AL.idproveedor = P.idproveedor " +
                           "LEFT JOIN poblaciones PB " +
                           "ON P.idpoblacion = PB.idpoblacion " +
                           "WHERE AL.FKIdCongreso = " + idCongreso + " AND " +
                           " P.IdConfEmpresa= " + idConfEmpresa + " AND " +
                           " PB.IdConfEmpresa= " + idConfEmpresa + " " +
                           " ORDER BY PB.Poblacion";
            else
                consulta = "SELECT P.IdPoblacion, P.Poblacion " +
                           "FROM poblaciones P " +
                           "WHERE ( (P.Poblacion = '---' AND P.IdConfEmpresa = " + idConfEmpresa + ") or " +
                           " P.IdPoblacion IN (SELECT DISTINCT IdPoblacion FROM proveedores WHERE  IdConfEmpresa= " + idConfEmpresa + " AND idtipoprv IN ('HOI','HOC','HON','HOT'))) " +
                           "ORDER BY P.Poblacion;";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);

            try
            {
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter {SelectCommand = new SqlCommand(consulta, conn)};
                adapter.Fill(ds, "table1");
                DataTable dt = ds.Tables[0];
                dt.DefaultView.RowFilter = "IdPoblacion <> " + idPoblacionOtros;
                lstPoblaciones.AddRange(from DataRow oDr in dt.DefaultView.ToTable().Rows select new poblacion {idPoblacion = (int) oDr["IdPoblacion"], nombre = (string) oDr["Poblacion"]});
                dt.DefaultView.RowFilter = "IdPoblacion = " + idPoblacionOtros;

                DataTable dtVista = dt.DefaultView.ToTable();
                if (dtVista.Rows.Count > 0)
                {
                    lstPoblaciones.Add(new poblacion { idPoblacion = (int)dtVista.Rows[0]["IdPoblacion"], nombre = (string)dtVista.Rows[0]["Poblacion"] });
                }

                poblacionDropDownList.DataSource = lstPoblaciones;
                poblacionDropDownList.DataTextField = "nombre";
                poblacionDropDownList.DataValueField = "idPoblacion";
                poblacionDropDownList.DataBind();

                //poblacionDropDownList.Items.Insert(poblacionDropDownList.Items.Count -1, new  ListItem("Select Employee Name", ""));
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }

        private void GetHotels(string idConfEmpresa)
        {
            // ********** Substituir por método de capas de acceso a datos
            // Consulta los hoteles disponibles segun paràmetros
            string consulta = string.Empty;

            switch (tipoDropDownList.SelectedValue)
            {
                case "0":
                    string filter = " WHERE IdConfEmpresa = "+ idConfEmpresa + " AND IdTipoPrv IN ('HON', 'HOC', 'HOT', 'HOI')" +
                                    (!string.IsNullOrEmpty(poblacionDropDownList.SelectedValue) ? " AND IdPoblacion = " + poblacionDropDownList.SelectedValue : string.Empty);
                    consulta = "SELECT IdProveedor, Proveedor FROM proveedores " + filter + " ORDER BY Proveedor ";
                    break;
                case "2":
                    filter = " WHERE pr.IdConfEmpresa = " + idConfEmpresa + " AND ta.idproducto IN ('HON', 'HOC', 'HOT', 'HOI')" +
                             (!string.IsNullOrEmpty(poblacionDropDownList.SelectedValue) ? " AND pr.IdPoblacion = " + poblacionDropDownList.SelectedValue : string.Empty) +
                             (!string.IsNullOrEmpty(idCongreso) ? " AND ta.FKIdCongreso = " + idCongreso : string.Empty);
                    consulta = "SELECT DISTINCT pr.IdProveedor, pr.Proveedor FROM tarifasaloj ta LEFT JOIN proveedores pr ON ta.IdProveedor = pr.IdProveedor " + filter + " ORDER BY pr.Proveedor ";
                    break;
            }

            //Ismael Ameller 21-03-2011 Control de que no salte la excepcion para tipoDropDownList.SelectedValue=1
            if (string.IsNullOrEmpty(consulta))
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString))
                {
                    hotelDropDownList.DataTextField = "Proveedor";
                    hotelDropDownList.DataValueField = "IdProveedor";
                    conn.Open();
                    hotelDropDownList.DataSource = new SqlCommand(consulta, conn).ExecuteReader();
                    hotelDropDownList.DataBind();
                }
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
            }
        }

        private void GetRoomTypes()
        {
            // ********** Substituir por método de capas de acceso a datos
            // Recupera los tipos de habitación
            string consulta;

            if (tipoDropDownList.SelectedValue == "2")
            {
                consulta = "SELECT IdTarifaAloj IdTipoHab, Descripcion TipoHab FROM tarifasaloj  WHERE IdProveedor = " + (hotelDropDownList.SelectedValue == "" ? "-1" : hotelDropDownList.SelectedValue) + " AND FKIdCongreso = " + idCongreso;
            }
            else
            {
                //Ismael Ameller Vidal 08-03-2011 Cambio el formato de carga para que por defecto me cargue Doble Uso Individual
                //consulta = "SELECT IdTipoHab, TipoHab FROM tipos_hab";
                consulta = "SELECT IdTipoHab, TipoHab FROM tipos_hab where IdTipoHab='DUI' UNION SELECT IdTipoHab, TipoHab FROM tipos_hab WHERE IdTipoHab IN ('DBL','TRP')";
                //FIN Ismael Ameller Vidal 08-03-2011 Cambio el formato de carga para que por defecto me cargue Doble Uso Individual
            }
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();

                tipoHabitacionDropDownList.DataSource = rea;
                tipoHabitacionDropDownList.DataTextField = "TipoHab";
                tipoHabitacionDropDownList.DataValueField = "IdTipoHab";
                tipoHabitacionDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }

        private string GetHotelFee()
        {
            // ********** Substituir por método de capas de acceso a datos
            // Recupera el precio de la tarifa seleccionada
            string rtv = precioNocheMaximoTextBox.Text;
            if (tipoDropDownList.SelectedValue != "2")
                return rtv;
            //Ismael Ameller 21-03-2011 Si no está informado el tipo de habitación no devolveremos el precio de la tarifa
            if (String.IsNullOrEmpty(tipoHabitacionDropDownList.SelectedValue))
            {
                return string.Empty;
            }
            //FIN Ismael Ameller 21-03-2011 Si no está informado el tipo de habitación no devolveremos el precio de la tarifa
            string consulta = "SELECT PrecioNoche FROM tarifasaloj WHERE IdTarifaAloj = " + tipoHabitacionDropDownList.SelectedValue;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                rtv = Convert.ToString(comm.ExecuteScalar());
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            return rtv;
        }

        private void CreateRecord()
        {
            SqlTransaction trans = null;

            // Guardar el servicio de alojamiento
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);

            try
            {
                conn.Open();

                string consultaIdConfEmpresa = string.Format("select idconfempresa from amec where idamec = (select top 1 idamec from expediente where idxpediente = {0})", idExpediente);
                SqlCommand commandIdConfEmpresa = new SqlCommand(consultaIdConfEmpresa, conn);
                string idConfEmpresa = commandIdConfEmpresa.ExecuteScalar().ToString();

                trans = conn.BeginTransaction(IsolationLevel.Serializable);
                string consulta = "INSERT INTO reservasviajes (idreserva, fkidexpediente, idpeticionario, idestado, fechapeticion, reserva, observaciones,LastUpd) VALUES " +
                                "(" + idReserva + ", " + idExpediente + ", " + idUsuario + ", 'AB', getdate(), 'Nombre reserva', '', getdate());";
                SqlCommand comm = new SqlCommand(consulta, conn, trans);
                comm.ExecuteNonQuery();

                idServicio = Convert.ToString(GetNextId("serviciosreservashotel", "idserviciohotel"));
                string fechaIni = llegadaInputDatePickerControl.Value == "" ? "NULL" : llegadaInputDatePickerControl.Value;
                if (fechaIni != "NULL")
                {
                    DateTime inidt = Convert.ToDateTime(fechaIni);
                    fechaIni = String.Format("{0:yyyyMMdd}", inidt);
                }
                string fechaFin = salidaInputDatePickerControl.Value == "" ? "NULL" : salidaInputDatePickerControl.Value;
                if (fechaFin != "NULL")
                {
                    DateTime Findt = Convert.ToDateTime(fechaFin);
                    fechaFin = String.Format("{0:yyyyMMdd}", Findt);
                }
                string idproveedor =
                    (tipoDropDownList.SelectedValue != "1"
                    ?
                    !String.IsNullOrEmpty(hotelDropDownList.SelectedValue) ? hotelDropDownList.SelectedValue : "NULL"
                    :
                    "NULL");
                string idtarifa = (tipoDropDownList.SelectedValue == "2" ? !String.IsNullOrEmpty(tipoHabitacionDropDownList.SelectedValue) ? tipoHabitacionDropDownList.SelectedValue : "NULL" : "NULL");
                string idtipohab = (tipoDropDownList.SelectedValue != "2" ? "'" + tipoHabitacionDropDownList.SelectedValue + "'" : "NULL");
                double val;
                double dImporte = Double.TryParse(precioNocheMaximoTextBox.Text, NumberStyles.Number, CultureInfo.GetCultureInfo("es-ES").NumberFormat, out val) ? val : 0; 
                string precio = Convert.ToString(dImporte).Replace(",", "@");
                string personas = Convert.ToString(Test.NumPassengers);
                string observaciones = Test.Observaciones.Replace("'", "");
                string hotel =
                    (tipoDropDownList.SelectedValue == "1" || tipoDropDownList.SelectedValue == "2"
                    ?
                    hotelTextBox.Text.Replace("'", "")
                    :
                    hotelDropDownList.SelectedItem != null ? hotelDropDownList.SelectedItem.Text.Replace("'", "") : "");
                string idpoblacion = !String.IsNullOrEmpty(poblacionDropDownList.SelectedValue) ? poblacionDropDownList.SelectedValue : "NULL";

                //Ismael Ameller 20/04/2011 --Si es tarifa precargada Recuperamos el IdTipoAloj de la tabla tarifasaloj
                string strIdTipoAloj = string.Empty;
                if ((!string.IsNullOrEmpty(idtarifa)) && (idtarifa != "NULL"))
                {
                    string consultaTipoAloj = "SELECT IdTipoAloj FROM tarifasaloj WHERE IdTarifaAloj = " + tipoHabitacionDropDownList.SelectedValue;
                    string consultaTipoHab = "SELECT IdTipoHab FROM tarifasaloj WHERE IdTarifaAloj = " + tipoHabitacionDropDownList.SelectedValue;

                    try
                    {
                        //conn2.Open();
                        SqlCommand comm2 = new SqlCommand(consultaTipoAloj, conn, trans);
                        strIdTipoAloj = Convert.ToString(comm2.ExecuteScalar());
                        //conn3.Open();
                        SqlCommand comm3 = new SqlCommand(consultaTipoHab, conn, trans);
                        idtipohab = "'" + Convert.ToString(comm3.ExecuteScalar()) + "'";
                    }
                    catch (Exception ex)
                    {
                        //Ismael Ameller 09-03-2011 Envio de Mail
                        Mail mail = new Mail();
                        mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                        //FIN Ismael Ameller 09-03-2011 Envio de Mail
                        Console.WriteLine(ex.ToString());
                    }
                }
                int IdTipoAloj = 3;
                if (!string.IsNullOrEmpty(strIdTipoAloj))
                {
                    IdTipoAloj = Convert.ToInt32(strIdTipoAloj);
                }
                //FIN Ismael Ameller 20/04/2011 --Si es tarifa precargada Recuperamos el IdTipoAloj de la tabla tarifasaloj

                consulta = "INSERT INTO serviciosreservashotel (idserviciohotel, fechahorallegada, fechahorasalida, IdPais, IdProvincia, IdPoblacion, poblacion, IdProveedor, hotel, IdTipoAloj, " +
                  "observaciones, idtipohab, num_habitaciones, desc_tipoalojamiento, desc_idtipo_habitacion, idtarifaaloj, pvp, idconfempresa) VALUES " +
                  "(" + idServicio + ", '" + fechaIni + "', '" + fechaFin + "', NULL, NULL, " + idpoblacion + ", NULL, " + idproveedor + ", '" + hotel + "'," + IdTipoAloj + ", " +
                  "'" + observaciones + "', " + idtipohab + ", " + personas + ", '', '', " + idtarifa + ", " + precio.Replace("@", ".") + ", " + idConfEmpresa + ");";
                comm = new SqlCommand(consulta, conn, trans);
                comm.ExecuteNonQuery();

                idServicioReserva = Convert.ToString(GetNextId("serviciosreservasviajes", "idservicio"));

                consulta = "INSERT INTO serviciosreservasviajes (idservicio, idreserva, idTipoBono, fechapeticion, idserviciohotel, idservicioactividad, idserviciotransporte, idservicioinscripcion, resumenservicio) VALUES " +
                  "(" + idServicioReserva + ", " + idReserva + ",'HOT', getdate(), " + idServicio + ", NULL, NULL, NULL, '" + hotel + "');";
                comm = new SqlCommand(consulta, conn, trans);
                comm.ExecuteNonQuery();

                trans.Commit();
                Test.IdServicio = idServicio;
                Test.Save();

                ShowMessage("El servicio se ha creado correctamente.", "DetalleExpediente.aspx?idexp=" + idExpediente);

                //añadimos esta llamada al procedure para actualizar los importes de las reservas, expediente y AMEC
                consulta = " EXEC dbo.actualizar_importes_totales " + idServicioReserva + " ;";
                comm = new SqlCommand(consulta, conn);
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                    trans.Rollback();
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
                ShowMessage("Se ha producido un error al crear el servicio. ", "");
            }
            conn.Close();
        }

        private void UpdateRecord()
        {
            SqlTransaction trans = null;
            // Actualizar el servicio de alojamiento
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);

            try
            {
                conn.Open();
                trans = conn.BeginTransaction(IsolationLevel.Serializable);
                string fechaIni = llegadaInputDatePickerControl.Value == "" ? "NULL" : llegadaInputDatePickerControl.Value;
                
                if (fechaIni != "NULL")
                {
                    DateTime inidt = Convert.ToDateTime(fechaIni);
                    fechaIni = String.Format("{0:yyyyMMdd}", inidt);
                }
                
                string fechaFin = salidaInputDatePickerControl.Value == "" ? "NULL" : salidaInputDatePickerControl.Value;

                if (fechaFin != "NULL")
                {
                    DateTime Findt = Convert.ToDateTime(fechaFin);
                    fechaFin = String.Format("{0:yyyyMMdd}", Findt);
                }
                
                string idproveedor = (tipoDropDownList.SelectedValue != "1" && !string.IsNullOrEmpty(hotelDropDownList.SelectedValue) ? hotelDropDownList.SelectedValue : "NULL");
                string idtarifa = (tipoDropDownList.SelectedValue == "2" && !string.IsNullOrEmpty(tipoHabitacionDropDownList.SelectedValue) ? tipoHabitacionDropDownList.SelectedValue : "NULL");
                string idtipohab = (tipoDropDownList.SelectedValue != "2" ? "'" + tipoHabitacionDropDownList.SelectedValue + "'" : "NULL");
                double val;
                double dImporte = Double.TryParse(precioNocheMaximoTextBox.Text, NumberStyles.Number, CultureInfo.GetCultureInfo("es-ES").NumberFormat, out val) ? val : 0;
                string precio = Convert.ToString(dImporte).Replace(",", "@");
                string personas = Convert.ToString(Test.NumPassengers);
                string observaciones = Test.Observaciones.Replace("'", "");
                string hotel = (tipoDropDownList.SelectedValue == "1" ? hotelTextBox.Text : (hotelDropDownList.SelectedItem != null) ? hotelDropDownList.SelectedItem.Text.Replace("'", "") : string.Empty);
                string idpoblacion = !string.IsNullOrEmpty(poblacionDropDownList.SelectedValue) ? poblacionDropDownList.SelectedValue : "NULL";

                //Ismael Ameller 20/04/2011 --Si es tarifa precargada Recuperamos el IdTipoAloj de la tabla tarifasaloj
                string strIdTipoAloj = string.Empty;

                if ((!string.IsNullOrEmpty(idtarifa)) && (idtarifa != "NULL"))
                {
                    string consultaTipoAloj = "SELECT IdTipoAloj FROM tarifasaloj WHERE IdTarifaAloj = " + tipoHabitacionDropDownList.SelectedValue;
                    string consultaTipoHab = "SELECT IdTipoHab FROM tarifasaloj WHERE IdTarifaAloj = " + tipoHabitacionDropDownList.SelectedValue;
                    try
                    {
                        //conn.Open();
                        SqlCommand comm2 = new SqlCommand(consultaTipoAloj, conn, trans);
                        strIdTipoAloj = Convert.ToString(comm2.ExecuteScalar());
                        //conn3.Open();
                        SqlCommand comm3 = new SqlCommand(consultaTipoHab, conn, trans);
                        idtipohab = "'" + Convert.ToString(comm3.ExecuteScalar()) + "'";
                    }
                    catch (Exception ex)
                    {
                        //Ismael Ameller 09-03-2011 Envio de Mail
                        Mail mail = new Mail();
                        mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                        //FIN Ismael Ameller 09-03-2011 Envio de Mail
                        Console.WriteLine(ex.ToString());
                    }
                }

                int IdTipoAloj = 3;

                if (!string.IsNullOrEmpty(strIdTipoAloj))
                {
                    IdTipoAloj = Convert.ToInt32(strIdTipoAloj);
                }
                //FIN Ismael Ameller 20/04/2011 --Si es tarifa precargada Recuperamos el IdTipoAloj de la tabla tarifasaloj

                string consulta = "UPDATE serviciosreservashotel SET fechahorallegada = '" + fechaIni + "', fechahorasalida = '" + fechaFin + "', IdProveedor = " + idproveedor + ", hotel = '" + hotel + "', " +
                  "observaciones = '" + observaciones + "', num_habitaciones = " + personas + ", idtarifaaloj = " + idtarifa + ", pvp = " + precio.Replace("@", ".") + ", idtipohab = " + idtipohab + ", IdPoblacion = " + idpoblacion + " " + ",IdTipoAloj = " + IdTipoAloj + " " +
                  "WHERE idserviciohotel = " + idServicio + ";";
                //+ "UPDATE reservasviajes SET idestado = '" + state + "' WHERE idreserva = (SELECT idreserva FROM serviciosreservashotel WHERE  idserviciohotel = " + idServicio + ")";
                SqlCommand comm = new SqlCommand(consulta, conn, trans);
                comm.ExecuteNonQuery();
                //Jose Laguna 06-05-2011 17:09 - Estas funciones no se llamaban y por tanto no actualizaba ni el estado ni los asistentes
                trans.Commit();
                SetState("AB");
                Test.Save();

                ShowMessage("El servicio se ha actualizado correctamente.", "DetalleExpediente.aspx?idexp=" + idExpediente);

                //añadimos esta llamada al procedure para actualizar los importes de las reservas, expediente y AMEC
                consulta = " exec dbo.actualizar_importes_totales " + GetServicioByReserva() + " ;";
                comm = new SqlCommand(consulta, conn);
                comm.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
                ShowMessage("Se ha producido un error al actualizar el servicio", "");

            }
            finally
            {
                conn.Close();
            }
        }

        private void SetState(string _state)
        {
            try
            {
                // Actualizar el estado del servicio de alojamiento            
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString))
                {
                    conn.Open();
                    string consulta = "UPDATE reservasviajes SET idestado = '" + _state + "', LastUpd = getdate() WHERE idreserva = " + idReserva + ";";
                    SqlCommand comm = new SqlCommand(consulta, conn, conn.BeginTransaction(IsolationLevel.Serializable));
                    comm.ExecuteNonQuery();
                    comm.Transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
            }
        }

        private void GetRecord()
        {
            if (string.IsNullOrEmpty(idReserva))
                return;

            // Recupera los datos del registro
            System.Text.StringBuilder consulta = new System.Text.StringBuilder();

            consulta.Append("SELECT SH.*, SR.*, RV.idestado FROM serviciosreservashotel SH ")
                    .Append("LEFT JOIN serviciosreservasviajes SR ")
                    .Append("ON SH.idserviciohotel = SR.idserviciohotel ")
                    .Append("LEFT JOIN reservasviajes RV ")
                    .Append("ON SR.idreserva = RV.idreserva ")
                    .AppendFormat("WHERE SR.idreserva = {0}", idReserva);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(consulta.ToString(), conn);
                    SqlDataReader rea = comm.ExecuteReader();

                    if (!rea.HasRows) 
                        return;

                    while (rea.Read())
                    {
                        idServicio = rea["idserviciohotel"].ToString();
                        state = rea["idestado"].ToString();
                        statename = GetStateName();

                        if (Page.IsPostBack) 
                            continue;

                        if (!IsDbNull(rea, "fechahorallegada"))
                        {
                            llegadaInputDatePickerControl.Value = DateTime.Parse(rea["fechahorallegada"].ToString()).ToString("dd/MM/yyyy");
                        }

                        if (!IsDbNull(rea, "fechahorasalida"))
                        {
                            salidaInputDatePickerControl.Value = DateTime.Parse(rea["fechahorasalida"].ToString()).ToString("dd/MM/yyyy"); 
                        }

                        if (!IsDbNull(rea, "idtarifaaloj"))
                        {
                            tipoDropDownList.SelectedValue = "2";
                        }
                        else if (!IsDbNull(rea, "idproveedor"))
                        {
                            tipoDropDownList.SelectedValue = "0";
                        }
                        else
                        {
                            tipoDropDownList.SelectedValue = "1";
                        }

                        ShowOtherHotel();
                        SetEditTarifa();

                        if (!IsDbNull(rea, "hotel"))
                        {
                            hotelTextBox.Text = rea["hotel"].ToString();
                        }

                        if (!IsDbNull(rea, "idpoblacion"))
                        {
                            poblacionDropDownList.SelectedValue = rea["idpoblacion"].ToString();
                        }

                        if (!IsDbNull(rea, "idproveedor"))
                        {
                            hotelDropDownList.SelectedValue = rea["idproveedor"].ToString();
                            GetRoomTypes();
                        }

                        if (tipoDropDownList.SelectedValue == "2")
                        {
                            if (!IsDbNull(rea, "idtarifaaloj"))
                            {
                                tipoHabitacionDropDownList.SelectedValue = rea["idtarifaaloj"].ToString();
                            }
                        }
                        else
                        {
                            if (!IsDbNull(rea, "idtipohab"))
                            {
                                tipoHabitacionDropDownList.SelectedValue = rea["idtipohab"].ToString();
                            }
                        }

                        if (!IsDbNull(rea, "pvp"))
                        {
                            precioNocheMaximoTextBox.Text = rea["pvp"].ToString();
                        }
                        
                        if (!IsDbNull(rea, "observaciones"))
                        {
                            Test.Observaciones = rea["observaciones"].ToString();
                        }
                    }

                    rea.Close();
                    rea.Dispose();
                }
                catch (Exception ex)
                {
                    //Ismael Ameller 09-03-2011 Envio de Mail
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                    //FIN Ismael Ameller 09-03-2011 Envio de Mail
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void SetButtons()
        {
            btnGuardar.Visible = false;
            btnEnviar.Visible = false;
            btnRechazar.Visible = false;
            btnAprobar.Visible = false;
            btnCancelar.Visible = false;
            switch (state)
            {
                case "AB": //sin enviar
                    btnGuardar.Visible = true;
                    //si es nuevo (no guardado) no hay que mostrar el boton enviar
                    if (string.IsNullOrEmpty(idServicioReserva) && string.IsNullOrEmpty(idReserva))
                        btnEnviar.Visible = false;
                    else
                        btnEnviar.Visible = true;
                    btnRechazar.Visible = true;
                    btnAprobar.Visible = false;
                    btnCancelar.Visible = false;
                    break;
                case "ABI": //abierto
                    break;
                case "AC": //aceptada
                case "ACP": //aceptada
                case "CFP": //aceptado
                    btnRechazar.Visible = true;
                    btnGuardar.Visible = false;
                    btnEnviar.Visible = false;
                    btnAprobar.Visible = false;
                    btnCancelar.Visible = false;
                    break;
                case "AN": //rechazado
                case "CTZD": //cotizando
                case "PTR": //tramitando
                    btnGuardar.Visible = false;
                    btnEnviar.Visible = false;
                    btnRechazar.Visible = false;
                    btnAprobar.Visible = false;
                    btnCancelar.Visible = false;
                    break;
                case "CER": //cerrado
                    break;
                case "CN": //cancelado
                case "CNTR": //cancelado
                    break;
                case "CR": //enviado
                    btnGuardar.Visible = true;
                    btnEnviar.Visible = false;
                    btnRechazar.Visible = true;
                    btnAprobar.Visible = false;
                    btnCancelar.Visible = false;
                    break;
                case "FZ": //finalizado
                    break;
                case "MDF": //MDF
                    break;
                case "NC": // en curso
                    break;
                case "CTZ": //cotizado
                    btnGuardar.Visible = false;
                    btnEnviar.Visible = false;
                    btnRechazar.Visible = true;
                    btnAprobar.Visible = true;
                    btnCancelar.Visible = false;
                    break;
                case "TR": //tramitado
                    btnGuardar.Visible = false;
                    btnEnviar.Visible = false;
                    btnRechazar.Visible = false;
                    btnAprobar.Visible = false;
                    btnCancelar.Visible = true;
                    break;
                //case "":
                //    if (string.IsNullOrEmpty(idReserva))
                //        btnGuardar.Visible = true;
                //    break;
                default:
                    if (string.IsNullOrEmpty(idReserva))
                        btnGuardar.Visible = true;
                    break;
            }
        }

        private string GetServicioByReserva()
        {
            // Recupera el id de servicio a partir del id de reserva
            string id;
            string consulta = "SELECT idservicio FROM serviciosreservasviajes WHERE idreserva = " + idReserva;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                {
                    id = Convert.ToString(int.Parse(rea["idservicio"].ToString()));
                    statename = GetStateName();
                }
                else
                    id = "";

                conn.Close();
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                id = "";
                Console.WriteLine(ex.ToString());
                conn.Close();
            }
            return id;
        }

        private string GetReservaByServicio()
        {
            // Recupera el id de servicio a partir del id de reserva
            string id;
            string consulta = "SELECT idreserva FROM serviciosreservasviajes WHERE idserviciohotel = " + idServicioReserva;

            using(SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(consulta, conn);
                    SqlDataReader rea = comm.ExecuteReader();
                    id = rea.HasRows && rea.Read() ? rea["idreserva"].ToString() : string.Empty;
                }
                catch (Exception ex)
                {
                    //Ismael Ameller 09-03-2011 Envio de Mail
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                    //FIN Ismael Ameller 09-03-2011 Envio de Mail
                    id = "";
                    Console.WriteLine(ex.ToString());
                }
            }
            

            return id;
        }

        private string GetStateName()
        {
            // Recupera el id de servicio a partir del id de reserva
            string id = "--";
            string consulta = "SELECT estado FROM estadosreservas WHERE idestado = '" + state + "';";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                    id = rea["estado"].ToString();
                conn.Close();
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
                conn.Close();
            }
            return id;
        }

        private bool IsDbNull(SqlDataReader data, string field)
        {
            return data.IsDBNull(data.GetOrdinal(field));
        }

        private int GetNextId(string table, string attribute)
        {
            int id = 0;
            string consulta = "SELECT COALESCE(MAX(" + attribute + "), 0) + 1 FROM " + table;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                    id = rea.GetInt32(0);
                conn.Close();
                return id;
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
                conn.Close();
                return 0;
            }
        }

        private void ShowMessage(string message, string navigation)
        {
            // Muestra mensaje y navega a una URL especificada
            navigation = (string.IsNullOrEmpty(navigation) ? "closeLoading()" : "document.location='" + navigation + "'");
            string script = String.Format("alert('{0}');{1};", message, navigation);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "dialog", script, true);
        }

        #endregion Functions
    }
}