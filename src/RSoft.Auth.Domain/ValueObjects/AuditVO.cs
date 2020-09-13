using System;

namespace RSoft.Auth.Domain.ValueObjects
{

    /// <summary>
    /// Audit value object model
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    public class AuditVO<TKey> : BaseVO
        where TKey : struct
    {

        #region Constructors

        /// <summary>
        /// Create a new AuditVO instance
        /// </summary>
        /// <param name="createdOn">Created date</param>
        /// <param name="changedOn">Changed date</param>
        /// <param name="createdBy">Created author</param>
        /// <param name="changedBy">Changed author</param>
        public AuditVO(DateTime createdOn, TKey createdBy, DateTime? changedOn, TKey? changedBy)
        {
            CreatedOn = createdOn;
            ChangedOn = changedOn;
            CreatedBy = createdBy;
            ChangedBy = changedBy;
            Validate();
        }

        /// <summary>
        /// Create a new AuditVO instance
        /// </summary>
        /// <param name="createdOn">Created date</param>
        /// <param name="createdBy">Created author</param>
        public AuditVO(DateTime createdOn, TKey createdBy) : this(createdOn, createdBy, null, null) { }

        #endregion

        #region Properties

        /// <summary>
        /// Row's create date
        /// </summary>
        public DateTime CreatedOn { get; private set; }

        /// <summary>
        /// Row's changed date
        /// </summary>
        public DateTime? ChangedOn { get; private set; }

        /// <summary>
        /// User's id created row
        /// </summary>
        public TKey CreatedBy { get; private set; }

        /// <summary>
        /// User's id changed row
        /// </summary>
        public TKey? ChangedBy { get; private set; }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override void Validate()
        {
            //TODO: Globalization
            //TODO: NotImplementedException
            throw new NotImplementedException();
        }

        #endregion

    }
}
