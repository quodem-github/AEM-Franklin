using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace EOS.Entidades.Datos
{
    /// <summary>
    /// Clase que mapea la vista cv_peticionarios_manager. Utilizada para conocer el superior jerárquico de un peticionario
    /// </summary>
    public class DPeticionarioManager
    {
        #region Propiedades
        private Int32 m_IdPeticionario;

        //[PropiedadOriginal("IdPeticionario", EsClave = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IdPeticionario
        {
            get { return m_IdPeticionario; }
            set
            {
                if (this.m_IdPeticionario != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdPeticionario", m_IdPeticionario, value));
                    m_IdPeticionario = value;

                }
            }
        }
        private Int32 m_FKIdEmpresa;

        //[PropiedadOriginal("FKIdEmpresa", TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 FKIdEmpresa
        {
            get { return m_FKIdEmpresa; }
            set
            {
                if (this.m_FKIdEmpresa != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FKIdEmpresa", m_FKIdEmpresa, value));
                    m_FKIdEmpresa = value;

                }
            }
        }
        private String m_login;

        //[PropiedadOriginal("login", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        public String login
        {
            get { return m_login; }
            set
            {
                if (this.m_login != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("login", m_login, value));
                    m_login = value;

                }
            }
        }
        private String m_password;

        //[PropiedadOriginal("password", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 40)]
        public String password
        {
            get { return m_password; }
            set
            {
                if (this.m_password != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("password", m_password, value));
                    m_password = value;

                }
            }
        }
        private String m_wein;

        //[PropiedadOriginal("wein", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 12)]
        public String wein
        {
            get { return m_wein; }
            set
            {
                if (this.m_wein != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("wein", m_wein, value));
                    m_wein = value;

                }
            }
        }
        private Int32 m_IdCargo;

        //[PropiedadOriginal("IdCargo", TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Int32 IdCargo
        {
            get { return m_IdCargo; }
            set
            {
                if (this.m_IdCargo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdCargo", m_IdCargo, value));
                    m_IdCargo = value;

                }
            }
        }
        private Nullable<Int32> m_idtratamiento;

        //[PropiedadOriginal("idtratamiento", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> idtratamiento
        {
            get { return m_idtratamiento; }
            set
            {
                if (this.m_idtratamiento != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idtratamiento", m_idtratamiento, value));
                    m_idtratamiento = value;

                }
            }
        }
        
        private String m_Nombre;

        //[PropiedadOriginal("Nombre", TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 52)]
        public String Nombre
        {
            get { return m_Nombre; }
            set
            {
                if (this.m_Nombre != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Nombre", m_Nombre, value));
                    m_Nombre = value;

                }
            }
        }
        private String m_Apellido1;

        //[PropiedadOriginal("Apellido1", TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 52)]
        public String Apellido1
        {
            get { return m_Apellido1; }
            set
            {
                if (this.m_Apellido1 != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Apellido1", m_Apellido1, value));
                    m_Apellido1 = value;

                }
            }
        }
        private String m_Apellido2;

        //[PropiedadOriginal("Apellido2", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 52)]
        public String Apellido2
        {
            get { return m_Apellido2; }
            set
            {
                if (this.m_Apellido2 != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Apellido2", m_Apellido2, value));
                    m_Apellido2 = value;

                }
            }
        }
        private String m_Direccion;

        //[PropiedadOriginal("Direccion", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public String Direccion
        {
            get { return m_Direccion; }
            set
            {
                if (this.m_Direccion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Direccion", m_Direccion, value));
                    m_Direccion = value;

                }
            }
        }

        private int m_Departamento;

        //[PropiedadOriginal("Departamento", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public int Departamento
        {
            get { return m_Departamento; }
            set
            {
                if (this.m_Departamento != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Departamento", m_Departamento, value));
                    m_Departamento = value;

                }
            }
        }

        private int m_Distrito;

        //[PropiedadOriginal("Distrito", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public int Distrito
        {
            get { return m_Distrito; }
            set
            {
                if (this.m_Distrito != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Distrito", m_Distrito, value));
                    m_Distrito = value;

                }
            }
        }

        private int m_FuerzaVentas;

        //[PropiedadOriginal("FuerzaVentas", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public int FuerzaVentas
        {
            get { return m_FuerzaVentas; }
            set
            {
                if (this.m_FuerzaVentas != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FuerzaVentas", m_FuerzaVentas, value));
                    m_FuerzaVentas = value;

                }
            }
        }

        private Nullable<Int32> m_IdPoblacion;

        //[PropiedadOriginal("IdPoblacion", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> IdPoblacion
        {
            get { return m_IdPoblacion; }
            set
            {
                if (this.m_IdPoblacion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdPoblacion", m_IdPoblacion, value));
                    m_IdPoblacion = value;

                }
            }
        }
        private String m_CodPostal;

        //[PropiedadOriginal("CodPostal", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 10)]
        public String CodPostal
        {
            get { return m_CodPostal; }
            set
            {
                if (this.m_CodPostal != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("CodPostal", m_CodPostal, value));
                    m_CodPostal = value;

                }
            }
        }
        private String m_Telefono;

        //[PropiedadOriginal("Telefono", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 25)]
        public String Telefono
        {
            get { return m_Telefono; }
            set
            {
                if (this.m_Telefono != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Telefono", m_Telefono, value));
                    m_Telefono = value;

                }
            }
        }
        private String m_extension;

        //[PropiedadOriginal("extension", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        public String extension
        {
            get { return m_extension; }
            set
            {
                if (this.m_extension != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("extension", m_extension, value));
                    m_extension = value;

                }
            }
        }
        private String m_Movil;

        //[PropiedadOriginal("Movil", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 15)]
        public String Movil
        {
            get { return m_Movil; }
            set
            {
                if (this.m_Movil != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Movil", m_Movil, value));
                    m_Movil = value;

                }
            }
        }
        private String m_Email;

        //[PropiedadOriginal("Email", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 80)]
        public String Email
        {
            get { return m_Email; }
            set
            {
                if (this.m_Email != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Email", m_Email, value));
                    m_Email = value;

                }
            }
        }
        private Boolean m_Inactivo;

        //[PropiedadOriginal("Inactivo", TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Boolean Inactivo
        {
            get { return m_Inactivo; }
            set
            {
                if (this.m_Inactivo != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Inactivo", m_Inactivo, value));
                    m_Inactivo = value;

                }
            }
        }
        private String m_Observaciones;

        //[PropiedadOriginal("Observaciones", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NText, Longitud = 21845)]
        public String Observaciones
        {
            get { return m_Observaciones; }
            set
            {
                if (this.m_Observaciones != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Observaciones", m_Observaciones, value));
                    m_Observaciones = value;

                }
            }
        }
        private Nullable<DateTime> m_fechaprimeraccesoportal;

        //[PropiedadOriginal("fechaprimeraccesoportal", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public Nullable<DateTime> fechaprimeraccesoportal
        {
            get { return m_fechaprimeraccesoportal; }
            set
            {
                if (this.m_fechaprimeraccesoportal != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fechaprimeraccesoportal", m_fechaprimeraccesoportal, value));
                    m_fechaprimeraccesoportal = value;

                }
            }
        }
        
       
        private Nullable<Int32> m_locked;

        //[PropiedadOriginal("locked", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> locked
        {
            get { return m_locked; }
            set
            {
                if (this.m_locked != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("locked", m_locked, value));
                    m_locked = value;

                }
            }
        }
        private Nullable<Boolean> m_atencionprimaria;

        //[PropiedadOriginal("atencionprimaria", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> atencionprimaria
        {
            get { return m_atencionprimaria; }
            set
            {
                if (this.m_atencionprimaria != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("atencionprimaria", m_atencionprimaria, value));
                    m_atencionprimaria = value;

                }
            }
        }
        private String m_descuentoresidente;

        //[PropiedadOriginal("descuentoresidente", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
        public String descuentoresidente
        {
            get { return m_descuentoresidente; }
            set
            {
                if (this.m_descuentoresidente != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("descuentoresidente", m_descuentoresidente, value));
                    m_descuentoresidente = value;

                }
            }
        }

       
        private Nullable<Boolean> m_admin;

        //[PropiedadOriginal("administrador", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
        public Nullable<Boolean> admin
        {
            get { return m_admin; }
            set
            {
                if (this.m_admin != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("administrador", m_admin, value));
                    m_admin = value;

                }
            }
        }

        private String m_weinmanager;
        //[PropiedadOriginal("WeinManager", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 11)]
        public String weinmanager
        {
            get { return m_weinmanager; }
            set
            {
                if (this.m_weinmanager != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("WeinManager", m_weinmanager, value));
                    m_weinmanager = value;

                }
            }
        }
        private String m_district;
        //[PropiedadOriginal("WeinManager", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 11)]
        public String district
        {
            get { return m_district; }
            set
            {
                if (this.m_district != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("WeinManager", m_weinmanager, value));
                    m_district = value;

                }
            }
        }
        private String m_saleforce;
        //[PropiedadOriginal("WeinManager", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 11)]
        public String saleforce
        {
            get { return m_saleforce; }
            set
            {
                if (this.m_saleforce != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("WeinManager", m_weinmanager, value));
                    m_saleforce = value;

                }
            }
        }
        private String m_departament;
        //[PropiedadOriginal("WeinManager", EsNullable = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 11)]
        public String departament
        {
            get { return m_departament; }
            set
            {
                if (this.m_departament != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("WeinManager", m_weinmanager, value));
                    m_departament = value;

                }
            }
        }


        private Nullable<Int32> m_idmanager;

        //[PropiedadOriginal("IdManager", EsClave = true, TablaOriginal = "cv_peticionario_manager", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> idmanager
        {
            get { return m_idmanager; }
            set
            {
                if (this.m_idmanager != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("IdManager", m_idmanager, value));
                    m_idmanager = value;

                }
            }
        }

        //Dawid Mateusz Lizurej (Ole ole)
        #endregion

        #region Constructores

        public DPeticionarioManager()
        {

        }

        #endregion

        #region Converter

        public static ICollection<DPeticionarioManager> ConvertToDto(DataTable dtTable)
        {
            ICollection<DPeticionarioManager> collection = new Collection<DPeticionarioManager>();
            foreach (DataRow row in dtTable.Rows)
            {
                DPeticionarioManager data = new DPeticionarioManager()
                {
                    IdPeticionario = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdPeticionario"),
                    FKIdEmpresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "FKIdEmpresa"),
                    login = Quodem.Utility.DataLayerUtil.GetStringValue(row, "login"),
                    password = Quodem.Utility.DataLayerUtil.GetStringValue(row, "password"),
                    wein = Quodem.Utility.DataLayerUtil.GetStringValue(row, "wein"),
                    IdCargo = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdCargo"),
                    idtratamiento = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idtratamiento"),
                    Nombre = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Nombre"),
                    Apellido1 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Apellido1"),
                    Apellido2 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Apellido2"),
                    Direccion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Direccion"),
                    departament = Quodem.Utility.DataLayerUtil.GetStringValue(row, "departament"),
                    district = Quodem.Utility.DataLayerUtil.GetStringValue(row, "district"),
                    saleforce = Quodem.Utility.DataLayerUtil.GetStringValue(row, "saleforce"),
                    IdPoblacion = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdPoblacion"),
                    CodPostal = Quodem.Utility.DataLayerUtil.GetStringValue(row, "CodPostal"),
                    Telefono = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Telefono"),
                    extension = Quodem.Utility.DataLayerUtil.GetStringValue(row, "extension"),
                    Movil = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Movil"),
                    Email = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Email"),
                    Inactivo = (string.IsNullOrEmpty(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Inactivo")) ? new Nullable<int>() : int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Inactivo"))) == 0,
                    Observaciones = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Observaciones"),
                    fechaprimeraccesoportal = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaprimeraccesoportal") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaprimeraccesoportal"),
                    locked = Quodem.Utility.DataLayerUtil.GetIntValue(row, "locked"),
                    atencionprimaria = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "atencionprimaria")) == 1,
                    descuentoresidente = Quodem.Utility.DataLayerUtil.GetStringValue(row, "descuentoresidente"),
                    admin = int.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "administrador")) == 1,
                    weinmanager = Quodem.Utility.DataLayerUtil.GetStringValue(row, "WeinManager"),
                    idmanager = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdPeticionarioManager"),
                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion

    }
}
