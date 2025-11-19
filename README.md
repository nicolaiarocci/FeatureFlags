# Feature Flags in ASP.NET Core

Demo project for my December 3rd session at **WPC 2025** on Feature Flags management in ASP.NET Core.

A [MVC version](https://github.com/nicolaiarocci/FeatureFlagsMvc?tab=readme-ov-file) is also available.

## Overview

This project demonstrates how to implement and use feature flags in an ASP.NET Core 9.0 application, starting from a basic example to advanced implementations with custom filters.

The application is a simple API that exposes a `/weatherforecast` endpoint whose availability is controlled through feature flags.

## Session Goals

- Understand what feature flags are and why they are useful
- Learn how to use `Microsoft.FeatureManagement.AspNetCore`
- Explore different configuration methods
- Use built-in filters for common scenarios
- Create custom filters for specific business logic

## Branches and Progression

The project is structured in sequential branches, each introducing new concepts. Follow the numerical order for the best learning experience.

### 01-start
**Initial branch** - Base application without feature flags.

```bash
git checkout 01-start
```

A simple Weather Forecast endpoint always active.

---

### 02-basic
**Disable feature with explicit code** - First introduction to feature flags.

```bash
git checkout 02-basic
```

**Concepts introduced:**
- Basic configuration of `IFeatureManager`
- Manual feature control in code
- `IsEnabledAsync()` method

---

### 03-appsettings
**Configuration via appsettings.json**

```bash
git checkout 03-appsettings
```

**Concepts introduced:**
- Feature flags configuration in `appsettings.json`
- `FeatureFlags` section in configuration
- Separation between code and configuration

**appsettings.json example:**
```json
{
  "FeatureFlags": {
    "WeatherForecast": true
  }
}
```

---

### 04-feature-packages
**Using Feature Filters for DRY**

```bash
git checkout 04-feature-packages
```

**Concepts introduced:**
- Custom Endpoint Filters
- Reusable pattern to protect endpoints
- `IEndpointFilter` with `FeatureManager`

---

### 05-fature-packages-and-filter
**Filters for DRY principle** (alternative branch)

```bash
git checkout 05-fature-packages-and-filter
```

Alternative implementation of the filters concept.

---

### 06-feature-packages-and-filters-with-groups
**Disable feature via group filters**

```bash
git checkout 06-feature-packages-and-filters-with-groups
```

**Concepts introduced:**
- `MapGroup()` to group endpoints
- Applying filters to endpoint groups
- Scalable API organization

---

### 07-built-in-percentage-filter
**Built-in percentage filter**

```bash
git checkout 07-built-in-percentage-filter
```

**Concepts introduced:**
- Built-in `PercentageFilter`
- Gradual feature rollout (e.g., 50% of users)
- A/B testing and canary releases

**Configuration example:**
```json
{
  "FeatureFlags": {
    "WeatherForecast": {
      "EnabledFor": [
        {
          "Name": "Percentage",
          "Parameters": {
            "Value": 50
          }
        }
      ]
    }
  }
}
```

---

### 08-built-in-time-window-filter
**Time window filter**

```bash
git checkout 08-built-in-time-window-filter
```

**Concepts introduced:**
- Built-in `TimeWindowFilter`
- Enable features only in specific time intervals
- Useful for time-limited features or scheduled maintenance

**Configuration example:**
```json
{
  "FeatureFlags": {
    "WeatherForecast": {
      "EnabledFor": [
        {
          "Name": "TimeWindow",
          "Parameters": {
            "Start": "2025-01-01T00:00:00Z",
            "End": "2025-12-31T23:59:59Z"
          }
        }
      ]
    }
  }
}
```

---

### 09-built-in-targeting-filter
**Targeting filter for specific users**

```bash
git checkout 09-built-in-targeting-filter
```

**Concepts introduced:**
- Built-in `TargetingFilter`
- Enable features for specific users or groups
- Percentage-based targeting per group
- `ITargetingContextAccessor` to provide user context

**Configuration example:**
```json
{
  "FeatureFlags": {
    "WeatherForecast": {
      "EnabledFor": [
        {
          "Name": "Targeting",
          "Parameters": {
            "Audience": {
              "Users": ["alice@example.com", "bob@example.com"],
              "Groups": [
                {
                  "Name": "BetaTesters",
                  "RolloutPercentage": 50
                }
              ],
              "DefaultRolloutPercentage": 10
            }
          }
        }
      ]
    }
  }
}
```

---

### 10-multiple-filters
**Multiple filters combination**

```bash
git checkout 10-multiple-filters
```

**Concepts introduced:**
- Using multiple filters simultaneously
- OR logic between filters (any one must be satisfied)
- Complex enablement strategies

**Configuration example:**
```json
{
  "FeatureFlags": {
    "WeatherForecast": {
      "EnabledFor": [
        {
          "Name": "Percentage",
          "Parameters": { "Value": 25 }
        },
        {
          "Name": "TimeWindow",
          "Parameters": {
            "Start": "2025-01-01T00:00:00Z"
          }
        }
      ]
    }
  }
}
```

---

### 11-custom-filter
**Custom filter**

```bash
git checkout 11-custom-filter
```

**Concepts introduced:**
- Implementing `IFeatureFilter`
- Custom feature enablement logic
- Custom filter registration

**Implementation:**
```csharp
[FilterAlias(nameof(MyCustomFilter))]
public class MyCustomFilter : IFeatureFilter
{
    public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
    {
        // Custom logic
        return Task.FromResult(true);
    }
}
```

---

### 12-custom-filter-with-parameters
**Custom filter with parameters and context**

```bash
git checkout 12-custom-filter-with-parameters
```

**Concepts introduced:**
- `IContextualFeatureFilter<TContext>` for contextual filters
- Configuration parameters for custom filters
- Runtime context passing (e.g., from HTTP headers)
- Parameter binding from configuration

**Implementation:**
```csharp
[FilterAlias(nameof(MyCustomFilter))]
public class MyCustomFilter : IContextualFeatureFilter<MyCustomFilterContext>
{
    public Task<bool> EvaluateAsync(
        FeatureFilterEvaluationContext evaluationContext,
        MyCustomFilterContext context)
    {
        var settings = evaluationContext.Parameters.Get<MyCustomFilterSettings>();
        return Task.FromResult(settings.LuckyNumber == context.InputNumber);
    }
}
```

**Configuration:**
```json
{
  "FeatureFlags": {
    "WeatherForecast": {
      "EnabledFor": [
        {
          "name": "MyCustomFilter",
          "parameters": {
            "LuckyNumber": 47
          }
        }
      ]
    }
  }
}
```

**Test with curl:**
```bash
# Feature disabled (wrong number)
curl -H "X-Lucky-Number: 42" http://localhost:5000/weatherforecast

# Feature enabled (correct number)
curl -H "X-Lucky-Number: 47" http://localhost:5000/weatherforecast
```

## How to Use This Project

### Prerequisites
- .NET 9.0 SDK
- Editor/IDE (Visual Studio, VS Code, Rider)

### Setup
1. Clone the repository
2. Navigate to the project directory

### Following the Session
1. Start from the `01-start` branch:
   ```bash
   git checkout 01-start
   dotnet run
   ```

2. Explore the base application

3. Move to the next branch:
   ```bash
   git checkout 02-basic
   ```

4. Continue sequentially through the branches

### Testing the API
```bash
# Start the application
dotnet run

# In another terminal, test the endpoint
curl http://localhost:5000/weatherforecast

# With custom header (branch 12)
curl -H "X-Lucky-Number: 47" http://localhost:5000/weatherforecast
```

## Key Concepts

### Feature Flags
Feature flags enable you to:
- Separate deployment from feature release
- Test features in production with limited users
- Immediate rollback without re-deployment
- A/B testing and experimentation
- Gradual releases (canary releases)

### Microsoft.FeatureManagement
Official Microsoft library that provides:
- Consistent API for feature flag management
- Integration with ASP.NET Core
- Built-in filters for common scenarios
- Extensibility through custom filters
- Support for configuration from various sources

### Built-in Filters
- **Percentage**: Enables for a percentage of requests
- **TimeWindow**: Enables in specific time intervals
- **Targeting**: Enables for specific users/groups

## Code Structure

```
FeatureFlags/
├── Program.cs                      # Configuration and endpoints
├── appsettings.json               # Feature flags configuration
├── FeatureFilter.cs               # Custom endpoint filter
├── MyCustomFilter.cs              # Custom filter with parameters
├── HttpTargetingContextAccessor.cs # (in some branches)
└── FeatureFlags.csproj            # Project dependencies
```

## Resources
- [Microsoft.FeatureManagement Documentation](https://learn.microsoft.com/en-us/azure/azure-app-configuration/use-feature-flags-dotnet-core)
- [Feature Flags Best Practices](https://learn.microsoft.com/en-us/devops/operate/progressive-experimentation-feature-flags)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)

## Author
[Nicola Iarocci](https://nicolaiarocci.com)

## Credits
Code sourced from Tim Deschryver's excellent [blog series](https://timdeschryver.dev/blog/feature-flags-in-net-from-simple-to-more-advanced).

---

**Enjoy the session!**
