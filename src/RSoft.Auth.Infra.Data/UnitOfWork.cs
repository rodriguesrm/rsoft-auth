using frw = RSoft.Lib.DDD.Infra.Data;

namespace RSoft.Auth.Infra.Data
{

    /// <summary>
    /// Unit of work object to maintain the integrity of transactional operations
    /// </summary>
    public class UnitOfWork : frw.UnitOfWorkBase, frw.IUnitOfWork
    {

        #region Constructors

        /// <summary>
        /// Create a new UnitOfWork instance
        /// </summary>
        /// <param name="ctx">Database context object</param>
        public UnitOfWork(AuthContext ctx) : base(ctx) { }

        #endregion

    }

}
