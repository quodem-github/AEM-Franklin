using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DComentariosAmec
    {
        #region Propiedades
        private Int64 m_idcomentarios;

        //[PropiedadOriginal("idcomentarios", EsClave = true, TablaOriginal = "comentasocamencs", TipoProveedor = ClepsydraDbType.Int64, Longitud = 11)]
        public Int64 idcomentarios
        {
            get { return m_idcomentarios; }
            set
            {
                if (this.m_idcomentarios != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idcomentarios", m_idcomentarios, value));
                    m_idcomentarios = value;

                }
            }
        }



        private string m_idamecs;

        //[PropiedadOriginal("idamecs", EsClave = true, TablaOriginal = "comentasocamencs", TipoProveedor = ClepsydraDbType.Int64, Longitud = 11)]
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

        private String m_comentariosdoc;

        //[PropiedadOriginal("comentariosdoc", EsNullable = true, TablaOriginal = "comentasocamencs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 11)]
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

        private int m_idcreadopor;

        //[PropiedadOriginal("idcreadopor", TablaOriginal = "comentasocamencs", TipoProveedor = ClepsydraDbType.Int64, Longitud = 11)]
        public int idcreadopor
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

        private String m_nombreusuario;

        //[PropiedadOriginal("nombreusuario", EsNullable = true, TablaOriginal = "comentasocamencs", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 11)]
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

        private Nullable<DateTime> m_fechacreacion;

        //[PropiedadOriginal("fechacreacion", EsNullable = true, TablaOriginal = "comentasocamencs", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
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

        public DComentariosAmec()
        {
        }
        public DComentariosAmec(string _idamecs)
        {
            idamecs = _idamecs;
        }

        #endregion

        #region Converter

        public static ICollection<DComentariosAmec> ConvertToDto(DataTable dtTable)
        {
            ICollection<DComentariosAmec> collection = new Collection<DComentariosAmec>();
            foreach (DataRow row in dtTable.Rows)
            {
                string idCreador = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idcreadopor");
                DComentariosAmec data = new DComentariosAmec()
                {
                    idcomentarios = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcomentarios"),
                    idamecs = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idamecs"),
                    comentariosdoc = Quodem.Utility.DataLayerUtil.GetStringValue(row, "comentariosdoc"),
                    idcreadopor = string.IsNullOrEmpty(idCreador)? new int() : int.Parse(idCreador),
                    nombreusuario = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nombreusuario"),
                    fechacreacion = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacreacion") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacreacion"),

                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
    }
}
