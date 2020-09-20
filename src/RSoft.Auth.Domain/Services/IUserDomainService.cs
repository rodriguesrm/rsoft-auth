using RSoft.Framework.Domain.Services;
using RSoft.Auth.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Auth.Domain.Services
{

    /// <summary>
    /// User domain service interface
    /// </summary>
    public interface IUserDomainService : IDomainServiceBase<User, Guid>
    {

        /// <summary>
        /// Get user by login and password
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="password">User passoword</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<User> GetByLoginAsync(string login, string password, CancellationToken cancellationToken = default);
    }

}
