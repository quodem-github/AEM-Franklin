using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
	public class DVParticipanteVeeva
	{
		#region Propiedades

        private String m_NivelRiesgo;
        //[PropiedadOriginal("nivelriesgo", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 50)]
        public String nivelriesgo
        {
            get { return m_NivelRiesgo; }
            set
            {
                if (this.m_NivelRiesgo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("nivelriesgo", m_NivelRiesgo, value));
                    m_NivelRiesgo = value;

                }
            }
        }

        private String m_pagosociedad;
        //[PropiedadOriginal("pagosociedad", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 50)]
        public String pagosociedad
        {
            get { return m_pagosociedad; }
            set
            {
                if (this.m_pagosociedad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("pagosociedad", m_pagosociedad, value));
                    m_pagosociedad = value;

                }
            }
        }

        private Int32 m_pagodirecto;

        //[PropiedadOriginal("pagodirecto", EsClave = true, TablaOriginal = "cv_participantes", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 pagodirecto
	    {
	        get { return m_pagodirecto; }
	        set
	        {
	            if (this.m_pagodirecto != value)
	            {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("pagodirecto", m_pagodirecto, value));
                    m_pagodirecto = value;

	            }
	        }
	    }

        private Int32 m_ficherogenesis;
        //[PropiedadOriginal("ficherogenesis", EsClave = true, TablaOriginal = "cv_participantes", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 ficherogenesis
		{
            get { return m_ficherogenesis; }
			set {
                if (this.m_ficherogenesis != value)
				{
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ficherogenesis", m_ficherogenesis, value));
                    m_ficherogenesis = value;
					
				}
			}
		}

        private Int64 m_IdTipoActividadPax;
        //[PropiedadOriginal("idtipoactividadpax", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int64 idtipoactividadpax
        {
            get { return m_IdTipoActividadPax; }
            set
            {
                if (this.m_IdTipoActividadPax != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtipoactividadpax", m_IdTipoActividadPax, value));
                    m_IdTipoActividadPax = value;

                }
            }
        }

        private String m_TipoActividadPax;
        //[PropiedadOriginal("tipoactividadpax", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 50)]
        public String tipoactividadpax
        {
            get { return m_TipoActividadPax; }
            set
            {
                if (this.m_TipoActividadPax != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("tipoactividadpax", m_TipoActividadPax, value));
                    m_TipoActividadPax = value;

                }
            }
        }

        private String m_TipoAsistente;
         //[PropiedadOriginal("tipoasistente", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 50)]
        public String tipoasistente
        {
            get { return m_TipoAsistente; }
            set
            {
                if (this.m_TipoAsistente != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("tipoasistente", m_TipoAsistente, value));
                    m_TipoAsistente = value;

                }
            }
        }

         private Int64 m_idTipoAsistente;

        //[PropiedadOriginal("idtipoasistente", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int64 idtipoasistente
         {
             get { return m_idTipoAsistente; }
             set
             {
                 if (this.m_idTipoAsistente != value)
                 {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtipoasistente", m_idTipoAsistente, value));
                    m_idTipoAsistente = value;

                 }
             }
         }


         private Decimal m_Honorarios;
         //[PropiedadOriginal("honorarios", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 50)]
         public Decimal honorarios
         {
             get { return m_Honorarios; }
             set
             {
                 if (this.m_Honorarios != value)
                 {
                     //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Honorarios", m_Honorarios, value));
                     m_Honorarios = value;

                 }
             }
         }


        private Int32 m_IdPassengerlist;
		//[PropiedadOriginal("IdPassengerlist" ,EsClave = true  , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Int32 IdPassengerlist
		{
			get { return m_IdPassengerlist; }
			set {
				if (this.m_IdPassengerlist != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdPassengerlist", m_IdPassengerlist, value));
					m_IdPassengerlist = value;
					
				}
			}
		}
		private String m_Nombre;

		//[PropiedadOriginal("Nombre"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
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

        private String m_FullName;

        //[PropiedadOriginal("Nombre"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
        public String FullName
        {
            get { return m_FullName; }
            set
            {
                if (this.m_FullName != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Nombre", m_Nombre, value));
                    m_FullName = value;

                }
            }
        }

        private String m_Apel1;

		//[PropiedadOriginal("Apel1"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
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

		//[PropiedadOriginal("Apel2"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
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
		private String m_Email;

		//[PropiedadOriginal("Email"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 128)]
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
		private String m_Pasaporte;

		//[PropiedadOriginal("Pasaporte"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 50)]
		public String Pasaporte
		{
			get { return m_Pasaporte; }
			set {
				if (this.m_Pasaporte != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Pasaporte", m_Pasaporte, value));
					m_Pasaporte = value;
					
				}
			}
		}
		private String m_Hospital;

		//[PropiedadOriginal("Hospital"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
		public String Hospital
		{
			get { return m_Hospital; }
			set {
				if (this.m_Hospital != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Hospital", m_Hospital, value));
					m_Hospital = value;
					
				}
			}
		}
		private String m_Localidad;

		//[PropiedadOriginal("Localidad"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 40)]
		public String Localidad
		{
			get { return m_Localidad; }
			set {
				if (this.m_Localidad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Localidad", m_Localidad, value));
					m_Localidad = value;
					
				}
			}
		}
		private String m_Msdid;

		//[PropiedadOriginal("Msdid"   , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 15)]
		public String Msdid
		{
			get { return m_Msdid; }
			set {
				if (this.m_Msdid != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Msdid", m_Msdid, value));
					m_Msdid = value;
					
				}
			}
		}
		private Nullable<Int32> m_IdDistrito;

		//[PropiedadOriginal("IdDistrito"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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
		private String m_Distrito;

		//[PropiedadOriginal("Distrito"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
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
		private Nullable<Int32> m_IdRegion;

		//[PropiedadOriginal("IdRegion"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdRegion
		{
			get { return m_IdRegion; }
			set {
				if (this.m_IdRegion != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdRegion", m_IdRegion, value));
					m_IdRegion = value;
					
				}
			}
		}
		private String m_Region;

		//[PropiedadOriginal("Region"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 25)]
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
		private Nullable<Int32> m_IdEmpresa;

		//[PropiedadOriginal("IdEmpresa"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdEmpresa
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

		//[PropiedadOriginal("RazonSocial"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 45)]
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
		private Nullable<Int32> m_IdEspecialidad;

		//[PropiedadOriginal("IdEspecialidad"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
		public Nullable<Int32> IdEspecialidad
		{
			get { return m_IdEspecialidad; }
			set {
				if (this.m_IdEspecialidad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEspecialidad", m_IdEspecialidad, value));
					m_IdEspecialidad = value;
					
				}
			}
		}
		private String m_Especialidad;

		//[PropiedadOriginal("Especialidad"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 80)]
		public String Especialidad
		{
			get { return m_Especialidad; }
			set {
				if (this.m_Especialidad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Especialidad", m_Especialidad, value));
					m_Especialidad = value;
					
				}
			}
		}
		private String m_CodEspecialidad;

		//[PropiedadOriginal("CodEspecialidad"  ,EsNullable = true , TablaOriginal = "cv_participantes" , TipoProveedor = ClepsydraDbType.NVarchar2 , Longitud = 5)]
		public String CodEspecialidad
		{
			get { return m_CodEspecialidad; }
			set {
				if (this.m_CodEspecialidad != value)
				{
					//OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("CodEspecialidad", m_CodEspecialidad, value));
					m_CodEspecialidad = value;
					
				}
			}
		}

	    private Nullable<Int32> m_tiporeunion;

        //[PropiedadOriginal("tiporeunion", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> tiporeunion
        {
            get { return m_tiporeunion; }
            set
            {
                if (this.m_tiporeunion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("tiporeunion", m_tiporeunion, value));
                    m_tiporeunion = value;

                }
            }
        }

	    private Nullable<Int32> m_ponentes_duracionactividad;

        //[PropiedadOriginal("ponentes_duracionactividad", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> ponentes_duracionactividad
        {
            get { return m_ponentes_duracionactividad; }
            set
            {
                if (this.m_ponentes_duracionactividad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ponentes_duracionactividad", m_ponentes_duracionactividad, value));
                    m_ponentes_duracionactividad = value;

                }
            }
        }

	    private Nullable<Int32> m_ponentes_preparacion;

        //[PropiedadOriginal("ponentes_preparacion", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> ponentes_preparacion
        {
            get { return m_ponentes_preparacion; }
            set
            {
                if (this.m_ponentes_preparacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ponentes_preparacion", m_ponentes_preparacion, value));
                    m_ponentes_preparacion = value;

                }
            }
        }

	    private Nullable<Int32> m_ponentes_nivelps;

        //[PropiedadOriginal("ponentes_nivelps", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> ponentes_nivelps
        {
            get { return m_ponentes_nivelps; }
            set
            {
                if (this.m_ponentes_nivelps != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ponentes_nivelps", m_ponentes_nivelps, value));
                    m_ponentes_nivelps = value;

                }
            }
        }

        private Nullable<Single> m_ponentes_honorariosmaximos;

        //[PropiedadOriginal("ponentes_honorariosmaximos", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 11)]
        public Nullable<Single> ponentes_honorariosmaximos
        {
            get { return m_ponentes_honorariosmaximos; }
            set
            {
                if (this.m_ponentes_honorariosmaximos != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ponentes_honorariosmaximos", m_ponentes_honorariosmaximos, value));
                    m_ponentes_honorariosmaximos = value;

                }
            }
        }

        private Nullable<double> m_FloatMinValue;

        //[PropiedadOriginal("FloatMinValue", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 11)]
        public Nullable<double> FloatMinValue
        {
            get { return m_FloatMinValue; }
            set
            {
                if (this.m_FloatMinValue != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FloatMinValue", m_FloatMinValue, value));
                    m_FloatMinValue = value;

                }
            }
        }
        
	    private Nullable<Int32> m_ponentes_idhonorariosmaximos;

        //[PropiedadOriginal("ponentes_idhonorariosmaximos", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> ponentes_idhonorariosmaximos
        {
            get { return m_ponentes_idhonorariosmaximos; }
            set
            {
                if (this.m_ponentes_idhonorariosmaximos != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ponentes_idhonorariosmaximos", m_ponentes_idhonorariosmaximos, value));
                    m_ponentes_idhonorariosmaximos = value;

                }
            }
        }

	    private Nullable<Int32> m_abeif_preparacion;

        //[PropiedadOriginal("abeif_preparacion", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> abeif_preparacion
        {
            get { return m_abeif_preparacion; }
            set
            {
                if (this.m_abeif_preparacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("abeif_preparacion", m_abeif_preparacion, value));
                    m_abeif_preparacion = value;

                }
            }
        }

        private Nullable<Int32> m_abeif_duracionactividad;

        //[PropiedadOriginal("abeif_duracionactividad", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> abeif_duracionactividad
        {
            get { return m_abeif_duracionactividad; }
            set
            {
                if (this.m_abeif_duracionactividad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("abeif_duracionactividad", m_abeif_duracionactividad, value));
                    m_abeif_duracionactividad = value;

                }
            }
        }

	    private Nullable<Int32> m_abeif_nivelps;

        //[PropiedadOriginal("abeif_nivelps", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> abeif_nivelps
        {
            get { return m_abeif_nivelps; }
            set
            {
                if (this.m_abeif_nivelps != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("abeif_nivelps", m_abeif_nivelps, value));
                    m_abeif_nivelps = value;

                }
            }
        }

        private Nullable<Single> m_abeif_honorariosmaximos;

        //[PropiedadOriginal("abeif_honorariosmaximos", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 11)]
        public Nullable<Single> abeif_honorariosmaximos
        {
            get { return m_abeif_honorariosmaximos; }
            set
            {
                if (this.m_abeif_honorariosmaximos != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("abeif_honorariosmaximos", m_abeif_honorariosmaximos, value));
                    m_abeif_honorariosmaximos = value;

                }
            }
        }

        private Nullable<Int32> m_ponencia_centro_salud;

        //[PropiedadOriginal("ponencia_centro_salud", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.UnsignedBigInt, Longitud = 11)]
        public Nullable<Int32> ponencia_centro_salud
        {
            get { return m_ponencia_centro_salud; }
            set
            {
                if (this.m_ponencia_centro_salud != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("ponencia_centro_salud", m_ponencia_centro_salud, value));
                    m_ponencia_centro_salud = value;

                }
            }
        }

        private Nullable<Int32> m_talleres;

        //[PropiedadOriginal("talleres", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.UnsignedBigInt, Longitud = 11)]
        public Nullable<Int32> talleres
        {
            get { return m_talleres; }
            set
            {
                if (this.m_talleres != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("talleres", m_talleres, value));
                    m_talleres = value;

                }
            }
        }
        
        private Nullable<Int32> m_videoconferencia_repetida;

        //[PropiedadOriginal("videoconferencia_repetida", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.UnsignedBigInt, Longitud = 11)]
        public Nullable<Int32> videoconferencia_repetida
        {
            get { return m_videoconferencia_repetida; }
            set
            {
                if (this.m_videoconferencia_repetida != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("videoconferencia_repetida", m_videoconferencia_repetida, value));
                    m_videoconferencia_repetida = value;

                }
            }
        }
        
	    private Nullable<Int32> m_tipo_ponente;

        //[PropiedadOriginal("tipo_ponente", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> tipo_ponente
        {
            get { return m_tipo_ponente; }
            set
            {
                if (this.m_tipo_ponente != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("tipo_ponente", m_tipo_ponente, value));
                    m_tipo_ponente = value;

                }
            }
        }

	    private String m_justificacion;

        //[PropiedadOriginal("justificacion", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 400)]
        public String justificacion
        {
            get { return m_justificacion; }
            set
            {
                if (this.m_justificacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("justificacion", m_justificacion, value));
                    m_justificacion = value;

                }
            }
        }

	    private Nullable<Int32> m_idTipoContratoConsultoria;

        //[PropiedadOriginal("idTipoContratoConsultoria", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> idTipoContratoConsultoria
        {
            get { return m_idTipoContratoConsultoria; }
            set
            {
                if (this.m_idTipoContratoConsultoria != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idTipoContratoConsultoria", m_idTipoContratoConsultoria, value));
                    m_idTipoContratoConsultoria = value;

                }
            }
        }

	    private Nullable<Int32> m_numero_dias_consultoria;

        //[PropiedadOriginal("numero_dias_consultoria", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> numero_dias_consultoria
        {
            get { return m_numero_dias_consultoria; }
            set
            {
                if (this.m_numero_dias_consultoria != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("numero_dias_consultoria", m_numero_dias_consultoria, value));
                    m_numero_dias_consultoria = value;

                }
            }
        }

        private String m_AttendeeType;

        //[PropiedadOriginal("justificacion", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 400)]
        public String AttendeeType
        {
            get { return m_AttendeeType; }
            set
            {
                if (this.m_AttendeeType != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("justificacion", m_justificacion, value));
                    m_AttendeeType = value;

                }
            }
        }

        private Nullable<Int32> m_AttendeeTypeOrder;

        //[PropiedadOriginal("numero_dias_consultoria", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> AttendeeTypeOrder
        {
            get { return m_AttendeeTypeOrder; }
            set
            {
                if (this.m_AttendeeTypeOrder != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("numero_dias_consultoria", m_numero_dias_consultoria, value));
                    m_AttendeeTypeOrder = value;

                }
            }
        }

        private String m_Status;

        //[PropiedadOriginal("justificacion", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 400)]
        public String Status
        {
            get { return m_Status; }
            set
            {
                if (this.m_Status != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("justificacion", m_justificacion, value));
                    m_Status = value;

                }
            }
        }

        #endregion
        #region Miembros de INotificaCambiosPropiedad

        #endregion

        #region Constructores

        public DVParticipanteVeeva()
		{
		}
		public DVParticipanteVeeva(Int32 _IdPassengerlist)
		{
			IdPassengerlist = _IdPassengerlist;
		}

		#endregion

        #region Converter

        public static ICollection<DVParticipanteVeeva> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVParticipanteVeeva> collection = new Collection<DVParticipanteVeeva>();

            foreach (DataRow row in dtTable.Rows)
            {
                DVParticipanteVeeva data = new DVParticipanteVeeva()
                {
                    nivelriesgo = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nivelriesgo"),
                    tipoactividadpax = Quodem.Utility.DataLayerUtil.GetStringValue(row, "tipoactividadpax"),
                    tipoasistente = Quodem.Utility.DataLayerUtil.GetStringValue(row, "tipoasistente"),
                    honorarios = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "honorarios")) ? new decimal() : decimal.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "honorarios"))),
                    IdPassengerlist = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdPassengerlist")),
                    Nombre = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Nombre"),
                    FullName = Quodem.Utility.DataLayerUtil.GetStringValue(row, "FullName"),
                    Apel1 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Apel1"),
                    Apel2 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Apel2"),
                    Email = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Email"),
                    Pasaporte = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Pasaporte"),
                    Hospital = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Hospital"),
                    Localidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Localidad"),
                    Msdid = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Msdid"),
                    IdDistrito = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdDistrito")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdDistrito"))),
                    Distrito = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Distrito"),
                    IdRegion = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdRegion")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdRegion"))),
                    Region = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Region"),
                    IdEmpresa = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEmpresa")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEmpresa"))),
                    RazonSocial = Quodem.Utility.DataLayerUtil.GetStringValue(row, "RazonSocial"),
                    IdEspecialidad = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEspecialidad")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEspecialidad"))),
                    Especialidad = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Especialidad"),
                    AttendeeType = Quodem.Utility.DataLayerUtil.GetStringValue(row, "AttendeeType"),
                    AttendeeTypeOrder = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "AttendeeTypeOrder")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "AttendeeTypeOrder"))),
                    Status = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Status"),
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
	}
}
