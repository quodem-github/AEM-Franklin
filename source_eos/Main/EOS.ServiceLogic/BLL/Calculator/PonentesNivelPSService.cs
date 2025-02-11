using EOS.ServiceLogic.Data;
using EOS.ServiceLogic.Data.DTO;
using EOS.ServiceLogic.Data.DTO.Calculator;
using EOS.ServiceModel;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace EOS.ServiceLogic.BLL.Calculator
{
    public class PonentesNivelPSService
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
        public PonentesNivelPSService()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
        }
        #endregion

        #region Methods

        public List<PonentesNivelPSDto> GetList()
        {
            var resultList =
                _session.Query<CalcPonentesNivelPs>()
                    .Where(
                        x =>
                            (x.FechaInicioVigencia == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : x.FechaInicioVigencia) <
                            DateTime.Now &&
                            (x.FechaFinVigencia == null ? System.Data.SqlTypes.SqlDateTime.MaxValue.Value : x.FechaFinVigencia) > DateTime.Now)
                    .ToList();
            return ConvertToDTO(resultList);
        }

        private List<PonentesNivelPSDto> ConvertToDTO(List<CalcPonentesNivelPs> dbObj)
        {
            var result = new List<PonentesNivelPSDto>();

            foreach (var nivelPs in dbObj)
            {
                PonentesNivelPSDto nivel = new PonentesNivelPSDto()
                {
                    id = nivelPs.Id,
                    FechaInicioVigencia = nivelPs.FechaInicioVigencia == null ? DateTime.MinValue : nivelPs.FechaInicioVigencia.Value,
                    FechaFinVigencia = nivelPs.FechaFinVigencia == null ? DateTime.MaxValue : nivelPs.FechaFinVigencia.Value,
                    Text = nivelPs.Text,
                    Value = nivelPs.Value
                };

                result.Add(nivel);
            }

            return result;
        }
        #endregion
    }
}
