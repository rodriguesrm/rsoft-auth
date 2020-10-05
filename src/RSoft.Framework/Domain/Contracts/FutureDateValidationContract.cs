using System;

namespace RSoft.Framework.Domain.Contracts
{

    /// <summary>
    /// Contract future date validation. Validate date if is in future.
    /// </summary>
    public class FutureDateValidationContract : BaseValidationContract
    {

        #region Constructors

        /// <summary>
        /// Create a new instance of contract
        /// </summary>
        /// <param name="date">Date to validate</param>
        /// <param name="field">Field name</param>
        /// <param name="message">Critical message</param>
        public FutureDateValidationContract(DateTime? date, string field, string message) : base()
        {

            //BACKLOG: Globalization
            Contract
                .IsNotNull(date, field, message)
                .IsGreaterOrEqualsThan(date.Value, DateTime.UtcNow, field, $"The '{field}' must be greater than the current date")
            ;

        }

        #endregion

    }
}
