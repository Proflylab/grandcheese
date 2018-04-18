using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Util.Exceptions
{
    public class UnhandledSerializerTypeException : Exception
    {
        public UnhandledSerializerTypeException()
        {
        }

        public UnhandledSerializerTypeException(string message)
            : base(message)
        {
        }

        public UnhandledSerializerTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
