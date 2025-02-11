using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DEstadoAmec
    {
        #region Propiedades
        private UInt32 m_idestado;

        //[PropiedadOriginal("idestado", TablaOriginal = "estadoamecs", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public UInt32 idestado
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

        //[PropiedadOriginal("estado", TablaOriginal = "estadoamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 70)]
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

        public DEstadoAmec()
        {
        }

        #endregion

        #region Converter

        public static ICollection<DEstadoAmec> ConvertToDto(DataTable dtTable)
        {
            ICollection<DEstadoAmec> collection = new Collection<DEstadoAmec>();
            foreach (DataRow row in dtTable.Rows)
            {
                DEstadoAmec data = new DEstadoAmec()
                {
                    idestado = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetIntValue(row, "idestado").ToString()),
                    estado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "estado"),

                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion

    }



}
