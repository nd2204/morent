using System;
using System.Reflection;

namespace Morent.Core;

public static class AssemblyReference
{
  public static Assembly Assembly => Assembly.GetExecutingAssembly();
}
