using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class ExpAreaProductoEmpresa
    {
         #region Propiedades
        private Int32 m_idExpedienteProductoEmpresa;

        //[PropiedadOriginal("idexpedienteproductoempresa", TablaOriginal = "expedientes_areasproductosempresa", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 idExpedienteProductoEmpresa
        {
            get { return m_idExpedienteProductoEmpresa; }
            set
            {
                if (this.m_idExpedienteProductoEmpresa != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idexpedienteproductoempresa", m_idExpedienteProductoEmpresa, value));
                    m_idExpedienteProductoEmpresa = value;

                }
            }
        }
        private Int32 m_idExpediente;

        //[PropiedadOriginal("idexpediente", TablaOriginal = "expedientes_areasproductosempresa", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 idExpediente
        {
            get { return m_idExpediente; }
            set
            {
                if (this.m_idExpediente != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idexpediente", m_idExpediente, value));
                    m_idExpediente = value;

                }
            }
        }

        private Int32 m_idAreaProductoEmpresa;

        //[PropiedadOriginal("IdareaProductoempresa", TablaOriginal = "expedientes_areasproductosempresa", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 idAreaProductoEmpresa
        {
            get { return m_idAreaProductoEmpresa; }
            set
            {
                if (this.m_idAreaProductoEmpresa != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdareaProductoempresa", m_idAreaProductoEmpresa, value));
                    m_idAreaProductoEmpresa = value;

                }
            }
        }

        private Int32 m_porcentaje;

        //[PropiedadOriginal("porcentaje", TablaOriginal = "expedientes_areasproductosempresa", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 porcentaje
        {
            get { return m_porcentaje; }
            set
            {
                if (this.m_porcentaje != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("porcentaje", m_porcentaje, value));
                    m_porcentaje = value;

                }
            }
        }

        private Int32 m_locked;

        //[PropiedadOriginal("locked", TablaOriginal = "expedientes_areasproductosempresa", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 locked
        {
            get { return m_locked; }
            set
            {
                if (this.m_locked != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("locked", m_locked, value));
                    m_locked = value;

                }
            }
        }
        
        #endregion

        #region Constructores

        public ExpAreaProductoEmpresa()
		{
		}

		#endregion
    }
}
