using System;

namespace RSoft.Framework.Domain.Entities
{

    /// <summary>
    /// Log audit interface
    /// </summary>
    /// <typeparam name="TKey">Refence author creation/changed key type</typeparam>
    public interface IAudit<TKey>
        where TKey : struct
    {

        #region Properties

        /// <summary>
        /// Row's create date
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Row's changed date
        /// </summary>
        DateTime? ChangedOn { get; set; }

        /// <summary>
        /// User's id created row
        /// </summary>
        TKey CreatedBy { get; set; }

        /// <summary>
        /// User's id changed row
        /// </summary>
        TKey? ChangedBy { get; set; }

        #endregion

    }

}
