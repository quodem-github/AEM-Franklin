using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.BLL
{
    public interface IServiceBase
    {
        ServiceError ValidFilter(QSuscriptor suscriptor, string data);
        string GetList(QSuscriptor suscriptor);
    }
}
