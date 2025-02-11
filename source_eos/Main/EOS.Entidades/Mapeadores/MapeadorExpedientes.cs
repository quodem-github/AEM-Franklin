using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;

namespace EOS.Entidades.Mapeadores
{
    public static class MapeadorExpedientes
    {
        public static ServicioTransporte Copiar_DVServicioTransporte_a_ServicioTransporte(DVServicioTransporte dvserv)
        {
            ServicioTransporte serv = new ServicioTransporte();

            serv.PAX = (int)dvserv.pax;
            serv.Cotizado = dvserv.cotizado;
            
            

            ServicioTransporte.Transporte ida1 = new ServicioTransporte.Transporte();
            ida1.Destino = dvserv.ida1_destino;
            ida1.FechaSalida = dvserv.ida1_fechasalida;
            ida1.HoraLlegada = dvserv.ida1_horallegada;
            ida1.HoraSalida = dvserv.ida1_horasalida;
            ida1.NumVueloTren = dvserv.ida1_numvuelo_tren;
            ida1.Origen = dvserv.ida1_origen;
            ida1.Tipo = dvserv.ida1_transporte;

            ServicioTransporte.Transporte ida2 = new ServicioTransporte.Transporte();
            ida2.Destino = dvserv.ida2_destino;
            ida2.FechaSalida = dvserv.ida2_fechasalida;
            ida2.HoraLlegada = dvserv.ida2_horallegada;
            ida2.HoraSalida = dvserv.ida2_horasalida;
            ida2.NumVueloTren = dvserv.ida2_numvuelo_tren;
            ida2.Origen = dvserv.ida2_origen;
            ida2.Tipo = dvserv.ida2_transporte;

            ServicioTransporte.Transporte reg1 = new ServicioTransporte.Transporte();
            reg1.Destino = dvserv.reg1_destino;
            reg1.FechaSalida = dvserv.reg1_fechasalida;
            reg1.HoraLlegada = dvserv.reg1_horallegada;
            reg1.HoraSalida = dvserv.reg1_horasalida;
            reg1.NumVueloTren = dvserv.reg1_numvuelo_tren;
            reg1.Origen = dvserv.reg1_origen;
            reg1.Tipo = dvserv.reg1_transporte;

            ServicioTransporte.Transporte reg2 = new ServicioTransporte.Transporte();
            reg2.Destino = dvserv.reg2_destino;
            reg2.FechaSalida = dvserv.reg2_fechasalida;
            reg2.HoraLlegada = dvserv.reg2_horallegada;
            reg2.HoraSalida = dvserv.reg2_horasalida;
            reg2.NumVueloTren = dvserv.reg2_numvuelo_tren;
            reg2.Origen = dvserv.reg2_origen;
            reg2.Tipo = dvserv.reg2_transporte;

            serv.Ida = new List<ServicioTransporte.Transporte>() { ida1, ida2 };
            serv.Regreso = new List<ServicioTransporte.Transporte>() { reg1, reg2 };

            return serv;
        }
    }
}
