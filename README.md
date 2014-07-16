# GeoIP2 .NET API #

## Beta Note ##

This is a beta release. The API may change before the first production
release, which will be numbered 2.0.0.

You may find information on the GeoIP2 beta release process on [our
website](http://www.maxmind.com/en/geoip2_beta).

## Description ##

This distribution provides an API for the GeoIP2 [web services]
(http://dev.maxmind.com/geoip/geoip2/web-services) and [databases]
(http://dev.maxmind.com/geoip/geoip2/downloadable). The API also works with
the free [GeoLite2 databases](http://dev.maxmind.com/geoip/geoip2/geolite2/).

## Requirements ##

This library works with .NET Framework version 4.0 and above. If you are
using Mono, 3.2 or greater is required.

This library depends on:

* [MaxMind DB Reader](https://github.com/maxmind/MaxMind-DB-Reader-dotnet)
* [RestSharp](http://restsharp.org/).

## Installation ##

### NuGet ###

We recommend installing this library with NuGet. To do this, type the
following into the Visual Studio Package Manager Console:

```
install-package MaxMind.GeoIP2
```

## Web Service Usage ##

To use the web service API, you must create a new
`MaxMind.GeoIP2.WebServiceClient` object with your `userID` and `licenseKey`.
You may also specify the fall-back locales, the host, timeout, or the request
timeout. You may then call the method corresponding to a specific end point,
passing it the IP address you want to look up. the IP address you want to look
up.

If the request succeeds, the method call will return a response class for the
endpoint you called. This response in turn contains multiple model classes,
each of which represents part of the data returned by the web service.

See the API documentation for more details.


## Web Service Example ##

```csharp
// This creates a WebServiceClient object that can be reused across requests.
// Replace "42" with your user ID and "license_key" with your license
// key.
var client = new WebServiceClient(42, "license_key");

// Replace "City" with the method corresponding to the web service that
// you are using, e.g., "Country", "Insights".
var city = client.City("128.101.101.101");

Console.WriteLine(city.Country.IsoCode); // 'US'
Console.WriteLine(city.Country.Name); // 'United States'
Console.WriteLine(city.Country.Names["zh-CN"]); // '美国'

Console.WriteLine(city.MostSpecificSubdivision.Name); // 'Minnesota'
Console.WriteLine(city.MostSpecificSubdivision.IsoCode); // 'MN'

Console.WriteLine(city.City.Name); // 'Minneapolis'

Console.WriteLine(city.Postal.Code); // '55455'

Console.WriteLine(city.Location.Latitude); // 44.9733
Console.WriteLine(city.Location.Longitude); // -93.2323

```

## Database Usage ##

To use the database API, you must create a new `DatabaseReader` with a string
representation of the path to your GeoIP2 database. You may also specify the
file access mode. You may then call the appropriate method (e.g., `city`) for
your database, passing it the IP address you want to look up.

If the lookup succeeds, the method call will return a response class for the
GeoIP2 lookup. This class in turn contains multiple model classes, each of
which represents part of the data returned by the database.

We recommend reusing the `DatabaseReader` object rather than creating a new
one for each lookup. The creation of this object is relatively expensive as it
must read in metadata for the file.

See the API documentation for more details.

## Database Examples ##

### City Database ###

```csharp
// This creates the DatabaseReader object, which should be reused across
// lookups.
var reader = new DatabaseReader("GeoIP2-City.mmdb");

// Replace "City" with the appropriate method for your database, e.g.,
// "Country".
var city = reader.City("128.101.101.101");

Console.WriteLine(city.Country.IsoCode); // 'US'
Console.WriteLine(city.Country.Name); // 'United States'
Console.WriteLine(city.Country.Names["zh-CN"]); // '美国'

Console.WriteLine(city.MostSpecificSubdivision.Name); // 'Minnesota'
Console.WriteLine(city.MostSpecificSubdivision.IsoCode); // 'MN'

Console.WriteLine(city.City.Name); // 'Minneapolis'

Console.WriteLine(city.Postal.Code); // '55455'

Console.WriteLine(city.Location.Latitude); // 44.9733
Console.WriteLine(city.Location.Longitude); // -93.2323

reader.Dispose();
```

### Connection-Type Database ###

```csharp

using (var reader = new DatabaseReader("GeoIP2-Connection-Type.mmdb"))
{
    var response = reader.ConnectionType("128.101.101.101");
    Console.WriteLine(response.ConnectionType); // 'Corporate'
    Console.WriteLine(response.IPAddress); // '128.101.101.101'
}
```

### Domain Database ###

```csharp

using (var reader = new DatabaseReader("GeoIP2-Domain.mmdb"))
{
    var response = reader.Domain("128.101.101.101");
    Console.WriteLine(response.Domain); // 'umn.edu'
    Console.WriteLine(response.IPAddress); // '128.101.101.101'
}
```

### ISP Database ###

```csharp

using (var reader = new DatabaseReader("GeoIP2-ISP.mmdb"))
{
    var response = reader.Isp("128.101.101.101");
    Console.WriteLine(response.AutonomousSystemNumber); // 217
    Console.WriteLine(response.AutonomousSystemOrganization); // 'University of Minnesota'
    Console.WriteLine(response.Isp); // 'University of Minnesota'
    Console.WriteLine(response.Organization); // 'University of Minnesota'
    Console.WriteLine(response.IPAddress); // '128.101.101.101'
}
```

## Exceptions ##

### Database ###

If the database is corrupt or otherwise invalid, a
`MaxMind.Db.InvalidDatabaseException` will be thrown.

If an address is not available in the database, a
`AddressNotFoundException` will be thrown.

### Web Service ###

For details on the possible errors returned by the web service itself, [see
the GeoIP2 web service documentation](http://dev.maxmind.com/geoip2/geoip/web-services).

If the web service returns an explicit error document, this is thrown as a
`AddressNotFoundException`, a `AuthenticationException`, a
`InvalidRequestException`, or a `OutOfQueriesException`.

If some sort of transport error occurs, an `HttpException` is thrown. This is
thrown when some sort of unanticipated error occurs, such as the web service
returning a 500 or an invalid error document. If the web service request
returns any status code besides 200, 4xx, or 5xx, this also becomes a
`HttpException`.

Finally, if the web service returns a 200 but the body is invalid, the client
throws a `GeoIP2Exception`. This exception also is the parent exception to the
above exceptions.


## Multi-Threaded Use ##

This API fully supports use in multi-threaded applications. When using the
`DatabaseReader` in a multi-threaded application, we suggest creating one
object and sharing that among threads.

## What data is returned? ##

While many of the end points return the same basic records, the attributes
which can be populated vary between end points. In addition, while an end
point may offer a particular piece of data, MaxMind does not always have every
piece of data for any given IP address.

Because of these factors, it is possible for any end point to return a record
where some or all of the attributes are unpopulated.

See the [GeoIP2 Precision web service
docs](http://dev.maxmind.com/geoip/geoip2/web-services) for details on what
data each end point may return.

The only piece of data which is always returned is the `ipAddress` attribute
in the `MaxMind.GeoIP2.Traits` record.

Every record class attribute has a corresponding predicate method so you can
check to see if the attribute is set.

## Integration with GeoNames ##

[GeoNames](http://www.geonames.org/) offers web services and downloadable
databases with data on geographical features around the world, including
populated places. They offer both free and paid premium data. Each feature is
unique identified by a `geonameId`, which is an integer.

Many of the records returned by the GeoIP2 web services and databases include
a `geonameId` property. This is the ID of a geographical feature (city,
region, country, etc.) in the GeoNames database.

Some of the data that MaxMind provides is also sourced from GeoNames. We
source things like place names, ISO codes, and other similar data from the
GeoNames premium data set.

## Reporting data problems ##

If the problem you find is that an IP address is incorrectly mapped,
please
[submit your correction to MaxMind](http://www.maxmind.com/en/correction).

If you find some other sort of mistake, like an incorrect spelling, please
check the [GeoNames site](http://www.geonames.org/) first. Once you've
searched for a place and found it on the GeoNames map view, there are a number
of links you can use to correct data ("move", "edit", "alternate names",
etc.). Once the correction is part of the GeoNames data set, it will be
automatically incorporated into future MaxMind releases.

If you are a paying MaxMind customer and you're not sure where to submit a
correction, please [contact MaxMind support
](http://www.maxmind.com/en/support) for help.

## Other Support ##

Please report all issues with this code using the
[GitHub issue tracker](https://github.com/maxmind/GeoIP2-dotnet/issues).

If you are having an issue with a MaxMind service that is not specific to the
client API, please see [our support page](http://www.maxmind.com/en/support).

## Contributing ##

Patches and pull requests are encouraged. Please include unit tests whenever
possible.

## Versioning ##

The API uses [Semantic Versioning](http://semver.org/).

## Copyright and License ##

This software is Copyright (c) 2013 by MaxMind, Inc.

This is free software, licensed under the Apache License, Version 2.0.
