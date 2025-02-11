using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
	public class DCabeceraExpedienteAmpliado
	{
		#region Propiedades
		private Int32 m_Idexpediente;

		//[PropiedadOriginal("IDEXPEDIENTE" ,EsClave = true  , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 Idexpediente
		{
			get { return m_Idexpediente; }
			set {
				if (this.m_Idexpediente != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Idexpediente", m_Idexpediente, value));
					m_Idexpediente = value;
					
				}
			}
		}
        //No cambiar tipo
		private Int32 m_Idamec;

		//[PropiedadOriginal("IDAMEC" ,EsClave = true  , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 Idamec
		{
			get { return m_Idamec; }
			set {
				if (this.m_Idamec != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Idamec", m_Idamec, value));
					m_Idamec = value;
					
				}
			}
		}
		private String m_Amec;

		//[PropiedadOriginal("AMEC"   , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 20)]
		public String Amec
		{
			get { return m_Amec; }
			set {
				if (this.m_Amec != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Amec", m_Amec, value));
					m_Amec = value;
					
				}
			}
		}
		private Nullable<DateTime> m_Fechacreacion;

		//[PropiedadOriginal("FECHACREACION"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> Fechacreacion
		{
			get { return m_Fechacreacion; }
			set {
				if (this.m_Fechacreacion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Fechacreacion", m_Fechacreacion, value));
					m_Fechacreacion = value;
					
				}
			}
		}
		private Nullable<Int32> m_Idactividad;

		//[PropiedadOriginal("IDACTIVIDAD" ,EsClave = true ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.Int64 , Longitud = 11)]
		public Nullable<Int32> Idactividad
		{
			get { return m_Idactividad; }
			set {
				if (this.m_Idactividad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Idactividad", m_Idactividad, value));
					m_Idactividad = value;
					
				}
			}
		}
		private String m_Actividad;

		//[PropiedadOriginal("ACTIVIDAD"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 70)]
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
		private Nullable<Int32> m_Idvaloracionfi;

		//[PropiedadOriginal("IDVALORACIONFI"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.Int64 , Longitud = 11)]
		public Nullable<Int32> Idvaloracionfi
		{
			get { return m_Idvaloracionfi; }
			set {
				if (this.m_Idvaloracionfi != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Idvaloracionfi", m_Idvaloracionfi, value));
					m_Idvaloracionfi = value;
					
				}
			}
		}
		private String m_Unidad;

		//[PropiedadOriginal("UNIDAD"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
		public String Unidad
		{
			get { return m_Unidad; }
			set {
				if (this.m_Unidad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Unidad", m_Unidad, value));
					m_Unidad = value;
					
				}
			}
		}
		private String m_Area;

		//[PropiedadOriginal("AREA"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 40)]
		public String Area
		{
			get { return m_Area; }
			set {
				if (this.m_Area != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Area", m_Area, value));
					m_Area = value;
					
				}
			}
		}
		private String m_Region;

		//[PropiedadOriginal("REGION"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 25)]
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
		private String m_Distrito;

		//[PropiedadOriginal("DISTRITO"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
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
		private String m_Peticionario;

		//[PropiedadOriginal("PETICIONARIO"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 105)]
		public String Peticionario
		{
			get { return m_Peticionario; }
			set {
				if (this.m_Peticionario != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Peticionario", m_Peticionario, value));
					m_Peticionario = value;
					
				}
			}
		}
		private String m_Cargo;

		//[PropiedadOriginal("CARGO"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
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
		private String m_Idestado;

		//[PropiedadOriginal("IDESTADO"   , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
		public String Idestado
		{
			get { return m_Idestado; }
			set {
				if (this.m_Idestado != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Idestado", m_Idestado, value));
					m_Idestado = value;
					
				}
			}
		}
		private String m_Tiporeserva;

		//[PropiedadOriginal("TIPORESERVA"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
		public String Tiporeserva
		{
			get { return m_Tiporeserva; }
			set {
				if (this.m_Tiporeserva != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Tiporeserva", m_Tiporeserva, value));
					m_Tiporeserva = value;
					
				}
			}
		}
		private Int32 m_Idtiporeserva;

		//[PropiedadOriginal("IDTIPORESERVA"   , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 Idtiporeserva
		{
			get { return m_Idtiporeserva; }
			set {
				if (this.m_Idtiporeserva != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Idtiporeserva", m_Idtiporeserva, value));
					m_Idtiporeserva = value;
					
				}
			}
		}
		private String m_Pedido;

		//[PropiedadOriginal("PEDIDO"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
		public String Pedido
		{
			get { return m_Pedido; }
			set {
				if (this.m_Pedido != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Pedido", m_Pedido, value));
					m_Pedido = value;
					
				}
			}
		}
		//añadido funcion mostrar datos Fecha -Marco 21/02/2011

		private Nullable<Int32> m_TipoPagoFee;

		//[PropiedadOriginal("PEDIDO"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
		public Nullable<Int32> TipoPagoFee
		{
			get { return m_TipoPagoFee; }
			set
			{
				if (this.m_TipoPagoFee != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Pedido", m_Pedido, value));
					m_TipoPagoFee = value;

				}
			}
		}


		private  Nullable<DateTime> m_FechaDesde;

        //[PropiedadOriginal("DESDE", EsNullable = true, TablaOriginal = "cv_expedientes_total_fa", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public Nullable<DateTime> FechaDesde
        {
            get { return m_FechaDesde; }
            set
            {
                if (this.m_FechaDesde != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Desde",m_FechaDesde , value));
                    m_FechaDesde = value;

                }
            }
        }

        //añadido funcion mostrar datos Fecha hasta -Marco 21/02/2011 
       private Nullable<DateTime> m_FechaHasta;

        //[PropiedadOriginal("HASTA", EsNullable = true, TablaOriginal = "cv_expedientes_total_fa", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public Nullable<DateTime> FechaHasta
        {
            get { return m_FechaHasta; }
            set
            {
                if (this.m_FechaHasta != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Hasta", m_FechaHasta, value));
                    m_FechaHasta = value;

                }
            }
        }

          //añadido funcion mostrar datos poblacion Fecha -Marco 21/02/2011
        
        private String m_Poblacion;

        //[PropiedadOriginal("POBLACION", EsNullable = true, TablaOriginal = "cv_expedientes_total_fa", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public String Poblacion
        {
            get { return m_Poblacion; }
            set
            {
                if (this.m_Poblacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Poblacion", m_Poblacion, value));
                    m_Poblacion = value;

                }
            }
        }
        
        private Nullable<Boolean> m_Urgente;

		//[PropiedadOriginal("URGENTE"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.Byte , Longitud = 1)]
		public Nullable<Boolean> Urgente
		{
			get { return m_Urgente; }
			set {
				if (this.m_Urgente != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Urgente", m_Urgente, value));
					m_Urgente = value;
					
				}
			}
		}
		private Nullable<Double> m_Importe;

		//[PropiedadOriginal("IMPORTE"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.BinaryDouble , Longitud = 23)]
		public Nullable<Double> Importe
		{
			get { return m_Importe; }
			set {
				if (this.m_Importe != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Importe", m_Importe, value));
					m_Importe = value;
					
				}
			}
		}

        private Nullable<int> m_idconfempresa;

        //[PropiedadOriginal("IMPORTE"  ,EsNullable = true , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.BinaryDouble , Longitud = 23)]
        public Nullable<int> idconfempresa
        {
            get { return m_idconfempresa; }
            set
            {
                if (this.m_idconfempresa != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Importe", m_Importe, value));
                    m_idconfempresa = value;

                }
            }
        }

        private Int32 m_idDistrict;

        //[PropiedadOriginal("IDTIPORESERVA"   , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
        public Int32 iddistrict
        {
            get { return m_idDistrict; }
            set
            {
                if (this.m_idDistrict != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Idtiporeserva", m_Idtiporeserva, value));
                    m_idDistrict = value;

                }
            }
        }

        private string m_district;

        public string district
        {
            get { return m_district; }
            set
            {
                if (this.m_district != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Unidad", m_Unidad, value));
                    m_district = value;

                }
            }
        }

        private Int32 m_IdDepartament;

        //[PropiedadOriginal("IDTIPORESERVA"   , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
        public Int32 iddepartament
        {
            get { return m_IdDepartament; }
            set
            {
                if (this.m_IdDepartament != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Idtiporeserva", m_Idtiporeserva, value));
                    m_IdDepartament = value;

                }
            }
        }

        private string m_departament;

        public string departament
        {
            get { return m_departament; }
            set
            {
                if (this.m_departament != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Unidad", m_Unidad, value));
                    m_departament = value;

                }
            }
        }


        private Int32 m_IdSaleForce;

        //[PropiedadOriginal("IDTIPORESERVA"   , TablaOriginal = "cv_expedientes_total_fa" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
        public Int32 idsaleforce
        {
            get { return m_IdSaleForce; }
            set
            {
                if (this.m_IdSaleForce != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Idtiporeserva", m_Idtiporeserva, value));
                    m_IdSaleForce = value;

                }
            }
        }

        private string m_saleforce;

        public string saleforce
        {
            get { return m_saleforce; }
            set
            {
                if (this.m_saleforce != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Unidad", m_Unidad, value));
                    m_saleforce = value;

                }
            }
        }


        private String m_TipoActividad;

	    public String tipoactividad
	    {
	        get { return m_TipoActividad; }
	        set
	        {
	            if (this.m_TipoActividad != value)
	            {
	                //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Unidad", m_Unidad, value));
	                m_TipoActividad = value;

	            }
	        }
	    }

	    public virtual long __hibernate_sort_row
        {
            set;
            get;
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


        #endregion

        #region Constructores

        public DCabeceraExpedienteAmpliado()
		{
		}
		public DCabeceraExpedienteAmpliado(Int32 _Idexpediente ,Int32 _Idamec ,Int32 _Idactividad)
		{
			Idexpediente = _Idexpediente;
			Idamec = _Idamec;
			Idactividad = _Idactividad;
		}

		#endregion

        #region Converter

        public static ICollection<DCabeceraExpedienteAmpliado> ConvertToDto(DataTable dtTable)
        {
            ICollection<DCabeceraExpedienteAmpliado> collection = new Collection<DCabeceraExpedienteAmpliado>();

            foreach(DataRow row in dtTable.Rows)
            {
                DCabeceraExpedienteAmpliado data = new DCabeceraExpedienteAmpliado()
                {
                    Idexpediente = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IDEXPEDIENTE"),
                    Idamec = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IDAMEC"),
                    Amec = Quodem.Utility.DataLayerUtil.GetStringValue(row, "AMEC"),
                    Fechacreacion = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FECHACREACION") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FECHACREACION"),
                    Idactividad = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IDACTIVIDAD"),
                    Actividad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ACTIVIDAD"),                    
                    Unidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "UNIDAD"),
                    Area = Quodem.Utility.DataLayerUtil.GetStringValue(row, "AREA"),
                    Region = Quodem.Utility.DataLayerUtil.GetStringValue(row, "REGION"),
                    Distrito = Quodem.Utility.DataLayerUtil.GetStringValue(row, "DISTRITO"),
                    Peticionario = Quodem.Utility.DataLayerUtil.GetStringValue(row, "PETICIONARIO"),
                    Cargo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "CARGO"),
                    Idestado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IDESTADO"),
                    Tiporeserva = Quodem.Utility.DataLayerUtil.GetStringValue(row, "TIPORESERVA"),
                    Idtiporeserva = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IDTIPORESERVA"),
                    iddistrict = Quodem.Utility.DataLayerUtil.GetStringValue(row, "iddistrict") == null ? 0 : Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddistrict") ,
                    iddepartament = Quodem.Utility.DataLayerUtil.GetStringValue(row, "iddepartament") == null ? 0 : Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddepartament"),
                    idsaleforce = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idsaleforce") == null ? 0 : Quodem.Utility.DataLayerUtil.GetIntValue(row, "idsaleforce"),
                    Pedido = Quodem.Utility.DataLayerUtil.GetStringValue(row, "PEDIDO"),
					TipoPagoFee = Quodem.Utility.DataLayerUtil.GetStringValue(row, "tipoPagoFee") == null ? 0 : Quodem.Utility.DataLayerUtil.GetIntValue(row, "tipoPagoFee"),
					FechaDesde = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "DESDE") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "DESDE"),
                    FechaHasta = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "HASTA") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "HASTA"),
                    Poblacion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "POBLACION"),
                    Urgente = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "URGENTE"),
                    Importe = Quodem.Utility.DataLayerUtil.GetDoubleValue(row, "IMPORTE", 0),
                    Idvaloracionfi = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IDVALORACIONFI")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IDVALORACIONFI"))),
                    idconfempresa = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idconfempresa")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idconfempresa"))),
                    tipoactividad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "tipoactividad"),
                    departament = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "departament")) ? "" : Quodem.Utility.DataLayerUtil.GetStringValue(row, "departament")),
                    district = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "district")) ? "" : Quodem.Utility.DataLayerUtil.GetStringValue(row, "district")),
                    saleforce = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "saleforce")) ? "" : Quodem.Utility.DataLayerUtil.GetStringValue(row, "saleforce")),
                    IdConfEmpresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdConfEmpresa")
                };

                collection.Add(data);
            }

            return collection;
        }


        /*
         
        HAY QUE INCLUIR LOS CAMPOS DE ID DISTRICTO, DEPARTAMENTO Y FUERZA DE VENTAS
         
         */

        #endregion
	}
}
