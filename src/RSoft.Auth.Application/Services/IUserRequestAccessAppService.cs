using RSoft.Auth.Cross.Common.Model.Args;
using RSoft.Lib.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Auth.Application.Services
{


    /// <summary>
    /// User request access application service interface
    /// </summary>
    public interface IUserRequestAccessAppService
    {

        /// <summary>
        /// Send e-mail to create/recovery message
        /// </summary>
        /// <param name="args">Request data arguments</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        Task<SimpleOperationResult> SendMail(UserRequestAccessArgs args, CancellationToken cancellationToken = default);

    }

}
