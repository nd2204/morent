using System;
using FluentValidation.Results;

namespace Morent.Application.Exceptions;

public class BadRequestException : Exception
{
  public List<string> ValidationErrors { get; set; } = new();

  public BadRequestException(string message) : base(message) { }

  public BadRequestException(string message, ValidationResult result) : base(message)
  {
    foreach (var error in result.Errors) {
      ValidationErrors.Add(error.ErrorMessage);
    }
  }
}
