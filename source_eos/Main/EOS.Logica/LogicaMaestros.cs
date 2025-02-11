using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using EOS.Repositorios;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Entidades.Maestros;

namespace EOS.Logica
{
    public class LogicaMaestros : ILogicaMaestros
    {
        IRepositorioMaestros MiRepositorioMaestros { get; set; }

        public LogicaMaestros()
        {
            MiRepositorioMaestros = new RepositorioMaestros();
        }

        public bool CambiarContraseña(string usuario, string password, string nuevaPassword)
        {
            throw new NotImplementedException();
        }
        
        public IList<DEstadoExpediente> ObtenerEstadosExpedientes()
        {
            return MiRepositorioMaestros.ObtenerEstadosExpedientes();
        }
        

        public IList<DTipoReserva> ObtenerTiposReserva()
        {
            return MiRepositorioMaestros.ObtenerTiposReserva();
        }

        public List<DDocType> GetDocType()
        {
            return MiRepositorioMaestros.GetDocType();
        }
        public List<DDocSubType> GetDocSubType()
        {
            return MiRepositorioMaestros.GetDocSubType();
        }
        public IList<DPoblacion> ObtenerPoblaciones(int idconfempresa)
        {
            return MiRepositorioMaestros.ObtenerPoblaciones(idconfempresa);
        }

        public IList<DVPoblacion> ObtenerPoblacionesTodas(int idconfempresa)
        {
            return MiRepositorioMaestros.ObtenerPoblacionesTodas(idconfempresa);
        }
        public IList<DVPais> ObtenerPaisesTodos()
        {
            return MiRepositorioMaestros.ObtenerPaisesTodos();
        }

        public IList<DVPoblacion> ObtenerPoblacionesTodasActivasInactivas(int idconfempresa)
        {
            return MiRepositorioMaestros.ObtenerPoblacionesTodasActivasInactivas(idconfempresa);
        }

        public IList<DTipoActividad> ObtenerTiposActividad(bool? isAdmin, bool? newco)
        {
            return MiRepositorioMaestros.ObtenerTiposActividad(isAdmin, newco);
        }

        public IList<DCentroCoste> ObtenerCentrosCoste()
        {
            return MiRepositorioMaestros.ObtenerCentrosCoste();
        }

        public IList<DTipoActividadCongreso> ObtenerTiposActividadCongreso()
        {
            return MiRepositorioMaestros.ObtenerTiposActividadCongreso();
        }

        public IList<DTipoPatrocinio> ObtenerTiposPatrocinio()
        {
            return MiRepositorioMaestros.ObtenerTiposPatrocinio();
        }

        public IList<DTipoEspecialidad> ObtenerTiposEspecialidades()
        {
            return MiRepositorioMaestros.ObtenerTiposEspecialidades();
        }

        //Ismael Ameller 08-03-2011 Cambio a la hora de rellenar el combo para que no salga evento-multidisciplinar
        public IList<DTipoEspecialidad> ObtenerTiposEspecialidadesNP()
        {
            return MiRepositorioMaestros.ObtenerTiposEspecialidadesNP();
        }
        //FIN Ismael Ameller 08-03-2011 Cambio a la hora de rellenar el combo para que no salga evento-multidisciplinar

        public IList<DVEmpresa> ObtenerEmpresas()
        {
            return MiRepositorioMaestros.ObtenerEmpresas();
        }

        public DEmpresaConf ObtenerEmpresaConf(int nIDEmpresa)
        {
            return MiRepositorioMaestros.ObtenerEmpresaConf(nIDEmpresa);
        }

        public IList<DDistrito> ObtenerDistritos()
        {
            return MiRepositorioMaestros.ObtenerDistritos();
        }

        public IList<DTratamiento> ObtenerTratamientos()
        {
            return MiRepositorioMaestros.ObtenerTratamientos();
        }

        public IList<DUnidad> ObtenerUnidades()
        {
            return MiRepositorioMaestros.ObtenerUnidades();
        }
        
        public IList<DRegion> ObtenerRegiones()
        {
            return MiRepositorioMaestros.ObtenerRegiones();
        }
                
        public IList<DArea> ObtenerAreas()
        {
            return MiRepositorioMaestros.ObtenerAreas();
        }

        

        public IList<DCriterioSeleccion> ObtenerCriteriosSeleccion()
        {
            return MiRepositorioMaestros.ObtenerCriteriosSeleccion();
        }

        public IList<DVCongresos> ObtenerCongresos()
        {
            return MiRepositorioMaestros.ObtenerCongresos();
        }

        public bool ActualizarDatosCompliance(DEmpresaConf datos)
        {
            string consulta = string.Format("update confempresa set mailfromcomunicfi = '{1}', mailtocomunicfi = '{2}', mailcccomunicfi = '{3}', mailtorespuesta = '{4}', formatoprgmailcomunicfi = '{5}', asuntomailcomunicfi = '{6}', cuerpomailcomunicfi = '{7}' where idconfempresa = '{0}'", datos.idconfempresa, datos.mailfromcomunicfi, datos.mailtocomunicfi, datos.mailcccomunicfi, datos.mailtorespuesta, datos.formatoprgmailcomunicfi, datos.asuntomailcomunicfi, datos.cuerpomailcomunicfi);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta) > 0;
        }

        //Ismael Ameller 04-03-2011 Saber si es delegado
        public bool UserEsDelegado(int idCargo)
        {
            return MiRepositorioMaestros.UserEsDelegado(idCargo);
        }
        //FIN Ismael Ameller 04-03-2011 Saber si es delegado

        //Ismael Ameller 22-03-2011 Control de productos
        public IList<DVProductos> ObtenerProductos(int? idAread)
        {
            return MiRepositorioMaestros.ObtenerProductos(idAread);
        }
        //FIN Ismael Ameller 22-03-2011 Control de productos

        public IList<ReservasInfo> obtenerInfoReserva(int IdExp)
        {
            return MiRepositorioMaestros.obtenerInfoReserva(IdExp);
        }

        public string ObtenerMailEmpleadoGt(string idempleadogp, int idconfempresa)
        {
            return MiRepositorioMaestros.ObtenerMailEmpleadoGt(idempleadogp, idconfempresa);
        }

        //marta mestre
        public IList<DArea> ObtenerAreasXidunidad(int idunidad)
        {
            return MiRepositorioMaestros.ObtenerAreasXidunidad(idunidad);
        }

        public IList<DRegion> ObtenerRegionesXidunidad(int idunidad)
        {
            return MiRepositorioMaestros.ObtenerRegionesXidunidad(idunidad);
        }

        public IList<DDistrito> ObtenerDistritosFiltrado(int idarea,int idregion)
        {
            return MiRepositorioMaestros.ObtenerDistritosFiltrado(idarea,idregion);
        }

        public bool ValidarReservaEstadoAmec(int idservicioReserva)
        {
            return MiRepositorioMaestros.ValidarReservaEstadoAmec(idservicioReserva);
        }

        public bool ValidarReservaImporteAmec(int idservicioReserva)
        {
            return MiRepositorioMaestros.ValidarReservaImporteAmec(idservicioReserva);
        }
                

        //marta mestre
        //JLV --------110512--------start
        //Obtener el idservicio a partir del idreserva
        public int GetServicioByReserva(int idReserva)
        {
            int respuesta = MiRepositorioMaestros.GetServicioByReserva(idReserva);
            return respuesta;
        }

        public IList<DEmpresaConf> ObtenerEmpresasConf()
        {
            return MiRepositorioMaestros.ObtenerEmpresasConf();
        }
        //JLV --------110512--------end

        public List<DDistrict> GetDistrict()
        {
            return MiRepositorioMaestros.GetDistrict();
        }

        public List<DSaleForce> GetSaleForce()
        {
            return MiRepositorioMaestros.GetSaleForce();
        }

        public List<DDepartament> GetDepartaments()
        {
            return MiRepositorioMaestros.GetDepartaments();
        }


    }
}
