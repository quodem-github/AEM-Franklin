using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DTipoActividadFlujo
    {
        #region Propiedades

        private UInt32 m_idtipoactividad;

        //[PropiedadOriginal("idtipoactividad", TablaOriginal = "tipoactividad", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
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


        private String m_tipoactividad;

        //[PropiedadOriginal("tipoactividad", TablaOriginal = "tipoactividad", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        public String tipoactividad
        {
            get { return m_tipoactividad; }
            set
            {
                if (this.m_tipoactividad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("tipoactividad", m_tipoactividad, value));
                    m_tipoactividad = value;

                }
            }
        }

        private UInt32 m_idtiporegistroactividad;

        //[PropiedadOriginal("idtiporegistroactividad", TablaOriginal = "tipoactividad", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public UInt32 idtiporegistroactividad
        {
            get { return m_idtiporegistroactividad; }
            set
            {
                if (this.m_idtiporegistroactividad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtiporegistroactividad", m_idtiporegistroactividad, value));
                    m_idtiporegistroactividad = value;

                }
            }
        }

        private bool m_veeva;

        //[PropiedadOriginal("idtiporegistroactividad", TablaOriginal = "tipoactividad", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public bool veeva
        {
            get { return m_veeva; }
            set
            {
                if (this.m_veeva != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtiporegistroactividad", m_idtiporegistroactividad, value));
                    m_veeva = value;

                }
            }
        }

        #endregion


        #region constructor
        public DTipoActividadFlujo() { }
        public DTipoActividadFlujo(UInt32 idtipoactividad)
        {
        }

        #endregion

        #region Converter

        public static ICollection<DTipoActividadFlujo> ConvertToDto(DataTable dtTable)
        {
            ICollection<DTipoActividadFlujo> collection = new Collection<DTipoActividadFlujo>();

            foreach (DataRow row in dtTable.Rows)
            {
                DTipoActividadFlujo data = new DTipoActividadFlujo() { 
                
                    idtipoactividad = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idtipoactividad")),
                    tipoactividad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "tipoactividad"),
                    idtiporegistroactividad = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idtiporegistroactividad")),
                    veeva = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "veeva")

                };
                collection.Add(data);
            }

            return collection;
        }

        #endregion

    }
}