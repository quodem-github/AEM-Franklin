using EOS.ServiceLogic.BLL.Token;
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
    public class PeticionariosService
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
                    _sessVariable = Quodem.ORM.NHibernate.Helper.GetCurrentSession(ServiceLogic.Enums.EDbConnection.Default.GetHashCode());
                }
                return _sessVariable;
            }
            set { _sessVariable = value; }
        }
        public enum ResultType { Ok = 1, PageError = 2, DateFormatError = 3, UnexpectedError = 4, DateFromBiggerThanDateTo = 5 };
        #endregion

        #region Constructor
        public PeticionariosService()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
            _logService = new QuodemLogService(_session);
        }
        #endregion

        #region Methods
        public List<ResultType> Validate(QSuscriptor suscriptor, ReceivedData encryptedFilter, string token) 
        {
            List<ResultType> result = new List<ResultType>();

            FilterPaginated filter = new FilterPaginated();
            DateTime dateFrom = DateTime.MaxValue;
            DateTime dateTo = DateTime.MaxValue;

            try
            {
                filter = DecryptData(suscriptor, encryptedFilter, token);
            }
            catch (Exception ex) 
            {
                Quodem.Monitor.Alerta.WriteLog("Excepción en PeticionariosService.Validate: " + ex.Message);
                result.Add(ResultType.UnexpectedError);
            }

            if (filter.CurrentPageIndex <= 0) 
            {
                result.Add(ResultType.PageError);
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(filter.LastUpdateDateFrom))
                {
                    dateFrom = new DateTime(int.Parse(filter.LastUpdateDateFrom.Substring(0, 4)), int.Parse(filter.LastUpdateDateFrom.Substring(4, 2)), int.Parse(filter.LastUpdateDateFrom.Substring(6, 2)));
                }
                if (!string.IsNullOrWhiteSpace(filter.LastUpdateDateTo))
                {
                    dateTo = new DateTime(int.Parse(filter.LastUpdateDateTo.Substring(0, 4)), int.Parse(filter.LastUpdateDateTo.Substring(4, 2)), int.Parse(filter.LastUpdateDateTo.Substring(6, 2)));
                }
            }
            catch (Exception ex) 
            {
                Quodem.Monitor.Alerta.WriteLog("Excepción en PeticionariosService.Validate: " + ex.Message);
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

        private FilterPeticionarriosList DecryptData(QSuscriptor suscriptor, ReceivedData encryptedFilter, string token) 
        {
            string json = Utility.Decrypt3DES(suscriptor, encryptedFilter.Data, token);

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            return json_serializer.Deserialize<FilterPeticionarriosList>(json);
        }

        private string EncryptData(QSuscriptor suscriptor, PeticionariosListDTO data, string token) 
        {
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            string encryptedResult = json_serializer.Serialize(data);

            return Utility.Encrypt3DES(suscriptor, encryptedResult, token);
        }

        public string GetList(QSuscriptor suscriptor, ReceivedData encryptedFilter, string token)
        {
            string encryptedResult = string.Empty;

            var result = new PeticionariosListDTO();

            FilterPeticionarriosList filter = DecryptData(suscriptor, encryptedFilter, token);

            DateTime dateFrom = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(filter.LastUpdateDateFrom))
            {
                dateFrom = new DateTime(int.Parse(filter.LastUpdateDateFrom.Substring(0, 4)), int.Parse(filter.LastUpdateDateFrom.Substring(4, 2)), int.Parse(filter.LastUpdateDateFrom.Substring(6, 2)));
            }
            DateTime dateTo = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(filter.LastUpdateDateFrom))
            {
                dateTo = new DateTime(int.Parse(filter.LastUpdateDateTo.Substring(0, 4)), int.Parse(filter.LastUpdateDateTo.Substring(4, 2)), int.Parse(filter.LastUpdateDateTo.Substring(6, 2)));
                dateTo = dateTo.AddDays(1);
            }

            if (filter.CurrentPageIndex > 0)
            {
                var query = _session.Query<Peticionarios>();
                if (dateFrom != DateTime.MinValue)
                {
                    query = query.Where(x => x.Lastupdatedate >= dateFrom);
                }
                if (dateFrom != DateTime.MinValue)
                {
                    query = query.Where(x => x.Lastupdatedate <= dateTo);
                }

                if (filter.IdPeticionario.HasValue)
                {
                    query = query.Where(x => x.Idpeticionario == filter.IdPeticionario.Value);
                }

                if (!string.IsNullOrWhiteSpace(filter.Login))
                {
                    query = query.Where(x => x.Login == filter.Login);
                }

                if (!string.IsNullOrWhiteSpace(filter.Wein))
                {
                    query = query.Where(x => x.Wein == filter.Wein);
                }

                if (!string.IsNullOrWhiteSpace(filter.WeinManager))
                {
                    query = query.Where(x => x.Weinmanager == filter.WeinManager);
                }

                result.PeticionarioList = query
                        .Skip((filter.CurrentPageIndex - 1)*Variables.PeticionariosListPageSize)
                        .Take(Variables.PeticionariosListPageSize)
                        .Select(x => ConvertToDTO(x))
                        .ToList();
                result.CurrentPageIndex = filter.CurrentPageIndex;
                result.LastUpdateDateFrom = filter.LastUpdateDateFrom;
                result.LastUpdateDateTo = filter.LastUpdateDateTo;
                result.IdPeticionario = filter.IdPeticionario;
                result.Wein = filter.Wein;
                result.WeinManager = filter.WeinManager;
                result.Login = filter.Login;
                result.PageSize = Variables.PeticionariosListPageSize;

                int rowCount = query.ToList().Count();

                result.PageCount = (int)Math.Ceiling((rowCount * 1.0) / Variables.PeticionariosListPageSize);
                
                encryptedResult = EncryptData(suscriptor, result, token);

                _logService.AddLogEntry("Peticionarios", JsonConvert.SerializeObject(filter), rowCount.ToString(), string.Empty, false);
            }

            return encryptedResult;
        }

        private Peticionario ConvertToDTO(Peticionarios dbObj) 
        {
            return new Peticionario
            {
                IdPeticionario = dbObj.Idpeticionario,
                Password = dbObj.Password,
                Username = dbObj.Login,
                Wein = dbObj.Wein,
                Name = dbObj.Nombre,
                LastName = dbObj.Apellido1,
                LastName2 = dbObj.Apellido2,
                Address = dbObj.Direccion,
                City = dbObj.Poblacion,
                Zip = dbObj.Codpostal,
                BusinessPhone = dbObj.Telefono,
                BusinessCellPhone = dbObj.Movil,
                Email = dbObj.Email,
                IdDepartament = dbObj.Iddepartament,
                IdSalesForce = dbObj.Idsaleforce,
                IdDistrict = dbObj.Iddistrict,
                IdManager = dbObj.Weinmanager,
                IdPosition = dbObj.Idposition,
                Medico = dbObj.Medico != null && dbObj.Medico.Value ? Variables.TRUE_VALUE : Variables.FALSE_VALUE,
                Legal = dbObj.Legal != null && dbObj.Legal.Value ? Variables.TRUE_VALUE : Variables.FALSE_VALUE,
                Executive = dbObj.Executive != null && dbObj.Executive.Value ? Variables.TRUE_VALUE : Variables.FALSE_VALUE,
                Director = dbObj.Director != null && dbObj.Director.Value ? Variables.TRUE_VALUE : Variables.FALSE_VALUE,
                Gerente = dbObj.Gerente != null && dbObj.Gerente.Value ? Variables.TRUE_VALUE : Variables.FALSE_VALUE,
                Assistant = dbObj.Assistant != null && dbObj.Assistant.Value ? Variables.TRUE_VALUE : Variables.FALSE_VALUE,
                AltoCargo = dbObj.Altocargo != null && dbObj.Altocargo.Value ? Variables.TRUE_VALUE : Variables.FALSE_VALUE,
                LevelCode = dbObj.Levelcode,
                FechaUltimaActualizacion = dbObj.Lastupdatedate != null ? dbObj.Lastupdatedate.GetValueOrDefault().ToString(Variables.DATE_TIME_FORMAT) : string.Empty,
                Inactivo = dbObj.Inactivo,
                Company = dbObj.Newco ? "ORGANON" : "MSD"
            };
        }
        #endregion
    }
}
