using System.Collections.Generic;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ServiceError
    {
        public bool IsError { get; set; }
        public List<ServiceErrorItem> ErrorList { get; set; }
    }
}
