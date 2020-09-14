namespace RSoft.Framework.Domain.ValueObjects
{

    /// <summary>
    /// Audit value object model
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    public class Audit<TKey> : BaseVO
        where TKey : struct
    {


        #region Constructors

        /// <summary>
        /// Create a new Audit-Value-Object instance
        /// </summary>
        /// <param name="creationAuthor">Creation author data</param>
        /// <param name="changeAuthor">Last change author data</param>
        public Audit(Author<TKey> creationAuthor, AuthorNullable<TKey> changeAuthor)
        {
            CreationAuthor = creationAuthor;
            ChangeAuthor = changeAuthor;
            Validate();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Creation author data
        /// </summary>
        public Author<TKey> CreationAuthor { get; set; }

        /// <summary>
        /// Last change author data
        /// </summary>
        public AuthorNullable<TKey> ChangeAuthor { get; set; }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override void Validate()
        {
            //TODO: Globalization
            if (CreationAuthor == null)
                AddNotification(nameof(CreationAuthor), $"{nameof(CreationAuthor)} is required");
            AddNotifications(CreationAuthor?.Notifications);
            AddNotifications(ChangeAuthor?.Notifications);
        }

        #endregion

    }
}
