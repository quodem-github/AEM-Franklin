using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DTipoReserva
    {
        #region Propiedades
        private Int32 m_IdTipoReserva;

        //[PropiedadOriginal("IdTipoReserva", EsClave = true, TablaOriginal = "tiposreservas_web", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IdTipoReserva
        {
            get { return m_IdTipoReserva; }
            set
            {
                if (this.m_IdTipoReserva != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdTipoReserva", m_IdTipoReserva, value));
                    m_IdTipoReserva = value;

                }
            }
        }
        private String m_Descripcion;

        //[PropiedadOriginal("Descripcion", TablaOriginal = "tiposreservas_web", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 80)]
        public String Descripcion
        {
            get { return m_Descripcion; }
            set
            {
                if (this.m_Descripcion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Descripcion", m_Descripcion, value));
                    m_Descripcion = value;

                }
            }
        }
        #endregion

        #region Constructores

        public DTipoReserva()
        {
        }
        public DTipoReserva(Int32 _IdTipoReserva)
        {
            IdTipoReserva = _IdTipoReserva;
        }

        #endregion

        #region Converter

        public static IList<DTipoReserva> ConvertToDto(DataTable dtTable)
        {
            IList<DTipoReserva> collection = new Collection<DTipoReserva>();
            return collection;
        }

        #endregion
    }
}
