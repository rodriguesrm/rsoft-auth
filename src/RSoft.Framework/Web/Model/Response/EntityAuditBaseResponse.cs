using RSoft.Framework.Application.Dto;
using RSoft.Framework.Application.Model;

namespace RSoft.Framework.Web.Model.Response
{

    /// <summary>
    /// Abstract audit-response base object
    /// </summary>
    public abstract class EntityAuditBaseResponse<TKey> : EntityBaseResponse, IAuditDto<TKey>
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
