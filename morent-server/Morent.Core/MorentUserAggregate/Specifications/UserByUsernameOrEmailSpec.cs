using System;

namespace Morent.Core.MorentUserAggregate.Specifications;

public class UserByUsernameOrEmailSpec : Specification<MorentUser>
{
  public UserByUsernameOrEmailSpec(string? username, string? email)
  {
    Query.Where(user => user.Username == username || user.Email == email);
  }

  public UserByUsernameOrEmailSpec(string usernameOrEmail)
  {
    Query.Where(user => user.Username == usernameOrEmail || user.Email == usernameOrEmail);
  }
}
