using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using MaxMind.Db;
using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Responses;
using Newtonsoft.Json.Linq;

namespace MaxMind.GeoIP2
{
    /// <summary>
    /// Instances of this class provide a reader for the GeoIP2 database format
    /// </summary>
    public class DatabaseReader : IGeoIP2Provider, IDisposable
    {
        private readonly List<string> _locales;
        private readonly Reader _reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseReader"/> class.
        /// </summary>
        /// <param name="file">The MaxMind DB file.</param>
        /// <param name="mode">The mode by which to access the DB file.</param>
        public DatabaseReader(string file, FileAccessMode mode = FileAccessMode.MemoryMapped)
            : this(file, new List<string> { "en" }, mode)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseReader"/> class.
        /// </summary>
        /// <param name="file">The MaxMind DB file.</param>
        /// <param name="locales">List of locale codes to use in name property from most preferred to least preferred.</param>
        /// <param name="mode">The mode by which to access the DB file.</param>
        public DatabaseReader(string file, List<string> locales, FileAccessMode mode = FileAccessMode.MemoryMapped)
        {
            _locales = locales;
            _reader = new Reader(file, mode);
        }

        private T Execute<T>(string ipAddress) where T : AbstractCountryResponse
        {
            IPAddress ip;
            if (ipAddress != null && !IPAddress.TryParse(ipAddress, out ip))
                throw new GeoIP2Exception(string.Format("The specified IP address was incorrectly formatted: {0}", ipAddress));

            var token = _reader.Find(ipAddress);

            if (token == null)
                throw new AddressNotFoundException("The address " + ipAddress + " is not in the database.");

            if (token["traits"] == null)
            {
                ((JObject)token).Add("traits", new JObject());
            }

            var traits = (JObject)token["traits"];
            traits.Add("ip_address", ipAddress);

            var response = token.ToObject<T>();
            response.SetLocales(_locales);

            return response;
        }

        /// <summary>
        /// Returns an <see cref="OmniResponse"/> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="OmniResponse"/></returns>
        public OmniResponse Omni(string ipAddress)
        {
            return Execute<OmniResponse>(ipAddress);
        }

        /// <summary>
        /// Returns an <see cref="CountryResponse"/> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="CountryResponse"/></returns>
        public CountryResponse Country(string ipAddress)
        {
            return Execute<CountryResponse>(ipAddress);
        }

        /// <summary>
        /// Returns an <see cref="CityResponse"/> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="CityResponse"/></returns>
        public CityResponse City(string ipAddress)
        {
            return Execute<CityResponse>(ipAddress);
        }

        /// <summary>
        /// Returns an <see cref="CityIspOrgResponse"/> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="CityIspOrgResponse"/></returns>
        public CityIspOrgResponse CityIspOrg(string ipAddress)
        {
            return Execute<CityIspOrgResponse>(ipAddress);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_reader != null)
                _reader.Dispose();
        }
    }
}
