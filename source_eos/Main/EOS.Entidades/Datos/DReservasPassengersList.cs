using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DReservasPassengersList
	{
		#region Propiedades
		private Int32 m_idreservapassengerlist;

		//[PropiedadOriginal("idreservapassengerlist" ,EsClave = true  , TablaOriginal = "reservas_passengers_list" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idreservapassengerlist
		{
			get { return m_idreservapassengerlist; }
			set {
				if (this.m_idreservapassengerlist != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idreservapassengerlist", m_idreservapassengerlist, value));
					m_idreservapassengerlist = value;
					
				}
			}
		}
		private Int32 m_idpassengerlist;

		//[PropiedadOriginal("idpassengerlist"   , TablaOriginal = "reservas_passengers_list" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idpassengerlist
		{
			get { return m_idpassengerlist; }
			set {
				if (this.m_idpassengerlist != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idpassengerlist", m_idpassengerlist, value));
					m_idpassengerlist = value;
					
				}
			}
		}
		private Nullable<Int32> m_locked;

		//[PropiedadOriginal("locked"  ,EsNullable = true , TablaOriginal = "reservas_passengers_list" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> locked
		{
			get { return m_locked; }
			set {
				if (this.m_locked != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("locked", m_locked, value));
					m_locked = value;
					
				}
			}
		}
		private Int32 m_idxpediente;

		//[PropiedadOriginal("idxpediente"   , TablaOriginal = "reservas_passengers_list" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idxpediente
		{
			get { return m_idxpediente; }
			set {
				if (this.m_idxpediente != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idxpediente", m_idxpediente, value));
					m_idxpediente = value;
					
				}
			}
		}
		#endregion

        #region Constructores

		public DReservasPassengersList()
		{
		}
		public DReservasPassengersList(Int32 _idreservapassengerlist)
		{
			idreservapassengerlist = _idreservapassengerlist;
		}

		#endregion

        #region Converter

        public static ICollection<DReservasPassengersList> ConvertToDto(DataTable dtTable)
        {
            ICollection<DReservasPassengersList> collection = new Collection<DReservasPassengersList>();
            foreach (DataRow row in dtTable.Rows)
            {
                DReservasPassengersList data = new DReservasPassengersList()
                {
                    idpassengerlist = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idpassengerlist"),
                    idreservapassengerlist = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idreservapassengerlist"),
                    idxpediente = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idxpediente"),
                    locked = Quodem.Utility.DataLayerUtil.GetIntValue(row, "locked"),

                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
	}
}
