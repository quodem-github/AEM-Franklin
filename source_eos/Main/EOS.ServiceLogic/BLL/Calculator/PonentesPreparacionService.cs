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
    public class PonentesPreparacionService
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
        public PonentesPreparacionService()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
        }
        #endregion

        #region Methods

        public List<PonentesPreparacionDto> GetList()
        {
            var resultList =
                _session.Query<CalcPonentesPreparacion>().ToList();
            return ConvertToDTO(resultList);
        }

        private List<PonentesPreparacionDto> ConvertToDTO(List<CalcPonentesPreparacion> dbObj)
        {
            var result = new List<PonentesPreparacionDto>();

            foreach (var preparacion in dbObj)
            {
                PonentesPreparacionDto preparacionDto = new PonentesPreparacionDto()
                {
                    id = preparacion.Id,
                    Text = preparacion.Text,
                    Value = preparacion.Value
                };

                result.Add(preparacionDto);
            }

            return result;
        }
        #endregion
    }
}
