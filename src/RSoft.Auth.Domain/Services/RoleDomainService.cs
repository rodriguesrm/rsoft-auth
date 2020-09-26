using RSoft.Framework.Domain.Services;
using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Framework.Cross;
using RSoft.Framework.Domain.ValueObjects;
using System.Threading.Tasks;
using System.Threading;

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
        /// <param name="repository">Role repository</param>
        /// <param name="authenticatedUser">Authenticaed user</param>
        public RoleDomainService(IRoleRepository repository, IAuthenticatedUser authenticatedUser) : base(repository, authenticatedUser)
        {
        }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        public override void PrepareSave(Role entity, bool isUpdate)
        {
            if (isUpdate)
                entity.ChangedAuthor = new AuthorNullable<Guid>(_authenticatedUser.Id.Value, $"{_authenticatedUser.FirstName} {_authenticatedUser.LastName}");
            else
                entity.CreatedAuthor = new Author<Guid>(_authenticatedUser.Id.Value, $"{_authenticatedUser.FirstName} {_authenticatedUser.LastName}");
        }

        ///<inheritdoc/>
        protected override async Task<Role> FindAsync(Role entity, CancellationToken cancellationToken = default)
        {
            Guid[] keys = new Guid[] { entity.Id };
            return await GetByKeyAsync(keys, cancellationToken);
        }

        #endregion

    }

}
