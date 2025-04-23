using Morent.Application.Repositories;
using Morent.Core.ValueObjects;

namespace Morent.Infrastructure.Data.Repositories;

public class UserRepository : EFRepository<MorentUser>, IUserRepository
{
  private readonly MorentDbContext _dbContext;

  public UserRepository(MorentDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<MorentUser?> GetByUsername(string username, CancellationToken cancellationToken)
  {
    return await _dbContext.Users
        .SingleOrDefaultAsync(u => u.Username == username, cancellationToken);
  }

  public async Task<MorentUser?> GetByEmailAsync(string email, CancellationToken cancellationToken)
  {
    return await _dbContext.Users
        .SingleOrDefaultAsync(u => u.Email.Value == email, cancellationToken);
  }

  public async Task<MorentUser?> GetByOAuthInfoAsync(string provider, string providerKey, CancellationToken cancellationToken)
  {
    return await _dbContext.Users
    .Include(u => u.OAuthLogins)
    .Include(u => u.RefreshTokens)
    .FirstOrDefaultAsync(u =>
        u.OAuthLogins.Any(l =>
            l.Provider == provider &&
            l.ProviderKey == providerKey));
  }

  public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Users
        .AnyAsync(u => u.Email.Value == email);
  }


  public async Task<MorentUser?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Users
        .Include(u => u.RefreshTokens)
        .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken), cancellationToken);
  }

  public async Task<MorentUser?> GetByUsernameOrEmail(string usernameOrEmail, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Users
        .SingleOrDefaultAsync(u => u.Email.Value == usernameOrEmail || u.Username == usernameOrEmail, cancellationToken);
  }
}