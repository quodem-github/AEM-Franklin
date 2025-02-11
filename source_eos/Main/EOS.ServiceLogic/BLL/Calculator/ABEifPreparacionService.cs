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
    public class ABEifPreparacionService
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
        public ABEifPreparacionService()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
        }
        #endregion

        #region Methods

        public List<ABEifPreparacionDto> GetList()
        {
            var resultList =
                _session.Query<CalcAbEifPreparacion>().ToList();
            return ConvertToDTO(resultList);
        }

        private List<ABEifPreparacionDto> ConvertToDTO(List<CalcAbEifPreparacion> dbObj)
        {
            var result = new List<ABEifPreparacionDto>();

            foreach (var preparacionActividad in dbObj)
            {
                ABEifPreparacionDto honorario = new ABEifPreparacionDto()
                {
                    id = preparacionActividad.Id,
                    Text = preparacionActividad.Text,
                    Value = preparacionActividad.Value
                };

                result.Add(honorario);
            }

            return result;
        }
        #endregion
    }
}
