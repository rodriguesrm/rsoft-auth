namespace RSoft.Framework.Cross.Entities
{

    /// <summary>
    /// Entity status interface
    /// </summary>
    /// <typeparam name="TKey">Status key type</typeparam>
    public interface IStatus<TKey>
        where TKey : struct
    {

        /// <summary>
        /// Status id
        /// </summary>
        TKey? StatusId { get; set; }

    }

}
