using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;

/*Pacifico 07012011*/
namespace EOS.Repositorios
{
    public interface IRepositorioServicios
    {

        DReservasViajes ObtenerDatosReservasPorID(int nID);
        int insertarReserva(DReservasViajes oReservasViajes);

        DServiciosReservas ObtenerDatosServiciosReservasPorID(int nID);
        bool insertarServicio(DServiciosReservas oServiciosReservas);

        DVServicioTransporte ObtenerDatosServiciosTransportePorID(int nID);
        bool insertarServicioTransporte(DVServicioTransporte oServiciosTransporte, string idExpediente);

        DServiciosPassengers ObtenerDatosServiciosPassengersPorID(int nID);
        bool insertarServicioPassengers(DServiciosPassengers oServiciosPassengers);
    }
    
}
