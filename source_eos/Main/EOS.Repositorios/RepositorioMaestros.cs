using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Datos;
using System.Data;
using System.Data.Common;
using EOS.Entidades.Filtros;
using EOS.Entidades.Maestros;

namespace EOS.Repositorios
{
    public class RepositorioMaestros : IRepositorioMaestros
    {

        public IList<DUnidad> ObtenerUnidades()
        {
            string consulta = "SELECT * FROM unidades";
            return DUnidad.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }


        public IList<DRegion> ObtenerRegiones()
        {
            string consulta = "SELECT * FROM regiones";
            return DRegion.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public IList<DDistrito> ObtenerDistritos()
        {
            string consulta = "SELECT * FROM distritos";
            return DDistrito.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }
        
        public IList<DVProductos> ObtenerProductos(int? idArea)
        {
            string consulta = string.Empty;
            if (idArea == null)
            {
                consulta = "SELECT IdareaProductoempresa,IdProductoempresa,Productoempresa FROM areas_productosempresa where inactivo = 0 order by Productoempresa";
            }
            else
            {
                consulta = "SELECT IdareaProductoempresa,IdProductoempresa,Productoempresa FROM areas_productosempresa where Idarea=" + idArea + " and inactivo = 0 order by Productoempresa";
            }
            return DVProductos.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }
        
        public bool UserEsDelegado(int idCargo)
        {
            string consulta = "SELECT COUNT(*) FROM cargos where IdCargo= " + idCargo + " and delegado=1";
            long delegado = long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            if (delegado > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        
        public IList<DTratamiento> ObtenerTratamientos()
        {
            string consulta = "SELECT * FROM tratamientos where  ttomsd != '' and ttomsd is not null";
            return DTratamiento.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public IList<DArea> ObtenerAreas()
        {
            string consulta = "SELECT * FROM areas";
            return DArea.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public IList<DEstadoExpediente> ObtenerEstadosExpedientes()
        {
            string consulta = "SELECT * FROM estadosreservas";
            return DEstadoExpediente.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }


        public IList<DTipoReserva> ObtenerTiposReserva()
        {
            string consulta = "SELECT IdTipoReserva, Descripcion FROM tiposreservas_web";
            return DTipoReserva.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }


        public IList<DPoblacion> ObtenerPoblaciones(int idconfempresa)
        {
            string consulta = string.Empty;
            if (idconfempresa > 0)
            {
                consulta = "SELECT IDPoblacion, Poblacion FROM cv_poblaciones_usadas where IdConfEmpresa = " + idconfempresa + "  order by Poblacion";
            }
            else
            {
                consulta = "SELECT IDPoblacion, Poblacion FROM cv_poblaciones_usadas order by Poblacion";
            }

            return DPoblacion.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public IList<DVPoblacion> ObtenerPoblacionesTodas(int idconfempresa)
        {
            string consulta = string.Empty;
            if (idconfempresa > 0)
            {
                consulta = "SELECT * FROM cv_poblaciones_ampliadas where IdConfEmpresa = " + idconfempresa + " order by Poblacion";
            }
            else
            {
                consulta = "SELECT * FROM cv_poblaciones_ampliadas order by Poblacion";
            }
            
            return DVPoblacion.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }
        public IList<DVPais> ObtenerPaisesTodos()
        {
            string consulta = string.Empty;

            consulta = "SELECT * FROM paises order by Pais";

            return DVPais.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }



        public IList<DVPoblacion> ObtenerPoblacionesTodasActivasInactivas(int idconfempresa)
        {
            string consulta = string.Empty;
            if (idconfempresa > 0)
            {
                consulta = " select po.IdPoblacion AS IDPoblacion,po.Poblacion AS Poblacion,po.IdProvincia AS IdProvincia,pr.Provincia AS Provincia,po.IdPais AS IdPais,pa.Pais AS Pais,po.IdConfEmpresa as IdConfEmpresa from ((poblaciones po left join provincias pr on((po.IdProvincia = pr.IdProvincia))) left join paises pa on((po.IdPais = pa.IdPais))) where IdConfEmpresa = " + idconfempresa + " ";
            }
            else
            {
                consulta = " select po.IdPoblacion AS IDPoblacion,po.Poblacion AS Poblacion,po.IdProvincia AS IdProvincia,pr.Provincia AS Provincia,po.IdPais AS IdPais,pa.Pais AS Pais,po.IdConfEmpresa as IdConfEmpresa from ((poblaciones po left join provincias pr on((po.IdProvincia = pr.IdProvincia))) left join paises pa on((po.IdPais = pa.IdPais))) ";
            }
            
            return DVPoblacion.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public IList<DTipoActividad> ObtenerTiposActividad(bool? isAdmin, bool? newco)
        {
            if (isAdmin.HasValue && isAdmin.Value)
            {
                string consulta = "SELECT IdTipoCongreso IdTipoActividad, TipoCongreso  + ' MSD' Nombre FROM tiposcongreso where visible_Web = 1 and newco = 0 UNION SELECT IdTipoCongreso IdTipoActividad, TipoCongreso + ' ORGANON' Nombre FROM tiposcongreso where visible_Web = 1 and newco = 1 ORDER BY Nombre";

                return DTipoActividad.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            }
            else
            {
                string consulta = (newco.HasValue && newco.Value) ? "SELECT IdTipoCongreso IdTipoActividad, TipoCongreso Nombre FROM tiposcongreso where visible_Web = 1 and newco = 1 ORDER BY Nombre" : "SELECT IdTipoCongreso IdTipoActividad, TipoCongreso Nombre FROM tiposcongreso where visible_Web = 1 and newco = 0 ORDER BY Nombre";

                return DTipoActividad.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
            }
        }

        public IList<DCentroCoste> ObtenerCentrosCoste()
        {
            string consulta = "select cc.idcentrocoste as idcentrocoste, cc.centrocoste as centrocoste, ccc.idcentrocostecategoria as idcentrocostecategoria, ccc.centrocostecategoria as centrocostecategoria from centros_coste cc inner join centros_coste_categoria ccc on cc.idcentrocostecategoria = ccc.idcentrocostecategoria where cc.deleted = 0 and ccc.deleted = 0 order by cc.centrocoste asc";

            return DCentroCoste.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public IList<DTipoActividadCongreso> ObtenerTiposActividadCongreso()
        {
            string consulta = "SELECT *  FROM tipos_actividad_congreso";
            return DTipoActividadCongreso.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public IList<DTipoPatrocinio> ObtenerTiposPatrocinio()
        {
            string consulta = "SELECT *  FROM tipospatrocinio";
            return DTipoPatrocinio.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public IList<DTipoEspecialidad> ObtenerTiposEspecialidades()
        {
            //Ismael Ameller 04-03-2011 No quieren que aparezca la opción no-especialidad y Evento multidisciplinar el primero
            string consulta = "SELECT IdEspecialidad, CodEspecialidad, Especialidad FROM especialidades where codEspecialidad= 'EMD' UNION SELECT IdEspecialidad, CodEspecialidad, Especialidad FROM especialidades where CodEspecialidad <> 'res' order by especialidad";
            return DTipoEspecialidad.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        //Ismael Ameller 08-03-2011 Cambio a la hora de rellenar el combo para que no salga evento-multidisciplinar
        public IList<DTipoEspecialidad> ObtenerTiposEspecialidadesNP()
        {
            string consulta = "SELECT IdEspecialidad, CodEspecialidad, Especialidad FROM especialidades where codEspecialidad<> 'EMD' order by especialidad";
            return DTipoEspecialidad.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }
        //FIN Ismael Ameller 08-03-2011 Cambio a la hora de rellenar el combo para que no salga evento-multidisciplinar
        public IList<DVEmpresa> ObtenerEmpresas()
        {
            string consulta = "SELECT * FROM cv_empresas";
            return DVEmpresa.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }
        public IList<DEmpresaConf> ObtenerEmpresasConf()
        {
            string consulta = "SELECT * FROM confempresa where inactive = 0";
            List<DEmpresaConf> listResult = new List<DEmpresaConf>();
            DEmpresaConf agencia = new DEmpresaConf
            {
                idconfempresa = 0,
                nombreagencia = "Sin asignar"
            };
            listResult.Add(agencia);
            foreach (var item in DEmpresaConf.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)))
            {
                listResult.Add(item);
            }
            return listResult;
        }
        public DEmpresaConf ObtenerEmpresaConf(int nIdEmpresa)
        {
            string consulta = string.Format("SELECT * FROM confempresa where idconfempresa = {0}", nIdEmpresa);
            return DEmpresaConf.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta)).FirstOrDefault();
        }

        public IList<DCriterioSeleccion> ObtenerCriteriosSeleccion()
        {
            string consulta = "SELECT * FROM criteriosseleccion where inactivo = 0";
            return DCriterioSeleccion.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public IList<DVCongresos> ObtenerCongresos()
        {
            string consulta = "SELECT * FROM cv_congresos order by Congreso";
            return DVCongresos.ConvertToIListDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public IList<ReservasInfo> obtenerInfoReserva(int IdExp)
        {
            string consulta = "SELECT * FROM cv_reservas_enviadas_exp where IdExpediente=" + IdExp;
            return ReservasInfo.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public string ObtenerMailEmpleadoGt(string idempleadogp, int idconfempresa)
        {
            string consulta = "SELECT email FROM empleadosgp where idempleadogp='" + idempleadogp + "' and idconfempresa=" + idconfempresa;
            return Quodem.Sql.SqlServerClient.GetValue(consulta);
        }

        public IList<DArea> ObtenerAreasXidunidad(int idunidad)
        {
            string consulta = "SELECT * FROM areas where idunidad=" + idunidad;
            return DArea.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }


        public IList<DRegion> ObtenerRegionesXidunidad(int idunidad)
        {
            string consulta = "SELECT * FROM regiones where idunidad=" + idunidad;
            return DRegion.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public IList<DDistrito> ObtenerDistritosFiltrado(int idArea, int idRegion)
        {
            string consulta = string.Format("select * from distritos {0}", CrearSeccionWhereDistrito(idArea, idRegion));
            return DDistrito.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        private string CrearSeccionWhereDistrito(int idArea, int idRegion)
        {
            StringBuilder db = new StringBuilder();
            bool primero = true;
            if (idArea != null && idArea != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" idarea = {0}", idArea);
            }

            if (idRegion != null && idRegion != -1)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" idregion = {0}", idRegion);
            }
            return db.ToString();

        }
        private void AgregarAndSiProcede(StringBuilder sb, ref bool primero)
        {
            if (primero == true)
            {
                primero = false;
                sb.Append(" WHERE ");
            }
            else sb.Append(" AND ");
        }

        //Jose Laguna Gimenez 11-05-2012 funcion para validar el estado de la reserva
        public bool ValidarReservaEstadoAmec(int idservicioReserva)
        {
            string consulta = string.Format("select dbo.validar_estado_amec({0})", idservicioReserva);
            int nResult = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));

            if (nResult == 1)
                return true;
            else
                return false;

        }

        //Jose Laguna Gimenez 11-05-2012 funcion para validar la reserva según el presupuesto del AMEC.
        public bool ValidarReservaImporteAmec(int idservicioReserva)
        {
            //LJM 25/01/2018 - Eliminar restricción
            //string consulta = string.Format("select dbo.validar_presupuesto_amec({0})", idservicioReserva);
            //int nResult = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            //if (System.Configuration.ConfigurationManager.AppSettings["controlPresupuesto"] == "0")
            //    nResult = 1;

            //if (nResult == 1)
                return true;
            //else
            //    return false;
        }
        
        public int GetServicioByReserva(int idReserva)
        {
            string consulta = string.Format("SELECT idServicio FROM serviciosreservasviajes WHERE idreserva = {0}", idReserva);
            int nResult = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            return nResult;
        }


        public List<DDocType> GetDocType()
        {
            string query = "SELECT * FROM gestordocumental_tipodoc;";
            return DDocType.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query));
        }

        public List<DDocSubType> GetDocSubType()
        {
            string query = "SELECT * FROM gestordocumental_subtipodoc;";
            return DDocSubType.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query));
        }

        public List<DDepartament> GetDepartaments()
        {
            string query = "SELECT * FROM departaments;";
            return DDepartament.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query));
        }

        public List<DSaleForce> GetSaleForce()
        {
            string query = "SELECT * FROM salesforce;";
            return DSaleForce.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query));
        }

        public List<DDistrict> GetDistrict()
        {
            string query = "SELECT * FROM districts;";
            return DDistrict.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(query));
        }

        
    }
}
