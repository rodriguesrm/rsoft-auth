using RSoft.Framework.Infra.Data;
using RSoft.Auth.Domain.Entities;
using System;

namespace RSoft.Auth.Domain.Repositories
{

    /// <summary>
    /// User repository contract interface
    /// </summary>
    public interface IUserRepository : IRepositoryBase<User, Guid>  { }

}
