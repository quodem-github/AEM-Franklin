using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DCentroCoste
    {
        #region Propiedades
        private Int32 m_IdCentroCoste;

        public Int32 IdCentroCoste
        {
            get { return m_IdCentroCoste; }
            set
            {
                if (this.m_IdCentroCoste != value)
                {
                    m_IdCentroCoste = value;
                }
            }
        }
        private String m_CentroCoste;

        public String CentroCoste
        {
            get { return m_CentroCoste; }
            set
            {
                if (this.m_CentroCoste != value)
                {
                    m_CentroCoste = value;
                }
            }
        }

        private Int32 m_IdCentroCosteCategoria;

        public Int32 IdCentroCosteCategoria
        {
            get { return m_IdCentroCosteCategoria; }
            set
            {
                if (this.m_IdCentroCosteCategoria != value)
                {
                    m_IdCentroCosteCategoria = value;
                }
            }
        }
        private String m_CentroCosteCategoria;

        public String CentroCosteCategoria
        {
            get { return m_CentroCosteCategoria; }
            set
            {
                if (this.m_CentroCosteCategoria != value)
                {
                    m_CentroCosteCategoria = value;
                }
            }
        }
        #endregion

        #region Constructores

        public DCentroCoste()
        {
        }
        public DCentroCoste(Int32 _IdCentroCoste)
        {
            IdCentroCoste = _IdCentroCoste;
        }

        #endregion

        #region Converter

        public static IList<DCentroCoste> ConvertToDto(DataTable dtTable)
        {
            IList<DCentroCoste> collection = new Collection<DCentroCoste>();

            foreach (DataRow row in dtTable.Rows)
            {
                DCentroCoste data = new DCentroCoste()
                {
                    IdCentroCoste = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idcentrocoste")),
                    CentroCoste = Quodem.Utility.DataLayerUtil.GetStringValue(row, "centrocoste"),
                    IdCentroCosteCategoria = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idcentrocostecategoria")),
                    CentroCosteCategoria = Quodem.Utility.DataLayerUtil.GetStringValue(row, "centrocostecategoria")
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
    }
}
