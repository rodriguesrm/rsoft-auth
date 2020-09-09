using RSoft.Framework.Domain.Services;
using RSofth.Auth.Domain.Entities;
using System;

namespace RSofth.Auth.Domain.Services
{

    /// <summary>
    /// Role domain service interface
    /// </summary>
    public interface IRoleDomainService : IDomainServiceBase<Role, Guid>
    {
    }

}
