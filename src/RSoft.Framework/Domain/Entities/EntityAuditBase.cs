using RSoft.Framework.Infra.Data;
using System;

namespace RSoft.Framework.Domain.Entities
{

    /// <summary>
    /// Entity abstract class with id column
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TKey">Entity key type</typeparam>
    public abstract class EntityAuditBase<TEntity, TKey> : EntityBase<TEntity>, IEntity, IAudit<TKey>
        where TEntity : EntityBase<TEntity>
        where TKey : struct
    {

        #region Constructors

        /// <summary>
        /// Create a new entity instance
        /// </summary>
        public EntityAuditBase() : base() { }

        #endregion

        #region Properties

        /// <summary>
        /// Log creation date
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Log creation user id
        /// </summary>
        TKey IAudit<TKey>.CreatedBy { get; set; }

        /// <summary>
        /// Log change date
        /// </summary>
        public DateTime? ChangedOn { get; set; }

        /// <summary>
        /// Log change user id
        /// </summary>
        TKey? IAudit<TKey>.ChangedBy { get; set; }

        #endregion

    }

}