namespace RSoft.Framework.Domain.Contracts
{

    /// <summary>
    /// E-mail validation contract. Validate if e-mail is valid.
    /// </summary>
    public class EmailValidationContract : BaseValidationContract
    {

        #region Constructors

        /// <summary>
        /// Craate a new instance of contract
        /// </summary>
        /// <param name="email">E-mail to validate</param>
        public EmailValidationContract(string email) : this(email, true) { }

        /// <summary>
        /// Craate a new instance of contract
        /// </summary>
        /// <param name="email">E-mail to validate</param>
        /// <param name="required">Indicates whether email is required (default true)</param>
        public EmailValidationContract(string email, bool required) : base()
        {
            //BACKLOG: Globalization
            if (required)
                Contract.Requires();
            Contract
                .IsEmailOrEmpty(email, "Email", "Invalid e-mail");
        }

        #endregion

    }
}
