using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DEstadoExpediente
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
        #endregion

        #region Constructores

        public DEstadoExpediente()
        {
        }
        public DEstadoExpediente(String _idestado)
        {
            idestado = _idestado;
        }

        #endregion

        #region Converter

        public static IList<DEstadoExpediente> ConvertToDto(DataTable dtTable)
        {
            IList<DEstadoExpediente> collection = new Collection<DEstadoExpediente>();
            return collection;
        }

        #endregion
    }
}
