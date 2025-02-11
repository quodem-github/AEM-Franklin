using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Web
{
    public class MaquinaEstados
    {
        /** Pendiente de enviar, no ha llegado a la agencia todavía */
        public const String SinEnviar = "AB";
        /** Enviado a la agencia pendiente de ser cotizado */
        public const String Enviado = "CR";
        /** La agencia esta cotizando la reserva por lo tanto no se puede hacer NADA */
        public const String Cotizando = "CTZD";
        /** En espera de recibir respuesta de aprobación por parte del peticionario */
        public const String Cotizado = "CTZ";
        /** Presupuesto aceptado por el peticionario */
        public const String Aceptado = "CFP";
        /** La agencia esta tramitando la reserva por lo tanto no se puede hacer NADA */
        public const String Tramitando = "PTR";
        /** Reserva Cerrada */
        public const String Tramitado = "TR";
        /** Cancelada sin gastos */
        public const String Rechazado = "AN";
        /** Cancelada la reserva con gastos */
        public const String Cancelado = "CN";
    }
}
