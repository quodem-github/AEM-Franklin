using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DEmpresaConf
	{
		#region Propiedades
		private Int32 m_idconfempresa;

		//[PropiedadOriginal("idconfempresa" ,EsClave = true  , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idconfempresa
		{
			get { return m_idconfempresa; }
			set {
				if (this.m_idconfempresa != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idconfempresa", m_idconfempresa, value));
					m_idconfempresa = value;
					
				}
			}
		}
		private String m_codpostal;

		//[PropiedadOriginal("codpostal"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String codpostal
		{
			get { return m_codpostal; }
			set {
				if (this.m_codpostal != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("codpostal", m_codpostal, value));
					m_codpostal = value;
					
				}
			}
		}
		private String m_direccion;

		//[PropiedadOriginal("direccion"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 150)]
		public String direccion
		{
			get { return m_direccion; }
			set {
				if (this.m_direccion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("direccion", m_direccion, value));
					m_direccion = value;
					
				}
			}
		}
		private String m_linkhorariosservicios;

		//[PropiedadOriginal("linkhorariosservicios"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String linkhorariosservicios
		{
			get { return m_linkhorariosservicios; }
			set {
				if (this.m_linkhorariosservicios != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("linkhorariosservicios", m_linkhorariosservicios, value));
					m_linkhorariosservicios = value;
					
				}
			}
		}
		private String m_logoinferiorpantalla;

		//[PropiedadOriginal("logoinferiorpantalla"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 255)]
		public String logoinferiorpantalla
		{
			get { return m_logoinferiorpantalla; }
			set {
				if (this.m_logoinferiorpantalla != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("logoinferiorpantalla", m_logoinferiorpantalla, value));
					m_logoinferiorpantalla = value;
					
				}
			}
		}
		private Nullable<Int32> m_locked;

		//[PropiedadOriginal("locked"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private String m_mailtorespuesta;

		//[PropiedadOriginal("mailtorespuesta"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 255)]
		public String mailtorespuesta
		{
			get { return m_mailtorespuesta; }
			set {
				if (this.m_mailtorespuesta != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("mailtorespuesta", m_mailtorespuesta, value));
					m_mailtorespuesta = value;
					
				}
			}
		}
		private String m_formatoprgmailcomunicfi;

		//[PropiedadOriginal("formatoprgmailcomunicfi"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 255)]
		public String formatoprgmailcomunicfi
		{
			get { return m_formatoprgmailcomunicfi; }
			set {
				if (this.m_formatoprgmailcomunicfi != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("formatoprgmailcomunicfi", m_formatoprgmailcomunicfi, value));
					m_formatoprgmailcomunicfi = value;
					
				}
			}
		}
		private String m_cuerpomailcomunicfi;

		//[PropiedadOriginal("cuerpomailcomunicfi"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NText , Longitud = 21845)]
		public String cuerpomailcomunicfi
		{
			get { return m_cuerpomailcomunicfi; }
			set {
				if (this.m_cuerpomailcomunicfi != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("cuerpomailcomunicfi", m_cuerpomailcomunicfi, value));
					m_cuerpomailcomunicfi = value;
					
				}
			}
		}

        private String m_nombreagencia;

        //[PropiedadOriginal("nombreagencia", EsNullable = true, TablaOriginal = "dbo_iw_confempresa", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public String nombreagencia
        {
            get { return m_nombreagencia; }
            set
            {
                if (this.m_nombreagencia != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("nombreagencia", m_nombreagencia, value));
                    m_nombreagencia = value;

                }
            }
        }
        private String m_asuntomailcomunicfi;

		//[PropiedadOriginal("asuntomailcomunicfi"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 255)]
		public String asuntomailcomunicfi
		{
			get { return m_asuntomailcomunicfi; }
			set {
				if (this.m_asuntomailcomunicfi != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("asuntomailcomunicfi", m_asuntomailcomunicfi, value));
					m_asuntomailcomunicfi = value;
					
				}
			}
		}
		private String m_mailcccomunicfi;

		//[PropiedadOriginal("mailcccomunicfi"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 255)]
		public String mailcccomunicfi
		{
			get { return m_mailcccomunicfi; }
			set {
				if (this.m_mailcccomunicfi != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("mailcccomunicfi", m_mailcccomunicfi, value));
					m_mailcccomunicfi = value;
					
				}
			}
		}
		private String m_mailtocomunicfi;

		//[PropiedadOriginal("mailtocomunicfi"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 255)]
		public String mailtocomunicfi
		{
			get { return m_mailtocomunicfi; }
			set {
				if (this.m_mailtocomunicfi != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("mailtocomunicfi", m_mailtocomunicfi, value));
					m_mailtocomunicfi = value;
					
				}
			}
		}
		private String m_mailfromcomunicfi;

		//[PropiedadOriginal("mailfromcomunicfi"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 255)]
		public String mailfromcomunicfi
		{
			get { return m_mailfromcomunicfi; }
			set {
				if (this.m_mailfromcomunicfi != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("mailfromcomunicfi", m_mailfromcomunicfi, value));
					m_mailfromcomunicfi = value;
					
				}
			}
		}
		private String m_gestordearchivo;

		//[PropiedadOriginal("gestordearchivo"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 100)]
		public String gestordearchivo
		{
			get { return m_gestordearchivo; }
			set {
				if (this.m_gestordearchivo != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("gestordearchivo", m_gestordearchivo, value));
					m_gestordearchivo = value;
					
				}
			}
		}
		private String m_poblacion;

		//[PropiedadOriginal("poblacion"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 100)]
		public String poblacion
		{
			get { return m_poblacion; }
			set {
				if (this.m_poblacion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("poblacion", m_poblacion, value));
					m_poblacion = value;
					
				}
			}
		}
		private Int32 m_IdEmpresa;

		//[PropiedadOriginal("IdEmpresa"   , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private String m_logosuperiorpantalla;

		//[PropiedadOriginal("logosuperiorpantalla"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 255)]
		public String logosuperiorpantalla
		{
			get { return m_logosuperiorpantalla; }
			set {
				if (this.m_logosuperiorpantalla != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("logosuperiorpantalla", m_logosuperiorpantalla, value));
					m_logosuperiorpantalla = value;
					
				}
			}
		}
		private String m_codigo_agencia;

		//[PropiedadOriginal("codigo_agencia"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 2)]
		public String codigo_agencia
		{
			get { return m_codigo_agencia; }
			set {
				if (this.m_codigo_agencia != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("codigo_agencia", m_codigo_agencia, value));
					m_codigo_agencia = value;
					
				}
			}
		}

        private String m_ruta_formulario_grupos;

        //[PropiedadOriginal("codigo_agencia"  ,EsNullable = true , TablaOriginal = "confempresa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 2)]
        public String ruta_formulario_grupos
        {
            get { return m_ruta_formulario_grupos; }
            set
            {
                if (this.m_ruta_formulario_grupos != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("codigo_agencia", m_codigo_agencia, value));
                    m_ruta_formulario_grupos = value;

                }
            }
        }

        #endregion

        #region Constructores

        public DEmpresaConf()
		{
		}
		public DEmpresaConf(Int32 _idconfempresa)
		{
			idconfempresa = _idconfempresa;
		}

		#endregion

        #region Converter

        public static IList<DEmpresaConf> ConvertToDto(DataTable dtTable)
        {
            IList<DEmpresaConf> collection = new Collection<DEmpresaConf>();
            foreach (DataRow row in dtTable.Rows)
            {
                DEmpresaConf data = new DEmpresaConf()
                {
                    idconfempresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idconfempresa"),
                    codpostal = Quodem.Utility.DataLayerUtil.GetStringValue(row, "codpostal"),
                    direccion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "direccion"),
                    linkhorariosservicios = Quodem.Utility.DataLayerUtil.GetStringValue(row, "linkhorariosservicios"),
                    logoinferiorpantalla = Quodem.Utility.DataLayerUtil.GetStringValue(row, "logoinferiorpantalla"),
                    locked = Quodem.Utility.DataLayerUtil.GetIntValue(row, "locked"),
                    mailtorespuesta = Quodem.Utility.DataLayerUtil.GetStringValue(row, "mailtorespuesta"),
                    formatoprgmailcomunicfi = Quodem.Utility.DataLayerUtil.GetStringValue(row, "formatoprgmailcomunicfi"),
                    cuerpomailcomunicfi = Quodem.Utility.DataLayerUtil.GetStringValue(row, "cuerpomailcomunicfi"),
                    nombreagencia = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nombreagencia"),
                    asuntomailcomunicfi = Quodem.Utility.DataLayerUtil.GetStringValue(row, "asuntomailcomunicfi"),
                    mailtocomunicfi = Quodem.Utility.DataLayerUtil.GetStringValue(row, "mailtocomunicfi"),
                    mailcccomunicfi = Quodem.Utility.DataLayerUtil.GetStringValue(row, "mailcccomunicfi"),
                    mailfromcomunicfi = Quodem.Utility.DataLayerUtil.GetStringValue(row, "mailfromcomunicfi"),
                    gestordearchivo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "gestordearchivo"),
                    poblacion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "poblacion"),
                    logosuperiorpantalla = Quodem.Utility.DataLayerUtil.GetStringValue(row, "logosuperiorpantalla"),
                    codigo_agencia = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idagencia"),
                    IdEmpresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdEmpresa"),
                    ruta_formulario_grupos = Quodem.Utility.DataLayerUtil.GetStringValue(row, "RutaFormularioGrupos")
                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
	}
}
