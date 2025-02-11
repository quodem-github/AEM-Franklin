using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DVServicioHotel
	{
		#region Propiedades
		private Int32 m_idexpediente;

		//[PropiedadOriginal("idexpediente" ,EsClave = true  , TablaOriginal = "cv_serviciohoteles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idexpediente
		{
			get { return m_idexpediente; }
			set {
				if (this.m_idexpediente != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idexpediente", m_idexpediente, value));
					m_idexpediente = value;
					
				}
			}
		}
		private Int32 m_idreserva;

		//[PropiedadOriginal("idreserva" ,EsClave = true  , TablaOriginal = "cv_serviciohoteles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idreserva
		{
			get { return m_idreserva; }
			set {
				if (this.m_idreserva != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idreserva", m_idreserva, value));
					m_idreserva = value;
					
				}
			}
		}
		private Int32 m_idservicio;

		//[PropiedadOriginal("idservicio" ,EsClave = true  , TablaOriginal = "cv_serviciohoteles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idservicio
		{
			get { return m_idservicio; }
			set {
				if (this.m_idservicio != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idservicio", m_idservicio, value));
					m_idservicio = value;
					
				}
			}
		}
		private Int32 m_idserviciohotel;

		//[PropiedadOriginal("idserviciohotel" ,EsClave = true  , TablaOriginal = "cv_serviciohoteles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idserviciohotel
		{
			get { return m_idserviciohotel; }
			set {
				if (this.m_idserviciohotel != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idserviciohotel", m_idserviciohotel, value));
					m_idserviciohotel = value;
					
				}
			}
		}
		private String m_hotel;

		//[PropiedadOriginal("hotel"  ,EsNullable = true , TablaOriginal = "cv_serviciohoteles" , TipoProveedor = ClepsydraDbType.NText , Longitud = 21845)]
		public String hotel
		{
			get { return m_hotel; }
			set {
				if (this.m_hotel != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("hotel", m_hotel, value));
					m_hotel = value;
					
				}
			}
		}
		private DateTime m_fechahorallegada;

		//[PropiedadOriginal("fechahorallegada"   , TablaOriginal = "cv_serviciohoteles" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public DateTime fechahorallegada
		{
			get { return m_fechahorallegada; }
			set {
				if (this.m_fechahorallegada != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fechahorallegada", m_fechahorallegada, value));
					m_fechahorallegada = value;
					
				}
			}
		}
		private DateTime m_fechahorasalida;

		//[PropiedadOriginal("fechahorasalida"   , TablaOriginal = "cv_serviciohoteles" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public DateTime fechahorasalida
		{
			get { return m_fechahorasalida; }
			set {
				if (this.m_fechahorasalida != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fechahorasalida", m_fechahorasalida, value));
					m_fechahorasalida = value;
					
				}
			}
		}
		private String m_idtipohab;

		//[PropiedadOriginal("idtipohab"  ,EsNullable = true , TablaOriginal = "cv_serviciohoteles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 3)]
		public String idtipohab
		{
			get { return m_idtipohab; }
			set {
				if (this.m_idtipohab != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtipohab", m_idtipohab, value));
					m_idtipohab = value;
					
				}
			}
		}
		private Nullable<Double> m_pvp;

		//[PropiedadOriginal("pvp"  ,EsNullable = true , TablaOriginal = "cv_serviciohoteles" , TipoProveedor = ClepsydraDbType.BinaryDouble , Longitud = 22)]
		public Nullable<Double> pvp
		{
			get { return m_pvp; }
			set {
				if (this.m_pvp != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("pvp", m_pvp, value));
					m_pvp = value;
					
				}
			}
		}
		private String m_observaciones;

		//[PropiedadOriginal("observaciones"  ,EsNullable = true , TablaOriginal = "cv_serviciohoteles" , TipoProveedor = ClepsydraDbType.NText , Longitud = 21845)]
		public String observaciones
		{
			get { return m_observaciones; }
			set {
				if (this.m_observaciones != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("observaciones", m_observaciones, value));
					m_observaciones = value;
					
				}
			}
		}
		private Nullable<Int64> m_pax;

		//[PropiedadOriginal("pax"  ,EsNullable = true , TablaOriginal = "cv_serviciohoteles" , TipoProveedor = ClepsydraDbType.Int64 , Longitud = 21)]
		public Nullable<Int64> pax
		{
			get { return m_pax; }
			set {
				if (this.m_pax != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("pax", m_pax, value));
					m_pax = value;
					
				}
			}
		}
		private Nullable<Single> m_cotizado;

		//[PropiedadOriginal("cotizado"  ,EsNullable = true , TablaOriginal = "cv_serviciohoteles" , TipoProveedor = ClepsydraDbType.Double , Longitud = 12)]
		public Nullable<Single> cotizado
		{
			get { return m_cotizado; }
			set {
				if (this.m_cotizado != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("cotizado", m_cotizado, value));
					m_cotizado = value;
					
				}
			}
		}
		private String m_IdEstado;

		//[PropiedadOriginal("IdEstado"   , TablaOriginal = "cv_serviciohoteles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
		public String IdEstado
		{
			get { return m_IdEstado; }
			set {
				if (this.m_IdEstado != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEstado", m_IdEstado, value));
					m_IdEstado = value;
					
				}
			}
		}
		#endregion

		#region Constructores

		public DVServicioHotel()
		{
		}
		public DVServicioHotel(Int32 _idexpediente ,Int32 _idreserva ,Int32 _idservicio ,Int32 _idserviciohotel)
		{
			idexpediente = _idexpediente;
			idreserva = _idreserva;
			idservicio = _idservicio;
			idserviciohotel = _idserviciohotel;
		}

		#endregion

        #region Converter

        public static ICollection<DVServicioHotel> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVServicioHotel> collection = new Collection<DVServicioHotel>();

            foreach (DataRow row in dtTable.Rows)
            {
                DVServicioHotel data = new DVServicioHotel()
                {
                    idexpediente = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idexpediente")),
                    idreserva = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idreserva")),
                    idservicio = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idservicio")),
                    idserviciohotel = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idserviciohotel")),
                    hotel = Quodem.Utility.DataLayerUtil.GetStringValue(row, "hotel"),
                    pvp = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "pvp")) ? new Nullable<double>() : double.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "pvp"))),
                    observaciones = Quodem.Utility.DataLayerUtil.GetStringValue(row, "observaciones"),
                    pax = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "pax")) ? new Nullable<long>() : long.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "pax"))),
                    cotizado = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "cotizado")) ? new Nullable<float>() : float.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "cotizado"))),
                    IdEstado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEstado"),
                    fechahorasalida = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechahorasalida"),
                    fechahorallegada = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechahorallegada"),
                    idtipohab = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idtipohab")

                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
	}
}
