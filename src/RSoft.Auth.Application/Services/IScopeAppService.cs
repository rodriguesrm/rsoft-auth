using RSoft.Auth.Application.Model;
using RSoft.Auth.Cross.Common.Model.Results;
using RSoft.Lib.Common.Models;
using RSoft.Lib.Design.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Auth.Application.Services
{

    /// <summary>
    /// Scope application service interface contract
    /// </summary>
    public interface IScopeAppService : IAppServiceBase<ScopeDto, Guid>
    {

        /// <summary>
        /// Export scope data (csv format)
        /// </summary>
        /// <param name="scopeId">Scope id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<OperationResult<byte[]>> ExportScope(Guid scopeId, CancellationToken cancellationToken);

        /// <summary>
        /// Import scopes data (csv format)
        /// </summary>
        /// <param name="buffer">Buffer file to import</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<OperationResult<IEnumerable<RowImportResult>>> ImportScope(byte[] buffer, CancellationToken cancellationToken);
    }

}
