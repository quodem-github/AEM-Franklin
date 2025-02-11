using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DVPais
	{
		#region Propiedades
		private String m_IdPais;

		//[PropiedadOriginal("IdPoblacion" ,EsClave = true  , TablaOriginal = "cv_poblaciones_ampliadas" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public String IdPais
		{
			get { return m_IdPais; }
			set {
				if (this.m_IdPais != value)
				{
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdPoblacion", m_IdPoblacion, value));
                    m_IdPais = value;
					
				}
			}
		}
		private String m_Pais;

		//[PropiedadOriginal("Poblacion"   , TablaOriginal = "cv_poblaciones_ampliadas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 30)]
		public String Pais
		{
			get { return m_Pais; }
			set {
				if (this.m_Pais != value)
				{
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Poblacion", m_Poblacion, value));
                    m_Pais = value;
					
				}
			}
		}
        private String m_IsoCode;

        //[PropiedadOriginal("Poblacion"   , TablaOriginal = "cv_poblaciones_ampliadas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 30)]
        public String IsoCode
        {
            get { return m_IsoCode; }
            set
            {
                if (this.m_IsoCode != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Poblacion", m_Poblacion, value));
                    m_IsoCode = value;

                }
            }
        }
        private String m_CountryName;

        //[PropiedadOriginal("Poblacion"   , TablaOriginal = "cv_poblaciones_ampliadas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 30)]
        public String CountryName
        {
            get { return m_CountryName; }
            set
            {
                if (this.m_CountryName != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Poblacion", m_Poblacion, value));
                    m_CountryName = value;

                }
            }
        }
        #endregion

        #region Constructores

        public DVPais()
		{
		}
		public DVPais(String _IdPais)
		{
			IdPais = _IdPais;
		}

        #endregion

        #region Converter

        public static IList<DVPais> ConvertToDto(DataTable dtTable)
        {
            IList<DVPais> collection = new Collection<DVPais>();

            foreach (DataRow row in dtTable.Rows)
            {
                DVPais data = new DVPais()
                {
                    IdPais = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdPais"),
                    Pais = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Pais"),
                    IsoCode = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IsoCode"),
                    CountryName = Quodem.Utility.DataLayerUtil.GetStringValue(row, "CountryName")
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion

    }
}
