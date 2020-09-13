using System;

namespace RSoft.Framework.Infra.Data.Tables
{

    /// <summary>
    /// Table entity abstract class with id column
    /// </summary>
    /// <typeparam name="TTable">Table entity type</typeparam>
    /// <typeparam name="TKey">Table entity key</typeparam>
    public abstract class TableAuditBase<TTable, TKey> : TableBase<TTable>, ITable, IAudit<TKey>
        where TTable : TableBase<TTable>
        where TKey : struct
    {

        #region Constructors

        public TableAuditBase() : base() { }

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
