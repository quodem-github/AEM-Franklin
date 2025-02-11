using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DVServicioPassengerResumen
	{
		#region Propiedades
		private Int32 m_IdPassengerList;

		//[PropiedadOriginal("IdPassengerList"   , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 IdPassengerList
		{
			get { return m_IdPassengerList; }
			set {
				if (this.m_IdPassengerList != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdPassengerList", m_IdPassengerList, value));
					m_IdPassengerList = value;
					
				}
			}
		}
		private Int32 m_IdExpediente;

		//[PropiedadOriginal("IdExpediente"   , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 IdExpediente
		{
			get { return m_IdExpediente; }
			set {
				if (this.m_IdExpediente != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdExpediente", m_IdExpediente, value));
					m_IdExpediente = value;
					
				}
			}
		}
		private String m_Nombre;

		//[PropiedadOriginal("Nombre"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
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
		private String m_Apel1;

		//[PropiedadOriginal("Apel1"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String Apel1
		{
			get { return m_Apel1; }
			set {
				if (this.m_Apel1 != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Apel1", m_Apel1, value));
					m_Apel1 = value;
					
				}
			}
		}
		private String m_Apel2;

		//[PropiedadOriginal("Apel2"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String Apel2
		{
			get { return m_Apel2; }
			set {
				if (this.m_Apel2 != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Apel2", m_Apel2, value));
					m_Apel2 = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdServicioINS;

        //[PropiedadOriginal("IdServicioINS", EsNullable = true, TablaOriginal = "cv_ser_passenger_resumen", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
		public Nullable<Int32> IdServicioINS
		{
			get { return m_IdServicioINS; }
			set {
				if (this.m_IdServicioINS != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdServicioINS", m_IdServicioINS, value));
					m_IdServicioINS = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdSerResINS;

		//[PropiedadOriginal("IdSerResINS" ,EsClave = true ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdSerResINS
		{
			get { return m_IdSerResINS; }
			set {
				if (this.m_IdSerResINS != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdSerResINS", m_IdSerResINS, value));
					m_IdSerResINS = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdReservaINS;

		//[PropiedadOriginal("IdReservaINS" ,EsClave = true ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdReservaINS
		{
			get { return m_IdReservaINS; }
			set {
				if (this.m_IdReservaINS != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdReservaINS", m_IdReservaINS, value));
					m_IdReservaINS = value;
					
				}
			}
		}
		private String m_IdEstadoINS;

		//[PropiedadOriginal("IdEstadoINS"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
		public String IdEstadoINS
		{
			get { return m_IdEstadoINS; }
			set {
				if (this.m_IdEstadoINS != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEstadoINS", m_IdEstadoINS, value));
					m_IdEstadoINS = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdServicioDSP;

		//[PropiedadOriginal("IdServicioDSP"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdServicioDSP
		{
			get { return m_IdServicioDSP; }
			set {
				if (this.m_IdServicioDSP != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdServicioDSP", m_IdServicioDSP, value));
					m_IdServicioDSP = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdSerResDSP;

		//[PropiedadOriginal("IdSerResDSP" ,EsClave = true ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdSerResDSP
		{
			get { return m_IdSerResDSP; }
			set {
				if (this.m_IdSerResDSP != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdSerResDSP", m_IdSerResDSP, value));
					m_IdSerResDSP = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdReservaDSP;

		//[PropiedadOriginal("IdReservaDSP" ,EsClave = true ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdReservaDSP
		{
			get { return m_IdReservaDSP; }
			set {
				if (this.m_IdReservaDSP != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdReservaDSP", m_IdReservaDSP, value));
					m_IdReservaDSP = value;
					
				}
			}
		}
		private String m_IdEstadoDSP;

		//[PropiedadOriginal("IdEstadoDSP"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
		public String IdEstadoDSP
		{
			get { return m_IdEstadoDSP; }
			set {
				if (this.m_IdEstadoDSP != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEstadoDSP", m_IdEstadoDSP, value));
					m_IdEstadoDSP = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdServicioHOT;

		//[PropiedadOriginal("IdServicioHOT"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdServicioHOT
		{
			get { return m_IdServicioHOT; }
			set {
				if (this.m_IdServicioHOT != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdServicioHOT", m_IdServicioHOT, value));
					m_IdServicioHOT = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdSerResHOT;

		//[PropiedadOriginal("IdSerResHOT" ,EsClave = true ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdSerResHOT
		{
			get { return m_IdSerResHOT; }
			set {
				if (this.m_IdSerResHOT != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdSerResHOT", m_IdSerResHOT, value));
					m_IdSerResHOT = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdReservaHOT;

		//[PropiedadOriginal("IdReservaHOT" ,EsClave = true ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdReservaHOT
		{
			get { return m_IdReservaHOT; }
			set {
				if (this.m_IdReservaHOT != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdReservaHOT", m_IdReservaHOT, value));
					m_IdReservaHOT = value;
					
				}
			}
		}
		private String m_IdEstadoHOT;

		//[PropiedadOriginal("IdEstadoHOT"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
		public String IdEstadoHOT
		{
			get { return m_IdEstadoHOT; }
			set {
				if (this.m_IdEstadoHOT != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEstadoHOT", m_IdEstadoHOT, value));
					m_IdEstadoHOT = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdServicioACT;

		//[PropiedadOriginal("IdServicioACT"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdServicioACT
		{
			get { return m_IdServicioACT; }
			set {
				if (this.m_IdServicioACT != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdServicioACT", m_IdServicioACT, value));
					m_IdServicioACT = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdSerResACT;

		//[PropiedadOriginal("IdSerResACT" ,EsClave = true ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdSerResACT
		{
			get { return m_IdSerResACT; }
			set {
				if (this.m_IdSerResACT != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdSerResACT", m_IdSerResACT, value));
					m_IdSerResACT = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdReservaACT;

		//[PropiedadOriginal("IdReservaACT" ,EsClave = true ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdReservaACT
		{
			get { return m_IdReservaACT; }
			set {
				if (this.m_IdReservaACT != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdReservaACT", m_IdReservaACT, value));
					m_IdReservaACT = value;
					
				}
			}
		}
		private String m_IdEstadoACT;

		//[PropiedadOriginal("IdEstadoACT"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
		public String IdEstadoACT
		{
			get { return m_IdEstadoACT; }
			set {
				if (this.m_IdEstadoACT != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEstadoACT", m_IdEstadoACT, value));
					m_IdEstadoACT = value;
					
				}
			}
		}
		private Nullable<Double> m_ImporteINS; 

		//[PropiedadOriginal("ImporteINS"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.BinaryDouble , Longitud = 12)]
		public Nullable<Double> ImporteINS
		{
			get { return m_ImporteINS; }
			set {
				if (this.m_ImporteINS != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ImporteINS", m_ImporteINS, value));
					m_ImporteINS = value;
					
				}
			}
		}
		private Nullable<Double> m_ImporteDSP;

		//[PropiedadOriginal("ImporteDSP"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.BinaryDouble , Longitud = 12)]
		public Nullable<Double> ImporteDSP
		{
			get { return m_ImporteDSP; }
			set {
				if (this.m_ImporteDSP != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ImporteDSP", m_ImporteDSP, value));
					m_ImporteDSP = value;
					
				}
			}
		}
		private Nullable<Double> m_ImporteHOT;

		//[PropiedadOriginal("ImporteHOT"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.BinaryDouble , Longitud = 12)]
		public Nullable<Double> ImporteHOT
		{
			get { return m_ImporteHOT; }
			set {
				if (this.m_ImporteHOT != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ImporteHOT", m_ImporteHOT, value));
					m_ImporteHOT = value;
					
				}
			}
		}
		private Nullable<Double> m_ImporteACT;

		//[PropiedadOriginal("ImporteACT"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.BinaryDouble , Longitud = 12)]
		public Nullable<Double> ImporteACT
		{
			get { return m_ImporteACT; }
			set {
				if (this.m_ImporteACT != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ImporteACT", m_ImporteACT, value));
					m_ImporteACT = value;
					
				}
			}
		}

		private Nullable<Double> m_ImporteFee;

		//[PropiedadOriginal("ImporteACT"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.BinaryDouble , Longitud = 12)]
		public Nullable<Double> ImporteFee
		{
			get { return m_ImporteFee; }
			set
			{
				if (this.m_ImporteFee != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ImporteACT", m_ImporteACT, value));
					m_ImporteFee = value;

				}
			}
		}

		private Nullable<Double> m_Total;

		//[PropiedadOriginal("Total"  ,EsNullable = true , TablaOriginal = "cv_ser_passenger_resumen" , TipoProveedor = ClepsydraDbType.BinaryDouble , Longitud = 23)]
		public Nullable<Double> Total
		{
			get { return m_Total; }
			set {
				if (this.m_Total != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Total", m_Total, value));
					m_Total = value;
					
				}
			}
		}
		#endregion

		#region Constructores

		public DVServicioPassengerResumen()
		{
		}
		public DVServicioPassengerResumen(Int32 _IdSerResINS ,Int32 _IdReservaINS ,Int32 _IdSerResDSP ,Int32 _IdReservaDSP ,Int32 _IdSerResHOT ,Int32 _IdReservaHOT ,Int32 _IdSerResACT ,Int32 _IdReservaACT)
		{
			IdSerResINS = _IdSerResINS;
			IdReservaINS = _IdReservaINS;
			IdSerResDSP = _IdSerResDSP;
			IdReservaDSP = _IdReservaDSP;
			IdSerResHOT = _IdSerResHOT;
			IdReservaHOT = _IdReservaHOT;
			IdSerResACT = _IdSerResACT;
			IdReservaACT = _IdReservaACT;
		}

		#endregion

        #region Converter

        public static ICollection<DVServicioPassengerResumen> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVServicioPassengerResumen> collection = new Collection<DVServicioPassengerResumen>();

            foreach(DataRow row in dtTable.Rows)
            {
                DVServicioPassengerResumen data = new DVServicioPassengerResumen()
                {
                    IdPassengerList = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdPassengerList")),
                    IdExpediente = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdExpediente")),
                    Nombre = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Nombre"),
                    Apel1 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Apel1"),
                    Apel2 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Apel2"),
                    IdServicioINS = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdServicioINS")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdServicioINS"))),
                    IdSerResINS = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdSerResINS")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdSerResINS"))),
                    IdReservaINS = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdReservaINS")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdReservaINS"))),
                    IdEstadoINS = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEstadoINS"),
                    IdServicioDSP = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdServicioDSP")) ? new Nullable<int>(): int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdServicioDSP"))),
                    IdSerResDSP = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdSerResDSP")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdSerResDSP"))),
                    IdReservaDSP = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdReservaDSP")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdReservaDSP"))),
                    IdEstadoDSP = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEstadoDSP"),
                    IdServicioHOT = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdServicioHOT")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdServicioHOT"))),
                    IdSerResHOT = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdSerResHOT")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdSerResHOT"))),
                    IdReservaHOT = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdReservaHOT")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdReservaHOT"))),
                    IdEstadoHOT = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEstadoHOT"),
                    IdServicioACT = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdServicioACT")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdServicioACT"))),
                    IdSerResACT = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdSerResACT")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdSerResACT"))),
                    IdReservaACT = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdReservaACT")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdReservaACT"))),
                    IdEstadoACT = Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEstadoACT"),
                    ImporteINS = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "ImporteINS")) ? new Nullable<double>() : double.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "ImporteINS"))),
                    ImporteDSP = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "ImporteDSP")) ? new Nullable<double>() : double.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "ImporteDSP"))),
                    ImporteHOT = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "ImporteHOT")) ? new Nullable<double>() : double.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "ImporteHOT"))),
                    ImporteACT = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "ImporteACT")) ? new Nullable<double>() : double.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "ImporteACT"))),
					ImporteFee = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "ImporteFee")) ? new Nullable<double>() : double.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "ImporteFee"))),
					Total = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Total")) ? new Nullable<double>() : double.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Total"))),
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
	}
}
