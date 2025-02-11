using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DTipoRegistroActividadFlujo
    {
        #region Propiedades

        private UInt32 m_idtiporegistroactividad;

        //[PropiedadOriginal("idtipoactividad", TablaOriginal = "tipoactividad", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public UInt32 idtiporegistroactividad
        {
            get { return m_idtiporegistroactividad; }
            set
            {
                if (this.m_idtiporegistroactividad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtipoactividad", m_idtipoactividad, value));
                    m_idtiporegistroactividad = value;

                }
            }
        }


        private String m_tipogrupoactividad;

        //[PropiedadOriginal("tipoactividad", TablaOriginal = "tipoactividad", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        public String tipogrupoactividad
        {
            get { return m_tipogrupoactividad; }
            set
            {
                if (this.m_tipogrupoactividad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("tipoactividad", m_tipoactividad, value));
                    m_tipogrupoactividad = value;

                }
            }
        }        

        #endregion


        #region constructor
        public DTipoRegistroActividadFlujo() { }
        public DTipoRegistroActividadFlujo(UInt32 idtiporegistroactividad)
        {
        }

        #endregion

        #region Converter

        public static ICollection<DTipoRegistroActividadFlujo> ConvertToDto(DataTable dtTable)
        {
            ICollection<DTipoRegistroActividadFlujo> collection = new Collection<DTipoRegistroActividadFlujo>();

            foreach (DataRow row in dtTable.Rows)
            {
                DTipoRegistroActividadFlujo data = new DTipoRegistroActividadFlujo() {                 

                    idtiporegistroactividad = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idtiporegistroactividad")),
                    tipogrupoactividad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "tiporegistroactividad")

                };
                collection.Add(data);
            }

            return collection;
        }

        #endregion

    }
}