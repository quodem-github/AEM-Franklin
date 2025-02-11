using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
	public class DAmecPassengerList
	{
		#region Propiedades
		private Nullable<Int32> m_locked;

		//[PropiedadOriginal("locked"  ,EsNullable = true , TablaOriginal = "amecs_passengers_list" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private Int32 m_idamecpassengerlist;

		//[PropiedadOriginal("idamecpassengerlist" ,EsClave = true  , TablaOriginal = "amecs_passengers_list" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idamecpassengerlist
		{
			get { return m_idamecpassengerlist; }
			set {
				if (this.m_idamecpassengerlist != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idamecpassengerlist", m_idamecpassengerlist, value));
					m_idamecpassengerlist = value;
					
				}
			}
		}
		private Int32 m_idamec;

		//[PropiedadOriginal("idamec"   , TablaOriginal = "amecs_passengers_list" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idamec
		{
			get { return m_idamec; }
			set {
				if (this.m_idamec != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idamec", m_idamec, value));
					m_idamec = value;
					
				}
			}
		}
		private Int32 m_idpassengerlist;

		//[PropiedadOriginal("idpassengerlist"   , TablaOriginal = "amecs_passengers_list" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private Nullable<Int32> m_idtipopatrocinio;

		//[PropiedadOriginal("idtipopatrocinio"  ,EsNullable = true , TablaOriginal = "amecs_passengers_list" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> idtipopatrocinio
		{
			get { return m_idtipopatrocinio; }
			set {
				if (this.m_idtipopatrocinio != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtipopatrocinio", m_idtipopatrocinio, value));
					m_idtipopatrocinio = value;
					
				}
			}
		}
		#endregion

		#region Constructores

		public DAmecPassengerList()
		{
		}
		public DAmecPassengerList(Int32 _idamecpassengerlist)
		{
			idamecpassengerlist = _idamecpassengerlist;
		}

		#endregion

        #region Converter

        public static ICollection<DAmecPassengerList> ConvertToDto(DataTable dtTable)
        {
            ICollection<DAmecPassengerList> collection = new Collection<DAmecPassengerList>();
            foreach (DataRow row in dtTable.Rows)
            {
                DAmecPassengerList data = new DAmecPassengerList()
                {
                    locked = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idamecs"),
                    idamecpassengerlist = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idamecs"),
                    idamec = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idamecs"),
                    idpassengerlist = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idamecs"),
                    idtipopatrocinio = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idamecs"),

                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
	}
}
