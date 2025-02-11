using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;


namespace EOS.Entidades.Datos
{
    public class DVTodosAmecs
    {
        #region Propiedades
        private Int32 m_idamec;

        //[PropiedadOriginal("IDAMEC" ,EsClave = true  , TablaOriginal = "" , TipoProveedor = ClepsydraDbType.Int32 , Longitud = 11)]
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

        private Nullable<Decimal> m_Importe;

        //[PropiedadOriginal("Importe", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Decimal, Longitud = 11)]
        public Nullable<Decimal> Importe
        {
            get { return m_Importe; }
            set
            {
                if (this.m_Importe != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Importe", m_Importe, value));
                    m_Importe = value;

                }
            }
        }

        private Nullable<Int32> m_IdPeticionario;

        //[PropiedadOriginal("IdPeticionario", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Int32, Longitud = 11)]
        public Nullable<Int32> IdPeticionario
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

        private Nullable<DateTime> m_FechaAMEC;

        //[PropiedadOriginal("FechaAMEC", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.DateTime, Longitud = 19)]
        public Nullable<DateTime> FechaAMEC
        {
            get { return m_FechaAMEC; }
            set
            {
                if (this.m_FechaAMEC != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("FechaAMEC", m_FechaAMEC, value));
                    m_FechaAMEC = value;

                }
            }
        }

        private Nullable<SByte> m_Aprobado;

        //[PropiedadOriginal("Aprobado", EsNullable = true, TablaOriginal = "", TipoProveedor = ClepsydraDbType.Byte, Longitud = 4)]
        public Nullable<SByte> Aprobado
        {
            get { return m_Aprobado; }
            set
            {
                if (this.m_Aprobado != value)
                {
                    //OnNotificaCambiosPropiedad(new NotificaCambiosPropiedadEventArgs("Aprobado", m_Aprobado, value));
                    m_Aprobado = value;

                }
            }
        }



        #endregion

        #region Constructores

        public DVTodosAmecs()
        {
        }


        #endregion

        #region Converter

        public static ICollection<DVTodosAmecs> ConvertToDto(DataTable dtTable)
        {
            ICollection<DVTodosAmecs> collection = new Collection<DVTodosAmecs>();
            foreach (DataRow row in dtTable.Rows)
            {
                DVTodosAmecs data = new DVTodosAmecs()
                {
                    idamec = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idamec"),
                    FechaAMEC = Quodem.Utility.DataLayerUtil.GetDateTimeValue(row, "FechaAMEC"),
                    Importe = Quodem.Utility.DataLayerUtil.GetIntValue(row, "Importe"),
                    IdPeticionario = Quodem.Utility.DataLayerUtil.GetIntValue(row, "IdPeticionario"),
                    Aprobado = SByte.Parse(Quodem.Utility.DataLayerUtil.GetStringValue(row, "Aprobado")),

                };

                collection.Add(data);

            }
            return collection;
        }

        #endregion
    }
}

