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
    public class PonentesDuracionActividadService
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
        public PonentesDuracionActividadService()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
        }
        #endregion

        #region Methods

        public List<PonentesDuracionActividadDto> GetList()
        {
            var resultList =
                _session.Query<CalcPonentesDuracionActividad>().ToList();
            return ConvertToDTO(resultList);
        }

        private List<PonentesDuracionActividadDto> ConvertToDTO(List<CalcPonentesDuracionActividad> dbObj)
        {
            var result = new List<PonentesDuracionActividadDto>();

            foreach (var duracionActividad in dbObj)
            {
                PonentesDuracionActividadDto honorario = new PonentesDuracionActividadDto()
                {
                    id = duracionActividad.Id,
                    Text = duracionActividad.Text,
                    Value = duracionActividad.Value
                };

                result.Add(honorario);
            }

            return result;
        }
        #endregion
    }
}
