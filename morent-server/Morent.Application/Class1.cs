// using System.ComponentModel.DataAnnotations;

// public class RegisterUserCommandValidator : IValidator<RegisterUserCommand>
// {
//   public Task<ValidationResult> ValidateAsync(RegisterUserCommand command, CancellationToken cancellationToken = default)
//   {
//     var result = new ValidationResult();

//     if (string.IsNullOrWhiteSpace(command.Name))
//       result.AddError("Name is required");

//     if (string.IsNullOrWhiteSpace(command.Email))
//       result.AddError("Email is required");
//     else if (!IsValidEmail(command.Email))
//       result.AddError("Email format is invalid");

//     if (string.IsNullOrWhiteSpace(command.Password))
//       result.AddError("Password is required");
//     else if (command.Password.Length < 8)
//       result.AddError("Password must be at least 8 characters long");

//     return Task.FromResult(result);
//   }

//   private bool IsValidEmail(string email)
//   {
//     try
//     {
//       var addr = new System.Net.Mail.MailAddress(email);
//       return addr.Address == email;
//     }
//     catch
//     {
//       return false;
//     }
//   }
// }

// public class RentCarCommandValidator : IValidator<RentCarCommand>
// {
//   public Task<ValidationResult> ValidateAsync(RentCarCommand command, CancellationToken cancellationToken = default)
//   {
//     var result = new ValidationResult();

//     if (command.CarId == Guid.Empty)
//       result.AddError("CarId is required");

//     if (command.PickupDate == default)
//       result.AddError("PickupDate is required");

//     if (command.DropoffDate == default)
//       result.AddError("DropoffDate is required");

//     if (command.PickupDate >= command.DropoffDate)
//       result.AddError("PickupDate must be before DropoffDate");

//     if (command.PickupDate.Date < DateTime.UtcNow.Date)
//       result.AddError("PickupDate cannot be in the past");

//     if (command.PickupLocation == null)
//       result.AddError("PickupLocation is required");
//     else
//     {
//       if (string.IsNullOrWhiteSpace(command.PickupLocation.Address))
//         result.AddError("PickupLocation.Address is required");
//       if (string.IsNullOrWhiteSpace(command.PickupLocation.City))
//         result.AddError("PickupLocation.City is required");
//       if (string.IsNullOrWhiteSpace(command.PickupLocation.Country))
//         result.AddError("PickupLocation.Country is required");
//     }

//     if (command.DropoffLocation == null)
//       result.AddError("DropoffLocation is required");
//     else
//     {
//       if (string.IsNullOrWhiteSpace(command.DropoffLocation.Address))
//         result.AddError("DropoffLocation.Address is required");
//       if (string.IsNullOrWhiteSpace(command.DropoffLocation.City))
//         result.AddError("DropoffLocation.City is required");
//       if (string.IsNullOrWhiteSpace(command.DropoffLocation.Country))
//         result.AddError("DropoffLocation.Country is required");
//     }

//     if (string.IsNullOrWhiteSpace(command.PaymentMethodId))
//       result.AddError("PaymentMethodId is required");

//     return Task.FromResult(result);
//   }
// }

// public class LeaveReviewCommandValidator : IValidator<LeaveReviewCommand>
// {
//   public Task<ValidationResult> ValidateAsync(LeaveReviewCommand command, CancellationToken cancellationToken = default)
//   {
//     var result = new ValidationResult();

//     if (command.CarId == Guid.Empty)
//       result.AddError("CarId is required");

//     if (command.RentalId == Guid.Empty)
//       result.AddError("RentalId is required");

//     if (command.Rating < 1 || command.Rating > 5)
//       result.AddError("Rating must be between 1 and 5");

//     if (string.IsNullOrWhiteSpace(command.Comment))
//       result.AddError("Comment is required");

//     return Task.FromResult(result);
//   }
// }

// // Additional validators would follow the same pattern

// public class ValidationResult
// {
//   private readonly List<string> _errors = new List<string>();

//   public IReadOnlyCollection<string> Errors => _errors.AsReadOnly();

//   public bool IsValid => !_errors.Any();

//   public void AddError(string error)
//   {
//     _errors.Add(error);
//   }
// }