using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DUnidadesOrganizativasAmec
    {
        
        #region Propiedades
        private Int64 m_idunidadamec;

        //[PropiedadOriginal("idunidadamec", EsClave = true, TablaOriginal = "unidorganizamecs", TipoProveedor = ClepsydraDbType.Int64, Longitud = 11)]
        public Int64 idunidadamec
        {
            get { return m_idunidadamec; }
            set
            {
                if (this.m_idunidadamec != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idunidadamec", m_idunidadamec, value));
                    m_idunidadamec = value;

                }
            }
        }
           



            private string m_idamecs;

            //[PropiedadOriginal("idamecs", EsClave = true, TablaOriginal = "unidorganizamecs", TipoProveedor = ClepsydraDbType.Int64, Longitud = 11)]
            public string idamecs
            {
                get { return m_idamecs; }
                set
                {
                    if (this.m_idamecs != value)
                    {
                        //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idamecs", m_idamecs, value));
                        m_idamecs = value;

                    }
                }
            }
           

            private Nullable<Int32> m_idunidad;

            //[PropiedadOriginal("idunidad", TablaOriginal = "unidorganizamecs", TipoProveedor = ClepsydraDbType.Int64, Longitud = 10)]
            public Nullable<Int32> idunidad
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

            private String m_unidad;

            //[PropiedadOriginal("unidad", TablaOriginal = "unidorganizamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
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

            private Nullable<Int32> m_idarea;

            //[PropiedadOriginal("idarea", TablaOriginal = "unidorganizamecs", TipoProveedor = ClepsydraDbType.Int64, Longitud = 10)]
            public Nullable<Int32> idarea
            {
                get { return m_idarea; }
                set
                {
                    if (this.m_idarea != value)
                    {
                        //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idarea", m_idarea, value));
                        m_idarea = value;

                    }
                }
            }

            private String m_area;

            //[PropiedadOriginal("area", TablaOriginal = "unidorganizamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
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

            private Nullable<Int32> m_idregion;

            //[PropiedadOriginal("idregion", TablaOriginal = "unidorganizamecs", TipoProveedor = ClepsydraDbType.Int64, Longitud = 10)]
            public Nullable<Int32> idregion
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

            private String m_region;

            //[PropiedadOriginal("region", TablaOriginal = "unidorganizamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
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


            private Nullable<Int32> m_iddistrito;

            //[PropiedadOriginal("iddistrito", TablaOriginal = "unidorganizamecs", TipoProveedor = ClepsydraDbType.Int64, Longitud = 11)]
            public Nullable<Int32> iddistrito
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

            private String m_distrito;

            //[PropiedadOriginal("distrito", TablaOriginal = "unidorganizamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
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

            private Int32 m_idcreadopor;

            //[PropiedadOriginal("idcreadopor", TablaOriginal = "unidorganizamecs", TipoProveedor = ClepsydraDbType.Int64, Longitud = 11)]
            public Int32 idcreadopor
            {
                get { return m_idcreadopor; }
                set
                {
                    if (this.m_idcreadopor != value)
                    {
                        //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idcreadopor", m_idcreadopor, value));
                        m_idcreadopor = value;

                    }
                }
            }           

            
            private Nullable<DateTime> m_fechacreacion;

            //[PropiedadOriginal("fechacreacion", EsNullable = true, TablaOriginal = "unidorganizamecs", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
            public Nullable<DateTime> fechacreacion
            {
                get { return m_fechacreacion; }
                set
                {
                    if (this.m_fechacreacion != value)
                    {
                        //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fechacreacion", m_fechacreacion, value));
                        m_fechacreacion = value;

                    }
                }
            }

            #endregion

            
            #region Constructores

            public DUnidadesOrganizativasAmec()
            {
            }
            public DUnidadesOrganizativasAmec(string _idamecs)
            {
                idamecs = _idamecs;
            }

            #endregion

            #region Converter

            public static ICollection<DUnidadesOrganizativasAmec> ConvertToDto(DataTable dtTable)
            {
                ICollection<DUnidadesOrganizativasAmec> collection = new Collection<DUnidadesOrganizativasAmec>();

                foreach (DataRow row in dtTable.Rows)
                {
                    DUnidadesOrganizativasAmec data = new DUnidadesOrganizativasAmec()
                    {
                        idunidadamec = long.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idunidadamec")),
                        idamecs = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idamecs"),
                        idunidad = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idunidad")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idunidad"))),
                        unidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "unidad"),
                        idarea = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idarea")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idarea"))),
                        area = Quodem.Utility.DataLayerUtil.GetStringValue(row, "area"),
                        idregion = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idregion")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idregion"))),
                        region = Quodem.Utility.DataLayerUtil.GetStringValue(row, "region"),
                        iddistrito = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "iddistrito")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "iddistrito"))),
                        distrito = Quodem.Utility.DataLayerUtil.GetStringValue(row, "distrito"),
                        idcreadopor = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idcreadopor")),
                        fechacreacion = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacreacion") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacreacion"),
                    };

                    collection.Add(data);
                }

                return collection;
            }

            #endregion
    }
}
