namespace RSoft.Framework.Domain.Contracts
{
    public class EmailValidationContract : BaseValidationContract
    {

        #region Constructors

        /// <summary>
        /// Craate a new instance of contract
        /// </summary>
        /// <param name="email">E-mail to validate</param>
        public EmailValidationContract(string email) : base()
        {

            //TODO: Globalization
            Contract
                .Requires()
                .IsEmailOrEmpty(email, "Email", "Invalid e-mail");
            ;

        }

        #endregion

    }
}
