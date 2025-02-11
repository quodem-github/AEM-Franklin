using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DVFlujoAprobacion
    {
        #region Propiedades

        private UInt32 m_idflujoaprobacion;

        //[PropiedadOriginal("idflujoaprobacion", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public UInt32 idflujoaprobacion
        {
            get { return m_idflujoaprobacion; }
            set
            {
                if (this.m_idflujoaprobacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("iddelegaprobacion", m_idflujoaprobacion, value));
                    m_idflujoaprobacion = value;

                }
            }
        }


        private UInt32 m_idtipoflujo;

        //[PropiedadOriginal("idtipoflujo", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public UInt32 idtipoflujo
        {
            get { return m_idtipoflujo; }
            set
            {
                if (this.m_idtipoflujo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtipoflujo", m_idtipoflujo, value));
                    m_idtipoflujo = value;

                }
            }
        }

        private UInt32 m_idtipoactividad;

        //[PropiedadOriginal("idtipoactividad", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public UInt32 idtipoactividad
        {
            get { return m_idtipoactividad; }
            set
            {
                if (this.m_idtipoactividad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtipoactividad", m_idtipoactividad, value));
                    m_idtipoactividad = value;

                }
            }
        }

        private Int32 m_idcreadopor;

        //[PropiedadOriginal("idcreadopor", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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

        private String m_fase;

        //[PropiedadOriginal("fase", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        public String fase
        {
            get { return m_fase; }
            set
            {
                if (this.m_fase != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fase", m_fase, value));
                    m_fase = value;

                }
            }
        }

        private Nullable<Boolean> m_condicionada;

        //[PropiedadOriginal("condicionada", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.Byte, Longitud = 11)]
        public Nullable<Boolean> condicionada
        {
            get { return m_condicionada; }
            set
            {
                if (this.m_condicionada != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("condicionada", m_condicionada, value));
                    m_condicionada = value;

                }
            }
        }

        private Nullable<DateTime> m_fechacreacion;

        //[PropiedadOriginal("fechacreacion", EsNullable = true, TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
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

        private Int32 m_ordenfase;

        //[PropiedadOriginal("ordenfase", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.Int32, Longitud = 10)]
        public Int32 ordenfase
        {
            get { return m_ordenfase; }
            set
            {
                if (this.m_ordenfase != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ordenfase", m_ordenfase, value));
                    m_ordenfase = value;

                }
            }
        }


        private String m_preaprobado;

        //[PropiedadOriginal("preaprobado", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 10)]
        public String preaprobado
        {
            get { return m_preaprobado; }
            set
            {
                if (this.m_preaprobado != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("preaprobado", m_preaprobado, value));
                    m_preaprobado = value;

                }
            }
        }


        private UInt32 m_idnivelinicial;

        //[PropiedadOriginal("idnivelinicial", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 10)]
        public UInt32 idnivelinicial
        {
            get { return m_idnivelinicial; }
            set
            {
                if (this.m_idnivelinicial != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idnivelinicial", m_idnivelinicial, value));
                    m_idnivelinicial = value;

                }
            }
        }


        private UInt32 m_idnivelsiguiente;

        //[PropiedadOriginal("idnivelsiguiente", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public UInt32 idnivelsiguiente
        {
            get { return m_idnivelsiguiente; }
            set
            {
                if (this.m_idnivelsiguiente != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idnivelsiguiente", m_idnivelsiguiente, value));
                    m_idnivelsiguiente = value;

                }
            }
        }



        private Double m_importepreaprobacion;

        //[PropiedadOriginal("importepreaprobacion", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.Double, Longitud = 11)]
        public Double importepreaprobacion
        {
            get { return m_importepreaprobacion; }
            set
            {
                if (this.m_importepreaprobacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("importepreaprobacion", m_importepreaprobacion, value));
                    m_importepreaprobacion = value;

                }
            }
        }

        private UInt32 m_IdCargoNivelSiguiente;

        //[PropiedadOriginal("IdCargoNivelSiguiente", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public UInt32 IdCargoNivelSiguiente
        {
            get { return m_IdCargoNivelSiguiente; }
            set
            {
                if (this.m_IdCargoNivelSiguiente != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdCargoNivelSiguiente", m_idnivelsiguiente, value));
                    m_IdCargoNivelSiguiente = value;
                }
            }
        }

        private String m_NombreCompletoCreadoPor;

        //[PropiedadOriginal("NombreCompletoCreadoPor", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 10)]
        public String NombreCompletoCreadoPor
        {
            get { return m_NombreCompletoCreadoPor; }
            set
            {
                if (this.m_NombreCompletoCreadoPor != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("NombreCompletoCreadoPor", m_NombreCompletoCreadoPor, value));
                    m_NombreCompletoCreadoPor = value;

                }
            }
        }

        private String m_NivelInicial;

        //[PropiedadOriginal("NivelInicial", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 10)]
        public String NivelInicial
        {
            get { return m_NivelInicial; }
            set
            {
                if (this.m_NivelInicial != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("NivelInicial", m_NivelInicial, value));
                    m_NivelInicial = value;

                }
            }
        }

        private String m_NivelSiguiente;

        //[PropiedadOriginal("NivelSiguiente", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 10)]
        public String NivelSiguiente
        {
            get { return m_NivelSiguiente; }
            set
            {
                if (this.m_NivelSiguiente != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("NivelSiguiente", m_NivelSiguiente, value));
                    m_NivelSiguiente = value;

                }
            }
        }

        private String m_tipoflujo;

        //[PropiedadOriginal("tipoflujo", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 10)]
        public String tipoflujo
        {
            get { return m_tipoflujo; }
            set
            {
                if (this.m_tipoflujo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("tipoflujo", m_tipoflujo, value));
                    m_tipoflujo = value;

                }
            }
        }

        private String m_tipoactividad;

        //[PropiedadOriginal("tipoactividad", TablaOriginal = "cv_flujoaprobacion", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 10)]
        public String tipoactividad
        {
            get { return m_tipoactividad; }
            set
            {
                if (this.m_tipoactividad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("tipoactividad", m_tipoflujo, value));
                    m_tipoactividad = value;

                }
            }
        }
        #endregion

        #region constructor
        public DVFlujoAprobacion() { }
        public DVFlujoAprobacion(UInt32 idflujoaprobacion)
        { 
        }

        #endregion

        #region Converter

        public static ICollection<DVFlujoAprobacion> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVFlujoAprobacion> collection = new Collection<DVFlujoAprobacion>();
            foreach (DataRow row in dtTable.Rows)
            {
                DVFlujoAprobacion data = new DVFlujoAprobacion()
                {
                    idcreadopor = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcreadopor"),
                    ordenfase = Quodem.Utility.DataLayerUtil.GetIntValue(row, "ordenfase"),
                    condicionada = Quodem.Utility.DataLayerUtil.GetStringValue(row, "condicionada") == "1",
                    fechacreacion = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacreacion") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacreacion"),
                    fase = Quodem.Utility.DataLayerUtil.GetStringValue(row, "fase"),
                    NivelInicial = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NivelInicial"),
                    NivelSiguiente = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NivelSiguiente"),
                    NombreCompletoCreadoPor = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NombreCompletoCreadoPor"),
                    preaprobado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "preaprobado"),
                    tipoactividad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "tipoactividad"),
                    tipoflujo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "tipoflujo"),
                    IdCargoNivelSiguiente = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdCargoNivelSiguiente")),
                    idflujoaprobacion = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idflujoaprobacion")),
                    idnivelinicial = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idnivelinicial")),
                    idnivelsiguiente = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idnivelsiguiente")),
                    idtipoactividad = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idtipoactividad")),
                    idtipoflujo = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idtipoflujo")),
                };

                collection.Add(data);
            }
            return collection;
        }

        #endregion
    }
}
