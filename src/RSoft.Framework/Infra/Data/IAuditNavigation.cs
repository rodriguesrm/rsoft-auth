namespace RSoft.Framework.Infra.Data
{

    /// <summary>
    /// Audit with navigation properties interface
    /// </summary>
    /// <typeparam name="TKey">Status key type</typeparam>
    /// <typeparam name="TUser">User entity type</typeparam>
    public interface IAuditNavigation<TKey, TUser> : IAudit<TKey>
        where TKey : struct
        where TUser : class
    {

        /// <summary>
        /// Created author data
        /// </summary>
        TUser CreatedAuthor { get; set; }

        /// <summary>
        /// Changed author data
        /// </summary>
        TUser ChangedAuthor { get; set; }

    }

}
