using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using msd.MailRecordatorio.DLL;


namespace msd.MailRecordatorio
{
    public class Proceso : Quodem.Monitor.Procesos.Ejecucion
    {
        private Quodem.Monitor.Procesos.Resultado _resultado;

        public Quodem.Monitor.Procesos.Resultado Resultado
        {
            set
            {
                _resultado = value;
            }
            get { return _resultado = new Quodem.Monitor.Procesos.Resultado(); }
        }

        protected override void Run()
        {
            base.Run();
            Quodem.Monitor.Alerta.WriteLog(" Metodo Run: ");
            Quodem.Monitor.Monitoring.ResultLogRecord result = new Quodem.Monitor.Monitoring.ResultLogRecord
            {
                Description = Variables.MonitorDescription,
                ResultMessage = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Status = Quodem.Monitor.Monitoring.AbstractLogRecord.LogStatusType.OK,
                ResultCode = Variables.ResultCode.Ok.GetHashCode()
            };
            try
            {
                //Recordatorio Amecs
                DataTable dtResult = new DataTable();
                BBDDManager manager = new BBDDManager();
                dtResult = manager.GetAmecsRecordatorio();
                if (dtResult == null) AddMensajeAndWriteLineLog("Error obteniendo datos del amec en el metodo GetAmecsRecordatorio()", -1);
                AddMensajeAndWriteLineLog("Mostrando datos de los amec recuperados", 5);
                foreach (DataRow rows in dtResult.Rows)
                {
                    if (manager.GetLogMailRecordatorio(rows["IdAmec"].ToString(), int.Parse(rows["IdPeticionario"].ToString())) == 0)
                    {
                        EnviaMailRecordatorio(rows["IdAmec"].ToString(), rows["Email"].ToString(), int.Parse(rows["IdPeticionario"].ToString()));
                        AddMensajeAndWriteLineLog("Envio de correo -> IdAmecs: " + rows["IdAmec"] + " Destinatario: " + rows["Email"], 5);
                    }
                    else
                    {
                        AddMensajeAndWriteLineLog(string.Format("No se envia recordatorio a {0} para el amec {1} debido a que ya se envió con anterioridad", rows["Email"], rows["IdAmec"]), 4);
                    }
                }

                if (dtResult.Rows.Count == 0)
                {
                    AddMensajeAndWriteLineLog("No se han recuperado amecs de los que enviar recordatorio", 5);
                }

                //Delegaciones
                EnviarMailAvisoCaducidadDelegacion();

            }

            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog(ex.Message);
                AddMensajeAndWriteLineLog("Mensaje de error; ", 0);
                AddMensajeAndWriteLineLog(ex.Message, 0);
                result.Status = Quodem.Monitor.Monitoring.AbstractLogRecord.LogStatusType.Error;
                result.ResultCode = Variables.ResultCode.InternalError.GetHashCode();
                result.ResultMessage = ex.Message;
                ResultRecordList.Add(result);
            }
        }

        public void EnviaMailRecordatorio(string idamec, string destinatario,int idPeticionario)
        {
            MailMessage message = new MailMessage();
            try
            {
                string link = string.Format("{0}DetalleAMEC.aspx?idamec={1}", Variables.RutaLinkMail, idamec);
                try
                {
                    if (destinatario.Contains('@')) message.To.Add(new MailAddress(destinatario));

                }
                catch (Exception ex)
                {
                    throw;
                }

                message.Subject = string.Format("MAIL RECORDATORIO para el AMEC {0}", idamec); 
                message.Body = string.Format("\r\n Les informamos que el AMEC {0} lleva más de 7 días pendiente de aprobacion y aún no se a aprobado.\r\n\r\nHaz click en el link adjunto para ver más detalles: {1}", idamec, link);
                EnviaMail(message); 
                BBDDManager manager = new BBDDManager();
                manager.GuardaLogMail(Int32.Parse(idamec), "MailRecordatorio", destinatario, message.Subject, message.Body, null, true, null, idPeticionario);
                manager.ActualizarAprobacionJefe(Int32.Parse(idamec), idPeticionario);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
        public void EnviaMail(System.Net.Mail.MailMessage message)
        {
            message.From = new MailAddress(Variables.FromEmail);
            SmtpClient smtpSender = new SmtpClient(Variables.HostMail, int.Parse(Variables.PortMail));
            smtpSender.EnableSsl = false;
            smtpSender.Credentials = new System.Net.NetworkCredential(Variables.UserNameMail, Variables.PasMail);

            try
            {
                smtpSender.Send(message);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void EnviaMailAlternativo(System.Net.Mail.MailMessage message)
        {
            SmtpClient smtpSenderAlternativo = new SmtpClient(Variables.HostMailAlternativo, int.Parse(Variables.PortMailAlternativo));
            smtpSenderAlternativo.EnableSsl = false;
            smtpSenderAlternativo.Credentials = new System.Net.NetworkCredential(Variables.UserNameMailAlternativo, Variables.PasswordMailAlternativo);

            try
            {
                smtpSenderAlternativo.Send(message);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void EnviarMailAvisoCaducidadDelegacion()
        {
            List<DelegacionDto> delegaciones = new List<DelegacionDto>();
            BBDDManager manager = new BBDDManager();
            delegaciones = manager.ObtenerListaDelegaciones();
            AddMensajeAndWriteLineLog("Se han recuperado " + delegaciones.Count + " delegaciones de las que informar de aviso de caducidad", 5);
            if (delegaciones.Count > 0)
            {
                MailMessage message = new MailMessage();
                StringBuilder sb = new StringBuilder();
                message.Subject = "MAIL RECORDATORIO DE DELAGACIONES A PUNTO DE CADUCAR";
                sb.Append("En el siguiente listado se muestran las delegaciones que estan próximas a caducar:");
                sb.Append("\n");

                foreach (var delegacionDto in delegaciones)
                {
                    sb.Append("\n");
                    sb.Append("\nUsuario: " + delegacionDto.NombreUsuario);
                    sb.Append("\nSustituto: " + delegacionDto.NombreDelegado);
                    sb.Append("\nFechaDesde: " + delegacionDto.FechaDesde.ToShortDateString());
                    sb.Append("\nFechaHasta: " + delegacionDto.FechaHasta.ToShortDateString());
                }
                message.Body = sb.ToString();
                List<string> destinatarios = Variables.MailsEnvioDelegacion.Split(',').ToList();
                foreach (var destinatario in destinatarios)
                {
                    message.To.Add(destinatario);
                }

                EnviaMail(message);
            }

        }
    }
}
