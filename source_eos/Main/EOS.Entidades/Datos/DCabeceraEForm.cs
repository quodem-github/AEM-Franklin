using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DCabeceraEForm
    {
        #region Constructores

        public DCabeceraEForm()
        {

        }


        #endregion

        #region Propiedades

        //[PropiedadOriginal("IdEventoFormulario", TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public UInt32 IdEventoFormulario
        {
            get { return this.idEventoFormulario; }

            set
            {
                if (this.idEventoFormulario != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEventoFormulario", this.idEventoFormulario, value));
                    this.idEventoFormulario = value;
                }
            }
        }

        //[PropiedadOriginal("descripcionGestor", TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 255)]
        public String DescripcionGestor
        {
            get { return this.descripcionGestor; }

            set
            {
                if (this.descripcionGestor != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("DescripcionGestor", this.descripcionGestor, value));
                    this.descripcionGestor = value;
                }
            }
        }

        //[PropiedadOriginal("fechaInicio", TablaOriginal = "", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public DateTime FechaInicio
        {
            get { return this.fechaInicio; }

            set
            {
                if (this.fechaInicio != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FechaInicio", this.fechaInicio, value));
                    this.fechaInicio = value;
                }
            }
        }

        //[PropiedadOriginal("fechaFin", TablaOriginal = "", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public DateTime FechaFin
        {
            get { return this.fechaFin; }

            set
            {
                if (this.fechaFin != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FechaFin", this.fechaFin, value));
                    this.fechaFin = value;
                }
            }
        }

        //[PropiedadOriginal("poblacion", TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 255)]
        public String Poblacion
        {
            get { return this.poblacion; }

            set
            {
                if (this.poblacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Poblacion", this.poblacion, value));
                    this.poblacion = value;
                }
            }
        }

        private int m_idconfempresa;
        public int idconfempresa
        {
            get { return this.m_idconfempresa; }

            set
            {
                if (this.m_idconfempresa != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Poblacion", this.poblacion, value));
                    this.m_idconfempresa = value;
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

        #endregion

        #region Variables

        private UInt32 idEventoFormulario;
        private String descripcionGestor;
        private DateTime fechaInicio;
        private DateTime fechaFin;
        private String poblacion;

        #endregion


        #region Converter

        public static ICollection<DCabeceraEForm> ConvertToDto(DataTable dtTable)
        {
            ICollection<DCabeceraEForm> collection = new Collection<DCabeceraEForm>();
            foreach (DataRow row in dtTable.Rows)
            {
                DCabeceraEForm data = new DCabeceraEForm()
                {
                    FechaFin = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaFin"),
                    FechaInicio = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaInicio"),
                    DescripcionGestor = Quodem.Utility.DataLayerUtil.GetStringValue(row, "DescripcionGestor"),
                    Poblacion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Poblacion"),
                    IdEventoFormulario = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEventoFormulario")),
                    idconfempresa = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idconfempresa")),
                    LinkGestorInvitados = Quodem.Utility.DataLayerUtil.GetStringValue(row, "LinkGestorInvitados"),
                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
    }
}
