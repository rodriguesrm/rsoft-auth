using System;

namespace RSoft.Framework.Infra.Data.Tables
{

    /// <summary>
    /// Abstract table entity class with id column
    /// </summary>
    /// <typeparam name="TKey">Table entity key type</typeparam>
    /// <typeparam name="TTable">Table entity type</typeparam>
    public abstract class TableIdAuditBase<TKey, TTable> : TableIdBase<TKey, TTable>, IAudit<TKey>
        where TKey : struct
        where TTable : TableIdAuditBase<TKey, TTable>
    {

        #region Constructors

        /// <summary>
        /// Create a new table entity instance
        /// </summary>
        public TableIdAuditBase() : base() { }

        /// <summary>
        /// Create a new table entity instance
        /// </summary>
        /// <param name="id">Entity id</param>
        public TableIdAuditBase(TKey id) : base(id) { }

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
