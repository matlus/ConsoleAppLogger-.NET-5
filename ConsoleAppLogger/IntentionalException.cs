using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppLogger
{


    [Serializable]
    public class IntentionalException : Exception
    {
        public IntentionalException() { }
        public IntentionalException(string message) : base(message) { }
        public IntentionalException(string message, Exception inner) : base(message, inner) { }
        protected IntentionalException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
