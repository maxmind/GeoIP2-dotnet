#region

using System.Diagnostics.CodeAnalysis;

#endregion

[assembly:
    SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Scope = "type",
        Target = "MaxMind.GeoIP2.Model.MaxMind")]
[assembly:
    SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope = "member",
        Target = "MaxMind.GeoIP2.Model.NamedEntity.#Locales")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Scope = "member",
        Target = "MaxMind.GeoIP2.Model.RepresentedCountry.#Type")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#", Scope = "member",
        Target = "MaxMind.GeoIP2.WebServiceClient.#.ctor(System.Int32,System.String,System.String,System.Int32)")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1058:TypesShouldNotExtendCertainBaseTypes", Scope = "type",
        Target = "MaxMind.GeoIP2.Exceptions.GeoIP2Exception")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member",
        Target = "MaxMind.GeoIP2.Responses.AbstractCityResponse.#Subdivisions")]