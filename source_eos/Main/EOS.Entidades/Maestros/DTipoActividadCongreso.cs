using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DTipoActividadCongreso
	{
		#region Propiedades
		private Int32 m_idtipoactividadcongreso;

		//[PropiedadOriginal("idtipoactividadcongreso" ,EsClave = true  , TablaOriginal = "tipos_actividad_congreso" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idtipoactividadcongreso
		{
			get { return m_idtipoactividadcongreso; }
			set {
				if (this.m_idtipoactividadcongreso != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtipoactividadcongreso", m_idtipoactividadcongreso, value));
					m_idtipoactividadcongreso = value;
					
				}
			}
		}
		private String m_tipoactividadcongreso;

		//[PropiedadOriginal("tipoactividadcongreso"   , TablaOriginal = "tipos_actividad_congreso" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 35)]
		public String tipoactividadcongreso
		{
			get { return m_tipoactividadcongreso; }
			set {
				if (this.m_tipoactividadcongreso != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("tipoactividadcongreso", m_tipoactividadcongreso, value));
					m_tipoactividadcongreso = value;
					
				}
			}
		}
		private Nullable<Int32> m_orden;

		//[PropiedadOriginal("orden"  ,EsNullable = true , TablaOriginal = "tipos_actividad_congreso" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> orden
		{
			get { return m_orden; }
			set {
				if (this.m_orden != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("orden", m_orden, value));
					m_orden = value;
					
				}
			}
		}
		private Nullable<Int32> m_locked;

		//[PropiedadOriginal("locked"  ,EsNullable = true , TablaOriginal = "tipos_actividad_congreso" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private String m_codtipoactividad;

		//[PropiedadOriginal("codtipoactividad"  ,EsNullable = true , TablaOriginal = "tipos_actividad_congreso" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String codtipoactividad
		{
			get { return m_codtipoactividad; }
			set {
				if (this.m_codtipoactividad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("codtipoactividad", m_codtipoactividad, value));
					m_codtipoactividad = value;
					
				}
			}
		}
		#endregion

		#region Constructores

		public DTipoActividadCongreso()
		{
		}
		public DTipoActividadCongreso(Int32 _idtipoactividadcongreso)
		{
			idtipoactividadcongreso = _idtipoactividadcongreso;
		}

		#endregion

        #region Converter

        public static IList<DTipoActividadCongreso> ConvertToDto(DataTable dtTable)
        {
            IList<DTipoActividadCongreso> collection = new Collection<DTipoActividadCongreso>();
            return collection;
        }

        #endregion
	}
}
