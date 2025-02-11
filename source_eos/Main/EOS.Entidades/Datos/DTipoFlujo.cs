using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;



namespace EOS.Entidades.Datos
{
    public class DTipoFlujo
    {
        #region Propiedades

        private UInt32 m_idtipoflujo;

        //[PropiedadOriginal("idtipoflujo", TablaOriginal = "tipoflujo", TipoProveedor = ClepsydraDbType.UnsignedInt, Longitud = 11)]
        public UInt32 idtipoflujo
        {
            get { return m_idtipoflujo; }
            set
            {
                if (this.m_idtipoflujo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtipoflujo", m_idtipoflujo, value));
                    m_idtipoflujo = value;

                }
            }
        }


        private String m_tipoflujo;

        //[PropiedadOriginal("tipoflujo", TablaOriginal = "tipoflujo", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        public String tipoflujo
        {
            get { return m_tipoflujo; }
            set
            {
                if (this.m_tipoflujo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("tipoflujo", m_tipoflujo, value));
                    m_tipoflujo = value;

                }
            }
        }

        #endregion

        #region constructor
        public DTipoFlujo() { }
        public DTipoFlujo(UInt32 idtipoflujo)
        { 
        }

        #endregion

        #region Converter

        public static ICollection<DTipoFlujo> ConvertToDto(DataTable dtTable)
        {
            ICollection<DTipoFlujo> collection = new Collection<DTipoFlujo>();
            foreach (DataRow row in dtTable.Rows)
            {
                DTipoFlujo data = new DTipoFlujo()
                {
                    idtipoflujo = UInt32.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idtipoflujo")),
                    tipoflujo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "tipoflujo"),


                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion

    }
}
