GeoIP2 .NET API Release Notes
=============================

4.0.0
------------------

* This library now requires .NET Framework 4.6.1 or greater or .NET Standard
  2.0 or greater.
* .NET 5.0 was added as a target framework.
* `System.Text.Json` is now used for deserialization of web service requests.
  `Newtonsoft.Json` is no longer supported for serialization or
  deserialization.
* The `Names` property on `NamedEntity` modules is now an
  `IReadOnlyDictionary<string, string>`.
* The `Subdivisions` property on `CityResponse` and `InsightsResponse` is now
  an `IReadOnlyList<Subdivision>`.
* The `httpMessageHandler` argument is now correctly initialized by the
  `WebServiceClient` constructor.

3.3.0 (2020-09-25)
------------------

* The `IsResidentialProxy` property has been added to
  `MaxMind.GeoIP2.Responses.AnonymousIPResponse` and
  `MaxMind.GeoIP2.Model.Traits`.

3.2.0 (2020-04-28)
------------------

* You may now create `WebServiceClient` as Typed Client with
  `IHttpClientFactory` in .NET Core 2.1+. Pull Request by Bojan Nikolić.
  GitHub #115 & #117.
* The `WebServiceClient` constructor now supports an optional
  `httpMessageHandler` parameter. This is used in creating the `HttpClient`
  for asynchronous requests.

3.1.0 (2019-12-06)
------------------

* This library has been updated to support the nullable reference types
  introduced in C# 8.0.
* A `Network` property has been added to the various response models. This
  represents the largest network where all the fields besides the IP
  address are the same.
* The `StaticIPScore` property has been added to `MaxMind.GeoIP2.Model.Traits`.
  This output is available from GeoIP2 Precision Insights. It is an indicator
  of how static or dynamic an IP address is.
* The `UserCount` property has been added to `MaxMind.GeoIP2.Model.Traits`.
  This output is available from GeoIP2 Precision Insights. It is an
  estimate of the number of users sharing the IP/network over the past
  24 hours.
* Updated documentation of anonymizer properties - `IsAnonymousVpn` and
  `IsHostingProvider` - to be more descriptive.
* `netstandard2.1` was added as a target framework.

3.0.0 (2018-04-11)
------------------

* The `userId` constructor parameter for `WebServiceClient` was renamed to
  `accountId` and support was added for the error codes `ACCOUNT_ID_REQUIRED`
  and `ACCOUNT_ID_UNKNOWN`.
* The exception classes are no longer serializable when using the .NET
  Framework. This eliminates a difference between the .NET Framework
  assemblies and the .NET Standard ones.
* The `AutonomousSystemNumber` properties on `MaxMind.GeoIP2.Model.Traits`,
  `MaxMind.GeoIP2.Responses.AsnResponse`, and
  `MaxMind.GeoIP2.Responses.IspResponse` are now `long?` to match the underlying
  types in the databases.
* `MaxMind.Db` was upgraded to 2.4.0. This adds a new file mode enum value for
  the database reader, `FileAccessMode.MemoryMappedGlobal`. When used, this will
  open the file in global memory map mode. This requires the "create global
  objects" right.

2.10.0 (2018-01-19)
-------------------

* The `IsInEuropeanUnion` property was added to `MaxMind.GeoIP2.Model.Country`
  and `MaxMind.GeoIP2.Model.RepresentedCountry`. This property is `true` if the
  country is a member state of the European Union.

2.9.0 (2017-10-27)
------------------

* The following new anonymizer properties were added to
  `MaxMind.GeoIP2.Model.Traits` for use with GeoIP2 Precision Insights:
  `IsAnonymous`, `IsAnonymousVpn`, `IsHostingProvider`, `IsPublicProxy`,
  and `IsTorExitNode`.
* Deserialization of the registered country when reading a GeoLite2 Country or
  GeoIP2 Country database now works. Previously, it was deserialized to the
  wrong name. Reported by oliverherdener.
* A `netstandard2.0` target was added to eliminate additional dependencies
  required by the `netstandard1.4` target. Pull request by Adeel Mujahid.
  GitHub #81.
* As part of the above work, the separate Mono build files were dropped. As
  of Mono 5.0.0, `msbuild` is supported.

2.8.0 (2017-05-08)
------------------

* Add support for GeoLite2 ASN.
* Switch to the updated MSBuild .NET Core build system.
* Move tests from  NUnit to xUnit.net.
* Upgrade to `MaxMind.Db` 2.2.0.

2.7.2 (2016-11-22)
------------------

* Use framework assembly for `System.Net.Http` on .NET 4.5.
* Update for .NET Core 1.1.

2.7.1 (2016-08-08)
------------------

* Re-release of 2.7.0 to fix strong name issue. No code changes.

2.7.0 (2016-08-01)
------------------

* First non-beta release with .NET Core support.
* The tests now use the .NET Core NUnit runner. Pull request by Adeel Mujahid.
  GitHub #68.
* Updated documentation to clarify what the accuracy radius refers to.

2.7.0-beta2 (2016-06-02)
------------------------

* Added handling of additional error codes that the web service may return.
* Update for .NET Core RC2. Pull request by Adeel Mujahid. GitHub #64.

2.7.0-beta1 (2016-05-15)
------------------------

* .NET Core support. Switched to `dotnet/cli` for building. Pull request by
  Adeel Mujahid. GitHub #60.
* Updated documentation to reflect that the accuracy radius is now included
  in City.

2.6.0 (2016-04-15)
------------------

* Added support for the GeoIP2 Enterprise database.

2.6.0-beta3 (2016-02-10)
------------------------

* Try-based lookup methods were added to `DatabaseReader` as an alternative to
  the existing methods. These methods return a boolean indicating whether the
  record was found rather than throwing an exception when it is not found.
  Pull request by Mani Gandham. GitHub #31, #50.

2.6.0-beta2 (2016-01-18)
------------------------

* Parameterless endpoint methods were added to `WebServiceClient`. These
  return the record for the requesting IP address using the `me` endpoint as
  documented in the web services API documentation.
* The target framework is now .NET 4.5 rather than 4.5.2 in order to work
  better with Mono. GitHub #44.

2.6.0-beta1 (2016-01-18)
------------------------

* Upgrade MaxMindb.Db reader to 2.0.0-beta1. This includes significant
  performance increases.

2.5.0 (2015-12-04)
------------------

* IMPORTANT: The target framework is now 4.5.2. Microsoft is ending support
  for 4.0, 4.5, and 4.5.1 on January 12, 2016. Removing support for these
  frameworks allows us to remove the dependency on the BCL libraries and fixes
  several outstanding issues. Closes #38, #39, #40, and #42.
* The assembly version was bumped to 2.5.0.
* Classes subclassing `NamedEntity` now have a default locale of `en`. This
  allows the `Name` property to be used (for English names) when the object is
  deserialized from JSON. Closes #41.
* The `locale` parameter for the `DatabaseReader` and `WebServiceClient`
  constructors is now an `IEnumerable<string>` rather than a `List<string>`.
* The tests now use NUnit 3.

2.4.0 (2015-09-23)
------------------

* Updated MaxMind.Db to 1.2.0.

2.4.0-beta1 (2015-09-10)
------------------------

* Async support was added to the `WebServiceClient`. Each web-service end
  point now has a corresponding `*Async(ip)` method. GitHub #1.
* Use of RestSharp was replaced by `HttpWebRequest` for synchronous HTTP
  requests and `HttpClient` for asynchronous requests. Microsoft BCL libraries
  and `System.Net.Http` are used to provide `async`/`await` and `HttpClient`
  support on .NET 4.0. GitHub #33.
* The library now has a strong name.

2.3.1 (2015-07-21)
------------------

* Upgrade to MaxMind.Db 1.1.0.
* Fix serialization on exceptions.

2.3.1-beta1 (2015-06-30)
------------------------

* Upgrade to Json.NET 7.0.1.
* Upgrade to MaxMind.Db 1.1.0-beta1. This release includes a number of
  significant improvements for the memory-mapped file mode.

2.3.0 (2015-06-29)
------------------

* `AverageIncome` and `PopulationDensity` were added to the `Location`
  model for use with the new fields in GeoIP2 Insights.
* `IsAnonymousProxy` and `IsSatelliteProvider` in `MaxMind.GeoIP2.Model.Traits`
  have been deprecated. Please use our [GeoIP2 Anonymous IP
  database](https://www.maxmind.com/en/geoip2-anonymous-ip-database) to
  determine whether an IP address is used by an anonymizing service.

2.2.0 (2015-05-19)
------------------

* All of the database methods in `DatabaseReader` and all of the web service
  methods in `WebServiceClient` now have a counterpart that takes an
  `IPAddress` instead of a `string`. Pull request by Guillaume Turri. GitHub
  #24.
* The `JsonIgnore` attribute was added to `Names` in `NamedEntity` and
  `Subdivisions` in `AbstractCityResponse` as  these were already exposed to
  JSON.NET through the private field backing them. Pull request by Dan Byrne
  GitHub #21.
* The interfaces `IGeoIP2DatabaseReader` and `IGeoIP2WebServicesClient` were
  added to facilitate dependency injection and mocking. Pull request by Naz
  Soogund. GitHub #22.
* A `HasCoordinates` getter was added to the `Location` class. This will
  return true if both the `Latitude` and `Longitude` have values. Pull request
  by Darren Hickling. GitHub #23.
* All of the response and model properties now set the appropriate
  `JsonProperty` rather than relying an JSON.NET's automatic matching.
  Serializing these objects should now product JSON much more similar to the
  JSON returned by the web service and internal structure of the data in
  database files.
* Dependencies were updated to most recent versions.

2.1.0 (2014-11-06)
------------------

* Added support for the GeoIP2 Anonymous IP database. The `DatabaseReader`
  class now has an `AnonymousIP()` method which returns an
  `AnonymousIPResponse` object.

2.0.0 (2014-09-29)
------------------

* First production release.

0.5.0 (2014-09-24)
------------------

* The deprecated `CityIspOrg` and `Omni` methods were removed.
* `DatabaseReader` methods will now throw an `InvalidOperationException` when
  called for the wrong database type.
* `DatabaseReader` now has a `Metadata` property that provides an object
   containing the metadata for the open database.

0.4.0 (2014-07-22)
------------------

* The web service client API has been updated for the v2.1 release of the web
  service. In particular, the `CityIspOrg` and `Omni` methods on
  `WebServiceClient` have been deprecated. The `City` method now provides all
  of the data formerly provided by `CityIspOrg`, and the `Omni` method has
  been replaced by the `Insights` method.
* Support was added for the GeoIP2 Connection Type, Domain, and ISP databases.


0.3.3 (2014-06-02)
------------------

* Constructors with named parameters were added to the model and response
  classes. (Jon Wynveen)

0.3.2 (2014-04-09)
------------------

* A constructor taking a `Stream` was added to `DatabaseReader`.
* Fixed dependency on wrong version of `Newtonsoft.Json`.

0.3.1 (2014-02-13)
------------------

* Fixed broken error handling. Previously the wrong exceptions and error
  messages were returned for some web service errors.

0.3.0 (2013-11-15)
------------------

* API CHANGE: Renamed exceptions to remove GeoIP2 prefixes.
* Improved error messages when RestSharp does not return an HTTP status code.

0.2.0 (2013-10-25)
------------------

* First release with GeoIP2 database support. See `DatabaseReader` class.

0.1.1 (2013-10-04)
------------------

* Build documentation.

0.1.0 (2013-09-20)
------------------

* Initial release.
