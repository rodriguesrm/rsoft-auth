using RSoft.Framework.Domain.Services;
using RSofth.Auth.Domain.Entities;
using System;

namespace RSofth.Auth.Domain.Services
{

    /// <summary>
    /// User domain service interface
    /// </summary>
    public interface IUserDomainService : IDomainServiceBase<User, Guid>
    {
    }

}
