using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DDatosPersonalesActualizarUsuarios
    {
        #region Propiedades
        private Int32 m_IdPeticionario;

        //[PropiedadOriginal("IdPeticionario", EsClave = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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

        //[PropiedadOriginal("FKIdEmpresa", TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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

        //[PropiedadOriginal("login", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
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

        //[PropiedadOriginal("password", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 40)]
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

        //[PropiedadOriginal("wein", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 12)]
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

        //[PropiedadOriginal("IdCargo", TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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

        //[PropiedadOriginal("idtratamiento", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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

        //[PropiedadOriginal("Nombre", TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 52)]
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

        //[PropiedadOriginal("Apellido1", TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 52)]
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

        //[PropiedadOriginal("Apellido2", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 52)]
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

        //[PropiedadOriginal("Direccion", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
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
        private Nullable<Int32> m_IdPoblacion;

        //[PropiedadOriginal("IdPoblacion", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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

        //[PropiedadOriginal("CodPostal", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 10)]
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

        //[PropiedadOriginal("Telefono", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 25)]
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

        //[PropiedadOriginal("extension", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
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

        //[PropiedadOriginal("Movil", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 15)]
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

        //[PropiedadOriginal("Email", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 45)]
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

        //[PropiedadOriginal("Inactivo", TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
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

        //[PropiedadOriginal("Observaciones", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NText, Longitud = 21845)]
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

        //[PropiedadOriginal("fechaprimeraccesoportal", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
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
        private String m_idempleadogp;

        //[PropiedadOriginal("idempleadogp", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 5)]
        public String idempleadogp
        {
            get { return m_idempleadogp; }
            set
            {
                if (this.m_idempleadogp != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idempleadogp", m_idempleadogp, value));
                    m_idempleadogp = value;

                }
            }
        }
        private Nullable<Int32> m_Idarea;

        //[PropiedadOriginal("Idarea", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> Idarea
        {
            get { return m_Idarea; }
            set
            {
                if (this.m_Idarea != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Idarea", m_Idarea, value));
                    m_Idarea = value;

                }
            }
        }
        private Nullable<Int32> m_idregion;

        //[PropiedadOriginal("idregion", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> idregion
        {
            get { return m_idregion; }
            set
            {
                if (this.m_idregion != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idregion", m_idregion, value));
                    m_idregion = value;

                }
            }
        }
        private Nullable<Int32> m_iddistrito;

        //[PropiedadOriginal("iddistrito", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> iddistrito
        {
            get { return m_iddistrito; }
            set
            {
                if (this.m_iddistrito != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("iddistrito", m_iddistrito, value));
                    m_iddistrito = value;

                }
            }
        }
        private Nullable<Int32> m_locked;

        //[PropiedadOriginal("locked", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
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

        //[PropiedadOriginal("atencionprimaria", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
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

        //[PropiedadOriginal("descuentoresidente", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 128)]
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
        private Nullable<Int32> m_idunidad;

        //[PropiedadOriginal("idunidad", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> idunidad
        {
            get { return m_idunidad; }
            set
            {
                if (this.m_idunidad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idunidad", m_idunidad, value));
                    m_idunidad = value;

                }
            }
        }
        private Nullable<Int32> m_idpeticionarioresponsable;

        //[PropiedadOriginal("idpeticionarioresponsable", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> idpeticionarioresponsable
        {
            get { return m_idpeticionarioresponsable; }
            set
            {
                if (this.m_idpeticionarioresponsable != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("idpeticionarioresponsable", m_idpeticionarioresponsable, value));
                    m_idpeticionarioresponsable = value;

                }
            }
        }
        //Ismael Ameller 20/04/2011 Añadimos AMEX al peticionario
        private String m_amex;

        //[PropiedadOriginal("amex", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.NVarchar2, Longitud = 20)]
        public String amex
        {
            get { return m_amex; }
            set
            {
                if (this.m_amex != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("amex", m_amex, value));
                    m_amex = value;

                }
            }
        }
        private Nullable<DateTime> m_fechacaducidad;

        //[PropiedadOriginal("fecha_caducidad", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public Nullable<DateTime> fechacaducidad
        {
            get { return m_fechacaducidad; }
            set
            {
                if (this.m_fechacaducidad != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("fechacaducidad", m_fechacaducidad, value));
                    m_fechacaducidad = value;

                }
            }
        }
        //FIN Ismael Ameller 20/04/2011 Añadimos AMEX al peticionario

        //Xavi Morell Propiedad Adminnistrador 05/09/2011
        private Nullable<Boolean> m_admin;

        //[PropiedadOriginal("administrador", EsNullable = true, TablaOriginal = "peticionarios", TipoProveedor = ClepsydraDbType.Byte, Longitud = 1)]
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
        // Fi Xavi Morell
        #endregion

        #region Constructores

        public DDatosPersonalesActualizarUsuarios()
        {
        }
        public DDatosPersonalesActualizarUsuarios(Int32 _IdPeticionario)
        {
            IdPeticionario = _IdPeticionario;
        }

        #endregion

        #region Converter

        public static ICollection<DDatosPersonalesActualizarUsuarios> ConvertToDto(DataTable dtTable)
        {
            ICollection<DDatosPersonalesActualizarUsuarios> collection = new Collection<DDatosPersonalesActualizarUsuarios>();
            foreach (DataRow row in dtTable.Rows)
            {
                DDatosPersonalesActualizarUsuarios data = new DDatosPersonalesActualizarUsuarios()
                {
                    Inactivo = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Inactivo") ==1,
                    FKIdEmpresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "FKIdEmpresa"),
                    IdCargo = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdCargo"),
                    IdPeticionario = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdPeticionario"),
                    admin = Quodem.Utility.DataLayerUtil.GetIntValue(row, "admin") ==1,
                    atencionprimaria = Quodem.Utility.DataLayerUtil.GetIntValue(row, "atencionprimaria") ==1,
                    fechacaducidad = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacaducidad") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechacaducidad"),
                    fechaprimeraccesoportal = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaprimeraccesoportal") == DateTime.MaxValue ? new Nullable<DateTime>() : Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "fechaprimeraccesoportal"),
                    Idarea = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Idarea"),
                    iddistrito = Quodem.Utility.DataLayerUtil.GetIntValue(row, "iddistrito"),
                    idpeticionarioresponsable = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idpeticionarioresponsable"),
                    IdPoblacion = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdPoblacion"),
                    idregion = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idregion"),
                    idtratamiento = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idtratamiento"),
                    idunidad = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idunidad"),
                    locked = Quodem.Utility.DataLayerUtil.GetIntValue(row, "locked"),
                    amex = Quodem.Utility.DataLayerUtil.GetStringValue(row, "amex"),
                    Apellido1 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Apellido1"),
                    Apellido2 = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Apellido2"),
                    CodPostal = Quodem.Utility.DataLayerUtil.GetStringValue(row, "CodPostal"),
                    descuentoresidente = Quodem.Utility.DataLayerUtil.GetStringValue(row, "descuentoresidente"),
                    Direccion = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Direccion"),
                    Email = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Email"),
                    extension = Quodem.Utility.DataLayerUtil.GetStringValue(row, "extension"),
                    idempleadogp = Quodem.Utility.DataLayerUtil.GetStringValue(row, "idempleadogp"),
                    login = Quodem.Utility.DataLayerUtil.GetStringValue(row, "login"),
                    Movil = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Movil"),
                    Nombre = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Nombre"),
                    Observaciones = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Observaciones"),
                    password = Quodem.Utility.DataLayerUtil.GetStringValue(row, "password"),
                    Telefono = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Telefono"),
                    wein = Quodem.Utility.DataLayerUtil.GetStringValue(row, "wein"),


                };
                collection.Add(data);
            }
            return collection;
        }

        #endregion
    }
}
