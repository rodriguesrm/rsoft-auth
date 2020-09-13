using RSoft.Framework.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Framework.Domain.Services
{

    /// <summary>
    /// Services domain insterface
    /// </summary>
    public interface IDomainServiceBase<TEntity, TKey> : ICommonBase<TEntity, TKey>
        where TEntity : EntityBase<TEntity>
        where TKey : struct
    {

    }

}
