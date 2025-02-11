using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DRegion
    {
        #region Propiedades
        private Int32 m_idregion;

        //[PropiedadOriginal("idregion", EsClave = true, TablaOriginal = "regiones", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 idregion
        {
            get { return m_idregion; }
            set
            {
                if (this.m_idregion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idregion", m_idregion, value));
                    m_idregion = value;

                }
            }
        }
        private Int32 m_FKIdEmpresa;

        //[PropiedadOriginal("FKIdEmpresa", TablaOriginal = "regiones", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 FKIdEmpresa
        {
            get { return m_FKIdEmpresa; }
            set
            {
                if (this.m_FKIdEmpresa != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FKIdEmpresa", m_FKIdEmpresa, value));
                    m_FKIdEmpresa = value;

                }
            }
        }
        private String m_region;

        //[PropiedadOriginal("region", TablaOriginal = "regiones", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 25)]
        public String region
        {
            get { return m_region; }
            set
            {
                if (this.m_region != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("region", m_region, value));
                    m_region = value;

                }
            }
        }
        private Nullable<Int32> m_locked;

        //[PropiedadOriginal("locked", EsNullable = true, TablaOriginal = "regiones", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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

        public DRegion()
        {
        }
        public DRegion(Int32 _idregion)
        {
            idregion = _idregion;
        }

        #endregion

        #region Converter

        public static IList<DRegion> ConvertToDto(DataTable dtTable)
        {
            IList<DRegion> collection = new Collection<DRegion>();

            foreach (DataRow row in dtTable.Rows)
            {
                DRegion data = new DRegion()
                {
                    idregion = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idregion")),
                    FKIdEmpresa = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "FKIdEmpresa")),
                    region = Quodem.Utility.DataLayerUtil.GetStringValue(row, "region"),
                    locked = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "locked")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "locked"))),
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
    }
}
