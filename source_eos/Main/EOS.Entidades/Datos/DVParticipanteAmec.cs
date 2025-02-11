using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DVParticipanteAmec
	{
		#region Propiedades
		private Int32 m_IdAmecPassengerList;

		//[PropiedadOriginal("IdAmecPassengerList" ,EsClave = true  , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 IdAmecPassengerList
		{
			get { return m_IdAmecPassengerList; }
			set {
				if (this.m_IdAmecPassengerList != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdAmecPassengerList", m_IdAmecPassengerList, value));
					m_IdAmecPassengerList = value;
					
				}
			}
		}
		private Int32 m_IdAmec;

		//[PropiedadOriginal("IdAmec"   , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 IdAmec
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
		private Int32 m_IdPassengerList;

		//[PropiedadOriginal("IdPassengerList"   , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 IdPassengerList
		{
			get { return m_IdPassengerList; }
			set {
				if (this.m_IdPassengerList != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdPassengerList", m_IdPassengerList, value));
					m_IdPassengerList = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdTipoPatrocinio;

		//[PropiedadOriginal("IdTipoPatrocinio"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdTipoPatrocinio
		{
			get { return m_IdTipoPatrocinio; }
			set {
				if (this.m_IdTipoPatrocinio != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdTipoPatrocinio", m_IdTipoPatrocinio, value));
					m_IdTipoPatrocinio = value;
					
				}
			}
		}
		private String m_TipoPatrocinio;

		//[PropiedadOriginal("TipoPatrocinio"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
		public String TipoPatrocinio
		{
			get { return m_TipoPatrocinio; }
			set {
				if (this.m_TipoPatrocinio != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("TipoPatrocinio", m_TipoPatrocinio, value));
					m_TipoPatrocinio = value;
					
				}
			}
		}
		private String m_Nombre;

		//[PropiedadOriginal("Nombre"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String Nombre
		{
			get { return m_Nombre; }
			set {
				if (this.m_Nombre != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Nombre", m_Nombre, value));
					m_Nombre = value;
					
				}
			}
		}
		private String m_Apel1;

		//[PropiedadOriginal("Apel1"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String Apel1
		{
			get { return m_Apel1; }
			set {
				if (this.m_Apel1 != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Apel1", m_Apel1, value));
					m_Apel1 = value;
					
				}
			}
		}
		private String m_Apel2;

		//[PropiedadOriginal("Apel2"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String Apel2
		{
			get { return m_Apel2; }
			set {
				if (this.m_Apel2 != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Apel2", m_Apel2, value));
					m_Apel2 = value;
					
				}
			}
		}
		private String m_NombreCompleto;

		//[PropiedadOriginal("NombreCompleto"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 152)]
		public String NombreCompleto
		{
			get { return m_NombreCompleto; }
			set {
				if (this.m_NombreCompleto != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("NombreCompleto", m_NombreCompleto, value));
					m_NombreCompleto = value;
					
				}
			}
		}
		private String m_Email;

		//[PropiedadOriginal("Email"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
		public String Email
		{
			get { return m_Email; }
			set {
				if (this.m_Email != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Email", m_Email, value));
					m_Email = value;
					
				}
			}
		}
		private String m_Pasaporte;

		//[PropiedadOriginal("Pasaporte"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String Pasaporte
		{
			get { return m_Pasaporte; }
			set {
				if (this.m_Pasaporte != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Pasaporte", m_Pasaporte, value));
					m_Pasaporte = value;
					
				}
			}
		}
		private String m_Hospital;

		//[PropiedadOriginal("Hospital"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
		public String Hospital
		{
			get { return m_Hospital; }
			set {
				if (this.m_Hospital != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Hospital", m_Hospital, value));
					m_Hospital = value;
					
				}
			}
		}
		private String m_Localidad;

		//[PropiedadOriginal("Localidad"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 40)]
		public String Localidad
		{
			get { return m_Localidad; }
			set {
				if (this.m_Localidad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Localidad", m_Localidad, value));
					m_Localidad = value;
					
				}
			}
		}
		private String m_Msdid;

		//[PropiedadOriginal("Msdid"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 15)]
		public String Msdid
		{
			get { return m_Msdid; }
			set {
				if (this.m_Msdid != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Msdid", m_Msdid, value));
					m_Msdid = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdDistrito;

		//[PropiedadOriginal("IdDistrito"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdDistrito
		{
			get { return m_IdDistrito; }
			set {
				if (this.m_IdDistrito != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdDistrito", m_IdDistrito, value));
					m_IdDistrito = value;
					
				}
			}
		}
		private String m_Distrito;

		//[PropiedadOriginal("Distrito"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
		public String Distrito
		{
			get { return m_Distrito; }
			set {
				if (this.m_Distrito != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Distrito", m_Distrito, value));
					m_Distrito = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdRegion;

		//[PropiedadOriginal("IdRegion"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdRegion
		{
			get { return m_IdRegion; }
			set {
				if (this.m_IdRegion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdRegion", m_IdRegion, value));
					m_IdRegion = value;
					
				}
			}
		}
		private String m_Region;

		//[PropiedadOriginal("Region"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 25)]
		public String Region
		{
			get { return m_Region; }
			set {
				if (this.m_Region != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Region", m_Region, value));
					m_Region = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdEmpresa;

		//[PropiedadOriginal("IdEmpresa"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdEmpresa
		{
			get { return m_IdEmpresa; }
			set {
				if (this.m_IdEmpresa != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEmpresa", m_IdEmpresa, value));
					m_IdEmpresa = value;
					
				}
			}
		}
		private String m_RazonSocial;

		//[PropiedadOriginal("RazonSocial"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 45)]
		public String RazonSocial
		{
			get { return m_RazonSocial; }
			set {
				if (this.m_RazonSocial != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("RazonSocial", m_RazonSocial, value));
					m_RazonSocial = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdEspecialidad;

		//[PropiedadOriginal("IdEspecialidad"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdEspecialidad
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
		private String m_Especialidad;

		//[PropiedadOriginal("Especialidad"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
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
		private String m_CodEspecialidad;

		//[PropiedadOriginal("CodEspecialidad"  ,EsNullable = true , TablaOriginal = "cv_participantes_amec" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
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
		#endregion

		#region Constructores

		public DVParticipanteAmec()
		{
		}
		public DVParticipanteAmec(Int32 _IdAmecPassengerList)
		{
			IdAmecPassengerList = _IdAmecPassengerList;
		}

		#endregion

        #region Converter

        public static ICollection<DVParticipanteAmec> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVParticipanteAmec> collection = new Collection<DVParticipanteAmec>();
            foreach (DataRow row in dtTable.Rows)
            {
                DVParticipanteAmec data = new DVParticipanteAmec()
                {
                    IdAmec = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdAmec"),
                    IdAmecPassengerList = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdAmecPassengerList"),
                    IdPassengerList = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdPassengerList"),
                    IdDistrito = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdDistrito"),
                    IdEmpresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdEmpresa"),
                    IdEspecialidad = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdEspecialidad"),
                    IdRegion = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdRegion"),
                    IdTipoPatrocinio = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdTipoPatrocinio"),
                    Apel1 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Apel1"),
                    Apel2 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Apel2"),
                    CodEspecialidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "CodEspecialidad"),
                    Distrito = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Distrito"),
                    Email = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Email"),
                    Especialidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Especialidad"),
                    Hospital = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Hospital"),
                    Localidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Localidad"),
                    Msdid = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Msdid"),
                    Nombre = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Nombre"),
                    NombreCompleto = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NombreCompleto"),
                    Pasaporte = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Pasaporte"),
                    RazonSocial = Quodem.Utility.DataLayerUtil.GetStringValue(row, "RazonSocial"),
                    Region = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Region"),
                    TipoPatrocinio = Quodem.Utility.DataLayerUtil.GetStringValue(row, "TipoPatrocinio"),

                };

                collection.Add(data);
            }
            return collection;
        }

        #endregion
	}
}
