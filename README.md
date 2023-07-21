# AperiodicCode.SourceGenerators.FeatureManagement

Generates constants based on features defined in `appsettings.json`.

## Pre-requisites

Must include the following in your project file:

```xml
<ItemGroup>
    <AdditionalFiles Include="appsettings.json" />
</ItemGroup>
```

## Example

Input `appsettings.json`:

```json
{
  "FeatureManagement": {
    "TestFeature1": true,
    "TestFeature2": false
  }
}
```

Output:

```cs
public static class FeatureConstants
{
    public const string TestFeature1 = "TestFeature1";
    public const string TestFeature2 = "TestFeature2";
}
```

## Limitations

Only works with `appsettings.json` currently. Does not support other files (like `appsettings.{Environment}.json`).