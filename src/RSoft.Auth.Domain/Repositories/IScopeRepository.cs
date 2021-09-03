using RSoft.Auth.Domain.Entities;
using System;
using RSoft.Lib.Design.Infra.Data;

namespace RSoft.Auth.Domain.Repositories
{

    /// <summary>
    /// Scope repository contract interface
    /// </summary>
    public interface IScopeRepository : IRepositoryBase<Scope, Guid>  { }

}
