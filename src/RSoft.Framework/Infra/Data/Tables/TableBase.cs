using System.Linq;

namespace RSoft.Framework.Infra.Data.Tables
{

    /// <summary>
    /// Entity table abstract class
    /// </summary>
    public class TableBase<TTable> : ITable
        where TTable : TableBase<TTable>
    {

        #region Public methods

        /// <summary>
        /// Get object entity name
        /// </summary>
        public virtual string GetName()
        {
            return GetType().Name.Split(".").ToList().Last();
        }

        #endregion

    }

}
