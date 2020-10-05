using FluentValidator.Validation;

namespace RSoft.Framework.Domain.Contracts
{

    /// <summary>
    /// Contract base validation
    /// </summary>
    public abstract class BaseValidationContract : IContract
    {

        #region Constructors

        /// <summary>
        /// Create a new instance of contract
        /// </summary>
        public BaseValidationContract()
        {
            Contract = new ValidationContract();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Validation contract
        /// </summary>
        public ValidationContract Contract { get; private set; }

        #endregion

    }
}
