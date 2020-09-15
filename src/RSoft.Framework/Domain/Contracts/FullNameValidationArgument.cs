namespace RSoft.Framework.Domain.Contracts
{

    /// <summary>
    /// Full name validation arguments
    /// </summary>
    public class FullNameValidationArgument
    {

        #region Properties

        /// <summary>
        /// Allowed characters list
        /// </summary>
        public string CharListAllowed { get; set; }

        /// <summary>
        /// First name mininum length
        /// </summary>
        public int FirstNameMinimumLength { get; set; }

        /// <summary>
        /// First name maximum length
        /// </summary>
        public int FirstNameMaximumLength { get; set; }

        /// <summary>
        /// Last name minimum length
        /// </summary>
        public int LastNameMinimumLength { get; set; }

        /// <summary>
        /// Last name maximum length
        /// </summary>
        public int LastNameMaximumLength { get; set; }


        #endregion

    }

}
