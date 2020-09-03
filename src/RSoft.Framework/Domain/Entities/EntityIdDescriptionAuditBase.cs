namespace RSoft.Framework.Domain.Entities
{

    /// <summary>
    /// Abstract entity class with id and description column
    /// </summary>
    /// <typeparam name="TKey">Entity key type</typeparam>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class EntityIdDescriptionAuditBase<TKey, TEntity> : EntityIdAuditBase<TKey, TEntity>
        where TKey : struct
        where TEntity : EntityIdAuditBase<TKey, TEntity>
    {

        #region Constructors

        /// <summary>
        /// Create a new entity instance
        /// </summary>
        public EntityIdDescriptionAuditBase() : base() { }

        /// <summary>
        /// Create a new entity instance
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <param name="description">Entity description value</param>
        public EntityIdDescriptionAuditBase(TKey id, string description) : base(id)
        {
            Description = description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Entity description value
        /// </summary>
        public string Description { get; set; }

        #endregion

    }

}
