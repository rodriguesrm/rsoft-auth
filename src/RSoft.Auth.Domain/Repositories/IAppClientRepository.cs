using RSoft.Auth.Domain.Entities;
using System;
using RSoft.Lib.Design.Infra.Data;

namespace RSoft.Auth.Domain.Repositories
{

    /// <summary>
    /// Application-Client repository contract interface
    /// </summary>
    public interface IAppClientRepository : IRepositoryBase<AppClient, Guid>  { }

}
