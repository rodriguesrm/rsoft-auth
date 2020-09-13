namespace RSoft.Framework.Infra.Data.Tables
{

    /// <summary>
    /// Abstract table entity class with id and description column
    /// </summary>
    /// <typeparam name="TKey">Table entity key type</typeparam>
    /// <typeparam name="TTable">Table entity type</typeparam>
    public abstract class TableIdNameAuditBase<TKey, TTable> : TableIdAuditBase<TKey, TTable>
        where TKey : struct
        where TTable : TableIdAuditBase<TKey, TTable>
    {

        #region Constructors

        /// <summary>
        /// Create a new table entity instance
        /// </summary>
        public TableIdNameAuditBase() : base() { }

        /// <summary>
        /// Create a new table entity instance
        /// </summary>
        /// <param name="id">Id value</param>
        /// <param name="name">Name value</param>
        public TableIdNameAuditBase(TKey id, string name) : base(id) 
        {
            Name = name;
        }

        #region Properties

        /// <summary>
        /// Table name value
        /// </summary>
        public string Name { get; set; }

        #endregion



        #endregion

    }

}
