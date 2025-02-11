using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
	public class DEmpleadosGP
	{
        #region Propiedades

        private Int32 m_id;

        public Int32 id
        {
            get { return m_id; }
            set
            {
                if (this.m_id != value)
                {
                    m_id = value;

                }
            }
        }

        private Int32 m_idconfempresa;

        public Int32 idconfempresa
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


        private String m_nombreagencia;

        public String nombreagencia
        {
            get { return m_nombreagencia; }
            set
            {
                if (this.m_nombreagencia != value)
                {
                    m_nombreagencia = value;

                }
            }
        }

        private String m_Idempleadogp;

        public String Idempleadogp
        {
            get { return m_Idempleadogp; }
            set
            {
                if (this.m_Idempleadogp != value)
                {
                    m_Idempleadogp = value;

                }
            }
        }

        private String m_logosuperiorpantalla;

        public String logosuperiorpantalla
        {
            get { return m_logosuperiorpantalla; }
            set
            {
                if (this.m_logosuperiorpantalla != value)
                {
                    m_logosuperiorpantalla = value;

                }
            }
        }

        private String m_Nombre;

        public String Nombre
        {
            get { return m_Nombre; }
            set
            {
                if (this.m_Nombre != value)
                {
                    m_Nombre = value;

                }
            }
        }
        private String m_Apellido;

        public String Apellido
        {
            get { return m_Apellido; }
            set
            {
                if (this.m_Apellido != value)
                {
                    m_Apellido = value;

                }
            }
        }
        private String m_Email;

        public String Email
        {
            get { return m_Email; }
            set
            {
                if (this.m_Email != value)
                {
                    m_Email = value;

                }
            }
        }
        #endregion

        #region Constructores

        public DEmpleadosGP()
		{
		}
		public DEmpleadosGP(String _Idempleadogp)
		{
			Idempleadogp = _Idempleadogp;
		}

		#endregion

        #region Converter

        public static ICollection<DEmpleadosGP> ConvertToDto(DataTable dtTable)
        {
            ICollection<DEmpleadosGP> collection = new Collection<DEmpleadosGP>();

            foreach (DataRow row in dtTable.Rows)
            {
                DEmpleadosGP data = new DEmpleadosGP() {
                    id = Quodem.Utility.DataLayerUtil.GetIntValue(row, "id"),
                    idconfempresa = Quodem.Utility.DataLayerUtil.GetIntValue(row, "idconfempresa"),
                    nombreagencia = Quodem.Utility.DataLayerUtil.GetStringValue(row, "nombreagencia"),
                    Idempleadogp = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Idempleadogp"),
                    logosuperiorpantalla = Quodem.Utility.DataLayerUtil.GetStringValue(row, "logosuperiorpantalla"),
                    Nombre = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Nombre"),
                    Apellido = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Apellido"),
                    Email = Quodem.Utility.DataLayerUtil.GetStringValue(row, "Email")
                };

                collection.Add(data);
            }

            return collection;
        }

        #endregion
	}
}
