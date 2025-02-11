using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DTipoEspecialidad
	{
		#region Propiedades
		private Int32 m_IdEspecialidad;

		//[PropiedadOriginal("IdEspecialidad" ,EsClave = true  , TablaOriginal = "especialidades" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 IdEspecialidad
		{
			get { return m_IdEspecialidad; }
			set {
				if (this.m_IdEspecialidad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEspecialidad", m_IdEspecialidad, value));
					m_IdEspecialidad = value;
					
				}
			}
		}
		private String m_CodEspecialidad;

		//[PropiedadOriginal("CodEspecialidad"  ,EsNullable = true , TablaOriginal = "especialidades" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
		public String CodEspecialidad
		{
			get { return m_CodEspecialidad; }
			set {
				if (this.m_CodEspecialidad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("CodEspecialidad", m_CodEspecialidad, value));
					m_CodEspecialidad = value;
					
				}
			}
		}
		private String m_Especialidad;

		//[PropiedadOriginal("Especialidad"   , TablaOriginal = "especialidades" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
		public String Especialidad
		{
			get { return m_Especialidad; }
			set {
				if (this.m_Especialidad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Especialidad", m_Especialidad, value));
					m_Especialidad = value;
					
				}
			}
		}
		#endregion

		#region Constructores

		public DTipoEspecialidad()
		{
		}
		public DTipoEspecialidad(Int32 _IdEspecialidad)
		{
			IdEspecialidad = _IdEspecialidad;
		}

		#endregion

        #region Converter

        public static IList<DTipoEspecialidad> ConvertToDto(DataTable dtTable)
        {
            IList<DTipoEspecialidad> collection = new Collection<DTipoEspecialidad>();

            foreach (DataRow row in dtTable.Rows)
            {
                DTipoEspecialidad data = new DTipoEspecialidad()
                {
                    IdEspecialidad = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEspecialidad")),
                    CodEspecialidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "CodEspecialidad"),
                    Especialidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Especialidad")
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
	}
}
