using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DVAmecCongresoConcatSolicitante
	{
		#region Propiedades
		private string m_IdAmec;

		//[PropiedadOriginal("IdAmec"   , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public string IdAmec
		{
			get { return m_IdAmec; }
			set {
				if (this.m_IdAmec != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdAmec", m_IdAmec, value));
					m_IdAmec = value;
					
				}
			}
		}

        private String m_idamecs;

        //[PropiedadOriginal("idamecs", TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        public String idamecs
        {
            get { return m_idamecs; }
            set
            {
                if (this.m_idamecs != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idamecs", m_idamecs, value));
                    m_idamecs = value;

                }
            }
        }
        private Int32 m_idsolicitante;

        //[PropiedadOriginal("idsolicitante", TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 idsolicitante
        {
            get { return m_idsolicitante; }
            set
            {
                if (this.m_idsolicitante != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idsolicitante", m_idsolicitante, value));
                    m_idsolicitante = value;

                }
            }
        }

        private String m_AmecConcatSolicitante;

        //[PropiedadOriginal("AmecConcatSolicitante", TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        public String AmecConcatSolicitante
        {
            get { return m_AmecConcatSolicitante; }
            set
            {
                if (this.m_AmecConcatSolicitante != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("AmecConcatSolicitante", m_AmecConcatSolicitante, value));
                    m_AmecConcatSolicitante = value;

                }
            }
        }

		private String m_AMEC;

		//[PropiedadOriginal("AMEC"   , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String AMEC
		{
			get { return m_AMEC; }
			set {
				if (this.m_AMEC != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("AMEC", m_AMEC, value));
					m_AMEC = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdCongreso;

		//[PropiedadOriginal("IdCongreso"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdCongreso
		{
			get { return m_IdCongreso; }
			set {
				if (this.m_IdCongreso != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdCongreso", m_IdCongreso, value));
					m_IdCongreso = value;
					
				}
			}
		}
		private String m_Actividad;

		//[PropiedadOriginal("Actividad"   , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 70)]
		public String Actividad
		{
			get { return m_Actividad; }
			set {
				if (this.m_Actividad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Actividad", m_Actividad, value));
					m_Actividad = value;
					
				}
			}
		}
		
		#endregion

		#region Constructores

		public DVAmecCongresoConcatSolicitante()
		{
		}
	

		#endregion

        #region Converter

        public static ICollection<DVAmecCongresoConcatSolicitante> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVAmecCongresoConcatSolicitante> collection = new Collection<DVAmecCongresoConcatSolicitante>();

            foreach(DataRow row in dtTable.Rows)
            {
                DVAmecCongresoConcatSolicitante data = new DVAmecCongresoConcatSolicitante()
                {
                    IdAmec = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdAmec"),
                    idamecs = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idamecs"),
                    idsolicitante = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idsolicitante")) ? 0 : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idsolicitante"))),
                    AmecConcatSolicitante = Quodem.Utility.DataLayerUtil.GetStringValue(row, "AmecConcatSolicitante"),
                    AMEC = Quodem.Utility.DataLayerUtil.GetStringValue(row, "AMEC"),
                    IdCongreso = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdCongreso")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdCongreso"))),
                    Actividad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Actividad")
                };

                collection.Add(data);
            }               

            return collection;
        }

        #endregion
	}
}
