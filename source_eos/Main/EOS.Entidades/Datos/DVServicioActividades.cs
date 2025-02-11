using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DVServicioActividades
	{
		#region Propiedades
		private Int32 m_idexpediente;

		//[PropiedadOriginal("idexpediente" ,EsClave = true  , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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

		//[PropiedadOriginal("idreserva" ,EsClave = true  , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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

		//[PropiedadOriginal("idservicio" ,EsClave = true  , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private Int32 m_idservicioactividad;

		//[PropiedadOriginal("idservicioactividad" ,EsClave = true  , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idservicioactividad
		{
			get { return m_idservicioactividad; }
			set {
				if (this.m_idservicioactividad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idservicioactividad", m_idservicioactividad, value));
					m_idservicioactividad = value;
					
				}
			}
		}
		private Nullable<Int32> m_idtarifaactividad;

		//[PropiedadOriginal("idtarifaactividad"  ,EsNullable = true , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> idtarifaactividad
		{
			get { return m_idtarifaactividad; }
			set {
				if (this.m_idtarifaactividad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtarifaactividad", m_idtarifaactividad, value));
					m_idtarifaactividad = value;
					
				}
			}
		}
		private String m_observaciones;

		//[PropiedadOriginal("observaciones"  ,EsNullable = true , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
		public String observaciones
		{
			get { return m_observaciones; }
			set {
				if (this.m_observaciones != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("observaciones", m_observaciones, value));
					m_observaciones = value;
					
				}
			}
		}
		private Nullable<Double> m_pvp;

		//[PropiedadOriginal("pvp"  ,EsNullable = true , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.BinaryDouble , Longitud = 22)]
		public Nullable<Double> pvp
		{
			get { return m_pvp; }
			set {
				if (this.m_pvp != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("pvp", m_pvp, value));
					m_pvp = value;
					
				}
			}
		}
		private Nullable<Int32> m_locked;

		//[PropiedadOriginal("locked"  ,EsNullable = true , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private String m_sede;

		//[PropiedadOriginal("sede"  ,EsNullable = true , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 60)]
		public String sede
		{
			get { return m_sede; }
			set {
				if (this.m_sede != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("sede", m_sede, value));
					m_sede = value;
					
				}
			}
		}
		private String m_Descripcion;

		//[PropiedadOriginal("Descripcion"   , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 200)]
		public String Descripcion
		{
			get { return m_Descripcion; }
			set {
				if (this.m_Descripcion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Descripcion", m_Descripcion, value));
					m_Descripcion = value;
					
				}
			}
		}
		private String m_tipo;

		//[PropiedadOriginal("tipo"  ,EsNullable = true , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 45)]
		public String tipo
		{
			get { return m_tipo; }
			set {
				if (this.m_tipo != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("tipo", m_tipo, value));
					m_tipo = value;
					
				}
			}
		}
		private Nullable<DateTime> m_fechainicio;

		//[PropiedadOriginal("fechainicio"  ,EsNullable = true , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.Date , Longitud = 10)]
		public Nullable<DateTime> fechainicio
		{
			get { return m_fechainicio; }
			set {
				if (this.m_fechainicio != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fechainicio", m_fechainicio, value));
					m_fechainicio = value;
					
				}
			}
		}
		private String m_horainicio;

		//[PropiedadOriginal("horainicio"  ,EsNullable = true , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String horainicio
		{
			get { return m_horainicio; }
			set {
				if (this.m_horainicio != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("horainicio", m_horainicio, value));
					m_horainicio = value;
					
				}
			}
		}
		private String m_minutosinicio;

		//[PropiedadOriginal("minutosinicio"  ,EsNullable = true , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String minutosinicio
		{
			get { return m_minutosinicio; }
			set {
				if (this.m_minutosinicio != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("minutosinicio", m_minutosinicio, value));
					m_minutosinicio = value;
					
				}
			}
		}
		private Nullable<DateTime> m_fechafin;

		//[PropiedadOriginal("fechafin"  ,EsNullable = true , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> fechafin
		{
			get { return m_fechafin; }
			set {
				if (this.m_fechafin != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fechafin", m_fechafin, value));
					m_fechafin = value;
					
				}
			}
		}
		private String m_horafin;

		//[PropiedadOriginal("horafin"  ,EsNullable = true , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String horafin
		{
			get { return m_horafin; }
			set {
				if (this.m_horafin != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("horafin", m_horafin, value));
					m_horafin = value;
					
				}
			}
		}
		private String m_minutosfin;

		//[PropiedadOriginal("minutosfin"  ,EsNullable = true , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String minutosfin
		{
			get { return m_minutosfin; }
			set {
				if (this.m_minutosfin != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("minutosfin", m_minutosfin, value));
					m_minutosfin = value;
					
				}
			}
		}
		private String m_IdEstado;

		//[PropiedadOriginal("IdEstado"   , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
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
		private Nullable<Int64> m_pax;

		//[PropiedadOriginal("pax"  ,EsNullable = true , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.Int64 , Longitud = 21)]
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
        // Qurius (EAS) 24/01/2011
        private Nullable<Int32> m_paxvis;

        //[PropiedadOriginal("paxvis", EsNullable = true, TablaOriginal = "cv_servicioactividades", TipoProveedor = ClepsydraDbType.Int32, Longitud = 21)]
        public Nullable<Int32> paxvis
        {
            get { return m_paxvis; }
            set
            {
                if (this.m_paxvis != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("paxvis", m_paxvis, value));
                    m_paxvis = value;

                }
            }
        }
		private Nullable<Single> m_cotizado;

		//[PropiedadOriginal("cotizado"  ,EsNullable = true , TablaOriginal = "cv_servicioactividades" , TipoProveedor = ClepsydraDbType.Double , Longitud = 12)]
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
		#endregion

		#region Constructores

		public DVServicioActividades()
		{
		}
		public DVServicioActividades(Int32 _idexpediente ,Int32 _idreserva ,Int32 _idservicio ,Int32 _idservicioactividad)
		{
			idexpediente = _idexpediente;
			idreserva = _idreserva;
			idservicio = _idservicio;
			idservicioactividad = _idservicioactividad;
		}

		#endregion
        
        #region Converter

        public static ICollection<DVServicioActividades> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVServicioActividades> collection = new Collection<DVServicioActividades>();

            foreach (DataRow row in dtTable.Rows)
            {
                DVServicioActividades data = new DVServicioActividades()
                {
                    idexpediente = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idexpediente")),
                    idreserva = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idreserva")),
                    idservicio = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idservicio")),
                    idservicioactividad = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idservicioactividad")),
                    Descripcion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Descripcion"),
                    pvp = Quodem.Utility.DataLayerUtil.GetDoubleValue(row, "pvp", 0),
                    observaciones = Quodem.Utility.DataLayerUtil.GetStringValue(row, "observaciones"),
                    cotizado = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "cotizado")) ? new Nullable<float>() : float.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "cotizado"))),
                    IdEstado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEstado"),
                    minutosinicio = Quodem.Utility.DataLayerUtil.GetStringValue(row, "minutosinicio"),
                    minutosfin = Quodem.Utility.DataLayerUtil.GetStringValue(row, "minutosfin"),
                    locked = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "locked")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "locked"))),
                    horainicio = Quodem.Utility.DataLayerUtil.GetStringValue(row, "horainicio"),
                    horafin = Quodem.Utility.DataLayerUtil.GetStringValue(row, "horafin"),
                    fechainicio = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechainicio") == DateTime.MaxValue ? new Nullable<DateTime>(): Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechainicio"),
                    fechafin = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechafin") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechafin"),
                    tipo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "tipo"),
                    sede = Quodem.Utility.DataLayerUtil.GetStringValue(row, "sede"),
                    paxvis = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "paxvis")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "paxvis"))),
                    pax = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "pax")) ? new Nullable<long>() : long.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "pax"))),
                    idtarifaactividad = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idtarifaactividad")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idtarifaactividad")))
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
	}
}
