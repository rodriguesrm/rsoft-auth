using RSoft.Auth.Domain.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Lib.Design.Domain.Services;
using RSoft.Lib.Common.ValueObjects;
using RSoft.Lib.Common.Contracts.Web;

namespace RSoft.Auth.Domain.Services
{

    /// <summary>
    /// Application-Client domain service operations
    /// </summary>
    public class AppClientDomainService : DomainServiceBase<AppClient, Guid, IAppClientRepository>, IAppClientDomainService
    {

        #region Constructors

        /// <summary>
        /// Create a new scopde domain service instance
        /// </summary>
        /// <param name="repository">Application-Client repository</param>
        /// <param name="authenticatedUser">Authenticated user</param>
        public AppClientDomainService(IAppClientRepository repository, IAuthenticatedUser authenticatedUser) : base(repository, authenticatedUser)
        {
        }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        public override void PrepareSave(AppClient entity, bool isUpdate)
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
        }

        #endregion

    }

}
