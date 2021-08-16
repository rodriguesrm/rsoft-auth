using RSoft.Lib.DDD.Infra.Data.Tables;
using System;

namespace RSoft.Auth.Infra.Data.Entities
{

    /// <summary>
    /// User Scopes
    /// </summary>
    public class UserScope : TableBase<UserScope>, ITable
    {

        #region Properties

        /// <summary>
        /// User id
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Scope id
        /// </summary>
        public Guid? ScopeId { get; set; }

        #endregion

        #region Navigation/Lazy

        /// <summary>
        /// User data
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Scope data
        /// </summary>
        public virtual Scope Scope { get; set; }

        #endregion

    }

}
