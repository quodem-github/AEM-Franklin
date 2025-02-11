using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DTratamiento
	{
		#region Propiedades
		private Int32 m_idtratamiento;

		//[PropiedadOriginal("idtratamiento" ,EsClave = true  , TablaOriginal = "tratamientos" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idtratamiento
		{
			get { return m_idtratamiento; }
			set {
				if (this.m_idtratamiento != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtratamiento", m_idtratamiento, value));
					m_idtratamiento = value;
					
				}
			}
		}
		private String m_ttocarta;

		//[PropiedadOriginal("ttocarta"   , TablaOriginal = "tratamientos" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String ttocarta
		{
			get { return m_ttocarta; }
			set {
				if (this.m_ttocarta != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ttocarta", m_ttocarta, value));
					m_ttocarta = value;
					
				}
			}
		}
		private String m_ttocertificado;

		//[PropiedadOriginal("ttocertificado"  ,EsNullable = true , TablaOriginal = "tratamientos" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String ttocertificado
		{
			get { return m_ttocertificado; }
			set {
				if (this.m_ttocertificado != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ttocertificado", m_ttocertificado, value));
					m_ttocertificado = value;
					
				}
			}
		}
		private String m_ttomsd;

		//[PropiedadOriginal("ttomsd"  ,EsNullable = true , TablaOriginal = "tratamientos" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String ttomsd
		{
			get { return m_ttomsd; }
			set {
				if (this.m_ttomsd != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ttomsd", m_ttomsd, value));
					m_ttomsd = value;
					
				}
			}
		}
		private Nullable<Int32> m_locked;

		//[PropiedadOriginal("locked"  ,EsNullable = true , TablaOriginal = "tratamientos" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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

		public DTratamiento()
		{
		}
		public DTratamiento(Int32 _idtratamiento)
		{
			idtratamiento = _idtratamiento;
		}

		#endregion

        #region Converter

        public static IList<DTratamiento> ConvertToDto(DataTable dtTable)
        {
            IList<DTratamiento> collection = new Collection<DTratamiento>();
            return collection;
        }

        #endregion
	}
}
