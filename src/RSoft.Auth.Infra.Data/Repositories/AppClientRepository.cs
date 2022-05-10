using AppClientDomain = RSoft.Auth.Domain.Entities.AppClient;
using RSoft.Auth.Infra.Data.Entities;
using RSoft.Auth.Domain.Repositories;
using System;
using RSoft.Auth.Infra.Data.Extensions;
using RSoft.Lib.Design.Infra.Data;

namespace RSoft.Auth.Infra.Data.Repositories
{

    /// <summary>
    /// Application-Client repository
    /// </summary>
    public class AppClientRepository : RepositoryBase<AppClientDomain, AppClient, Guid>, IAppClientRepository
    {

        #region Constructors

        ///<inheritdoc/>
        public AppClientRepository(AuthContext ctx) : base(ctx) { }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override AppClientDomain Map(AppClient table)
            => table.Map();

        ///<inheritdoc/>
        protected override AppClient MapForAdd(AppClientDomain entity)
            => entity.Map();

        ///<inheritdoc/>
        protected override AppClient MapForUpdate(AppClientDomain entity, AppClient table)
            => entity.Map(table);

        #endregion

    }

}
