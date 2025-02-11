using EOS.ServiceLogic.Data.DTO;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.BLL
{
    public interface IServiceExtended : IService
    {
        ServiceError ValidDataDelete(QSuscriptor suscriptor, string data);
        SrvResponse Delete(QSuscriptor suscriptor);
    }
}
