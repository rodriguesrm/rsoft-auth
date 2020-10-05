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
            {
                entity.ChangedAuthor = new AuthorNullable<Guid>(_authenticatedUser.Id.Value, $"{_authenticatedUser.FirstName} {_authenticatedUser.LastName}");
                entity.ChangedOn = DateTime.UtcNow;
            }
            else
            {
                entity.CreatedAuthor = new Author<Guid>(_authenticatedUser.Id.Value, $"{_authenticatedUser.FirstName} {_authenticatedUser.LastName}");
                entity.CreatedOn = DateTime.UtcNow;
            }
            entity.Name = entity.Name.ToLower();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Find role in the scope by name
        /// </summary>
        /// <param name="scopeId">Scope id key</param>
        /// <param name="roleName">Role name</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        public async Task<Role> GetByNameAsync(Guid scopeId, string roleName, CancellationToken cancellationToken = default)
            => await _repository.GetByNameAsync(scopeId, roleName, cancellationToken);

        #endregion

    }

}
