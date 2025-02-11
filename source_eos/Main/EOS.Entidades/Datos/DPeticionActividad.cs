using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DPeticionActividad
    {
        #region Propiedades
        private Int32 m_idpeticionactividad;

        //[PropiedadOriginal("idpeticionactividad", EsClave = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 idpeticionactividad
        {
            get { return m_idpeticionactividad; }
            set
            {
                if (this.m_idpeticionactividad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idpeticionactividad", m_idpeticionactividad, value));
                    m_idpeticionactividad = value;

                }
            }
        }
        private String m_nombre;

        //[PropiedadOriginal("nombre", TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 70)]
        public String nombre
        {
            get { return m_nombre; }
            set
            {
                if (this.m_nombre != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("nombre", m_nombre, value));
                    m_nombre = value;

                }
            }
        }
        private DateTime m_desde;

        //[PropiedadOriginal("desde", TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public DateTime desde
        {
            get { return m_desde; }
            set
            {
                if (this.m_desde != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("desde", m_desde, value));
                    m_desde = value;

                }
            }
        }
        private DateTime m_hasta;

        //[PropiedadOriginal("hasta", TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public DateTime hasta
        {
            get { return m_hasta; }
            set
            {
                if (this.m_hasta != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("hasta", m_hasta, value));
                    m_hasta = value;

                }
            }
        }
        private Int32 m_IDPoblacion;

        //[PropiedadOriginal("IDPoblacion", TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IDPoblacion
        {
            get { return m_IDPoblacion; }
            set
            {
                if (this.m_IDPoblacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IDPoblacion", m_IDPoblacion, value));
                    m_IDPoblacion = value;

                }
            }
        }
        private Nullable<Int32> m_IdCongreso;

        //[PropiedadOriginal("IdCongreso", EsNullable = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> IdCongreso
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
        private Nullable<Int32> m_idespecialidad;

        //[PropiedadOriginal("idespecialidad", EsNullable = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> idespecialidad
        {
            get { return m_idespecialidad; }
            set
            {
                if (this.m_idespecialidad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idespecialidad", m_idespecialidad, value));
                    m_idespecialidad = value;

                }
            }
        }
        private String m_sede;

        //[PropiedadOriginal("sede", EsNullable = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public String sede
        {
            get { return m_sede; }
            set
            {
                if (this.m_sede != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("sede", m_sede, value));
                    m_sede = value;

                }
            }
        }
        private Nullable<Int32> m_IdTipoCongreso;

        //[PropiedadOriginal("IdTipoCongreso", EsNullable = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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
        private Nullable<Int32> m_locked;

        //[PropiedadOriginal("locked", EsNullable = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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
        private Nullable<Boolean> m_internacional;

        //[PropiedadOriginal("internacional", EsNullable = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> internacional
        {
            get { return m_internacional; }
            set
            {
                if (this.m_internacional != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("internacional", m_internacional, value));
                    m_internacional = value;

                }
            }
        }
        private String m_url_web;

        //[PropiedadOriginal("url_web", EsNullable = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public String url_web
        {
            get { return m_url_web; }
            set
            {
                if (this.m_url_web != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("url_web", m_url_web, value));
                    m_url_web = value;

                }
            }
        }
        private String m_email_secretaria;

        //[PropiedadOriginal("email_secretaria", EsNullable = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public String email_secretaria
        {
            get { return m_email_secretaria; }
            set
            {
                if (this.m_email_secretaria != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("email_secretaria", m_email_secretaria, value));
                    m_email_secretaria = value;

                }
            }
        }
        private String m_especialidad;

        //[PropiedadOriginal("especialidad", EsNullable = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public String especialidad
        {
            get { return m_especialidad; }
            set
            {
                if (this.m_especialidad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("especialidad", m_especialidad, value));
                    m_especialidad = value;

                }
            }
        }
        private Nullable<DateTime> m_fechacreacion;

        //[PropiedadOriginal("fechacreacion", EsNullable = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
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
        private Nullable<Int32> m_idpeticionario;

        //[PropiedadOriginal("idpeticionario", EsNullable = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> idpeticionario
        {
            get { return m_idpeticionario; }
            set
            {
                if (this.m_idpeticionario != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idpeticionario", m_idpeticionario, value));
                    m_idpeticionario = value;

                }
            }
        }
        private Nullable<Boolean> m_comunicar;

        //[PropiedadOriginal("comunicar", EsNullable = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> comunicar
        {
            get { return m_comunicar; }
            set
            {
                if (this.m_comunicar != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("comunicar", m_comunicar, value));
                    m_comunicar = value;

                }
            }
        }
        private String m_valoracion_farmaindustria;

        //[PropiedadOriginal("valoracion_farmaindustria", EsNullable = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public String valoracion_farmaindustria
        {
            get { return m_valoracion_farmaindustria; }
            set
            {
                if (this.m_valoracion_farmaindustria != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("valoracion_farmaindustria", m_valoracion_farmaindustria, value));
                    m_valoracion_farmaindustria = value;

                }
            }
        }

        private Nullable<Int32> m_idconfempresa;

        //[PropiedadOriginal("idpeticionario", EsNullable = true, TablaOriginal = "peticiones_actividad", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> idconfempresa
        {
            get { return m_idconfempresa; }
            set
            {
                if (this.m_idconfempresa != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idpeticionario", m_idpeticionario, value));
                    m_idconfempresa = value;

                }
            }
        }

        #endregion

        #region Constructores

        public DPeticionActividad()
        {
        }
        public DPeticionActividad(Int32 _idpeticionactividad)
        {
            idpeticionactividad = _idpeticionactividad;
        }

        #endregion
    }
}
