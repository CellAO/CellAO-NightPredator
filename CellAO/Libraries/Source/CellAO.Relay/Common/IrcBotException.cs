using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellAO.Relay.Common
{
    public class IrcBotException : Exception
    {
        public IrcBotException(IrcBotExceptionType type, string message)
            : base(message)
        {
        }
    }

    public enum IrcBotExceptionType
    {
        Unknown,
        NoConnection,
    }
}
