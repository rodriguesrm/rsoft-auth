using RSoft.Framework.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Framework.Infra.Data
{

    /// <summary>
    /// Generic repository interface
    /// </summary>
    public interface IRepositoryBase<TEntity, TKey> : ICommonBase<TEntity, TKey>
        where TEntity : EntityBase<TEntity>
        where TKey : struct
    {

    }

}