using System;

namespace Morent.Core.Exceptions;

public class DomainException : Exception
{
  public DomainException(string message) : base(message)
  {
  }

  public DomainException(string message, Exception innerException) : base(message, innerException)
  {
  }
}