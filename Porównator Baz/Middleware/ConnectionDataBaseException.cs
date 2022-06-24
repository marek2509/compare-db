using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Middleware
{
    class ConnectionDataBaseException : Exception
    {
        public ConnectionDataBaseException()
        {
        }

        public ConnectionDataBaseException(string message) : base(message)
        {
        }
        public ConnectionDataBaseException(string message, Exception inner) : base(message, inner) { }
    }
}
