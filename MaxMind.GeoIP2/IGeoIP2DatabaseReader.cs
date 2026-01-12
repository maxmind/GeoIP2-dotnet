#region

using System.Diagnostics.CodeAnalysis;
using MaxMind.Db;
using MaxMind.GeoIP2.Http;
using MaxMind.GeoIP2.Responses;
using System.Net;

#endregion

namespace MaxMind.GeoIP2
{
    /// <summary>
    ///     Interface for database reader
    /// </summary>
    public interface IGeoIP2DatabaseReader : IGeoIP2Provider
    {
        /// <summary>
        ///     The metadata for the open MaxMind DB file.
        /// </summary>
        Metadata Metadata { get; }

        /// <summary>
        ///     Look up an IP address in a GeoIP2 Anonymous IP.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="AnonymousIPResponse" /></returns>
        AnonymousIPResponse AnonymousIP(IPAddress ipAddress);

        /// <summary>
        ///     Look up an IP address in a GeoIP2 Anonymous IP.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="AnonymousIPResponse" /></returns>
        AnonymousIPResponse AnonymousIP(string ipAddress);

        /// <summary>
        ///     Tries to lookup an <see cref="AnonymousIPResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="AnonymousIPResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryAnonymousIP(IPAddress ipAddress, [MaybeNullWhen(false)] out AnonymousIPResponse response);

        /// <summary>
        ///     Tries to lookup an <see cref="AnonymousIPResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="AnonymousIPResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryAnonymousIP(string ipAddress, [MaybeNullWhen(false)] out AnonymousIPResponse response);

        /// <summary>
        ///     Look up an IP address in a GeoIP Anonymous Plus.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="AnonymousPlusResponse" /></returns>
        AnonymousPlusResponse AnonymousPlus(IPAddress ipAddress);

        /// <summary>
        ///     Look up an IP address in a GeoIP Anonymous Plus.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="AnonymousPlusResponse" /></returns>
        AnonymousPlusResponse AnonymousPlus(string ipAddress);

        /// <summary>
        ///     Tries to lookup an <see cref="AnonymousPlusResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="AnonymousPlusResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryAnonymousPlus(IPAddress ipAddress, [MaybeNullWhen(false)] out AnonymousPlusResponse response);

        /// <summary>
        ///     Tries to lookup an <see cref="AnonymousPlusResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="AnonymousPlusResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryAnonymousPlus(string ipAddress, [MaybeNullWhen(false)] out AnonymousPlusResponse response);

        /// <summary>
        ///     Returns an <see cref="AsnResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="Response" /></returns>
        AsnResponse Asn(IPAddress ipAddress);

        /// <summary>
        ///     Returns an <see cref="AsnResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="AsnResponse" /></returns>
        AsnResponse Asn(string ipAddress);

        /// <summary>
        ///     Tries to lookup an <see cref="AsnResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="AsnResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryAsn(IPAddress ipAddress, [MaybeNullWhen(false)] out AsnResponse response);

        /// <summary>
        ///     Tries to lookup an <see cref="AsnResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="AsnResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryAsn(string ipAddress, [MaybeNullWhen(false)] out AsnResponse response);

        /// <summary>
        ///     Tries to lookup a <see cref="CityResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="CityResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryCity(IPAddress ipAddress, [MaybeNullWhen(false)] out CityResponse response);

        /// <summary>
        ///     Tries to lookup a <see cref="CityResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="CityResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryCity(string ipAddress, [MaybeNullWhen(false)] out CityResponse response);

        /// <summary>
        ///     Tries to lookup a <see cref="CountryResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="CountryResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryCountry(IPAddress ipAddress, [MaybeNullWhen(false)] out CountryResponse response);

        /// <summary>
        ///     Tries to lookup a <see cref="CountryResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="CountryResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryCountry(string ipAddress, [MaybeNullWhen(false)] out CountryResponse response);

        /// <summary>
        ///     Returns an <see cref="ConnectionTypeResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="ConnectionTypeResponse" /></returns>
        ConnectionTypeResponse ConnectionType(IPAddress ipAddress);

        /// <summary>
        ///     Returns an <see cref="ConnectionTypeResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="ConnectionTypeResponse" /></returns>
        ConnectionTypeResponse ConnectionType(string ipAddress);

        /// <summary>
        ///     Tries to lookup a <see cref="ConnectionTypeResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="ConnectionTypeResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryConnectionType(IPAddress ipAddress, [MaybeNullWhen(false)] out ConnectionTypeResponse response);

        /// <summary>
        ///     Tries to lookup a <see cref="ConnectionTypeResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="ConnectionTypeResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryConnectionType(string ipAddress, [MaybeNullWhen(false)] out ConnectionTypeResponse response);

        /// <summary>
        ///     Returns an <see cref="DomainResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="DomainResponse" /></returns>
        DomainResponse Domain(IPAddress ipAddress);

        /// <summary>
        ///     Returns an <see cref="DomainResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="DomainResponse" /></returns>
        DomainResponse Domain(string ipAddress);

        /// <summary>
        ///     Tries to lookup a <see cref="DomainResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="DomainResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryDomain(IPAddress ipAddress, [MaybeNullWhen(false)] out DomainResponse response);

        /// <summary>
        ///     Tries to lookup a <see cref="DomainResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="DomainResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryDomain(string ipAddress, [MaybeNullWhen(false)] out DomainResponse response);

        /// <summary>
        ///     Returns an <see cref="EnterpriseResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="EnterpriseResponse" /></returns>
        EnterpriseResponse Enterprise(IPAddress ipAddress);

        /// <summary>
        ///     Returns an <see cref="EnterpriseResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="EnterpriseResponse" /></returns>
        EnterpriseResponse Enterprise(string ipAddress);

        /// <summary>
        ///     Tries to lookup a <see cref="EnterpriseResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="EnterpriseResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryEnterprise(IPAddress ipAddress, [MaybeNullWhen(false)] out EnterpriseResponse response);

        /// <summary>
        ///     Tries to lookup a <see cref="EnterpriseResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="EnterpriseResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryEnterprise(string ipAddress, [MaybeNullWhen(false)] out EnterpriseResponse response);

        /// <summary>
        ///     Returns an <see cref="IspResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="IspResponse" /></returns>
        IspResponse Isp(IPAddress ipAddress);

        /// <summary>
        ///     Returns an <see cref="IspResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="IspResponse" /></returns>
        IspResponse Isp(string ipAddress);

        /// <summary>
        ///     Tries to lookup an <see cref="IspResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="IspResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryIsp(IPAddress ipAddress, [MaybeNullWhen(false)] out IspResponse response);

        /// <summary>
        ///     Tries to lookup an <see cref="IspResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <param name="response">The <see cref="IspResponse" />.</param>
        /// <returns>A <see cref="bool" /> describing whether the IP address was found.</returns>
        bool TryIsp(string ipAddress, [MaybeNullWhen(false)] out IspResponse response);
    }
}