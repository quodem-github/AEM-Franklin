using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DAprovadorAmec
    {
        #region Constructores

        public DAprovadorAmec()
        {

        }
        #endregion

        #region Propiedades

        private Int32 m_IdAprobadorAMEC;

        public Int32 IdAprobadorAMEC
        {
            get { return m_IdAprobadorAMEC; }
            set
            {
                if (this.m_IdAprobadorAMEC != value)
                {
                    m_IdAprobadorAMEC = value;

                }
            }
        }

        private string m_idamecs;

        public string IdAmecs
        {
            get { return m_idamecs; }
            set
            {
                if (this.m_idamecs != value)
                {
                    m_idamecs = value;

                }
            }
        }

        private Int32 m_idCreadoPor;

        public Int32 IdCreador
        {
            get { return m_idCreadoPor; }
            set
            {
                if (this.m_idCreadoPor != value)
                {
                    m_idCreadoPor = value;

                }
            }
        }

        private Int32 m_idaprovador;

        public Int32 IdAprovador
        {
            get { return m_idaprovador; }
            set
            {
                if (this.m_idaprovador != value)
                {
                    m_idaprovador = value;

                }
            }
        }

        private Int32 m_fechacreacion;

        public Int32 FechaCreacion
        {
            get { return m_fechacreacion; }
            set
            {
                if (this.m_fechacreacion != value)
                {
                    m_fechacreacion = value;

                }
            }
        }

        #endregion


        #region Converter

        public static ICollection<DAprovadorAmec> ConvertToDto(DataTable dtTable)
        {
            ICollection<DAprovadorAmec> collection = new Collection<DAprovadorAmec>();
            foreach (DataRow row in dtTable.Rows)
            {
                DAprovadorAmec data = new DAprovadorAmec()
                {
                    FechaCreacion = Quodem.Utility.DataLayerUtil.GetIntValue(row, "fechacreacion"),
                    IdAmecs = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idamecs"),
                    IdAprobadorAMEC = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idaprobadorAMEC"),
                    IdAprovador = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idaprovador"),
                    IdCreador = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idcreador"),

                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
    }
}
