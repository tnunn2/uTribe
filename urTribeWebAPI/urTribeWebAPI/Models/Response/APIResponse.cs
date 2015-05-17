using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urTribeWebAPI.Models.Response
{
    //Based on JSend standard:  http://labs.omniti.com/labs/jsend
    public class APIResponse
    {
        #region enum
        public enum ReponseStatus {success, fail, error}
        #endregion

        #region Member Variable
        private ReponseStatus _response;
        private object _data;
        #endregion

        #region Properties
        public string Status
        {
            get
            {
                return _response.ToString();
            }
            private set
            {
                _response = (ReponseStatus)Enum.Parse(typeof(ReponseStatus), value);
            }
        }
        public object Data
        {
            get
            {
                return _data;
            }
            private set
            {
                _data = value;
            }
        }
        #endregion

        #region Constructor
        public APIResponse (ReponseStatus status, object data)
        {
            _response = status;
            _data = data;
        }
        #endregion
    }
}