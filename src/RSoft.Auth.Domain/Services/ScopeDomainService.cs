using RSoft.Framework.Domain.Services;
using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Framework.Cross;
using RSoft.Framework.Domain.ValueObjects;

namespace RSoft.Auth.Domain.Services
{

    /// <summary>
    /// Scope domain service operations
    /// </summary>
    public class ScopeDomainService : DomainServiceBase<Scope, Guid, IScopeRepository>, IScopeDomainService
    {

        #region Constructors

        /// <summary>
        /// Create a new scopde domain service instance
        /// </summary>
        /// <param name="repository">Scope repository</param>
        /// <param name="authenticatedUser">Authenticated user</param>
        public ScopeDomainService(IScopeRepository repository, IAuthenticatedUser authenticatedUser) : base(repository, authenticatedUser)
        {
        }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        public override void PrepareSave(Scope entity, bool isUpdate)
        {
            if (isUpdate)
                entity.ChangedAuthor = new AuthorNullable<Guid>(_authenticatedUser.Id.Value, $"{_authenticatedUser.FirstName} {_authenticatedUser.LastName}");
            else
                entity.CreatedAuthor = new Author<Guid>(_authenticatedUser.Id.Value, $"{_authenticatedUser.FirstName} {_authenticatedUser.LastName}");
            entity.Prefix = entity.Prefix.ToUpper();
        }

        #endregion

    }

}
