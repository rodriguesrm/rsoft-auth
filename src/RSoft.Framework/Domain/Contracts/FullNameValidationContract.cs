using RSoft.Framework.Domain.Entities;

namespace RSoft.Framework.Domain.Contracts
{

    /// <summary>
    /// Full name validation contract.
    /// First name and last name are required.
    /// First name must between 2 and 50 characters.
    /// Last name must between 2 and 100 characters
    /// </summary>
    public class FullNameValidationContract : BaseValidationContract
    {

        #region Constructors

        /// <summary>
        /// Create a new instance of object
        /// </summary>
        /// <param name="name">Name object instance</param>
        public FullNameValidationContract(IFullName name) : this(name, string.Empty) { }

        /// <summary>
        /// Create a new instance of object
        /// </summary>
        /// <param name="name">Name object instance</param>
        /// <param name="charListAllowed">Char list of allowed characters</param>
        public FullNameValidationContract(IFullName name, string charListAllowed)
        {

            // Regular expression for all characteres name (global)
            // ^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$

            //TODO: Globalization
            //TODO: Add characters len parameter
            Contract
                .Requires()
                
                .IsNotNullOrEmpty(name.FirstName, "First name", "First name is required")
                .HasMinLen(name.FirstName ?? string.Empty, 2, "First name", "First name must contain at least 2 characters")
                .HasMaxLen(name.FirstName ?? string.Empty, 50, "Last name", "First name must contain a maximum of 50 characters")
                .Matchs(name.FirstName, $"^[a-zA-Z{charListAllowed} ,.'-]+$", "First name", "First name contains invalid characters")
                
                .IsNotNullOrEmpty(name.LastName, "Last name", "Last name is required")
                .HasMinLen(name.LastName ?? string.Empty, 2, "Last name", "Last name must contain at least 2 characters")
                .HasMaxLen(name.LastName ?? string.Empty, 100, "Last name", "Last name must contain a maximum of 50 characters")
                .Matchs(name.FirstName, $"^[a-zA-Z{charListAllowed} ,.'-]+$", "Last name", "Last name contains invalid characters")
            ;

        }

        #endregion

    }
}
