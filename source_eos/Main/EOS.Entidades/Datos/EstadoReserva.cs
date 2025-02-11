using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class EstadoReserva
    {
        #region Propiedades
        private String m_idestado;

        //[PropiedadOriginal("idestado", EsClave = true, TablaOriginal = "estadosreservas", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 5)]
        public String idestado
        {
            get { return m_idestado; }
            set
            {
                if (this.m_idestado != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idestado", m_idestado, value));
                    m_idestado = value;

                }
            }
        }
        private String m_estado;

        //[PropiedadOriginal("estado", TablaOriginal = "estadosreservas", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 30)]
        public String estado
        {
            get { return m_estado; }
            set
            {
                if (this.m_estado != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("estado", m_estado, value));
                    m_estado = value;

                }
            }
        }
        private Nullable<Int32> m_locked;

        //[PropiedadOriginal("locked", EsNullable = true, TablaOriginal = "estadosreservas", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> locked
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

        public EstadoReserva()
        {
        }
        public EstadoReserva(String _idestado)
        {
            idestado = _idestado;
        }

        #endregion
    }
}
