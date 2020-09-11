using RSoft.Framework.Domain.Services;
using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Repositories;
using System;

namespace RSoft.Auth.Domain.Services
{

    /// <summary>
    /// Role domain service operations
    /// </summary>
    public class RoleDomainService : DomainServiceBase<Role, Guid, IRoleRepository>, IRoleDomainService
    {

        #region Constructors

        /// <summary>
        /// Create a new scopde domain service instance
        /// </summary>
        public RoleDomainService(IRoleRepository repository) : base(repository)
        {
        }

        #endregion

    }

}
