using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;

namespace EOS.Logica
{
    public interface ILogicaMaestros
    {
        bool CambiarContraseña(string usuario, string password, string nuevaPassword);
        IList<DEstadoExpediente> ObtenerEstadosExpedientes();
        IList<DTipoReserva> ObtenerTiposReserva();
        IList<DPoblacion> ObtenerPoblaciones(int idconfempresa);
        IList<DVPoblacion> ObtenerPoblacionesTodas(int idconfempresa);
        IList<DVPoblacion> ObtenerPoblacionesTodasActivasInactivas(int idconfempresa);
        IList<DTipoActividad> ObtenerTiposActividad(bool? isAdmin, bool? newco);
        IList<DTipoActividadCongreso> ObtenerTiposActividadCongreso();
        IList<DTipoPatrocinio> ObtenerTiposPatrocinio();
        IList<DTipoEspecialidad> ObtenerTiposEspecialidades();
        IList<DVEmpresa> ObtenerEmpresas();
        IList<DEmpresaConf> ObtenerEmpresasConf();
        DEmpresaConf ObtenerEmpresaConf(int nIDEmpresa);
        IList<DDistrito> ObtenerDistritos();
        IList<DTratamiento> ObtenerTratamientos();
        IList<DUnidad> ObtenerUnidades();
        IList<DRegion> ObtenerRegiones();
        IList<DArea> ObtenerAreas();
        IList<DCriterioSeleccion> ObtenerCriteriosSeleccion();
        IList<DVCongresos> ObtenerCongresos();

        bool ActualizarDatosCompliance(DEmpresaConf datos);

        //marta mestre
        IList<DArea> ObtenerAreasXidunidad(int idunidad);
        IList<DRegion> ObtenerRegionesXidunidad(int idunidad);
        IList<DDistrito> ObtenerDistritosFiltrado(int idarea, int idregion);
        
    }
}
