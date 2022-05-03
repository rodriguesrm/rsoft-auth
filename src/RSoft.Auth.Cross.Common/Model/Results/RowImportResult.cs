using System;

namespace RSoft.Auth.Cross.Common.Model.Results
{

    /// <summary>
    /// Row import result model
    /// </summary>
    public class RowImportResult
    {

        #region Constructors

        /// <summary>
        /// Create a new object
        /// </summary>
        /// <param name="line">Line number</param>
        /// <param name="id">Record id</param>
        /// <param name="importSuccess">Import success flag</param>
        /// <param name="errorMessage">Error message when import fail</param>
        public RowImportResult(int line, Guid? id, bool importSuccess, string errorMessage)
        {
            Line = line;
            Id = id;
            ImportSuccess = importSuccess;
            ErrorMessage = errorMessage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Line number
        /// </summary>
        public int Line { get; private set; }

        /// <summary>
        /// Record id
        /// </summary>
        public Guid? Id { get; private set; }

        /// <summary>
        /// Import success flag
        /// </summary>
        public bool ImportSuccess { get; private set; }

        /// <summary>
        /// Error message when import fail
        /// </summary>
        public string ErrorMessage { get; private set; }

        #endregion

    }
}
