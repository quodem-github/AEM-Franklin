using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DVPeticionariosRoles
	{
		#region Propiedades
		private Int32 m_IdPeticionario;

		//[PropiedadOriginal("IdPeticionario" ,EsClave = true  , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 IdPeticionario
		{
			get { return m_IdPeticionario; }
			set {
				if (this.m_IdPeticionario != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdPeticionario", m_IdPeticionario, value));
					m_IdPeticionario = value;
					
				}
			}
		}
		private Int32 m_FKIdEmpresa;

		//[PropiedadOriginal("FKIdEmpresa"   , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 FKIdEmpresa
		{
			get { return m_FKIdEmpresa; }
			set {
				if (this.m_FKIdEmpresa != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FKIdEmpresa", m_FKIdEmpresa, value));
					m_FKIdEmpresa = value;
					
				}
			}
		}
		private String m_login;

		//[PropiedadOriginal("login"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String login
		{
			get { return m_login; }
			set {
				if (this.m_login != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("login", m_login, value));
					m_login = value;
					
				}
			}
		}
		private String m_password;

		//[PropiedadOriginal("password"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String password
		{
			get { return m_password; }
			set {
				if (this.m_password != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("password", m_password, value));
					m_password = value;
					
				}
			}
		}
		private String m_wein;

		//[PropiedadOriginal("wein"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 12)]
		public String wein
		{
			get { return m_wein; }
			set {
				if (this.m_wein != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("wein", m_wein, value));
					m_wein = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdCargo;

		//[PropiedadOriginal("IdCargo"   , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdCargo
		{
			get { return m_IdCargo; }
			set {
				if (this.m_IdCargo != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdCargo", m_IdCargo, value));
					m_IdCargo = value;
					
				}
			}
		}
		private Nullable<Int32> m_idtratamiento;

		//[PropiedadOriginal("idtratamiento"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private String m_Nombre;

		//[PropiedadOriginal("Nombre"   , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 52)]
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
		private String m_Apellido1;

		//[PropiedadOriginal("Apellido1"   , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 52)]
		public String Apellido1
		{
			get { return m_Apellido1; }
			set {
				if (this.m_Apellido1 != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Apellido1", m_Apellido1, value));
					m_Apellido1 = value;
					
				}
			}
		}
		private String m_Apellido2;

		//[PropiedadOriginal("Apellido2"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 52)]
		public String Apellido2
		{
			get { return m_Apellido2; }
			set {
				if (this.m_Apellido2 != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Apellido2", m_Apellido2, value));
					m_Apellido2 = value;
					
				}
			}
		}
		private String m_Direccion;

		//[PropiedadOriginal("Direccion"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
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
		private Nullable<Int32> m_IDPoblacion;

		//[PropiedadOriginal("IDPoblacion"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IDPoblacion
		{
			get { return m_IDPoblacion; }
			set {
				if (this.m_IDPoblacion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IDPoblacion", m_IDPoblacion, value));
					m_IDPoblacion = value;
					
				}
			}
		}
		private String m_CodPostal;

		//[PropiedadOriginal("CodPostal"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
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
		private String m_Telefono;

		//[PropiedadOriginal("Telefono"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 25)]
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
		private String m_extension;

		//[PropiedadOriginal("extension"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String extension
		{
			get { return m_extension; }
			set {
				if (this.m_extension != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("extension", m_extension, value));
					m_extension = value;
					
				}
			}
		}
		private String m_Movil;

		//[PropiedadOriginal("Movil"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 15)]
		public String Movil
		{
			get { return m_Movil; }
			set {
				if (this.m_Movil != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Movil", m_Movil, value));
					m_Movil = value;
					
				}
			}
		}
		private String m_Email;

		//[PropiedadOriginal("Email"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 45)]
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
		private Boolean m_Inactivo;

		//[PropiedadOriginal("Inactivo"   , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Byte , Longitud = 1)]
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
		private String m_Observaciones;

		//[PropiedadOriginal("Observaciones"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NText , Longitud = 21845)]
		public String Observaciones
		{
			get { return m_Observaciones; }
			set {
				if (this.m_Observaciones != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Observaciones", m_Observaciones, value));
					m_Observaciones = value;
					
				}
			}
		}
		private Nullable<DateTime> m_fechaprimeraccesoportal;

		//[PropiedadOriginal("fechaprimeraccesoportal"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> fechaprimeraccesoportal
		{
			get { return m_fechaprimeraccesoportal; }
			set {
				if (this.m_fechaprimeraccesoportal != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fechaprimeraccesoportal", m_fechaprimeraccesoportal, value));
					m_fechaprimeraccesoportal = value;
					
				}
			}
		}
		private String m_idempleadogp;

		//[PropiedadOriginal("idempleadogp"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
		public String idempleadogp
		{
			get { return m_idempleadogp; }
			set {
				if (this.m_idempleadogp != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idempleadogp", m_idempleadogp, value));
					m_idempleadogp = value;
					
				}
			}
		}
		private Nullable<Int32> m_Idarea;

		//[PropiedadOriginal("Idarea"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> Idarea
		{
			get { return m_Idarea; }
			set {
				if (this.m_Idarea != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Idarea", m_Idarea, value));
					m_Idarea = value;
					
				}
			}
		}
		private Nullable<Int32> m_idregion;

		//[PropiedadOriginal("idregion"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> idregion
		{
			get { return m_idregion; }
			set {
				if (this.m_idregion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idregion", m_idregion, value));
					m_idregion = value;
					
				}
			}
		}
		private Nullable<Int32> m_iddistrito;

		//[PropiedadOriginal("iddistrito"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private Nullable<Int32> m_locked;

		//[PropiedadOriginal("locked"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private Nullable<Boolean> m_atencionprimaria;

		//[PropiedadOriginal("atencionprimaria"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Byte , Longitud = 1)]
		public Nullable<Boolean> atencionprimaria
		{
			get { return m_atencionprimaria; }
			set {
				if (this.m_atencionprimaria != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("atencionprimaria", m_atencionprimaria, value));
					m_atencionprimaria = value;
					
				}
			}
		}
		private String m_descuentoresidente;

		//[PropiedadOriginal("descuentoresidente"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
		public String descuentoresidente
		{
			get { return m_descuentoresidente; }
			set {
				if (this.m_descuentoresidente != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("descuentoresidente", m_descuentoresidente, value));
					m_descuentoresidente = value;
					
				}
			}
		}
		private Nullable<Int32> m_idunidad;

		//[PropiedadOriginal("idunidad"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> idunidad
		{
			get { return m_idunidad; }
			set {
				if (this.m_idunidad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idunidad", m_idunidad, value));
					m_idunidad = value;
					
				}
			}
		}
		private String m_Cargo;

		//[PropiedadOriginal("Cargo"   , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String Cargo
		{
			get { return m_Cargo; }
			set {
				if (this.m_Cargo != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Cargo", m_Cargo, value));
					m_Cargo = value;
					
				}
			}
		}
		private Nullable<Int32> m_nivelestructura;

		//[PropiedadOriginal("nivelestructura"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> nivelestructura
		{
			get { return m_nivelestructura; }
			set {
				if (this.m_nivelestructura != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("nivelestructura", m_nivelestructura, value));
					m_nivelestructura = value;
					
				}
			}
		}
		private Nullable<Boolean> m_gestorreservas;

		//[PropiedadOriginal("gestorreservas"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Byte , Longitud = 1)]
		public Nullable<Boolean> gestorreservas
		{
			get { return m_gestorreservas; }
			set {
				if (this.m_gestorreservas != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("gestorreservas", m_gestorreservas, value));
					m_gestorreservas = value;
					
				}
			}
		}
		private Nullable<Boolean> m_aprobador;

		//[PropiedadOriginal("aprobador"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Byte , Longitud = 1)]
		public Nullable<Boolean> aprobador
		{
			get { return m_aprobador; }
			set {
				if (this.m_aprobador != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("aprobador", m_aprobador, value));
					m_aprobador = value;
					
				}
			}
		}
		private Int32 m_idtipoaprobador;

		//[PropiedadOriginal("idtipoaprobador"   , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idtipoaprobador
		{
			get { return m_idtipoaprobador; }
			set {
				if (this.m_idtipoaprobador != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtipoaprobador", m_idtipoaprobador, value));
					m_idtipoaprobador = value;
					
				}
			}
		}
		private Nullable<Boolean> m_comunicafi;

		//[PropiedadOriginal("comunicafi"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Byte , Longitud = 1)]
		public Nullable<Boolean> comunicafi
		{
			get { return m_comunicafi; }
			set {
				if (this.m_comunicafi != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("comunicafi", m_comunicafi, value));
					m_comunicafi = value;
					
				}
			}
		}
		private Nullable<Boolean> m_marketing;

		//[PropiedadOriginal("marketing"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Byte , Longitud = 1)]
		public Nullable<Boolean> marketing
		{
			get { return m_marketing; }
			set {
				if (this.m_marketing != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("marketing", m_marketing, value));
					m_marketing = value;
					
				}
			}
		}
		private Nullable<Boolean> m_delegado;

		//[PropiedadOriginal("delegado"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Byte , Longitud = 1)]
		public Nullable<Boolean> delegado
		{
			get { return m_delegado; }
			set {
				if (this.m_delegado != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("delegado", m_delegado, value));
					m_delegado = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdProductoempresa;

		//[PropiedadOriginal("IdProductoempresa"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdProductoempresa
		{
			get { return m_IdProductoempresa; }
			set {
				if (this.m_IdProductoempresa != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdProductoempresa", m_IdProductoempresa, value));
					m_IdProductoempresa = value;
					
				}
			}
		}

        // Qurius (EAS) 26/01/2011
        private Nullable<Boolean> m_administrador;

        //[PropiedadOriginal("administrador", EsNullable = true, TablaOriginal = "cv_peticionarios_roles", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> administrador
        {
            get { return m_administrador; }
            set
            {
                if (this.m_administrador != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("administrador", m_administrador, value));
                    m_administrador = value;

                }
            }
        }

        private int? m_iddistrict;

        //[PropiedadOriginal("IdProductoempresa"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
        public int? IdDistrict
        {
            get { return m_iddistrict; }
            set
            {
                if (this.m_iddistrict != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdProductoempresa", m_IdProductoempresa, value));
                    m_iddistrict = value;

                }
            }
        }

        private int? m_idsaleforce;

        //[PropiedadOriginal("IdProductoempresa"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
        public int? IdSaleforce
        {
            get { return m_idsaleforce; }
            set
            {
                if (this.m_idsaleforce != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdProductoempresa", m_IdProductoempresa, value));
                    m_idsaleforce = value;

                }
            }
        }

        private int? m_iddepartament;

        //[PropiedadOriginal("IdProductoempresa"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
        public int? IdDepartament
        {
            get { return m_iddepartament; }
            set
            {
                if (this.m_iddepartament != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdProductoempresa", m_IdProductoempresa, value));
                    m_iddepartament = value;

                }
            }
        }

        private int? m_idposition;

        //[PropiedadOriginal("IdProductoempresa"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
        public int? IdPosition
        {
            get { return m_idposition; }
            set
            {
                if (this.m_idposition != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdProductoempresa", m_IdProductoempresa, value));
                    m_idposition = value;

                }
            }
        }

        private String m_Position;

        //[PropiedadOriginal("Cargo"   , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
        public String Position
        {
            get { return m_Position; }
            set
            {
                if (this.m_Position != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Cargo", m_Cargo, value));
                    m_Position = value;

                }
            }
        }

        private Nullable<Boolean> m_medico;

        public Nullable<Boolean> medico
        {
            get { return m_medico; }
            set
            {
                if (this.m_medico != value)
                {
                    m_medico = value;
                }
            }
        }

        private Nullable<Boolean> m_legal;

        public Nullable<Boolean> legal
        {
            get { return m_legal; }
            set
            {
                if (this.m_legal != value)
                {
                    m_legal = value;
                }
            }
        }

        private Nullable<Boolean> m_executive;

        public Nullable<Boolean> executive
        {
            get { return m_executive; }
            set
            {
                if (this.m_executive != value)
                {
                    m_executive = value;
                }
            }
        }

        private Nullable<Boolean> m_director;

        public Nullable<Boolean> director
        {
            get { return m_director; }
            set
            {
                if (this.m_director != value)
                {
                    m_director = value;
                }
            }
        }

        private Nullable<Boolean> m_gerente;

        public Nullable<Boolean> gerente
        {
            get { return m_gerente; }
            set
            {
                if (this.m_gerente != value)
                {
                    m_gerente = value;
                }
            }
        }

        private Nullable<Boolean> m_assistant;

        public Nullable<Boolean> assitant
        {
            get { return m_assistant; }
            set
            {
                if (this.m_assistant != value)
                {
                    m_assistant = value;
                }
            }
        }

        private Nullable<Boolean> m_altocargo;

        public Nullable<Boolean> altocargo
        {
            get { return m_altocargo; }
            set
            {
                if (this.m_altocargo != value)
                {
                    m_altocargo = value;
                }
            }
        }

        private Nullable<Boolean> m_externalUser;

        public Nullable<Boolean> externalUser
        {
            get { return m_externalUser; }
            set
            {
                if (this.m_externalUser != value)
                {
                    m_externalUser = value;

                }
            }
        }

        private Nullable<Int32> m_IdConfEmpresa;

        //[PropiedadOriginal("IDPoblacion"  ,EsNullable = true , TablaOriginal = "cv_peticionarios_roles" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
        public Nullable<Int32> IdConfEmpresa
        {
            get { return m_IdConfEmpresa; }
            set
            {
                if (this.m_IdConfEmpresa != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IDPoblacion", m_IDPoblacion, value));
                    m_IdConfEmpresa = value;

                }
            }
        }

        private Nullable<Boolean> m_newco;

        //[PropiedadOriginal("administrador", EsNullable = true, TablaOriginal = "cv_peticionarios_roles", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> newco
        {
            get { return m_newco; }
            set
            {
                if (this.m_newco != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("administrador", m_administrador, value));
                    m_newco = value;

                }
            }
        }

        #endregion

        #region Constructores

        public DVPeticionariosRoles()
		{
		}
		public DVPeticionariosRoles(Int32 _IdPeticionario)
		{
			IdPeticionario = _IdPeticionario;
		}

		#endregion

        #region Converter

        public static ICollection<DVPeticionariosRoles> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVPeticionariosRoles> collection = new Collection<DVPeticionariosRoles>();

            foreach (DataRow row in dtTable.Rows)
            {
                DVPeticionariosRoles data = new DVPeticionariosRoles()
                {
                    IdPeticionario = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdPeticionario"),
                    FKIdEmpresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "FKIdEmpresa"),
                    login = Quodem.Utility.DataLayerUtil.GetStringValue(row, "login"),
                    password = Quodem.Utility.DataLayerUtil.GetStringValue(row, "password"),
                    wein = Quodem.Utility.DataLayerUtil.GetStringValue(row, "wein"),
                    IdCargo = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdCargo") > 0 ? Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdCargo") : new int?(),
                    idtratamiento = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idtratamiento"),
                    Nombre = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Nombre"),
                    Apellido1 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Apellido1"),
                    Apellido2 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Apellido2"),
                    Direccion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Direccion"),
                    IDPoblacion = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IDPoblacion") > 0 ? Quodem.Utility.DataLayerUtil.GetIntValue(row, "IDPoblacion"): new int?(),
                    CodPostal = Quodem.Utility.DataLayerUtil.GetStringValue(row, "CodPostal"),
                    Telefono = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Telefono"),
                    extension = Quodem.Utility.DataLayerUtil.GetStringValue(row, "extension"),
                    Movil = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Movil"),
                    Email = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Email"),
                    Inactivo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Inactivo") == "1",
                    Observaciones = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Observaciones"),
                    fechaprimeraccesoportal = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaprimeraccesoportal") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaprimeraccesoportal"),
                    idempleadogp = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idempleadogp"),
                    Idarea = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idarea") > 0 ? Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idarea") : new int?(),
                    idregion = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idregion") > 0 ? Quodem.Utility.DataLayerUtil.GetIntValue(row, "idregion") : new int?(),
                    iddistrito = Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddistrito") > 0 ? Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddistrito") : new int?(),
                    locked = Quodem.Utility.DataLayerUtil.GetIntValue(row, "locked"),
                    atencionprimaria = Quodem.Utility.DataLayerUtil.GetStringValue(row, "atencionprimaria") == "1",
                    descuentoresidente = Quodem.Utility.DataLayerUtil.GetStringValue(row, "descuentoresidente"),
                    idunidad = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idunidad") > 0 ? Quodem.Utility.DataLayerUtil.GetIntValue(row, "idunidad") : new int?(),
                    Cargo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Cargo"),
                    nivelestructura = Quodem.Utility.DataLayerUtil.GetIntValue(row, "nivelestructura") > 0 ? Quodem.Utility.DataLayerUtil.GetIntValue(row, "nivelestructura") : new int?(),
                    gestorreservas = Quodem.Utility.DataLayerUtil.GetStringValue(row, "gestorreservas") == "1",
                    aprobador = Quodem.Utility.DataLayerUtil.GetStringValue(row, "aprobador") == "1",
                    idtipoaprobador = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idtipoaprobador"),
                    comunicafi = Quodem.Utility.DataLayerUtil.GetStringValue(row, "comunicafi") == "1",
                    marketing = Quodem.Utility.DataLayerUtil.GetStringValue(row, "marketing") == "1",
                    delegado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "delegado") == "1",
                    IdProductoempresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdProductoempresa") > 0 ? Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdProductoempresa") : new int?(),
                    administrador = Quodem.Utility.DataLayerUtil.GetStringValue(row, "administrador") == "1",
                    medico = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "medico") != null ? Quodem.Utility.DataLayerUtil.GetBoolValue(row, "medico") : false,
                    legal = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "legal") != null ? Quodem.Utility.DataLayerUtil.GetBoolValue(row, "legal") : false,
                    executive = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "executive") != null ? Quodem.Utility.DataLayerUtil.GetBoolValue(row, "executive") : false,
                    director = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "director") != null ? Quodem.Utility.DataLayerUtil.GetBoolValue(row, "director") : false,
                    gerente = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "gerente") != null ? Quodem.Utility.DataLayerUtil.GetBoolValue(row, "gerente") : false,
                    assitant = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "assistant") != null ? Quodem.Utility.DataLayerUtil.GetBoolValue(row, "assistant") : false,
                    altocargo = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "altocargo") != null ? Quodem.Utility.DataLayerUtil.GetBoolValue(row, "altocargo") : false,
                    externalUser = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "externalUser") != null ? Quodem.Utility.DataLayerUtil.GetBoolValue(row, "externalUser") : false,
                    IdDepartament = Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddepartament") > 0 ? Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddepartament") : new int?(),
                    IdDistrict =  Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddistrict") > 0 ? Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddistrict") : new int?(),
                    IdSaleforce  = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idsaleforce") > 0 ? Quodem.Utility.DataLayerUtil.GetIntValue(row, "idsaleforce") : new int?(),
                    IdConfEmpresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idconfempresa") > 0 ? Quodem.Utility.DataLayerUtil.GetIntValue(row, "idconfempresa") : new int?(),
                    IdPosition = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idposition") > 0 ? Quodem.Utility.DataLayerUtil.GetIntValue(row, "idposition") : new int?(),
                    Position = Quodem.Utility.DataLayerUtil.GetStringValue(row, "position"),
                    newco = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "newco") != null ? Quodem.Utility.DataLayerUtil.GetBoolValue(row, "newco") : false,
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
	}
}
