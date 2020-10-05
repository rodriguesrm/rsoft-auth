namespace RSoft.Framework.Infra.Data.Tables
{

    /// <summary>
    /// Abstract table entity class with id column
    /// </summary>
    public abstract class TableIdBase<TKey, TTable> : TableBase<TTable>
        where TTable : TableIdBase<TKey, TTable>
        where TKey : struct
    {

        #region Constructors

        /// <summary>
        /// Create a new table entity instance
        /// </summary>
        public TableIdBase() { }


        /// <summary>
        /// Create a new table entity instance
        /// </summary>
        /// <param name="id">Table entity id value</param>
        public TableIdBase(TKey id)
        {
            Id = id;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Table entity id
        /// </summary>
        public TKey Id { get; protected set; }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public override string ToString()
            => $"{GetType().Name} - Id = {Id}";

        #endregion

    }

}