using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using System.Web;
using EOS.Logica;

/*Pacifico 07012011*/
namespace EOS.Web
{
    public class AgenteServicio
    {
        public LogicaServicios MiLogicaServicios{ get; set; }

        public AgenteServicio()
        {
            MiLogicaServicios = new LogicaServicios();
        }       

        public int InsertarReserva(DReservasViajes oReservas)
        { 
            return MiLogicaServicios.insertarReserva(oReservas);
        }

        public bool InsertarServicioReserva(DServiciosReservas oServicioReservas)
        {
            return MiLogicaServicios.insertarServicio(oServicioReservas);
        }

        public bool InsertarServicioTransporte(DVServicioTransporte oServicioTransporte, string idExpediente)
        {
            return MiLogicaServicios.insertarServicioTransporte(oServicioTransporte, idExpediente);
        }
        
        public bool InsertarPassengers(DServiciosPassengers oPassengers)
        {
            return MiLogicaServicios.insertarServicioPassengers(oPassengers);
        }
    }
}
