using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DAmec
    {
        #region Propiedades
        private Int32 m_idamec;

        //[PropiedadOriginal("idamec", EsClave = true, TablaOriginal = "amec", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        //NO CAMBIAR EL TIPO INT
        public Int32 idamec
        {
            get { return m_idamec; }
            set
            {
                if (this.m_idamec != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idamec", m_idamec, value));
                    m_idamec = value;

                }
            }
        }
        private String m_amec;

        //[PropiedadOriginal("amec", TablaOriginal = "amec", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        public String amec
        {
            get { return m_amec; }
            set
            {
                if (this.m_amec != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("amec", m_amec, value));
                    m_amec = value;

                }
            }
        }
        private Int32 m_IdEmpresa;

        //[PropiedadOriginal("IdEmpresa", TablaOriginal = "amec", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IdEmpresa
        {
            get { return m_IdEmpresa; }
            set
            {
                if (this.m_IdEmpresa != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdEmpresa", m_IdEmpresa, value));
                    m_IdEmpresa = value;

                }
            }
        }
        private String m_idestado;

   
        public String idestado
        {
            get { return m_idestado; }
            set
            {
                if (this.m_idestado != value)
                {
              
                    m_idestado = value;

                }
            }
        }
        private Nullable<Boolean> m_aprobado;

     
        public Nullable<Boolean> aprobado
        {
            get { return m_aprobado; }
            set
            {
                if (this.m_aprobado != value)
                {
     
                    m_aprobado = value;

                }
            }
        }
        private Nullable<Int32> m_idpeticionario;

     
        public Nullable<Int32> idpeticionario
        {
            get { return m_idpeticionario; }
            set
            {
                if (this.m_idpeticionario != value)
                {
           
                    m_idpeticionario = value;

                }
            }
        }
        private Nullable<DateTime> m_fechaaprobadolegal;

    
        public Nullable<DateTime> fechaaprobadolegal
        {
            get { return m_fechaaprobadolegal; }
            set
            {
                if (this.m_fechaaprobadolegal != value)
                {
               
                    m_fechaaprobadolegal = value;

                }
            }
        }
        private Nullable<Boolean> m_aprobadolegal;

       
        public Nullable<Boolean> aprobadolegal
        {
            get { return m_aprobadolegal; }
            set
            {
                if (this.m_aprobadolegal != value)
                {
       
                    m_aprobadolegal = value;

                }
            }
        }
        private Nullable<Int32> m_idpeticionario2;

       
        public Nullable<Int32> idpeticionario2
        {
            get { return m_idpeticionario2; }
            set
            {
                if (this.m_idpeticionario2 != value)
                {
       
                    m_idpeticionario2 = value;

                }
            }
        }
        private Nullable<DateTime> m_fechaaprobadocomplaice;

    
        public Nullable<DateTime> fechaaprobadocomplaice
        {
            get { return m_fechaaprobadocomplaice; }
            set
            {
                if (this.m_fechaaprobadocomplaice != value)
                {
         
                    m_fechaaprobadocomplaice = value;

                }
            }
        }
        private Nullable<Boolean> m_aprobadocomplaice;

      
        public Nullable<Boolean> aprobadocomplaice
        {
            get { return m_aprobadocomplaice; }
            set
            {
                if (this.m_aprobadocomplaice != value)
                {
              
                    m_aprobadocomplaice = value;

                }
            }
        }
        private Nullable<Int32> m_idpeticionactividad;

        //[PropiedadOriginal("idpeticionactividad", EsNullable = true, TablaOriginal = "amec", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> idpeticionactividad
        {
            get { return m_idpeticionactividad; }
            set
            {
                if (this.m_idpeticionactividad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idpeticionactividad", m_idpeticionactividad, value));
                    m_idpeticionactividad = value;

                }
            }
        }
        private Nullable<Int32> m_IdCongreso;

        //[PropiedadOriginal("IdCongreso", EsNullable = true, TablaOriginal = "amec", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> IdCongreso
        {
            get { return m_IdCongreso; }
            set
            {
                if (this.m_IdCongreso != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdCongreso", m_IdCongreso, value));
                    m_IdCongreso = value;

                }
            }
        }
        private Nullable<Int32> m_Idarea;

    
        public Nullable<Int32> Idarea
        {
            get { return m_Idarea; }
            set
            {
                if (this.m_Idarea != value)
                {
     
                    m_Idarea = value;

                }
            }
        }
        private Nullable<Int32> m_idarea1;

     
        public Nullable<Int32> idarea1
        {
            get { return m_idarea1; }
            set
            {
                if (this.m_idarea1 != value)
                {
       
                    m_idarea1 = value;

                }
            }
        }
        private Nullable<Int32> m_idarea2;

     
        public Nullable<Int32> idarea2
        {
            get { return m_idarea2; }
            set
            {
                if (this.m_idarea2 != value)
                {
        
                    m_idarea2 = value;

                }
            }
        }
        private Nullable<Int32> m_idarea3;

      
        public Nullable<Int32> idarea3
        {
            get { return m_idarea3; }
            set
            {
                if (this.m_idarea3 != value)
                {
      
                    m_idarea3 = value;

                }
            }
        }
        private Nullable<Int32> m_idunidad;

      
        public Nullable<Int32> idunidad
        {
            get { return m_idunidad; }
            set
            {
                if (this.m_idunidad != value)
                {
           
                    m_idunidad = value;

                }
            }
        }
        private Nullable<Int32> m_idtipoactividadcongreso;

        
        public Nullable<Int32> idtipoactividadcongreso
        {
            get { return m_idtipoactividadcongreso; }
            set
            {
                if (this.m_idtipoactividadcongreso != value)
                {
            
                    m_idtipoactividadcongreso = value;

                }
            }
        }
        private Nullable<Int32> m_iddistrito;

     
        public Nullable<Int32> iddistrito
        {
            get { return m_iddistrito; }
            set
            {
                if (this.m_iddistrito != value)
                {
         
                    m_iddistrito = value;

                }
            }
        }
        private Nullable<Boolean> m_paraguas;

    
        public Nullable<Boolean> paraguas
        {
            get { return m_paraguas; }
            set
            {
                if (this.m_paraguas != value)
                {
            
                    m_paraguas = value;

                }
            }
        }
        private String m_especificarotras;

      
        public String especificarotras
        {
            get { return m_especificarotras; }
            set
            {
                if (this.m_especificarotras != value)
                {
     
                    m_especificarotras = value;

                }
            }
        }
        private String m_nombreprograma;

    
        public String nombreprograma
        {
            get { return m_nombreprograma; }
            set
            {
                if (this.m_nombreprograma != value)
                {
       
                    m_nombreprograma = value;

                }
            }
        }
        private String m_codgenesis;

    
        public String codgenesis
        {
            get { return m_codgenesis; }
            set
            {
                if (this.m_codgenesis != value)
                {
      
                    m_codgenesis = value;

                }
            }
        }
        private String m_objetivosprograma;

     
        public String objetivosprograma
        {
            get { return m_objetivosprograma; }
            set
            {
                if (this.m_objetivosprograma != value)
                {
        
                    m_objetivosprograma = value;

                }
            }
        }
        private String m_contenido;

     
        public String contenido
        {
            get { return m_contenido; }
            set
            {
                if (this.m_contenido != value)
                {
                 
                    m_contenido = value;

                }
            }
        }
        private String m_lugar;

   
        public String lugar
        {
            get { return m_lugar; }
            set
            {
                if (this.m_lugar != value)
                {
           
                    m_lugar = value;

                }
            }
        }
        private String m_ambitogeografico;

    
        public String ambitogeografico
        {
            get { return m_ambitogeografico; }
            set
            {
                if (this.m_ambitogeografico != value)
                {
           
                    m_ambitogeografico = value;

                }
            }
        }
        private String m_duracion;

     
        public String duracion
        {
            get { return m_duracion; }
            set
            {
                if (this.m_duracion != value)
                {
     
                    m_duracion = value;

                }
            }
        }
        private Nullable<Int32> m_numparticipantes;

     
        public Nullable<Int32> numparticipantes
        {
            get { return m_numparticipantes; }
            set
            {
                if (this.m_numparticipantes != value)
                {
     
                    m_numparticipantes = value;

                }
            }
        }
        private Nullable<DateTime> m_fechacomienzop;


        public Nullable<DateTime> fechacomienzop
        {
            get { return m_fechacomienzop; }
            set
            {
                if (this.m_fechacomienzop != value)
                {

                    m_fechacomienzop = value;

                }
            }
        }
        private Nullable<DateTime> m_fechafinp;


        public Nullable<DateTime> fechafinp
        {
            get { return m_fechafinp; }
            set
            {
                if (this.m_fechafinp != value)
                {

                    m_fechafinp = value;

                }
            }
        }
        private String m_gastosdes_aloj;


        public String gastosdes_aloj
        {
            get { return m_gastosdes_aloj; }
            set
            {
                if (this.m_gastosdes_aloj != value)
                {

                    m_gastosdes_aloj = value;

                }
            }
        }
       
      
        private String m_otros;


        public String otros
        {
            get { return m_otros; }
            set
            {
                if (this.m_otros != value)
                {
                
                    m_otros = value;

                }
            }
        }
        private String m_relponentes;


        public String relponentes
        {
            get { return m_relponentes; }
            set
            {
                if (this.m_relponentes != value)
                {

                    m_relponentes = value;

                }
            }
        }
        private Nullable<Int32> m_numponentes;


        public Nullable<Int32> numponentes
        {
            get { return m_numponentes; }
            set
            {
                if (this.m_numponentes != value)
                {

                    m_numponentes = value;

                }
            }
        }
        private String m_observacioneslegal;


        public String observacioneslegal
        {
            get { return m_observacioneslegal; }
            set
            {
                if (this.m_observacioneslegal != value)
                {

                    m_observacioneslegal = value;

                }
            }
        }
        private String m_honorarios;


        public String honorarios
        {
            get { return m_honorarios; }
            set
            {
                if (this.m_honorarios != value)
                {

                    m_honorarios = value;

                }
            }
        }
        private String m_conceptogastos;


        public String conceptogastos
        {
            get { return m_conceptogastos; }
            set
            {
                if (this.m_conceptogastos != value)
                {

                    m_conceptogastos = value;

                }
            }
        }
        private Nullable<Single> m_importetotal;


        public Nullable<Single> importetotal
        {
            get { return m_importetotal; }
            set
            {
                if (this.m_importetotal != value)
                {

                    m_importetotal = value;

                }
            }
        }
        private Nullable<Int32> m_numpropuestas;


        public Nullable<Int32> numpropuestas
        {
            get { return m_numpropuestas; }
            set
            {
                if (this.m_numpropuestas != value)
                {

                    m_numpropuestas = value;

                }
            }
        }
        private Nullable<Int32> m_numcartas;


        public Nullable<Int32> numcartas
        {
            get { return m_numcartas; }
            set
            {
                if (this.m_numcartas != value)
                {

                    m_numcartas = value;

                }
            }
        }
        private Nullable<Int32> m_numhojasinscripcion;


        public Nullable<Int32> numhojasinscripcion
        {
            get { return m_numhojasinscripcion; }
            set
            {
                if (this.m_numhojasinscripcion != value)
                {

                    m_numhojasinscripcion = value;

                }
            }
        }
        private Nullable<Int32> m_idtipopatrocinio;


        public Nullable<Int32> idtipopatrocinio
        {
            get { return m_idtipopatrocinio; }
            set
            {
                if (this.m_idtipopatrocinio != value)
                {

                    m_idtipopatrocinio = value;

                }
            }
        }
        private String m_mailautorizadofi;


        public String mailautorizadofi
        {
            get { return m_mailautorizadofi; }
            set
            {
                if (this.m_mailautorizadofi != value)
                {

                    m_mailautorizadofi = value;

                }
            }
        }
        private String m_ficheroprograma;


        public String ficheroprograma
        {
            get { return m_ficheroprograma; }
            set
            {
                if (this.m_ficheroprograma != value)
                {

                    m_ficheroprograma = value;

                }
            }
        }
        private Nullable<Int32> m_locked;


        public Nullable<Int32> locked
        {
            get { return m_locked; }
            set
            {
                if (this.m_locked != value)
                {

                    m_locked = value;

                }
            }
        }
        private Nullable<DateTime> m_fechaaprobadodg;


        public Nullable<DateTime> fechaaprobadodg
        {
            get { return m_fechaaprobadodg; }
            set
            {
                if (this.m_fechaaprobadodg != value)
                {

                    m_fechaaprobadodg = value;

                }
            }
        }
        private Nullable<Boolean> m_aprobadodg;


        public Nullable<Boolean> aprobadodg
        {
            get { return m_aprobadodg; }
            set
            {
                if (this.m_aprobadodg != value)
                {

                    m_aprobadodg = value;

                }
            }
        }
        private String m_observacionescomplaice;


        public String observacionescomplaice
        {
            get { return m_observacionescomplaice; }
            set
            {
                if (this.m_observacionescomplaice != value)
                {

                    m_observacionescomplaice = value;

                }
            }
        }
        private String m_observacionesdg;


        public String observacionesdg
        {
            get { return m_observacionesdg; }
            set
            {
                if (this.m_observacionesdg != value)
                {

                    m_observacionesdg = value;

                }
            }
        }
        private Nullable<DateTime> m_fecha;


        public Nullable<DateTime> fecha
        {
            get { return m_fecha; }
            set
            {
                if (this.m_fecha != value)
                {

                    m_fecha = value;

                }
            }
        }

        private Nullable<Boolean> m_amec_comunicado;


        public Nullable<Boolean> amec_comunicado
        {
            get { return m_amec_comunicado; }
            set
            {
                if (this.m_amec_comunicado != value)
                {

                    m_amec_comunicado = value;

                }
            }
        }
        private Nullable<Boolean> m_comunicar_amec;


        public Nullable<Boolean> comunicar_amec
        {
            get { return m_comunicar_amec; }
            set
            {
                if (this.m_comunicar_amec != value)
                {

                    m_comunicar_amec = value;

                }
            }
        }
        private String m_observaciones;


        public String observaciones
        {
            get { return m_observaciones; }
            set
            {
                if (this.m_observaciones != value)
                {

                    m_observaciones = value;

                }
            }
        }

        private String m_area;


        public String area
        {
            get { return m_area; }
            set
            {
                if (this.m_area != value)
                {

                    m_area = value;

                }
            }
        }
        private Nullable<Int32> m_numboletines;


        public Nullable<Int32> numboletines
        {
            get { return m_numboletines; }
            set
            {
                if (this.m_numboletines != value)
                {

                    m_numboletines = value;

                }
            }
        }
        private Nullable<Single> m_PrevisionInicial;


        public Nullable<Single> PrevisionInicial
        {
            get { return m_PrevisionInicial; }
            set
            {
                if (this.m_PrevisionInicial != value)
                {
                    m_PrevisionInicial = value;

                }
            }
        }

        private Nullable<Int32> m_idconfempresa;


        public Nullable<Int32> idconfempresa
        {
            get { return m_idconfempresa; }
            set
            {
                if (this.m_idconfempresa != value)
                {
                    m_idconfempresa = value;
                }
            }
        }

        private Nullable<Boolean> m_newco;


        public Nullable<Boolean> newco
        {
            get { return m_newco; }
            set
            {
                if (this.m_newco != value)
                {

                    m_newco = value;

                }
            }
        }

        #endregion

        #region Constructores

        public DAmec()
        {
        }
        public DAmec(Int32 _idamec)
        {
            idamec = _idamec;
        }

        #endregion


        #region Converter

        public static ICollection<DAmec> ConvertToDto(DataTable dtTable)
        {
            ICollection<DAmec> collection = new Collection<DAmec>();

            foreach(DataRow row in dtTable.Rows)
            {
                DAmec data = new DAmec()
                {
                    idamec = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idamec")),
                    amec = Quodem.Utility.DataLayerUtil.GetStringValue(row, "amec"),
                    IdEmpresa = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdEmpresa")),
                    idestado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idestado"),
                    aprobado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "aprobado") == "1",
                    idpeticionario = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idpeticionario")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idpeticionario"))),
                    fechaaprobadolegal = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaaprobadolegal") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaaprobadolegal"),
                    aprobadolegal = Quodem.Utility.DataLayerUtil.GetStringValue(row, "aprobadolegal") == "1",
                    idpeticionario2 = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idpeticionario2")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idpeticionario2"))),
                    fechaaprobadocomplaice = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaaprobadocomplaice") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaaprobadocomplaice"),
                    aprobadocomplaice = Quodem.Utility.DataLayerUtil.GetStringValue(row, "aprobadocomplaice") == "1",
                    idpeticionactividad = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idpeticionactividad")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idpeticionactividad"))),
                    IdCongreso = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdCongreso")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "IdCongreso"))),
                    Idarea = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Idarea")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Idarea"))),
                    idarea1 = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idarea1")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idarea1"))),
                    idarea2 = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idarea2")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idarea2"))),
                    idarea3 = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idarea3")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idarea3"))),
                    idunidad = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idunidad")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idunidad"))),
                    idtipoactividadcongreso = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idtipoactividadcongreso")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idtipoactividadcongreso"))),
                    iddistrito = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "iddistrito")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "iddistrito"))),
                    paraguas = Quodem.Utility.DataLayerUtil.GetStringValue(row, "paraguas") == "1",
                    especificarotras = Quodem.Utility.DataLayerUtil.GetStringValue(row, "especificarotras"),
                    nombreprograma = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nombreprograma"),
                    codgenesis = Quodem.Utility.DataLayerUtil.GetStringValue(row, "codgenesis"),
                    objetivosprograma = Quodem.Utility.DataLayerUtil.GetStringValue(row, "objetivosprograma"),
                    contenido = Quodem.Utility.DataLayerUtil.GetStringValue(row, "contenido"),
                    lugar = Quodem.Utility.DataLayerUtil.GetStringValue(row, "lugar"),
                    ambitogeografico = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ambitogeografico"),
                    duracion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "duracion"),
                    numparticipantes = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "numparticipantes")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "numparticipantes"))),
                    fechacomienzop = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacomienzop") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacomienzop"),
                    fechafinp = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechafinp") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechafinp"),
                    gastosdes_aloj = Quodem.Utility.DataLayerUtil.GetStringValue(row, "gastosdes_aloj"),
                    otros = Quodem.Utility.DataLayerUtil.GetStringValue(row, "otros"),
                    relponentes = Quodem.Utility.DataLayerUtil.GetStringValue(row, "relponentes"),
                    numponentes = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "numponentes")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "numponentes"))),
                    observacioneslegal = Quodem.Utility.DataLayerUtil.GetStringValue(row, "observacioneslegal"),
                    honorarios = Quodem.Utility.DataLayerUtil.GetStringValue(row, "honorarios"),
                    conceptogastos = Quodem.Utility.DataLayerUtil.GetStringValue(row, "conceptogastos"),
                    importetotal = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "importetotal")) ? new float?() : float.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "importetotal"))),
                    numpropuestas = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "numpropuestas")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "numpropuestas"))),
                    numcartas = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "numcartas")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "numcartas"))),
                    numhojasinscripcion = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "numhojasinscripcion")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "numhojasinscripcion"))),
                    idtipopatrocinio = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idtipopatrocinio")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idtipopatrocinio"))),
                    mailautorizadofi = Quodem.Utility.DataLayerUtil.GetStringValue(row, "mailautorizadofi"),
                    ficheroprograma = Quodem.Utility.DataLayerUtil.GetStringValue(row, "ficheroprograma"),
                    locked = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "locked")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "locked"))),
                    fechaaprobadodg = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaaprobadodg") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaaprobadodg"),
                    aprobadodg = Quodem.Utility.DataLayerUtil.GetStringValue(row, "aprobadodg") == "1",
                    observacionescomplaice = Quodem.Utility.DataLayerUtil.GetStringValue(row, "observacionescomplaice"),
                    observacionesdg = Quodem.Utility.DataLayerUtil.GetStringValue(row, "observacionesdg"),
                    fecha = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fecha") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fecha"),
                    amec_comunicado = Quodem.Utility.DataLayerUtil.GetStringValue(row, "amec_comunicado") == "1",
                    comunicar_amec = Quodem.Utility.DataLayerUtil.GetStringValue(row, "comunicar_amec") == "1",
                    observaciones = Quodem.Utility.DataLayerUtil.GetStringValue(row, "observaciones"),
                    area = Quodem.Utility.DataLayerUtil.GetStringValue(row, "area"),
                    numboletines = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "numboletines")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "numboletines"))),
                    PrevisionInicial = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "PrevisionInicial")) ? new float?() : float.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "PrevisionInicial"))),
                    idconfempresa = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idconfempresa")) ? new int?() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "idconfempresa"))),
                    newco = Quodem.Utility.DataLayerUtil.GetBoolValue(row, "newco", false)
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
    }
}

