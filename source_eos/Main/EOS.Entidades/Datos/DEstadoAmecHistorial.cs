using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DEstadoAmecHistorial
    {

        #region Constructores

        public DEstadoAmecHistorial()
        {

        }
        #endregion

        #region Propiedades

        private Int32 m_IdEstadoAmecHist;

        //[PropiedadOriginal("idestadoamechist", EsClave = true, TablaOriginal = "dbo_iw_estadoamechist;", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IDEstadoAmecHist
        {
            get { return m_IdEstadoAmecHist; }
            set
            {
                if (this.m_IdEstadoAmecHist != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idestadoamechist", m_IdEstadoAmecHist, value));
                    m_IdEstadoAmecHist = value;

                }
            }
        }

        private string m_idamecs;

        //[PropiedadOriginal("idamecs", TablaOriginal = "dbo_iw_estadoamechist;", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public string IdAmecs
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

        private Int32 m_idestado;

        //[PropiedadOriginal("idestado", TablaOriginal = "dbo_iw_estadoamechist;", TipoProveedor = ClepsydraDbType.Int32, Longitud = 10)]
        public Int32 IdEstado
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

        private Int32 m_idnivelaprobacion;

        //[PropiedadOriginal("idnivelaprobacion", TablaOriginal = "dbo_iw_estadoamechist;", TipoProveedor = ClepsydraDbType.Int32, Longitud = 10)]
        public Int32 IdEstadoAprobacion
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

        private String m_ComentariosAprob;

        //[PropiedadOriginal("comentariosaprob", TablaOriginal = "dbo_iw_estadoamechist", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String Congreso
        {
            get { return m_ComentariosAprob; }
            set
            {
                if (this.m_ComentariosAprob != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("comentariosaprob", m_ComentariosAprob, value));
                    m_ComentariosAprob = value;

                }
            }
        }

        private Int32 m_idCreadoPor;

        //[PropiedadOriginal("idcreadopor", TablaOriginal = "dbo_iw_estadoamechist;", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IdCreador
        {
            get { return m_idCreadoPor; }
            set
            {
                if (this.m_idCreadoPor != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idcreadorpor", m_idCreadoPor, value));
                    m_idCreadoPor = value;

                }
            }
        }

        private Int32 m_fechacreacion;

        //[PropiedadOriginal("fechacreacion", TablaOriginal = "dbo_iw_aprovadoramec;", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public Int32 FechaCreacion
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
    }
}
