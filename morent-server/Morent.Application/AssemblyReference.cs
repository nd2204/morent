using System.Reflection;

namespace Morent.Application;

public static class AssemblyReference
{
  public static Assembly Assembly => Assembly.GetExecutingAssembly();
}
