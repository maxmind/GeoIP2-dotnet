#region

using System;
#if !NETSTANDARD1_4
using System.Runtime.Serialization;
using System.Security.Permissions;
#endif
#endregion

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    ///     This exception is thrown when your account does not have any queries remaining for the called service.
    /// </summary>
#if !NETSTANDARD1_4
    [Serializable]
#endif
    public class OutOfQueriesException : GeoIP2Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OutOfQueriesException" /> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public OutOfQueriesException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OutOfQueriesException" /> class.
        /// </summary>
        /// <param name="message">A message explaining the cause of the error.</param>
        /// <param name="innerException">The inner exception.</param>
        public OutOfQueriesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if !NETSTANDARD1_4
        /// <summary>
        ///     Constructor for deserialization.
        /// </summary>
        /// <param name="info">The SerializationInfo with data.</param>
        /// <param name="context">The source for this deserialization.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected OutOfQueriesException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
