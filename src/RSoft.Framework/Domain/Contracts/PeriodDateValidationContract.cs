using System;

namespace RSoft.Framework.Domain.Contracts
{
    public class PeriodDateValidationContract : BaseValidationContract
    {

        #region Constructors

        /// <summary>
        /// Create a new instance of contract
        /// </summary>
        /// <param name="startDate">Start date to validate</param>
        /// <param name="endDate">End date to validate</param>
        /// <param name="field">Field name</param>
        public PeriodDateValidationContract(DateTime? startDate, DateTime? endDate, string field) : base()
        {

            //TODO: Globalization
            Contract
                .IsNotNull(startDate, field, $"[{field}] Invalid start date")
                .IsNotNull(endDate, field, $"[{field}] Invalid end date")
                .IsGreaterOrEqualsThan(endDate.Value, startDate.Value, field, $"[{field}] The end date must be greater than the start date")
            ;

        }

        #endregion

    }
}
