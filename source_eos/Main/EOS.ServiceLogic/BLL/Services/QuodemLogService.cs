using System;
using System.Linq;
using EOS.ServiceModel;
using NHibernate;
using NHibernate.Linq;

namespace EOS.ServiceLogic.BLL.Services
{
    public class QuodemLogService
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

        public QuodemLogService(ISession session)
        {
            _session = session;
        }

        public void AddLogEntry(string table, string receivedData, string resultData, string agencyKey, bool isEdit = true)
        {
            //Obtenemos la empresa asociada a la clave de agencia recibida.
            var currentAgency = new Confempresa();
            if (!string.IsNullOrWhiteSpace(agencyKey))
            {
                currentAgency = _session.Query<Confempresa>().First(x => x.Idagencia == agencyKey);
            }

            //Generamos y guardamos el nuevo item del log.
            if (isEdit)
            {
                var log = new QuodemAuditLog()
                {
                    Idconfempresa = currentAgency.Idconfempresa,
                    DatosResultado = resultData,
                    Fecha = DateTime.Now,
                    NombreTabla = table,
                    DatosRecibidos = receivedData
                };
                _session.Save(log);
            }
            else
            {
                var log = new QuodemQueryLog()
                {
                    Idconfempresa = currentAgency.Idconfempresa == 0 ? new int?() : currentAgency.Idconfempresa,
                    Resultado = resultData,
                    Fecha = DateTime.Now,
                    NombreTabla = table,
                    FiltrosRecibidos = receivedData
                };
                _session.Save(log);
            }
        }
    }
}