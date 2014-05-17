using System;
using System.Runtime.Serialization;

namespace Koalite.EFSample
{
    public static class Check
    {
        public static void Require(bool condition, string message = null)
        {
            if (!condition)
                throw new DBCException(message ?? "Precondition failed");
        }
    }

    public class DBCException : Exception
    {
        public DBCException()
        {
        }

        public DBCException(string message) : base(message)
        {
        }

        public DBCException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DBCException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
