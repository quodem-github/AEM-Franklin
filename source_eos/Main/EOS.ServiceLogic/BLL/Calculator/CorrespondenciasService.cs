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
    public class CorrespondenciasService
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
        public CorrespondenciasService()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
        }
        #endregion

        #region Methods

        public List<CorrespondenciasDto> GetList()
        {
            var resultList =
                _session.Query<CalcCorrespondencias>().ToList();
            return ConvertToDTO(resultList);
        }

        private List<CorrespondenciasDto> ConvertToDTO(List<CalcCorrespondencias> dbObj)
        {
            var result = new List<CorrespondenciasDto>();

            foreach (var correspondencia in dbObj)
            {
                CorrespondenciasDto correspondenciaDto = new CorrespondenciasDto()
                {
                    Id = correspondencia.Id,
                    IdTipoAsistente = correspondencia.Idtipoasistente,
                    IdTipoReunion = correspondencia.Idtiporeunion,
                    IdTipoPsPonentes = correspondencia.Idtipopsponentes,
                    IdTipoNacionalLocal = correspondencia.Idtiponacionallocal,
                    IdDuracionActividadPonentes = correspondencia.Idduracionactividadponentes,
                    IdTiempoPreparacionPonentes = correspondencia.Idtiempopreparacionponentes,
                    IdTipoPsAbEif = correspondencia.Idtipopsabeif,
                    IdDuracionActividadAbEif = correspondencia.Idduracionactividadabeif,
                    IdTiempoPreparacionAbEif = correspondencia.Idtiempopreparacionabeif,
                    PonenciaCentroSalud = correspondencia.Ponenciacentrosalud,
                    TallerOCurso = correspondencia.Tallerocurso,
                    VideoconferenciaRepetida = correspondencia.Videoconferenciarepetida,
                    IdCalcHonorariosMaximos = correspondencia.Idcalchonorariosmaximos,
                    IdTipoContratoConsultoria = correspondencia.Idtipocontratoconsultoria
                };

                result.Add(correspondenciaDto);
            }

            return result;
        }

        #endregion
    }
}
