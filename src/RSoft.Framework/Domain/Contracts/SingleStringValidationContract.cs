namespace RSoft.Framework.Domain.Contracts
{
    public class SingleStringValidationContract : BaseValidationContract
    {

        #region Constructors

        /// <summary>
        /// Create a new instance of object
        /// </summary>
        /// <param name="expression">Expression to validate</param>
        /// <param name="fieldName">Field name</param>
        /// <param name="required">Indicate if field is required</param>
        public SingleStringValidationContract(string expression, string fieldName, bool required) : this(expression, fieldName, required, null, null) { }

        /// <summary>
        /// Create a new instance of object
        /// </summary>
        /// <param name="expression">Expression to validate</param>
        /// <param name="fieldName">Field name</param>
        /// <param name="required">Indicate if field is required</param>
        /// <param name="minLen">Indicate a mininum length expression</param>
        /// <param name="maxLen">Indicate a maximum length expression</param>
        public SingleStringValidationContract(string expression, string fieldName, bool required, int? minLen, int? maxLen)
        {

            if (expression != null)
                expression = expression.Trim();

            //TODO: Globalization
            if (required)
                Contract
                    .Requires()
                    .IsNotNullOrEmpty(expression, fieldName, $"The {fieldName} field is required");

            if (expression != null)
            {

                if (minLen.HasValue && minLen.Value > 0)
                    Contract
                        .HasMinLen(expression, minLen.Value, fieldName, $"The {fieldName} field must contain at least {minLen.Value} character(s)");

                if (maxLen.HasValue && maxLen.Value > 0 && maxLen.Value >= (minLen.HasValue ? minLen.Value : 0))
                    Contract
                        .HasMaxLen(expression, maxLen.Value, fieldName, $"The {fieldName} fields must contain a maximum {maxLen.Value} character(s)");

            }

        }

        #endregion

    }
}
