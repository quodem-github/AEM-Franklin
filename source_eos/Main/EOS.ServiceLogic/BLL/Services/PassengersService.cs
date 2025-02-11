using EOS.ServiceLogic.Data;
using EOS.ServiceLogic.Data.DTO;
using EOS.ServiceModel;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using EOS.ServiceLogic.Data.Filters;
using Newtonsoft.Json;

namespace EOS.ServiceLogic.BLL.Services
{
    public class PassengersService
    {
        #region Definitions
        private QuodemLogService _logService;
        private ISession _sessVariable;
        public ISession _session
        {
            get
            {
                if (!_sessVariable.IsOpen)
                {
                    _sessVariable = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
                }
                return _sessVariable;
            }
            set { _sessVariable = value;}
        }

        public enum ResultType { Ok = 1, PageError = 2, DateFormatError = 3, UnexpectedError = 4, DateFromBiggerThanDateTo = 5 };
        #endregion

        #region Constructor
        public PassengersService()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
            _logService = new QuodemLogService(_session);
        }
        #endregion

        #region Methods
        public List<ResultType> Validate(QSuscriptor suscriptor, ReceivedData encryptedFilter, string token)
        {
            List<ResultType> result = new List<ResultType>();

            FilterPassengerList filter = new FilterPassengerList();
            DateTime dateFrom = DateTime.MinValue;
            DateTime dateTo = DateTime.MaxValue;

            try
            {
                filter = DecryptData(suscriptor, encryptedFilter, token);
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Excepción en PassengersService.Validate: " + ex.Message);
                result.Add(ResultType.UnexpectedError);
            }

            if (filter.CurrentPageIndex <= 0)
            {
                result.Add(ResultType.PageError);
            }

            try
            {
                if (!String.IsNullOrWhiteSpace(filter.LastUpdateDateFrom))
                {
                    dateFrom = new DateTime(int.Parse(filter.LastUpdateDateFrom.Substring(0, 4)), int.Parse(filter.LastUpdateDateFrom.Substring(4, 2)), int.Parse(filter.LastUpdateDateFrom.Substring(6, 2)));
                }
                if (!String.IsNullOrWhiteSpace(filter.LastUpdateDateTo))
                {
                    dateTo = new DateTime(int.Parse(filter.LastUpdateDateTo.Substring(0, 4)), int.Parse(filter.LastUpdateDateTo.Substring(4, 2)), int.Parse(filter.LastUpdateDateTo.Substring(6, 2)));
                    dateTo = dateTo.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Excepción en PassengersService.Validate: " + ex.Message);
                result.Add(ResultType.DateFormatError);
            }

            if (dateFrom != DateTime.MaxValue && dateTo != DateTime.MaxValue && dateFrom > dateTo) 
            {
                result.Add(ResultType.DateFromBiggerThanDateTo);
            }

            if (result.Count > 0)
            {
                return result;
            }

            return new List<ResultType>() { ResultType.Ok };
        }

        private FilterPassengerList DecryptData(QSuscriptor suscriptor, ReceivedData encryptedFilter, string token)
        {
            string json = Utility.Decrypt3DES(suscriptor, encryptedFilter.Data, token);

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            return json_serializer.Deserialize<FilterPassengerList>(json);
        }

        private string EncryptData(QSuscriptor suscriptor, PassengersListDTO data, string token)
        {
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            string encryptedResult = json_serializer.Serialize(data);

            return Utility.Encrypt3DES(suscriptor, encryptedResult, token);
        }

        public string GetList(QSuscriptor suscriptor, ReceivedData encryptedFilter, string token)
        {
            string encryptedResult = string.Empty;

            PassengersListDTO result = new PassengersListDTO();

            FilterPassengerList filter = DecryptData(suscriptor, encryptedFilter, token);

            DateTime dateFrom = DateTime.MinValue;
            DateTime dateTo = DateTime.MaxValue;

            if (!String.IsNullOrWhiteSpace(filter.LastUpdateDateFrom))
            {
                dateFrom = new DateTime(int.Parse(filter.LastUpdateDateFrom.Substring(0, 4)), int.Parse(filter.LastUpdateDateFrom.Substring(4, 2)), int.Parse(filter.LastUpdateDateFrom.Substring(6, 2)));
            }
            if (!String.IsNullOrWhiteSpace(filter.LastUpdateDateTo))
            {
                dateTo = new DateTime(int.Parse(filter.LastUpdateDateTo.Substring(0, 4)), int.Parse(filter.LastUpdateDateTo.Substring(4, 2)), int.Parse(filter.LastUpdateDateTo.Substring(6, 2)));
                dateTo = dateTo.AddDays(1);
            }

            if (filter.CurrentPageIndex > 0)
            {
                var query = _session.Query<PassengersList>();

                if (dateFrom != DateTime.MinValue)
                {
                    query = query.Where(x => (x.Updatedate >= dateFrom));
                }
                if (dateTo != DateTime.MaxValue)
                {
                    query = query.Where(x => (x.Updatedate <= dateTo));
                }
                if (filter.IdPassengerList != null)
                {
                    query = query.Where(x => x.Idpassengerlist == filter.IdPassengerList.Value);
                }

                if (!string.IsNullOrWhiteSpace(filter.Msdid))
                {
                    query = query.Where(x => x.Msdid == filter.Msdid);
                }

                if (filter.Internacional != null)
                {
                    query = query.Where(x => x.Internacional == filter.Internacional.Value);
                }

                if (filter.Peticionario != null)
                {
                    query = query.Where(x => x.Peticionario == filter.Peticionario.Value);
                }

                if (!string.IsNullOrWhiteSpace(filter.GoldenId))
                {
                    query = query.Where(x => x.Goldenid == filter.GoldenId);
                }

                if (!string.IsNullOrWhiteSpace(filter.GenesysCode))
                {
                    query = query.Where(x => x.Genesyscode == filter.GenesysCode);
                }

                result.PassengerList = query.Skip((filter.CurrentPageIndex - 1) * Variables.PassengersListPageSize).Take(Variables.PassengersListPageSize).Select(x => ConvertToDTO(x)).ToList();
                result.CurrentPageIndex = filter.CurrentPageIndex;
                result.LastUpdateDateFrom = filter.LastUpdateDateFrom;
                result.LastUpdateDateTo = filter.LastUpdateDateTo;
                result.GenesysCode = filter.GenesysCode;
                result.GoldenId = filter.GoldenId;
                result.IdPassengerList = filter.IdPassengerList;
                result.Internacional = filter.Internacional;
                result.Msdid = filter.Msdid;
                result.IdPassengerList = filter.IdPassengerList;
                result.PageSize = Variables.PassengersListPageSize;

                int rowCount = query.Count();
                
                result.PageCount = (int)Math.Ceiling((rowCount * 1.0) / Variables.PassengersListPageSize);

                encryptedResult = EncryptData(suscriptor, result, token);

                _logService.AddLogEntry("Passengers", JsonConvert.SerializeObject(filter), rowCount.ToString(), string.Empty, false);
            }

            return encryptedResult;
        }

        private Passenger ConvertToDTO(PassengersList dbObj)
        {
            Passenger result = new Passenger();

            result.apel1 = dbObj.Apel1;
            result.apel2 = dbObj.Apel2;
            result.CodigoPostal = dbObj.Codigopostal;
            result.Descuentoresidente = dbObj.Descuentoresidente;
            result.DireccionCentroTrabajo = dbObj.Direccioncentrotrabajo;
            result.DireccionEmail = dbObj.Direccionemail;
            result.DireccionProfesionalOrganizacion = dbObj.Direccionprofesionalorganizacion;
            result.FechaCaducidadPasaporte = dbObj.Fechacaducidadpasaporte != null ? dbObj.Fechacaducidadpasaporte.GetValueOrDefault().ToString(Variables.DATE_TIME_FORMAT): string.Empty;
            result.FechaNacimiento = dbObj.Fechanacimiento != null ? dbObj.Fechanacimiento.GetValueOrDefault().ToString(Variables.DATE_TIME_FORMAT) : string.Empty;
            result.FechaUltimaActualizacion = dbObj.Updatedate != null ? dbObj.Updatedate.GetValueOrDefault().ToString(Variables.DATE_TIME_FORMAT) : string.Empty;
            result.Iddistrito = dbObj.Iddistrito ?? -1;
            result.Idespecialidad = dbObj.Idespecialidad ?? -1;
            result.Idnivelriesgo = (int)dbObj.Idnivelriesgo;
            result.Idpassengerlist = dbObj.Idpassengerlist;
            result.Idtratamiento = dbObj.Idtratamiento ?? -1;
            result.Inactivo = dbObj.Inactivo ?? -1;
            result.Localidad = dbObj.Localidad;
            result.LocalidadCentroTrabajo = dbObj.Localidadcentrotrabajo;
            result.Locked = dbObj.Locked ?? -1;
            result.LugarEmisionPasaporte = dbObj.Lugaremisionpasaporte;
            result.msdid = dbObj.Msdid;
            result.NacionalidadPasaporte = dbObj.Nacionalidadpasaporte;
            result.NIF = dbObj.Nif;
            result.Nombre = dbObj.Nombre;
            result.NombreCentroTrabajo = dbObj.Nombrecentrotrabajo;
            result.OrganizacionVisitaProfesional = dbObj.Organizacionvisitaprofesional;
            result.Pais = dbObj.Pais;
            result.Pasaporte = dbObj.Pasaporte;
            result.Provincia = dbObj.Provincia;
            result.TelefonoContacto = dbObj.Telefonocontacto;
            result.Centercode = dbObj.Centercode;
            result.Internacional = dbObj.Internacional == null ? 0 : dbObj.Internacional.Value;
            result.GoldenId = dbObj.Goldenid;
            result.GenesysCode = dbObj.Genesyscode;
            //result.Peticionario = dbObj.Peticionario == null ? 0 : dbObj.Peticionario.Value;
            result.IdpassengerlistOriginGP = dbObj.IdpassengerlistoriginGp;
            result.IdpassengerlistOriginMT = dbObj.IdpassengerlistoriginMt;
            result.IdpassengerlistOriginAMEX = dbObj.IdpassengerlistoriginAmex;

            return result;
        }
        #endregion
    }
}
