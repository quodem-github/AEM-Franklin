using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;



namespace EOS.Entidades.Datos
{
    public class DVProductos 
    {
        #region Propiedades
        private Int32 m_idareaProducto;

        //[PropiedadOriginal("IdareaProductoempresa", TablaOriginal = "areas_productosempresa", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 idareaProducto
        {
            get { return m_idareaProducto; }
            set
            {
                if (this.m_idareaProducto != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdareaProductoempresa", m_idareaProducto, value));
                    m_idareaProducto = value;

                }
            }
        }
        private Int32 m_idProducto;

        //[PropiedadOriginal("IdProductoempresa", TablaOriginal = "areas_productosempresa", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 idProducto
        {
            get { return m_idProducto; }
            set
            {
                if (this.m_idProducto != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdProductoempresa", m_idProducto, value));
                    m_idProducto = value;

                }
            }
        }
        private String m_Producto;

        //[PropiedadOriginal("Productoempresa", TablaOriginal = "areas_productosempresa", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 35)]
        public String Producto
        {
            get { return m_Producto; }
            set
            {
                if (this.m_Producto != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Productoempresa", m_Producto, value));
                    m_Producto = value;

                }
            }
        }
        #endregion
        
        #region Constructores

        public DVProductos()
		{
		}

		#endregion

        #region Converter

        public static IList<DVProductos> ConvertToDto(DataTable dtTable)
        {
            IList<DVProductos> collection = new Collection<DVProductos>();

            foreach(DataRow row in dtTable.Rows)
            {
                DVProductos data = new DVProductos()
                {
                    idareaProducto = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdareaProductoempresa")),
                    idProducto = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdProductoempresa")),
                    Producto = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Productoempresa")
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
    }
}
