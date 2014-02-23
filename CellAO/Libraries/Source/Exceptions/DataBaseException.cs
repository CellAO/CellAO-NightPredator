using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Core.Exceptions
{

    [Serializable]
    public class DataBaseException : ApplicationException
    {
        public DataBaseException() : base() { }

        public DataBaseException(string message) : base(message) { }

        public DataBaseException(string message, Exception innerException) : base(message, innerException) { }

    }
}
