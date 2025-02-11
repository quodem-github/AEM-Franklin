using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Repositorios;
using EOS.Entidades.Datos;
using System.Data.Common;



/*Pacifico 07012011*/
namespace EOS.Logica
{
    public class LogicaServicios : ILogicaServicios
    {
        IRepositorioServicios MiRepositorioServicios { get; set; }

        public LogicaServicios()
        {
            this.MiRepositorioServicios = new RepositorioServicios();
        }

        public DReservasViajes ObtenerDatosReservasPorID(int nID)
        {
            DReservasViajes oReservasViajes = new DReservasViajes();
            return oReservasViajes;
        }
        public int insertarReserva(DReservasViajes oReservasViajes)
        {

            return MiRepositorioServicios.insertarReserva(oReservasViajes);
        }

        public DServiciosReservas ObtenerDatosServiciosReservasPorID(int nID)
        {

            DServiciosReservas oServiciosReservas = new DServiciosReservas();
            return oServiciosReservas;
        }
        public bool insertarServicio(DServiciosReservas oServiciosReservas) {
            return MiRepositorioServicios.insertarServicio(oServiciosReservas);
        }

        public DVServicioTransporte ObtenerDatosServiciosTransportePorID(int nID)
        {

            DVServicioTransporte oServicioTransporte = new DVServicioTransporte();
            return oServicioTransporte;
        }
        public bool insertarServicioTransporte(DVServicioTransporte oServiciosTransporte, string idExpediente) {
            return MiRepositorioServicios.insertarServicioTransporte(oServiciosTransporte, idExpediente);
        }

        public DServiciosPassengers ObtenerDatosServiciosPassengersPorID(int nID)
        {

            DServiciosPassengers oServiciosPassengers = new DServiciosPassengers();
            return oServiciosPassengers;
        }
        public bool insertarServicioPassengers(DServiciosPassengers oServiciosPassengers) {
            return MiRepositorioServicios.insertarServicioPassengers(oServiciosPassengers);
        }
    }
}
