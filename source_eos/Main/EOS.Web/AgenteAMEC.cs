using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Logica;
using System.Net.Mail;


namespace EOS.Web
{
    public class AgenteAMEC : AgenteBase
    {
        private DDatosPersonalesUsuario miUsuarioAmec = null;
        private DVPeticionariosRoles miUsuarioRoles = null;
        private DCabeceraActividad miDatosCongreso = null;

        private AgenteUsuarios agenteUsu = new AgenteUsuarios();
        private AgenteExpedientes agenteExp = new AgenteExpedientes();
        private AgenteMaestros agenteMaestro = new AgenteMaestros();
        public AgenteParticipantes agentePar = new AgenteParticipantes();

        public string nIDAmec = "0";
        public ICollection<DVParticipanteAmec> dParticipantesAmec = null;

        /*
        public AgenteAMEC(DDatosPersonalesUsuario dUsuario)
        {
            miUsuarioAmec = dUsuario;
        }*/

        public AgenteAMEC()
        {
            if (Session["NuevoAMEC"] != null)
            {
                DAmec miAmec = (DAmec)Session["NuevoAMEC"];
                nIDAmec = miAmec.idamec.ToString();
            }

            if (Session["NuevoAgente"] != null)
            {
                AgenteAMEC agAMECTemp = (AgenteAMEC)Session["NuevoAgente"];
                dParticipantesAmec = agAMECTemp.dParticipantesAmec;
            }
        }

        public AgenteAMEC(int _nIdAMEC, int nIdPeticionario, int? nIdCongreso)
        {
            nIDAmec = _nIdAMEC.ToString();
            miUsuarioAmec = agenteUsu.ObtenerDatosPersonalesPorIDPeticionario(nIdPeticionario.ToString());
            miUsuarioRoles = agenteUsu.ObtenerDatosRolesPorLogin(miUsuarioAmec.login);
            miDatosCongreso = agenteExp.ObtenerActividadPorID(nIdCongreso.Value.ToString());
            dParticipantesAmec = RecargaParticipantes(_nIdAMEC);
        }

        //marta mestre
        public AgenteAMEC(int nIdPeticionario)
        {
            miUsuarioAmec = agenteUsu.ObtenerDatosPersonalesPorIDPeticionario(nIdPeticionario.ToString());
            miUsuarioRoles = agenteUsu.ObtenerDatosRolesPorLogin(miUsuarioAmec.login);
        }
        //marta mestre

        public bool HayCongreso
        {
            get { return miDatosCongreso != null; }
        }

        public ICollection<DVParticipanteAmec> RecargaParticipantes(int _nIdAMEC)
        {
            return dParticipantesAmec = agentePar.ObtenerParticipantesAMEC(_nIdAMEC.ToString(), "NOMBRECOMPLETO", 0, 1000);
        }

        public string ObtenerNombreUsuario()
        {
            return string.Format("{0} {1} {2}", miUsuarioAmec.Nombre, miUsuarioAmec.Apellido1, miUsuarioAmec.Apellido2);
        }

        public string ObtenerAMEC()
        {
            string sCodAgencia = "XX";

            IEnumerable<DVEmpresa> lEmpresas = agenteMaestro.ObtenerEmpresas(null);
            if (lEmpresas != null)
            {
                DVEmpresa miEmpresa = lEmpresas.FirstOrDefault();
                if (miEmpresa != null)
                {
                    sCodAgencia = miEmpresa.CodigoAgencia;
                }
            }

            return agenteExp.ObtenerSiguienteNombreAMEC(sCodAgencia);
        }

        public string ObtenerPosition()
        {
            string sPosition = "";
            if (miUsuarioRoles != null)
            {
                sPosition = miUsuarioRoles.Position;
            }
            return sPosition;
        }

        public string ObtenerCargo()
        {
            string sCargo = "";
            if (miUsuarioRoles != null)
            {
                sCargo = miUsuarioRoles.Cargo;
            }
            return sCargo;
        }

        public string ObtenerArea()
        {
            string sArea = "";

            bool bEsAP = EsAtencionPrimaria();
            if (bEsAP)
            {
                DRegion miRegion = agenteMaestro.ObtenerRegiones(null).FirstOrDefault(f => (f.idregion == miUsuarioAmec.idregion));
                if (miRegion != null) { sArea = miRegion.region; }
            }
            else
            {
                DUnidad miUnidad = agenteMaestro.ObtenerUnidades(null).FirstOrDefault(f => (f.idunidad == miUsuarioAmec.idunidad));
                if (miUnidad != null) { sArea = miUnidad.unidad; }
            }
            return sArea;
        }

        public string ObtenerAreaLiteral(DAmec miAmec)
        {
            string sAreaLiteral = "";
            DArea miArea = (miAmec.idamec > 0) ? agenteMaestro.ObtenerAreas(null).FirstOrDefault(f => (f.Idarea == miAmec.Idarea)) :
                agenteMaestro.ObtenerAreas(null).FirstOrDefault(f => (f.Idarea == miUsuarioAmec.Idarea));
            if (miArea != null)
            {
                sAreaLiteral = miArea.area;
            }
            return sAreaLiteral;
        }

        public string ObtenerDistritoLiteral(DAmec miAmec)
        {
            string sDistritoLiteral = "";
            DDistrito miDistrito = (miAmec.idamec > 0) ? agenteMaestro.ObtenerDistritos(null).FirstOrDefault(f => (f.iddistrito == miAmec.iddistrito)) :
                agenteMaestro.ObtenerDistritos(null).FirstOrDefault(f => (f.iddistrito == miUsuarioAmec.iddistrito));
            if (miDistrito != null)
            {
                sDistritoLiteral = miDistrito.distrito;
            }
            return sDistritoLiteral;
        }

        public string ObtenerComunicarFarma()
        {
            bool bComunicar = false;
            if (miDatosCongreso != null)
            {
                bComunicar = miDatosCongreso.Comunicar.HasValue ? Convert.ToBoolean(miDatosCongreso.Comunicar.Value) : false;
            }
            return ObtenerValorBool(bComunicar);
        }

        public string ObtenerNombreActividad()
        {
            string sNombre = "";
            if (miDatosCongreso != null)
            {
                sNombre = miDatosCongreso.Congreso;
            }
            return sNombre;
        }

        public string ObtenerLugar(int idconfempresa)
        {
            string sLugar = "";
            if (miDatosCongreso != null)
            {
                DVPoblacion sede = agenteMaestro.ObtenerPoblacionesTodas(null, idconfempresa).FirstOrDefault(f => (f.IdPoblacion == miDatosCongreso.IdPoblacion));
                if (sede != null)
                {
                    sLugar = sede.Poblacion;
                }
            }
            return sLugar;
        }

        public string ObtenerAmbitoGeografico()
        {
            string sAmbito = "Internacional";
            if (miDatosCongreso != null)
            {
                bool bInternacional = miDatosCongreso.Internacional.HasValue ? Convert.ToBoolean(miDatosCongreso.Internacional.Value) : true;
                sAmbito = bInternacional ? "Internacional" : "Nacional";
            }
            return sAmbito;
        }

        public DateTime? ObtenerFechaInicioActividad()
        {
            return miDatosCongreso.Desde;
        }

        public DateTime? ObtenerFechaFinActividad()
        {
            return miDatosCongreso.Hasta;
        }

        public string ObtenerEstado(string sEstado)
        {
            string sEstadoNombre = "";
            if (sEstado.Equals("ABI")) { sEstadoNombre = "Abierto"; }
            else if (sEstado.Equals("CR")) { sEstadoNombre = "Enviado"; }
            else if (sEstado.Equals("CER")) { sEstadoNombre = "Cerrado"; }
            else if (sEstado.Equals("CN") || sEstado.Equals("AN")) { sEstadoNombre = "Cancelado"; }

            return sEstadoNombre;
        }

        public string ObtenerPreaprobada()
        {
            string sPreaprobada = "";
            if (miDatosCongreso != null)
            {
                bool bInternacional = miDatosCongreso.Internacional.HasValue ? Convert.ToBoolean(miDatosCongreso.Internacional) : false;
                int nIdValoracionFi = miDatosCongreso.IdValoracionfi.HasValue ? Convert.ToInt16(miDatosCongreso.IdValoracionfi) : 0;
                sPreaprobada = ObtenerValorBool(!bInternacional && nIdValoracionFi == 4);
            }
            return sPreaprobada;
        }

        public bool? ObtenerPreaprobadaRol()
        {
            bool? bAprobado = null;
            if (ObtenerPreaprobada() != ObtenerValorBool(false))
            {
                bAprobado = true;
            }
            return bAprobado;
        }

        public string ObtenerPending()
        {
            return "** Pendiente ** ";
        }

        public string ObtenerValorBool(bool? bValor)
        {
            string sSalida = "";
            if (bValor.HasValue)
            {
                sSalida = (bValor.Value) ? "Sí" : "No";
            }
            return sSalida;
        }

        public string ObtenerValorBoolTemp(string sValor)
        {
            bool? bSalida = null;
            if (!string.IsNullOrEmpty(sValor))
            {
                bSalida = (sValor == "1") ? true : false;
            }
            return ObtenerValorBool(bSalida);
        }

        public string ObtenerValorEntero(int? nValor)
        {
            string sSalida = "";
            if (nValor.HasValue)
            {
                sSalida = nValor.Value.ToString();
            }
            return sSalida;
        }

        public string ObtenerValorEntero(int? nValor, string sDefaultValue)
        {
            string sSalida = sDefaultValue;
            if (nValor.HasValue)
            {
                sSalida = nValor.Value.ToString();
            }
            return sSalida;
        }

        public string ObtenerValorDecimal(float? dValor)
        {
            string sSalida = "";
            if (dValor.HasValue)
            {
                sSalida = string.Format("{0:0.00}", dValor.Value);
            }
            return sSalida;
        }

        public string ObtenerValorFecha(DateTime? dFecha)
        {
            string sSalida = "";
            if (dFecha.HasValue)
            {
                sSalida = dFecha.Value.ToString("dd/MM/yy");
            }
            return sSalida;
        }
        /*
        public string ObtenerValorFechaTemp(string sFecha)
        {
            DateTime? dFecha = null;
            DateTime dFechaTemp;
            string[] sFormatos = { "dd/MM/yyyy", "dd/MM/yy", "dd/MM/yyyy HH:mm" };

            if (DateTime.TryParseExact(sFecha, sFormatos, null, System.Globalization.DateTimeStyles.AssumeLocal, out dFechaTemp))
            {
                dFecha = dFechaTemp;
            }

            return ObtenerValorFecha(dFecha);
        }
        */
        public string ObtenerValorHora(DateTime? dFecha)
        {
            string sSalida = "";
            if (dFecha.HasValue)
            {
                sSalida = dFecha.Value.ToString("HH:mm");
            }
            return sSalida;
        }

        public int? GuardarValorEntero(string sValor)
        {
            int? nValor = null;
            if (!string.IsNullOrEmpty(sValor))
            {
                int nValorFinal = 0;
                int.TryParse(sValor, out nValorFinal);
                nValor = nValorFinal;
            }
            return nValor;
        }

        public float? GuardarValorDecimal(string sValor)
        {
            float? nValor = null;
            if (!string.IsNullOrEmpty(sValor))
            {
                float nValorFinal = 0;
                float.TryParse(sValor, out nValorFinal);
                nValor = nValorFinal;
            }
            return nValor;
        }

        public int? GuardarValorCombo(object sValor)
        {
            int? nValor = null;
            int nValorTemp = 0;
            if (int.TryParse(sValor.ToString(), out nValorTemp))
            {
                nValor = (nValorTemp > 0) ? nValorTemp : new Nullable<int>();

            }
            return nValor;
        }

        public bool? GuardarValorBool(string sValor)
        {
            bool? bValor = null;
            if (!string.IsNullOrEmpty(sValor))
            {
                bValor = (sValor.ToUpper() == "NO") ? false : true;
            }
            return bValor;
        }

        public string GuardarValorBoolTemp(string sValor)
        {
            string sRetorno = null;
            if (!string.IsNullOrEmpty(sValor))
            {
                sRetorno = (sValor.ToUpper() == "NO") ? "0" : "1";
            }
            return sRetorno;
        }
        public string GuardarValorFecha(string sValor, string sFechaSoporte, DateTime? dFechaRangoInf)
        {
            DateTime dFecha = DateTime.Now;
            DateTime? dFechaSoporte = null;
            string sRetorno = "";
            string[] sFormatos = { "dd/MM/yyyy", "dd/MM/yy" };

            if (DateTime.TryParseExact(sFechaSoporte, sFormatos, null, System.Globalization.DateTimeStyles.AssumeLocal, out dFecha))
            {
                dFechaSoporte = dFecha;
            }
            dFechaSoporte = GuardarValorFecha(sValor, dFechaSoporte, dFechaRangoInf);

            if (dFechaSoporte.HasValue)
            {
                sRetorno = dFechaSoporte.Value.ToString("dd/MM/yyyy HH:mm");
            }
            return sRetorno;
        }

        public DateTime? GuardarValorFecha(string sValor, DateTime? dFechaSoporte, DateTime? dFechaRangoInf)
        {
            DateTime? dFecha = dFechaSoporte;
            DateTime dFechaTemp = System.DateTime.Now;
            string[] sFormatos = { "dd/MM/yyyy", "dd/MM/yy" };

            if (DateTime.TryParseExact(sValor, sFormatos, null, System.Globalization.DateTimeStyles.AssumeLocal, out dFechaTemp))
                dFecha = dFechaTemp;

            if (dFechaRangoInf.HasValue)
            {
                dFecha = (dFecha < dFechaRangoInf) ? dFechaRangoInf.Value : dFecha;
            }

            return dFecha;
        }

        public ICollection<DVParticipanteAmec> ObtenerParticipantesAMEC(int filtroIDAMEC,
            string sortParameter, int startRowIndex, int maximumRows)
        {
            //TODO: Ver como ordenar por el campo sortParameter recibido
            if (sortParameter.Contains("DESC"))
            {
                return dParticipantesAmec.OrderByDescending(x=>x.NombreCompleto).Skip(startRowIndex).Take(maximumRows).ToList();
            }
            else
            {
                return dParticipantesAmec.OrderBy(x => x.NombreCompleto).Skip(startRowIndex).Take(maximumRows).ToList();
            }
            /*
            Clepsydra.Comun.Colecciones.DireccionOrdenacion dir = sortParameter.Contains("DESC") ? Clepsydra.Comun.Colecciones.DireccionOrdenacion.Desc : Clepsydra.Comun.Colecciones.DireccionOrdenacion.Asc;
            return dParticipantesAmec.Ordena(sortParameter.RemoveSubcadena(" DESC"), dir).Skip(startRowIndex).Take(maximumRows).ToList();
             * */
        }



        public int ObtenerNumeroParticipantesAMEC(string filtroIDAMEC)
        {
            return dParticipantesAmec.Count();
        }

        public bool EsCancelable()
        {
            bool bEsCancelable = true;

            if (nIDAmec != "0")
            {
                bEsCancelable = agenteExp.EsCancelableAMEC(nIDAmec.ToString());
            }

            return bEsCancelable;
        }

        public bool EsAtencionPrimaria()
        {
            return (miUsuarioAmec.atencionprimaria.HasValue) ? miUsuarioAmec.atencionprimaria.Value : false;
        }

        #region "Enviar Mail"

        public void EnviaMailAprobado(DAmec miAmec)
        {
            try
            {
                AgenteMaestros agMaes = new AgenteMaestros();
                DEmpresaConf datosEmpresa = agMaes.ObtenerEmpresaConf(1);
                AgenteUsuarios agUsu = new AgenteUsuarios();
                DDatosPersonalesUsuario dPeticionario = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idpeticionario.ToString());

                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                //message.From = new MailAddress("sender@foo.bar.com");
                try { message.To.Add(new MailAddress(datosEmpresa.gestordearchivo)); }
                catch { Alert.Show("Se ha producido un error enviando el mail de Notificación al Gestor de Archivo. Por favor revise la dirección de envío en la configuración del sistema.", null); return; }
                try
                {
                    string sMailResponsable = MailInmediatoSuperior();
                    if (!string.IsNullOrEmpty(sMailResponsable))
                    {
                        message.To.Add(new MailAddress(sMailResponsable));
                    }
                }
                catch { Alert.Show("Se ha producido un error enviando el mail de Notificación al Superior del Peticionario. Por favor revise la dirección de envío en la configuración del sistema.", null); return; }

                message.Subject = string.Format("Aprobación AMEC {0}", miAmec.amec);
                message.Body = string.Format("Se ha solicitado aprobación para el AMEC {0}\r\nSolicitante: {1} {2} {3}\r\nActividad: {4}",
                    miAmec.amec, dPeticionario.Nombre, dPeticionario.Apellido1, dPeticionario.Apellido2, ObtenerNombreActividad());

                Mail.EnviaMail(message);
            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail de Notificación al Gestor de Archivo", null);

            }
        }

        public void EnviaMailFarmaIndustria(DAmec miAmec)
        {
            try
            {
                AgenteMaestros agMaes = new AgenteMaestros();
                DEmpresaConf datosEmpresa = agMaes.ObtenerEmpresaConf(1);
                AgenteUsuarios agUsu = new AgenteUsuarios();
                DDatosPersonalesUsuario dPeticionario = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idpeticionario.ToString());

                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                try { message.To.Add(new MailAddress(datosEmpresa.mailfromcomunicfi)); }
                catch { Alert.Show("Se ha producido un error enviando el mail a Farmaindustria. Por favor revise la dirección From en la configuración del sistema.", null); return; }
                try { message.To.Add(new MailAddress(datosEmpresa.mailtocomunicfi)); }
                catch { Alert.Show("Se ha producido un error enviando el mail a Farmaindustria. Por favor revise la dirección To en la configuración del sistema.", null); return; }
                try { if (!string.IsNullOrEmpty(datosEmpresa.mailcccomunicfi)) { message.CC.Add(new MailAddress(datosEmpresa.mailcccomunicfi)); } }
                catch { Alert.Show("Se ha producido un error enviando el mail a Farmaindustria. Por favor revise la dirección CC en la configuración del sistema.", null); return; }
                try { if (!string.IsNullOrEmpty(datosEmpresa.mailtorespuesta)) { message.ReplyToList.Add(new MailAddress(datosEmpresa.mailtorespuesta)); } }
                catch { Alert.Show("Se ha producido un error enviando el mail a Farmaindustria. Por favor revise la dirección ReplyTo en la configuración del sistema.", null); return; }

                message.Subject = datosEmpresa.asuntomailcomunicfi;
                message.Body = datosEmpresa.cuerpomailcomunicfi;

                Mail.EnviaMail(message);
            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail a FarmaIndustria. Por favor revise la configuración del sistema", null);
            }
        }

        public void EnviaMailaPeticionario(DAmec miAmec)
        {
            try
            {
                AgenteUsuarios agUsu = new AgenteUsuarios();
                DDatosPersonalesUsuario dPeticionario = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idpeticionario.ToString());

                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                //message.From = new MailAddress("sender@foo.bar.com");
                try { message.To.Add(new MailAddress(dPeticionario.Email)); }
                catch { Alert.Show("Se ha producido un error enviando el mail de Notificación de Aprobado al peticionario del AMEC. Por favor revise la dirección de correo del peticionario.", null); return; }

                message.Subject = string.Format("Aprobación AMEC {0}", miAmec.amec);
                message.Body = string.Format("Se ha aprobado el AMEC {0}\r\nActividad: {1}\r\nSede: {2}, Fecha: {3}\r\nImporte Aprobado: {4}\r\nNº de personas: {5}",
                miAmec.amec, ObtenerNombreActividad(), miAmec.lugar, ObtenerValorFecha(miAmec.fecha), ObtenerValorDecimal(miAmec.importetotal), ObtenerValorEntero(miAmec.numparticipantes));

                Mail.EnviaMail(message);

            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail de Notificación de Aprobado al peticionario", null);
            }
        }

        #endregion

        #region "Validacion Importe Inicial"

        public float GuardaValorInicial(DAmec miAmec)
        {
            float dPrevisionInicial = 0;
            if (miAmec.PrevisionInicial.HasValue)
            {
                if (miAmec.PrevisionInicial.Value > 0)
                {
                    dPrevisionInicial = miAmec.PrevisionInicial.Value;
                }
            }
            if (dPrevisionInicial > 0)
            {
                // comprueba que no sea >15%
                if (miAmec.importetotal > (dPrevisionInicial * 1.15))
                {
                    if (ObtenerPreaprobada() == ObtenerValorBool(false))
                    {
                        dPrevisionInicial = float.MinValue;

                        miAmec.aprobado = null;

                        miAmec.aprobadolegal = null;
                        miAmec.aprobadocomplaice = null;
                        miAmec.aprobadodg = null;

                        miAmec.fechaaprobadolegal = null;
                        miAmec.fechaaprobadocomplaice = null;
                        miAmec.fechaaprobadodg = null;
                    }
                }
            }
            else
            {
                miAmec.PrevisionInicial = miAmec.importetotal;
            }
            return dPrevisionInicial;
        }


        public string MailInmediatoSuperior()
        {
            string sMailSuperior = "";

            if (miUsuarioAmec.idpeticionarioresponsable.HasValue)
            {
                DDatosPersonalesUsuario responsable = agenteUsu.ObtenerDatosPersonalesPorIDPeticionario(miUsuarioAmec.idpeticionarioresponsable.ToString());
                if (responsable != null)
                {
                    sMailSuperior = responsable.Email;
                }

            }
            return sMailSuperior;
        }

        #endregion
    }


}
