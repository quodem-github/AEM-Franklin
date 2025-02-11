using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class ListadoAmecs
    {               
            #region Propiedades
            private string m_idamecs;

            //[PropiedadOriginal("idamecs", EsClave = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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

            private Int64 m_idestado;
            
            //[PropiedadOriginal("idestado", EsClave = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
            public Int64 idestado
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

            private String m_estado;

            //[PropiedadOriginal("estado", TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
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
            

            private Nullable<Int32> m_idsolicitante;

            //[PropiedadOriginal("idsolicitante", TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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

            private Int32 m_idcargo;

            //[PropiedadOriginal("idcargo", TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
            public Int32 idcargo
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

            private Int32 m_idcreadopor;

            //[PropiedadOriginal("idcreadopor", TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Int32, Longitud = 10)]
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

            //[PropiedadOriginal("fechaamecs", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
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

            //[PropiedadOriginal("nwein", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 11)]
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

            //[PropiedadOriginal("idtipoactividad", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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
           
            private Nullable<Boolean> m_preaprobadaamed;

            //[PropiedadOriginal("preaprobadaamed", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
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

            //[PropiedadOriginal("preaprobadaneg", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
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

            //[PropiedadOriginal("preaprobadaleg", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
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

            //[PropiedadOriginal("farmaindustria", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
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

            //[PropiedadOriginal("casosclinicos", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
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
            
            
            private Nullable<Int64> m_participantesmsd;

            //[PropiedadOriginal("participantesmsd", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 10)]
            public Nullable<Int64> participantesmsd
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
            

             private String m_detallecriterios;

            //[PropiedadOriginal("detallecriterios", TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
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
         

            private Nullable<Boolean> m_medicosfichero;

            //[PropiedadOriginal("medicosfichero", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
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

            //[PropiedadOriginal("duracionhoras", TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Varchar2, Longitud = 10)]
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

            private Nullable<Int64> m_ponentespatrocinados;

            //[PropiedadOriginal("ponentespatrocinados", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 10)]
            public Nullable<Int64> ponentespatrocinados
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

            //[PropiedadOriginal("conceptogastos", TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
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

            private Nullable<Decimal>m_importegasto;

            //[PropiedadOriginal("importegasto", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Decimal, Longitud = 4)]
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
            
            private Nullable<Int64> m_cartascontrato;

            //[PropiedadOriginal("cartascontrato", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 10)]
            public Nullable<Int64> cartascontrato
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

            //[PropiedadOriginal("programaamecs", TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 150)]
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

            //[PropiedadOriginal("urlprograma", TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 150)]
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

            //[PropiedadOriginal("descripcionobjetivo", TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 150)]
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

            //[PropiedadOriginal("descripcion", TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
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

        private Nullable<Boolean> _nuevosFlujosAprobacion;

        //[PropiedadOriginal("medicosfichero", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> nuevosFlujosAprobacion
        {
            get { return _nuevosFlujosAprobacion; }
            set
            {
                if (this._nuevosFlujosAprobacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("medicosfichero", m_medicosfichero, value));
                    _nuevosFlujosAprobacion = value;

                }
            }
        }

        private Boolean m_veeva;

        //[PropiedadOriginal("preaprobadaneg", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Boolean veeva
        {
            get { return m_veeva; }
            set
            {
                if (this.m_veeva != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("preaprobadaneg", m_preaprobadaneg, value));
                    m_veeva = value;

                }
            }
        }

        private Boolean m_newco;

        //[PropiedadOriginal("preaprobadaneg", EsNullable = true, TablaOriginal = "amecs", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Boolean newco
        {
            get { return m_newco; }
            set
            {
                if (this.m_newco != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("preaprobadaneg", m_preaprobadaneg, value));
                    m_newco = value;

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

        public ListadoAmecs()
            {
            }
            #endregion

            #region Converter

            public static ICollection<ListadoAmecs> ConvertToDto(DataTable dtTable)
            {
                ICollection<ListadoAmecs> collection = new Collection<ListadoAmecs>();
            foreach (DataRow row in dtTable.Rows)
            {
                ListadoAmecs data = new ListadoAmecs()
                {
                    idamecs = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idamecs"),
                    preaprobadaneg = Quodem.Utility.DataLayerUtil.GetIntValue(row, "preaprobadaneg") ==1,
                    preaprobadaleg = Quodem.Utility.DataLayerUtil.GetIntValue(row, "preaprobadaleg") ==1,
                    farmaindustria = Quodem.Utility.DataLayerUtil.GetIntValue(row, "farmaindustria") ==1,
                    casosclinicos = Quodem.Utility.DataLayerUtil.GetIntValue(row, "casosclinicos") ==1,
                    participantesmsd = Int64.Parse(Quodem.Utility.DataLayerUtil.GetIntValue(row, "participantesmsd").ToString()),
                    detallecriterios = Quodem.Utility.DataLayerUtil.GetStringValue(row, "detallecriterios"),
                    medicosfichero = Quodem.Utility.DataLayerUtil.GetIntValue(row, "medicosfichero") ==1,
                    duracionhoras = Quodem.Utility.DataLayerUtil.GetStringValue(row, "duracionhoras"),
                    ponentespatrocinados = Int64.Parse(Quodem.Utility.DataLayerUtil.GetIntValue(row, "ponentespatrocinados").ToString()),
                    conceptogastos = Quodem.Utility.DataLayerUtil.GetStringValue(row, "conceptogastos"),
                    importegasto = Quodem.Utility.DataLayerUtil.GetIntValue(row, "importegasto"),
                    cartascontrato = Int64.Parse(Quodem.Utility.DataLayerUtil.GetIntValue(row, "cartascontrato").ToString()),
                    programaamecs = Quodem.Utility.DataLayerUtil.GetStringValue(row, "programaamecs"),
                    urlprograma = Quodem.Utility.DataLayerUtil.GetStringValue(row, "urlprograma"),
                    descripcionobjetivo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "descripcionobjetivo"),
                    descripcion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "descripcion"),
                    nwein = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nwein"),
                    cargoadaxas = Quodem.Utility.DataLayerUtil.GetStringValue(row, "cargoadaxas"),
                    preaprobadaamed = Quodem.Utility.DataLayerUtil.GetIntValue(row, "preaprobadaamed") ==1,
                    idsolicitante = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idsolicitante"),
                    idcargo = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcargo"),
                    idcreadopor = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcreadopor"),
                    fechaamecs = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaamecs") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaamecs"),
                    idtipoactividad = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idtipoactividad"),
                    paraguas = Quodem.Utility.DataLayerUtil.GetIntValue(row, "paraguas") ==1,
                    idestado = Int32.Parse(Quodem.Utility.DataLayerUtil.GetIntValue(row, "idestado").ToString()),
                    estado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "estado"),
                    nuevosFlujosAprobacion = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "nuevosFlujosAprobacion"),
                    veeva = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "veeva"),
                    newco = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "newco")
                };
                collection.Add(data);
            }
            return collection;
            }

            #endregion
    }
}
