using RSoft.Framework.Infra.Data;
using RSofth.Auth.Domain.Entities;
using System;

namespace RSofth.Auth.Domain.Repositories
{

    /// <summary>
    /// User repository contract interface
    /// </summary>
    public interface IUserRepository : IRepositoryBase<User, Guid>  { }

}
