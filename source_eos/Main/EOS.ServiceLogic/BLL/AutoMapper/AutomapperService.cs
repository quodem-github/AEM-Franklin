using System;
using AutoMapper;
using EOS.ServiceLogic.Data.DTO;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceLogic.Data.DTO.ServiceEdition;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.BLL.AutoMapper
{
    public class AutomapperService
    {

        public static void InitializeAutomapper()
        {
            Mapper.Initialize(cfg =>
            {
                //TIPOS
                cfg.CreateMap<DateTime?, string>().ConvertUsing(new DateTimeNullableTypeConverter());
                cfg.CreateMap<string, DateTime?>().ConvertUsing(new StringDateTimeNullableTypeConverter());
                cfg.CreateMap<DateTime, string>().ConvertUsing(new DateTimeTypeConverter());
                cfg.CreateMap<string, DateTime>().ConvertUsing(new StringDateTimeTypeConverter());
                cfg.CreateMap<short?, int?>().ConvertUsing(new ShortNullableTypeConverter());
                cfg.CreateMap<int?, short?>().ConvertUsing(new IntShortNullableTypeConverter());

                cfg.CreateMap<short, int>().ConvertUsing(new ShortTypeConverter());
                cfg.CreateMap<int, short>().ConvertUsing(new IntShortTypeConverter());

                cfg.CreateMap<decimal?, double?>().ConvertUsing(new DecimalNullableTypeConverter());
                cfg.CreateMap<double?, decimal?>().ConvertUsing(new DoubleDecimalNullableTypeConverter());

                cfg.CreateMap<long?, int?>().ConvertUsing(new LongNullableTypeConverter());
                cfg.CreateMap<int?, long?>().ConvertUsing(new IntLongNullableTypeConverter());

                cfg.CreateMap<decimal, double>().ConvertUsing(new DecimalTypeConverter());
                cfg.CreateMap<double, decimal>().ConvertUsing(new DoubleDecimalTypeConverter());

                cfg.CreateMap<long, int>().ConvertUsing(new LongTypeConverter());
                cfg.CreateMap<int, long>().ConvertUsing(new IntLongTypeConverter());

                cfg.CreateMap<byte, int>().ConvertUsing(new ByteTypeConverter());
                cfg.CreateMap<int, byte>().ConvertUsing(new IntByteTypeConverter());

                cfg.CreateMap<byte?, int?>().ConvertUsing(new ByteNullableTypeConverter());
                cfg.CreateMap<int?, byte?>().ConvertUsing(new IntByteNullableTypeConverter());

                //SELECCION
                cfg.CreateMap<ActividadesPassengersList, ActividadesPassengersListDto>();
                cfg.CreateMap<Amec, AmecDto>().ForMember(x => x.Company, opt => opt.Ignore());
                cfg.CreateMap<Amecs, AmecsDto>().ForMember(x => x.Company, opt => opt.Ignore()); 
                cfg.CreateMap<Congresos, CongresosDto>();
                cfg.CreateMap<Districts, DistrictsDto>();
                cfg.CreateMap<Departaments, DepartamentsDto>();
                cfg.CreateMap<Expediente, ExpedienteDto>();
                cfg.CreateMap<Gestorinvitados, GestorInvitadosDto>();
                cfg.CreateMap<HotelPassengersList, HotelPassengerListDto>();
                cfg.CreateMap<ReservasPassengersList, ReservaPassengersDto>();
                cfg.CreateMap<Tarifasactividad, TarifasActividadDto>();
                cfg.CreateMap<Tiposcongreso, TipoCongresoDto>();
                cfg.CreateMap<Tiposinscripcion, TipoInscripcionDto>();
                cfg.CreateMap<TiposActividadCongreso, TiposActividadCongresoDto>();
                cfg.CreateMap<Tiposaloj, TiposAlojamientoDto>();
                cfg.CreateMap<TiposHab, TiposHabitacionDto>();
                cfg.CreateMap<Tramitacionesserviciosreservas, TramitacionesServiciosReservasDto>();
                cfg.CreateMap<ValoracionFi, ValoracionFiDto>();
                cfg.CreateMap<Tarifasinscripcion, TarifasInscripcionDto>();
                cfg.CreateMap<Tarifasaloj, TarifasAlojDto>();
                cfg.CreateMap<Reservasviajes, ReservaViajesDto>();
                cfg.CreateMap<Serviciosreservasactividades, ServiciosReservasActividadesDto>();                
                cfg.CreateMap<InsPassengersList, InscripcionPassengersListDto>();
                cfg.CreateMap<Transportepassengerslist, TransportePassengersListDto>();
                cfg.CreateMap<Nivelesaprobacion, NivelesAprobacionDto>();
                cfg.CreateMap<PeticionesActividad, PeticionActividadDto>();
                cfg.CreateMap<Poblaciones, PoblacionDto>();
                cfg.CreateMap<Productos, ProductoDto>();
                cfg.CreateMap<Provincias, ProvinciasDto>();
                cfg.CreateMap<Proveedores, ProveedorDto>();
                cfg.CreateMap<Serviciosreservashotel, ServiciosReservasHotelDto>();
                cfg.CreateMap<Serviciosreservasinscripciones, ServiciosReservasInscripcionDto>();
                cfg.CreateMap<Serviciosreservastransporte, ServiciosReservasTransporteDto>();
                cfg.CreateMap<Serviciosreservasviajes, ServiciosReservasViajesDto>();
                cfg.CreateMap<DatosadicionalesReservasPassenger, DatosAdicionalesReservasPassengerDto>();
                cfg.CreateMap<Comunidad, ComunidadDto>();
                cfg.CreateMap<Confempresa, ConfEmpresaDto>();
                cfg.CreateMap<Criteriosseleccion, CriteriosSeleccionDto>();
                cfg.CreateMap<Empleadosgp, EmpleadosGpDto>();
                cfg.CreateMap<PeticionariosEmpleadosgp, PeticionariosEmpleadosGPDto>();
                cfg.CreateMap<Especialidades, EspecialidadesDto>();
                cfg.CreateMap<Estadosreservas, EstadosReservasDto>();
                cfg.CreateMap<EstadosReservas, Estados_ReservasDto>();                
                cfg.CreateMap<Iatas, IatasDto>();
                cfg.CreateMap<JustificacionesDatosadicionales, JustificacionesDatosAdicionalesDto>();
                cfg.CreateMap<MensajeReservasPassengerDatosadicionales, MensajeReservasPassengerDatosAdicionalesDto>();
                cfg.CreateMap<NivelRiesgoHcpDatosadicionales, NivelRiesgoHCPDatosAdicionalesDto>();
                cfg.CreateMap<NivelRiesgoHcp, NivelRiesgoHCPDto>();
                cfg.CreateMap<ProductosPrv, ProductosPRVDto>();
                cfg.CreateMap<Salesforce, SalesForceDto>();
                cfg.CreateMap<Servicios, ServiciosDto>();
                cfg.CreateMap<TipoServicio, TipoServiciosDto>();
                cfg.CreateMap<Tipoactividad, TipoActividadDto>();
                cfg.CreateMap<TiposActividadCongreso, TiposActividadCongresoDto>();
                cfg.CreateMap<Tipospatrocinio, TiposPatrocinioDto>();
                cfg.CreateMap<TiposPrv, TiposPRVDto>();
                cfg.CreateMap<TiposreservasWeb, TiposReservasWebDto>();
                cfg.CreateMap<PeticionGrupos, PeticionGruposDto>();
                cfg.CreateMap<TipoActividadPaxDatosadicionales, TipoActividadPaxDatosAdicionalesDto>();
                cfg.CreateMap<TipoAsistenteDatosadicionales, TipoAsistenteDatosAdicionalesDto>();
                cfg.CreateMap<Tratamientos, TratamientosDto>();
                cfg.CreateMap<TiposBonos, TiposBonosDto>();
                cfg.CreateMap<Aprobadoramec, AprobadorAmecDto>();
                cfg.CreateMap<AgencyUserApprovalStructure, EstructuraOrganizativaDto>();
                cfg.CreateMap<Positions, PositionsDto>();
                cfg.CreateMap<GestordocumentalTipoSeccion, GestorDocumentalTipoSeccionDto>();
                cfg.CreateMap<GestordocumentalTipodoc, GestorDocumentalTipoDocDto>();
                cfg.CreateMap<GestordocumentalSubtipodoc, GestorDocumentalSubTipoDocDto>();
                
                //EDICION
                cfg.CreateMap<ActividadesPassengersListEditDto, ActividadesPassengersList>();
                cfg.CreateMap<ActividadesPassengersList, ActividadesPassengersListEditDto>();
                cfg.CreateMap<EmpleadosGpEditDto, Empleadosgp>();
                cfg.CreateMap<Empleadosgp, EmpleadosGpEditDto>();
                cfg.CreateMap<PeticionariosEmpleadosgp, PeticionariosEmpleadosGPEditDto>();
                cfg.CreateMap<PeticionariosEmpleadosGPEditDto, PeticionariosEmpleadosgp>();
                cfg.CreateMap<EstadosReservasEditDto, Estadosreservas>();
                cfg.CreateMap<Estadosreservas, EstadosReservasEditDto>();
                cfg.CreateMap<Estados_ReservasEditDto, EstadosReservas>();
                cfg.CreateMap<EstadosReservas, Estados_ReservasEditDto>();
                cfg.CreateMap<AmecEditDto, Amec>().ForMember(x => x.Newco, opt => opt.Ignore());
                cfg.CreateMap<Amec, AmecEditDto>();
                cfg.CreateMap<HotelPassengerListEditDto, HotelPassengersList>();
                cfg.CreateMap<HotelPassengersList, HotelPassengerListEditDto>();
                cfg.CreateMap<InscripcionPassengersListEditDto,InsPassengersList>();
                cfg.CreateMap<InsPassengersList, InscripcionPassengersListEditDto>();
                cfg.CreateMap<TransportePassengersListEditDto, Transportepassengerslist>();
                cfg.CreateMap<Transportepassengerslist, TransportePassengersListEditDto>();
                cfg.CreateMap<TipoBonosEditDto, TiposBonos>();
                cfg.CreateMap<TiposBonos, TipoBonosEditDto>();
                cfg.CreateMap<ServiciosEditDto, Servicios>();
                cfg.CreateMap<Servicios, ServiciosEditDto>();
                cfg.CreateMap<NivelesAprobacionEditDto, Nivelesaprobacion>();
                cfg.CreateMap<Nivelesaprobacion, NivelesAprobacionEditDto>();
                cfg.CreateMap<GestorInvitadosEditDto, Gestorinvitados>();
                cfg.CreateMap<Gestorinvitados, GestorInvitadosEditDto>();
                cfg.CreateMap<Tiposaloj, TiposAlojamientoEditDto>();
                cfg.CreateMap<TiposAlojamientoEditDto, Tiposaloj>();
                cfg.CreateMap<TiposPrv, TiposPRVEditDto>();
                cfg.CreateMap<TiposPRVEditDto, TiposPrv>();
                cfg.CreateMap<ReservasViajesEditDto, Reservasviajes>().ConvertUsing(new ReservasViajesReturnCustomConverter());
                cfg.CreateMap<Reservasviajes, ReservasViajesEditDto>().ConvertUsing(new ReservasViajesCustomConverter());
                cfg.CreateMap<ReservaPassengersEditDto, ReservasPassengersList>();
                cfg.CreateMap<ReservasPassengersList, ReservaPassengersEditDto>();
                cfg.CreateMap<TarifasAlojEditDto, Tarifasaloj>();
                cfg.CreateMap<Tarifasaloj, TarifasAlojEditDto>();
                cfg.CreateMap<Tramitacionesserviciosreservas, TramitacionesServiciosReservasEditDto>();
                cfg.CreateMap<TramitacionesServiciosReservasEditDto, Tramitacionesserviciosreservas>();
                cfg.CreateMap<Tarifasinscripcion, TarifasInscripcionEditDto>();
                cfg.CreateMap<TarifasInscripcionEditDto, Tarifasinscripcion>();
                cfg.CreateMap<PeticionesActividad, PeticionActividadEditDto>();
                cfg.CreateMap<PeticionActividadEditDto, PeticionesActividad>();
                cfg.CreateMap<Serviciosreservasactividades, ServiciosReservasActividadesEditDto>().ConvertUsing(new ServicioReservaActividadesCustomConverter());
                cfg.CreateMap<ServiciosReservasActividadesEditDto, Serviciosreservasactividades>().ConvertUsing(new ServicioReservaActividadesReturnCustomConverter());
                cfg.CreateMap<Congresos, CongresosEditDto>();
                cfg.CreateMap<CongresosEditDto, Congresos>();
                cfg.CreateMap<Poblaciones, PoblacionEditDto>();
                cfg.CreateMap<PoblacionEditDto, Poblaciones>();
                cfg.CreateMap<Productos, ProductoEditDto>();
                cfg.CreateMap<ProductoEditDto, Productos>();
                cfg.CreateMap<Serviciosreservashotel, ServiciosReservasHotelEditDto>();
                cfg.CreateMap<ServiciosReservasHotelEditDto, Serviciosreservashotel>();
                cfg.CreateMap<Expediente, ExpedienteEditDto>();
                cfg.CreateMap<ExpedienteEditDto, Expediente>();
                cfg.CreateMap<Tarifasactividad, TarifasActividadEditDto>();
                cfg.CreateMap<TarifasActividadEditDto, Tarifasactividad>();
                cfg.CreateMap<Iatas, IatasEditDto>();
                cfg.CreateMap<IatasEditDto, Iatas>();
                cfg.CreateMap<ProductosPrv, ProductosPRVEditDto>();
                cfg.CreateMap<ProductosPRVEditDto, ProductosPrv>();
                cfg.CreateMap<Proveedores, ProveedorEditDto>();
                cfg.CreateMap<ProveedorEditDto, Proveedores>();
                cfg.CreateMap<Provincias, ProvinciasEditDto>();
                cfg.CreateMap<ProvinciasEditDto, Provincias>();
                cfg.CreateMap<PeticionGrupos, PeticionGruposEditDto>();
                cfg.CreateMap<PeticionGruposEditDto, PeticionGrupos>().ConvertUsing(new PeticionGruposCustomConverter());
                cfg.CreateMap<Serviciosreservastransporte, ServiciosReservasTransporteEditDto>();
                cfg.CreateMap<ServiciosReservasTransporteEditDto, Serviciosreservastransporte>();
                cfg.CreateMap<Serviciosreservasinscripciones, ServiciosReservasInscripcionEditDto>();
                cfg.CreateMap<ServiciosReservasInscripcionEditDto, Serviciosreservasinscripciones>();
                cfg.CreateMap<Serviciosreservasviajes, ServiciosReservasViajesEditDto>();
                cfg.CreateMap<ServiciosReservasViajesEditDto, Serviciosreservasviajes>();
                cfg.CreateMap<GestordocumentalDocumentoversion, GestorDocumentalDocumentoVersionDto>().ConvertUsing(new GestorDocumentalDocumentoVersionCustomConverter());
                cfg.CreateMap<GestordocumentalDocumento, GestorDocumentalDocumentoDto>().ConvertUsing(new GestorDocumentalDocumentoCustomConverter());


            });
            Mapper.AssertConfigurationIsValid();
        }
    }
}
