namespace RSoft.Framework.Infra.Data
{

    /// <summary>
    /// Soft deletion interface
    /// </summary>
    public interface ISoftDeletion
    {

        /// <summary>
        /// Soft deletion flag
        /// </summary>
        bool IsDeleted { get; set; }

    }

}
