using RSoft.Framework.Domain.Entities;
using System;

namespace RSoft.Framework.Domain.ValueObjects
{

    /// <summary>
    /// Audit value object model
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    public class Audit<TKey> : BaseVO, IAuditAuthor<TKey> 
        where TKey : struct
    {


        #region Constructors

        /// <summary>
        /// Create a new Audit-Value-Object instance
        /// </summary>
        /// <param name="createdOn">Row created date</param>
        /// <param name="createdAuthor">Created author data</param>
        /// <param name="changedOn">Row changed date</param>
        /// <param name="changeAuthor">Last change author data</param>
        public Audit(DateTime createdOn, Author<TKey> createdAuthor, DateTime? changedOn, AuthorNullable<TKey> changeAuthor)
        {
            CreatedOn = createdOn;
            CreatedAuthor = createdAuthor;
            ChangedOn = changedOn;
            ChangedAuthor = changeAuthor;
            Validate();
        }

        #endregion

        #region Properties

        ///<inheritdoc/>
        public DateTime CreatedOn { get; set; }

        ///<inheritdoc/>
        public Author<TKey> CreatedAuthor { get; set; }

        ///<inheritdoc/>
        public DateTime? ChangedOn { get; set; }

        ///<inheritdoc/>
        public AuthorNullable<TKey> ChangedAuthor { get; set; }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override void Validate()
        {
            //BACKLOG: Globalization
            if (CreatedAuthor == null)
                AddNotification(nameof(CreatedAuthor), $"{nameof(CreatedAuthor)} is required");
            if (CreatedAuthor != null) AddNotifications(CreatedAuthor.Notifications);
            if (ChangedAuthor != null) AddNotifications(ChangedAuthor.Notifications);
        }

        #endregion

    }
}