using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
	public class DExpediente
	{
		#region Propiedades
		private Int32 m_idxpediente;

		//[PropiedadOriginal("idxpediente" ,EsClave = true  , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idxpediente
		{
			get { return m_idxpediente; }
			set {
				if (this.m_idxpediente != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idxpediente", m_idxpediente, value));
					m_idxpediente = value;
					
				}
			}
		}
		private Nullable<Int32> m_idregion;

		//[PropiedadOriginal("idregion"  ,EsNullable = true , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private Int32 m_idamec;

		//[PropiedadOriginal("idamec"   , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idamec
		{
			get { return m_idamec; }
			set {
				if (this.m_idamec != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idamec", m_idamec, value));
					m_idamec = value;
					
				}
			}
		}
		private String m_expediente;

		//[PropiedadOriginal("expediente"  ,EsNullable = true , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
		public String expediente
		{
			get { return m_expediente; }
			set {
				if (this.m_expediente != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("expediente", m_expediente, value));
					m_expediente = value;
					
				}
			}
		}
		private Nullable<Int32> m_idunidad;

		//[PropiedadOriginal("idunidad"  ,EsNullable = true , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private Nullable<Int32> m_Idarea;

		//[PropiedadOriginal("Idarea"  ,EsNullable = true , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private Int32 m_IdPeticionario;

		//[PropiedadOriginal("IdPeticionario"   , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private Int32 m_IdTipoReserva;

		//[PropiedadOriginal("IdTipoReserva"   , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 IdTipoReserva
		{
			get { return m_IdTipoReserva; }
			set {
				if (this.m_IdTipoReserva != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdTipoReserva", m_IdTipoReserva, value));
					m_IdTipoReserva = value;
					
				}
			}
		}
		private Int32 m_IdEmpresa;

		//[PropiedadOriginal("IdEmpresa"   , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private String m_idestado;

		//[PropiedadOriginal("idestado"   , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
		public String idestado
		{
			get { return m_idestado; }
			set {
				if (this.m_idestado != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idestado", m_idestado, value));
					m_idestado = value;
					
				}
			}
		}
		private Nullable<Int32> m_iddistrito;

		//[PropiedadOriginal("iddistrito"  ,EsNullable = true , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private Nullable<DateTime> m_fechacreacion;

		//[PropiedadOriginal("fechacreacion"  ,EsNullable = true , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.DateTime , Longitud = 19)]
		public Nullable<DateTime> fechacreacion
		{
			get { return m_fechacreacion; }
			set {
				if (this.m_fechacreacion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fechacreacion", m_fechacreacion, value));
					m_fechacreacion = value;
					
				}
			}
		}
		private String m_idempleadogp;

		//[PropiedadOriginal("idempleadogp"  ,EsNullable = true , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
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
		private Nullable<Int32> m_locked;

		//[PropiedadOriginal("locked"  ,EsNullable = true , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private String m_codexpediente;

		//[PropiedadOriginal("codexpediente"  ,EsNullable = true , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
		public String codexpediente
		{
			get { return m_codexpediente; }
			set {
				if (this.m_codexpediente != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("codexpediente", m_codexpediente, value));
					m_codexpediente = value;
					
				}
			}
		}

		private Nullable<Int32> m_tipoPagoFee;

		//[PropiedadOriginal("codexpediente"  ,EsNullable = true , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
		public Nullable<Int32> tipoPagoFee
		{
			get { return m_tipoPagoFee; }
			set
			{
				if (this.m_tipoPagoFee != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("codexpediente", m_codexpediente, value));
					m_tipoPagoFee = value;

				}
			}
		}

		private Nullable<Boolean> m_urgente;

		//[PropiedadOriginal("urgente"  ,EsNullable = true , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.Byte , Longitud = 1)]
		public Nullable<Boolean> urgente
		{
			get { return m_urgente; }
			set {
				if (this.m_urgente != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("urgente", m_urgente, value));
					m_urgente = value;
					
				}
			}
		}

        private Nullable<Int32> m_iddepartament;

        //[PropiedadOriginal("idregion"  ,EsNullable = true , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
        public Nullable<Int32> iddepartament
        {
            get { return m_iddepartament; }
            set
            {
                if (this.m_iddepartament != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idregion", m_idregion, value));
                    m_iddepartament = value;

                }
            }
        }

        private Nullable<Int32> m_idsaleforce;

        //[PropiedadOriginal("idregion"  ,EsNullable = true , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
        public Nullable<Int32> idsaleforce
        {
            get { return m_idsaleforce; }
            set
            {
                if (this.m_idsaleforce != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idregion", m_idregion, value));
                    m_idsaleforce = value;

                }
            }
        }
        private Nullable<Int32> m_iddistrict;

        //[PropiedadOriginal("idregion"  ,EsNullable = true , TablaOriginal = "expediente" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
        public Nullable<Int32> iddistrict
        {
            get { return m_iddistrict; }
            set
            {
                if (this.m_iddistrict != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idregion", m_idregion, value));
                    m_iddistrict = value;

                }
            }
        }

        #endregion

        #region Constructores

        public DExpediente()
		{
		}
		public DExpediente(Int32 _idxpediente)
		{
			idxpediente = _idxpediente;
		}

		#endregion

        #region Converter

        public static ICollection<DExpediente> ConvertToDto(DataTable dtTable)
        {
            ICollection<DExpediente> collection = new Collection<DExpediente>();
            foreach (DataRow row in dtTable.Rows)
            {
                DExpediente data = new DExpediente()
                {
                    idamec = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idamec"),
                    IdEmpresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdEmpresa"),
                    IdPeticionario = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdPeticionario"),
                    IdTipoReserva = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdTipoReserva"),
                    idxpediente = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idxpediente"),
                    urgente = Quodem.Utility.DataLayerUtil.GetIntValue(row, "urgente") ==1,
                    fechacreacion = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacreacion") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacreacion"),
                    Idarea = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idarea"),
                    iddistrito = Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddistrito"),
                    idregion = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idregion"),
                    idunidad = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idunidad"),
                    locked = Quodem.Utility.DataLayerUtil.GetIntValue(row, "locked"),
                    codexpediente = Quodem.Utility.DataLayerUtil.GetStringValue(row, "codexpediente"),
					tipoPagoFee = Quodem.Utility.DataLayerUtil.GetStringValue(row, "tipoPagoFee") == null ? 0 : Quodem.Utility.DataLayerUtil.GetIntValue(row, "tipoPagoFee"),
					expediente = Quodem.Utility.DataLayerUtil.GetStringValue(row, "expediente"),
                    idempleadogp = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idempleadogp"),
                    idestado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idestado"),
                    iddepartament = Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddepartament"),
                    idsaleforce = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idsaleforce"),
                    iddistrict = Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddistrict"),

                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
	}
}
