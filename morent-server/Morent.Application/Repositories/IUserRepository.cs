using System;

namespace Morent.Application.Repositories;

public interface IUserRepository : IRepository<MorentUser>
{
    Task<MorentUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<MorentUser?> GetByUsername(string username, CancellationToken cancellationToken = default);
    Task<MorentUser?> GetByUsernameOrEmail(string usernameOrEmail, CancellationToken cancellationToken = default);
    Task<MorentUser?> GetByOAuthInfoAsync(string provider, string providerKey, CancellationToken cancellationToken = default);
    Task<MorentUser?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
}
