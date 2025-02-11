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
    public class TipoReunionService
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
        public TipoReunionService()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
        }
        #endregion

        #region Methods

        public List<TipoReunionDto> GetList()
        {
            var resultList =
                _session.Query<CalcTipoReunion>().ToList();
            return ConvertToDTO(resultList);
        }

        private List<TipoReunionDto> ConvertToDTO(List<CalcTipoReunion> dbObj)
        {
            var result = new List<TipoReunionDto>();

            foreach (var preparacion in dbObj)
            {
                TipoReunionDto preparacionDto = new TipoReunionDto()
                {
                    id = preparacion.Id,
                    Tipo = preparacion.Tipo
                };

                result.Add(preparacionDto);
            }

            return result;
        }
        #endregion
    }
}
