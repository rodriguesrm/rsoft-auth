namespace RSoft.Framework.Domain.Contracts
{

    /// <summary>
    /// String validaction contract
    /// </summary>
    public class SimpleStringValidationContract : BaseValidationContract
    {

        #region Constructors

        /// <summary>
        /// Create a new instance of object
        /// </summary>
        /// <param name="expression">Expression to validate</param>
        /// <param name="fieldName">Field name</param>
        /// <param name="required">Indicate if field is required</param>
        public SimpleStringValidationContract(string expression, string fieldName, bool required) : this(expression, fieldName, required, null, null, null) { }

        /// <summary>
        /// Create a new instance of object
        /// </summary>
        /// <param name="expression">Expression to validate</param>
        /// <param name="fieldName">Field name</param>
        /// <param name="required">Indicate if field is required</param>
        /// <param name="pattern">Acceptance pattern regular expression</param>
        public SimpleStringValidationContract(string expression, string fieldName, bool required, string pattern) : this(expression, fieldName, required, pattern, null, null) { }

        /// <summary>
        /// Create a new instance of object
        /// </summary>
        /// <param name="expression">Expression to validate</param>
        /// <param name="fieldName">Field name</param>
        /// <param name="required">Indicate if field is required</param>
        /// <param name="minLen">Indicate a mininum length expression</param>
        /// <param name="maxLen">Indicate a maximum length expression</param>
        public SimpleStringValidationContract(string expression, string fieldName, bool required, int? minLen, int? maxLen) : this(expression, fieldName, required, null, minLen, maxLen) { }

        /// <summary>
        /// Create a new instance of object
        /// </summary>
        /// <param name="expression">Expression to validate</param>
        /// <param name="fieldName">Field name</param>
        /// <param name="required">Indicate if field is required</param>
        /// <param name="pattern">Acceptance pattern regular expression</param>
        /// <param name="minLen">Indicate a mininum length expression</param>
        /// <param name="maxLen">Indicate a maximum length expression</param>
        public SimpleStringValidationContract(string expression, string fieldName, bool required, string pattern, int? minLen, int? maxLen)
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

                if (!string.IsNullOrWhiteSpace(pattern))
                    Contract.Matchs(expression, pattern, fieldName, $"{fieldName} is invalid");

                if ((minLen ?? 0) > 0 && (maxLen ?? 0) > 0 && minLen.Value == maxLen.Value)
                {
                    Contract.HasLen(expression, minLen.Value, fieldName, $"The {fieldName} must have {minLen.Value} character(es)");
                }
                else
                {

                    if (minLen.HasValue && minLen.Value > 0)
                        Contract
                            .HasMinLen(expression, minLen.Value, fieldName, $"The {fieldName} field must contain at least {minLen.Value} character(s)");

                    if (maxLen.HasValue && maxLen.Value > 0 && maxLen.Value >= (minLen ?? 0))
                        Contract
                            .HasMaxLen(expression, maxLen.Value, fieldName, $"The {fieldName} fields must contain a maximum {maxLen.Value} character(s)");

                }

            }

        }

        #endregion

    }
}
