using RSoft.Framework.Domain.Contracts;

namespace RSoft.Framework.Domain.ValueObjects
{

    /// <summary>
    /// Nullable author value object model
    /// </summary>
    public class AuthorNullable<TKey> : BaseVO
        where TKey : struct
    {

        #region Constructors

        /// <summary>
        /// Create a new Nullable-Author-Value-Object instance
        /// </summary>
        /// <param name="Id">Id key value</param>
        /// <param name="name">Author name</param>
        public AuthorNullable(TKey Id, string name)
        {
            this.Id = Id;
            Name = name;
            Validate();
        }

        #endregion

        #region Properties

        public TKey? Id { get; private set; }

        /// <summary>
        /// Author name
        /// </summary>
        public string Name { get; private set; }

        #endregion

        #region Overrides

        ///<inheritdoc/>
        protected override void Validate()
        {
            AddNotifications(new SimpleStringValidationContract(Name, nameof(Name), false, 2, 150).Contract.Notifications);
        }

        #endregion

    }
}
