using System.Collections.Generic;
using EOS.ServiceLogic.Enums;

namespace EOS.ServiceLogic.Data.ServiceResponse
{
    public class SrvResponse
    {
        #region Properties

        public bool IsError
        {
            get
            {
                return ErrorList.Count > 0;
            }
        }

        public object Response { get; set; }

        public List<ErrorItem> ErrorList { get; set; }

        #endregion

        #region Constructors

        public SrvResponse()
        {
            ErrorList = new List<ErrorItem>();
        }

        public SrvResponse(string response)
            : this()
        {
            Response = response;
        }

        public SrvResponse(int code, string description)
            : this()
        {
            AddError(code, description);
        }

        #endregion

        #region Methods

        public void AddError(int code, string description)
        {
            ErrorList.Add(new ErrorItem { Code = code, Description = description });
        }

        public void AddError(ErrorCode code, string description)
        {
            ErrorList.Add(new ErrorItem { Code = code.GetHashCode(), Description = description });
        }

        #endregion
    }
}
