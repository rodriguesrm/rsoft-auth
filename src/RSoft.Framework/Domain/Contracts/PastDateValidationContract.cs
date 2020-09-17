using System;

namespace RSoft.Framework.Domain.Contracts
{


    /// <summary>
    /// Past date validate contract. Validate if date is in past.
    /// </summary>
    public class PastDateValidationContract : BaseValidationContract
    {

        #region Constructors

        /// <summary>
        /// Create a new instance of contract
        /// </summary>
        /// <param name="date">Date to validate</param>
        /// <param name="field">Field name</param>
        /// <param name="message">Critical message</param>
        public PastDateValidationContract(DateTime? date, string field, string message) : base()
        {

            //TODO: Globalization
            Contract
                .IsNotNull(date, field, message)
                .IsLowerOrEqualsThan((date == null ? DateTime.UtcNow : date.Value), DateTime.UtcNow, field, $"The '{field}' must be less than the current date")
            ;

        }

        #endregion

    }
}
