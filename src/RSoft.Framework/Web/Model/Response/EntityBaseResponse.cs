using System.Linq;

namespace RSoft.Framework.Web.Model.Response
{


    /// <summary>
    /// Abstract response base object
    /// </summary>
    public abstract class EntityBaseResponse
    {

        #region Métodos Públicos

        /// <summary>
        /// Get object name
        /// </summary>
        public string GetName()
            => GetType().Name.Split(".").ToList().Last();

        #endregion

    }

}
