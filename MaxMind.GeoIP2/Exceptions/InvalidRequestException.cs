#region

using System;
using System.Runtime.Serialization;

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
#pragma warning disable IDE0003 // Mono gets confused if 'this' is missing
            this.Uri = uri;
#pragma warning restore IDE0003
        }


        /// <summary>
        ///     Constructor for deserialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Code = info.GetString("MaxMind.GeoIP2.Exceptions.InvalidRequestException.Code") 
                ?? throw new SerializationException("Unexcepted null Code value");
            Uri = (Uri)(info.GetValue("MaxMind.GeoIP2.Exceptions.InvalidRequestException.Uri", typeof(Uri))
                ?? throw new SerializationException("Unexcepted null Uri value"));
        }

        /// <summary>
        ///     Method to serialize data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("MaxMind.GeoIP2.Exceptions.InvalidRequestException.Code", Code);
            info.AddValue("MaxMind.GeoIP2.Exceptions.InvalidRequestException.Uri", Uri, typeof(Uri));
        }

        /// <summary>
        ///     The error code returned by the web service.
        /// </summary>
        public string Code { get; }

        /// <summary>
        ///     The URI queried by the web service.
        /// </summary>
        public Uri Uri { get; }
    }
}
