using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DAmecInfo
    {
        #region Propiedades
        private string m_idamecs;

        //[PropiedadOriginal("idamecs", EsClave = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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

        //[PropiedadOriginal("idestado", EsClave = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
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


        private Nullable<Boolean> m_paraguas;

        //[PropiedadOriginal("paraguas", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> paraguas
        {
            get { return m_paraguas; }
            set
            {
                if (this.m_paraguas != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("paraguas", m_paraguas, value));
                    m_paraguas = value;

                }
            }
        }

        private Nullable<Int32> m_idsolicitante;

        //[PropiedadOriginal("idsolicitante", TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> idsolicitante
        {
            get { return m_idsolicitante; }
            set
            {
                if (this.m_idsolicitante != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idsolicitante", m_idsolicitante, value));
                    m_idsolicitante = value;

                }
            }
        }

        private Nullable<Int32> m_idcargo;

        //[PropiedadOriginal("idcargo", TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> idcargo
        {
            get { return m_idcargo; }
            set
            {
                if (this.m_idcargo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idcargo", m_idcargo, value));
                    m_idcargo = value;

                }
            }
        }

        private Nullable<Int32> m_idposition;

        //[PropiedadOriginal("idcargo", TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> idposition
        {
            get { return m_idposition; }
            set
            {
                if (this.m_idposition != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idcargo", m_idcargo, value));
                    m_idposition = value;

                }
            }
        }

        private Int32 m_idcreadopor;

        //[PropiedadOriginal("idcreadopor", TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 10)]
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

        private Nullable<DateTime> m_fechaamecs;

        //[PropiedadOriginal("fechaamecs", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public Nullable<DateTime> fechaamecs
        {
            get { return m_fechaamecs; }
            set
            {
                if (this.m_fechaamecs != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fechaamecs", m_fechaamecs, value));
                    m_fechaamecs = value;

                }
            }
        }

        private String m_nwein;

        //[PropiedadOriginal("nwein", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 11)]
        public String nwein
        {
            get { return m_nwein; }
            set
            {
                if (this.m_nwein != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("nwein", m_nwein, value));
                    m_nwein = value;

                }
            }
        }


        private Nullable<Int32> m_idtipoactividad;

        //[PropiedadOriginal("idtipoactividad", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> idtipoactividad
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


        private Nullable<Boolean> m_preaprobadaamed;

        //[PropiedadOriginal("preaprobadaamed", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> preaprobadaamed
        {
            get { return m_preaprobadaamed; }
            set
            {
                if (this.m_preaprobadaamed != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("preaprobadaamed", m_preaprobadaamed, value));
                    m_preaprobadaamed = value;

                }
            }
        }

        private Nullable<Boolean> m_preaprobadaneg;

        //[PropiedadOriginal("preaprobadaneg", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> preaprobadaneg
        {
            get { return m_preaprobadaneg; }
            set
            {
                if (this.m_preaprobadaneg != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("preaprobadaneg", m_preaprobadaneg, value));
                    m_preaprobadaneg = value;

                }
            }
        }

        private Nullable<Boolean> m_preaprobadaleg;

        //[PropiedadOriginal("preaprobadaleg", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> preaprobadaleg
        {
            get { return m_preaprobadaleg; }
            set
            {
                if (this.m_preaprobadaleg != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("preaprobadaleg", m_preaprobadaleg, value));
                    m_preaprobadaleg = value;

                }
            }
        }

        private Nullable<Boolean> m_farmaindustria;

        //[PropiedadOriginal("farmaindustria", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> farmaindustria
        {
            get { return m_farmaindustria; }
            set
            {
                if (this.m_farmaindustria != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("farmaindustria", m_farmaindustria, value));
                    m_farmaindustria = value;

                }
            }
        }

        private Nullable<Boolean> m_casosclinicos;

        //[PropiedadOriginal("casosclinicos", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> casosclinicos
        {
            get { return m_casosclinicos; }
            set
            {
                if (this.m_casosclinicos != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("casosclinicos", m_casosclinicos, value));
                    m_casosclinicos = value;

                }
            }
        }


        private Nullable<UInt32> m_participantesmsd;

        //[PropiedadOriginal("participantesmsd", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 10)]
        public Nullable<UInt32> participantesmsd
        {
            get { return m_participantesmsd; }
            set
            {
                if (this.m_participantesmsd != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("participantesmsd", m_participantesmsd, value));
                    m_participantesmsd = value;

                }
            }
        }



        private Int32 m_idcriterioseleccion;

        //[PropiedadOriginal("idcriterioseleccion", EsClave = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 idcriterioseleccion
        {
            get { return m_idcriterioseleccion; }
            set
            {
                if (this.m_idcriterioseleccion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idcriterioseleccion", m_idcriterioseleccion, value));
                    m_idcriterioseleccion = value;

                }
            }
        }


        private String m_detallecriterios;

        //[PropiedadOriginal("detallecriterios", TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String detallecriterios
        {
            get { return m_detallecriterios; }
            set
            {
                if (this.m_detallecriterios != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("detallecriterios", m_detallecriterios, value));
                    m_detallecriterios = value;

                }
            }
        }



        private String m_criterioespecificado;

        //[PropiedadOriginal("criterioespecificado", TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String criterioespecificado
        {
            get { return m_criterioespecificado; }
            set
            {
                if (this.m_criterioespecificado != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("criterioespecificado", m_criterioespecificado, value));
                    m_criterioespecificado = value;

                }
            }
        }



        private Nullable<Boolean> m_medicosfichero;

        //[PropiedadOriginal("medicosfichero", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> medicosfichero
        {
            get { return m_medicosfichero; }
            set
            {
                if (this.m_medicosfichero != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("medicosfichero", m_medicosfichero, value));
                    m_medicosfichero = value;

                }
            }
        }

        private String m_duracionhoras;

        //[PropiedadOriginal("duracionhoras", TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 10)]
        public String duracionhoras
        {
            get { return m_duracionhoras; }
            set
            {
                if (this.m_duracionhoras != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("duracionhoras", m_duracionhoras, value));
                    m_duracionhoras = value;

                }
            }
        }

        private Nullable<UInt32> m_ponentespatrocinados;

        //[PropiedadOriginal("ponentespatrocinados", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 10)]
        public Nullable<UInt32> ponentespatrocinados
        {
            get { return m_ponentespatrocinados; }
            set
            {
                if (this.m_ponentespatrocinados != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ponentespatrocinados", m_ponentespatrocinados, value));
                    m_ponentespatrocinados = value;

                }
            }
        }

        private String m_conceptogastos;

        //[PropiedadOriginal("conceptogastos", TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String conceptogastos
        {
            get { return m_conceptogastos; }
            set
            {
                if (this.m_conceptogastos != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("conceptogastos", m_conceptogastos, value));
                    m_conceptogastos = value;

                }
            }
        }

        private String m_cargoadaxas;

        //[PropiedadOriginal("cargoadaxas", TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 150)]
        public String cargoadaxas
        {
            get { return m_cargoadaxas; }
            set
            {
                if (this.m_cargoadaxas != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("cargoadaxas", m_cargoadaxas, value));
                    m_cargoadaxas = value;

                }
            }
        }

        private Nullable<Decimal> m_importegasto;

        //[PropiedadOriginal("importegasto", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Decimal, Longitud = 4)]
        public Nullable<Decimal> importegasto
        {
            get { return m_importegasto; }
            set
            {
                if (this.m_importegasto != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("importegasto", m_importegasto, value));
                    m_importegasto = value;

                }
            }
        }

        private Nullable<UInt32> m_cartascontrato;

        //[PropiedadOriginal("cartascontrato", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 10)]
        public Nullable<UInt32> cartascontrato
        {
            get { return m_cartascontrato; }
            set
            {
                if (this.m_cartascontrato != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("cartascontrato", m_cartascontrato, value));
                    m_cartascontrato = value;

                }
            }
        }

        private String m_programaamecs;

        //[PropiedadOriginal("programaamecs", TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 150)]
        public String programaamecs
        {
            get { return m_programaamecs; }
            set
            {
                if (this.m_programaamecs != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("programaamecs", m_programaamecs, value));
                    m_programaamecs = value;

                }
            }
        }

        private String m_urlprograma;

        //[PropiedadOriginal("urlprograma", TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 150)]
        public String urlprograma
        {
            get { return m_urlprograma; }
            set
            {
                if (this.m_urlprograma != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("urlprograma", m_urlprograma, value));
                    m_urlprograma = value;

                }
            }
        }

        private String m_descripcionobjetivo;

        //[PropiedadOriginal("descripcionobjetivo", TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 150)]
        public String descripcionobjetivo
        {
            get { return m_descripcionobjetivo; }
            set
            {
                if (this.m_descripcionobjetivo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("descripcionobjetivo", m_descripcionobjetivo, value));
                    m_descripcionobjetivo = value;

                }
            }
        }


        private String m_descripcion;

        //[PropiedadOriginal("descripcion", TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String descripcion
        {
            get { return m_descripcion; }
            set
            {
                if (this.m_descripcion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("descripcion", m_descripcion, value));
                    m_descripcion = value;

                }
            }
        }


        private Nullable<Boolean> m_politicaN20;

        //[PropiedadOriginal("politicaN20", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> politicaN20
        {
            get { return m_politicaN20; }
            set
            {
                if (this.m_politicaN20 != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("politicaN20", m_politicaN20, value));
                    m_politicaN20 = value;

                }
            }
        }



        private Nullable<DateTime> m_fechacomienzo;

        //[PropiedadOriginal("fechacomienzo", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public Nullable<DateTime> fechacomienzo
        {
            get { return m_fechacomienzo; }
            set
            {
                if (this.m_fechacomienzo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fechacomienzo", m_fechacomienzo, value));
                    m_fechacomienzo = value;

                }
            }
        }

        private Nullable<DateTime> m_fechafinalizacion;

        //[PropiedadOriginal("fechafinalizacion", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public Nullable<DateTime> fechafinalizacion
        {
            get { return m_fechafinalizacion; }
            set
            {
                if (this.m_fechafinalizacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fechafinalizacion", m_fechafinalizacion, value));
                    m_fechafinalizacion = value;

                }
            }
        }

        private String m_lugarsede;

        //[PropiedadOriginal("lugarsede", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 11)]
        public String lugarsede
        {
            get { return m_lugarsede; }
            set
            {
                if (this.m_lugarsede != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("lugarsede", m_lugarsede, value));
                    m_lugarsede = value;

                }
            }
        }

        private Nullable<UInt32> m_profesionalessanitarios;

        //[PropiedadOriginal("profesionalessanitarios", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 10)]
        public Nullable<UInt32> profesionalessanitarios
        {
            get { return m_profesionalessanitarios; }
            set
            {
                if (this.m_profesionalessanitarios != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("profesionalessanitarios", m_participantesmsd, value));
                    m_profesionalessanitarios = value;

                }
            }
        }

        private int m_idnivelaprobacion;

        public int idnivelaprobacion
        {
            get { return m_idnivelaprobacion; }
            set
            {
                if (this.m_idnivelaprobacion != value)
                {
                    m_idnivelaprobacion = value;
                }
            }
        }

        private int m_idconfempresa;

        public int idconfempresa
        {
            get { return m_idconfempresa; }
            set
            {
                if (this.m_idconfempresa != value)
                {
                    m_idconfempresa = value;
                }
            }
        }

        private string m_idamecsrelacionado;

        //[PropiedadOriginal("idamecs", EsClave = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public string idamecsrelacionado
        {
            get { return m_idamecsrelacionado; }
            set
            {
                if (this.m_idamecsrelacionado != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idamecs", m_idamecs, value));
                    m_idamecsrelacionado = value;

                }
            }
        }

        private Boolean m_veeva;

        //[PropiedadOriginal("farmaindustria", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Boolean veeva
        {
            get { return m_veeva; }
            set
            {
                if (this.m_veeva != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("farmaindustria", m_farmaindustria, value));
                    m_veeva = value;
                }
            }
        }

        private Boolean m_newco;

        //[PropiedadOriginal("farmaindustria", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Boolean newco
        {
            get { return m_newco; }
            set
            {
                if (this.m_newco != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("farmaindustria", m_farmaindustria, value));
                    m_newco = value;
                }
            }
        }

        #endregion

        #region Constructores

        public DAmecInfo()
        {
        }
        public DAmecInfo(string _idamec)
        {
            idamecs = _idamec;
        }

        #region Converter

        public static ICollection<DAmecInfo> ConvertToDto(DataTable dtTable)
        {
            ICollection<DAmecInfo> collection = new Collection<DAmecInfo>();
            foreach (DataRow row in dtTable.Rows)
            {
                DateTime fechacomienzo = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacomienzo");
                DateTime fechafinalizacion = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechafinalizacion");

                DAmecInfo data = new DAmecInfo()
                {
                    idamecs = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idamecs"),
                    idestado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idestado")!=""?UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idestado")):0,
                    idsolicitante = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idsolicitante"),
                    idcargo = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcargo") < 0 ? new int?() : Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcargo"),
                    idposition = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idposition") < 0 ? new int?() : Quodem.Utility.DataLayerUtil.GetIntValue(row, "idposition"),
                    idnivelaprobacion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idnivelaprobacion")!=""?Int32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idnivelaprobacion")):new int(),
                    idcreadopor = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcreadopor"),
                    fechaamecs = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaamecs") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaamecs"),
                    nwein = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nwein"),
                    idtipoactividad = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idtipoactividad"),
                    preaprobadaamed = Quodem.Utility.DataLayerUtil.GetStringValue(row, "preaprobadaamed") == "1",
                    preaprobadaneg = Quodem.Utility.DataLayerUtil.GetStringValue(row, "preaprobadaneg") == "1",
                    preaprobadaleg = Quodem.Utility.DataLayerUtil.GetStringValue(row, "preaprobadaleg") == "1",
                    farmaindustria = Quodem.Utility.DataLayerUtil.GetStringValue(row, "farmaindustria") == "1",
                    casosclinicos = Quodem.Utility.DataLayerUtil.GetStringValue(row, "casosclinicos") == "1",
                    politicaN20 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "politicaN20") == "1",
                    participantesmsd = Quodem.Utility.DataLayerUtil.GetStringValue(row, "participantesmsd") != "" ? UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "participantesmsd")) : 0,
                    detallecriterios = Quodem.Utility.DataLayerUtil.GetStringValue(row, "detallecriterios"),
                    medicosfichero = Quodem.Utility.DataLayerUtil.GetStringValue(row, "medicosfichero") == "1",
                    duracionhoras = Quodem.Utility.DataLayerUtil.GetStringValue(row, "duracionhoras"),
                    ponentespatrocinados = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ponentespatrocinados") != "" ? UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "ponentespatrocinados")) : 0,
                    conceptogastos = Quodem.Utility.DataLayerUtil.GetStringValue(row, "conceptogastos"),
                    cargoadaxas = Quodem.Utility.DataLayerUtil.GetStringValue(row, "cargoadaxas"),
                    importegasto =string.IsNullOrWhiteSpace(Quodem.Utility.DataLayerUtil.GetStringValue(row, "importegasto")) ? new Nullable<Decimal>() : Decimal.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "importegasto")),
                    cartascontrato = Quodem.Utility.DataLayerUtil.GetStringValue(row, "cartascontrato") != "" ? UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "cartascontrato")) : 0,
                    fechacomienzo = fechacomienzo == DateTime.MaxValue || fechacomienzo == DateTime.MinValue ? new DateTime?() : fechacomienzo,
                    fechafinalizacion = fechafinalizacion == DateTime.MaxValue || fechafinalizacion == DateTime.MinValue ? new DateTime?() : fechafinalizacion,
                    lugarsede = Quodem.Utility.DataLayerUtil.GetStringValue(row, "lugarsede"),
                    idcriterioseleccion = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcriterioseleccion"),
                    criterioespecificado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "criterioespecificado"),
                    programaamecs = Quodem.Utility.DataLayerUtil.GetStringValue(row, "programaamecs"),
                    urlprograma = Quodem.Utility.DataLayerUtil.GetStringValue(row, "urlprograma"),
                    descripcionobjetivo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "descripcionobjetivo"),
                    descripcion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "descripcion"),
                    paraguas = Quodem.Utility.DataLayerUtil.GetStringValue(row, "paraguas") == "1",
                    profesionalessanitarios = Quodem.Utility.DataLayerUtil.GetStringValue(row, "profesionalessanitarios") != "" ? UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "profesionalessanitarios")) : 0,
                    idconfempresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idconfempresa"),
                    idamecsrelacionado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idamecsrelacionado"),
                    veeva = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "veeva", false),
                    newco = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "newco", false)
                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion

        #endregion
    }

}
