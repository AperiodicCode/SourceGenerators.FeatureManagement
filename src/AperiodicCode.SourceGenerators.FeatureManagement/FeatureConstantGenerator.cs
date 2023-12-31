using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AperiodicCode.SourceGenerators.FeatureManagement;

[Generator]
public class FeatureConstantGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        try
        {
            var settingsFiles = context.AdditionalTextsProvider.Where(
                static file => file.Path.EndsWith("appsettings.json", StringComparison.OrdinalIgnoreCase)
            );

            // generate a class that contains their values as const strings
            context.RegisterSourceOutput(
                settingsFiles,
                (sourceContext, file) =>
                {
                    var text = file.GetText()?.ToString();

                    GenerateFeatureConstantsClass(text, sourceContext);
                }
            );
        }
        catch (Exception e)
        {
            context.RegisterPostInitializationOutput(errorContext =>
            {
                errorContext.AddSource("Error.g.cs", $"/** {e} **/");
            });
        }
    }

    private static void GenerateFeatureConstantsClass(string? featureManagementJson, SourceProductionContext context)
    {
        if (featureManagementJson is null)
        {
            return;
        }

        var root = JsonDocument.Parse(featureManagementJson).RootElement;

        if (!root.TryGetProperty("FeatureManagement", out var featureManagement))
        {
            return;
        }

        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine("// <auto-generated />");
        sourceBuilder.AppendLine("public static class FeatureConstants");
        sourceBuilder.AppendLine("{");

        foreach (var feature in featureManagement.EnumerateObject())
        {
            sourceBuilder.AppendLine($"    public const string {feature.Name} = \"{feature.Name}\";");
        }

        sourceBuilder.AppendLine("}");

        context.AddSource("FeatureConstants.g.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
    }
}
