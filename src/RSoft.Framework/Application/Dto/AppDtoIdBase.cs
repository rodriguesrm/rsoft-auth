namespace RSoft.Framework.Application.Dto
{

    /// <summary>
    /// Abstract dto-id model base object
    /// </summary>
    /// <typeparam name="TKey">Entity key type</typeparam>
    public abstract class AppDtoIdBase<TKey> : AppDtoBase
        where TKey : struct
    {

        #region Properties

        /// <summary>
        /// Entity key
        /// </summary>
        public TKey Id { get; set; }

        #endregion

    }

}
