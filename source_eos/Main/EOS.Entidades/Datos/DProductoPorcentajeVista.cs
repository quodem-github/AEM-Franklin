using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;


namespace EOS.Entidades.Datos
{
    public class DProductoPorcentajeVista
    {
        #region Propiedades

        private Int32 m_idareaProductoempresa;
        public Int32 IdareaProductoempresa
        {
            get { return m_idareaProductoempresa; }
            set
            {
                if (this.m_idareaProductoempresa != value)
                {
                    m_idareaProductoempresa = value;

                }
            }
        }
        
        private String m_producto;

        //[PropiedadOriginal("Productoempresa", TablaOriginal = "cv_productos_expedientes", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 30)]
		public String Producto
		{
            get { return m_producto; }
			set {
                if (this.m_producto != value)
				{
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Producto", m_producto, value));
                    m_producto = value;
					
				}
			}
		}
		private Int32 m_Porcentaje;

        //[PropiedadOriginal("porcentaje", TablaOriginal = "cv_productos_expedientes", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 Porcentaje
		{
            get { return m_Porcentaje; }
			set {
                if (this.m_Porcentaje != value)
				{
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Porcentaje", m_Porcentaje, value));
                    m_Porcentaje = value;
					
				}
			}
		}
		
		#endregion

		#region Constructores

        public DProductoPorcentajeVista()
		{
		}
		

		#endregion

        #region Converter

        public static ICollection<DProductoPorcentajeVista> ConvertToDto(DataTable dtTable)
        {
            ICollection<DProductoPorcentajeVista> collection = new Collection<DProductoPorcentajeVista>();
            foreach (DataRow row in dtTable.Rows)
            {
                DProductoPorcentajeVista data = new DProductoPorcentajeVista()
                {
                    IdareaProductoempresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdareaProductoempresa"),
                    Porcentaje = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Porcentaje"),
                    Producto = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Producto"),


                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
    }
}
