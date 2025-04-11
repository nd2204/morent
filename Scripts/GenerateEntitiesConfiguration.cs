using System.Reflection;
using System.Text;

namespace Scripts;

public class ConfigurationGenerator
{
  public static void GenerateMissingConfiguration()
  {
    var currentAssemblyPath = Assembly.GetExecutingAssembly().Location;
    var scriptsDirectory = Path.GetDirectoryName(currentAssemblyPath)!;
    var solutionRoot = Directory.GetParent(scriptsDirectory)!.Parent!.Parent!.Parent!.FullName;

    var entitiesPath = Path.Combine(solutionRoot, "Morent.Core", "Entities");
    var outputPath = Path.Combine(solutionRoot, "Morent.Infra", "Data", "Configurations");

    if (!Directory.Exists(outputPath))
    {
      Directory.CreateDirectory(outputPath);
    }

    var files = Directory.GetFiles(entitiesPath, "Morent*.cs");

    foreach (var file in files)
    {
      var className = Path.GetFileNameWithoutExtension(file);
      var configContent = GenerateConfigurationClass(className);
      var outputFile = Path.Combine(outputPath, $"{className}Configuration.cs");

      if (File.Exists(outputFile)) continue;

      File.WriteAllText(outputFile, configContent);
      Console.WriteLine($"Generated: {outputFile}:\n{configContent}\n");
    }
  }

  private static string GenerateConfigurationClass(string entityName)
  {
    var builder = new StringBuilder();
    builder.AppendLine("using Microsoft.EntityFrameworkCore;");
    builder.AppendLine("using Microsoft.EntityFrameworkCore.Metadata.Builders;");
    builder.AppendLine("using Morent.Core.Entities;");
    builder.AppendLine();
    builder.AppendLine("namespace Morent.Infra.Data.Configurations;");
    builder.AppendLine();
    builder.AppendLine($"public class {entityName}Configuration : IEntityTypeConfiguration<{entityName}>");
    builder.AppendLine("{");
    builder.AppendLine("    public void Configure(EntityTypeBuilder<" + entityName + "> builder)");
    builder.AppendLine("    {");
    builder.AppendLine("    }");
    builder.AppendLine("}");

    return builder.ToString();
  }
}