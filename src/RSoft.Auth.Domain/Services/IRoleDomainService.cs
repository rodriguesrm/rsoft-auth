using RSoft.Framework.Domain.Services;
using RSoft.Auth.Domain.Entities;
using System;

namespace RSoft.Auth.Domain.Services
{

    /// <summary>
    /// Role domain service interface
    /// </summary>
    public interface IRoleDomainService : IDomainServiceBase<Role, Guid>
    {
    }

}
