#region

using System;
#endregion

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    ///     This exception is thrown when there is an authentication error.
    /// </summary>
    public class AuthenticationException : GeoIP2Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthenticationException" /> class.
        /// </summary>
        /// <param name="message">A message explaining the cause of the error.</param>
        public AuthenticationException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthenticationException" /> class.
        /// </summary>
        /// <param name="message">A message explaining the cause of the error.</param>
        /// <param name="innerException">The inner exception.</param>
        public AuthenticationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
