using RSoft.Framework.Domain.Services;
using RSofth.Auth.Domain.Entities;
using RSofth.Auth.Domain.Repositories;
using System;

namespace RSofth.Auth.Domain.Services
{

    /// <summary>
    /// User domain service operations
    /// </summary>
    public class UserDomainService : DomainServiceBase<User, Guid, IUserRepository>, IUserDomainService
    {

        #region Constructors

        /// <summary>
        /// Create a new scopde domain service instance
        /// </summary>
        public UserDomainService(IUserRepository repository) : base(repository)
        {
        }

        #endregion

    }

}
