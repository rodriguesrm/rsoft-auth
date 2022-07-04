using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using System.Threading.Tasks;
using System.Threading;
using RSoft.Lib.Design.Domain.Services;
using RSoft.Lib.Common.Contracts.Web;
using RSoft.Lib.Common.ValueObjects;

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
        /// Find role in the application-client by name
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        public async Task<Role> GetByNameAsync(string roleName, CancellationToken cancellationToken = default)
            => await _repository.GetByNameAsync(roleName, cancellationToken);

        #endregion

    }

}
