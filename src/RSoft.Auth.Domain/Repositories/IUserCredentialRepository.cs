using RSoft.Auth.Domain.Entities;
using RSoft.Lib.Design.Infra.Data;
using System;

namespace RSoft.Auth.Domain.Repositories
{

    /// <summary>
    /// User credential repository interface
    /// </summary>
    public interface IUserCredentialRepository : IRepositoryBase<UserCredential, Guid>
    {
    }

}
