using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DDocumentacionAmec
    {
        #region Propiedades
        private Int32 m_iddocumentacion;

        //[PropiedadOriginal("iddocumentacion", TablaOriginal = "docasocamecs", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 iddocumentacion
        {
            get { return m_iddocumentacion; }
            set
            {
                if (this.m_iddocumentacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("iddocumentacion", m_iddocumentacion, value));
                    m_iddocumentacion = value;

                }
            }
        }
        private string m_idamecs;

        //[PropiedadOriginal("idamecs", TablaOriginal = "docasocamecs", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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


        private String m_tipodoc;

        //[PropiedadOriginal("tipodoc", TablaOriginal = "docasocamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String tipodoc
        {
            get { return m_tipodoc; }
            set
            {
                if (this.m_tipodoc != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("tipodoc", m_tipodoc, value));
                    m_tipodoc = value;

                }
            }
        }

        private Int32 m_idcreadopor;

        //[PropiedadOriginal("idcreadopor", TablaOriginal = "docasocamecs", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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


        private String m_ubicaciondoc;

        //[PropiedadOriginal("ubicaciondoc", TablaOriginal = "docasocamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String ubicaciondoc
        {
            get { return m_ubicaciondoc; }
            set
            {
                if (this.m_ubicaciondoc != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ubicaciondoc", m_ubicaciondoc, value));
                    m_ubicaciondoc = value;

                }
            }
        }

        private String m_nombredoc;

        //[PropiedadOriginal("nombredoc", TablaOriginal = "docasocamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String nombredoc
        {
            get { return m_nombredoc; }
            set
            {
                if (this.m_nombredoc != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("nombredoc", m_nombredoc, value));
                    m_nombredoc = value;

                }
            }
        }

        private String m_comentariosdoc;

        //[PropiedadOriginal("comentariosdoc", TablaOriginal = "docasocamecs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String comentariosdoc
        {
            get { return m_comentariosdoc; }
            set
            {
                if (this.m_comentariosdoc != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("comentariosdoc", m_comentariosdoc, value));
                    m_comentariosdoc = value;

                }
            }
        }


        private Nullable<Boolean> m_adjuntaraemail;

        //[PropiedadOriginal("adjuntaraemail", TablaOriginal = "docasocamecs", TipoProveedor = ClepsydraDbType.Byte, Longitud = 11)]
        public Nullable<Boolean> adjuntaraemail
        {
            get { return m_adjuntaraemail; }
            set
            {
                if (this.m_adjuntaraemail != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("adjuntaraemail", m_adjuntaraemail, value));
                    m_adjuntaraemail = value;

                }
            }
        }

        private Nullable<DateTime> m_fechacreacion;

        //[PropiedadOriginal("fechacreacion", EsNullable = true, TablaOriginal = "docasocamecs", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
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

        private Int32 m_idcategoriadocumento;

        //[PropiedadOriginal("idcategoriadocumento", TablaOriginal = "docasocamecs", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 idcategoriadocumento
        {
            get { return m_idcategoriadocumento; }
            set
            {
                if (this.m_idcategoriadocumento != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idcategoriadocumento", m_idcategoriadocumento, value));
                    m_idcategoriadocumento = value;

                }
            }
        }

        private String m_categoriadocumento;

        //[PropiedadOriginal("categoriadocumento", EsNullable = true, TablaOriginal = "categoria_documento", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 100)]
        public String categoriadocumento
        {
            get { return m_categoriadocumento; }
            set
            {
                if (this.m_categoriadocumento != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("categoriadocumento", m_categoriadocumento, value));
                    m_categoriadocumento = value;

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

        #endregion


        #region Constructores

        public DDocumentacionAmec()
        {
        }

        #endregion


        #region Converter

        public static ICollection<DDocumentacionAmec> ConvertToDto(DataTable dtTable)
        {
            ICollection<DDocumentacionAmec> collection = new Collection<DDocumentacionAmec>();
            foreach (DataRow row in dtTable.Rows)
            {
                DDocumentacionAmec data = new DDocumentacionAmec()
                {
                    categoriadocumento = Quodem.Utility.DataLayerUtil.GetStringValue(row, "categoriadocumento"),
                    idcategoriadocumento = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcategoriadocumento"),
                    fechacreacion = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacreacion") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacreacion"),
                    adjuntaraemail = Quodem.Utility.DataLayerUtil.GetStringValue(row, "adjuntaraemail") == "1",
                    comentariosdoc = Quodem.Utility.DataLayerUtil.GetStringValue(row, "comentariosdoc"),
                    nombredoc = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nombredoc"),
                    ubicaciondoc = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ubicaciondoc"),
                    idcreadopor = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcreadopor"),
                    tipodoc = Quodem.Utility.DataLayerUtil.GetStringValue(row, "tipodoc"),
                    idamecs = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idamecs"),
                    iddocumentacion = Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddocumentacion"),
                    nombreusuario = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nombreusuario"),
                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
    }


}
