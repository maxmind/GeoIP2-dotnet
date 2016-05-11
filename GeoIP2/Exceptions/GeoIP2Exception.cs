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
    ///     This class represents a generic GeoIP2 error. All other exceptions thrown by
    ///     the GeoIP2 API subclass this exception
    /// </summary>
#if !NETSTANDARD1_4
    [Serializable]
    public class GeoIP2Exception : ApplicationException
#else
    public class GeoIP2Exception : Exception
#endif
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GeoIP2Exception" /> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public GeoIP2Exception(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GeoIP2Exception" /> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">The inner exception.</param>
        public GeoIP2Exception(string message, Exception innerException)
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
        protected GeoIP2Exception(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
