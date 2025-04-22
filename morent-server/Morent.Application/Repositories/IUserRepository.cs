using System;

namespace Morent.Application.Repositories;

public interface IUserRepository : IRepository<MorentUser>
{
    Task<MorentUser> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<MorentUser> GetByOAuthInfoAsync(OAuthProvider provider, string oauthId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
}
