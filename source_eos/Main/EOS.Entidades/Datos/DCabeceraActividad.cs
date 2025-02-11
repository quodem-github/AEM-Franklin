using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DCabeceraActividad
    {
        #region Propiedades
        private Int32 m_IdCongreso;

        //[PropiedadOriginal("IdCongreso", TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IdCongreso
        {
            get { return m_IdCongreso; }
            set
            {
                if (this.m_IdCongreso != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdCongreso", m_IdCongreso, value));
                    m_IdCongreso = value;

                }
            }
        }
        private String m_Congreso;

        //[PropiedadOriginal("Congreso", TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 70)]
        public String Congreso
        {
            get { return m_Congreso; }
            set
            {
                if (this.m_Congreso != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Congreso", m_Congreso, value));
                    m_Congreso = value;

                }
            }
        }
        private Nullable<Int32> m_IdTipoCongreso;

        //[PropiedadOriginal("IdTipoCongreso", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> IdTipoCongreso
        {
            get { return m_IdTipoCongreso; }
            set
            {
                if (this.m_IdTipoCongreso != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdTipoCongreso", m_IdTipoCongreso, value));
                    m_IdTipoCongreso = value;

                }
            }
        }
        private String m_TipoCongreso;

        //[PropiedadOriginal("TipoCongreso", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 50)]
        public String TipoCongreso
        {
            get { return m_TipoCongreso; }
            set
            {
                if (this.m_TipoCongreso != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("TipoCongreso", m_TipoCongreso, value));
                    m_TipoCongreso = value;

                }
            }
        }
        private Int32 m_IdPoblacion;

        //[PropiedadOriginal("IdPoblacion", TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IdPoblacion
        {
            get { return m_IdPoblacion; }
            set
            {
                if (this.m_IdPoblacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdPoblacion", m_IdPoblacion, value));
                    m_IdPoblacion = value;

                }
            }
        }
        private String m_Poblacion;

        //[PropiedadOriginal("Poblacion", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 30)]
        public String Poblacion
        {
            get { return m_Poblacion; }
            set
            {
                if (this.m_Poblacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Poblacion", m_Poblacion, value));
                    m_Poblacion = value;

                }
            }
        }
        private String m_IdProvincia;

        //[PropiedadOriginal("IdProvincia", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 3)]
        public String IdProvincia
        {
            get { return m_IdProvincia; }
            set
            {
                if (this.m_IdProvincia != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdProvincia", m_IdProvincia, value));
                    m_IdProvincia = value;

                }
            }
        }
        private String m_Provincia;

        //[PropiedadOriginal("Provincia", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        public String Provincia
        {
            get { return m_Provincia; }
            set
            {
                if (this.m_Provincia != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Provincia", m_Provincia, value));
                    m_Provincia = value;

                }
            }
        }
        private DateTime m_Desde;

        //[PropiedadOriginal("Desde", TablaOriginal = "", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public DateTime Desde
        {
            get { return m_Desde; }
            set
            {
                if (this.m_Desde != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Desde", m_Desde, value));
                    m_Desde = value;

                }
            }
        }
        private DateTime m_Hasta;

        //[PropiedadOriginal("Hasta", TablaOriginal = "", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public DateTime Hasta
        {
            get { return m_Hasta; }
            set
            {
                if (this.m_Hasta != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Hasta", m_Hasta, value));
                    m_Hasta = value;

                }
            }
        }
        /*private String m_AMEC;

        //[PropiedadOriginal("AMEC"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
        public String AMEC
        {
            get { return m_AMEC; }
            set {
                if (this.m_AMEC != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("AMEC", m_AMEC, value));
                    m_AMEC = value;
					
                }
            }
        }*/
        private Nullable<Int16> m_Comunicar;

        //[PropiedadOriginal("Comunicar", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Byte, Longitud = 4)]
        public Nullable<Int16> Comunicar
        {
            get { return m_Comunicar; }
            set
            {
                if (this.m_Comunicar != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Comunicar", m_Comunicar, value));
                    m_Comunicar = value;

                }
            }
        }
        private Nullable<Int16> m_Internacional;

        //[PropiedadOriginal("Internacional", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Byte, Longitud = 4)]
        public Nullable<Int16> Internacional
        {
            get { return m_Internacional; }
            set
            {
                if (this.m_Internacional != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Internacional", m_Internacional, value));
                    m_Internacional = value;

                }
            }
        }
        private Nullable<Int64> m_IdValoracionfi;

        //[PropiedadOriginal("IdValoracionfi", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int64> IdValoracionfi
        {
            get { return m_IdValoracionfi; }
            set
            {
                if (this.m_IdValoracionfi != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdValoracionfi", m_IdValoracionfi, value));
                    m_IdValoracionfi = value;

                }
            }
        }
        
        private Nullable<Int64> m_IdEventoFormulario;

        //[PropiedadOriginal("IdEventoFormulario", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public Nullable<Int64> IdEventoFormulario
        {
            get { return m_IdEventoFormulario; }
            set
            {
                if (this.m_IdEventoFormulario != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEventoFormulario", m_IdEventoFormulario, value));
                    m_IdEventoFormulario = value;

                }
            }
        }

        private Nullable<Int32> m_idconfempresa;

        //[PropiedadOriginal("IdEventoFormulario", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public Nullable<Int32> idconfempresa
        {
            get { return m_idconfempresa; }
            set
            {
                if (this.m_idconfempresa != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEventoFormulario", m_IdEventoFormulario, value));
                    m_idconfempresa = value;

                }
            }
        }

        private Int64 m_EsCongreso;

        //[PropiedadOriginal("EsCongreso", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Byte, Longitud = 4)]
        public Int64 EsCongreso
        {
            get { return m_EsCongreso; }
            set
            {
                if (this.m_EsCongreso != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("EsCongreso", m_EsCongreso, value));
                    m_EsCongreso = value;

                }
            }
        }

        private String m_LinkGestorInvitados;

        //[PropiedadOriginal("Poblacion", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 30)]
        public String LinkGestorInvitados
        {
            get { return m_LinkGestorInvitados; }
            set
            {
                if (this.m_LinkGestorInvitados != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Poblacion", m_Poblacion, value));
                    m_LinkGestorInvitados = value;

                }
            }
        }

        public virtual long __hibernate_sort_row
        {
            set;
            get;
        }

        #endregion

        #region Constructores

        public DCabeceraActividad()
        {
        }


        #endregion


        #region Converter

        public static ICollection<DCabeceraActividad> ConvertToDto(DataTable dtTable)
        {
            ICollection<DCabeceraActividad> collection = new Collection<DCabeceraActividad>();

            foreach(DataRow row in dtTable.Rows)
            {
                DCabeceraActividad data = new DCabeceraActividad()
                {
                    IdCongreso = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdCongreso")),
                    Congreso = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Congreso"),
                    IdTipoCongreso = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdTipoCongreso")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdTipoCongreso"))),
                    TipoCongreso = Quodem.Utility.DataLayerUtil.GetStringValue(row, "TipoCongreso"),
                    IdPoblacion = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdPoblacion")),
                    Poblacion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Poblacion"),
                    IdProvincia = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdProvincia"),
                    Provincia = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Provincia"),
                    Desde = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "Desde"),
                    Hasta = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "Hasta"),
                    Comunicar = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Comunicar")) ? new Nullable<sbyte>() : sbyte.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Comunicar"))),
                    Internacional = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Internacional")) ? new Nullable<sbyte>() : sbyte.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Internacional"))),
                    IdValoracionfi = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdValoracionfi")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdValoracionfi"))),
                    IdEventoFormulario = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEventoFormulario")) ? new Nullable<uint>() : uint.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEventoFormulario"))),
                    EsCongreso = long.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "EsCongreso")),
                    LinkGestorInvitados = Quodem.Utility.DataLayerUtil.GetStringValue(row, "LinkGestorInvitados"),
                    idconfempresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdConfEmpresa")
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
    }
}
