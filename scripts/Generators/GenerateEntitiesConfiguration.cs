using System.Reflection;
using System.Text;

namespace Scripts;

public class ConfigurationGenerator
{
  public static void GenerateMissingConfiguration()
  {
    var entitiesPath = Path.Combine("../../morent-server", "Morent.Core", "Entities");
    var outputPath = Path.Combine("../../morent-server", "Morent.Infrastructure", "Data", "Configs");

    if (!Directory.Exists(outputPath))
    {
      Directory.CreateDirectory(outputPath);
    }

    var inputFiles = Directory.GetFiles(entitiesPath, "Morent*.cs");
    var configFiles = Directory.GetFiles(entitiesPath, "Morent*.cs");



    foreach (var file in inputFiles)
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