using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DVDelegacion
    {
        #region Propiedades

        private Int64 m_iddelegaprobacion;

        //[PropiedadOriginal("iddelegaprobacion", TablaOriginal = "cv_delegaprobacion", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public Int64 iddelegaprobacion
        {
            get { return m_iddelegaprobacion; }
            set
            {
                if (this.m_iddelegaprobacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("iddelegaprobacion", m_iddelegaprobacion, value));
                    m_iddelegaprobacion = value;

                }
            }
        }


        private Int64 m_iddelegado;

        //[PropiedadOriginal("iddelegado", TablaOriginal = "cv_delegaprobacion", TipoProveedor = ClepsydraDbType.Int64, Longitud = 11)]
        public Int64 iddelegado
        {
            get { return m_iddelegado; }
            set
            {
                if (this.m_iddelegado != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("iddelegado", m_iddelegado, value));
                    m_iddelegado = value;

                }
            }
        }



        private Int64 m_idusuariodel;

        //[PropiedadOriginal("idusuariodel", TablaOriginal = "cv_delegaprobacion", TipoProveedor = ClepsydraDbType.Int64, Longitud = 11)]
        public Int64 idusuariodel
        {
            get { return m_idusuariodel; }
            set
            {
                if (this.m_idusuariodel != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idusuario", m_idusuariodel, value));
                    m_idusuariodel = value;

                }
            }
        }


        private Int64 m_idcreadopor;

        //[PropiedadOriginal("idcreadopor", TablaOriginal = "cv_delegaprobacion", TipoProveedor = ClepsydraDbType.Int64, Longitud = 11)]
        public Int64 idcreadopor
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


        private Nullable<DateTime> m_fechadesde;

        //[PropiedadOriginal("fechadesde", EsNullable = true, TablaOriginal = "cv_delegaprobacion", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public Nullable<DateTime> fechadesde
        {
            get { return m_fechadesde; }
            set
            {
                if (this.m_fechadesde != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fechadesde", m_fechadesde, value));
                    m_fechadesde = value;

                }
            }
        }



        private Nullable<DateTime> m_fechahasta;

        //[PropiedadOriginal("fechahasta", EsNullable = true, TablaOriginal = "cv_delegaprobacion", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public Nullable<DateTime> fechahasta
        {
            get { return m_fechahasta; }
            set
            {
                if (this.m_fechahasta != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fechahasta", m_fechahasta, value));
                    m_fechahasta = value;

                }
            }
        }

        private Nullable<DateTime> m_fechacreacion;

        //[PropiedadOriginal("fechacreacion", EsNullable = true, TablaOriginal = "cv_delegaprobacion", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
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


        //private String m_estado;

        ////[PropiedadOriginal("estado", TablaOriginal = "cv_delegaprobacion", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        //public String estado
        //{
        //    get { return m_estado; }
        //    set
        //    {
        //        if (this.m_estado != value)
        //        {
        //            //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("estado", m_estado, value));
        //            m_estado = value;

        //        }
        //    }
        //}

        private String m_NombreCompletoUsuario;

        //[PropiedadOriginal("NombreCompletoUsuario", TablaOriginal = "cv_delegaprobacion", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        public String NombreCompletoUsuario
        {
            get { return m_NombreCompletoUsuario; }
            set
            {
                if (this.m_NombreCompletoUsuario != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("NombreCompletoUsuario", m_NombreCompletoUsuario, value));
                    m_NombreCompletoUsuario = value;

                }
            }
        }


        private String m_NombreCompletoDelegado;

        //[PropiedadOriginal("NombreCompletoDelegado", TablaOriginal = "cv_delegaprobacion", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        public String NombreCompletoDelegado
        {
            get { return m_NombreCompletoDelegado; }
            set
            {
                if (this.m_NombreCompletoDelegado != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("NombreCompletoDelegado", m_NombreCompletoDelegado, value));
                    m_NombreCompletoDelegado = value;

                }
            }
        }


        private String m_NombreCompletoCreadoPor;

        //[PropiedadOriginal("NombreCompletoCreadoPor", TablaOriginal = "cv_delegaprobacion", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
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

        private Int64 m_IdEstado;

        //[PropiedadOriginal("idusuariodel", TablaOriginal = "cv_delegaprobacion", TipoProveedor = ClepsydraDbType.Int64, Longitud = 11)]
        public Int64 IdEstado
        {
            get { return m_IdEstado; }
            set
            {
                if (this.m_IdEstado != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idusuario", m_idusuariodel, value));
                    m_IdEstado = value;

                }
            }
        }

        public virtual long __hibernate_sort_row
        {
            set;
            get;
        }

        #endregion

        #region constructor
        public DVDelegacion() { }
        public DVDelegacion(Int64 iddelegaprobacion)
        {
        }

        #endregion

        #region Converter

        public static ICollection<DVDelegacion> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVDelegacion> collection = new Collection<DVDelegacion>();
            foreach (DataRow row in dtTable.Rows)
            {
                DVDelegacion data = new DVDelegacion()
                {
                    idcreadopor = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcreadopor"),
                    iddelegado = Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddelegado"),
                    idusuariodel = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idusuariodel"),
                    fechacreacion = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacreacion") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacreacion"),
                    fechadesde = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechadesde") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechadesde"),
                    fechahasta = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechahasta") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechahasta"),
                    //estado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "estado"),
                    NombreCompletoCreadoPor = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NombreCompletoCreadoPor"),
                    NombreCompletoDelegado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NombreCompletoDelegado"),
                    NombreCompletoUsuario = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NombreCompletoUsuario"),
                    iddelegaprobacion = Int64.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "iddelegaprobacion")),
                    IdEstado = Int64.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEstado")),

                };

                collection.Add(data);
            }
            return collection;
        }

        #endregion
    }
}
