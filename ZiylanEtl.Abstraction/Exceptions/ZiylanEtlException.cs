using System;

namespace ZiylanEtl.Abstraction.Exceptions
{
    public class ZiylanEtlException : Exception
    {

        public ZiylanEtlException()
            : base()
        {

        }

        public ZiylanEtlException(string message)
            : base(message)
        {

        }

        public ZiylanEtlException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}
