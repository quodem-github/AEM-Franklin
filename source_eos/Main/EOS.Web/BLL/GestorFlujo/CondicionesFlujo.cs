using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EOS.Entidades.Datos;
using EOS.Web;

namespace EOS.Web.BLL.GestorFlujo
{
    public class CondicionesFlujo
    {
        //Preaprobado Negocio
        public bool PreNegocio;
        //Preaprobado Medico
        public bool PreMedico;
        //Preaprobado Legal
        public bool PreLegal;

        public bool EsDifSolicitante;
        public bool EstaSometido;
        public bool ConParaguas;
        public bool EsAsistente;
        public bool EsDelegado;
        public bool EsDirector;
        public bool SolicitanteDirector;

        /// <summary>
        /// Establece los valores booleanos en funcion del cargo del usuario. Director/Asistent/Delegado
        /// </summary>
        /// <param name="idPeticionario">Id del cargo del solicitante del amec</param>
        public CondicionesFlujo(int  idPeticionario,int idSolicitante)
        {
            AgenteUsuarios agUsuarios = new AgenteUsuarios();
            
            DDatosPersonalesUsuario cargoSolicitante = agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(idSolicitante.ToString());
            SolicitanteDirector = cargoSolicitante.director ?? false;

            DDatosPersonalesUsuario cargo = agUsuarios.ObtenerDatosPersonalesPorIDPeticionario(idPeticionario.ToString());
            EsDirector = cargo.director ?? false;
            EsAsistente = false;//cargo.assistant ?? false;
            EsDelegado = !EsDirector && !EsAsistente;
            
        }
    }
}