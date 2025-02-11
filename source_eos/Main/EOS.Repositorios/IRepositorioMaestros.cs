using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Entidades.Maestros;

namespace EOS.Repositorios
{
    public interface IRepositorioMaestros
    {
        IList<DEstadoExpediente> ObtenerEstadosExpedientes();
        IList<DTipoReserva> ObtenerTiposReserva();
        IList<DPoblacion> ObtenerPoblaciones(int idconfempresa);
        IList<DVPoblacion> ObtenerPoblacionesTodas(int idconfempresa);
        IList<DVPais> ObtenerPaisesTodos();
        IList<DVPoblacion> ObtenerPoblacionesTodasActivasInactivas(int idconfempresa);
        IList<DTipoActividad> ObtenerTiposActividad(bool? isAdmin, bool? newco);
        IList<DCentroCoste> ObtenerCentrosCoste();
        IList<DTipoActividadCongreso> ObtenerTiposActividadCongreso();
        IList<DTipoPatrocinio> ObtenerTiposPatrocinio();
        IList<DTipoEspecialidad> ObtenerTiposEspecialidades();
        //Ismael Ameller 08-03-2011 Cambio a la hora de rellenar el combo para que no salga evento-multidisciplinar
        IList<DTipoEspecialidad> ObtenerTiposEspecialidadesNP();
        //FIN Ismael Ameller 08-03-2011 Cambio a la hora de rellenar el combo para que no salga evento-multidisciplinar
        IList<DVEmpresa> ObtenerEmpresas();
        DEmpresaConf ObtenerEmpresaConf(int nIdEmpresa);

        IList<DUnidad> ObtenerUnidades();
        IList<DRegion> ObtenerRegiones();
        IList<DDistrito> ObtenerDistritos();
        List<DDocType> GetDocType();
        IList<DArea> ObtenerAreas();
        List<DDocSubType> GetDocSubType();
        IList<DTratamiento> ObtenerTratamientos();
        IList<DCriterioSeleccion> ObtenerCriteriosSeleccion();
        IList<DVCongresos> ObtenerCongresos();
        IList<DEmpresaConf> ObtenerEmpresasConf();

        //Ismael Ameller 04-03-2011 Saber si es delegado
        bool UserEsDelegado(int idCargo);
        //Ismael Ameller 22-03-2011 Control de productos
        IList<DVProductos> ObtenerProductos(int? idArea);

        IList<ReservasInfo> obtenerInfoReserva(int IdExp);


        string ObtenerMailEmpleadoGt(string idempleadogp, int idconfempresa);

        //marta mestre
        IList<DArea> ObtenerAreasXidunidad(int idunidad);
        IList<DRegion> ObtenerRegionesXidunidad(int idunidad);
        IList<DDistrito> ObtenerDistritosFiltrado(int idarea, int idregion);
        //marta mestre

        //jose Laguna 11-05-2012 
        bool ValidarReservaEstadoAmec(int idservicioReserva);

        bool ValidarReservaImporteAmec(int idservicioReserva);
        int GetServicioByReserva(int idReserva);

         List<DDistrict> GetDistrict();
         List<DSaleForce> GetSaleForce();
         List<DDepartament> GetDepartaments();
    }
}
