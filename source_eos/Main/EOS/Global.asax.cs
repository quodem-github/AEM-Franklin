using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using EOS.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using EOS.security;
using EOS.ServiceLogic.BLL.AutoMapper;

namespace EOS
{
    public class Global : HttpApplication
    {
        public void Application_Start(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;

            // Code that runs on application startup
            Application[Constantes.Application.LogPath] = EOSLogger.getLogPath(Server.MapPath("~"));

            var config = System.Web.Http.GlobalConfiguration.Configuration;

            var sessionHandler = new SessionHandler(new List<ISessionInspector>() { new SessionTimeInspector() }) { InnerHandler = new HttpControllerDispatcher(config) };

            config.Routes.MapHttpRoute(
                name: "Public",
                routeTemplate: "api/public/IsTokenManagement.aspx/{param}",
                defaults: new { param = System.Web.Http.RouteParameter.Optional, controller = "IsTokenManagement" });

            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}.aspx/{param}",
                defaults: new { param = System.Web.Http.RouteParameter.Optional },
                constraints: null,
                handler: sessionHandler);
            
            config.Routes.MapHttpRoute(
               name: "Private",
               routeTemplate: "api/private/{controller}.aspx/{param}",
               defaults: new { param = System.Web.Http.RouteParameter.Optional });

            config.Routes.MapHttpRoute(
               name: "Download",
               routeTemplate: "api/private/{controller}.ashx/{param}",
               defaults: new { param = System.Web.Http.RouteParameter.Optional });

            config.Routes.MapHttpRoute(
               name: "Upload",
               routeTemplate: "api/{controller}/{action}.aspx/{param}",
               defaults: new { param = System.Web.Http.RouteParameter.Optional },
               constraints: null);

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{param}",
                defaults: new { param = System.Web.Http.RouteParameter.Optional },
                constraints: null,
                handler: sessionHandler);
          

            RegisterApis(config);

            config.MessageHandlers.Add(new Api.CorsCustomHandler());
            
            AutomapperService.InitializeAutomapper();


        }


       
        /// <summary>
        /// Method to hook up formatters and message handlers
        /// </summary>
        /// <param name="config">Http configuration context</param>
        private static void RegisterApis(HttpConfiguration config)
        {
            var formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);
        }




        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
        }

        void Application_Error(object sender, EventArgs e) 
        {
            try
            {
                SendApplicationError(Server.GetLastError(), Request, Session, GetType().Name);
            }
            catch (Exception)
            {

            }
        }

       void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            //Pau Ferrer  20-06-2011 Iniciar Log
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

        public static void SendApplicationError(Exception serverError, System.Web.HttpRequest request, System.Web.SessionState.HttpSessionState session, string name)
        {
            if (serverError != null && request.Url.AbsolutePath.ToLower().EndsWith(".aspx"))
            {
                Exception lastError = serverError;
                while (lastError.InnerException != null)
                {
                    lastError = lastError.InnerException;
                }

                string errormes = string.Empty;

                int result = 0;

                try
                {
                    #region Quodem.Monitor User Data

                    string userName = HttpContext.Current.User.Identity.Name;
                    string userInfo = String.Empty;

                    if (HttpContext.Current.Session != null && HttpContext.Current.Session["rolesUser"] != null)
                    {
                        EOS.Entidades.Datos.DVPeticionariosRoles rolesUser = session["rolesUser"] as EOS.Entidades.Datos.DVPeticionariosRoles;
                        userInfo = "Email:" + rolesUser.Email;
                    }
                    else
                    {
                        userInfo = "<Sesión no definida>";
                    }

                    #endregion

                    result = Quodem.Monitor.Alerta.Send(lastError, request, userName, userInfo);

                    if (HttpContext.Current.Session != null)
                    {
                        session["LastError"] = lastError;
                    }

                }
                catch (Exception ex)
                {
                    errormes += "<Error al enviar la notificación: " + ex.Message + ">";
                    result = -1;
                }

                //Si el resultado de enviar la alerta ha fallado o devuelve -1
                if (result != 0)
                {
                    try
                    {
                        errormes += lastError.Message + " [" + request.Url.ToString() + "]";
                        errormes += errormes.Replace(Environment.NewLine, "--");

                        //Escribir en el log de errores
                        Logger.Logger.PrintError(name, "Application_Error", errormes, lastError);

                        //enviar error por correo electronico
                        Mail mail = new Mail();
                        mail.Send(ConfigUtil.GetAppSetting(Constantes.AppParams.MailContactoError), lastError);
                    }
                    catch (Exception) { };
                }
            }
        }
    }
}
