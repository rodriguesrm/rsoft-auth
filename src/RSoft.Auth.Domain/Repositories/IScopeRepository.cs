using RSoft.Framework.Infra.Data;
using RSoft.Auth.Domain.Entities;
using System;

namespace RSoft.Auth.Domain.Repositories
{

    /// <summary>
    /// Scope repository contract interface
    /// </summary>
    public interface IScopeRepository : IRepositoryBase<Scope, Guid>  { }

}
