using EOS.ServiceLogic.Data.DTO;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.BLL
{
    public interface IService : IServiceBase
    {
        ServiceError ValidData(QSuscriptor suscriptor, string data);
        SrvResponse Edit(QSuscriptor suscriptor);
    }
}
