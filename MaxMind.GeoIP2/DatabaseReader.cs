#region

using MaxMind.Db;
using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

#endregion

namespace MaxMind.GeoIP2
{
    /// <summary>
    ///     Instances of this class provide a reader for the GeoIP2 database format
    /// </summary>
    public class DatabaseReader : IGeoIP2DatabaseReader, IDisposable
    {
        private bool _disposed;
        private readonly IReadOnlyList<string> _locales;
        private readonly Reader _reader;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseReader" /> class.
        /// </summary>
        /// <param name="file">The MaxMind DB file.</param>
        /// <param name="mode">The mode by which to access the DB file.</param>
        public DatabaseReader(string file, FileAccessMode mode = FileAccessMode.MemoryMapped)
            : this(file, new List<string> { "en" }, mode)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseReader" /> class.
        /// </summary>
        /// <param name="file">The MaxMind DB file.</param>
        /// <param name="locales">List of locale codes to use in name property from most preferred to least preferred.</param>
        /// <param name="mode">The mode by which to access the DB file.</param>
        public DatabaseReader(string file, IEnumerable<string> locales,
            FileAccessMode mode = FileAccessMode.MemoryMapped)
        {
            _locales = new List<string>(locales).AsReadOnly();
            _reader = new Reader(file, mode);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseReader" /> class.
        /// </summary>
        /// <param name="stream">A stream of the MaxMind DB file.</param>
        public DatabaseReader(Stream stream)
            : this(stream, new List<string> { "en" })
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseReader" /> class.
        /// </summary>
        /// <param name="stream">A stream of the MaxMind DB file.</param>
        /// <param name="locales">List of locale codes to use in name property from most preferred to least preferred.</param>
        public DatabaseReader(Stream stream, IEnumerable<string> locales)
        {
            _locales = new List<string>(locales);
            _reader = new Reader(stream);
        }

        /// <summary>
        ///     The metadata for the open MaxMind DB file.
        /// </summary>
        public Metadata Metadata => _reader.Metadata;

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _reader.Dispose();
            }

            _disposed = true;
        }

        /// <summary>
        ///     Returns an <see cref="CountryResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CountryResponse" /></returns>
        public CountryResponse Country(IPAddress ipAddress)
        {
            return Execute<CountryResponse>(ipAddress, "Country")!;
        }

        /// <summary>
        ///     Returns an <see cref="CountryResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CountryResponse" /></returns>
        public CountryResponse Country(string ipAddress)
        {
            return Execute<CountryResponse>(ipAddress, "Country")!;
        }

        /// <summary>
        ///     Tries to lookup a <see cref="CountryResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="CountryResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryCountry(IPAddress ipAddress, out CountryResponse? response)
        {
            response = Execute<CountryResponse>(ipAddress, "Country", false);
            return response != null;
        }

        /// <summary>
        ///     Tries to lookup a <see cref="CountryResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="CountryResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryCountry(string ipAddress, out CountryResponse? response)
        {
            response = Execute<CountryResponse>(ipAddress, "Country", false);
            return response != null;
        }

        /// <summary>
        ///     Returns an <see cref="CityResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CityResponse" /></returns>
        public CityResponse City(IPAddress ipAddress)
        {
            return Execute<CityResponse>(ipAddress, "City")!;
        }

        /// <summary>
        ///     Returns an <see cref="CityResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CityResponse" /></returns>
        public CityResponse City(string ipAddress)
        {
            return Execute<CityResponse>(ipAddress, "City")!;
        }

        /// <summary>
        ///     Tries to lookup a <see cref="CityResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="CityResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryCity(IPAddress ipAddress, out CityResponse? response)
        {
            response = Execute<CityResponse>(ipAddress, "City", false);
            return response != null;
        }

        /// <summary>
        ///     Tries to lookup a <see cref="CityResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="CityResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryCity(string ipAddress, out CityResponse? response)
        {
            response = Execute<CityResponse>(ipAddress, "City", false);
            return response != null;
        }

        /// <summary>
        ///     Look up an IP address in a GeoIP2 Anonymous IP.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="AnonymousIPResponse" /></returns>
        public AnonymousIPResponse AnonymousIP(IPAddress ipAddress)
        {
            return Execute<AnonymousIPResponse>(ipAddress, "GeoIP2-Anonymous-IP")!;
        }

        /// <summary>
        ///     Look up an IP address in a GeoIP2 Anonymous IP.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="AnonymousIPResponse" /></returns>
        public AnonymousIPResponse AnonymousIP(string ipAddress)
        {
            return Execute<AnonymousIPResponse>(ipAddress, "GeoIP2-Anonymous-IP")!;
        }

        /// <summary>
        ///     Tries to lookup an <see cref="AnonymousIPResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="AnonymousIPResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryAnonymousIP(IPAddress ipAddress, out AnonymousIPResponse? response)
        {
            response = Execute<AnonymousIPResponse>(ipAddress, "GeoIP2-Anonymous-IP", false);
            return response != null;
        }

        /// <summary>
        ///     Tries to lookup an <see cref="AnonymousIPResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="AnonymousIPResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryAnonymousIP(string ipAddress, out AnonymousIPResponse? response)
        {
            response = Execute<AnonymousIPResponse>(ipAddress, "GeoIP2-Anonymous-IP", false);
            return response != null;
        }

        /// <summary>
        ///     Returns an <see cref="AsnResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="AsnResponse" /></returns>
        public AsnResponse Asn(IPAddress ipAddress)
        {
            return Execute<AsnResponse>(ipAddress, "GeoLite2-ASN")!;
        }

        /// <summary>
        ///     Returns an <see cref="AsnResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="AsnResponse" /></returns>
        public AsnResponse Asn(string ipAddress)
        {
            return Execute<AsnResponse>(ipAddress, "GeoLite2-ASN")!;
        }

        /// <summary>
        ///     Tries to lookup an <see cref="AsnResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="AsnResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryAsn(IPAddress ipAddress, out AsnResponse? response)
        {
            response = Execute<AsnResponse>(ipAddress, "GeoLite2-ASN", false);
            return response != null;
        }

        /// <summary>
        ///     Tries to lookup an <see cref="AsnResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="AsnResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryAsn(string ipAddress, out AsnResponse? response)
        {
            response = Execute<AsnResponse>(ipAddress, "GeoLite2-ASN", false);
            return response != null;
        }

        /// <summary>
        ///     Returns an <see cref="ConnectionTypeResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="ConnectionTypeResponse" /></returns>
        public ConnectionTypeResponse ConnectionType(IPAddress ipAddress)
        {
            return Execute<ConnectionTypeResponse>(ipAddress, "GeoIP2-Connection-Type")!;
        }

        /// <summary>
        ///     Returns an <see cref="ConnectionTypeResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="ConnectionTypeResponse" /></returns>
        public ConnectionTypeResponse ConnectionType(string ipAddress)
        {
            return Execute<ConnectionTypeResponse>(ipAddress, "GeoIP2-Connection-Type")!;
        }

        /// <summary>
        ///     Tries to lookup a <see cref="ConnectionTypeResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="ConnectionTypeResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryConnectionType(IPAddress ipAddress, out ConnectionTypeResponse? response)
        {
            response = Execute<ConnectionTypeResponse>(ipAddress, "GeoIP2-Connection-Type", false);
            return response != null;
        }

        /// <summary>
        ///     Tries to lookup a <see cref="ConnectionTypeResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="ConnectionTypeResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryConnectionType(string ipAddress, out ConnectionTypeResponse? response)
        {
            response = Execute<ConnectionTypeResponse>(ipAddress, "GeoIP2-Connection-Type", false);
            return response != null;
        }

        /// <summary>
        ///     Returns an <see cref="DomainResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="DomainResponse" /></returns>
        public DomainResponse Domain(IPAddress ipAddress)
        {
            return Execute<DomainResponse>(ipAddress, "GeoIP2-Domain")!;
        }

        /// <summary>
        ///     Returns an <see cref="DomainResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="DomainResponse" /></returns>
        public DomainResponse Domain(string ipAddress)
        {
            return Execute<DomainResponse>(ipAddress, "GeoIP2-Domain")!;
        }

        /// <summary>
        ///     Tries to lookup a <see cref="DomainResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="DomainResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryDomain(IPAddress ipAddress, out DomainResponse? response)
        {
            response = Execute<DomainResponse>(ipAddress, "GeoIP2-Domain", false);
            return response != null;
        }

        /// <summary>
        ///     Tries to lookup a <see cref="DomainResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="DomainResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryDomain(string ipAddress, out DomainResponse? response)
        {
            response = Execute<DomainResponse>(ipAddress, "GeoIP2-Domain", false);
            return response != null;
        }

        /// <summary>
        ///     Returns an <see cref="EnterpriseResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="EnterpriseResponse" /></returns>
        public EnterpriseResponse Enterprise(IPAddress ipAddress)
        {
            return Execute<EnterpriseResponse>(ipAddress, "Enterprise")!;
        }

        /// <summary>
        ///     Returns an <see cref="EnterpriseResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="EnterpriseResponse" /></returns>
        public EnterpriseResponse Enterprise(string ipAddress)
        {
            return Execute<EnterpriseResponse>(ipAddress, "Enterprise")!;
        }

        /// <summary>
        ///     Tries to lookup a <see cref="EnterpriseResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="EnterpriseResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryEnterprise(IPAddress ipAddress, out EnterpriseResponse? response)
        {
            response = Execute<EnterpriseResponse>(ipAddress, "Enterprise", false);
            return response != null;
        }

        /// <summary>
        ///     Tries to lookup a <see cref="EnterpriseResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="EnterpriseResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryEnterprise(string ipAddress, out EnterpriseResponse? response)
        {
            response = Execute<EnterpriseResponse>(ipAddress, "Enterprise", false);
            return response != null;
        }

        /// <summary>
        ///     Returns an <see cref="IspResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="IspResponse" /></returns>
        public IspResponse Isp(IPAddress ipAddress)
        {
            return Execute<IspResponse>(ipAddress, "GeoIP2-ISP")!;
        }

        /// <summary>
        ///     Returns an <see cref="IspResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="IspResponse" /></returns>
        public IspResponse Isp(string ipAddress)
        {
            return Execute<IspResponse>(ipAddress, "GeoIP2-ISP")!;
        }

        /// <summary>
        ///     Tries to lookup an <see cref="IspResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="IspResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryIsp(IPAddress ipAddress, out IspResponse? response)
        {
            response = Execute<IspResponse>(ipAddress, "GeoIP2-ISP", false);
            return response != null;
        }

        /// <summary>
        ///     Tries to lookup an <see cref="IspResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="IspResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryIsp(string ipAddress, out IspResponse? response)
        {
            response = Execute<IspResponse>(ipAddress, "GeoIP2-ISP", false);
            return response != null;
        }

        private T? Execute<T>(string ipStr, string type, bool throwOnNullResponse = true) where T : AbstractResponse
        {
            if (IPAddress.TryParse(ipStr, out var ip))
            {
                return Execute<T>(ipStr, ip, type, throwOnNullResponse);
            }

            throw new GeoIP2Exception($"The specified IP address was incorrectly formatted: {ipStr}");
        }

        private T? Execute<T>(IPAddress ipAddress, string type, bool throwOnNullResponse = true)
            where T : AbstractResponse
        {
            return Execute<T>(ipAddress.ToString(), ipAddress, type, throwOnNullResponse);
        }

        private T? Execute<T>(string ipStr, IPAddress ipAddress, string type, bool throwOnNullResponse = true)
            where T : AbstractResponse
        {
            if (!Metadata.DatabaseType.Contains(type))
            {
                var frame = new StackFrame(2, true);
                throw new InvalidOperationException(
                    $"A {Metadata.DatabaseType} database cannot be opened with the {frame.GetMethod()?.Name} method");
            }

            var injectables = new InjectableValues();
            injectables.AddValue("ip_address", ipStr);
            var response = _reader.Find<T>(ipAddress, injectables);

            if (response == null)
            {
                if (throwOnNullResponse)
                {
                    throw new AddressNotFoundException("The address " + ipStr + " is not in the database.");
                }
                return null;
            }

            response.SetLocales(_locales);

            return response;
        }
    }
}
