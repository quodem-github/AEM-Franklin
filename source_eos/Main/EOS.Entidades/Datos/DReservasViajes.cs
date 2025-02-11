using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


/*Pacifico 07012011*/
namespace EOS.Entidades.Datos
{
    public class DReservasViajes
    {
       
        #region Constructores

		public DReservasViajes()
		{
		}
        public DReservasViajes(Int32 _IDENTIFICADOR)
		{
			IDENTIFICADOR = _IDENTIFICADOR;
		}

		#endregion
        
        #region Propiedades
        private Int32 m_IDENTIFICADOR;

        //[PropiedadOriginal("IDENTIFICADOR", EsClave = true, TablaOriginal = "reservasviajes", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IDENTIFICADOR
        {
            get { return m_IDENTIFICADOR; }
            set
            {
                if (this.m_IDENTIFICADOR != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IDENTIFICADOR", m_IDENTIFICADOR, value));
                    m_IDENTIFICADOR = value;

                }
            }
        }

        private int _idreserva;

        public int Idreserva
        {
            get { return _idreserva; }
            set { _idreserva = value; }
        }
        private string _reserva;

        public string Reserva
        {
            get { return _reserva; }
            set { _reserva = value; }
        }
        private DateTime _fechapeticion;

        public DateTime Fechapeticion
        {
            get { return _fechapeticion; }
            set { _fechapeticion = value; }
        }
        private string _idestado;

        public string Idestado
        {
            get { return _idestado; }
            set { _idestado = value; }
        }
        private DateTime _LastUpd;

        public DateTime LastUpd
        {
            get { return _LastUpd; }
            set { _LastUpd = value; }
        }
        private string _LastLog;

        public string LastLog
        {
            get { return _LastLog; }
            set { _LastLog = value; }
        }
        private int _IdPeticionario;

        public int IdPeticionario
        {
            get { return _IdPeticionario; }
            set { _IdPeticionario = value; }
        }
        private string _Observaciones;

        public string Observaciones
        {
            get { return _Observaciones; }
            set { _Observaciones = value; }
        }
        private string _observ_agencia;

        public string Observ_agencia
        {
            get { return _observ_agencia; }
            set { _observ_agencia = value; }
        }
        private string _mainreserva;

        public string Mainreserva
        {
            get { return _mainreserva; }
            set { _mainreserva = value; }
        }
        private int _fkidexpediente;

        public int Fkidexpediente
        {
            get { return _fkidexpediente; }
            set { _fkidexpediente = value; }
        }
        private int _locked;

        public int Locked
        {
            get { return _locked; }
            set { _locked = value; }
        }


        #endregion

        #region Converter

        public static ICollection<DReservasViajes> ConvertToDto(DataTable dtTable)
        {
            ICollection<DReservasViajes> collection = new Collection<DReservasViajes>();
            foreach (DataRow row in dtTable.Rows)
            {
                DReservasViajes data = new DReservasViajes()
                {
                    Fechapeticion = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "Fechapeticion"),
                    LastUpd = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "LastUpd"),
                    Fkidexpediente = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Fkidexpediente"),
                    IdPeticionario = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdPeticionario"),
                    Idreserva = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idreserva"),
                    Locked = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Locked"),
                    IDENTIFICADOR = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IDENTIFICADOR"),
                    Idestado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Idestado"),
                    LastLog = Quodem.Utility.DataLayerUtil.GetStringValue(row, "LastLog"),
                    Mainreserva = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Mainreserva"),
                    Observ_agencia = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Observ_agencia"),
                    Observaciones = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Observaciones"),
                    Reserva = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Reserva"),


                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
    }
}
