using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DVPoblacion
	{
		#region Propiedades
		private Int32 m_IdPoblacion;

		//[PropiedadOriginal("IdPoblacion" ,EsClave = true  , TablaOriginal = "cv_poblaciones_ampliadas" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 IdPoblacion
		{
			get { return m_IdPoblacion; }
			set {
				if (this.m_IdPoblacion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdPoblacion", m_IdPoblacion, value));
					m_IdPoblacion = value;
					
				}
			}
		}
		private String m_Poblacion;

		//[PropiedadOriginal("Poblacion"   , TablaOriginal = "cv_poblaciones_ampliadas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 30)]
		public String Poblacion
		{
			get { return m_Poblacion; }
			set {
				if (this.m_Poblacion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Poblacion", m_Poblacion, value));
					m_Poblacion = value;
					
				}
			}
		}
		private String m_IdProvincia;

		//[PropiedadOriginal("IdProvincia"  ,EsNullable = true , TablaOriginal = "cv_poblaciones_ampliadas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 3)]
		public String IdProvincia
		{
			get { return m_IdProvincia; }
			set {
				if (this.m_IdProvincia != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdProvincia", m_IdProvincia, value));
					m_IdProvincia = value;
					
				}
			}
		}
		private String m_Provincia;

		//[PropiedadOriginal("Provincia"  ,EsNullable = true , TablaOriginal = "cv_poblaciones_ampliadas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String Provincia
		{
			get { return m_Provincia; }
			set {
				if (this.m_Provincia != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Provincia", m_Provincia, value));
					m_Provincia = value;
					
				}
			}
		}
		private String m_IdPais;

		//[PropiedadOriginal("IdPais"  ,EsNullable = true , TablaOriginal = "cv_poblaciones_ampliadas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 3)]
		public String IdPais
		{
			get { return m_IdPais; }
			set {
				if (this.m_IdPais != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdPais", m_IdPais, value));
					m_IdPais = value;
					
				}
			}
		}
		private String m_Pais;

		//[PropiedadOriginal("Pais"  ,EsNullable = true , TablaOriginal = "cv_poblaciones_ampliadas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String Pais
		{
			get { return m_Pais; }
			set {
				if (this.m_Pais != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Pais", m_Pais, value));
					m_Pais = value;
					
				}
			}
		}
		#endregion

		#region Constructores

		public DVPoblacion()
		{
		}
		public DVPoblacion(Int32 _IdPoblacion)
		{
			IdPoblacion = _IdPoblacion;
		}

		#endregion

        #region Converter

        public static IList<DVPoblacion> ConvertToDto(DataTable dtTable)
        {
            IList<DVPoblacion> collection = new Collection<DVPoblacion>();

            foreach (DataRow row in dtTable.Rows)
            {
                DVPoblacion data = new DVPoblacion()
                {
                    IdPoblacion = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdPoblacion")),
                    Poblacion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Poblacion"),
                    IdProvincia = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdProvincia"),
                    Provincia = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Provincia"),
                    IdPais = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdPais"),
                    Pais = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Pais")
                };
            
                collection.Add(data);
            }

            return collection;
        }

        #endregion
	}
}
