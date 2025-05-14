using System.Reflection;

namespace Morent.WebApi;

public static class AssemblyReference
{
  public static Assembly Assembly => Assembly.GetExecutingAssembly();
}
