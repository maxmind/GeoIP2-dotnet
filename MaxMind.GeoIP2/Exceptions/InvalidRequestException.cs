#region

using System;
#if !NETSTANDARD1_4
using System.Runtime.Serialization; 
using System.Security;
using System.Security.Permissions;
#endif
#endregion

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    ///     This class represents a non-specific error returned by MaxMind's GeoIP2 web
    ///     service. This occurs when the web service is up and responding to requests,
    ///     but the request sent was invalid in some way.
    /// </summary>
#if !NETSTANDARD1_4
    [Serializable]
#endif
    public class InvalidRequestException : GeoIP2Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidRequestException" /> class.
        /// </summary>
        /// <param name="message">A message explaining the cause of the error.</param>
        /// <param name="code">The error code returned by the web service.</param>
        /// <param name="uri">The URL queried.</param>
        public InvalidRequestException(string message, string code, Uri uri)
            : base(message)
        {
            Code = code;
#pragma warning disable IDE0003 // Mono gets confused if 'this' is missing
            this.Uri = uri;
#pragma warning restore IDE0003
        }

#if !NETSTANDARD1_4
        /// <summary>
        ///     Constructor for deserialization.
        /// </summary>
        /// <param name="info">The SerializationInfo with data.</param>
        /// <param name="context">The source for this deserialization.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected InvalidRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Code = info.GetString("Code");
#pragma warning disable IDE0003 // Mono gets confused if 'this' is missing
            this.Uri = (Uri)info.GetValue("Uri", typeof(Uri));
#pragma warning restore IDE0003
        }
#endif

        /// <summary>
        ///     The error code returned by the web service.
        /// </summary>
        public string Code { get; }

        /// <summary>
        ///     The URI queried by the web service.
        /// </summary>
        public Uri Uri { get; }

#if !NETSTANDARD1_4
        /// <summary>
        ///     Populates a SerializationInfo with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The SerializationInfo to populate with data.</param>
        /// <param name="context">The destination (see StreamingContext) for this serialization.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            info.AddValue("Code", Code);
#pragma warning disable IDE0003 // Mono gets confused if 'this' is missing
            info.AddValue("Uri", this.Uri);
#pragma warning restore IDE0003
            base.GetObjectData(info, context);
        }
#endif
    }
}
