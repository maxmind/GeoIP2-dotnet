﻿#region

using MaxMind.Db;
using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Responses;
using Newtonsoft.Json.Linq;
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
        private readonly List<string> _locales;
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
        public DatabaseReader(string file, List<string> locales, FileAccessMode mode = FileAccessMode.MemoryMapped)
        {
            _locales = locales;
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
        public DatabaseReader(Stream stream, List<string> locales)
        {
            _locales = locales;
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
            return Execute<CountryResponse>(ipAddress, true, "Country");
        }

        /// <summary>
        ///     Returns an <see cref="CountryResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CountryResponse" /></returns>
        public CountryResponse Country(string ipAddress)
        {
            return Execute<CountryResponse>(ipAddress, true, "Country");
        }

        /// <summary>
        ///     Returns an <see cref="CityResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CityResponse" /></returns>
        public CityResponse City(IPAddress ipAddress)
        {
            return Execute<CityResponse>(ipAddress, true, "City");
        }

        /// <summary>
        ///     Returns an <see cref="CityResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CityResponse" /></returns>
        public CityResponse City(string ipAddress)
        {
            return Execute<CityResponse>(ipAddress, true, "City");
        }

        /// <summary>
        ///     Look up an IP address in a GeoIP2 Anonymous IP.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="AnonymousIPResponse" /></returns>
        public AnonymousIPResponse AnonymousIP(string ipAddress)
        {
            return Execute<AnonymousIPResponse>(ipAddress, false, "GeoIP2-Anonymous-IP");
        }

        /// <summary>
        ///     Returns an <see cref="ConnectionTypeResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="ConnectionTypeResponse" /></returns>
        public ConnectionTypeResponse ConnectionType(string ipAddress)
        {
            return Execute<ConnectionTypeResponse>(ipAddress, false, "GeoIP2-Connection-Type");
        }

        /// <summary>
        ///     Returns an <see cref="DomainResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="DomainResponse" /></returns>
        public DomainResponse Domain(string ipAddress)
        {
            return Execute<DomainResponse>(ipAddress, false, "GeoIP2-Domain");
        }

        /// <summary>
        ///     Returns an <see cref="IspResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="IspResponse" /></returns>
        public IspResponse Isp(string ipAddress)
        {
            return Execute<IspResponse>(ipAddress, false, "GeoIP2-ISP");
        }

        private T Execute<T>(string ipStr, bool hasTraits, string type) where T : AbstractResponse
        {
            IPAddress ip = null;
            if (ipStr != null && !IPAddress.TryParse(ipStr, out ip))
                throw new GeoIP2Exception($"The specified IP address was incorrectly formatted: {ipStr}");
            return Execute<T>(ipStr, ip, hasTraits, type);
        }

        private T Execute<T>(IPAddress ipAddress, bool hasTraits, string type) where T : AbstractResponse
        {
            return Execute<T>(ipAddress.ToString(), ipAddress, hasTraits, type);
        }

        private T Execute<T>(string ipStr, IPAddress ipAddress, bool hasTraits, string type) where T : AbstractResponse
        {
            if (!Metadata.DatabaseType.Contains(type))
            {
                var frame = new StackFrame(2, true);
                throw new InvalidOperationException(
                    $"A {Metadata.DatabaseType} database cannot be opened with the {frame.GetMethod().Name} method");
            }

            var token = (JObject)_reader.Find(ipAddress);

            if (token == null)
                throw new AddressNotFoundException("The address " + ipStr + " is not in the database.");

            JObject ipObject;
            if (hasTraits)
            {
                if (token["traits"] == null)
                {
                    token.Add("traits", new JObject());
                }

                ipObject = (JObject)token["traits"];
            }
            else
            {
                ipObject = token;
            }
            ipObject.Add("ip_address", ipStr);

            var response = token.ToObject<T>();
            response.SetLocales(_locales);

            return response;
        }

        /// <summary>
        ///     Look up an IP address in a GeoIP2 Anonymous IP.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="AnonymousIPResponse" /></returns>
        public AnonymousIPResponse AnonymousIP(IPAddress ipAddress)
        {
            return Execute<AnonymousIPResponse>(ipAddress, false, "GeoIP2-Anonymous-IP");
        }

        /// <summary>
        ///     Returns an <see cref="ConnectionTypeResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="ConnectionTypeResponse" /></returns>
        public ConnectionTypeResponse ConnectionType(IPAddress ipAddress)
        {
            return Execute<ConnectionTypeResponse>(ipAddress, false, "GeoIP2-Connection-Type");
        }

        /// <summary>
        ///     Returns an <see cref="DomainResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="DomainResponse" /></returns>
        public DomainResponse Domain(IPAddress ipAddress)
        {
            return Execute<DomainResponse>(ipAddress, false, "GeoIP2-Domain");
        }

        /// <summary>
        ///     Returns an <see cref="IspResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="IspResponse" /></returns>
        public IspResponse Isp(IPAddress ipAddress)
        {
            return Execute<IspResponse>(ipAddress, false, "GeoIP2-ISP");
        }
    }
}