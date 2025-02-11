using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DUnidad
    {
        #region Propiedades
        private Int32 m_idunidad;

        //[PropiedadOriginal("idunidad", EsClave = true, TablaOriginal = "unidades", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 idunidad
        {
            get { return m_idunidad; }
            set
            {
                if (this.m_idunidad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idunidad", m_idunidad, value));
                    m_idunidad = value;

                }
            }
        }
        private Int32 m_IdEmpresa;

        //[PropiedadOriginal("IdEmpresa", TablaOriginal = "unidades", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IdEmpresa
        {
            get { return m_IdEmpresa; }
            set
            {
                if (this.m_IdEmpresa != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEmpresa", m_IdEmpresa, value));
                    m_IdEmpresa = value;

                }
            }
        }
        private String m_codigo;

        //[PropiedadOriginal("codigo", TablaOriginal = "unidades", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 5)]
        public String codigo
        {
            get { return m_codigo; }
            set
            {
                if (this.m_codigo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("codigo", m_codigo, value));
                    m_codigo = value;

                }
            }
        }
        private String m_unidad;

        //[PropiedadOriginal("unidad", TablaOriginal = "unidades", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 80)]
        public String unidad
        {
            get { return m_unidad; }
            set
            {
                if (this.m_unidad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("unidad", m_unidad, value));
                    m_unidad = value;

                }
            }
        }
        private Nullable<Int32> m_locked;

        //[PropiedadOriginal("locked", EsNullable = true, TablaOriginal = "unidades", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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

        public DUnidad()
        {
        }
        public DUnidad(Int32 _idunidad)
        {
            idunidad = _idunidad;
        }

        #endregion

        #region Converter

        public static IList<DUnidad> ConvertToDto(DataTable dtTable)
        {
            IList<DUnidad> collection = new List<DUnidad>();

            foreach (DataRow row in dtTable.Rows)
            {
                DUnidad data = new DUnidad()
                {
                    idunidad = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idunidad")),
                    IdEmpresa = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEmpresa")),
                    codigo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "codigo"),
                    unidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "unidad"),
                    locked = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "locked")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "locked"))),
                };

                collection.Add(data);
            }
            return collection;
        }

        #endregion
    }
}
