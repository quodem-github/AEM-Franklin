using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


/*Pacifico 07012011*/
namespace EOS.Entidades.Datos
{
    public class DServiciosReservas
    {

        #region Constructores

        public DServiciosReservas()
        {
        }


        #endregion

        #region Propiedades
        private int _idservicio;
        private int _idreserva;
        private string _IdTipoBono;
        private DateTime _fechapeticion;
        private string _resumenservicio;
        private int _Cotizado;
        private int _locked;
        private int _idservicioinscripcion;
        private int _idserviciohotel;
        private int _idservicioactividad;
        private int _idserviciotransporte;
        private int _idtransporte_servicioavion1;
        private int idtransporte_serviciotren1;
        private int idtransporte_serviciocar1;

        public int Idtransporte_serviciocar1
        {
            get { return idtransporte_serviciocar1; }
            set { idtransporte_serviciocar1 = value; }
        }

        public int Idtransporte_serviciotren1
        {
            get { return idtransporte_serviciotren1; }
            set { idtransporte_serviciotren1 = value; }
        }

        public int Idtransporte_servicioavion1
        {
            get { return _idtransporte_servicioavion1; }
            set { _idtransporte_servicioavion1 = value; }
        }

        public int Idserviciotransporte
        {
            get { return _idserviciotransporte; }
            set { _idserviciotransporte = value; }
        }

        public int Idservicioactividad
        {
            get { return _idservicioactividad; }
            set { _idservicioactividad = value; }
        }


        public int Idserviciohotel
        {
            get { return _idserviciohotel; }
            set { _idserviciohotel = value; }
        }


        public int Idservicioinscripcion
        {
            get { return _idservicioinscripcion; }
            set { _idservicioinscripcion = value; }
        }

        public int Cotizado
        {
            get { return _Cotizado; }
            set { _Cotizado = value; }
        }

        public string Resumenservicio
        {
            get { return _resumenservicio; }
            set { _resumenservicio = value; }
        }


        public int Idservicio
        {
            get { return _idservicio; }
            set { _idservicio = value; }

        }

        public int Idreserva
        {
            get { return _idreserva; }
            set { _idreserva = value; }
        }

        public string IdTipoBono
        {
            get { return _IdTipoBono; }
            set { _IdTipoBono = value; }
        }

        public DateTime Fechapeticion
        {
            get { return _fechapeticion; }
            set { _fechapeticion = value; }
        }

        public int Locked
        {
            get { return _locked; }
            set { _locked = value; }
        }


        #endregion

        #region Converter

        public static ICollection<DServiciosReservas> ConvertToDto(DataTable dtTable)
        {
            ICollection<DServiciosReservas> collection = new Collection<DServiciosReservas>();
            foreach (DataRow row in dtTable.Rows)
            {
                DServiciosReservas data = new DServiciosReservas()
                {
                    Fechapeticion = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "Fechapeticion"),
                    Cotizado = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Cotizado"),
                    Idreserva = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idreserva"),
                    Idservicio = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idservicio"),
                    Idservicioactividad = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idservicioactividad"),
                    Idserviciohotel = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idserviciohotel"),
                    Idservicioinscripcion = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idservicioinscripcion"),
                    Idserviciotransporte = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idserviciotransporte"),
                    Idtransporte_servicioavion1 = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idtransporte_servicioavion1"),
                    Idtransporte_serviciocar1 = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idtransporte_serviciocar1"),
                    Idtransporte_serviciotren1 = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idtransporte_serviciotren1"),
                    Locked = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Locked"),
                    IdTipoBono = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdTipoBono"),
                    Resumenservicio = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Resumenservicio"),

                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
    }
}
