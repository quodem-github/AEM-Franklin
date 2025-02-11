using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Modelo
{
    public partial class passengers_list
    {
        public int DataValueField
        {
            get
            {
                return this.idpassengerlist;
            }
        }

        public String DataTextField
        {
            get
            {
                return this.ToString();
            }
        }
        
        public override string ToString()
        {
            return String.Format("{0} {1}, {2}", this.apel1, this.apel2, this.nombre);
        }

        
        /* Pacifico 07/01/2010  INICIO */

        public static int getNextInscripcionesId(wintour_robotEntities db)
        {

            int result = 0;
            try
            {
                result = (from p in db.ins_passengers_list select (p.idinspassengerlist)).Max();
            }
            catch (Exception ex) { }
            finally { result += 1; }

            return result;
        }

        public static int getNextTransporteId(wintour_robotEntities db)
        {

            int result = 0;
            try
            {
                result = (from p in db.transportepassengerslist select (p.idtransportepassengerlist)).Max();
            }
            catch (Exception ex) { }
            finally { result += 1; }

            return result;
        }

        public static int getNextHotelId(wintour_robotEntities db)
        {

            int result = 0;
            try
            {
                result = (from p in db.hotel_passengers_list select (p.idhotelpassengerlist)).Max();
            }
            catch (Exception ex) { }
            finally { result += 1; }

            return result;
        }

        public static int getNextOtrosId(wintour_robotEntities db)
        {

            int result = 0;
            try
            {
                result = (from p in db.actividades_passengers_list select (p.idactividadpassengerlist)).Max();
            }
            catch (Exception ex) { }
            finally { result += 1; }

            return result;
        }

        public static serviciosreservasviajes getServiciosReservasViajesFromExp(wintour_robotEntities db, int idres)
        {
            return (
                        from srv in db.serviciosreservasviajes
                        join rv in db.reservasviajes on srv.idreserva equals rv.idreserva
                        where srv.idreserva  == idres
                        select srv
                    ).ToList()[0];
        }
         
        /**
         * Actualiza el listado de participantes para Inscripción */
        public static void updateInscripcionList(wintour_robotEntities db, System.Collections.ArrayList newList, int idexp , int idres)
        {
            serviciosreservasviajes srv = getServiciosReservasViajesFromExp(db, idres);

            IEnumerable<ins_passengers_list> toDelete = from p in db.ins_passengers_list
                                                           join isrv in db.serviciosreservasinscripciones on p.idservicioinscripcion equals isrv.idservicioinscripcion
                                                           join dsrv in db.serviciosreservasviajes on isrv.idservicioinscripcion equals dsrv.idservicioinscripcion
                                                           join rv in db.reservasviajes on dsrv.idreserva equals rv.idreserva
                                                           join exp in db.expediente on rv.fkidexpediente equals exp.idxpediente
                                                           where exp.idxpediente == idexp
                                                           select p;
            foreach (var item in toDelete)
            {

                db.DeleteObject(item);
            }

            db.SaveChanges();
            
            foreach (int idpersona in newList)
            {
                ins_passengers_list item = new ins_passengers_list();
                item.idinspassengerlist = getNextInscripcionesId(db);
                item.idpassengerlist = idpersona;
                item.idservicioinscripcion = (int)srv.idservicioinscripcion;
                db.ins_passengers_list.AddObject(item);
                db.SaveChanges();
            }
        }

        /*** Actualiza el listado de participantes para Transportes */
        public static void updateTransporteList(wintour_robotEntities db, System.Collections.ArrayList newList, int idexp, int idres)
        {
            serviciosreservasviajes srv =  getServiciosReservasViajesFromExp(db, idres);

            IEnumerable<transportepassengerslist> toDelete = from p in db.transportepassengerslist
                                                                    join isrv in db.serviciosreservastransporte on p.idserviciotransporte equals isrv.idserviciotransporte
                                                                    join dsrv in db.serviciosreservasviajes on isrv.idserviciotransporte equals dsrv.idserviciotransporte
                                                               join rv in db.reservasviajes on dsrv.idreserva equals rv.idreserva
                                                               join exp in db.expediente on rv.fkidexpediente equals exp.idxpediente
                                                               where exp.idxpediente == idexp
                                                               select p;
            foreach (var item in toDelete)
            {

                db.DeleteObject(item);
            }

            db.SaveChanges();

            foreach (int idpersona in newList)
            {
                transportepassengerslist item = new transportepassengerslist();
                item.idtransportepassengerlist = getNextTransporteId(db);                  
                item.idpassengerlist = idpersona;
                item.idserviciotransporte = (int)srv.idserviciotransporte;
                db.transportepassengerslist.AddObject(item);
                db.SaveChanges();
            }
        } 
               
        /*** Actualiza el listado de participantes para Otros */
        public static void updateOtrosList(wintour_robotEntities db, System.Collections.ArrayList newList, int idexp, int idres)
        {
            serviciosreservasviajes srv = getServiciosReservasViajesFromExp(db, idres);

            IEnumerable<actividades_passengers_list> toDelete = from p in db.actividades_passengers_list
                                                                       join isrv in db.serviciosreservasactividades on p.idservicioactividad  equals isrv.idservicioactividad 
                                                                    join dsrv in db.serviciosreservasviajes on isrv.idservicioactividad equals dsrv.idservicioactividad
                                                                    join rv in db.reservasviajes on dsrv.idreserva equals rv.idreserva
                                                                    join exp in db.expediente on rv.fkidexpediente equals exp.idxpediente
                                                                    where exp.idxpediente == idexp
                                                                    select p;
            foreach (var item in toDelete)
            {

                db.DeleteObject(item);
            }

            db.SaveChanges();

            foreach (int idpersona in newList)
            {
                actividades_passengers_list item = new actividades_passengers_list();
                item.idactividadpassengerlist = getNextOtrosId(db);
                item.idpassengerlist = idpersona;
                item.idservicioactividad  = (int)srv.idservicioactividad;
                db.actividades_passengers_list.AddObject(item);
                db.SaveChanges();
            }
        }

        /*** Actualiza el listado de participantes para Alojamiento */
        public static void updateAlojamientoList(wintour_robotEntities db, System.Collections.ArrayList newList, int idexp, int idres)
        {
            serviciosreservasviajes srv = getServiciosReservasViajesFromExp(db, idres);

            IEnumerable<hotel_passengers_list> toDelete = from p in db.hotel_passengers_list
                                                                 join isrv in db.serviciosreservashotel on p.idserviciohotel  equals isrv.idserviciohotel 
                                                                    join dsrv in db.serviciosreservasviajes on isrv.idserviciohotel  equals dsrv.idserviciohotel 
                                                                    join rv in db.reservasviajes on dsrv.idreserva equals rv.idreserva
                                                                    join exp in db.expediente on rv.fkidexpediente equals exp.idxpediente
                                                                    where exp.idxpediente == idexp
                                                                    select p;
            foreach (var item in toDelete)
            {

                db.DeleteObject(item);
            }

            db.SaveChanges();

            foreach (int idpersona in newList)
            {
                hotel_passengers_list item = new hotel_passengers_list();
                item.idhotelpassengerlist  = getNextHotelId(db);
                item.idpassengerlist = idpersona;
                item.idserviciohotel = (int)srv.idserviciohotel;
                db.hotel_passengers_list.AddObject(item);
                db.SaveChanges();
            }
        } 


        /* Pacifico 07/01/2010   FIN */
        /**
         * Devuelve todos los Participantes Inscritos a Inscripcion */
        public static IEnumerable<passengers_list> getInscripcionList(wintour_robotEntities db, int idexp)
        {
            return from p in db.passengers_list
            join t in db.ins_passengers_list on p.idpassengerlist equals t.idpassengerlist
            join isrv in db.serviciosreservasinscripciones on t.idservicioinscripcion equals isrv.idservicioinscripcion
            join srv in db.serviciosreservasviajes on isrv.idservicioinscripcion equals srv.idservicioinscripcion 
            join rv in db.reservasviajes on srv.idreserva equals rv.idreserva
            join exp in db.expediente on rv.fkidexpediente equals exp.idxpediente 
            where exp.idxpediente == idexp
            select p;
        }
        /**
         * Devuelve todos los Participantes disponibles para Inscripción no seleccionados aún */
        public static IEnumerable<passengers_list> getAvailableInscripcionList(wintour_robotEntities db, int idexp)
        {
            return from p in db.passengers_list
                   join r in db.reservas_passengers_list on p.idpassengerlist equals r.idpassengerlist
                   where r.idxpediente == idexp
                   && !(
                            from i in db.ins_passengers_list
                            join isrv in db.serviciosreservasinscripciones on i.idservicioinscripcion equals isrv.idservicioinscripcion
                            join srv in db.serviciosreservasviajes on isrv.idservicioinscripcion equals srv.idservicioinscripcion 
                            join rv in db.reservasviajes on srv.idreserva equals rv.idreserva
                            join exp in db.expediente on rv.fkidexpediente equals exp.idxpediente 
                            where exp.idxpediente == idexp
                            select i.idpassengerlist
                   ).Contains(p.idpassengerlist)
                   select p;
        }
        /**
         * Devuelve todos los Participantes Inscritos a Alojamiento */
        public static IEnumerable<passengers_list> getAlojamientoList(wintour_robotEntities db, int idexp)
        {
            return from p in db.passengers_list
                   join t in db.hotel_passengers_list on p.idpassengerlist equals t.idpassengerlist
                   join isrv in db.serviciosreservashotel on t.idserviciohotel equals isrv.idserviciohotel
                   join srv in db.serviciosreservasviajes on isrv.idserviciohotel equals srv.idserviciohotel
                   join rv in db.reservasviajes on srv.idreserva equals rv.idreserva
                   join exp in db.expediente on rv.fkidexpediente equals exp.idxpediente
                   where exp.idxpediente == idexp
                   select p;
        }
        /**
         * Devuelve todos los Participantes disponibles para Alojamiento no seleccionados aún */
        public static IEnumerable<passengers_list> getAvailableAlojamientoList(wintour_robotEntities db, int idexp)
        {
            return from p in db.passengers_list
                   join r in db.reservas_passengers_list on p.idpassengerlist equals r.idpassengerlist
                   where r.idxpediente == idexp
                   && !(
                            from i in db.hotel_passengers_list
                            join isrv in db.serviciosreservashotel on i.idserviciohotel equals isrv.idserviciohotel
                            join srv in db.serviciosreservasviajes on isrv.idserviciohotel equals srv.idserviciohotel
                            join rv in db.reservasviajes on srv.idreserva equals rv.idreserva
                            join exp in db.expediente on rv.fkidexpediente equals exp.idxpediente
                            where exp.idxpediente == idexp
                            select i.idpassengerlist
                   ).Contains(p.idpassengerlist)
                   select p;
        }
        /**
         * Devuelve todos los Participantes Inscritos a Transporte */
        public static IEnumerable<passengers_list> getTransporteList(wintour_robotEntities db, int idexp)
        {
            return from p in db.passengers_list
                   join t in db.transportepassengerslist on p.idpassengerlist equals t.idpassengerlist
                   join isrv in db.serviciosreservastransporte on t.idserviciotransporte equals isrv.idserviciotransporte
                   join srv in db.serviciosreservasviajes on isrv.idserviciotransporte equals srv.idserviciotransporte
                   join rv in db.reservasviajes on srv.idreserva equals rv.idreserva
                   join exp in db.expediente on rv.fkidexpediente equals exp.idxpediente
                   where exp.idxpediente == idexp
                   select p;
        }
        /**
         * Devuelve todos los Participantes disponibles para Transporte no seleccionados aún */
        public static IEnumerable<passengers_list> getAvailableTransporteList(wintour_robotEntities db, int idexp)
        {
            return from p in db.passengers_list
                   join r in db.reservas_passengers_list on p.idpassengerlist equals r.idpassengerlist
                   where r.idxpediente == idexp
                   && !(
                            from i in db.transportepassengerslist
                            join isrv in db.serviciosreservastransporte on i.idserviciotransporte equals isrv.idserviciotransporte
                            join srv in db.serviciosreservasviajes on isrv.idserviciotransporte equals srv.idserviciotransporte
                            join rv in db.reservasviajes on srv.idreserva equals rv.idreserva
                            join exp in db.expediente on rv.fkidexpediente equals exp.idxpediente
                            where exp.idxpediente == idexp
                            select i.idpassengerlist
                   ).Contains(p.idpassengerlist)
                   select p;
        }
        /**
         * Devuelve todos los Participantes Inscritos a Otros Servicios */
        public static IEnumerable<passengers_list> getOtrosServiciosList(wintour_robotEntities db, int idexp)
        {
            return from p in db.passengers_list
                   join t in db.actividades_passengers_list on p.idpassengerlist equals t.idpassengerlist
                   join isrv in db.serviciosreservasactividades on t.idservicioactividad equals isrv.idservicioactividad
                   join srv in db.serviciosreservasviajes on isrv.idservicioactividad equals srv.idservicioactividad
                   join rv in db.reservasviajes on srv.idreserva equals rv.idreserva
                   join exp in db.expediente on rv.fkidexpediente equals exp.idxpediente
                   where exp.idxpediente == idexp
                   select p;
        }
        /**
         * Devuelve todos los Participantes disponibles para Otros Servicios no seleccionados aún */
        public static IEnumerable<passengers_list> getAvailableOtrosServiciosList(wintour_robotEntities db, int idexp)
        {
            return from p in db.passengers_list
                   join r in db.reservas_passengers_list on p.idpassengerlist equals r.idpassengerlist
                   where r.idxpediente == idexp
                   && !(
                            from i in db.actividades_passengers_list
                            join isrv in db.serviciosreservasactividades on i.idservicioactividad equals isrv.idservicioactividad
                            join srv in db.serviciosreservasviajes on isrv.idservicioactividad equals srv.idservicioactividad
                            join rv in db.reservasviajes on srv.idreserva equals rv.idreserva
                            join exp in db.expediente on rv.fkidexpediente equals exp.idxpediente
                            where exp.idxpediente == idexp
                            select i.idpassengerlist
                   ).Contains(p.idpassengerlist)
                   select p;
        }
    }
}
