#region

using MaxMind.Db;
using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
            : this(file, ["en"], mode)
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
            _locales = [.. locales];
            _reader = new Reader(file, mode);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseReader" /> class.
        /// </summary>
        /// <param name="stream">A stream of the MaxMind DB file.</param>
        public DatabaseReader(Stream stream)
            : this(stream, ["en"])
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseReader" /> class.
        /// </summary>
        /// <param name="stream">A stream of the MaxMind DB file.</param>
        /// <param name="locales">List of locale codes to use in name property from most preferred to least preferred.</param>
        public DatabaseReader(Stream stream, IEnumerable<string> locales)
        {
            _locales = [.. locales];
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
        public bool TryCountry(IPAddress ipAddress, [MaybeNullWhen(false)] out CountryResponse response)
        {
            return TryExecute(ipAddress, "Country", out response);
        }

        /// <summary>
        ///     Tries to lookup a <see cref="CountryResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="CountryResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryCountry(string ipAddress, [MaybeNullWhen(false)] out CountryResponse response)
        {
            return TryExecute(ipAddress, "Country", out response);
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
        public bool TryCity(IPAddress ipAddress, [MaybeNullWhen(false)] out CityResponse response)
        {
            return TryExecute(ipAddress, "City", out response);
        }

        /// <summary>
        ///     Tries to lookup a <see cref="CityResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="CityResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryCity(string ipAddress, [MaybeNullWhen(false)] out CityResponse response)
        {
            return TryExecute(ipAddress, "City", out response);
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
        public bool TryAnonymousIP(IPAddress ipAddress, [MaybeNullWhen(false)] out AnonymousIPResponse response)
        {
            return TryExecute(ipAddress, "GeoIP2-Anonymous-IP", out response);
        }

        /// <summary>
        ///     Tries to lookup an <see cref="AnonymousIPResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="AnonymousIPResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryAnonymousIP(string ipAddress, [MaybeNullWhen(false)] out AnonymousIPResponse response)
        {
            return TryExecute(ipAddress, "GeoIP2-Anonymous-IP", out response);
        }

        /// <summary>
        ///     Look up an IP address in a GeoIP Anonymous Plus.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="AnonymousPlusResponse" /></returns>
        public AnonymousPlusResponse AnonymousPlus(IPAddress ipAddress)
        {
            return Execute<AnonymousPlusResponse>(ipAddress, "GeoIP-Anonymous-Plus")!;
        }

        /// <summary>
        ///     Look up an IP address in a GeoIP Anonymous Plus.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="AnonymousPlusResponse" /></returns>
        public AnonymousPlusResponse AnonymousPlus(string ipAddress)
        {
            return Execute<AnonymousPlusResponse>(ipAddress, "GeoIP-Anonymous-Plus")!;
        }

        /// <summary>
        ///     Tries to lookup an <see cref="AnonymousPlusResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="AnonymousPlusResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryAnonymousPlus(IPAddress ipAddress, [MaybeNullWhen(false)] out AnonymousPlusResponse response)
        {
            return TryExecute(ipAddress, "GeoIP-Anonymous-Plus", out response);
        }

        /// <summary>
        ///     Tries to lookup an <see cref="AnonymousPlusResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="AnonymousPlusResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryAnonymousPlus(string ipAddress, [MaybeNullWhen(false)] out AnonymousPlusResponse response)
        {
            return TryExecute(ipAddress, "GeoIP-Anonymous-Plus", out response);
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
        public bool TryAsn(IPAddress ipAddress, [MaybeNullWhen(false)] out AsnResponse response)
        {
            return TryExecute(ipAddress, "GeoLite2-ASN", out response);
        }

        /// <summary>
        ///     Tries to lookup an <see cref="AsnResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="AsnResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryAsn(string ipAddress, [MaybeNullWhen(false)] out AsnResponse response)
        {
            return TryExecute(ipAddress, "GeoLite2-ASN", out response);
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
        public bool TryConnectionType(IPAddress ipAddress, [MaybeNullWhen(false)] out ConnectionTypeResponse response)
        {
            return TryExecute(ipAddress, "GeoIP2-Connection-Type", out response);
        }

        /// <summary>
        ///     Tries to lookup a <see cref="ConnectionTypeResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="ConnectionTypeResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryConnectionType(string ipAddress, [MaybeNullWhen(false)] out ConnectionTypeResponse response)
        {
            return TryExecute(ipAddress, "GeoIP2-Connection-Type", out response);
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
        public bool TryDomain(IPAddress ipAddress, [MaybeNullWhen(false)] out DomainResponse response)
        {
            return TryExecute(ipAddress, "GeoIP2-Domain", out response);
        }

        /// <summary>
        ///     Tries to lookup a <see cref="DomainResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="DomainResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryDomain(string ipAddress, [MaybeNullWhen(false)] out DomainResponse response)
        {
            return TryExecute(ipAddress, "GeoIP2-Domain", out response);
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
        public bool TryEnterprise(IPAddress ipAddress, [MaybeNullWhen(false)] out EnterpriseResponse response)
        {
            return TryExecute(ipAddress, "Enterprise", out response);
        }

        /// <summary>
        ///     Tries to lookup a <see cref="EnterpriseResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="EnterpriseResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryEnterprise(string ipAddress, [MaybeNullWhen(false)] out EnterpriseResponse response)
        {
            return TryExecute(ipAddress, "Enterprise", out response);
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
        public bool TryIsp(IPAddress ipAddress, [MaybeNullWhen(false)] out IspResponse response)
        {
            return TryExecute(ipAddress, "GeoIP2-ISP", out response);
        }

        /// <summary>
        ///     Tries to lookup an <see cref="IspResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="IspResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        public bool TryIsp(string ipAddress, [MaybeNullWhen(false)] out IspResponse response)
        {
            return TryExecute(ipAddress, "GeoIP2-ISP", out response);
        }

        private T Execute<T>(string ipStr, string type) where T : AbstractResponse
        {
            if (IPAddress.TryParse(ipStr, out var ip))
            {
                var response = Execute<T>(ipStr, ip, type);
                return response ?? throw new AddressNotFoundException("The address " + ipStr + " is not in the database.");
            }

            throw new GeoIP2Exception($"The specified IP address was incorrectly formatted: {ipStr}");
        }

        private T Execute<T>(IPAddress ipAddress, string type)
            where T : AbstractResponse
        {
            var ipStr = ipAddress.ToString();
            var response = Execute<T>(ipStr, ipAddress, type);
            return response ?? throw new AddressNotFoundException("The address " + ipStr + " is not in the database.");
        }

        private bool TryExecute<T>(string ipStr, string type, [MaybeNullWhen(false)] out T response)
            where T : AbstractResponse
        {
            if (IPAddress.TryParse(ipStr, out var ip))
            {
                response = Execute<T>(ipStr, ip, type);
                return response != null;
            }

            throw new GeoIP2Exception($"The specified IP address was incorrectly formatted: {ipStr}");
        }

        private bool TryExecute<T>(IPAddress ipAddress, string type, [MaybeNullWhen(false)] out T response)
            where T : AbstractResponse
        {
            response = Execute<T>(ipAddress.ToString(), ipAddress, type);
            return response != null;
        }

        private T? Execute<T>(string ipStr, IPAddress ipAddress, string type)
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
                return null;
            }

            response.SetLocales(_locales);

            return response;
        }
    }
}