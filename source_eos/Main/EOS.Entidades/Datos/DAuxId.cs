using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
	public class DAuxId
	{
		#region Propiedades
		private Int32 m_IDENTIFICADOR;

		//[PropiedadOriginal("IDENTIFICADOR" ,EsClave = true  , TablaOriginal = "reservasviajes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 IDENTIFICADOR
		{
			get { return m_IDENTIFICADOR; }
			set {
				if (this.m_IDENTIFICADOR != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IDENTIFICADOR", m_IDENTIFICADOR, value));
					m_IDENTIFICADOR = value;
					
				}
			}
		}
		#endregion

		#region Constructores

		public DAuxId()
		{
		}
		public DAuxId(Int32 _IDENTIFICADOR)
		{
			IDENTIFICADOR = _IDENTIFICADOR;
		}

		#endregion

        #region Converter

        public static ICollection<DAuxId> ConvertToDto(DataTable dtTable)
        {
            ICollection<DAuxId> collection = new Collection<DAuxId>();
            foreach (DataRow row in dtTable.Rows)
            {
                DAuxId data = new DAuxId()
                {
                    IDENTIFICADOR = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IDENTIFICADOR"),
                   
                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
	}
}
