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
    /// Application-Client application service interface contract
    /// </summary>
    public interface IAppClientAppService : IAppServiceBase<AppClientDto, Guid>
    {

        /// <summary>
        /// Export application-client data (csv format)
        /// </summary>
        /// <param name="clientId">Client id key</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<OperationResult<byte[]>> ExportAppClient(Guid clientId, CancellationToken cancellationToken);

        /// <summary>
        /// Import application-client data (csv format)
        /// </summary>
        /// <param name="buffer">Buffer file to import</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<OperationResult<IEnumerable<RowImportResult>>> ImportAppClient(byte[] buffer, CancellationToken cancellationToken);
    }

}
