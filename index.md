---
layout: default
title: MaxMind GeoIP2 .NET API
language: dotnet
version: v2.7.0
---

# GeoIP2 .NET API #

## Description ##

This distribution provides an API for the GeoIP2
[web services](http://dev.maxmind.com/geoip/geoip2/web-services) and
[databases](http://dev.maxmind.com/geoip/geoip2/downloadable). The API also
works with the free [GeoLite2 databases](http://dev.maxmind.com/geoip/geoip2/geolite2/).

## Requirements ##

This library works with .NET Framework version 4.5 and above. If you are
using Mono, 3.2 or greater is required.

This library depends on
[MaxMind DB Reader](https://github.com/maxmind/MaxMind-DB-Reader-dotnet).

## Installation ##

### NuGet ###

We recommend installing this library with NuGet. To do this, type the
following into the Visual Studio Package Manager Console:

```
install-package MaxMind.GeoIP2
```

## IP Geolocation Usage ##

IP geolocation is inherently imprecise. Locations are often near the center of
the population. Any location provided by a GeoIP2 database or web service
should not be used to identify a particular address or household.

## Web Service Usage ##

To use the web service API, you must create a new
`MaxMind.GeoIP2.WebServiceClient` object with your `userID` and `licenseKey`.
You may also specify the fall-back locales, the host, timeout, or the request
timeout. You may then call the sync or async method corresponding to the
specific end point, passing it the IP address you want to look up or no
parameters if you want to look up the current device.

If the request succeeds, the method call will return a response class for the
endpoint you called. This response in turn contains multiple model classes,
each of which represents part of the data returned by the web service.

See the API documentation for more details.

## Web Service Example ##

### Country Service (Sync) ###

```csharp
// This creates a WebServiceClient object that can be reused across requests.
// Replace "42" with your user ID and "license_key" with your license
// key.
using (var client = new WebServiceClient(42, "license_key"))
{
    // Do the lookup
    var response = client.Country("128.101.101.101");

    Console.WriteLine(response.Country.IsoCode);        // 'US'
    Console.WriteLine(response.Country.Name);           // 'United States'
    Console.WriteLine(response.Country.Names["zh-CN"]); // '美国'
}
```

### Country Service (Async) ###

```csharp
// This creates a WebServiceClient object that can be reused across requests.
// Replace "42" with your user ID and "license_key" with your license
// key.
using (var client = new WebServiceClient(42, "license_key"))
{
    // Do the lookup
    var response = await client.CountryAsync("128.101.101.101");

    Console.WriteLine(response.Country.IsoCode);        // 'US'
    Console.WriteLine(response.Country.Name);           // 'United States'
    Console.WriteLine(response.Country.Names["zh-CN"]); // '美国'
}
```

### City Service (Sync) ###

```csharp
// This creates a WebServiceClient object that can be reused across requests.
// Replace "42" with your user ID and "license_key" with your license
// key.
using (var client = new WebServiceClient(42, "license_key"))
{
    // Do the lookup
    var response = client.City("128.101.101.101");

    Console.WriteLine(response.Country.IsoCode);        // 'US'
    Console.WriteLine(response.Country.Name);           // 'United States'
    Console.WriteLine(response.Country.Names["zh-CN"]); // '美国'

    Console.WriteLine(response.MostSpecificSubdivision.Name);    // 'Minnesota'
    Console.WriteLine(response.MostSpecificSubdivision.IsoCode); // 'MN'

    Console.WriteLine(response.City.Name); // 'Minneapolis'

    Console.WriteLine(response.Postal.Code); // '55455'

    Console.WriteLine(response.Location.Latitude);  // 44.9733
    Console.WriteLine(response.Location.Longitude); // -93.2323
}
```

### City Service (Async) ###

```csharp
// This creates a WebServiceClient object that can be reused across requests.
// Replace "42" with your user ID and "license_key" with your license
// key.
using (var client = new WebServiceClient(42, "license_key"))
{
    // Do the lookup
    var response = await client.CityAsync("128.101.101.101");

    Console.WriteLine(response.Country.IsoCode);        // 'US'
    Console.WriteLine(response.Country.Name);           // 'United States'
    Console.WriteLine(response.Country.Names["zh-CN"]); // '美国'

    Console.WriteLine(response.MostSpecificSubdivision.Name);    // 'Minnesota'
    Console.WriteLine(response.MostSpecificSubdivision.IsoCode); // 'MN'

    Console.WriteLine(response.City.Name); // 'Minneapolis'

    Console.WriteLine(response.Postal.Code); // '55455'

    Console.WriteLine(response.Location.Latitude);  // 44.9733
    Console.WriteLine(response.Location.Longitude); // -93.2323
}
```

### Insights Service (Sync) ###

```csharp
// This creates a WebServiceClient object that can be reused across requests.
// Replace "42" with your user ID and "license_key" with your license
// key.
using (var client = new WebServiceClient(42, "license_key"))
{
    // Do the lookup
    var response = client.Insights("128.101.101.101");

    Console.WriteLine(response.Country.IsoCode);        // 'US'
    Console.WriteLine(response.Country.Name);           // 'United States'
    Console.WriteLine(response.Country.Names["zh-CN"]); // '美国'

    Console.WriteLine(response.MostSpecificSubdivision.Name);    // 'Minnesota'
    Console.WriteLine(response.MostSpecificSubdivision.IsoCode); // 'MN'

    Console.WriteLine(response.City.Name); // 'Minneapolis'

    Console.WriteLine(response.Postal.Code); // '55455'

    Console.WriteLine(response.Location.Latitude);  // 44.9733
    Console.WriteLine(response.Location.Longitude); // -93.2323
}
```

### Insights Service (Async) ###

```csharp
// This creates a WebServiceClient object that can be reused across requests.
// Replace "42" with your user ID and "license_key" with your license
// key.
using (var client = new WebServiceClient(42, "license_key"))
{
    // Do the lookup
    var response = await client.InsightsSync("128.101.101.101");

    Console.WriteLine(response.Country.IsoCode);        // 'US'
    Console.WriteLine(response.Country.Name);           // 'United States'
    Console.WriteLine(response.Country.Names["zh-CN"]); // '美国'

    Console.WriteLine(response.MostSpecificSubdivision.Name);    // 'Minnesota'
    Console.WriteLine(response.MostSpecificSubdivision.IsoCode); // 'MN'

    Console.WriteLine(response.City.Name); // 'Minneapolis'

    Console.WriteLine(response.Postal.Code); // '55455'

    Console.WriteLine(response.Location.Latitude);  // 44.9733
    Console.WriteLine(response.Location.Longitude); // -93.2323
}
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

### Anonymous IP Database ###

```csharp

using (var reader = new DatabaseReader("GeoIP2-Anonymous-IP.mmdb"))
{
    var response = reader.AnonymousIP("85.25.43.84");
    Console.WriteLine(response.IsAnonymous);       // true
    Console.WriteLine(response.IsAnonymousVpn);    // false
    Console.WriteLine(response.IsHostingProvider); // false
    Console.WriteLine(response.IsPublicProxy);      // false
    Console.WriteLine(response.IsTorExitNode);     // true
    Console.WriteLine(response.IPAddress);         // '85.25.43.84'
}
```

### City Database ###

```csharp
// This creates the DatabaseReader object, which should be reused across
// lookups.
using (var reader = new DatabaseReader("GeoIP2-City.mmdb"))
{
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
}
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

### Enterprise Database ###

```csharp
using (var reader = new DatabaseReader("/path/to/GeoIP2-Enterprise.mmdb"))
{
    //  Use the Enterprise(ip) method to do a lookup in the Enterprise database
    var response = reader.enterprise("128.101.101.101");

    var country = response.Country;
    Console.WriteLine(country.IsoCode);            // 'US'
    Console.WriteLine(country.Name);               // 'United States'
    Console.WriteLine(country.Names["zh-CN"]);     // '美国'
    Console.WriteLine(country.Confidence);         // 99

    var subdivision = response.MostSpecificSubdivision;
    Console.WriteLine(subdivision.Name);           // 'Minnesota'
    Console.WriteLine(subdivision.IsoCode);        // 'MN'
    Console.WriteLine(subdivision.Confidence);     // 77

    var city = response.City;
    Console.WriteLine(city.Name);       // 'Minneapolis'
    Console.WriteLine(city.Confidence); // 11

    var postal = response.Postal;
    Console.WriteLine(postal.Code); // '55455'
    Console.WriteLine(postal.Confidence); // 5

    var location = response.Location;
    Console.WriteLine(location.Latitude);       // 44.9733
    Console.WriteLine(location.Longitude);      // -93.2323
    Console.WriteLine(location.AccuracyRadius); // 50
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

## Values to use for Database or Dictionary Keys ##

**We strongly discourage you from using a value from any `Names` property as
a key in a database or dictionary.**

These names may change between releases. Instead we recommend using one of the
following:

* `MaxMind.GeoIP2.Model.City` - `City.GeoNameId`
* `MaxMind.GeoIP2.Model.Continent` - `Continent.Code` or `Continent.GeoNameId`
* `MaxMind.GeoIP2.Model.Country` and `MaxMind.GeoIP2.Model.RepresentedCountry`
  - `Country.IsoCode` or `Country.GeoNameId`
* `MaxMind.GeoIP2.Model.Subdivision` - `Subdivision.IsoCode` or
  `Subdivision.GeoNameId`

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

The API uses [Semantic Versioning](http://semver.org/). However, adding new
methods to `IGeoIP2Provider`, `IGeoIP2DatabaseReader`, and
`IGeoIP2WebServicesClient` will not be considered a breaking change. These
interfaces only exist to facilitate unit testing and dependency injection.
They will be updated to match additional methods added to the `DatabaseReader`
and `WebServiceClient`. Such additions will be accompanied by a minor version
bump (e.g., 1.2.x to 1.3.0).

## Copyright and License ##

This software is Copyright (c) 2013-2016 by MaxMind, Inc.

This is free software, licensed under the Apache License, Version 2.0.
