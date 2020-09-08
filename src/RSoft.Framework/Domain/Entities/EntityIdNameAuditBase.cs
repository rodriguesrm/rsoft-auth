namespace RSoft.Framework.Domain.Entities
{

    /// <summary>
    /// Abstract entity class with id and description column
    /// </summary>
    /// <typeparam name="TKey">Entity key type</typeparam>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class EntityIdNameAuditBase<TKey, TEntity> : EntityIdAuditBase<TKey, TEntity>
        where TKey : struct
        where TEntity : EntityIdAuditBase<TKey, TEntity>
    {

        #region Constructors

        /// <summary>
        /// Create a new entity instance
        /// </summary>
        public EntityIdNameAuditBase() : base() { }

        /// <summary>
        /// Create a new entity instance
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <param name="description">Entity description value</param>
        public EntityIdNameAuditBase(TKey id, string description) : base(id)
        {
            Name = description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Entity name value
        /// </summary>
        public string Name { get; set; }

        #endregion

    }

}
