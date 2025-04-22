using System;

namespace Morent.Application.Interfaces;

public interface IOAuthService
{
  Task<(string id, string email, string name, string pictureUrl)> ValidateGoogleTokenAsync(string token, CancellationToken cancellationToken = default);
  Task<(string id, string email, string name, string pictureUrl)> ValidateGitHubTokenAsync(string token, CancellationToken cancellationToken = default);
}
