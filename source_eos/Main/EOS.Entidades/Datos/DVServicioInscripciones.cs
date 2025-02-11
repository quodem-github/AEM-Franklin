using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DVServicioInscripciones
	{
		#region Propiedades
		private Int32 m_idexpediente;

		//[PropiedadOriginal("idexpediente" ,EsClave = true  , TablaOriginal = "cv_servicioinscripciones" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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

		//[PropiedadOriginal("idreserva" ,EsClave = true  , TablaOriginal = "cv_servicioinscripciones" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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

		//[PropiedadOriginal("idservicio" ,EsClave = true  , TablaOriginal = "cv_servicioinscripciones" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private Int32 m_idservicioinscripcion;

		//[PropiedadOriginal("idservicioinscripcion" ,EsClave = true  , TablaOriginal = "cv_servicioinscripciones" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 idservicioinscripcion
		{
			get { return m_idservicioinscripcion; }
			set {
				if (this.m_idservicioinscripcion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idservicioinscripcion", m_idservicioinscripcion, value));
					m_idservicioinscripcion = value;
					
				}
			}
		}
		private String m_inscripcion;

		//[PropiedadOriginal("inscripcion"   , TablaOriginal = "cv_servicioinscripciones" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
		public String inscripcion
		{
			get { return m_inscripcion; }
			set {
				if (this.m_inscripcion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("inscripcion", m_inscripcion, value));
					m_inscripcion = value;
					
				}
			}
		}
        //Ismael Ameller 30-03-2011 
        private String m_descripcion_inscripcion;

        //[PropiedadOriginal("descripcion", TablaOriginal = "cv_servicioinscripciones", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
        public String descripcion
        {
            get { return m_descripcion_inscripcion; }
            set
            {
                if (this.m_descripcion_inscripcion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("descripcion", m_descripcion_inscripcion, value));
                    m_descripcion_inscripcion = value;

                }
            }
        }
        //FIN Ismael Ameller 30-03-2011
		private Nullable<Double> m_pvp;

		//[PropiedadOriginal("pvp"  ,EsNullable = true , TablaOriginal = "cv_servicioinscripciones" , TipoProveedor = ClepsydraDbType.BinaryDouble , Longitud = 22)]
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
		private String m_observaciones;

		//[PropiedadOriginal("observaciones"  ,EsNullable = true , TablaOriginal = "cv_servicioinscripciones" , TipoProveedor = ClepsydraDbType.NText , Longitud = 21845)]
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
		private Nullable<Int64> m_pax;

		//[PropiedadOriginal("pax"  ,EsNullable = true , TablaOriginal = "cv_servicioinscripciones" , TipoProveedor = ClepsydraDbType.Int64 , Longitud = 21)]
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

		//[PropiedadOriginal("cotizado"  ,EsNullable = true , TablaOriginal = "cv_servicioinscripciones" , TipoProveedor = ClepsydraDbType.Double , Longitud = 12)]
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

		//[PropiedadOriginal("IdEstado"   , TablaOriginal = "cv_servicioinscripciones" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
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
		#endregion

		#region Constructores

		public DVServicioInscripciones()
		{
		}
		public DVServicioInscripciones(Int32 _idexpediente ,Int32 _idreserva ,Int32 _idservicio ,Int32 _idservicioinscripcion)
		{
			idexpediente = _idexpediente;
			idreserva = _idreserva;
			idservicio = _idservicio;
			idservicioinscripcion = _idservicioinscripcion;
		}

		#endregion

        #region Converter

        public static ICollection<DVServicioInscripciones> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVServicioInscripciones> collection = new Collection<DVServicioInscripciones>();

            foreach(DataRow row in dtTable.Rows)
            {
                DVServicioInscripciones data = new DVServicioInscripciones() {
                    idexpediente = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idexpediente")),
                    idreserva = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idreserva")),
                    idservicio = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idservicio")),
                    idservicioinscripcion = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idservicioinscripcion")),
                    inscripcion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "inscripcion"),
                    descripcion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "descripcion"),
                    pvp = Quodem.Utility.DataLayerUtil.GetDoubleValue(row, "pvp", 0),
                    observaciones = Quodem.Utility.DataLayerUtil.GetStringValue(row, "observaciones"),
                    pax = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "pax")) ? new Nullable<long>() : long.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "pax"))),
                    cotizado = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "cotizado")) ? new Nullable<float>() : float.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "cotizado"))),
                    IdEstado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEstado"),
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
	}
}
