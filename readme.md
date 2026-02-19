# <img src="/src/icon.png" height="30px"> Verify.OpenTelemetry

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://img.shields.io/appveyor/build/SimonCropp/verify-opentelemetry)](https://ci.appveyor.com/project/SimonCropp/verify-opentelemetry)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.OpenTelemetry.svg)](https://www.nuget.org/packages/Verify.OpenTelemetry/)

Extends [Verify](https://github.com/VerifyTests/Verify) to allow verification of [OpenTelemetry](https://www.nuget.org/packages/OpenTelemetry) types including [System.Diagnostics.Activity](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activity) and [LogRecord](https://github.com/open-telemetry/opentelemetry-dotnet)<!-- singleLineInclude: intro. path: /docs/intro.include.md -->

**See [Milestones](../../milestones?state=closed) for release notes.**


## Sponsors


### Entity Framework Extensions<!-- include: zzz. path: /docs/zzz.include.md -->

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.OpenTelemetry) is a major sponsor and is proud to contribute to the development this project.

[![Entity Framework Extensions](https://raw.githubusercontent.com/VerifyTests/Verify.OpenTelemetry/refs/heads/main/docs/zzz.png)](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.OpenTelemetry)<!-- endInclude -->


## NuGet

* https://nuget.org/packages/Verify.OpenTelemetry


## Usage

<!-- snippet: Enable -->
<a id='snippet-Enable'></a>
```cs
[ModuleInitializer]
public static void Initialize() =>
    VerifyOpenTelemetry.Initialize();
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L3-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-Enable' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Activity Verification

`VerifyOpenTelemetry` allows, when a method is being tested, for any [Activity](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activity) created as part of that method call to be recorded and verified.

Call `Recording.Start()` to begin listening. All activities from any `ActivitySource` will be captured by default.

<!-- snippet: Usage -->
<a id='snippet-Usage'></a>
```cs
[Fact]
public Task Usage()
{
    Recording.Start();
    using var source = new ActivitySource("TestSource");

    using (var activity = source.StartActivity("MyOperation"))
    {
        activity!.SetTag("key1", "value1");
        activity.SetTag("key2", 42);
    }

    return Verify("result");
}
```
<sup><a href='/src/Tests/Tests.cs#L3-L20' title='Snippet source file'>snippet source</a> | <a href='#snippet-Usage' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Results in:

<!-- snippet: Tests.Usage.verified.txt -->
<a id='snippet-Tests.Usage.verified.txt'></a>
```txt
{
  target: result,
  activity: {
    MyOperation: {
      Tags: {
        key1: value1,
        key2: 42
      }
    }
  }
}
```
<sup><a href='/src/Tests/Tests.Usage.verified.txt#L1-L11' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.Usage.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### LogRecord Verification

OpenTelemetry `LogRecord` instances can be captured using `InMemoryExporter` and verified directly.

<!-- snippet: LogRecordUsage -->
<a id='snippet-LogRecordUsage'></a>
```cs
[Fact]
public Task LogRecordVerification()
{
    var logRecords = new List<LogRecord>();
    using var loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddOpenTelemetry(options =>
        {
            options.AddInMemoryExporter(logRecords);
        });
    });

    var logger = loggerFactory.CreateLogger("TestCategory");
    logger.LogInformation("Hello {Name}", "World");

    return Verify(logRecords);
}
```
<sup><a href='/src/Tests/Tests.cs#L120-L140' title='Snippet source file'>snippet source</a> | <a href='#snippet-LogRecordUsage' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Results in:

<!-- snippet: Tests.LogRecordVerification.verified.txt -->
<a id='snippet-Tests.LogRecordVerification.verified.txt'></a>
```txt
[
  {
    CategoryName: TestCategory,
    LogLevel: Information,
    Body: Hello {Name},
    Attributes: {
      {OriginalFormat}: Hello {Name},
      Name: World
    }
  }
]
```
<sup><a href='/src/Tests/Tests.LogRecordVerification.verified.txt#L1-L11' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.LogRecordVerification.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Serialization

Activities are serialized with the following conventions:

* `OperationName` is used as the JSON property key
* `DisplayName` only included if different from `OperationName`
* `Kind` only included if not `Internal`
* `Status` and `StatusDescription` only included if not `Unset`
* `Tags`, `Events`, `Links`, and `Baggage` included when present
* Non-deterministic values (`Id`, `TraceId`, `SpanId`, `ParentSpanId`, `Duration`, `StartTimeUtc`, `Source`) are omitted

LogRecords are serialized with the following conventions:

* `Timestamp`, `TraceId`, `SpanId` are omitted (non-deterministic)
* `CategoryName`, `LogLevel`, `Body`, `FormattedMessage` included when present
* `EventId` only included if non-default
* `Exception` included when present
* `Attributes` included when present

## Icon

[Diagnostic](https://thenounproject.com/icon/diagnostic-8246832/) from [The Noun Project](https://thenounproject.com).
