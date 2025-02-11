using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DVEmpresa
	{
		#region Propiedades
		private Int32 m_IdEmpresa;

		//[PropiedadOriginal("IdEmpresa" ,EsClave = true  , TablaOriginal = "cv_empresas" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 IdEmpresa
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

		//[PropiedadOriginal("RazonSocial"   , TablaOriginal = "cv_empresas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 45)]
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
		private String m_CIF;

		//[PropiedadOriginal("CIF"   , TablaOriginal = "cv_empresas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String CIF
		{
			get { return m_CIF; }
			set {
				if (this.m_CIF != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("CIF", m_CIF, value));
					m_CIF = value;
					
				}
			}
		}
		private String m_Telefono;

		//[PropiedadOriginal("Telefono"   , TablaOriginal = "cv_empresas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 25)]
		public String Telefono
		{
			get { return m_Telefono; }
			set {
				if (this.m_Telefono != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Telefono", m_Telefono, value));
					m_Telefono = value;
					
				}
			}
		}
		private String m_Fax;

		//[PropiedadOriginal("Fax"  ,EsNullable = true , TablaOriginal = "cv_empresas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 25)]
		public String Fax
		{
			get { return m_Fax; }
			set {
				if (this.m_Fax != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Fax", m_Fax, value));
					m_Fax = value;
					
				}
			}
		}
		private String m_Email;

		//[PropiedadOriginal("Email"  ,EsNullable = true , TablaOriginal = "cv_empresas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 45)]
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
		private String m_Http;

		//[PropiedadOriginal("Http"  ,EsNullable = true , TablaOriginal = "cv_empresas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 45)]
		public String Http
		{
			get { return m_Http; }
			set {
				if (this.m_Http != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Http", m_Http, value));
					m_Http = value;
					
				}
			}
		}
		private String m_CodPostal;

		//[PropiedadOriginal("CodPostal"  ,EsNullable = true , TablaOriginal = "cv_empresas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String CodPostal
		{
			get { return m_CodPostal; }
			set {
				if (this.m_CodPostal != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("CodPostal", m_CodPostal, value));
					m_CodPostal = value;
					
				}
			}
		}
		private String m_Direccion;

		//[PropiedadOriginal("Direccion"  ,EsNullable = true , TablaOriginal = "cv_empresas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 150)]
		public String Direccion
		{
			get { return m_Direccion; }
			set {
				if (this.m_Direccion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Direccion", m_Direccion, value));
					m_Direccion = value;
					
				}
			}
		}
		private String m_Poblacion;

		//[PropiedadOriginal("Poblacion"  ,EsNullable = true , TablaOriginal = "cv_empresas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 100)]
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
		private String m_CodigoAgencia;

		//[PropiedadOriginal("CodigoAgencia"  ,EsNullable = true , TablaOriginal = "cv_empresas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 2)]
		public String CodigoAgencia
		{
			get { return m_CodigoAgencia; }
			set {
				if (this.m_CodigoAgencia != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("CodigoAgencia", m_CodigoAgencia, value));
					m_CodigoAgencia = value;
					
				}
			}
		}
		private String m_LogoInferiorPantalla;

		//[PropiedadOriginal("LogoInferiorPantalla"  ,EsNullable = true , TablaOriginal = "cv_empresas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 255)]
		public String LogoInferiorPantalla
		{
			get { return m_LogoInferiorPantalla; }
			set {
				if (this.m_LogoInferiorPantalla != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("LogoInferiorPantalla", m_LogoInferiorPantalla, value));
					m_LogoInferiorPantalla = value;
					
				}
			}
		}
		private String m_LogoSuperiorPantalla;

		//[PropiedadOriginal("LogoSuperiorPantalla"  ,EsNullable = true , TablaOriginal = "cv_empresas" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 255)]
		public String LogoSuperiorPantalla
		{
			get { return m_LogoSuperiorPantalla; }
			set {
				if (this.m_LogoSuperiorPantalla != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("LogoSuperiorPantalla", m_LogoSuperiorPantalla, value));
					m_LogoSuperiorPantalla = value;
					
				}
			}
		}
		private Boolean m_Inactivo;

		//[PropiedadOriginal("Inactivo"   , TablaOriginal = "cv_empresas" , TipoProveedor = ClepsydraDbType.Byte , Longitud = 1)]
		public Boolean Inactivo
		{
			get { return m_Inactivo; }
			set {
				if (this.m_Inactivo != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Inactivo", m_Inactivo, value));
					m_Inactivo = value;
					
				}
			}
		}
        //Ismael Ameller 28-02-2011 Cambio en la pantalla de contacto
        private String m_NombreContacto;

        //[PropiedadOriginal("NombreContacto", TablaOriginal = "cv_empresas", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public String NombreContacto
        {
            get { return m_NombreContacto; }
            set
            {
                if (this.m_NombreContacto != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("NombreContacto", m_NombreContacto, value));
                    m_NombreContacto = value;

                }
            }
        }

        private String m_NombreAgencia;

        //[PropiedadOriginal("NombreAgencia", TablaOriginal = "cv_empresas", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public String NombreAgencia
        {
            get { return m_NombreAgencia; }
            set
            {
                if (this.m_NombreAgencia != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("NombreAgencia", m_NombreAgencia, value));
                    m_NombreAgencia = value;

                }
            }
        }

        private String m_MailContacto;

        //[PropiedadOriginal("MailContacto", TablaOriginal = "cv_empresas", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public String MailContacto
        {
            get { return m_MailContacto; }
            set
            {
                if (this.m_MailContacto != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("MailContacto", m_MailContacto, value));
                    m_MailContacto = value;

                }
            }
        }

        private String m_TelefonoAgencia;

        //[PropiedadOriginal("TelefonoAgencia", TablaOriginal = "cv_empresas", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String TelefonoAgencia
        {
            get { return m_TelefonoAgencia; }
            set
            {
                if (this.m_TelefonoAgencia != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("TelefonoAgencia", m_TelefonoAgencia, value));
                    m_TelefonoAgencia = value;

                }
            }
        }
        private String m_FaxAgencia;

        //[PropiedadOriginal("FaxAgencia", TablaOriginal = "cv_empresas", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String FaxAgencia
        {
            get { return m_FaxAgencia; }
            set
            {
                if (this.m_FaxAgencia != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FaxAgencia", m_FaxAgencia, value));
                    m_FaxAgencia = value;

                }
            }
        }
        //FIN Ismael Ameller 28-02-2011 Cambio en la pantalla de contacto
		#endregion

		#region Constructores

		public DVEmpresa()
		{
		}
		public DVEmpresa(Int32 _IdEmpresa)
		{
			IdEmpresa = _IdEmpresa;
		}

		#endregion

        #region Converter

        public static IList<DVEmpresa> ConvertToDto(DataTable dtTable)
        {
            IList<DVEmpresa> collection = new Collection<DVEmpresa>();

            foreach (DataRow row in dtTable.Rows)
            {
                DVEmpresa data = new DVEmpresa()
                {
                    IdEmpresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdEmpresa"),
                    RazonSocial = Quodem.Utility.DataLayerUtil.GetStringValue(row, "RazonSocial"),
                    CIF = Quodem.Utility.DataLayerUtil.GetStringValue(row, "CIF"),
                    Telefono = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Telefono"),
                    Fax = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Fax"),
                    Email = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Email"),
                    Http = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Http"),
                    CodPostal = Quodem.Utility.DataLayerUtil.GetStringValue(row, "CodPostal"),
                    Direccion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Direccion"),
                    Poblacion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Poblacion"),
                    CodigoAgencia = Quodem.Utility.DataLayerUtil.GetStringValue(row, "CodigoAgencia"),
                    LogoInferiorPantalla = Quodem.Utility.DataLayerUtil.GetStringValue(row, "LogoInferiorPantalla"),
                    LogoSuperiorPantalla = Quodem.Utility.DataLayerUtil.GetStringValue(row, "LogoSuperiorPantalla"),
                    Inactivo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Inactivo") == "1",
                    NombreContacto = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NombreContacto"),
                    NombreAgencia = Quodem.Utility.DataLayerUtil.GetStringValue(row, "NombreAgencia"),
                    MailContacto = Quodem.Utility.DataLayerUtil.GetStringValue(row, "MailContacto"),
                    TelefonoAgencia = Quodem.Utility.DataLayerUtil.GetStringValue(row, "TelefonoAgencia"),
                    FaxAgencia = Quodem.Utility.DataLayerUtil.GetStringValue(row, "FaxAgencia")
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
	}
}
