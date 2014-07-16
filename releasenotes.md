GeoIP2 .NET API Release Notes
=============================

0.4.0 (2014-07-xx)
------------------

* The web service client API has been updated for the v2.1 release of the web
  service. In particular, the `CityIspOrg` and `Omni` methods on
  `WebServiceClient` have been deprecated. The `City` method now provides all
  of the data formerly provided by `CityIspOrg`, and the `Omni` method has
  been replaced by the `Insights` method.


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
