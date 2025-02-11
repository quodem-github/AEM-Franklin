using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DTipoActividad
    {
        #region Propiedades
        private Int32 m_IdTipoActividad;

        //[PropiedadOriginal("IdTipoActividad", EsClave = true, TablaOriginal = "tipos_actividad_congreso", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IdTipoActividad
        {
            get { return m_IdTipoActividad; }
            set
            {
                if (this.m_IdTipoActividad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdTipoActividad", m_IdTipoActividad, value));
                    m_IdTipoActividad = value;

                }
            }
        }
        private String m_Nombre;

        //[PropiedadOriginal("Nombre", TablaOriginal = "tipos_actividad_congreso", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 35)]
        public String Nombre
        {
            get { return m_Nombre; }
            set
            {
                if (this.m_Nombre != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Nombre", m_Nombre, value));
                    m_Nombre = value;

                }
            }
        }
        #endregion

        #region Constructores

        public DTipoActividad()
        {
        }
        public DTipoActividad(Int32 _IdTipoActividad)
        {
            IdTipoActividad = _IdTipoActividad;
        }

        #endregion

        #region Converter

        public static IList<DTipoActividad> ConvertToDto(DataTable dtTable)
        {
            IList<DTipoActividad> collection = new Collection<DTipoActividad>();

            foreach (DataRow row in dtTable.Rows)
            {
                DTipoActividad data = new DTipoActividad()
                {
                    IdTipoActividad = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdTipoActividad")),
                    Nombre = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Nombre")
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
    }
}
