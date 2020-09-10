namespace RSoft.Framework.Application.Model
{

    /// <summary>
    /// Simple registration data structure (Key + Name)
    /// </summary>
    public class SimpleIdentification<TKey>
        where TKey : struct
    {

        /// <summary>
        /// Creates a new instance of the structure
        /// </summary>
        /// <param name="id">Id key value</param>
        /// <param name="name">Full name</param>
        public SimpleIdentification(TKey? id, string name)
        {
            Id = id;
            Nome = name;
        }

        /// <summary>
        /// Id key value
        /// </summary>
        public TKey? Id { get; set; }

        /// <summary>
        /// Full name
        /// </summary>
        public string Nome { get; set; }

    }

}
