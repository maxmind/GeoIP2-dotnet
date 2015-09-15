#region

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

#endregion

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    ///     This class represents a non-specific error returned by MaxMind's GeoIP2 web
    ///     service. This occurs when the web service is up and responding to requests,
    ///     but the request sent was invalid in some way.
    /// </summary>
    [Serializable]
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
            this.Uri = uri;
        }

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
            this.Uri = (Uri)info.GetValue("Uri", typeof(Uri));
        }

        /// <summary>
        ///     The error code returned by the web service.
        /// </summary>
        public string Code { get; }

        /// <summary>
        ///     The URI queried by the web service.
        /// </summary>
        public Uri Uri { get; }

        /// <summary>
        ///     Populates a SerializationInfo with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The SerializationInfo to populate with data.</param>
        /// <param name="context">The destination (see StreamingContext) for this serialization.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            info.AddValue("Code", Code);
            info.AddValue("Uri", this.Uri);
            base.GetObjectData(info, context);
        }
    }
}