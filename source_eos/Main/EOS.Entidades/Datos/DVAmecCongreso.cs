using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DVAmecCongreso
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
		private String m_IdEstado;

		//[PropiedadOriginal("IdEstado"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
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
		private Nullable<SByte> m_Aprobado;

		//[PropiedadOriginal("Aprobado"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.Byte , Longitud = 4)]
		public Nullable<SByte> Aprobado
		{
			get { return m_Aprobado; }
			set {
				if (this.m_Aprobado != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Aprobado", m_Aprobado, value));
					m_Aprobado = value;
					
				}
			}
		}
		private Nullable<SByte> m_AprobadoLegal;

		//[PropiedadOriginal("AprobadoLegal"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.Byte , Longitud = 4)]
		public Nullable<SByte> AprobadoLegal
		{
			get { return m_AprobadoLegal; }
			set {
				if (this.m_AprobadoLegal != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("AprobadoLegal", m_AprobadoLegal, value));
					m_AprobadoLegal = value;
					
				}
			}
		}
		private Nullable<SByte> m_AprobadoComplaice;

		//[PropiedadOriginal("AprobadoComplaice"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.Byte , Longitud = 4)]
		public Nullable<SByte> AprobadoComplaice
		{
			get { return m_AprobadoComplaice; }
			set {
				if (this.m_AprobadoComplaice != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("AprobadoComplaice", m_AprobadoComplaice, value));
					m_AprobadoComplaice = value;
					
				}
			}
		}
		private Nullable<SByte> m_AprobadoDG;

		//[PropiedadOriginal("AprobadoDG"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.Byte , Longitud = 4)]
		public Nullable<SByte> AprobadoDG
		{
			get { return m_AprobadoDG; }
			set {
				if (this.m_AprobadoDG != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("AprobadoDG", m_AprobadoDG, value));
					m_AprobadoDG = value;
					
				}
			}
		}
		private Nullable<DateTime> m_FechaAprobadolegal;

		//[PropiedadOriginal("FechaAprobadolegal"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> FechaAprobadolegal
		{
			get { return m_FechaAprobadolegal; }
			set {
				if (this.m_FechaAprobadolegal != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FechaAprobadolegal", m_FechaAprobadolegal, value));
					m_FechaAprobadolegal = value;
					
				}
			}
		}
		private Nullable<DateTime> m_FechaAprobadoComplaice;

		//[PropiedadOriginal("FechaAprobadoComplaice"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> FechaAprobadoComplaice
		{
			get { return m_FechaAprobadoComplaice; }
			set {
				if (this.m_FechaAprobadoComplaice != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FechaAprobadoComplaice", m_FechaAprobadoComplaice, value));
					m_FechaAprobadoComplaice = value;
					
				}
			}
		}
		private Nullable<DateTime> m_FechaAprobadoDG;

		//[PropiedadOriginal("FechaAprobadoDG"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> FechaAprobadoDG
		{
			get { return m_FechaAprobadoDG; }
			set {
				if (this.m_FechaAprobadoDG != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FechaAprobadoDG", m_FechaAprobadoDG, value));
					m_FechaAprobadoDG = value;
					
				}
			}
		}
		private Nullable<DateTime> m_FechaComienzoPrograma;

		//[PropiedadOriginal("FechaComienzoPrograma"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> FechaComienzoPrograma
		{
			get { return m_FechaComienzoPrograma; }
			set {
				if (this.m_FechaComienzoPrograma != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FechaComienzoPrograma", m_FechaComienzoPrograma, value));
					m_FechaComienzoPrograma = value;
					
				}
			}
		}
		private Nullable<DateTime> m_FechaAMEC;

		//[PropiedadOriginal("FechaAMEC"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> FechaAMEC
		{
			get { return m_FechaAMEC; }
			set {
				if (this.m_FechaAMEC != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FechaAMEC", m_FechaAMEC, value));
					m_FechaAMEC = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdPeticionario;

		//[PropiedadOriginal("IdPeticionario"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdPeticionario
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
		private String m_Peticionario;

		//[PropiedadOriginal("Peticionario"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 105)]
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
		private Nullable<Decimal> m_Importe;

		//[PropiedadOriginal("Importe"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.Decimal , Longitud = 11)]
		public Nullable<Decimal> Importe
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
		private Nullable<Decimal> m_ImporteRestante;

		//[PropiedadOriginal("ImporteRestante"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.Decimal , Longitud = 12)]
		public Nullable<Decimal> ImporteRestante
		{
			get { return m_ImporteRestante; }
			set {
				if (this.m_ImporteRestante != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ImporteRestante", m_ImporteRestante, value));
					m_ImporteRestante = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdArea;

		//[PropiedadOriginal("IdArea"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdArea
		{
			get { return m_IdArea; }
			set {
				if (this.m_IdArea != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdArea", m_IdArea, value));
					m_IdArea = value;
					
				}
			}
		}
		private String m_CodArea;

		//[PropiedadOriginal("CodArea"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String CodArea
		{
			get { return m_CodArea; }
			set {
				if (this.m_CodArea != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("CodArea", m_CodArea, value));
					m_CodArea = value;
					
				}
			}
		}
		private String m_Area;

		//[PropiedadOriginal("Area"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 40)]
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
		private Nullable<Int32> m_IdDistrito;

		//[PropiedadOriginal("IdDistrito"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private String m_CodDistrito;

		//[PropiedadOriginal("CodDistrito"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 10)]
		public String CodDistrito
		{
			get { return m_CodDistrito; }
			set {
				if (this.m_CodDistrito != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("CodDistrito", m_CodDistrito, value));
					m_CodDistrito = value;
					
				}
			}
		}
		private String m_Distrito;

		//[PropiedadOriginal("Distrito"  ,EsNullable = true , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
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
		#endregion

		#region Constructores

		public DVAmecCongreso()
		{
		}
	

		#endregion

        #region Converter

        public static ICollection<DVAmecCongreso> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVAmecCongreso> collection = new Collection<DVAmecCongreso>();
            foreach (DataRow row in dtTable.Rows)
            {
                DVAmecCongreso data = new DVAmecCongreso()
                {
                    IdAmec = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdAmec"),
                    FechaAMEC = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaAMEC") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaAMEC"),
                    FechaAprobadoComplaice = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaAprobadoComplaice") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaAprobadoComplaice"),
                    FechaAprobadoDG = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaAprobadoDG") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaAprobadoDG"),
                    FechaAprobadolegal = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaAprobadolegal") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaAprobadolegal"),
                    FechaComienzoPrograma = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaComienzoPrograma") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaComienzoPrograma"),
                    Importe = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Importe"),
                    ImporteRestante = Quodem.Utility.DataLayerUtil.GetIntValue(row, "ImporteRestante"),
                    IdArea = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdArea"),
                    IdCongreso = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdCongreso"),
                    IdDistrito = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdDistrito"),
                    IdPeticionario = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdPeticionario"),
                    Aprobado = String.IsNullOrWhiteSpace(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Aprobado")) ? new SByte?(): SByte.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Aprobado")),
                    AprobadoComplaice = String.IsNullOrWhiteSpace(Quodem.Utility.DataLayerUtil.GetStringValue(row, "AprobadoComplaice")) ? new SByte?() : SByte.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "AprobadoComplaice")),
                    AprobadoDG = String.IsNullOrWhiteSpace(Quodem.Utility.DataLayerUtil.GetStringValue(row, "AprobadoDG")) ? new SByte?() : SByte.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "AprobadoDG")),
                    AprobadoLegal = String.IsNullOrWhiteSpace(Quodem.Utility.DataLayerUtil.GetStringValue(row, "AprobadoLegal")) ? new SByte?() : SByte.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "AprobadoLegal")),
                    Actividad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Actividad"),
                    AMEC = Quodem.Utility.DataLayerUtil.GetStringValue(row, "AMEC"),
                    Area = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Area"),
                    CodArea = Quodem.Utility.DataLayerUtil.GetStringValue(row, "CodArea"),
                    CodDistrito = Quodem.Utility.DataLayerUtil.GetStringValue(row, "CodDistrito"),
                    Distrito = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Distrito"),
                    IdEstado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEstado"),
                    Peticionario = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Peticionario"),

                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
	}
}
