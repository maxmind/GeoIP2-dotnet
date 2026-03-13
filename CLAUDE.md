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

**Key Technologies:**

- .NET 10.0, .NET 9.0, .NET 8.0, .NET Standard 2.1, and .NET Standard 2.0
- System.Text.Json for JSON serialization/deserialization
- MaxMind.Db library for binary database file reading
- xUnit for testing
- Modern C# features (nullable reference types, collection expressions, etc.)

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

Model and response classes use C# records with `init` properties:

```csharp
public record Traits
{
    [JsonInclude]
    [JsonPropertyName("autonomous_system_number")]
    [MapKey("autonomous_system_number")]
    public long? AutonomousSystemNumber { get; init; }

    [JsonInclude]
    [JsonPropertyName("autonomous_system_organization")]
    [MapKey("autonomous_system_organization")]
    public string? AutonomousSystemOrganization { get; init; }
}
```

**Key Points:**

- Use `[JsonPropertyName]` for JSON field mapping (System.Text.Json)
- Use `[MapKey("field_name")]` for MaxMind DB field mapping (replaces
  `[Parameter]`)
- Properties use `init` setters for immutability
- No constructors needed — deserialization uses property initialization
- Records provide built-in equality, `with` expressions, and `ToString()`

#### 2. **Conditional Compilation for .NET Version Differences**

Some features are only available in newer .NET versions (e.g., `DateOnly` in
.NET 6+):

```csharp
#if NET6_0_OR_GREATER
    [JsonInclude]
    [JsonPropertyName("network_last_seen")]
    public DateOnly? NetworkLastSeen { get; init; }
#endif
```

When adding features that use newer .NET types, ensure backward compatibility
with .NET Standard 2.0/2.1.

#### 3. **Deprecation Strategy**

When deprecating properties, mark them with `[Obsolete]` and provide guidance:

```csharp
[Obsolete("The metro code is no longer being maintained and should not be used.")]
public int? MetroCode { get; init; }
```

#### 4. **Database vs Web Service Architecture**

**Database Reader:**

- Reads binary MMDB files using `MaxMind.Db` library
- Methods may throw `AddressNotFoundException` or `InvalidOperationException`
- Support for multiple database types via specific methods (`City()`,
  `Country()`, `AnonymousIP()`, etc.)
- Thread-safe, should be reused across lookups

**Web Service Client:**

- Uses `HttpClient` for HTTP requests
- Methods throw `GeoIP2Exception` or subclasses on errors
- Supports custom timeouts, locales, host configuration, and dependency
  injection
- Thread-safe, connection pooling via client reuse
- Supports both sync and async methods

#### 5. **MaxMind DB Attributes**

Records that are deserialized from MMDB databases use special attributes on
properties:

- `[MapKey("field_name")]`: Maps a property to a database field (replaces
  `[Parameter]`)
- `[MapKey("field_name", true)]`: Maps a single nested sub-object. The `true`
  flag tells the deserializer to construct the nested object from a sub-map
  rather than a scalar field
- `[Inject("field_name")]`: Injects metadata like IP address
- `[Network]`: Injects the network information for the IP

#### 6. **ASP.NET Core Integration**

The library supports dependency injection via
`IOptions<WebServiceClientOptions>` and `IHttpClientFactory` pattern for typed
clients. Configuration is done via `appsettings.json`.

## Testing Conventions

### Test Structure

- Tests use xUnit framework
- Test databases are in `MaxMind.GeoIP2.UnitTests/TestData/MaxMind-DB/` (git
  submodule)
- Environment variable `MAXMIND_TEST_BASE_DIR` must point to the test project
  directory

### Running Tests

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

### Test Fixtures

When adding new fields to responses:

1. Verify test databases in `TestData/MaxMind-DB/test-data/` include the new
   fields
2. Update test assertions in corresponding test classes
3. Test both database reader and web service client paths

## Working with This Codebase

### Adding New Fields to Existing Models

1. **Add the property** with appropriate attributes:

   ```csharp
   [JsonInclude]
   [JsonPropertyName("field_name")]
   [MapKey("field_name")]  // If supported by database
   public TypeName? FieldName { get; init; }
   ```

2. **No constructor changes needed** — records use property initialization, so
   adding a new `init` property with a default value is not a breaking change.

3. **Update tests** with assertions for the new field

4. **Update `releasenotes.md`** with the change

### Avoiding Breaking Changes in Minor Versions

With records using `init` properties and object initializer syntax, adding new
properties is inherently non-breaking — consumers using
`new Traits { Domain = "example.com" }` are unaffected by new properties.

**What IS breaking in minor versions:**

- Removing or renaming existing properties
- Changing the type of existing properties
- Changing default values of existing properties

### Adding New Response Types

When creating a new response class (e.g., for a new database type):

1. **Determine if it extends existing response** (e.g., `AbstractCityResponse`,
   `AbstractCountryResponse`, `AbstractResponse`)
2. **Follow patterns** from similar responses
3. **Add corresponding database reader method** in `DatabaseReader.cs` and
   `IGeoIP2DatabaseReader.cs`
4. **Add corresponding web service method** (if applicable) in
   `WebServiceClient.cs` and `IGeoIP2WebServicesClient.cs`
5. **Provide comprehensive XML documentation** for all public members
6. **Add unit tests** in `DatabaseReaderTests.cs` or `WebServiceClientTests.cs`

### Deprecation Guidelines

When deprecating fields or methods:

1. **Use `[Obsolete]` attribute** with helpful messages pointing to alternatives
2. **Keep deprecated functionality working** - don't break existing code
3. **Update `releasenotes.md`** with deprecation notices
4. **Use `#pragma warning disable 618` around internal uses** of deprecated APIs

### releasenotes.md Format

Always update `releasenotes.md` for user-facing changes:

```markdown
## 5.4.0 (YYYY-MM-DD)

- A new `PropertyName` property has been added to
  `MaxMind.GeoIP2.Model.ModelClass`. This provides information about...
- The `OldProperty` property in `MaxMind.GeoIP2.Model.ModelClass` has been
  marked `Obsolete`. Please use `NewProperty` instead.
- **BREAKING:** Description of any breaking changes (major versions only).
```

### Multi-threaded Safety

Both `DatabaseReader` and `WebServiceClient` are **thread-safe** and should be
reused:

- Create once, share across threads
- Reusing clients improves performance (connection pooling for web service
  client)
- Document thread-safety in XML documentation for all client classes

## Common Patterns

### Pattern: Exception Hierarchy

All GeoIP2 exceptions inherit from `GeoIP2Exception`:

```
GeoIP2Exception
├── AddressNotFoundException      // IP not found in database or blocked
├── AuthenticationException       // Invalid credentials
├── InvalidRequestException       // Invalid request parameters
├── OutOfQueriesException        // Query limit exceeded
├── PermissionRequiredException  // Service requires higher tier
└── HttpException                // Network/HTTP errors
```

### Pattern: Nullable Reference Types

The codebase uses nullable reference types (`<Nullable>enable</Nullable>`):

- Use `?` for nullable reference types: `string?`
- Use `?` for nullable value types: `int?`, `long?`
- Ensure all public APIs properly annotate nullability

### Pattern: Target Framework Conditionals

Different target frameworks may require different approaches:

```csharp
#if NETSTANDARD2_0 || NETSTANDARD2_1
    // .NET Standard-specific code (e.g., synchronous HTTP client)
#else
    // .NET Core/.NET 5+ code
#endif
```

### Pattern: MaxMind DB Date Parsing with Backing Fields

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

## Code Quality

### Static Analysis

The project enforces strict code quality standards:

- **EnforceCodeStyleInBuild**: Code style violations are build errors
- **TreatWarningsAsErrors**: All warnings must be resolved
- **EnableNETAnalyzers**: .NET code analyzers are enabled
- **.editorconfig**: Defines consistent coding style

### Code Style

- Use the `.editorconfig` settings for consistent formatting
- Follow C# naming conventions (PascalCase for public members, camelCase for
  parameters)
- Use XML documentation for all public types and members
- Prefer alphabetical ordering for `init` properties unless there is a
  preexisting logical grouping

## Version Requirements

- **Target Frameworks**: net10.0, net9.0, net8.0, netstandard2.1, netstandard2.0
- **Language Version**: C# 14.0
- **Key Dependencies**:
  - `MaxMind.Db`: 4.2.0+ (MMDB database reader)
  - `Microsoft.Extensions.Options`: 9.0.4+ (DI configuration)
  - `System.Text.Json`: 9.0.0+ (for .NET Standard targets)

## Additional Resources

- [API Documentation](https://maxmind.github.io/GeoIP2-dotnet/)
- [GeoIP2 Web Services Docs](https://dev.maxmind.com/geoip/docs/web-services?lang=en)
- [GeoIP2 Database Docs](https://dev.maxmind.com/geoip/docs/databases?lang=en)
- [MaxMind DB Format](https://maxmind.github.io/MaxMind-DB/)
- GitHub Issues: https://github.com/maxmind/GeoIP2-dotnet/issues

---

_Last Updated: 2026-03-13_
