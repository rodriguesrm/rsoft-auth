using RSoft.Framework.Infra.Data;
using RSoft.Auth.Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace RSoft.Auth.Domain.Repositories
{

    /// <summary>
    /// User repository contract interface
    /// </summary>
    public interface IUserRepository : IRepositoryBase<User, Guid>
    {

        /// <summary>
        /// Get user by login or e-mail
        /// </summary>
        /// <param name="login">User login or e-mail</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<User> GetByLoginAsync(string login, CancellationToken cancellationToken);

    }

}
