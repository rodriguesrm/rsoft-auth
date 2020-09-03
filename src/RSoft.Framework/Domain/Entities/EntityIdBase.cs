namespace RSoft.Framework.Domain.Entities
{

    /// <summary>
    /// Abstract entity class with id column
    /// </summary>
    /// <typeparam name="TKey">Entity key type</typeparam>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class EntityIdBase<TKey, TEntity> : EntityBase<TEntity> 
        where TEntity : EntityIdBase<TKey, TEntity>
    {

        #region Construtores

        /// <summary>
        /// Create a new entity instance
        /// </summary>
        public EntityIdBase() : base() { }

        /// <summary>
        /// Create a new entity instance
        /// </summary>
        /// <param name="id">Entity id</param>
        public EntityIdBase(TKey id) : base()
        {
            Id = id;
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// Entity id
        /// </summary>
        public TKey Id { get; protected set; }

        #endregion

        #region Overrides

        /// <summary>
        /// Compare two objects
        /// </summary>
        /// <param name="obj">Object to compare</param>
        public override bool Equals(object obj)
        {
            var compareTo = obj as EntityIdBase<TKey, TEntity>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (compareTo is null) return false;

            return Id.Equals(compareTo.Id);
        }

        /// <summary>
        /// Get HashCode 
        /// </summary>
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 293) + Id.GetHashCode();
        }

        /// <summary>
        /// Get string object
        /// </summary>
        public override string ToString()
        {
            return $"{GetType().Name} - Id = {Id}";
        }

        #endregion

    }

}
