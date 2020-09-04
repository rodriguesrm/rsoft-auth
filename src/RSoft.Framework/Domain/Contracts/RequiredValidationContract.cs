namespace RSoft.Framework.Domain.Contracts
{

    /// <summary>
    /// Fields required contract validation
    /// </summary>
    /// <typeparam name="TObj">Object type</typeparam>
    public class RequiredValidationContract<TObj> : BaseValidationContract
    {

        #region Constructors

        /// <summary>
        /// Create a new instance of contract
        /// </summary>
        /// <param name="obj">Object to validate</param>
        /// <param name="field">Field name</param>
        /// <param name="message">Critical message</param>
        public RequiredValidationContract(TObj obj, string field, string message) : base()
        {

            //TODO: Globalization
            Contract
                .Requires()
                .IsNotNull(obj, field, message)
            ;


            if (typeof(TObj) == typeof(string))
            {
                string value = obj as string;
                value = value?.Trim();
                Contract
                    .Requires()
                    .IsNotNullOrEmpty(value, field, message);
            }


        }

        #endregion

    }
}
