using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DCriterioSeleccion
	{
		#region Propiedades
		private Int32 m_idcriterio;

		//[PropiedadOriginal("idcriterio" ,EsClave = true  , TablaOriginal = "criteriosseleccion" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idcriterio
		{
			get { return m_idcriterio; }
			set {
				if (this.m_idcriterio != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idcriterio", m_idcriterio, value));
					m_idcriterio = value;
					
				}
			}
		}
		private String m_criterioseleccion;

		//[PropiedadOriginal("criterioseleccion"  ,EsNullable = true , TablaOriginal = "criteriosseleccion" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String criterioseleccion
		{
			get { return m_criterioseleccion; }
			set {
				if (this.m_criterioseleccion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("criterioseleccion", m_criterioseleccion, value));
					m_criterioseleccion = value;
					
				}
			}
		}
		private Nullable<Int32> m_locked;

		//[PropiedadOriginal("locked"  ,EsNullable = true , TablaOriginal = "criteriosseleccion" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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

		public DCriterioSeleccion()
		{
		}
		public DCriterioSeleccion(Int32 _idcriterio)
		{
			idcriterio = _idcriterio;
		}

		#endregion

        #region Converter

        public static IList<DCriterioSeleccion> ConvertToDto(DataTable dtTable)
        {
            IList<DCriterioSeleccion> collection = new Collection<DCriterioSeleccion>();
            foreach (DataRow row in dtTable.Rows)
            {
                DCriterioSeleccion data = new DCriterioSeleccion()
                {
                    idcriterio = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcriterio"),
                    criterioseleccion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "criterioseleccion"),
                    locked = Quodem.Utility.DataLayerUtil.GetIntValue(row, "locked"),

                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
	}
}
