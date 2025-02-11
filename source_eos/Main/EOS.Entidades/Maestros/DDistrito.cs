using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DDistrito
    {
        #region Propiedades
        private Int32 m_iddistrito;

        //[PropiedadOriginal("iddistrito", EsClave = true, TablaOriginal = "distritos", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 iddistrito
        {
            get { return m_iddistrito; }
            set
            {
                if (this.m_iddistrito != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("iddistrito", m_iddistrito, value));
                    m_iddistrito = value;

                }
            }
        }
        private Int32 m_IdEmpresa;

        //[PropiedadOriginal("IdEmpresa", TablaOriginal = "distritos", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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
        private Int32 m_idregion;

        //[PropiedadOriginal("idregion", TablaOriginal = "distritos", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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
        private String m_distrito;

        //[PropiedadOriginal("distrito", TablaOriginal = "distritos", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 80)]
        public String distrito
        {
            get { return m_distrito; }
            set
            {
                if (this.m_distrito != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("distrito", m_distrito, value));
                    m_distrito = value;

                }
            }
        }
        private Nullable<Int32> m_locked;

        //[PropiedadOriginal("locked", EsNullable = true, TablaOriginal = "distritos", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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

        public DDistrito()
        {
        }
        public DDistrito(Int32 _iddistrito)
        {
            iddistrito = _iddistrito;
        }

        #endregion

        #region Converter

        public static IList<DDistrito> ConvertToDto(DataTable dtTable)
        {
            IList<DDistrito> collection = new Collection<DDistrito>();

            foreach (DataRow row in dtTable.Rows)
            {
                DDistrito data = new DDistrito()
                {
                    iddistrito = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "iddistrito")),
                    IdEmpresa = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEmpresa")),
                    idregion = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idregion")),
                    distrito = Quodem.Utility.DataLayerUtil.GetStringValue(row, "distrito"),
                    locked = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "locked")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "locked"))),
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
    }
}
