using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Filtros;
using EOS.Entidades.Datos;
using System.Data.Common;
using System.Data;


/*Pacifico 07012011*/
namespace EOS.Repositorios
{
    public class RepositorioServicios : IRepositorioServicios
    {

        public int insertarReserva(DReservasViajes oReservasViajes)
        {
            oReservasViajes.Idreserva = ObtenerSiguienteID("SELECT max(idreserva) from reservasviajes;");

            string consulta =
                string.Format(
                    "INSERT INTO reservasviajes(idreserva,reserva,fechapeticion,idestado,IdPeticionario,fkidexpediente,locked,observaciones,LastUpd) VALUES({0},'{1}','{2}','{3}',{4},{5},{6},'{7}','{8}')",
                    oReservasViajes.Idreserva, oReservasViajes.Reserva,
                    System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), oReservasViajes.Idestado,
                    oReservasViajes.IdPeticionario, oReservasViajes.Fkidexpediente, 0, oReservasViajes.Observaciones,
                    System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
            return oReservasViajes.Idreserva;

        }

        public DReservasViajes ObtenerDatosReservasPorID(int nID)
        {
            string consulta = string.Format("select * from reservasviajes where idreserva = {0}", nID);
            return DReservasViajes.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
        }

        public DServiciosReservas ObtenerDatosServiciosReservasPorID(int nID)
        {
            string consulta = string.Format("select * from serviciosreservasviajes where idreserva = {0}", nID);
            return DServiciosReservas.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
        }


        public bool insertarServicio(DServiciosReservas oServiciosReservas)
        {
            try
            {
                oServiciosReservas.Idservicio = ObtenerSiguienteID("SELECT max(idservicio) from serviciosreservasviajes;");

                string consulta = "INSERT INTO serviciosreservasviajes (idservicio,idreserva,IdTipoBono,resumenservicio,fechapeticion,idservicioinscripcion,idserviciohotel,idservicioactividad,idserviciotransporte,locked) VALUES(" + oServiciosReservas.Idservicio + "," + oServiciosReservas.Idreserva + ",'" + oServiciosReservas.IdTipoBono + "','resumen','" + oServiciosReservas.Fechapeticion.ToString("yyyy/MM/dd HH:mm:ss") + "',null,null,null," + oServiciosReservas.Idserviciotransporte + "," + oServiciosReservas.Locked + ")";
                Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DVServicioTransporte ObtenerDatosServiciosTransportePorID(int nID)
        {
            string consulta = string.Format("select * from serviciosreservastransporte where idserviciotransporte = {0}", nID);
            return DVServicioTransporte.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();

        }

        public bool insertarServicioTransporte(DVServicioTransporte oServicioTransporte, string idExpediente)
        {

            try
            {
                oServicioTransporte.idserviciotransporte =
                    ObtenerSiguienteID("SELECT max(idserviciotransporte) from serviciosreservastransporte;");

                string consultaIdConfEmpresa = string.Format("select idconfempresa from amec where idamec = (select top 1 idamec from expediente where idxpediente = {0})", idExpediente);
                string idConfEmpresa = Quodem.Sql.SqlServerClient.GetValue(consultaIdConfEmpresa);


                string consulta =
                    string.Format(
                        "INSERT INTO serviciosreservastransporte VALUES({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},{9},'{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}',{22},{23},'{24}','{25}','{26}','{27}','{28}',{29},'{30}','{31}','{32}','{33}','{34}','{35}',{36}, {37})",
                        +
                            oServicioTransporte.idserviciotransporte,
                        oServicioTransporte.ida1_transporte,
                        oServicioTransporte.ida1_fechasalida.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                        oServicioTransporte.ida1_origen,
                        oServicioTransporte.ida1_destino,
                        oServicioTransporte.ida1_numvuelo_tren,
                        oServicioTransporte.ida1_horasalida,
                        oServicioTransporte.ida1_horallegada,

                        oServicioTransporte.ida2_transporte == null
                            ? "null"
                            : "'" + oServicioTransporte.ida2_transporte + "'",
                        oServicioTransporte.ida2_fechasalida == null
                            ? "null"
                            : "'" + oServicioTransporte.ida2_fechasalida.Value.ToString("yyyy/MM/dd HH:mm:ss") + "'",
                        oServicioTransporte.ida2_origen == null ? "" : oServicioTransporte.ida2_origen,
                        oServicioTransporte.ida2_destino == null ? "" : oServicioTransporte.ida2_destino,
                        oServicioTransporte.ida2_numvuelo_tren == null ? "" : oServicioTransporte.ida2_numvuelo_tren,
                        oServicioTransporte.ida2_horasalida == null ? "" : oServicioTransporte.ida2_horasalida,
                        oServicioTransporte.ida2_horallegada == null ? "" : oServicioTransporte.ida2_horallegada,

                        oServicioTransporte.reg1_transporte,
                        oServicioTransporte.reg1_fechasalida.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                        oServicioTransporte.reg1_origen,
                        oServicioTransporte.reg1_destino,
                        oServicioTransporte.reg1_numvuelo_tren,
                        oServicioTransporte.reg1_horasalida,
                        oServicioTransporte.reg1_horallegada,


                        oServicioTransporte.reg2_transporte == null
                            ? "null"
                            : "'" + oServicioTransporte.reg2_transporte + "'",
                        oServicioTransporte.reg2_fechasalida == null
                            ? "null"
                            : "'" + oServicioTransporte.reg2_fechasalida.Value.ToString("yyyy/MM/dd HH:mm:ss") + "'",
                        oServicioTransporte.reg2_origen == null ? "" : oServicioTransporte.reg2_origen,
                        oServicioTransporte.reg2_destino == null ? "" : oServicioTransporte.reg2_destino,
                        oServicioTransporte.reg2_numvuelo_tren == null ? "" : oServicioTransporte.reg2_numvuelo_tren,
                        oServicioTransporte.reg2_horasalida == null ? "" : oServicioTransporte.reg2_horasalida,
                        oServicioTransporte.reg2_horallegada == null ? "" : oServicioTransporte.reg2_horallegada,
                        oServicioTransporte.cotizado.ToString().Replace(",", "."),
                        oServicioTransporte.observaciones_ida, oServicioTransporte.observaciones_reg,
                        oServicioTransporte.observaciones, null, null, null, 0, idConfEmpresa);


                Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DServiciosPassengers ObtenerDatosServiciosPassengersPorID(int nID)
        {
            string consulta = string.Format("select * from transportepassengerslist where idreserva = {0}", nID);
            return DServiciosPassengers.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
        }

        public bool insertarServicioPassengers(DServiciosPassengers oServiciosPassengers)
        {
            try
            {
                oServiciosPassengers.Idtransportepassengerlist = ObtenerSiguienteID("SELECT max(Idtransportepassengerlist) from transportepassengerslist");

                string consulta = string.Format("INSERT INTO transportepassengerslist(idtransportepassengerlist,idserviciotransporte,idpassengerlist,locked) VALUES({0},{1},{2},{3})",
                  oServiciosPassengers.Idtransportepassengerlist, oServiciosPassengers.Idserviciotransporte, oServiciosPassengers.Idpassengerlist , oServiciosPassengers.Locked);

                Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private int ObtenerSiguienteID(string consulta)
        {
            int max = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            return ++max;
        }
    }
}
