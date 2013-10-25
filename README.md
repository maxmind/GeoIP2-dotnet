# GeoIP2 .NET API #

## Beta Note ##

This is a beta release. The API may change before the first production
release, which will be numbered 2.0.0.

You may find information on the GeoIP2 beta release process on [our
website](http://www.maxmind.com/en/geoip2_beta).

## Description ##

This distribution provides an API for the [GeoIP2 web services]
(http://dev.maxmind.com/geoip/geoip2/web-services) and the [GeoLite2
databases](http://dev.maxmind.com/geoip/geoip2/geolite2/). The commercial
GeoIP2 databases have not yet been released as a downloadable product.

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

## Usage ##

To use this API, you must create a new `MaxMind.GeoIP2.WebServiceClient`
object with your `userID` and `licenseKey`. You may then call the method
corresponding to a specific end point, passing it the IP address you want to
look up.

If the request succeeds, the method call will return a response class for the
endpoint you called. This response in turn contains multiple model classes,
each of which represents part of the data returned by the web service.

See the API documentation for more details.

## Web Service Example ##

```csharp
var client = new WebServiceClient(42, "abcdef12345");
var omni = client.Omni("128.101.101.101");

Console.WriteLine(omni.Country.IsoCode); // 'US'
Console.WriteLine(omni.Country.Name); // 'United States'
Console.WriteLine(omni.Country.Names["zh-CN"]); // '美国'

Console.WriteLine(omni.MostSpecificSubdivision.Name); // 'Minnesota'
Console.WriteLine(omni.MostSpecificSubdivision.IsoCode); // 'MN'

Console.WriteLine(omni.City.Name); // 'Minneapolis'

Console.WriteLine(omni.Postal.Code); // '55455'

Console.WriteLine(omni.Location.Latitude); // 44.9733
Console.WriteLine(omni.Location.Longitude); // -93.2323

```

## Database Example ##

```csharp
var reader = new DatabaseReader("GeoIP2-City.mmdb");
var omni = reader.Omni("128.101.101.101");

Console.WriteLine(omni.Country.IsoCode); // 'US'
Console.WriteLine(omni.Country.Name); // 'United States'
Console.WriteLine(omni.Country.Names["zh-CN"]); // '美国'

Console.WriteLine(omni.MostSpecificSubdivision.Name); // 'Minnesota'
Console.WriteLine(omni.MostSpecificSubdivision.IsoCode); // 'MN'

Console.WriteLine(omni.City.Name); // 'Minneapolis'

Console.WriteLine(omni.Postal.Code); // '55455'

Console.WriteLine(omni.Location.Latitude); // 44.9733
Console.WriteLine(omni.Location.Longitude); // -93.2323

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

### What data is returned? ###

While many of the end points return the same basic records, the attributes
which can be populated vary between end points. In addition, while an end
point may offer a particular piece of data, MaxMind does not always have every
piece of data for any given IP address.

Because of these factors, it is possible for any end point to return a record
where some or all of the attributes are unpopulated.

See the [GeoIP2 web service
docs](http://dev.maxmind.com/geoip/geoip2/web-services) for details on what
data each end point may return.

The only piece of data which is always returned is the `ipAddress` attribute
in the `GeoIp2\Record\Traits` record.

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
[GitHub issue tracker](https://github.com/maxmind/GeoIP2-php/issues).

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
