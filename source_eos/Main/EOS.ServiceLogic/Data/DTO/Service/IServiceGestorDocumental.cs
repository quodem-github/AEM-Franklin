using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using EOS.ServiceLogic.BLL;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public interface IServiceGestorDocumental:IService
    {
        ServiceError ValidDataEditor(QSuscriptor suscriptor, string data);
        SrvResponse Delete(QSuscriptor suscriptor);
        string GetListTipoSeccion(QSuscriptor suscriptor);
        string GetListTipoDoc(QSuscriptor suscriptor);
        string GetListSubTipoDoc(QSuscriptor suscriptor);
        string GetListDoc(QSuscriptor suscriptor);
        string GetListDocVersion(QSuscriptor suscriptor);
        SrvResponse SaveFile(QSuscriptor suscriptor, string fileName,string tempDirectory,string targetDirectory, HttpContext context);
    }
}
