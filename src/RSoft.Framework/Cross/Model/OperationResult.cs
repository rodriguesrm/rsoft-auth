namespace RSoft.Framework.Cross.Model
{

    /// <summary>
    /// Base result model
    /// </summary>
    /// <typeparam name="TObject">Type object result data</typeparam>
    public class OperationResult<TObject>
        where TObject : class
    {

        /// <summary>
        /// Indicates whether the operation was performed successfully
        /// </summary>
        public bool Sucess { get; set; }

        /// <summary>
        /// Message produced with the result of the operation
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Data obtained from the result of the operation
        /// </summary>
        public TObject Result { get; set; }

    }

}
