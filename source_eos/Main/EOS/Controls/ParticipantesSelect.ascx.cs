using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using EOS.Entidades.Datos;
using System.Web;
using EOS.Web;

namespace EOS.Controls
{
    public partial class ParticipantesSelect : System.Web.UI.UserControl
    {
        //Xavier Morell (GP)
        public string TIPO_RESERVA_ALOJAMIENTO = "1";
        public string TIPO_RESERVA_TRANSPORTE = "2";
        public string TIPO_RESERVA_OOSS = "3";
        public string TIPO_RESERVA_INSCRIPCION = "0";

        #region Properties

        private string idExpediente = "";
        private string idServicio = "";
        private bool enabled = true;

        public String IdExpediente
        {
            get
            {
                if (this.idExpediente.Equals(String.Empty) && this.Page.Request.QueryString["idexp"] != null)
                {
                    idExpediente = this.Page.Request.QueryString["idexp"];
                }
                return idExpediente;
            }

            set
            {
                idExpediente = value;
            }
        }

        public String IdServicio
        {
            get
            {
                return idServicio;
            }

            set
            {
                idServicio = value;
            }
        }

        public string Observaciones
        {
            get
            {
                return this.txtRemarks.Text;
            }

            set
            {
                this.txtRemarks.Text = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                enabled = value;
                this.lstAvailable.Enabled = value;
                this.lstSelected.Enabled = value;
                this.btnDeselect.Enabled = value;
                this.btnSelect.Enabled = value;
                this.txtRemarks.Enabled = value;
                this.lstAvailable.ForeColor = (value ? System.Drawing.Color.Black : System.Drawing.Color.Gray);
                this.lstSelected.ForeColor = (value ? System.Drawing.Color.Black : System.Drawing.Color.Gray);
            }
        }

        public int NumPassengers
        {
            get
            {
                return this.lstSelected.Items.Count;
            }
        }
        public string ListaPasajeros
        {
            get
            {
                string pasajeros = "";
                for (int i = 0; i < lstSelected.Items.Count;i++)
                {
                    if (string.IsNullOrEmpty(pasajeros))
                    {
                        pasajeros = lstSelected.Items[i].Text;
                    }
                    else
                    {
                        pasajeros = pasajeros + "," + lstSelected.Items[i].Text;
                    }
                }
                return pasajeros;
            }
        }

        private DCabeceraExpedienteAmpliado expediente //Qurius (EAS) 24/01/2011
        {
            get 
            {
                if (HttpContext.Current.Session["currentExpediente"] != null)
                {
                    return (DCabeceraExpedienteAmpliado)HttpContext.Current.Session["currentExpediente"];
                }
                else
                {
                    AgenteExpedientes agenteExp = new AgenteExpedientes();
                    DCabeceraExpedienteAmpliado value = agenteExp.ObtenerExpedientePorID(Request.QueryString["idexp"]);
                    HttpContext.Current.Session["currentFKIdCongreso"] = value.Idactividad;

                    return value;
                }
            }
        }

        #endregion


        #region Methods
        //Xavier Morell (GP)  Añadido parámetro según el tipo de reserva
        public void Save()
        {
            // Guarda participantes seleccionados
            string tipo = Request.Params["tab"];
            SaveParticipants(tipo);
        }

        #endregion


        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && expediente.Idtiporeserva != 2)   //Qurius (EAS) 24/01/2011
            {
                // Recupera participantes disponibles y seleccionados
                string tipo = Request.Params["tab"];
                GetParticipants(tipo);
            }
        }

        protected void btnSelect_Click(object sender, ImageClickEventArgs e)
        {
            // Selecciona participantes
            int[] selected = this.lstAvailable.GetSelectedIndices();

            for (int i = selected.Length - 1; i >= 0; --i)
                this.lstSelected.Items.Add(this.lstAvailable.Items[selected[i]]);

            for (int i = selected.Length - 1; i >= 0; --i)
                this.lstAvailable.Items.Remove(this.lstAvailable.Items[selected[i]]);

        }

        protected void btnDeselect_Click(object sender, ImageClickEventArgs e)
        {
            // Deselecciona participantes
            int[] selected = this.lstSelected.GetSelectedIndices();

            for (int i = selected.Length - 1; i >= 0; --i)
                this.lstAvailable.Items.Add(this.lstSelected.Items[selected[i]]);

            for (int i = selected.Length - 1; i >= 0; --i)
                this.lstSelected.Items.Remove(this.lstSelected.Items[selected[i]]);

        }

        #endregion


        #region Functions

        private void GetParticipants(string tipo)
        {
          if (idExpediente == "")
            idExpediente = this.Page.Request.QueryString["idexp"];
          
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                // Disponibles
                string consulta = "";
                if (tipo == TIPO_RESERVA_ALOJAMIENTO)
                {
                    consulta = "SELECT P.idpassengerlist IdPassenger, " +
                                  " COALESCE(P.nombre, '') + ' ' + COALESCE(P.apel1, '') + ' ' + COALESCE(P.apel2, '')  NamePassenger " +
                                  "FROM reservas_passengers_list R " +
                                  "LEFT JOIN passengers_list P " +
                                  "ON R.idpassengerlist = P.idpassengerlist " +
                                  "WHERE R.idxpediente = " + idExpediente + " AND " +
                                  "R.idpassengerlist NOT IN  " +
                                  "( " +
                                  "    SELECT S.idpassengerlist " +
                                  "    FROM hotel_passengers_list S " +
                                  "    LEFT JOIN serviciosreservasviajes SH " +
                                  "    ON S.idserviciohotel = SH.idserviciohotel " +
                                  "    WHERE SH.idserviciohotel = " + (idServicio == null || idServicio == "" ? "-1" : idServicio) +
                                  ") ";
                }
                else if (tipo == TIPO_RESERVA_INSCRIPCION)
                {
                    consulta = "SELECT P.idpassengerlist IdPassenger, " +
                                  " COALESCE(P.nombre, '') + ' ' + COALESCE(P.apel1, '') + ' ' + COALESCE(P.apel2, '')  NamePassenger " +
                                  "FROM reservas_passengers_list R " +
                                  "LEFT JOIN passengers_list P " +
                                  "ON R.idpassengerlist = P.idpassengerlist " +
                                  "WHERE R.idxpediente = " + idExpediente + " AND " +
                                  "R.idpassengerlist NOT IN  " +
                                  "( " +
                                  "    SELECT S.idpassengerlist " +
                                  "    FROM ins_passengers_list S " +
                                  "    LEFT JOIN serviciosreservasviajes SH " +
                                  "    ON S.idservicioinscripcion = SH.idservicioinscripcion " +
                                  "    WHERE SH.idservicioinscripcion = " + (idServicio == null || idServicio == "" ? "-1" : idServicio) +
                                  ") ";
                }
                else if (tipo == TIPO_RESERVA_OOSS)
                {
                    consulta = "SELECT P.idpassengerlist IdPassenger, " +
                                  " COALESCE(P.nombre, '') + ' ' + COALESCE(P.apel1, '') + ' ' + COALESCE(P.apel2, '')  NamePassenger " +
                                  "FROM reservas_passengers_list R " +
                                  "LEFT JOIN passengers_list P " +
                                  "ON R.idpassengerlist = P.idpassengerlist " +
                                  "WHERE R.idxpediente = " + idExpediente + " AND " +
                                  "R.idpassengerlist NOT IN  " +
                                  "( " +
                                  "    SELECT S.idpassengerlist " +
                                  "    FROM actividades_passengers_list S " +
                                  "    LEFT JOIN serviciosreservasviajes SH " +
                                  "    ON S.idservicioactividad = SH.idservicioactividad " +
                                  "    WHERE SH.idservicioactividad = " + (idServicio == null || idServicio == "" ? "-1" : idServicio) +
                                  ") ";
                }
                else if (tipo == TIPO_RESERVA_TRANSPORTE)
                {
                    consulta = "SELECT P.idpassengerlist IdPassenger, " +
                                  " COALESCE(P.nombre, '') + ' ' + COALESCE(P.apel1, '') + ' ' + COALESCE(P.apel2, '')  NamePassenger " +
                                  "FROM reservas_passengers_list R " +
                                  "LEFT JOIN passengers_list P " +
                                  "ON R.idpassengerlist = P.idpassengerlist " +
                                  "WHERE R.idxpediente = " + idExpediente + " AND " +
                                  "R.idpassengerlist NOT IN  " +
                                  "( " +
                                  "    SELECT S.idpassengerlist " +
                                  "    FROM transportepassengerslist S " +
                                  "    LEFT JOIN serviciosreservasviajes SH " +
                                  "    ON S.idserviciotransporte = SH.idserviciotransporte " +
                                  "    WHERE SH.idserviciotransporte = " + (idServicio == null || idServicio == "" ? "-1" : idServicio) +
                                  ") ";
                }
                SqlCommand comm1 = new SqlCommand(consulta, conn);
                SqlDataReader rea1 = comm1.ExecuteReader();
                this.lstAvailable.DataSource = rea1;
                this.lstAvailable.DataTextField = "NamePassenger";
                this.lstAvailable.DataValueField = "IdPassenger";
                this.lstAvailable.DataBind();
                rea1.Close();

                // Seleccionados
                if (tipo == TIPO_RESERVA_ALOJAMIENTO)
                {
                    consulta = "SELECT P.idpassengerlist IdPassenger, " +
                                " COALESCE(P.nombre, '') + ' ' + COALESCE(P.apel1, '') + ' ' + COALESCE(P.apel2, '')  NamePassenger " +
                                "FROM hotel_passengers_list S " +
                                "LEFT JOIN serviciosreservasviajes SH " +
                                "ON S.idserviciohotel = SH.idserviciohotel " +
                                "LEFT JOIN passengers_list P " +
                                "ON P.idpassengerlist = S.idpassengerlist " +
                                "WHERE SH.idserviciohotel = " + (idServicio == null || idServicio == "" ? "-1" : idServicio);
                }
                else if (tipo == TIPO_RESERVA_INSCRIPCION)
                {
                    consulta = "SELECT P.idpassengerlist IdPassenger, " +
                                " COALESCE(P.nombre, '') + ' ' + COALESCE(P.apel1, '') + ' ' + COALESCE(P.apel2, '')  NamePassenger " +
                                "FROM ins_passengers_list S " +
                                "LEFT JOIN serviciosreservasviajes SH " +
                                "ON S.idservicioinscripcion = SH.idservicioinscripcion " +
                                "LEFT JOIN passengers_list P " +
                                "ON P.idpassengerlist = S.idpassengerlist " +
                                "WHERE SH.idservicioinscripcion = " + (idServicio == null || idServicio == "" ? "-1" : idServicio);
                }
                else if (tipo == TIPO_RESERVA_TRANSPORTE)
                {
                    consulta = "SELECT P.idpassengerlist IdPassenger, " +
                                " COALESCE(P.nombre, '') + ' ' + COALESCE(P.apel1, '') + ' ' + COALESCE(P.apel2, '')  NamePassenger " +
                                "FROM transportepassengerslist S " +
                                "LEFT JOIN serviciosreservasviajes SH " +
                                "ON S.idserviciotransporte = SH.idserviciotransporte " +
                                "LEFT JOIN passengers_list P " +
                                "ON P.idpassengerlist = S.idpassengerlist " +
                                "WHERE SH.idserviciotransporte = " + (idServicio == null || idServicio == "" ? "-1" : idServicio);
                }
                else if (tipo == TIPO_RESERVA_OOSS)
                {
                    consulta = "SELECT P.idpassengerlist IdPassenger, " +
                                " COALESCE(P.nombre, '') + ' ' + COALESCE(P.apel1, '') + ' ' + COALESCE(P.apel2, '')  NamePassenger " +
                                "FROM actividades_passengers_list S " +
                                "LEFT JOIN serviciosreservasviajes SH " +
                                "ON S.idservicioactividad = SH.idservicioactividad " +
                                "LEFT JOIN passengers_list P " +
                                "ON P.idpassengerlist = S.idpassengerlist " +
                                "WHERE SH.idservicioactividad = " + (idServicio == null || idServicio == "" ? "-1" : idServicio);
                }

                SqlCommand comm2 = new SqlCommand(consulta, conn);
                SqlDataReader rea2 = comm2.ExecuteReader();
                this.lstSelected.DataSource = rea2;
                this.lstSelected.DataTextField = "NamePassenger";
                this.lstSelected.DataValueField = "IdPassenger";
                this.lstSelected.DataBind();
                rea2.Close();

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


        private void SaveParticipants(string tipo)
        {
            // Guardar participantes seleccionados
            ListItemCollection selected = this.lstSelected.Items;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm;
                string consulta = "";
                if (tipo == TIPO_RESERVA_ALOJAMIENTO)
                {
                    consulta = "DELETE FROM hotel_passengers_list " +
                              "WHERE idserviciohotel = " + idServicio;
                }
                else if (tipo == TIPO_RESERVA_OOSS)
                {
                    consulta = "DELETE FROM actividades_passengers_list " +
                              "WHERE idservicioactividad = " + idServicio;
                }
                else if (tipo == TIPO_RESERVA_INSCRIPCION)
                {
                    consulta = "DELETE FROM ins_passengers_list " +
                                "WHERE idservicioinscripcion = " + idServicio;
                }
                else if (tipo == TIPO_RESERVA_TRANSPORTE)
                {
                    consulta = "DELETE FROM transportepassengerslist " +
                                "WHERE idserviciotransporte = " + idServicio;
                }
                comm = new SqlCommand(consulta, conn);
                comm.ExecuteNonQuery();
                foreach (ListItem i in selected)
                {
                    int id = 0;
                    if (tipo == TIPO_RESERVA_ALOJAMIENTO)
                    {
                        consulta = "SELECT COALESCE(MAX(idhotelpassengerlist), 0) + 1 FROM hotel_passengers_list";
                    }
                    else if (tipo == TIPO_RESERVA_OOSS)
                    {
                        consulta = "SELECT COALESCE(MAX(idactividadpassengerlist), 0) + 1 FROM actividades_passengers_list";
                    }
                    else if (tipo == TIPO_RESERVA_INSCRIPCION)
                    {
                        consulta = "SELECT COALESCE(MAX(idinspassengerlist), 0) + 1 FROM ins_passengers_list";
                    }
                    else if (tipo == TIPO_RESERVA_TRANSPORTE)
                    {
                        consulta = "SELECT COALESCE(MAX(idtransportepassengerlist), 0) + 1 FROM transportepassengerslist";
                    }
                    comm = new SqlCommand(consulta, conn);
                    SqlDataReader rea = comm.ExecuteReader();
                    if (rea.Read())
                        id = rea.GetInt32(0);
                    rea.Close();

                    // Disponibles
                    if (tipo == TIPO_RESERVA_ALOJAMIENTO)
                    {
                        consulta = "INSERT INTO hotel_passengers_list " +
                                    "(idhotelpassengerlist, idserviciohotel, idpassengerlist) " +
                                    "VALUES " +
                                    "(" + id.ToString() + ", " + idServicio + ", " + i.Value + ");";
                    }
                    else if (tipo == TIPO_RESERVA_OOSS)
                    {
                        consulta = "INSERT INTO actividades_passengers_list " +
                                    "(idactividadpassengerlist, idservicioactividad, idpassengerlist) " +
                                    "VALUES " +
                                    "(" + id.ToString() + ", " + idServicio + ", " + i.Value + ");";
                    }
                    else if (tipo == TIPO_RESERVA_TRANSPORTE)
                    {
                        consulta = "INSERT INTO transportepassengerslist " +
                                    "(idtransportepassengerlist, idserviciotransporte, idpassengerlist) " +
                                    "VALUES " +
                                    "(" + id.ToString() + ", " + idServicio + ", " + i.Value + ");";
                    }
                    else if (tipo == TIPO_RESERVA_INSCRIPCION)
                    {
                        consulta = "INSERT INTO ins_passengers_list " +
                                    "(idinspassengerlist, idservicioinscripcion, idpassengerlist) " +
                                    "VALUES " +
                                    "(" + id.ToString() + ", " + idServicio + ", " + i.Value + ");";
                    }
                    comm = new SqlCommand(consulta, conn);
                    comm.ExecuteNonQuery();
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
            conn.Close();
        }

        #endregion

    }
}