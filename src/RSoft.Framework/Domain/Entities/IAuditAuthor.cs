using RSoft.Framework.Domain.ValueObjects;
using System;

namespace RSoft.Framework.Domain.Entities
{
    public interface IAuditAuthor<TKey>
        where TKey : struct
    {
        
        /// <summary>
        /// Row create date
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Created author data
        /// </summary>
        Author<TKey> CreatedAuthor { get; set; }

        /// <summary>
        /// Row changed date
        /// </summary>
        DateTime? ChangedOn { get; set; }

        /// <summary>
        /// Last change author data
        /// </summary>
        AuthorNullable<TKey> ChangedAuthor { get; set; }

    }
}