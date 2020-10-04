using hlp = RSoft.Helpers.Validations.BrasilianDocument;

namespace RSoft.Framework.Domain.Contracts
{

    /// <summary>
    /// Brasilian document cpf validation contract
    /// </summary>
    public class BrasilianCpfValidationContract : BaseValidationContract
    {

        #region Constructors

        /// <summary>
        /// Create a new instance of object
        /// </summary>
        /// <param name="cpf">Cpf number (withou mask)</param>
        /// <param name="fieldName">Field name</param>
        /// <param name="required">Indicates whether the CPF is mandatory</param>
        public BrasilianCpfValidationContract(string cpf, string fieldName, bool required)
        {

            if (required)
                if (string.IsNullOrWhiteSpace(cpf))
                    Contract.AddNotification(fieldName, $"{fieldName} is required");

            if (!string.IsNullOrWhiteSpace(cpf))
                Contract
                    .IsTrue(hlp.CheckDocument(cpf), fieldName, $"{fieldName} invalid");
        }

        #endregion

    }
}
