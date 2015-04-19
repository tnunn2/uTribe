using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urTribeWebAPI.Common.Logging
{
    public class Logger
    {
        #region Member Variables
        private static readonly Logger _instance = new Logger();
        private readonly Queue<ExceptionDTO> _exceptionQueue;
        #endregion

        #region Properties
        public static Logger Instance
        {
            get
            {
                return _instance;
            }
        }
        public ExceptionDTO Log
        {
            set 
            {
                _exceptionQueue.Enqueue(value);
            }
        }
        #endregion

        #region Constructor
        private Logger ()
        {
            _exceptionQueue = new Queue<ExceptionDTO>();
        }
        #endregion

        #region Public Method
        #endregion

        #region Private Method
        #endregion
    }
}
