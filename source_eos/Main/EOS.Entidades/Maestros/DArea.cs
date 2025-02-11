using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DArea
    {
        #region Propiedades
        private Int32 m_Idarea;

        //[PropiedadOriginal("Idarea", EsClave = true, TablaOriginal = "areas", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 Idarea
        {
            get { return m_Idarea; }
            set
            {
                if (this.m_Idarea != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Idarea", m_Idarea, value));
                    m_Idarea = value;

                }
            }
        }
        private Int32 m_FKIdEmpresa;

        //[PropiedadOriginal("FKIdEmpresa", TablaOriginal = "areas", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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
        private Int32 m_idunidad;

        //[PropiedadOriginal("idunidad", TablaOriginal = "areas", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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
        private String m_Codarea;

        //[PropiedadOriginal("Codarea", TablaOriginal = "areas", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 10)]
        public String Codarea
        {
            get { return m_Codarea; }
            set
            {
                if (this.m_Codarea != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Codarea", m_Codarea, value));
                    m_Codarea = value;

                }
            }
        }
        private String m_area;

        //[PropiedadOriginal("area", TablaOriginal = "areas", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 40)]
        public String area
        {
            get { return m_area; }
            set
            {
                if (this.m_area != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("area", m_area, value));
                    m_area = value;

                }
            }
        }
        private Nullable<Int32> m_inactivo;

        //[PropiedadOriginal("inactivo", EsNullable = true, TablaOriginal = "areas", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> inactivo
        {
            get { return m_inactivo; }
            set
            {
                if (this.m_inactivo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("inactivo", m_inactivo, value));
                    m_inactivo = value;

                }
            }
        }
        private Nullable<Int32> m_locked;

        //[PropiedadOriginal("locked", EsNullable = true, TablaOriginal = "areas", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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

        public DArea()
        {
        }
        public DArea(Int32 _Idarea)
        {
            Idarea = _Idarea;
        }

        #endregion

        #region Converter

        public static IList<DArea> ConvertToDto(DataTable dtTable)
        {
            IList<DArea> collection = new Collection<DArea>();

            foreach (DataRow row in dtTable.Rows)
            {
                DArea data = new DArea()
                {
                    Idarea = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Idarea")),
                    FKIdEmpresa = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "FKIdEmpresa")),
                    idunidad = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idunidad")),
                    Codarea = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Codarea"),
                    area = Quodem.Utility.DataLayerUtil.GetStringValue(row, "area"),
                    inactivo = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "inactivo")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "inactivo"))),
                    locked = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "locked")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "locked"))),
                };

                collection.Add(data);
            }
            return collection;
        }

        #endregion
    }
}
