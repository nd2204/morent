using Morent.Core.Entities;
using Morent.Infra.Data.Persistence;
using System;

namespace Scripts;

public class Program
{
  public static void ShowHelp()
  {
    Console.WriteLine("Usage: dotnet run [--help|-h]");
    Console.WriteLine("Options:");
    Console.WriteLine("  --help,-h        Show this help message");
    Console.WriteLine("  --generate, -g   [1|2] Choose an action");
    Console.WriteLine("      1: Generate TypeScript interfaces");
    Console.WriteLine("      2: Generate missing entities configuration classes");
  }

  public static void Main(string[] args)
  {
    Console.WriteLine("Choose an action:");
    Console.WriteLine("[0] Do nothing and exit");
    Console.WriteLine("[1] Generate TypeScript interfaces");
    Console.WriteLine("[2] Generate missing entities configuration classes");
    Console.Write("Enter your choice (default [1]): ");
    string? input = Console.ReadLine() ?? "";
    int choice = Convert.ToInt32(input == "" ? "1" : input);

    switch (choice)
    {
      case 0: break;
      case 1:
        TypeScriptGenerator.Generate();
        break;
      case 2:
        ConfigurationGenerator.GenerateMissingConfiguration();
        break;
      default:
        TypeScriptGenerator.Generate();
        break;
    }
  }
}
