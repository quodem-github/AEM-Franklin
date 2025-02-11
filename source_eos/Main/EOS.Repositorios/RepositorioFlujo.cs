using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Common;

using EOS.Entidades;

namespace EOS.Repositorios
{
    public class RepositorioFlujo
    {
        private string _connectionString = "";

        public string idAMEC { get; set; }
        public int idUsuario { get; set; }

        public bool isAceptar { get; set; }
        public bool isRechazar { get; set; }
        public bool isSometer {get; set; }
        

        //Constructor
        //idAMEC identificador del AMEC consultado
        //idUsuario identificador del peticionario que lo abre
        public RepositorioFlujo(string idAMEC, Int32 idUsuario, string connectionString)
        {
            this._connectionString = connectionString;
            this.idAMEC = idAMEC;
            this.idUsuario = idUsuario;

            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand();
            SqlConnection conx = new SqlConnection();
            SqlDataAdapter addapter = new SqlDataAdapter();

            try {
                command.CommandText = "amecDescription";
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = conx;
                conx.ConnectionString = this._connectionString;

                command.Parameters.Add("in_idamec", SqlDbType.VarChar, 128);
                command.Parameters.Add("in_idpeticionario",SqlDbType.Int,10);
                conx.Open();
                command.Parameters["in_idamec"].Value = idAMEC;
                command.Parameters["in_idpeticionario"].Value = idUsuario;

                SqlDataAdapter slData = new SqlDataAdapter(command);
                slData.Fill(ds);

                foreach (DataRow fila in ds.Tables[0].Rows)
                {
                    //Comprobamos isSometer
                    if (fila["_isSometer"].ToString() == "1")
                    {
                        isSometer = true;
                    }
                    else
                    {
                        isSometer = false;
                    }

                    //Comprobamos isAceptar
                    if (fila["_isAceptar"].ToString() == "1")
                    {
                        isAceptar = true;
                    }
                    else
                    {
                        isAceptar = false;
                    }

                    //Comprobamos isAceptar
                    if (fila["_isRechazar"].ToString() == "1")
                    {
                        isRechazar = true;
                    }
                    else
                    {
                        isRechazar = false;
                    }
                }
                
            }
            catch (SqlException sqle)
            {
                Logger.Logger.PrintError(this.GetType().Name, "RepositorioFlujo", sqle.Message, sqle);
            }
            finally
            {
                conx.Close();
            }
        }

        

        //Someter AMEC
        public bool Someter()
        {

            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand();
            SqlConnection conx = new SqlConnection();
            SqlDataAdapter addapter = new SqlDataAdapter();

            try
            {
                command.CommandText = "amecSometer";
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = conx;
                conx.ConnectionString = this._connectionString;

                command.Parameters.Add("in_idamec", SqlDbType.VarChar, 128);
                command.Parameters.Add("in_idpeticionario", SqlDbType.Int, 10);
                conx.Open();
                command.Parameters["in_idamec"].Value = idAMEC;
                command.Parameters["in_idpeticionario"].Value = idUsuario;

                SqlDataAdapter slData = new SqlDataAdapter(command);
                slData.Fill(ds);
                
                foreach (DataRow fila in ds.Tables[0].Rows)
                {
                    //Comprobamos isSometer
                    if (fila["resultado"].ToString() == "finish")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (SqlException sqle)
            {
                Logger.Logger.PrintError(this.GetType().Name, "RepositorioFlujo.someter()", sqle.Message, sqle);
                throw sqle;
            }
            finally
            {
                conx.Close();
            }
        }

        //Aceptar AMEC
        public bool Aceptar(int esAprobacionCondicionada)
        {
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand();
            SqlConnection conx = new SqlConnection();
            SqlDataAdapter addapter = new SqlDataAdapter();

            try
            {
                command.CommandText = "amecAceptar";
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = conx;
                conx.ConnectionString = this._connectionString;

                command.Parameters.Add("in_idamec", SqlDbType.VarChar, 128);
                command.Parameters.Add("in_idpeticionario", SqlDbType.Int, 10);
                command.Parameters.Add("in_esAprobadoCondicionado", SqlDbType.Int, 1);
                conx.Open();
                command.Parameters["in_idamec"].Value = idAMEC;
                command.Parameters["in_idpeticionario"].Value = idUsuario;
                command.Parameters["in_esAprobadoCondicionado"].Value = esAprobacionCondicionada;

                SqlDataAdapter slData = new SqlDataAdapter(command);
                slData.Fill(ds);
                
                foreach (DataRow fila in ds.Tables[0].Rows)
                {
                    //Comprobamos isSometer
                    if (fila["resultado"].ToString() == "finish")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (SqlException sqle)
            {
                Logger.Logger.PrintError(this.GetType().Name, "RepositorioFlujo.aceptar()", sqle.Message, sqle);
                throw sqle;
            }
            finally
            {
                conx.Close();
            }
        }

        public List<String> ObtenerCorreoNegocioParaEnviar(string idAMEC, string connectionString)
        {
            string _connectionString = "";
            _connectionString = connectionString;
            List<string> Correos = new List<string>();
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand();
            SqlConnection conx = new SqlConnection();
            SqlDataAdapter addapter = new SqlDataAdapter();

            try
            {
                command.CommandText = "amecNotificacionesNegocio";
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = conx;
                conx.ConnectionString = _connectionString;

                command.Parameters.Add("in_idamec", SqlDbType.VarChar, 128);
                conx.Open();
                command.Parameters["in_idamec"].Value = idAMEC;

                SqlDataAdapter slData = new SqlDataAdapter(command);
                slData.Fill(ds);
                int i;
                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables.Count == 1)
                    {
                        foreach (DataRow fila in ds.Tables[0].Rows)
                        {
                            Correos.Add(fila[0].ToString());
                        }
                    }
                    else
                        for (i = 0; i < ds.Tables.Count; i++)
                        {
                            foreach (DataRow fila in ds.Tables[i].Rows)
                            {
                                Correos.Add(fila[0].ToString());
                            }
                        }
                }

            }
            catch (SqlException sqle)
            {
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", sqle.Message, sqle);
            }
            finally
            {
                conx.Close();
            }
            return Correos;
        }

        public List<String> ObtenerCorreoParaEnviar(string idAMEC, string connectionString)
        {
            string _connectionString = "";
            _connectionString = connectionString;
            List<string> Correos = new List<string>();
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand();
            SqlConnection conx = new SqlConnection();
            SqlDataAdapter addapter = new SqlDataAdapter();

            try
            {
                command.CommandText = "amecNotificaciones";
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = conx;
                conx.ConnectionString = _connectionString;

                command.Parameters.Add("in_idamec", SqlDbType.VarChar, 128);
                conx.Open();
                command.Parameters["in_idamec"].Value = idAMEC;

                SqlDataAdapter slData = new SqlDataAdapter(command);
                slData.Fill(ds);
                int i;
                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables.Count == 1)
                    {
                        foreach (DataRow fila in ds.Tables[0].Rows)
                        {
                            Correos.Add(fila[0].ToString());
                        }
                    }
                    else
                        for (i = 0; i < ds.Tables.Count; i++)
                        {
                            foreach (DataRow fila in ds.Tables[i].Rows)
                            {
                                Correos.Add(fila[0].ToString());
                            }
                        }
                }

            }
            catch (SqlException sqle)
            {
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", sqle.Message, sqle);
            }
            finally
            {
                conx.Close();
            }
            return Correos;
        }

        public InformacionExpediente IncorporarExpediente(int idexpediente)
        {
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand();
            SqlConnection conx = new SqlConnection();
            SqlDataAdapter addapter = new SqlDataAdapter();

            try
            {
                if (idexpediente == 0)
                {
                    command.CommandText = "amecExpediente";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conx;
                    conx.ConnectionString = this._connectionString;

                    command.Parameters.Add("in_idamec", SqlDbType.VarChar, 128);

                    conx.Open();
                    command.Parameters["in_idamec"].Value = idAMEC;

                    SqlDataAdapter slData = new SqlDataAdapter(command);
                    slData.Fill(ds);

                    InformacionExpediente datos = new InformacionExpediente();
                    List<DetalleServicios> lista = new List<DetalleServicios>();
                    datos.detalleservicios = lista;

                    foreach (DataRow fila in ds.Tables[0].Rows)
                    {
                        datos.importe = decimal.Parse(fila["importetotal"].ToString());
                        DetalleServicios servicio = new DetalleServicios();
                        servicio.descripcion = fila["servicio"].ToString();
                        datos.detalleservicios.Add(servicio);
                        datos.FechaComienzo = (DateTime)fila[2];
                        datos.FechaFin = (DateTime)fila[3];
                        datos.LugarRealizacion = fila[4].ToString();
                    }
                    return datos;
                }
                else
                {
                    command.CommandText = "amecParaguasExpediente";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conx;
                    conx.ConnectionString = this._connectionString;

                    command.Parameters.Add("in_idamec", SqlDbType.VarChar, 128);
                    command.Parameters.Add("in_idexpediente", SqlDbType.Int, 10);

                    conx.Open();
                    command.Parameters["in_idamec"].Value = idAMEC;
                    command.Parameters["in_idexpediente"].Value = idexpediente;

                    SqlDataAdapter slData = new SqlDataAdapter(command);
                    slData.Fill(ds);

                    InformacionExpediente datos = new InformacionExpediente();
                    List<DetalleServicios> lista = new List<DetalleServicios>();
                    datos.detalleservicios = lista;

                    foreach (DataRow fila in ds.Tables[0].Rows)
                    {
                        datos.importe = decimal.Parse(fila["importetotal"].ToString());
                        DetalleServicios servicio = new DetalleServicios();
                        servicio.descripcion = fila["servicio"].ToString();
                        datos.detalleservicios.Add(servicio);
                        datos.FechaComienzo = (DateTime)fila[2];
                        datos.FechaFin = (DateTime)fila[3];
                        datos.LugarRealizacion = fila[4].ToString();
                    }
                    return datos;
                
                }

            }
            catch (SqlException sqle)
            {
                Logger.Logger.PrintError(this.GetType().Name, "RepositorioFlujo.rechazar()", sqle.Message, sqle);
                throw sqle;
            }
            finally
            {
                conx.Close();
            }
        }

        public int PermisoVerAmec(string idamec, int idpeticionario)
        {
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand();
            SqlConnection conx = new SqlConnection();
            SqlDataAdapter addapter = new SqlDataAdapter();
            int puedeVerAmec=0;

            try
            {
                command.CommandText = "amecPermisos";
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = conx;
                conx.ConnectionString = this._connectionString;

                command.Parameters.Add("in_idamec", SqlDbType.VarChar, 128);
                command.Parameters.Add("in_idpeticionario", SqlDbType.Int, 10);
                conx.Open();
                command.Parameters["in_idamec"].Value = idamec;
                command.Parameters["in_idpeticionario"].Value = idpeticionario;

                SqlDataAdapter slData = new SqlDataAdapter(command);
                puedeVerAmec = slData.Fill(ds);
                return puedeVerAmec;

            }

            catch (SqlException sqle)
            {
                Logger.Logger.PrintError(this.GetType().Name, "RepositorioFlujo.PermisoVerAmec()", sqle.Message, sqle);
                throw sqle;
            }
            finally
            {

                conx.Close();
            }
        }
        
        //Rechazar AMEC
        public bool Rechazar(string comentario)
        {
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand();
            SqlConnection conx = new SqlConnection();
            SqlDataAdapter addapter = new SqlDataAdapter();

            try
            {
                command.CommandText = "amecRechazar";
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = conx;
                conx.ConnectionString = this._connectionString;

                command.Parameters.Add("in_idamec", SqlDbType.VarChar, 128);
                command.Parameters.Add("in_idpeticionario", SqlDbType.Int, 10);
                conx.Open();
                command.Parameters["in_idamec"].Value = idAMEC;
                command.Parameters["in_idpeticionario"].Value = idUsuario;

                SqlDataAdapter slData = new SqlDataAdapter(command);
                slData.Fill(ds);

                foreach (DataRow fila in ds.Tables[0].Rows)
                {
                    //Comprobamos isSometer
                    if (fila["resultado"].ToString() == "finish")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (SqlException sqle)
            {
                Logger.Logger.PrintError(this.GetType().Name, "RepositorioFlujo.rechazar()", sqle.Message, sqle);
                throw sqle;
            }
            finally
            {
                conx.Close();
            }
        }
    }
}
