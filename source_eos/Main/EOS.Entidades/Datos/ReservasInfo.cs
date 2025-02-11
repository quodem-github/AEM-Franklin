using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class ReservasInfo
    {
        #region Propiedades
		private Int32 m_Idexpediente;

        //[PropiedadOriginal("IdExpediente", EsClave = false, TablaOriginal = "cv_reservas_enviadas_exp", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
		public Int32 Idexpediente
		{
			get { return m_Idexpediente; }
			set {
				if (this.m_Idexpediente != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Idexpediente", m_Idexpediente, value));
					m_Idexpediente = value;
					
				}
			}
		}

        private Int32 m_IdReserva;
        //[PropiedadOriginal("IdReserva", EsClave = false, TablaOriginal = "cv_reservas_enviadas_exp", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IdReserva
        {
            get { return m_IdReserva; }
            set
            {
                if (this.m_IdReserva != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdReserva", m_IdReserva, value));
                    m_IdReserva = value;

                }
            }
        }

        private String m_IdTipo;
        //[PropiedadOriginal("IdTipo", EsClave = false, TablaOriginal = "cv_reservas_enviadas_exp", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 3)]
        public String IdTipo
        {
            get { return m_IdTipo; }
            set
            {
                if (this.m_IdTipo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdTipo", m_IdTipo, value));
                    m_IdTipo = value;

                }
            }
        }

        private Nullable<Int32> m_IdServicioInscripcion;
        //[PropiedadOriginal("IdServicioInscripcion", EsClave = false, TablaOriginal = "cv_reservas_enviadas_exp", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> IdServicioInscripcion
        {
            get { return m_IdServicioInscripcion; }
            set
            {
                if (this.m_IdServicioInscripcion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdServicioInscripcion", m_IdServicioInscripcion, value));
                    m_IdServicioInscripcion = value;

                }
            }
        }

        private Nullable<Int32> m_IdServicioHotel;
        //[PropiedadOriginal("IdServicioHotel", EsClave = false, TablaOriginal = "cv_reservas_enviadas_exp", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> IdServicioHotel
        {
            get { return m_IdServicioHotel; }
            set
            {
                if (this.m_IdServicioHotel != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdServicioHotel", m_IdServicioHotel, value));
                    m_IdServicioHotel = value;

                }
            }
        }

        private Nullable<Int32> m_IdServicioActividad;
        //[PropiedadOriginal("IdServicioActividad", EsClave = false, TablaOriginal = "cv_reservas_enviadas_exp", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> IdServicioActividad
        {
            get { return m_IdServicioActividad; }
            set
            {
                if (this.m_IdServicioActividad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdServicioActividad", m_IdServicioActividad, value));
                    m_IdServicioActividad = value;

                }
            }
        }

        private Nullable<Int32> m_IdServicioTransporte;
        //[PropiedadOriginal("IdServicioTransporte", EsClave = false, TablaOriginal = "cv_reservas_enviadas_exp", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> IdServicioTransporte
        {
            get { return m_IdServicioTransporte; }
            set
            {
                if (this.m_IdServicioTransporte != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdServicioTransporte", m_IdServicioTransporte, value));
                    m_IdServicioTransporte = value;

                }
            }
        }
        private String m_idEstado;
        //[PropiedadOriginal("idestado", EsClave = false, TablaOriginal = "cv_reservas_enviadas_exp", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 5)]
        public String idestado
        {
            get { return m_idEstado; }
            set
            {
                if (this.m_idEstado != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idestado", m_idEstado, value));
                    m_idEstado = value;

                }
            }
        }
		#endregion

		#region Constructores

        public ReservasInfo()
		{
		}

		#endregion

        #region Converter

        public static IList<ReservasInfo> ConvertToDto(DataTable dtTable)
        {
            IList<ReservasInfo> collection = new Collection<ReservasInfo>();
            foreach (DataRow row in dtTable.Rows)
            {
                ReservasInfo data = new ReservasInfo()
                {
                    Idexpediente = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idexpediente"),
                    IdReserva = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdReserva"),
                    IdServicioActividad = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdServicioActividad"),
                    IdServicioHotel = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdServicioHotel"),
                    IdServicioInscripcion = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdServicioInscripcion"),
                    IdServicioTransporte = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdServicioTransporte"),
                    idestado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idestado"),
                    IdTipo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdTipo"),
                };

                collection.Add(data);

            }
            return collection;
        }

        #endregion
    }
}
