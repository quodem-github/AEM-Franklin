using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.Entidades.Filtros;
using EOS.Entidades.Datos;
using System.Data.Common;
using System.Data;
using Microsoft.SqlServer.Server;
using NHibernate;

namespace EOS.Repositorios
{
    public class RepositorioParticipantes : IRepositorioParticipantes
    {

        #region Definitions
        private ISession _sessVariable;
        public ISession _session
        {
            get
            {
                if (!_sessVariable.IsOpen)
                {
                    _sessVariable = Quodem.ORM.NHibernate.Helper.GetCurrentSession(ServiceLogic.Enums.EDbConnection.Default.GetHashCode());
                }
                return _sessVariable;
            }
            set { _sessVariable = value; }
        }
        #endregion

        #region Constructor

        public RepositorioParticipantes()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(EOS.ServiceLogic.Enums.EDbConnection.Default.GetHashCode());
        }

        #endregion

        #region "Participantes Expediente"

        public int InsertaEntidad(DReservasPassengersList reservasPassenger)
        {
            string consulta =
                string.Format(
                    "INSERT INTO reservas_passengers_list (idreservapassengerlist,idpassengerlist,locked,idxpediente) VALUES ({0},{1},{2},{3});",
                    reservasPassenger.idreservapassengerlist, reservasPassenger.idpassengerlist,
                    reservasPassenger.locked != null?reservasPassenger.locked.ToString(): "null", reservasPassenger.idxpediente);

            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }

        public ICollection<DVParticipante> ObtenerParticipantes(FiltroParticipantes filtro)
        {
            string consulta ="";
            if (filtro.IdExpediente <= 0 && filtro.IdAmec.HasValue)
            {
                consulta = string.Format("select CAST(a.idnivelriesgo as CHAR(5)) as nivelriesgo ,null as tipoactividadpax,null as tipoasistente ,null as honorarios ,pl.IdPassengerlist ,a.Nombre ,a.Apel1 ,a.Apel2 ,Email ,a.Pasaporte ,Hospital ,a.Localidad ,a.Msdid ,a.IdDistrito ,Distrito ,IdRegion ,Region ,IdEmpresa ,RazonSocial ,a.IdEspecialidad ,Especialidad ,CodEspecialidad from cv_participantes cpa join (select idpassengerlist from amecs_passengers_list where idamec='{0}') apa on apa.idpassengerlist = cpa.idpassengerlist inner join passengers_list a on a.idpassengerlist = cpa.idpassengerlist", filtro.IdAmec);                
            }
            else
            {
                consulta = string.Format("select CAST(a.idnivelriesgo as CHAR(5)) as nivelriesgo ,null as tipoactividadpax,null as tipoasistente ,null as honorarios ,pl.IdPassengerlist ,a.Nombre ,a.Apel1 ,a.Apel2 ,Email ,a.Pasaporte ,Hospital ,a.Localidad ,a.Msdid ,a.IdDistrito ,Distrito ,IdRegion ,Region ,IdEmpresa ,RazonSocial ,a.IdEspecialidad ,Especialidad ,CodEspecialidad from cv_participantes pl inner join passengers_list a on a.idpassengerlist = pl.idpassengerlist {0}", CrearSeccionWhereFiltroParticipante(filtro));
            }

            if (filtro.MaximumRows != null && filtro.StartRowIndex != null)
            {
                return _session.CreateSQLQuery(consulta).SetFirstResult(filtro.StartRowIndex.Value).SetMaxResults(filtro.MaximumRows.Value).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DVParticipante))).List<DVParticipante>();
            }
            
            return _session.CreateSQLQuery(consulta).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DVParticipante))).List<DVParticipante>();
        }

        public ICollection<DVParticipanteVeeva> ObtenerParticipantesVeeva(FiltroParticipantesVeeva filtro)
        {
            string consulta = "";
            if (filtro.IdExpediente <= 0 && filtro.IdAmec.HasValue)
            {
                consulta = string.Format("select CAST(a.idnivelriesgo as CHAR(5)) as nivelriesgo ,null as tipoactividadpax,null as tipoasistente ,null as honorarios ,pl.IdPassengerlist ,a.Nombre ,a.Apel1 ,a.Apel2, pl.FullName, Email ,a.Pasaporte ,Hospital ,a.Localidad ,a.Msdid ,a.IdDistrito ,Distrito ,IdRegion ,Region ,IdEmpresa ,RazonSocial ,a.IdEspecialidad ,Especialidad ,CodEspecialidad, pl.AttendeeType, pl.AttendeeTypeOrder, pl.Status from cv_participantes_veeva cpa join (select idpassengerlist from amecs_passengers_list where idamec='{0}') apa on apa.idpassengerlist = cpa.idpassengerlist and cpa.IdAmecs = '{1}' inner join passengers_list a on a.idpassengerlist = cpa.idpassengerlist", filtro.IdAmec, filtro.IdAmecString);
            }
            else if (!String.IsNullOrWhiteSpace(filtro.IdAmecString))
            {
                consulta = string.Format("select CAST(a.idnivelriesgo as CHAR(5)) as nivelriesgo ,null as tipoactividadpax,null as tipoasistente ,null as honorarios ,pl.IdPassengerlist ,a.Nombre ,a.Apel1 ,a.Apel2 , pl.FullName, Email ,a.Pasaporte ,Hospital ,a.Localidad ,a.Msdid ,a.IdDistrito ,Distrito ,IdRegion ,Region ,IdEmpresa ,RazonSocial ,a.IdEspecialidad ,Especialidad ,CodEspecialidad, pl.AttendeeType, pl.AttendeeTypeOrder, pl.Status from cv_participantes_veeva pl inner join passengers_list a on pl.IdAmecs = '{1}' and a.idpassengerlist = pl.idpassengerlist {0}", CrearSeccionWhereFiltroParticipanteVeeva(filtro), filtro.IdAmecString);
            }
            else
            {
                consulta = string.Format("select CAST(a.idnivelriesgo as CHAR(5)) as nivelriesgo ,null as tipoactividadpax,null as tipoasistente ,null as honorarios ,pl.IdPassengerlist ,a.Nombre ,a.Apel1 ,a.Apel2 , pl.FullName, Email ,a.Pasaporte ,Hospital ,a.Localidad ,a.Msdid ,a.IdDistrito ,Distrito ,IdRegion ,Region ,IdEmpresa ,RazonSocial ,a.IdEspecialidad ,Especialidad ,CodEspecialidad, pl.AttendeeType, pl.AttendeeTypeOrder, pl.Status from cv_participantes_veeva pl inner join passengers_list a on pl.IdAmecs = '{1}' and a.idpassengerlist = pl.idpassengerlist {0}", CrearSeccionWhereFiltroParticipanteVeeva(filtro), filtro.IdAmecString);
            }

            if (filtro.MaximumRows != null && filtro.StartRowIndex != null)
            {
                return _session.CreateSQLQuery(consulta).SetFirstResult(filtro.StartRowIndex.Value).SetMaxResults(filtro.MaximumRows.Value).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DVParticipanteVeeva))).List<DVParticipanteVeeva>();
            }

            return _session.CreateSQLQuery(consulta).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DVParticipanteVeeva))).List<DVParticipanteVeeva>();
        }

        public ICollection<DVParticipante> ObtenerParticipantesExpediente(FiltroParticipantes filtro)
        {
            string consulta = "";
            if (filtro.IdExpediente <= 0 && filtro.IdAmec.HasValue)
            {
                consulta = string.Format("select '' as nivelriesgo ,'' as tipoactividadpax ,'' as tipoasistente ,null as honorarios,cpa.IdPassengerlist ,Nombre ,Apel1 ,Apel2 ,Email ,Pasaporte ,Hospital ,Localidad ,Msdid ,IdDistrito ,Distrito ,IdRegion ,Region ,IdEmpresa ,RazonSocial ,IdEspecialidad ,Especialidad ,CodEspecialidad from cv_participantes cpa join (select idpassengerlist from amecs_passengers_list where idamec={0}) apa on apa.idpassengerlist = cpa.idpassengerlist", filtro.IdAmec);
            }
            else
            {
                consulta = string.Format("select nivelriesgo ,tipoactividadpax ,tipoasistente ,pl.IdPassengerlist, Nombre ,Apel1 ,Apel2 ,Email ,Pasaporte ,Hospital ,Localidad ,Msdid ,IdDistrito ,Distrito ,IdRegion ,Region ,IdEmpresa ,RazonSocial ,IdEspecialidad ,Especialidad ,CodEspecialidad, h.idtipoactividadpax, h.idtipoasistente, h.honorarios, h.pagodirecto, h.pagosociedad, h.ficherogenesis, h.tiporeunion, h.ponentes_duracionactividad, h.ponentes_preparacion, h.ponentes_nivelps, h.ponentes_honorariosmaximos, h.abeif_preparacion, h.abeif_duracionactividad, h.abeif_nivelps, h.abeif_honorariosmaximos, cast(isnull(h.ponencia_centro_salud, 0) as int) as ponencia_centro_salud, cast(isnull(h.talleres, 0) as int) as talleres, cast(isnull(h.videoconferencia_repetida, 0) as int) as videoconferencia_repetida, h.tipo_ponente, h.ponentes_idhonorariosmaximos, h.justificacion, h.idTipoContratoConsultoria, h.numero_dias_consultoria from cv_participantes pl left join datosadicionales_reservas_passenger h on h.idpassengerlist = pl.idpassengerlist and h.idexpediente = {1} left join calc_honorarios_maximos calhon on calhon.id = h.ponentes_idhonorariosmaximos left join tipo_actividad_pax_datosadicionales d on d.idtipoactividadpax = h.idtipoactividadpax left join tipo_asistente_datosadicionales ta on ta.idtipoasistente = h.idtipoasistente left join nivel_riesgo_HCP_datosadicionales nr on nr.idnivelriesgo = h.idnivelriesgo {0}", CrearSeccionWhereFiltroParticipante(filtro), filtro.IdExpediente);
            }

            if (filtro.MaximumRows != null && filtro.StartRowIndex != null)
            {
                return _session.CreateSQLQuery(consulta).SetFirstResult(filtro.StartRowIndex.Value).SetMaxResults(filtro.MaximumRows.Value).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DVParticipante))).List<DVParticipante>();
            }
            
            return _session.CreateSQLQuery(consulta).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DVParticipante))).List<DVParticipante>();
        }

        public ICollection<DVParticipante> ObtenerTodosParticipantesExpediente(int idexpediente)
        {
            string consulta = string.Format("select * from cv_participantes pl inner join datosadicionales_reservas_passenger h on h.idpassengerlist = pl.idpassengerlist and h.idexpediente = {0} left join tipo_actividad_pax_datosadicionales d on d.idtipoactividadpax = h.idtipoactividadpax left join tipo_asistente_datosadicionales ta on ta.idtipoasistente = h.idtipoasistente left join nivel_riesgo_HCP_datosadicionales nr on nr.idnivelriesgo = h.idnivelriesgo left join calc_honorarios_maximos calhon on calhon.id = h.ponentes_idhonorariosmaximos ", idexpediente);
            return DVParticipante.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public ICollection<DVParticipante> ObtenerPonentesParticipantesExpediente(int idexpediente)
        {
            string consulta = string.Format("select * from cv_participantes pl inner join datosadicionales_reservas_passenger h on h.idpassengerlist = pl.idpassengerlist and h.idexpediente = {0} inner join tipo_actividad_pax_datosadicionales d on d.idtipoactividadpax = h.idtipoactividadpax inner join tipo_asistente_datosadicionales ta on ta.idtipoasistente = h.idtipoasistente left join nivel_riesgo_HCP_datosadicionales nr on nr.idnivelriesgo = h.idnivelriesgo left join calc_honorarios_maximos calhon on calhon.id = h.ponentes_idhonorariosmaximos where ta.idtipoABC = 1   ", idexpediente);
            return DVParticipante.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public long ObtenerNumeroParticipantes(FiltroParticipantes filtro)
        {
            string consulta = string.Format("SELECT count(*) FROM cv_participantes pl {0}", CrearSeccionWhereFiltroParticipante(filtro, true));
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }

        public long ObtenerNumeroParticipantesVeeva(FiltroParticipantesVeeva filtro)
        {
            string consulta = string.Format("SELECT count(*) FROM cv_participantes_veeva pl {0}", CrearSeccionWhereFiltroParticipanteVeeva(filtro, true));
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }

        /**
        * Devuelve todos los Participantes Inscritos a Inscripcion */
        public ICollection<DPassengerList>  getInscripcionList(int idexp)
        {
            string consulta = string.Format("select p.[idpassengerlist],p.[nombre],p.[apel1],p.[apel2],p.[NIF],p.[DireccionEmail],p.[TelefonoContacto],p.[FechaNacimiento],p.[Pasaporte],p.[FechaCaducidadPasaporte],p.[NombreCentroTrabajo],p.[LocalidadCentroTrabajo],p.[DireccionCentroTrabajo],p.[LugarEmisionPasaporte],p.[NacionalidadPasaporte],p.[OrganizacionVisitaProfesional],p.[DireccionProfesionalOrganizacion],p.[CodigoPostal],p.[Localidad],p.[Provincia],p.[Pais],p.[msdid],p.[locked],p.[idtratamiento],p.[iddistrito],p.[idespecialidad],p.[descuentoresidente],p.[idnivelriesgo],p.[inactivo],p.[GenesysCode],p.[GoldenId],p.[ProvinceCenter],p.[PostalCodeCenter],p.[UpdateDate],p.[CenterCode],p.[internacional] from passengers_list p" +
                                            " join ins_passengers_list t on p.idpassengerlist = t.idpassengerlist" +
                                            " join serviciosreservasinscripciones isrv on t.idservicioinscripcion = isrv.idservicioinscripcion" +
                                            " join serviciosreservasviajes srv on isrv.idservicioinscripcion = srv.idservicioinscripcion" +
                                            " join reservasviajes rv on srv.idreserva = rv.idreserva" +
                                            " join expediente exp on rv.fkidexpediente = exp.idxpediente" +
                                            " where exp.idxpediente = {0}", idexp);

            return _session.CreateSQLQuery(consulta).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DPassengerList))).List<DPassengerList>();

        }

        public ICollection<DPassengerList> getAlojamientoList(int idexp)
        {
            string consulta = string.Format("select p.[idpassengerlist],p.[nombre],p.[apel1],p.[apel2],p.[NIF],p.[DireccionEmail],p.[TelefonoContacto],p.[FechaNacimiento],p.[Pasaporte],p.[FechaCaducidadPasaporte],p.[NombreCentroTrabajo],p.[LocalidadCentroTrabajo],p.[DireccionCentroTrabajo],p.[LugarEmisionPasaporte],p.[NacionalidadPasaporte],p.[OrganizacionVisitaProfesional],p.[DireccionProfesionalOrganizacion],p.[CodigoPostal],p.[Localidad],p.[Provincia],p.[Pais],p.[msdid],p.[locked],p.[idtratamiento],p.[iddistrito],p.[idespecialidad],p.[descuentoresidente],p.[idnivelriesgo],p.[inactivo],p.[GenesysCode],p.[GoldenId],p.[ProvinceCenter],p.[PostalCodeCenter],p.[UpdateDate],p.[CenterCode],p.[internacional] from passengers_list p" +
                                            " join hotel_passenger_list t on p.idpassengerlist = t.idpassengerlist" +
                                            " join serviciosreservasinscripciones isrv on t.idservicioinscripcion = isrv.idservicioinscripcion" +
                                            " join serviciosreservasviajes srv on isrv.idservicioinscripcion = srv.idservicioinscripcion" +
                                            " join reservasviajes rv on srv.idreserva = rv.idreserva" +
                                            " join expediente exp on rv.fkidexpediente = exp.idxpediente" +
                                            " where exp.idxpediente = {0}", idexp);

            return _session.CreateSQLQuery(consulta).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DPassengerList))).List<DPassengerList>();

        }

        public ICollection<DPassengerList> getTransporteList(int idexp)
        {
            string consulta = string.Format("select p.[idpassengerlist],p.[nombre],p.[apel1],p.[apel2],p.[NIF],p.[DireccionEmail],p.[TelefonoContacto],p.[FechaNacimiento],p.[Pasaporte],p.[FechaCaducidadPasaporte],p.[NombreCentroTrabajo],p.[LocalidadCentroTrabajo],p.[DireccionCentroTrabajo],p.[LugarEmisionPasaporte],p.[NacionalidadPasaporte],p.[OrganizacionVisitaProfesional],p.[DireccionProfesionalOrganizacion],p.[CodigoPostal],p.[Localidad],p.[Provincia],p.[Pais],p.[msdid],p.[locked],p.[idtratamiento],p.[iddistrito],p.[idespecialidad],p.[descuentoresidente],p.[idnivelriesgo],p.[inactivo],p.[GenesysCode],p.[GoldenId],p.[ProvinceCenter],p.[PostalCodeCenter],p.[UpdateDate],p.[CenterCode],p.[internacional] from passengers_list p" +
                                            " join transportepassengerslist t on p.idpassengerlist = t.idpassengerlist" +
                                            " join serviciosreservasinscripciones isrv on t.idservicioinscripcion = isrv.idservicioinscripcion" +
                                            " join serviciosreservasviajes srv on isrv.idservicioinscripcion = srv.idservicioinscripcion" +
                                            " join reservasviajes rv on srv.idreserva = rv.idreserva" +
                                            " join expediente exp on rv.fkidexpediente = exp.idxpediente" +
                                            " where exp.idxpediente = {0}", idexp);

            return _session.CreateSQLQuery(consulta).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DPassengerList))).List<DPassengerList>();

        }

        public ICollection<DPassengerList> getActividadesList(int idexp)
        {
            string consulta = string.Format("select p.[idpassengerlist],p.[nombre],p.[apel1],p.[apel2],p.[NIF],p.[DireccionEmail],p.[TelefonoContacto],p.[FechaNacimiento],p.[Pasaporte],p.[FechaCaducidadPasaporte],p.[NombreCentroTrabajo],p.[LocalidadCentroTrabajo],p.[DireccionCentroTrabajo],p.[LugarEmisionPasaporte],p.[NacionalidadPasaporte],p.[OrganizacionVisitaProfesional],p.[DireccionProfesionalOrganizacion],p.[CodigoPostal],p.[Localidad],p.[Provincia],p.[Pais],p.[msdid],p.[locked],p.[idtratamiento],p.[iddistrito],p.[idespecialidad],p.[descuentoresidente],p.[idnivelriesgo],p.[inactivo],p.[GenesysCode],p.[GoldenId],p.[ProvinceCenter],p.[PostalCodeCenter],p.[UpdateDate],p.[CenterCode],p.[internacional] from passengers_list p" +
                                            " join actividades_passengers_list t on p.idpassengerlist = t.idpassengerlist" +
                                            " join serviciosreservasinscripciones isrv on t.idservicioinscripcion = isrv.idservicioinscripcion" +
                                            " join serviciosreservasviajes srv on isrv.idservicioinscripcion = srv.idservicioinscripcion" +
                                            " join reservasviajes rv on srv.idreserva = rv.idreserva" +
                                            " join expediente exp on rv.fkidexpediente = exp.idxpediente" +
                                            " where exp.idxpediente = {0}", idexp);

            return _session.CreateSQLQuery(consulta).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(DPassengerList))).List<DPassengerList>();

        }

        private string CrearSeccionWhereFiltroParticipante(FiltroParticipantes filtro, bool count = false)
        {
            if (filtro == null) return null;

            StringBuilder db = new StringBuilder();

            bool primero = true;

            // Primero se filtra los datos Exists o No Exists si buscamos la lista de participantes ya añadidos o los no añadidos
            if (filtro.IdExpediente != null || filtro.IdExpedienteConjunto != null)
            {
                AgregarAndSiProcede(db, ref primero);
                if (filtro.IdExpedienteConjunto != null)
                {
                    db.AppendFormat(" pl.idpassengerlist not in ({0}) ", filtro.IdExpedienteConjunto);
                }
                else
                {
                    db.AppendFormat(" {0} (select 1 from reservas_passengers_list rp where pl.idpassengerlist = rp.idpassengerlist and idxpediente={1}) ", filtro.Existe, filtro.IdExpediente);
                }
            }

            if (filtro.IdParticipante != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.IdPassengerlist = {0}", filtro.IdParticipante);
            }
            if (filtro.Nombre != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.Nombre like '{0}'", filtro.Nombre.Replace("'", "''"));
            }
            if (filtro.Apel1 != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" (pl.Apel1 like '{0}'", filtro.Apel1.Replace("'", "''"));
                if (filtro.Apel2 == null)
                    db.Append(")");
            }
            if (filtro.Apel2 != null)
            {
                if (filtro.Apel1 != null)
                    AgregarOrSiProcede(db, ref primero);
                else
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (");
                }
                db.AppendFormat(" pl.Apel2 like '{0}')", filtro.Apel2.Replace("'", "''"));
            }
            if (filtro.Hospital != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.Hospital like '{0}'", filtro.Hospital.Replace("'", "''"));
            }
            if (filtro.MSDID != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.Msdid like '{0}'", filtro.MSDID);
            }
            if (filtro.IdDistrito != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.IdDistrito = {0}", filtro.IdDistrito);
            }
            if (filtro.IdRegion != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.IdRegion = {0}", filtro.IdRegion);
            }
            if (filtro.IdEmpresa != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.IdEmpresa = {0}", filtro.IdEmpresa);
            }

            if (!string.IsNullOrEmpty(filtro.SortParameter))
            {
                db.AppendFormat(" ORDER BY {0}", filtro.SortParameter);
            }
            else
            {
                if (!count)
                {
                    db.Append(" ORDER BY pl.Nombre");
                }
            }
            //if (filtro.MaximumRows != null && filtro.MaximumRows > 0)
            //{
            //    db.AppendFormat(" LIMIT {0}", filtro.MaximumRows);
            //}
            //if (filtro.StartRowIndex != null && filtro.StartRowIndex > 0)
            //{
            //    db.AppendFormat(" OFFSET {0}", filtro.StartRowIndex);
            //}

            return db.ToString();
        }

        private string CrearSeccionWhereFiltroParticipanteVeeva(FiltroParticipantesVeeva filtro, bool count = false)
        {
            if (filtro == null) return null;

            StringBuilder db = new StringBuilder();

            bool primero = true;

            // Primero se filtra los datos Exists o No Exists si buscamos la lista de participantes ya añadidos o los no añadidos
            if (filtro.IdExpediente != null || filtro.IdExpedienteConjunto != null)
            {
                AgregarAndSiProcede(db, ref primero);
                if (filtro.IdExpedienteConjunto != null)
                {
                    db.AppendFormat(" pl.idpassengerlist not in ({0}) ", filtro.IdExpedienteConjunto);
                }
                else
                {
                    db.AppendFormat(" {0} (select 1 from reservas_passengers_list rp where pl.idpassengerlist = rp.idpassengerlist and idxpediente={1}) ", filtro.Existe, filtro.IdExpediente);
                }
            }

            if (!string.IsNullOrWhiteSpace(filtro.IdAmecString)) {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.IdAmecs = '{0}' ", filtro.IdAmecString);
            }

            if (filtro.IdParticipante != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.IdPassengerlist = {0}", filtro.IdParticipante);
            }
            if (filtro.Nombre != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.Nombre like '{0}'", filtro.Nombre.Replace("'", "''"));
            }
            if (filtro.Apel1 != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" (pl.Apel1 like '{0}'", filtro.Apel1.Replace("'", "''"));
                if (filtro.Apel2 == null)
                    db.Append(")");
            }
            if (filtro.Apel2 != null)
            {
                if (filtro.Apel1 != null)
                    AgregarOrSiProcede(db, ref primero);
                else
                {
                    AgregarAndSiProcede(db, ref primero);
                    db.Append(" (");
                }
                db.AppendFormat(" pl.Apel2 like '{0}')", filtro.Apel2.Replace("'", "''"));
            }
            if (filtro.Hospital != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.Hospital like '{0}'", filtro.Hospital.Replace("'", "''"));
            }
            if (filtro.MSDID != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.Msdid like '{0}'", filtro.MSDID);
            }
            if (filtro.IdDistrito != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.IdDistrito = {0}", filtro.IdDistrito);
            }
            if (filtro.IdRegion != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.IdRegion = {0}", filtro.IdRegion);
            }
            if (filtro.IdEmpresa != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" pl.IdEmpresa = {0}", filtro.IdEmpresa);
            }

            if (!string.IsNullOrEmpty(filtro.SortParameter))
            {
                db.AppendFormat(" ORDER BY {0}", filtro.SortParameter);
            }
            else
            {
                if (!count)
                {
                    db.Append(" ORDER BY pl.AttendeeTypeOrder, pl.Nombre");
                }
            }
            //if (filtro.MaximumRows != null && filtro.MaximumRows > 0)
            //{
            //    db.AppendFormat(" LIMIT {0}", filtro.MaximumRows);
            //}
            //if (filtro.StartRowIndex != null && filtro.StartRowIndex > 0)
            //{
            //    db.AppendFormat(" OFFSET {0}", filtro.StartRowIndex);
            //}

            return db.ToString();
        }

        private void AgregarOrSiProcede(StringBuilder sb, ref bool primero)
        {
            if (primero == true)
            {
                primero = false;
                sb.Append(" WHERE ");
            }
            else sb.Append(" OR ");
        }

        #endregion

        #region "Participantes AMEC"

        public ICollection<DVParticipanteAmec> ObtenerParticipantesAMEC(FiltroParticipantes filtro)
        {
            string consulta = string.Format("select *, null as tipoactividadpax, null as tipoasistente, null as honorarios from cv_participantes_amec pl {0}", CrearSeccionWhereFiltroAMEC(filtro));
            return DVParticipanteAmec.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public long ObtenerNumeroParticipantesAMEC(FiltroParticipantes filtro)
        {
            string consulta = string.Format("SELECT count(*) FROM cv_participantes_amec pl {0}", CrearSeccionWhereFiltroAMEC(filtro));
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }

        private string CrearSeccionWhereFiltroAMEC(FiltroParticipantes filtro)
        {
            if (filtro == null) return null;

            StringBuilder db = new StringBuilder();

            bool primero = true;

            // Primero se filtra los datos Exists o No Exists si buscamos la lista de participantes ya añadidos o los no añadidos
            if (filtro.IdAmec != null || filtro.IdExpedienteConjunto != null)
            {
                AgregarAndSiProcede(db, ref primero);
                if (filtro.IdExpedienteConjunto != null)
                {
                    db.AppendFormat(" pl.idpassengerlist not in ({0}) ", filtro.IdExpedienteConjunto);
                }
                else
                {
                    db.AppendFormat(" {0} (select 1 from amecs_passengers_list rp where pl.idpassengerlist = rp.idpassengerlist) and idamec={1} ", filtro.Existe, filtro.IdAmec);
                }
            }

            if (filtro.IdParticipante != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" IdPassengerlist = {0}", filtro.IdParticipante);
            }
            if (filtro.Nombre != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" Nombre like '{0}'", filtro.Nombre.Replace("'", "''"));
            }
            if (filtro.Apel1 != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" Apel1 like '{0}'", filtro.Apel1.Replace("'", "''"));
            }
            if (filtro.Apel2 != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" Apel2 like '{0}'", filtro.Apel2.Replace("'", "''"));
            }
            if (filtro.Hospital != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" Hospital like '{0}'", filtro.Hospital.Replace("'", "''"));
            }
            if (filtro.MSDID != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" Msdid like '{0}'", filtro.MSDID);
            }
            if (filtro.IdDistrito != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" IdDistrito = {0}", filtro.IdDistrito);
            }
            if (filtro.IdRegion != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" IdRegion = {0}", filtro.IdRegion);
            }
            if (filtro.IdEmpresa != null)
            {
                AgregarAndSiProcede(db, ref primero);
                db.AppendFormat(" IdEmpresa = {0}", filtro.IdEmpresa);
            }

            if (!string.IsNullOrEmpty(filtro.SortParameter))
            {
                db.AppendFormat(" ORDER BY {0}", filtro.SortParameter);
            }
            if (filtro.MaximumRows != null && filtro.MaximumRows > 0)
            {
                db.AppendFormat(" LIMIT {0}", filtro.MaximumRows);
            }
            if (filtro.StartRowIndex != null && filtro.StartRowIndex > 0)
            {
                db.AppendFormat(" OFFSET {0}", filtro.StartRowIndex);
            }

            return db.ToString();
        }

        #endregion

        private void AgregarAndSiProcede(StringBuilder sb, ref bool primero)
        {
            if (primero == true)
            {
                primero = false;
                sb.Append(" WHERE ");
            }
            else sb.Append(" AND ");
        }


        public int ObtenerSiguienteIdPassenger()
        {
            string consulta = "SELECT MAX(idpassengerlist) FROM passengers_list";
            int max = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            return ++max;
        }

        public int ObtenerSiguienteIdReservaPassenger()
        {
            string consulta = "SELECT MAX(idreservapassengerlist) FROM reservas_passengers_list";
            int max = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            return ++max;
        }

        public int ObtenerSiguienteIdReservaAMECPassenger()
        {
            string consulta = "SELECT ISNULL(MAX(idamecpassengerlist), 0) FROM amecs_passengers_list";
            int max = int.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));

            max = max + 1;
            return max;
        }
        //Ismael Ameller 22/02/2011 Control de duplicados a la hora de dar de alta un nuevo participante
        public long ExisteDuplicadoDNI(string dni)
        {
            string consulta = string.Format("SELECT count(*) FROM passengers_list where NIF= '{0}'",dni);
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }

        public bool ExisteReserva(int idpassengerlist, int idExpediente)
        {
            string consulta = string.Format("select count(*) from reservas_passengers_list where idxpediente = {0} and idpassengerlist = {1} ", idExpediente, idpassengerlist);
            long countReserva = long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
            if (countReserva != 0)
                return true;
            else
                return false;
        }

        public long ExisteDuplicadoMSDID(string msdid)
        {
            string consulta = string.Format("SELECT count(*) FROM passengers_list where MSDID= '{0}'", msdid);
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }
        //FIN Ismael Ameller 22/02/2011 Control de duplicados a la hora de dar de alta un nuevo participante
        public ICollection<DReservasPassengersList> ObtenerReservasParticipantes(int nIdPassengerList, int nIdExpediente)
        {
            string consulta = string.Format("select * from reservas_passengers_list where idpassengerlist = {0} and idxpediente = {1}", nIdPassengerList,nIdExpediente);
            return DReservasPassengersList.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public ICollection<DAmecPassengerList> ObtenerReservasAMECParticipantes(int nIdPassengerList, int nIDAmec)
        {
            string consulta = string.Format("select * from amecs_passengers_list where idpassengerlist = {0} and idamec = {1}", nIdPassengerList, nIDAmec);
            return DAmecPassengerList.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }


        public int EliminarReservasExpediente(int nIDExpediente)
        {
            string consulta = string.Format("DELETE from reservas_passengers_list where idxpediente ={0}",nIDExpediente);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }
        
        public int EliminarReservasExpediente(int participanteEliminado, int nIDExpediente)
        {
            string consulta = string.Format("DELETE from reservas_passengers_list where idxpediente ={0} and idpassengerlist = {1}", nIDExpediente, participanteEliminado);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }


        public int EliminarReservasServiciosParticipante(int nIDExpediente)
        {
            string consulta = string.Format("delete from hotel_passengers_list where idserviciohotel in (select Servicio from cv_vista_indexservicios where idexpediente = {0} and TipoServicio='HOT') and idpassengerlist not in (select idpassengerlist from reservas_passengers_list where idxpediente = {0})", nIDExpediente);
            int nHotel = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            consulta = string.Format("delete from actividades_passengers_list where idservicioactividad in (select Servicio from cv_vista_indexservicios where idexpediente = {0} and TipoServicio='ACT') and idpassengerlist not in (select idpassengerlist from reservas_passengers_list where idxpediente = {0})", nIDExpediente);
            int nActividades = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            consulta = string.Format("delete from ins_passengers_list where idservicioinscripcion in (select Servicio from cv_vista_indexservicios where idexpediente = {0} and TipoServicio='INS') and idpassengerlist not in (select idpassengerlist from reservas_passengers_list where idxpediente = {0})", nIDExpediente);
            int nInscripcion = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            consulta = string.Format("delete from transportepassengerslist where idserviciotransporte in (select Servicio from cv_vista_indexservicios where idexpediente = {0} and TipoServicio='DSP') and idpassengerlist not in (select idpassengerlist from reservas_passengers_list where idxpediente = {0})", nIDExpediente);
            int nTransporte = Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);

            return (nHotel + nActividades + nInscripcion + nTransporte);
        }

        public int EliminarReservasAMEC(int nIDAmec)
        {
            string consulta = string.Format("DELETE from amecs_passengers_list where idamec ={0}", nIDAmec);
            return Quodem.Sql.SqlServerClient.ExecuteQuery(consulta);
        }


        public ICollection<DVServicioPassengerResumen> ObtenerParticipantesServicio(int nIdExpediente, int? nIdServicio, int nTipoSer)
        {
            string[] sTipoServicio = { "IdServicioACT", "IdServicioDSP", "IdServicioHOT", "IdServicioINS" };
            string sWhere = string.Format(" idexpediente = {0} ", nIdExpediente);
            if (nIdServicio.HasValue)
            {
                sWhere += string.Format(" AND {0} = {1} ", sTipoServicio[nTipoSer], nIdServicio.Value);
            }
            else
            {
                sWhere += string.Format(" AND {0} is not null", sTipoServicio[nTipoSer]);
            }

            string consulta = string.Format("select * from cv_ser_passenger_resumen where {0}", sWhere);

            return DVServicioPassengerResumen.ConvertToDto(Quodem.Sql.SqlServerClient.GetQuery(consulta));
        }

        public long ObtenerNumeroServiciosParticipante(int nIdExpediente, int nIdPassengerList)
        {
            string consulta = string.Format("SELECT count(*) FROM cv_ser_passenger_resumen c where IdExpediente ={0} and idpassengerlist = {1}", nIdExpediente, nIdPassengerList);
            return long.Parse(Quodem.Sql.SqlServerClient.GetValue(consulta));
        }

    }
}
