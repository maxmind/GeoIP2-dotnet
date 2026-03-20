# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with
code in this repository.

## Project Overview

**GeoIP2-dotnet** is MaxMind's official .NET client library for:

- **GeoIP2/GeoLite2 Web Services**: Country, City, and Insights endpoints
- **GeoIP2/GeoLite2 Databases**: Local MMDB file reading for various database
  types (City, Country, ASN, Anonymous IP, Anonymous Plus, ISP, etc.)

The library provides both web service clients and database readers that return
strongly-typed model objects containing geographic, ISP, anonymizer, and other
IP-related data.

## Code Architecture

### Project Structure

```
MaxMind.GeoIP2/
├── Responses/          # Response models (CityResponse, InsightsResponse, etc.)
├── Model/              # Data models (City, Location, Traits, etc.)
├── Exceptions/         # Custom exceptions for error handling
├── Http/               # HTTP client infrastructure
├── DatabaseReader.cs   # Local MMDB file reader
├── WebServiceClient.cs # HTTP client for MaxMind web services
└── I*Provider/I*Client interfaces for testing and DI

MaxMind.GeoIP2.UnitTests/
├── TestData/           # Test MMDB databases (via submodule)
├── DatabaseReaderTests.cs
├── WebServiceClientTests.cs
└── DeserializationTests.cs
```

### Key Design Patterns

#### 1. **Immutable Model Records with Init Properties**

Model and response classes use C# records with `init` properties and three
attribute types:

- `[JsonPropertyName]` for JSON field mapping (System.Text.Json)
- `[MapKey("field_name")]` for MaxMind DB field mapping
- `[MapKey("field_name", true)]` for nested sub-objects (constructs from a
  sub-map rather than a scalar field)
- `[Inject("field_name")]` injects metadata like IP address
- `[Network]` injects network information for the IP

No constructors needed — deserialization uses property initialization.

#### 2. **Conditional Compilation**

Some features are only available in newer .NET versions (e.g., `DateOnly` in
.NET 6+). When adding features that use newer .NET types, ensure backward
compatibility with .NET Standard 2.0/2.1.

#### 3. **MaxMind DB Date Parsing with Backing Fields**

For date fields stored as strings in MMDB databases, use a backing field with an
internal string property for deserialization. Invalid values should throw a
`GeoIP2Exception` with enough context to identify the field and bad value:

```csharp
#if NET6_0_OR_GREATER
private DateOnly? _networkLastSeen;

[JsonInclude]
[JsonPropertyName("network_last_seen")]
public DateOnly? NetworkLastSeen
{
    get => _networkLastSeen;
    init => _networkLastSeen = value;
}

[JsonIgnore]
[MapKey("network_last_seen")]
internal string? NetworkLastSeenString
{
    get => _networkLastSeen?.ToString("o");
    init => _networkLastSeen = value == null ? null
        : DateOnly.TryParse(value, out var result) ? result
        : throw new GeoIP2Exception(
            $"Could not parse 'network_last_seen' value '{value}' as a valid date.");
}
#endif
```

#### 4. **Database vs Web Service Architecture**

- **DatabaseReader**: Reads MMDB files via `MaxMind.Db`. Thread-safe, reuse
  across lookups. May throw `AddressNotFoundException` or
  `InvalidOperationException`.
- **WebServiceClient**: Uses `HttpClient`. Thread-safe, reuse for connection
  pooling. Supports sync and async. Supports DI via
  `IOptions<WebServiceClientOptions>` and `IHttpClientFactory`.

#### 5. **Exception Hierarchy**

```
GeoIP2Exception
├── AddressNotFoundException      // IP not found in database or blocked
├── AuthenticationException       // Invalid credentials
├── InvalidRequestException       // Invalid request parameters
├── OutOfQueriesException        // Query limit exceeded
├── PermissionRequiredException  // Service requires higher tier
└── HttpException                // Network/HTTP errors
```

## Testing

```bash
# Build all projects
dotnet build MaxMind.GeoIP2
dotnet build MaxMind.GeoIP2.UnitTests
dotnet build MaxMind.GeoIP2.Benchmark

# Run all tests
dotnet test MaxMind.GeoIP2.UnitTests/MaxMind.GeoIP2.UnitTests.csproj

# Run specific test class
dotnet test --filter "FullyQualifiedName~DatabaseReaderTests"

# Run benchmark
dotnet run -f net8.0 -p MaxMind.GeoIP2.Benchmark/MaxMind.GeoIP2.Benchmark.csproj
```

- Test databases are in `MaxMind.GeoIP2.UnitTests/TestData/MaxMind-DB/` (git
  submodule)
- `MAXMIND_TEST_BASE_DIR` env var must point to the test project directory (set
  automatically in CI)

## Working with This Codebase

### Adding New Fields to Existing Models

1. **Add the property** with appropriate attributes:

   ```csharp
   [JsonInclude]
   [JsonPropertyName("field_name")]
   [MapKey("field_name")]  // If supported by database
   public TypeName? FieldName { get; init; }
   ```

2. **No constructor changes needed** — adding a new `init` property with a
   default value is not a breaking change.

3. **Update tests** with assertions for the new field

4. **Update `releasenotes.md`** with the change

### Adding New Response Types

1. Determine if it extends an existing response (`AbstractCityResponse`,
   `AbstractCountryResponse`, `AbstractResponse`)
2. Follow patterns from similar responses
3. Add corresponding method in `DatabaseReader.cs` and
   `IGeoIP2DatabaseReader.cs`
4. Add web service method (if applicable) in `WebServiceClient.cs` and
   `IGeoIP2WebServicesClient.cs`
5. Provide comprehensive XML documentation for all public members
6. Add unit tests in `DatabaseReaderTests.cs` or `WebServiceClientTests.cs`

### Deprecation Guidelines

1. Use `[Obsolete]` with helpful messages pointing to alternatives
2. Keep deprecated functionality working
3. Update `releasenotes.md` with deprecation notices
4. Use `#pragma warning disable 618` around internal uses of deprecated APIs

### releasenotes.md Format

Always update `releasenotes.md` for user-facing changes:

```markdown
## X.Y.Z (YYYY-MM-DD)

- A new `PropertyName` property has been added to
  `MaxMind.GeoIP2.Model.ModelClass`. This provides information about...
- The `OldProperty` property in `MaxMind.GeoIP2.Model.ModelClass` has been
  marked `Obsolete`. Please use `NewProperty` instead.
- **BREAKING:** Description of any breaking changes (major versions only).
```

### What IS Breaking in Minor Versions

- Removing or renaming existing properties
- Changing the type of existing properties
- Changing default values of existing properties

## Code Quality

- **TreatWarningsAsErrors** and **EnforceCodeStyleInBuild** are enabled — code
  style violations and warnings are build errors
- Use `.editorconfig` settings for formatting
- XML documentation required for all public types and members
- Prefer alphabetical ordering for `init` properties unless there is a
  preexisting logical grouping

## Version Requirements

- **Target Frameworks**: net10.0, net9.0, net8.0, netstandard2.1, netstandard2.0
- **Language Version**: C# 14.0
- Key dependencies and their versions are defined in `MaxMind.GeoIP2.csproj`.

## Additional Resources

- [API Documentation](https://maxmind.github.io/GeoIP2-dotnet/)
- [GeoIP2 Web Services Docs](https://dev.maxmind.com/geoip/docs/web-services?lang=en)
- [GeoIP2 Database Docs](https://dev.maxmind.com/geoip/docs/databases?lang=en)
- [MaxMind DB Format](https://maxmind.github.io/MaxMind-DB/)
- GitHub Issues: https://github.com/maxmind/GeoIP2-dotnet/issues

---

_Last Updated: 2026-03-20_
