using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Scripts;

public class TypeScriptGenerator
{
    private static readonly Dictionary<string, string> TypeMappings = new()
    {
        { "int", "number" },
        { "long", "number" },
        { "decimal", "number" },
        { "double", "number" },
        { "float", "number" },
        { "string", "string" },
        { "bool", "boolean" },
        { "DateTime", "Date" },
        { "Guid", "string" },
        { "ICollection<", "Array<" },
        { "List<", "Array<" }
    };

    public static void Generate()
    {
        var currentAssemblyPath = Assembly.GetExecutingAssembly().Location;
        var scriptsDirectory = Path.GetDirectoryName(currentAssemblyPath)!;
        var solutionRoot = Directory.GetParent(scriptsDirectory)!.Parent!.Parent!.Parent!.FullName;

        var entitiesPath = Path.Combine(solutionRoot, "Morent.Core", "Entities");
        var outputPath = Path.Combine(solutionRoot, "morent-client", "src", "types");

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        var files = Directory.GetFiles(entitiesPath, "*.cs");
        var interfaceBuilder = new StringBuilder();

        interfaceBuilder.AppendLine("// Auto-generated - Do not modify");
        interfaceBuilder.AppendLine("// Generated on: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        interfaceBuilder.AppendLine();

        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            var className = Path.GetFileNameWithoutExtension(file);
            var properties = ExtractProperties(content);

            // Keep the Morent prefix
            interfaceBuilder.AppendLine($"export interface {className} {{");

            foreach (var prop in properties)
            {
                var (name, type) = prop;
                var tsType = ConvertToTypeScript(type);
                interfaceBuilder.AppendLine($"  {name}: {tsType};");
            }

            interfaceBuilder.AppendLine("}");
            interfaceBuilder.AppendLine();
        }

        File.WriteAllText(Path.Combine(outputPath, "morent-entities.ts"), interfaceBuilder.ToString());
        Console.WriteLine(interfaceBuilder.ToString());
    }

    private static List<(string Name, string Type)> ExtractProperties(string content)
    {
        var properties = new List<(string Name, string Type)>();

        // Updated regex to handle property initializers and nullable types
        var propertyPattern = @"public\s+(?:virtual\s+)?([^\s\?]+\??)\s+([^\s\{]+)\s*{\s*get;\s*set;\s*}\s*=?\s*[^;]*;";
        var matches = Regex.Matches(content, propertyPattern);

        // Track foreign key attributes to avoid duplicates
        var foreignKeyProps = new HashSet<string>();
        var fkPattern = @"\[ForeignKey\(nameof\((\w+)\)\)\]";
        var fkMatches = Regex.Matches(content, fkPattern);

        foreach (Match fk in fkMatches)
        {
            foreignKeyProps.Add(fk.Groups[1].Value);
        }

        foreach (Match match in matches)
        {
            var type = match.Groups[1].Value;
            var name = match.Groups[2].Value;

            // Skip if this is a foreign key property
            if (foreignKeyProps.Contains(name))
                continue;

            // Add navigation properties
            if (!foreignKeyProps.Contains(name))
            {
                properties.Add((name, type));
            }
        }

        return properties;
    }

    private static string ConvertToTypeScript(string csharpType)
    {
        // Handle nullable types first
        if (csharpType.EndsWith("?"))
        {
            var baseType = csharpType[..^1];
            var convertedType = ConvertToTypeScript(baseType);
            return $"{convertedType} | null";
        }

        // Handle collection types
        foreach (var mapping in TypeMappings.Where(m => m.Key.EndsWith("<")))
        {
            if (csharpType.StartsWith(mapping.Key))
            {
                var genericType = csharpType[mapping.Key.Length..^1];
                return $"{mapping.Value}{ConvertToTypeScript(genericType)}>";
            }
        }

        // Handle primitive types
        foreach (var mapping in TypeMappings.Where(m => !m.Key.EndsWith("<")))
        {
            if (csharpType == mapping.Key)
            {
                return mapping.Value;
            }
        }

        // For entity types, keep the Morent prefix
        return csharpType;
    }
}