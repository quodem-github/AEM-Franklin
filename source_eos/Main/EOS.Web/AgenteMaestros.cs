using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Logica;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Entidades.Maestros;

namespace EOS.Web
{
    public class AgenteMaestros : AgenteBase
    {
        public const string COLECCION_ESTADOS = "ColeccionEstados";
        public const string COLECCION_TIPOSRESERVA = "ColeccionTiposReserva";
        public const string COLECCION_POBLACIONES = "ColeccionPoblacines";
        public const string COLECCION_POBLACIONES_TODAS = "ColeccionPoblacinesTodas";
        public const string COLECCION_TIPOACTIVIDAD = "ColeccionTipoActividad";
        public const string COLECCION_TIPOACTIVIDADCONGRESO = "ColeccionTipoActividadCongreso";
        public const string COLECCION_TIPOPATROCINIO = "ColeccionTipoPatrocinio";
        public const string COLECCION_TIPOESPECIALIDAD = "ColeccionTipoEspecialidad";
        public const string COLECCION_EMPRESAS = "ColeccionEmpresas";
        public const string COLECCION_EMPRESAS_CONF = "ColeccionEmpresasConf";
        public const string COLECCION_DISTRITOS = "ColeccionDistritos";
        public const string COLECCION_TRATAMIENTOS = "ColeccionTratamientos";
        public const string COLECCION_REGIONES = "ColeccionRegiones";
        public const string COLECCION_UNIDADES = "ColeccionUnidades";
        public const string COLECCION_AREAS = "ColeccionAreas";
        public const string COLECCION_CRITERIOS_SELECCION = "ColeccionCriteriosSeleccion";
        public const string COLECCION_CONGRESOS = "ColeccionCongresos";

        public LogicaMaestros MiLogicaMaestros { get; set; }

        public AgenteMaestros()
        {
            MiLogicaMaestros = new LogicaMaestros();
        }

        public IEnumerable<DEstadoExpediente> ObtenerEstadosExpedientes(string porDefecto)
        {
            IList<DEstadoExpediente> listaSalida =
                ObtenerDesdeCacheORepositorio<DEstadoExpediente>(COLECCION_ESTADOS, MiLogicaMaestros.ObtenerEstadosExpedientes).ToList();

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DEstadoExpediente { idestado = null, estado = porDefecto });
            }
            return listaSalida;
        }


        public IEnumerable<DVPoblacion> ObtenerPoblacionesTodasActivasInactivas(string porDefecto, int idconfempresa)
        {
            IList<DVPoblacion> listaSalida = MiLogicaMaestros.ObtenerPoblacionesTodasActivasInactivas(idconfempresa).ToList();

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DVPoblacion() { IdPoblacion = -1, Poblacion = porDefecto });
            }

            return listaSalida;

        }

        public IEnumerable<DVPoblacion> ObtenerPoblacionesTodas(string porDefecto, int idconfempresa)
        {
            IList<DVPoblacion> listaSalida = MiLogicaMaestros.ObtenerPoblacionesTodas(idconfempresa).ToList();
            //ObtenerDesdeCacheORepositorio<DVPoblacion>(COLECCION_POBLACIONES_TODAS, MiLogicaMaestros.ObtenerPoblacionesTodas).ToList();

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DVPoblacion() { IdPoblacion = -1, Poblacion = porDefecto });
            }

            return listaSalida;

        }

        public IEnumerable<DVPais> ObtenerPaisesTodos(string porDefecto, int idconfempresa)
        {
            IList<DVPais> listaSalida = MiLogicaMaestros.ObtenerPaisesTodos().ToList();

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DVPais() { IdPais = "", Pais = porDefecto });
            }

            return listaSalida;

        }

        public IEnumerable<DPoblacion> ObtenerPoblaciones(string porDefecto, int idconfempresa)
        {
            IList<DPoblacion> listaSalida = MiLogicaMaestros.ObtenerPoblaciones(idconfempresa).ToList(); 

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DPoblacion() { IdPoblacion = -1, Poblacion = porDefecto });
            }

            return listaSalida;

        }

        public IList<DEmpresaConf> ObtenerEmpresasConf()
        {
            IList<DEmpresaConf> listaSalida =
                ObtenerDesdeCacheORepositorio<DEmpresaConf>(COLECCION_EMPRESAS_CONF, MiLogicaMaestros.ObtenerEmpresasConf, true).ToList();

            return listaSalida;
        }

        public IList<DDistrito> ObtenerDistritos(string porDefecto)
        {
            IList<DDistrito> listaSalida =
                ObtenerDesdeCacheORepositorio<DDistrito>(COLECCION_DISTRITOS, MiLogicaMaestros.ObtenerDistritos, true).ToList();

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DDistrito() { iddistrito = -1, distrito = porDefecto });
            }

            return listaSalida;
        }

        public IList<DTratamiento> ObtenerTratamientos(string porDefecto)
        {
            IList<DTratamiento> listaSalida =
                ObtenerDesdeCacheORepositorio<DTratamiento>(COLECCION_TRATAMIENTOS, MiLogicaMaestros.ObtenerTratamientos).ToList();

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DTratamiento() { idtratamiento = -1, ttomsd = porDefecto });
            }

            return listaSalida;

        }
        public List<DDocType> GetDocType()
        {
            return MiLogicaMaestros.GetDocType();
        }
        public List<DDocSubType> GetDocSubType()
        {
            return MiLogicaMaestros.GetDocSubType();
        }
        public IEnumerable<DEstadoExpediente> ObtenerEstadosReservas(string porDefecto)
        {
            return ObtenerEstadosExpedientes(porDefecto);
        }
        //Ismael Ameller 04-03-2011 Saber si es delegado
        public bool UserEsDelegado(int idCargo)
        {
            return MiLogicaMaestros.UserEsDelegado(idCargo);
        }
        //FIN Ismael Ameller 04-03-2011 Saber si es delegado
        public IEnumerable<DTipoReserva> ObtenerTiposReservas(string porDefecto)
        {
            IList<DTipoReserva> listaSalida =
                ObtenerDesdeCacheORepositorio<DTipoReserva>(COLECCION_TIPOSRESERVA, MiLogicaMaestros.ObtenerTiposReserva).ToList();

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DTipoReserva() { IdTipoReserva = -1, Descripcion = porDefecto });
            }

            return listaSalida;
        }

        public IEnumerable<DTipoActividad> ObtenerTiposActividad(string porDefecto, bool? isAdmin, bool? newco)
        {
            IList<DTipoActividad> listaSalida = MiLogicaMaestros.ObtenerTiposActividad(isAdmin, newco).ToList();

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DTipoActividad() { IdTipoActividad = -1, Nombre = porDefecto });
            }
            return listaSalida;
        }

        public IEnumerable<DCentroCoste> ObtenerCentrosCoste(string porDefecto)
        {
            IList<DCentroCoste> listaSalida = MiLogicaMaestros.ObtenerCentrosCoste().ToList();

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DCentroCoste() { IdCentroCoste = -1, CentroCoste = porDefecto });
            }
            return listaSalida;
        }

        public IEnumerable<DTipoActividadCongreso> ObtenerTiposActividadCongreso(string porDefecto)
        {
            IList<DTipoActividadCongreso> lista =
                ObtenerDesdeCacheORepositorio<DTipoActividadCongreso>(COLECCION_TIPOACTIVIDADCONGRESO, MiLogicaMaestros.ObtenerTiposActividadCongreso).ToList();

            List<DTipoActividadCongreso> listaSalida = new List<DTipoActividadCongreso>(lista);

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DTipoActividadCongreso() { idtipoactividadcongreso = -1, tipoactividadcongreso = porDefecto });
            }
            return listaSalida;
        }

        public IEnumerable<DTipoPatrocinio> ObtenerTiposPatrocinio(string porDefecto)
        {
            IList<DTipoPatrocinio> lista =
                ObtenerDesdeCacheORepositorio<DTipoPatrocinio>(COLECCION_TIPOPATROCINIO, MiLogicaMaestros.ObtenerTiposPatrocinio).ToList();

            List<DTipoPatrocinio> listaSalida = new List<DTipoPatrocinio>(lista);

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DTipoPatrocinio() { idtipopatrocinio = -1, tipopatrocinio = porDefecto });
            }
            return listaSalida;
        }

        public IEnumerable<DTipoEspecialidad> ObtenerTiposEspecialidades(string porDefecto)
        {
            IList<DTipoEspecialidad> listaSalida =
                ObtenerDesdeCacheORepositorio<DTipoEspecialidad>(COLECCION_TIPOESPECIALIDAD, MiLogicaMaestros.ObtenerTiposEspecialidades).ToList();

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DTipoEspecialidad() { IdEspecialidad = -1, CodEspecialidad = "", Especialidad = porDefecto });
            }
            return listaSalida;
        }
        //Ismael Ameller 08-03-2011 Cambio a la hora de rellenar el combo para que no salga evento-multidisciplinar
        public IEnumerable<DTipoEspecialidad> ObtenerTiposEspecialidadesNP(string porDefecto)
        {
            IList<DTipoEspecialidad> listaSalida =
                ObtenerDesdeCacheORepositorio<DTipoEspecialidad>(COLECCION_TIPOESPECIALIDAD, MiLogicaMaestros.ObtenerTiposEspecialidadesNP).ToList();

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DTipoEspecialidad() { IdEspecialidad = -1, CodEspecialidad = "", Especialidad = porDefecto });
            }
            return listaSalida;
        }
        //FIN Ismael Ameller 08-03-2011 Cambio a la hora de rellenar el combo para que no salga evento-multidisciplinar
        public IEnumerable<DVEmpresa> ObtenerEmpresas(string porDefecto)
        {
            IList<DVEmpresa> listaSalida =
                ObtenerDesdeCacheORepositorio<DVEmpresa>(COLECCION_EMPRESAS, MiLogicaMaestros.ObtenerEmpresas).ToList();

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DVEmpresa() { IdEmpresa = -1, RazonSocial = porDefecto, CIF = "", Telefono = "", Inactivo = true });
            }
            return listaSalida;
        }

        public DEmpresaConf ObtenerEmpresaConf(int nIDEmpresa)
        {
            return MiLogicaMaestros.ObtenerEmpresaConf(nIDEmpresa);
        }

        public IEnumerable<DUnidad> ObtenerUnidades(string porDefecto)
        {
            IList<DUnidad> lista =
                ObtenerDesdeCacheORepositorio<DUnidad>(COLECCION_UNIDADES, MiLogicaMaestros.ObtenerUnidades, true);

            List<DUnidad> listaSalida = new List<DUnidad>(lista);
            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DUnidad() { idunidad = -1, codigo = "", unidad = porDefecto });
            }
            return listaSalida;
        }

        public IEnumerable<DRegion> ObtenerRegiones(string porDefecto)
        {
            IList<DRegion> lista =
                ObtenerDesdeCacheORepositorio<DRegion>(COLECCION_REGIONES, MiLogicaMaestros.ObtenerRegiones, true);

            List<DRegion> listaSalida = new List<DRegion>(lista);
            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DRegion() { idregion = -1, region = porDefecto });
            }
            return listaSalida;
        }

        public IEnumerable<DArea> ObtenerAreas(string porDefecto)
        {
            IList<DArea> lista =
                ObtenerDesdeCacheORepositorio<DArea>(COLECCION_AREAS, MiLogicaMaestros.ObtenerAreas, true);

            List<DArea> listaSalida = new List<DArea>(lista);
            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DArea() { Idarea = -1, area = porDefecto });
            }
            return listaSalida;
        }

        public IEnumerable<DCriterioSeleccion> ObtenerCriteriosSeleccion(string porDefecto)
        {
            IList<DCriterioSeleccion> lista =
                ObtenerDesdeCacheORepositorio<DCriterioSeleccion>(COLECCION_CRITERIOS_SELECCION, MiLogicaMaestros.ObtenerCriteriosSeleccion,true);

            List<DCriterioSeleccion> listaSalida = new List<DCriterioSeleccion>(lista);

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DCriterioSeleccion() { idcriterio = -1, criterioseleccion = porDefecto });
            }
            return listaSalida;
        }

        public IEnumerable<DVCongresos> ObtenerCongresos(string porDefecto, bool bReload)
        {
            IList<DVCongresos> lista =
                ObtenerDesdeCacheORepositorio<DVCongresos>(COLECCION_CONGRESOS, MiLogicaMaestros.ObtenerCongresos, bReload).ToList();

            List<DVCongresos> listaSalida = new List<DVCongresos>(lista);

            if (!string.IsNullOrEmpty(porDefecto))
            {
                listaSalida.Insert(0, new DVCongresos() { IdCongreso = -1, Congreso = porDefecto });
            }
            return listaSalida;
        }


        public bool ActualizarDatosCompliance(DEmpresaConf datos)
        {
            return MiLogicaMaestros.ActualizarDatosCompliance(datos);
        }
        //Ismael Ameller 22-03-2011 Control de productos
        public IList<DVProductos> ObtenerProductos(int? idArea)
        {
            return MiLogicaMaestros.ObtenerProductos(idArea);
        }
        //FIN Ismael Ameller 22-03-2011 Control de productos

        public IList<ReservasInfo> obtenerInfoReserva(int IdExp)
        {
            return MiLogicaMaestros.obtenerInfoReserva(IdExp);
        }

        public string ObtenerMailEmpleadoGt(string idempleadogp, int idconfempresa)
        {
            return MiLogicaMaestros.ObtenerMailEmpleadoGt(idempleadogp, idconfempresa);
        }

        //marta mestre
        public IList<DArea> ObtenerAreasXidunidad(string porDefecto, int idunidad)
        {
            IList<DArea> lista = MiLogicaMaestros.ObtenerAreasXidunidad(idunidad);

            return lista;

        }

        public IList<DRegion> ObtenerRegionesXidunidad(string porDefecto, int idunidad)
        {
            IList<DRegion> lista = MiLogicaMaestros.ObtenerRegionesXidunidad(idunidad);

            return lista;


        }

        public IList<DDistrito> ObtenerDistritosFiltrado(string porDefecto, int idarea, int idregion)
        {
            IList<DDistrito> lista = MiLogicaMaestros.ObtenerDistritosFiltrado(idarea, idregion);

            return lista;
        }

        //Jose Laguna 11-05-2012 creamos la funcion en el agente para poder rescatar la funcinón en el repositorio y validar una reserva según estado y presupuesto de AMEC
        public bool ValidarReservaEstadoAmec(int idservicioReserva)
        {
            bool respuesta = MiLogicaMaestros.ValidarReservaEstadoAmec(idservicioReserva);

            return respuesta;
        }
        public bool ValidarReservaImporteAmec(int idservicioReserva)
        {
            bool respuesta = MiLogicaMaestros.ValidarReservaImporteAmec(idservicioReserva);

            return respuesta;
        }
        //marta mestre
        //JLV --------110512--------start
        //Obtener el idservicio a partir del idreserva
        public int GetServicioByReserva(int idReserva)
        {
            int respuesta = MiLogicaMaestros.GetServicioByReserva(idReserva);
            return respuesta;
        }
        //JLV --------110512--------end
        public List<DDistrict> GetDistrict()
        {
            return MiLogicaMaestros.GetDistrict();
        }

        public List<DSaleForce> GetSaleForce()
        {
            return MiLogicaMaestros.GetSaleForce();
        }

        public List<DDepartament> GetDepartaments()
        {
            return MiLogicaMaestros.GetDepartaments();
        }
    }
}
