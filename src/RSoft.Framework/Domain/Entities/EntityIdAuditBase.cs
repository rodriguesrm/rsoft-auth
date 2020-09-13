using RSoft.Framework.Infra.Data;
using System;

namespace RSoft.Framework.Domain.Entities
{

    /// <summary>
    /// Abstract entity class with id column
    /// </summary>
    /// <typeparam name="TKey">Entity key type</typeparam>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class EntityIdAuditBase<TKey, TEntity> : EntityIdBase<TKey, TEntity>, IAudit<TKey>
        where TKey : struct
        where TEntity : EntityIdAuditBase<TKey, TEntity>
    {

        #region Constructors

        /// <summary>
        /// Create a new entity instance
        /// </summary>
        public EntityIdAuditBase() : base() { }

        /// <summary>
        /// Create a new entity instance
        /// </summary>
        /// <param name="id">Entity id</param>
        public EntityIdAuditBase(TKey id) : base(id) { }

        #endregion

        #region Properties

        /// <summary>
        /// Log creation date
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Log creation user id
        /// </summary>
        public TKey CreatedBy { get; set; }

        /// <summary>
        /// Log change date
        /// </summary>
        public DateTime? ChangedOn { get; set; }

        /// <summary>
        /// Log change user id
        /// </summary>
        public TKey? ChangedBy { get; set; }

        #endregion

    }

}
