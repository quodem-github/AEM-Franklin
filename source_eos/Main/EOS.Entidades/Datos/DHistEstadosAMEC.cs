using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DHistEstadosAMEC
    {
        #region Propiedades
        private Int64 m_idestadoamechist;

        //[PropiedadOriginal("idestadoamechist", EsClave = true, TablaOriginal = "histasocamecs", TipoProveedor = ClepsydraDbType.Int64, Longitud = 11)]
        public Int64 idestadoamechist
        {
            get { return m_idestadoamechist; }
            set
            {
                if (this.m_idestadoamechist != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idestadoamechist", m_idestadoamechist, value));
                    m_idestadoamechist = value;

                }
            }
        }


        private string m_idamecs;

        //[PropiedadOriginal("idamecs", EsClave = true, TablaOriginal = "histasocamecs", TipoProveedor = ClepsydraDbType.Int64, Longitud = 11)]
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


        private UInt32 m_idestado;

        //[PropiedadOriginal("idestado", TablaOriginal = "histasocamecs", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 10)]
        public UInt32 idestado
        {
            get { return m_idestado; }
            set
            {
                if (this.m_idestado != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idestado", m_idestado, value));
                    m_idestado = value;

                }
            }
        }

        private Nullable<Int32> m_idnivelaprobacion;

        //[PropiedadOriginal("idnivelaprobacion", TablaOriginal = "histasocamecs", TipoProveedor = ClepsydraDbType.Int64, Longitud = 10)]
        public Nullable<Int32> idnivelaprobacion
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

        private String m_comentariosaprob;

        //[PropiedadOriginal("comentariosaprob", TablaOriginal = "histasocamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 50)]
        public String comentariosaprob
        {
            get { return m_comentariosaprob; }
            set
            {
                if (this.m_comentariosaprob != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("comentariosaprob", m_comentariosaprob, value));
                    m_comentariosaprob = value;
                }
            }
        }

        private String m_estado;

        //[PropiedadOriginal("estado", TablaOriginal = "histasocamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String estado
        {
            get { return m_estado; }
            set
            {
                if (this.m_estado != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("estado", m_estado, value));
                    m_estado = value;
                }
            }
        }

        private String m_nivelaprobacion;

        //[PropiedadOriginal("nivelaprobacion", TablaOriginal = "histasocamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
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



        private String m_cargo;

        //[PropiedadOriginal("cargo", TablaOriginal = "histasocamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String cargo
        {
            get { return m_cargo; }
            set
            {
                if (this.m_cargo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("cargo", m_cargo, value));
                    m_cargo = value;
                }
            }
        }


        private String m_nombreusuario;

        //[PropiedadOriginal("nombreusuario", TablaOriginal = "histasocamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String nombreusuario
        {
            get { return m_nombreusuario; }
            set
            {
                if (this.m_nombreusuario != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("nombreusuario", m_nombreusuario, value));
                    m_nombreusuario = value;
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

        private String m_nombreusuariopendiente;

        //[PropiedadOriginal("nombreusuario", TablaOriginal = "histasocamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String nombreusuariopendiente
        {
            get { return m_nombreusuariopendiente; }
            set
            {
                if (this.m_nombreusuariopendiente != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("nombreusuario", m_nombreusuario, value));
                    m_nombreusuariopendiente = value;
                }
            }
        }
        private String m_ultimaaccion;

        //[PropiedadOriginal("nombreusuario", TablaOriginal = "histasocamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String ultimaaccion
        {
            get { return m_ultimaaccion; }
            set
            {
                if (this.m_ultimaaccion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("nombreusuario", m_nombreusuario, value));
                    m_ultimaaccion = value;
                }
            }
        }

        #endregion


        #region Constructores

        public DHistEstadosAMEC()
        {
        }
        public DHistEstadosAMEC(string _idamecs)
        {
            idamecs = _idamecs;
        }

        #endregion

        #region Converter

        public static ICollection<DHistEstadosAMEC> ConvertToDto(DataTable dtTable)
        {
            ICollection<DHistEstadosAMEC> collection = new Collection<DHistEstadosAMEC>();
            foreach (DataRow row in dtTable.Rows)
            {
                DHistEstadosAMEC data = new DHistEstadosAMEC()
                {
                    idcreadopor = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcreadopor"),
                    idamecs = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idamecs"),
                    idestadoamechist = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idestadoamechist"),
                    fechacreacion = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacreacion") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacreacion"),
                    idnivelaprobacion = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idnivelaprobacion")),
                    cargo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "cargo"),
                    comentariosaprob = Quodem.Utility.DataLayerUtil.GetStringValue(row, "comentariosaprob"),
                    estado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "estado"),
                    nivelaprobacion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nivelaprobacion"),
                    nombreusuario = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nombreusuario"),
                    idestado = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idestado")),
                    nombreusuariopendiente = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nombreusuariopendiente"),
                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion

    }
}
