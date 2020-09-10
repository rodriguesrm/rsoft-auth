namespace RSoft.Framework.Application.Model
{

    /// <summary>
    /// Abstract dto-id-audit model base object
    /// </summary>
    public abstract class AppDtoIdAuditBase<TKey> : AppDtoIdBase<TKey>
        where TKey : struct
    {

        #region Properties

        /// <summary>
        /// Created author data
        /// </summary>
        public AuditAuthor<TKey> CreatedBy { get; set; }

        /// <summary>
        /// Changed author data
        /// </summary>
        public AuditAuthor<TKey> ChangedBy { get; set; }

        #endregion

    }

}
