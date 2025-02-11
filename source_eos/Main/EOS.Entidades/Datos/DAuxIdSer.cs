using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
	public class DAuxIdSer
	{
		#region Propiedades
		private Int32 m_IDENTIFICADOR;

		//[PropiedadOriginal("IDENTIFICADOR" ,EsClave = true  , TablaOriginal = "serviciosreservasviajes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 IDENTIFICADOR
		{
			get { return m_IDENTIFICADOR; }
			set {
				if (this.m_IDENTIFICADOR != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IDENTIFICADOR", m_IDENTIFICADOR, value));
					m_IDENTIFICADOR = value;
					
				}
			}
		}
		private Nullable<Int32> m_idservicioinscripcion;

		//[PropiedadOriginal("idservicioinscripcion"  ,EsNullable = true , TablaOriginal = "serviciosreservasviajes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> idservicioinscripcion
		{
			get { return m_idservicioinscripcion; }
			set {
				if (this.m_idservicioinscripcion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idservicioinscripcion", m_idservicioinscripcion, value));
					m_idservicioinscripcion = value;
					
				}
			}
		}
		private Nullable<Int32> m_idserviciohotel;

		//[PropiedadOriginal("idserviciohotel"  ,EsNullable = true , TablaOriginal = "serviciosreservasviajes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> idserviciohotel
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
		private Nullable<Int32> m_idservicioactividad;

		//[PropiedadOriginal("idservicioactividad"  ,EsNullable = true , TablaOriginal = "serviciosreservasviajes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> idservicioactividad
		{
			get { return m_idservicioactividad; }
			set {
				if (this.m_idservicioactividad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idservicioactividad", m_idservicioactividad, value));
					m_idservicioactividad = value;
					
				}
			}
		}
		private Nullable<Int32> m_idserviciotransporte;

		//[PropiedadOriginal("idserviciotransporte"  ,EsNullable = true , TablaOriginal = "serviciosreservasviajes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> idserviciotransporte
		{
			get { return m_idserviciotransporte; }
			set {
				if (this.m_idserviciotransporte != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idserviciotransporte", m_idserviciotransporte, value));
					m_idserviciotransporte = value;
					
				}
			}
		}
		#endregion

		#region Constructores

		public DAuxIdSer()
		{
		}
		public DAuxIdSer(Int32 _IDENTIFICADOR)
		{
			IDENTIFICADOR = _IDENTIFICADOR;
		}

		#endregion

        #region Converter

        public static ICollection<DAuxIdSer> ConvertToDto(DataTable dtTable)
        {
            ICollection<DAuxIdSer> collection = new Collection<DAuxIdSer>();
            foreach (DataRow row in dtTable.Rows)
            {
                DAuxIdSer data = new DAuxIdSer()
                {
                    IDENTIFICADOR = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IDENTIFICADOR"),
                    idservicioactividad = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idservicioactividad"),
                    idserviciohotel = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idserviciohotel"),
                    idservicioinscripcion = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idservicioinscripcion"),
                    idserviciotransporte = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idserviciotransporte"),


                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
	}
}
