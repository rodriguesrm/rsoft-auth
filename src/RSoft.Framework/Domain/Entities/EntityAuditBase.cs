using RSoft.Framework.Domain.ValueObjects;
using System;

namespace RSoft.Framework.Domain.Entities
{

    /// <summary>
    /// Entity abstract class with id column
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TKey">Entity key type</typeparam>
    public abstract class EntityAuditBase<TEntity, TKey> : EntityBase<TEntity>, IEntity, IAuditAuthor<TKey>
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