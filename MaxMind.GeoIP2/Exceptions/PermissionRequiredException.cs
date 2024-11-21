﻿#region

using System;
using System.Runtime.Serialization;

#endregion

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    /// This class represents an authentication error.
    /// </summary>
    [Serializable]
    public class PermissionRequiredException : GeoIP2Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public PermissionRequiredException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">The underlying exception that caused this one.</param>
        public PermissionRequiredException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Constructor for deserialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
#if NET8_0_OR_GREATER
        [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
        protected PermissionRequiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
