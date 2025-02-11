using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DPassengerList
	{
		#region Propiedades
		private Int64 m_idpassengerlist;

		//[PropiedadOriginal("idpassengerlist" ,EsClave = true  , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.Int64 , Longitud = 11)]
		public Int64 idpassengerlist
		{
			get { return m_idpassengerlist; }
			set {
				if (this.m_idpassengerlist != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idpassengerlist", m_idpassengerlist, value));
					m_idpassengerlist = value;
					
				}
			}
		}
		private String m_nombre;

		//[PropiedadOriginal("nombre"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String nombre
		{
			get { return m_nombre; }
			set {
				if (this.m_nombre != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("nombre", m_nombre, value));
					m_nombre = value;
					
				}
			}
		}
		private String m_apel1;

		//[PropiedadOriginal("apel1"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String apel1
		{
			get { return m_apel1; }
			set {
				if (this.m_apel1 != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("apel1", m_apel1, value));
					m_apel1 = value;
					
				}
			}
		}
		private String m_apel2;

		//[PropiedadOriginal("apel2"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String apel2
		{
			get { return m_apel2; }
			set {
				if (this.m_apel2 != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("apel2", m_apel2, value));
					m_apel2 = value;
					
				}
			}
		}
		private String m_NIF;

		//[PropiedadOriginal("NIF"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String NIF
		{
			get { return m_NIF; }
			set {
				if (this.m_NIF != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("NIF", m_NIF, value));
					m_NIF = value;
					
				}
			}
		}
		private String m_DireccionEmail;

		//[PropiedadOriginal("DireccionEmail"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
		public String DireccionEmail
		{
			get { return m_DireccionEmail; }
			set {
				if (this.m_DireccionEmail != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("DireccionEmail", m_DireccionEmail, value));
					m_DireccionEmail = value;
					
				}
			}
		}
		private String m_TelefonoContacto;

		//[PropiedadOriginal("TelefonoContacto"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String TelefonoContacto
		{
			get { return m_TelefonoContacto; }
			set {
				if (this.m_TelefonoContacto != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("TelefonoContacto", m_TelefonoContacto, value));
					m_TelefonoContacto = value;
					
				}
			}
		}
		private Nullable<DateTime> m_FechaNacimiento;

		//[PropiedadOriginal("FechaNacimiento"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> FechaNacimiento
		{
			get { return m_FechaNacimiento; }
			set {
				if (this.m_FechaNacimiento != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FechaNacimiento", m_FechaNacimiento, value));
					m_FechaNacimiento = value;
					
				}
			}
		}
		private String m_Pasaporte;

		//[PropiedadOriginal("Pasaporte"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
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
		private Nullable<DateTime> m_FechaCaducidadPasaporte;

		//[PropiedadOriginal("FechaCaducidadPasaporte"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> FechaCaducidadPasaporte
		{
			get { return m_FechaCaducidadPasaporte; }
			set {
				if (this.m_FechaCaducidadPasaporte != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FechaCaducidadPasaporte", m_FechaCaducidadPasaporte, value));
					m_FechaCaducidadPasaporte = value;
					
				}
			}
		}
		private String m_NombreCentroTrabajo;

		//[PropiedadOriginal("NombreCentroTrabajo"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
		public String NombreCentroTrabajo
		{
			get { return m_NombreCentroTrabajo; }
			set {
				if (this.m_NombreCentroTrabajo != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("NombreCentroTrabajo", m_NombreCentroTrabajo, value));
					m_NombreCentroTrabajo = value;
					
				}
			}
		}
		private String m_LocalidadCentroTrabajo;

		//[PropiedadOriginal("LocalidadCentroTrabajo"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 40)]
		public String LocalidadCentroTrabajo
		{
			get { return m_LocalidadCentroTrabajo; }
			set {
				if (this.m_LocalidadCentroTrabajo != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("LocalidadCentroTrabajo", m_LocalidadCentroTrabajo, value));
					m_LocalidadCentroTrabajo = value;
					
				}
			}
		}
		private String m_DireccionCentroTrabajo;

		//[PropiedadOriginal("DireccionCentroTrabajo"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
		public String DireccionCentroTrabajo
		{
			get { return m_DireccionCentroTrabajo; }
			set {
				if (this.m_DireccionCentroTrabajo != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("DireccionCentroTrabajo", m_DireccionCentroTrabajo, value));
					m_DireccionCentroTrabajo = value;
					
				}
			}
		}
		private String m_LugarEmisionPasaporte;

		//[PropiedadOriginal("LugarEmisionPasaporte"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 40)]
		public String LugarEmisionPasaporte
		{
			get { return m_LugarEmisionPasaporte; }
			set {
				if (this.m_LugarEmisionPasaporte != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("LugarEmisionPasaporte", m_LugarEmisionPasaporte, value));
					m_LugarEmisionPasaporte = value;
					
				}
			}
		}
		private String m_NacionalidadPasaporte;

		//[PropiedadOriginal("NacionalidadPasaporte"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 40)]
		public String NacionalidadPasaporte
		{
			get { return m_NacionalidadPasaporte; }
			set {
				if (this.m_NacionalidadPasaporte != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("NacionalidadPasaporte", m_NacionalidadPasaporte, value));
					m_NacionalidadPasaporte = value;
					
				}
			}
		}
		private String m_OrganizacionVisitaProfesional;

		//[PropiedadOriginal("OrganizacionVisitaProfesional"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
		public String OrganizacionVisitaProfesional
		{
			get { return m_OrganizacionVisitaProfesional; }
			set {
				if (this.m_OrganizacionVisitaProfesional != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("OrganizacionVisitaProfesional", m_OrganizacionVisitaProfesional, value));
					m_OrganizacionVisitaProfesional = value;
					
				}
			}
		}
		private String m_DireccionProfesionalOrganizacion;

		//[PropiedadOriginal("DireccionProfesionalOrganizacion"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
		public String DireccionProfesionalOrganizacion
		{
			get { return m_DireccionProfesionalOrganizacion; }
			set {
				if (this.m_DireccionProfesionalOrganizacion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("DireccionProfesionalOrganizacion", m_DireccionProfesionalOrganizacion, value));
					m_DireccionProfesionalOrganizacion = value;
					
				}
			}
		}
		private String m_CodigoPostal;

		//[PropiedadOriginal("CodigoPostal"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String CodigoPostal
		{
			get { return m_CodigoPostal; }
			set {
				if (this.m_CodigoPostal != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("CodigoPostal", m_CodigoPostal, value));
					m_CodigoPostal = value;
					
				}
			}
		}
		private String m_Localidad;

		//[PropiedadOriginal("Localidad"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
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
		private String m_Provincia;

		//[PropiedadOriginal("Provincia"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
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
		private String m_Pais;

		//[PropiedadOriginal("Pais"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
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
		private String m_msdid;

		//[PropiedadOriginal("msdid"   , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 15)]
		public String msdid
		{
			get { return m_msdid; }
			set {
				if (this.m_msdid != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("msdid", m_msdid, value));
					m_msdid = value;
					
				}
			}
		}
		private Nullable<Int32> m_locked;

		//[PropiedadOriginal("locked"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.Int64 , Longitud = 11)]
		public Nullable<Int32> locked
		{
			get { return m_locked; }
			set {
				if (this.m_locked != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("locked", m_locked, value));
					m_locked = value;
					
				}
			}
		}
		private Nullable<Int32> m_idtratamiento;

		//[PropiedadOriginal("idtratamiento"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.Int64 , Longitud = 11)]
		public Nullable<Int32> idtratamiento
		{
			get { return m_idtratamiento; }
			set {
				if (this.m_idtratamiento != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtratamiento", m_idtratamiento, value));
					m_idtratamiento = value;
					
				}
			}
		}
		private Nullable<Int32> m_iddistrito;

		//[PropiedadOriginal("iddistrito"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.Int64 , Longitud = 11)]
		public Nullable<Int32> iddistrito
		{
			get { return m_iddistrito; }
			set {
				if (this.m_iddistrito != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("iddistrito", m_iddistrito, value));
					m_iddistrito = value;
					
				}
			}
		}
        private String m_descuentoresidente;

        //[PropiedadOriginal("descuentoresidente", EsNullable = true, TablaOriginal = "passengers_list", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public String descuentoresidente
        {
            get { return m_descuentoresidente; }
            set
            {
                if (this.m_descuentoresidente != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("descuentoresidente", m_descuentoresidente, value));
                    m_descuentoresidente = value;

                }
            }
        }
		private Nullable<Int32> m_idespecialidad;

		//[PropiedadOriginal("idespecialidad"  ,EsNullable = true , TablaOriginal = "passengers_list" , TipoProveedor = ClepsydraDbType.Int64 , Longitud = 11)]
		public Nullable<Int32> idespecialidad
		{
			get { return m_idespecialidad; }
			set {
				if (this.m_idespecialidad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idespecialidad", m_idespecialidad, value));
					m_idespecialidad = value;
					
				}
			}
		}

        private Nullable<Int64> m_idnivelriesgo;

        //[PropiedadOriginal("idnivelriesgo", EsNullable = true, TablaOriginal = "passengers_list", TipoProveedor = ClepsydraDbType.Int64, Longitud = 11)]
        public Nullable<Int64> idnivelriesgo
        {
            get { return m_idnivelriesgo; }
            set
            {
                if (this.m_idnivelriesgo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idnivelriesgo", m_idnivelriesgo, value));
                    m_idnivelriesgo = value;

                }
            }
        }

        public int? inactivo { get; set; }
        public string GenesysCode { get; set; }
        public string GoldenId { get; set; }
        public string ProvinceCenter { get; set; }
        public string PostalCodeCenter { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CenterCode { get; set; }
        public int? internacional { get; set; }

        #endregion

        #region Constructores

        public DPassengerList()
		{
		}
		public DPassengerList(Int64 _idpassengerlist)
		{
			idpassengerlist = _idpassengerlist;
		}

		#endregion

        #region Converter

        public static ICollection<DPassengerList> ConvertToDto(DataTable dtTable)
        {
            ICollection<DPassengerList> collection = new Collection<DPassengerList>();
            foreach (DataRow row in dtTable.Rows)
            {
                DPassengerList data = new DPassengerList()
                {
                    idpassengerlist = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idpassengerlist"),
                    FechaCaducidadPasaporte = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaCaducidadPasaporte") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaCaducidadPasaporte"),
                    FechaNacimiento = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaNacimiento") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaNacimiento"),
                    iddistrito = Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddistrito"),
                    idespecialidad = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idespecialidad"),
                    idnivelriesgo = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idnivelriesgo"),
                    idtratamiento = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idtratamiento"),
                    locked = Quodem.Utility.DataLayerUtil.GetIntValue(row, "locked"),
                    apel1 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "apel1"),
                    apel2 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "apel2"),
                    CodigoPostal = Quodem.Utility.DataLayerUtil.GetStringValue(row, "CodigoPostal"),
                    descuentoresidente = Quodem.Utility.DataLayerUtil.GetStringValue(row, "descuentoresidente"),
                    DireccionCentroTrabajo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "DireccionCentroTrabajo"),
                    DireccionEmail = Quodem.Utility.DataLayerUtil.GetStringValue(row, "DireccionEmail"),
                    DireccionProfesionalOrganizacion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "DireccionProfesionalOrganizacion"),
                    Localidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Localidad"),
                    LocalidadCentroTrabajo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "LocalidadCentroTrabajo"),
                    LugarEmisionPasaporte = Quodem.Utility.DataLayerUtil.GetStringValue(row, "LugarEmisionPasaporte"),
                    msdid = Quodem.Utility.DataLayerUtil.GetStringValue(row, "msdid"),
                    NacionalidadPasaporte = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NacionalidadPasaporte"),
                    NIF = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NIF"),
                    nombre = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nombre"),
                    NombreCentroTrabajo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NombreCentroTrabajo"),
                    OrganizacionVisitaProfesional = Quodem.Utility.DataLayerUtil.GetStringValue(row, "OrganizacionVisitaProfesional"),
                    Pais = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Pais"),
                    Pasaporte = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Pasaporte"),
                    Provincia = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Provincia"),
                    TelefonoContacto = Quodem.Utility.DataLayerUtil.GetStringValue(row, "TelefonoContacto"),

                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
	}
}
