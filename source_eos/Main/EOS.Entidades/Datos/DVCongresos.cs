using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DVCongresos
	{
		#region Propiedades
		private Int32 m_IdCongreso;

		//[PropiedadOriginal("IdCongreso"   , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 IdCongreso
		{
			get { return m_IdCongreso; }
			set {
				if (this.m_IdCongreso != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdCongreso", m_IdCongreso, value));
					m_IdCongreso = value;
					
				}
			}
		}
		private String m_Congreso;

		//[PropiedadOriginal("Congreso"   , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 70)]
		public String Congreso
		{
			get { return m_Congreso; }
			set {
				if (this.m_Congreso != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Congreso", m_Congreso, value));
					m_Congreso = value;
					
				}
			}
		}
        /*Pacifico 07012011*/
        private Int32 m_IdPoblacion;

        //[PropiedadOriginal("IdPoblacion", TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IdPoblacion
        {
            get { return m_IdPoblacion; }
            set
            {
                if (this.m_IdPoblacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdPoblacion", m_IdPoblacion, value));
                    m_IdPoblacion = value;

                }
            }
        }

        private Nullable<DateTime> m_desde;

        //[PropiedadOriginal("Desde", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public Nullable<DateTime> Desde
        {
            get { return m_desde; }
            set
            {
                if (this.m_desde != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Desde", m_desde, value));
                    m_desde = value;

                }
            }
        }

        private Nullable<DateTime> m_hasta;
        //[PropiedadOriginal("Hasta", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public Nullable<DateTime> Hasta
        {
            get { return m_hasta; }
            set
            {
                if (this.m_hasta != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Hasta", m_hasta, value));
                    m_hasta = value;

                }
            }
        }
		#endregion

		#region Constructores

		public DVCongresos()
		{
		}

		#endregion

        #region Converter

        public static ICollection<DVCongresos> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVCongresos> collection = new Collection<DVCongresos>();

            foreach (DataRow row in dtTable.Rows)
            {
                DVCongresos data = new DVCongresos()
                {
                    IdCongreso = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdCongreso")),
                    Congreso = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Congreso"),
                    IdPoblacion = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdPoblacion")),
                    Desde = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "Desde") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "Desde"),
                    Hasta = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "Hasta") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "Hasta"),
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion

        #region Converter

        public static IList<DVCongresos> ConvertToIListDto(DataTable dtTable)
        {
            IList<DVCongresos> collection = new Collection<DVCongresos>();
            foreach (DataRow row in dtTable.Rows)
            {
                DVCongresos data = new DVCongresos()
                {
                    IdCongreso = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdCongreso"),
                    IdPoblacion = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdPoblacion"),
                    Desde = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "Desde") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "Desde"),
                    Hasta = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "Hasta") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "Hasta"),
                    Congreso = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Congreso"),
                };

                collection.Add(data);
            }
            return collection;
        }

        #endregion
	}
}
