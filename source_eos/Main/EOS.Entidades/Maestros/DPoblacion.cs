using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DPoblacion
    {
        #region Propiedades
        private Int32 m_IdPoblacion;

        //[PropiedadOriginal("IdPoblacion", EsClave = true, TablaOriginal = "poblaciones", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IdPoblacion
        {
            get { return m_IdPoblacion; }
            set
            {
                if (this.m_IdPoblacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdPoblacion", m_IdPoblacion, value));
                    m_IdPoblacion = value;

                }
            }
        }
        private String m_Poblacion;

        //[PropiedadOriginal("Poblacion", TablaOriginal = "poblaciones", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 30)]
        public String Poblacion
        {
            get { return m_Poblacion; }
            set
            {
                if (this.m_Poblacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Poblacion", m_Poblacion, value));
                    m_Poblacion = value;

                }
            }
        }
        #endregion

        #region Constructores

        public DPoblacion()
        {
        }
        public DPoblacion(Int32 _IdPoblacion)
        {
            IdPoblacion = _IdPoblacion;
        }

        #endregion

        #region Converter

        public static IList<DPoblacion> ConvertToDto(DataTable dtTable)
        {
            IList<DPoblacion> collection = new Collection<DPoblacion>();

            foreach (DataRow row in dtTable.Rows)
            {
                DPoblacion data = new DPoblacion()
                {
                    IdPoblacion = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdPoblacion")),
                    Poblacion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Poblacion")
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
    }
}
