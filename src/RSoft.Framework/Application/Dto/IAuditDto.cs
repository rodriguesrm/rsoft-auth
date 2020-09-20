using RSoft.Framework.Application.Model;

namespace RSoft.Framework.Application.Dto
{

    /// <summary>
    /// Audit Dto interface
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IAuditDto<TKey>
        where TKey : struct
    {

        /// <summary>
        /// Created author data
        /// </summary>
        AuditAuthor<TKey> CreatedBy { get; set; }

        /// <summary>
        /// Changed author data
        /// </summary>
        AuditAuthor<TKey> ChangedBy { get; set; }

    }

}
