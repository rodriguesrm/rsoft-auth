using RSoft.Framework.Cross.Entities;

namespace RSoft.Framework.Domain.Contracts
{

    /// <summary>
    /// Full name validation contract.
    /// </summary>
    public class FullNameValidationContract : BaseValidationContract
    {

        #region Constructors

        /// <summary>
        /// Create a new instance of object
        /// </summary>
        /// <param name="name">Name object instance</param>
        public FullNameValidationContract(IFullName name) : this(name, new FullNameValidationArgument()
        {
            CharListAllowed = string.Empty,
            FirstNameMinimumLength = 2,
            FirstNameMaximumLength = 50,
            LastNameMinimumLength = 2,
            LastNameMaximumLength = 100
        }) { }

        /// <summary>
        /// Create a new instance of object
        /// </summary>
        /// <param name="name">Name object instance</param>
        /// <param name="args">Full name validation arguments</param>
        public FullNameValidationContract(IFullName name, FullNameValidationArgument args)
        {

            // Regular expression for all characteres name (global)
            // ^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$

            //TODO: Globalization
            if (string.IsNullOrWhiteSpace(name.FirstName))
            {
                Contract.IsNotNullOrEmpty(name.FirstName, "First name", "First name is required");
            }
            else
            {
                Contract
                    .HasMinLen(name.FirstName ?? string.Empty, args.FirstNameMinimumLength, "First name", "First name must contain at least 2 characters")
                    .HasMaxLen(name.FirstName ?? string.Empty, args.FirstNameMaximumLength, "Last name", "First name must contain a maximum of 50 characters")
                    .Matchs(name.FirstName, $"^[a-zA-Z{args.CharListAllowed} ,.'-]+$", "First name", "First name contains invalid characters");
            }

            if (string.IsNullOrWhiteSpace(name.LastName))
            {
                Contract.IsNotNullOrEmpty(name.LastName, "Last name", "Last name is required");
            }
            else
            { 
                Contract
                    .HasMinLen(name.LastName ?? string.Empty, args.LastNameMinimumLength, "Last name", "Last name must contain at least 2 characters")
                    .HasMaxLen(name.LastName ?? string.Empty, args.LastNameMaximumLength, "Last name", "Last name must contain a maximum of 50 characters")
                    .Matchs(name.LastName, $"^[a-zA-Z{args.CharListAllowed} ,.'-]+$", "Last name", "Last name contains invalid characters");

            }

        }

        #endregion

    }
}
