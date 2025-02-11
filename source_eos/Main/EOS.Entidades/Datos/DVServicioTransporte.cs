using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DVServicioTransporte
	{
		#region Propiedades
		private Int32 m_idexpediente;

		//[PropiedadOriginal("idexpediente" ,EsClave = true  , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idexpediente
		{
			get { return m_idexpediente; }
			set {
				if (this.m_idexpediente != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idexpediente", m_idexpediente, value));
					m_idexpediente = value;
					
				}
			}
		}
		private Int32 m_idreserva;

		//[PropiedadOriginal("idreserva" ,EsClave = true  , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idreserva
		{
			get { return m_idreserva; }
			set {
				if (this.m_idreserva != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idreserva", m_idreserva, value));
					m_idreserva = value;
					
				}
			}
		}
		private Int32 m_idservicio;

		//[PropiedadOriginal("idservicio" ,EsClave = true  , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idservicio
		{
			get { return m_idservicio; }
			set {
				if (this.m_idservicio != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idservicio", m_idservicio, value));
					m_idservicio = value;
					
				}
			}
		}
		private Int32 m_idserviciotransporte;

		//[PropiedadOriginal("idserviciotransporte" ,EsClave = true  , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idserviciotransporte
		{
			get { return m_idserviciotransporte; }
			set {
				if (this.m_idserviciotransporte != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idserviciotransporte", m_idserviciotransporte, value));
					m_idserviciotransporte = value;
					
				}
			}
		}
		private String m_ida1_transporte;

		//[PropiedadOriginal("ida1_transporte"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 3)]
		public String ida1_transporte
		{
			get { return m_ida1_transporte; }
			set {
				if (this.m_ida1_transporte != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ida1_transporte", m_ida1_transporte, value));
					m_ida1_transporte = value;
					
				}
			}
		}
		private Nullable<DateTime> m_ida1_fechasalida;

		//[PropiedadOriginal("ida1_fechasalida"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> ida1_fechasalida
		{
			get { return m_ida1_fechasalida; }
			set {
				if (this.m_ida1_fechasalida != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ida1_fechasalida", m_ida1_fechasalida, value));
					m_ida1_fechasalida = value;
					
				}
			}
		}
		private String m_ida1_numvuelo_tren;

		//[PropiedadOriginal("ida1_numvuelo_tren"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String ida1_numvuelo_tren
		{
			get { return m_ida1_numvuelo_tren; }
			set {
				if (this.m_ida1_numvuelo_tren != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ida1_numvuelo_tren", m_ida1_numvuelo_tren, value));
					m_ida1_numvuelo_tren = value;
					
				}
			}
		}
		private String m_ida1_origen;

		//[PropiedadOriginal("ida1_origen"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String ida1_origen
		{
			get { return m_ida1_origen; }
			set {
				if (this.m_ida1_origen != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ida1_origen", m_ida1_origen, value));
					m_ida1_origen = value;
					
				}
			}
		}
		private String m_ida1_destino;

		//[PropiedadOriginal("ida1_destino"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String ida1_destino
		{
			get { return m_ida1_destino; }
			set {
				if (this.m_ida1_destino != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ida1_destino", m_ida1_destino, value));
					m_ida1_destino = value;
					
				}
			}
		}
		private String m_ida1_horasalida;

		//[PropiedadOriginal("ida1_horasalida"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String ida1_horasalida
		{
			get { return m_ida1_horasalida; }
			set {
				if (this.m_ida1_horasalida != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ida1_horasalida", m_ida1_horasalida, value));
					m_ida1_horasalida = value;
					
				}
			}
		}
		private String m_ida1_horallegada;

		//[PropiedadOriginal("ida1_horallegada"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String ida1_horallegada
		{
			get { return m_ida1_horallegada; }
			set {
				if (this.m_ida1_horallegada != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ida1_horallegada", m_ida1_horallegada, value));
					m_ida1_horallegada = value;
					
				}
			}
		}
		private String m_ida2_transporte;

		//[PropiedadOriginal("ida2_transporte"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 3)]
		public String ida2_transporte
		{
			get { return m_ida2_transporte; }
			set {
				if (this.m_ida2_transporte != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ida2_transporte", m_ida2_transporte, value));
					m_ida2_transporte = value;
					
				}
			}
		}
		private Nullable<DateTime> m_ida2_fechasalida;

		//[PropiedadOriginal("ida2_fechasalida"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> ida2_fechasalida
		{
			get { return m_ida2_fechasalida; }
			set {
				if (this.m_ida2_fechasalida != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ida2_fechasalida", m_ida2_fechasalida, value));
					m_ida2_fechasalida = value;
					
				}
			}
		}
		private String m_ida2_numvuelo_tren;

		//[PropiedadOriginal("ida2_numvuelo_tren"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String ida2_numvuelo_tren
		{
			get { return m_ida2_numvuelo_tren; }
			set {
				if (this.m_ida2_numvuelo_tren != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ida2_numvuelo_tren", m_ida2_numvuelo_tren, value));
					m_ida2_numvuelo_tren = value;
					
				}
			}
		}
		private String m_ida2_origen;

		//[PropiedadOriginal("ida2_origen"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String ida2_origen
		{
			get { return m_ida2_origen; }
			set {
				if (this.m_ida2_origen != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ida2_origen", m_ida2_origen, value));
					m_ida2_origen = value;
					
				}
			}
		}
		private String m_ida2_destino;

		//[PropiedadOriginal("ida2_destino"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String ida2_destino
		{
			get { return m_ida2_destino; }
			set {
				if (this.m_ida2_destino != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ida2_destino", m_ida2_destino, value));
					m_ida2_destino = value;
					
				}
			}
		}
		private String m_ida2_horasalida;

		//[PropiedadOriginal("ida2_horasalida"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String ida2_horasalida
		{
			get { return m_ida2_horasalida; }
			set {
				if (this.m_ida2_horasalida != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ida2_horasalida", m_ida2_horasalida, value));
					m_ida2_horasalida = value;
					
				}
			}
		}
		private String m_ida2_horallegada;

		//[PropiedadOriginal("ida2_horallegada"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String ida2_horallegada
		{
			get { return m_ida2_horallegada; }
			set {
				if (this.m_ida2_horallegada != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ida2_horallegada", m_ida2_horallegada, value));
					m_ida2_horallegada = value;
					
				}
			}
		}
		private String m_reg1_transporte;

		//[PropiedadOriginal("reg1_transporte"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 3)]
		public String reg1_transporte
		{
			get { return m_reg1_transporte; }
			set {
				if (this.m_reg1_transporte != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("reg1_transporte", m_reg1_transporte, value));
					m_reg1_transporte = value;
					
				}
			}
		}
		private Nullable<DateTime> m_reg1_fechasalida;

		//[PropiedadOriginal("reg1_fechasalida"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> reg1_fechasalida
		{
			get { return m_reg1_fechasalida; }
			set {
				if (this.m_reg1_fechasalida != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("reg1_fechasalida", m_reg1_fechasalida, value));
					m_reg1_fechasalida = value;
					
				}
			}
		}
		private String m_reg1_numvuelo_tren;

		//[PropiedadOriginal("reg1_numvuelo_tren"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String reg1_numvuelo_tren
		{
			get { return m_reg1_numvuelo_tren; }
			set {
				if (this.m_reg1_numvuelo_tren != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("reg1_numvuelo_tren", m_reg1_numvuelo_tren, value));
					m_reg1_numvuelo_tren = value;
					
				}
			}
		}
		private String m_reg1_origen;

		//[PropiedadOriginal("reg1_origen"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String reg1_origen
		{
			get { return m_reg1_origen; }
			set {
				if (this.m_reg1_origen != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("reg1_origen", m_reg1_origen, value));
					m_reg1_origen = value;
					
				}
			}
		}
		private String m_reg1_destino;

		//[PropiedadOriginal("reg1_destino"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String reg1_destino
		{
			get { return m_reg1_destino; }
			set {
				if (this.m_reg1_destino != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("reg1_destino", m_reg1_destino, value));
					m_reg1_destino = value;
					
				}
			}
		}
		private String m_reg1_horasalida;

		//[PropiedadOriginal("reg1_horasalida"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String reg1_horasalida
		{
			get { return m_reg1_horasalida; }
			set {
				if (this.m_reg1_horasalida != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("reg1_horasalida", m_reg1_horasalida, value));
					m_reg1_horasalida = value;
					
				}
			}
		}
		private String m_reg1_horallegada;

		//[PropiedadOriginal("reg1_horallegada"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String reg1_horallegada
		{
			get { return m_reg1_horallegada; }
			set {
				if (this.m_reg1_horallegada != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("reg1_horallegada", m_reg1_horallegada, value));
					m_reg1_horallegada = value;
					
				}
			}
		}
		private String m_reg2_transporte;

		//[PropiedadOriginal("reg2_transporte"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 3)]
		public String reg2_transporte
		{
			get { return m_reg2_transporte; }
			set {
				if (this.m_reg2_transporte != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("reg2_transporte", m_reg2_transporte, value));
					m_reg2_transporte = value;
					
				}
			}
		}
		private Nullable<DateTime> m_reg2_fechasalida;

		//[PropiedadOriginal("reg2_fechasalida"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> reg2_fechasalida
		{
			get { return m_reg2_fechasalida; }
			set {
				if (this.m_reg2_fechasalida != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("reg2_fechasalida", m_reg2_fechasalida, value));
					m_reg2_fechasalida = value;
					
				}
			}
		}
		private String m_reg2_numvuelo_tren;

		//[PropiedadOriginal("reg2_numvuelo_tren"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String reg2_numvuelo_tren
		{
			get { return m_reg2_numvuelo_tren; }
			set {
				if (this.m_reg2_numvuelo_tren != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("reg2_numvuelo_tren", m_reg2_numvuelo_tren, value));
					m_reg2_numvuelo_tren = value;
					
				}
			}
		}
		private String m_reg2_origen;

		//[PropiedadOriginal("reg2_origen"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String reg2_origen
		{
			get { return m_reg2_origen; }
			set {
				if (this.m_reg2_origen != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("reg2_origen", m_reg2_origen, value));
					m_reg2_origen = value;
					
				}
			}
		}
		private String m_reg2_destino;

		//[PropiedadOriginal("reg2_destino"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String reg2_destino
		{
			get { return m_reg2_destino; }
			set {
				if (this.m_reg2_destino != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("reg2_destino", m_reg2_destino, value));
					m_reg2_destino = value;
					
				}
			}
		}
		private String m_reg2_horasalida;

		//[PropiedadOriginal("reg2_horasalida"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String reg2_horasalida
		{
			get { return m_reg2_horasalida; }
			set {
				if (this.m_reg2_horasalida != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("reg2_horasalida", m_reg2_horasalida, value));
					m_reg2_horasalida = value;
					
				}
			}
		}
		private String m_reg2_horallegada;

		//[PropiedadOriginal("reg2_horallegada"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String reg2_horallegada
		{
			get { return m_reg2_horallegada; }
			set {
				if (this.m_reg2_horallegada != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("reg2_horallegada", m_reg2_horallegada, value));
					m_reg2_horallegada = value;
					
				}
			}
		}
		private Nullable<Int64> m_pax;

		//[PropiedadOriginal("pax"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.Int64 , Longitud = 21)]
		public Nullable<Int64> pax
		{
			get { return m_pax; }
			set {
				if (this.m_pax != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("pax", m_pax, value));
					m_pax = value;
					
				}
			}
		}
		private Nullable<Single> m_cotizado;

		//[PropiedadOriginal("cotizado"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.Double , Longitud = 12)]
		public Nullable<Single> cotizado
		{
			get { return m_cotizado; }
			set {
				if (this.m_cotizado != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("cotizado", m_cotizado, value));
					m_cotizado = value;
					
				}
			}
		}
		private String m_IdEstado;

		//[PropiedadOriginal("IdEstado"   , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
		public String IdEstado
		{
			get { return m_IdEstado; }
			set {
				if (this.m_IdEstado != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEstado", m_IdEstado, value));
					m_IdEstado = value;
					
				}
			}
		}
		private String m_observaciones_ida;

		//[PropiedadOriginal("observaciones_ida"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
		public String observaciones_ida
		{
			get { return m_observaciones_ida; }
			set {
				if (this.m_observaciones_ida != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("observaciones_ida", m_observaciones_ida, value));
					m_observaciones_ida = value;
					
				}
			}
		}
		private String m_observaciones_reg;

		//[PropiedadOriginal("observaciones_reg"  ,EsNullable = true , TablaOriginal = "cv_serviciotransportes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
		public String observaciones_reg
		{
			get { return m_observaciones_reg; }
			set {
				if (this.m_observaciones_reg != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("observaciones_reg", m_observaciones_reg, value));
					m_observaciones_reg = value;
					
				}
			}
        }
        private String m_observaciones;

        //[PropiedadOriginal("observaciones", EsNullable = true, TablaOriginal = "cv_serviciotransportes", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public String observaciones
        {
            get { return m_observaciones; }
            set
            {
                if (this.m_observaciones != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("observaciones", m_observaciones, value));
                    m_observaciones = value;

                }
            }
        }
        
        private Nullable<Double> m_importe_max;

        //[PropiedadOriginal("importe_max", EsNullable = true, TablaOriginal = "cv_serviciotransportes", TipoProveedor = ClepsydraDbType.BinaryDouble, Longitud = 22)]
        public Nullable<Double> importe_max
        {
            get { return m_importe_max; }
            set
            {
                if (this.m_importe_max != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("importe_max", m_importe_max, value));
                    m_importe_max = value;

                }
            }
        }
		#endregion

		#region Constructores

		public DVServicioTransporte()
		{
		}
		public DVServicioTransporte(Int32 _idexpediente ,Int32 _idreserva ,Int32 _idservicio ,Int32 _idserviciotransporte)
		{
			idexpediente = _idexpediente;
			idreserva = _idreserva;
			idservicio = _idservicio;
			idserviciotransporte = _idserviciotransporte;
		}

		#endregion

        #region Converter

        public static ICollection<DVServicioTransporte> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVServicioTransporte> collection = new Collection<DVServicioTransporte>();

            foreach (DataRow row in dtTable.Rows)
            {
                DVServicioTransporte data = new DVServicioTransporte()
                {
                    idexpediente = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idexpediente")),
                    idreserva = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idreserva")),
                    idservicio = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idservicio")),
                    idserviciotransporte = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idserviciotransporte")),
                    observaciones = Quodem.Utility.DataLayerUtil.GetStringValue(row, "observaciones"),
                    pax = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "pax")) ? new Nullable<long>() : long.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "pax"))),
                    cotizado = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "cotizado")) ? new Nullable<float>() : float.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "cotizado"))),
                    IdEstado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEstado"),
                    reg2_transporte = Quodem.Utility.DataLayerUtil.GetStringValue(row, "reg2_transporte"),
                    reg2_origen = Quodem.Utility.DataLayerUtil.GetStringValue(row, "reg2_origen"),
                    reg2_numvuelo_tren = Quodem.Utility.DataLayerUtil.GetStringValue(row, "reg2_numvuelo_tren"),
                    reg2_horasalida = Quodem.Utility.DataLayerUtil.GetStringValue(row, "reg2_horasalida"),
                    reg2_horallegada = Quodem.Utility.DataLayerUtil.GetStringValue(row, "reg2_horallegada"),
                    reg2_fechasalida = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "reg2_fechasalida") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "reg2_fechasalida"),
                    reg2_destino = Quodem.Utility.DataLayerUtil.GetStringValue(row, "reg2_destino"),
                    reg1_transporte = Quodem.Utility.DataLayerUtil.GetStringValue(row, "reg1_transporte"),
                    reg1_origen = Quodem.Utility.DataLayerUtil.GetStringValue(row, "reg1_origen"),
                    reg1_numvuelo_tren = Quodem.Utility.DataLayerUtil.GetStringValue(row, "reg1_numvuelo_tren"),
                    reg1_horasalida = Quodem.Utility.DataLayerUtil.GetStringValue(row, "reg1_horasalida"),
                    reg1_horallegada = Quodem.Utility.DataLayerUtil.GetStringValue(row, "reg1_horallegada"),
                    reg1_fechasalida = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "reg1_fechasalida") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "reg1_fechasalida"),
                    reg1_destino = Quodem.Utility.DataLayerUtil.GetStringValue(row, "reg1_destino"),
                    observaciones_reg = Quodem.Utility.DataLayerUtil.GetStringValue(row, "observaciones_reg"),
                    observaciones_ida = Quodem.Utility.DataLayerUtil.GetStringValue(row, "observaciones_ida"),
                    importe_max = Quodem.Utility.DataLayerUtil.GetDoubleValue(row, "importe_max", 0),
                    ida2_transporte = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ida2_transporte"),
                    ida2_origen = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ida2_origen"),
                    ida2_numvuelo_tren = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ida2_numvuelo_tren"),
                    ida2_horasalida = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ida2_horasalida"),
                    ida2_horallegada = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ida2_horallegada"),
                    ida2_fechasalida = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "ida2_fechasalida") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "ida2_fechasalida"),
                    ida2_destino = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ida2_destino"),
                    ida1_transporte = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ida1_transporte"),
                    ida1_origen = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ida1_origen"),
                    ida1_numvuelo_tren = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ida1_numvuelo_tren"),
                    ida1_horasalida = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ida1_horasalida"),
                    ida1_horallegada = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ida1_horallegada"),
                    ida1_fechasalida = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "ida1_fechasalida") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "ida1_fechasalida"),
                    ida1_destino = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ida1_destino")
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
	}
}
