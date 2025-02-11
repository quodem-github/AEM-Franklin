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
    public class TipoPonenteService
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
        public TipoPonenteService()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
        }
        #endregion

        #region Methods

        public List<TipoPonenteDto> GetList()
        {
            var resultList =
                _session.Query<CalcTipoPonente>().ToList();
            return ConvertToDTO(resultList);
        }

        private List<TipoPonenteDto> ConvertToDTO(List<CalcTipoPonente> dbObj)
        {
            var result = new List<TipoPonenteDto>();

            foreach (var preparacion in dbObj)
            {
                TipoPonenteDto preparacionDto = new TipoPonenteDto()
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
