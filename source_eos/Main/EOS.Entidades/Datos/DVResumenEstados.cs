using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DVResumenEstados
	{
		#region Propiedades
		private String m_IdEstado;

		//[PropiedadOriginal("IdEstado"   , TablaOriginal = "reservasviajes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
		public String IdEstado
		{
			get { return m_IdEstado; }
			set {
				if (this.m_IdEstado != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEstado", m_IdEstado, value));
					m_IdEstado = value;
					
				}
			}
		}
		private Int64 m_Total;

		//[PropiedadOriginal("Total"   , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.Int64 , Longitud = 21)]
		public Int64 Total
		{
			get { return m_Total; }
			set {
				if (this.m_Total != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Total", m_Total, value));
					m_Total = value;
					
				}
			}
		}
		#endregion

		#region Constructores

		public DVResumenEstados()
		{
		}

		#endregion

        #region Converter

        public static ICollection<DVResumenEstados> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVResumenEstados> collection = new Collection<DVResumenEstados>();

            foreach(DataRow row in dtTable.Rows)
            {
                DVResumenEstados data = new DVResumenEstados()
                {
                    IdEstado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEstado"),
                    Total = long.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Total"))
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
	}
}
