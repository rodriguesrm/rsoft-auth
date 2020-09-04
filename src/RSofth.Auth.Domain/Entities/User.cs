using RSoft.Framework.Domain.Contracts;
using RSoft.Framework.Domain.Entities;
using System;

namespace RSofth.Auth.Domain.Entities
{

    public class User : EntityIdAuditBase<Guid, User>, IEntity, IAuditNavigation<Guid, User>, ISoftDeletion, IActive, IFullName
    {

        #region Local objects/variables

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new user instance
        /// </summary>
        public User() : base(Guid.NewGuid())
        {
            Initialize();
        }

        /// <summary>
        /// Create a new user instance
        /// </summary>
        /// <param name="id">User id value</param>
        public User(Guid id) : base(id)
        {
            Initialize();
        }

        /// <summary>
        /// Create a new user instance
        /// </summary>
        /// <param name="id">User id text</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.OverflowException"></exception>
        public User(string id) : base()
        {
            Id = new Guid(id);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Indicate if entity is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Soft deletion
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User's date of birth
        /// </summary>
        public DateTime? BornDate { get; set; }

        /// <summary>
        /// User e-mail
        /// </summary>
        public string Email { get; set; }

        #endregion

        #region Navigation Lazy

        /// <summary>
        /// Created author data
        /// </summary>
        public User CreatedAuthor { get; set; }

        /// <summary>
        /// Changed author data
        /// </summary>
        public User ChangedAuthor { get; set; }

        #endregion

        #region Local Methods

        private void Initialize()
        {
            //Roles = new HashSet<UserRole>();
            IsActive = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Validate entity
        /// </summary>
        public override void Validate()
        {

            //TODO: Globalization
            AddNotifications(new FullNameValidationContract(this).Contract.Notifications);
            AddNotifications(new EmailValidationContract(Email).Contract.Notifications);
            AddNotifications(new PastDateValidationContract(BornDate, "Born date", "Burn date is required").Contract.Notifications);

        }

        /// <summary>
        /// Get full name
        /// </summary>
        public string GetFullName()
        {
            return $"{FirstName ?? string.Empty} {LastName ?? string.Empty}";
        }

        #endregion

    }

}
