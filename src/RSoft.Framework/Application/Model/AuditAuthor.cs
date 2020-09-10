using System;

namespace RSoft.Framework.Application.Model
{

    /// <summary>
    /// Creating and changing data author data structure
    /// </summary>
    public class AuditAuthor<TKey>
        where TKey : struct
    {

        /// <summary>
        /// Create a new object instance
        /// </summary>
        /// <param name="date">Occurrence date</param>
        /// <param name="id">Author id key value</param>
        /// <param name="name">Author full name</param>
        public AuditAuthor(DateTime date, TKey id, string name)
        {
            Date = date;
            Id = id;
            Name = name;
        }


        /// <summary>
        /// Occurrence date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Author key value
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// Author full name
        /// </summary>
        public string Name { get; set; }

    }

}
