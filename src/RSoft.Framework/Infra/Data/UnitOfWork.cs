using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Framework.Infra.Data
{

    /// <summary>
    /// Unit of work object to maintain the integrity of transactional operations
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {

        #region Local objects/variables

        private DbContext _ctx;
        private IDbContextTransaction _transaction;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new UnitOfWork instance
        /// </summary>
        /// <param name="ctx">Database context object</param>
        public UnitOfWork(DbContext ctx)
        {
            _ctx = ctx;
            TransactionStarted = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Flag to indicate whether the transaction was started
        /// </summary>
        public bool TransactionStarted { get; private set; }

        #endregion

        #region Public methods

        ///<inheritdoc/>
        public int SaveChanges()
            => _ctx.SaveChanges();

        ///<inheritdoc/>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _ctx.SaveChangesAsync(cancellationToken);
        }

        ///<inheritdoc/>
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _ctx.Database.BeginTransactionAsync(cancellationToken)
                    .ContinueWith((tsk) =>
                    {
                        _transaction = tsk.Result;
                        TransactionStarted = true;
                    });
            }
        }

        ///<inheritdoc/>
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _transaction.CommitAsync(cancellationToken)
                .ContinueWith((tsk) =>
                {
                    TransactionStarted = false;
                });

        }

        ///<inheritdoc/>
        public async Task RollBackAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.RollbackAsync(cancellationToken)
                .ContinueWith((tsk) =>
                {
                    TransactionStarted = false;
                });
        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false;

        /// <summary>
        /// Release resources
        /// </summary>
        /// <param name="disposing">Flag to indicate release interna resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _ctx?.Dispose();
                    _transaction?.Dispose();
                }

                _ctx = null;
                _transaction = null;

                disposedValue = true;
            }
        }


        /// <summary>
        /// Destroy object instance and release resources
        /// </summary>
        ~UnitOfWork()
        {
            Dispose(false);
        }

        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }

}
