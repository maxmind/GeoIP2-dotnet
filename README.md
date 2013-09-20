# GeoIP2-dotnet #

## Installation ##

### NuGet ###

We recommend installing this library with NuGet. To do this, type the following into the Visual Studio Package Manager Console:

```
install-package MaxMind.GeoIP2
```

## Usage ##

To use this API, you must create a new `MaxMind.GeoIP2.WebServiceClient` object with your `userID` and `licenseKey`. You may then call the method corresponding to a specific end point, passing it the IP address you want to look up.

If the request succeeds, the method call will return a response class for the endpoint you called. This response in turn contains multiple model classes, each of which represents part of the data returned by the web service.

See the API documentation for more details.

## Web Service Example ##

```CSharp
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

## Exceptions ##

For details on the possible errors returned by the web service itself, [see the GeoIP2 web service documentation](http://dev.maxmind.com/geoip2/geoip/web-services).

If the web service returns an explicit error document, this is thrown as a `GeoIP2AddressNotFoundException`, a `GeoIP2AuthenticationException`, a `GeoIP2InvalidRequestException`, or a `GeoIP2OutOfQueriesException`.

If some sort of transport error occurs, an `GeoIP2HttpException` is thrown. This is thrown when some sort of unanticipated error occurs, such as the web service returning a 500 or an invalid error document. If the web service request returns any status code besides 200, 4xx, or 5xx, this also becomes a `GeoIP2HttpException`.

Finally, if the web service returns a 200 but the body is invalid, the client throws a `GeoIP2Exception`. This exception also is the parent exception to the above exceptions.

## Requirements ##

This library works with .NET Framework version 3.5 and above. It has been tested with Mono 2.8, but should work with any version 1.9 and above.