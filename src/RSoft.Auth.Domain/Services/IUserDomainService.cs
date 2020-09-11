using RSoft.Framework.Domain.Services;
using RSoft.Auth.Domain.Entities;
using System;

namespace RSoft.Auth.Domain.Services
{

    /// <summary>
    /// User domain service interface
    /// </summary>
    public interface IUserDomainService : IDomainServiceBase<User, Guid>
    {
    }

}
