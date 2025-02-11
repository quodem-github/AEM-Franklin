using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DTipoPatrocinio
	{
		#region Propiedades
		private Int32 m_idtipopatrocinio;

		//[PropiedadOriginal("idtipopatrocinio" ,EsClave = true  , TablaOriginal = "tipospatrocinio" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idtipopatrocinio
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
		private String m_tipopatrocinio;

		//[PropiedadOriginal("tipopatrocinio"  ,EsNullable = true , TablaOriginal = "tipospatrocinio" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
		public String tipopatrocinio
		{
			get { return m_tipopatrocinio; }
			set {
				if (this.m_tipopatrocinio != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("tipopatrocinio", m_tipopatrocinio, value));
					m_tipopatrocinio = value;
					
				}
			}
		}
		private String m_tiponump;

		//[PropiedadOriginal("tiponump"  ,EsNullable = true , TablaOriginal = "tipospatrocinio" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String tiponump
		{
			get { return m_tiponump; }
			set {
				if (this.m_tiponump != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("tiponump", m_tiponump, value));
					m_tiponump = value;
					
				}
			}
		}
		private Nullable<Int32> m_locked;

		//[PropiedadOriginal("locked"  ,EsNullable = true , TablaOriginal = "tipospatrocinio" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		#endregion

		#region Constructores

		public DTipoPatrocinio()
		{
		}
		public DTipoPatrocinio(Int32 _idtipopatrocinio)
		{
			idtipopatrocinio = _idtipopatrocinio;
		}

		#endregion

        #region Converter

        public static IList<DTipoPatrocinio> ConvertToDto(DataTable dtTable)
        {
            IList<DTipoPatrocinio> collection = new Collection<DTipoPatrocinio>();
            return collection;
        }

        #endregion
	}
}
