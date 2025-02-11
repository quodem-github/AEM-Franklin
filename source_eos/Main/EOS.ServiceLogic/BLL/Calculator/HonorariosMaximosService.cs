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
    public class HonorariosMaximosService
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
        public HonorariosMaximosService()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
        }
        #endregion

        #region Methods

        public List<HonorariosMaximosDto> GetList()
        {

            

            var resultList =
                _session.Query<CalcHonorariosMaximos>()
                    .Where(
                        x =>
                            (x.FechaInicioVigencia == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : x.FechaInicioVigencia.Value) <
                            DateTime.Now &&
                            (x.FechaFinVigencia == null ? System.Data.SqlTypes.SqlDateTime.MaxValue.Value : x.FechaFinVigencia.Value) > DateTime.Now)
                    .ToList();
            return ConvertToDTO(resultList);
        }

        private List<HonorariosMaximosDto> ConvertToDTO(List<CalcHonorariosMaximos> dbObj)
        {
            var result = new List<HonorariosMaximosDto>();

            foreach (var honorarioMaximo in dbObj)
            {
                HonorariosMaximosDto honorario = new HonorariosMaximosDto()
                {
                    id = honorarioMaximo.Id,
                    FechaInicioVigencia = honorarioMaximo.FechaInicioVigencia == null ? DateTime.MinValue : honorarioMaximo.FechaInicioVigencia.Value,
                    FechaFinVigencia = honorarioMaximo.FechaFinVigencia == null ? DateTime.MaxValue : honorarioMaximo.FechaFinVigencia.Value,
                    Colspan = honorarioMaximo.Colspan,
                    Text = honorarioMaximo.Text,
                    Value = honorarioMaximo.Value,
                    Value2 = honorarioMaximo.Value2,
                    FloatValue = honorarioMaximo.Floatminvalue.GetValueOrDefault(),
                    SpecialCase = honorarioMaximo.Specialcase,
                    Visible = honorarioMaximo.Visible
                };

                result.Add(honorario);
            }

            return result;
        }
        #endregion
    }
}
