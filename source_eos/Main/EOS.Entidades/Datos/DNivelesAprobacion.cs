using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DNivelesAprobacion
    {
        #region Propiedades

        private UInt32 m_idnivelaprobacion;

        //[PropiedadOriginal("idnivelaprobacion", TablaOriginal = "nivelesaprobacion", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public UInt32 idnivelaprobacion
        {
            get { return m_idnivelaprobacion; }
            set
            {
                if (this.m_idnivelaprobacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idnivelaprobacion", m_idnivelaprobacion, value));
                    m_idnivelaprobacion = value;

                }
            }
        }


        private String m_nivelaprobacion;

        //[PropiedadOriginal("nivelaprobacion", TablaOriginal = "nivelesaprobacion", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        public String nivelaprobacion
        {
            get { return m_nivelaprobacion; }
            set
            {
                if (this.m_nivelaprobacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("nivelaprobacion", m_nivelaprobacion, value));
                    m_nivelaprobacion = value;

                }
            }
        }


        private UInt32 m_idcargo;

        //[PropiedadOriginal("idcargo", TablaOriginal = "nivelesaprobacion", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public UInt32 idcargo
        {
            get { return m_idcargo; }
            set
            {
                if (this.m_idcargo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idcargo", m_idcargo, value));
                    m_idcargo = value;

                }
            }
        }

        #endregion

        #region constructor
        public DNivelesAprobacion() { }
        public DNivelesAprobacion(UInt32 idnivelaprobacion)
        {
        }

        #endregion

        #region Converter

        public static ICollection<DNivelesAprobacion> ConvertToDto(DataTable dtTable)
        {
            ICollection<DNivelesAprobacion> collection = new Collection<DNivelesAprobacion>();
            foreach (DataRow row in dtTable.Rows)
            {
                DNivelesAprobacion data = new DNivelesAprobacion()
                {
                    idnivelaprobacion = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idnivelaprobacion")),
                    nivelaprobacion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nivelaprobacion"),
                    idcargo = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idcargo")),

                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion

    }
}
