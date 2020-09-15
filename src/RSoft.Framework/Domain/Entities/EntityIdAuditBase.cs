using RSoft.Framework.Domain.ValueObjects;
using System;

namespace RSoft.Framework.Domain.Entities
{

    /// <summary>
    /// Abstract entity class with id column
    /// </summary>
    /// <typeparam name="TKey">Entity key type</typeparam>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class EntityIdAuditBase<TKey, TEntity> : EntityIdBase<TKey, TEntity>, IAuditAuthor<TKey>
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

        ///<inheritdoc/>
        public DateTime CreatedOn { get; set; }

        ///<inheritdoc/>
        public Author<TKey> CreatedAuthor { get; set; }

        ///<inheritdoc/>
        public DateTime? ChangedOn { get; set; }

        ///<inheritdoc/>
        public AuthorNullable<TKey> ChangedAuthor { get; set; }

        #endregion

    }

}