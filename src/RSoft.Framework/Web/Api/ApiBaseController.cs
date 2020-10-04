using FluentValidator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RSoft.Framework.Web.Model.Response;
using RSoft.Logs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Framework.Web.Api
{

    /// <summary>
    /// API Controller base
    /// </summary>
    [Produces("application/json")]
    [Authorize]
    public abstract class ApiBaseController : ControllerBase, IDisposable
    {

        #region Local objects/variables

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new controller instance
        /// </summary>
        public ApiBaseController() { }

        #endregion

        #region Local Properties

        /// <summary>
        /// Application id passed in the request header
        protected Guid? AppKey
        {
            get
            {
                var headerInfo = Request?.Headers["app-key"];
                if (!Guid.TryParse(headerInfo, out Guid result))
                    return null;
                return result;
            }
        }

        /// <summary>
        /// Application access key passed in the request header
        /// </summary>
        protected Guid? AppAccess
        {
            get
            {
                var headerInfo = Request?.Headers["app-access"];
                if (!Guid.TryParse(headerInfo, out Guid result))
                    return null;
                return result;
            }
        }

        /// <summary>
        /// Convert a dictionary into a GenericNotificationResponse list
        /// </summary>
        /// <param name="dictionary">Notifications dictionary(string, string)</param>
        protected IEnumerable<GenericNotificationResponse> PrepareNotifications(IDictionary<string, string> dictionary)
            => dictionary.Select(itm => new GenericNotificationResponse(itm.Key, itm.Value));

        #endregion

        #region Local Methods

        /// <summary>
        /// Handles exception
        /// </summary>
        /// <param name="code">Response http code</param>
        /// <param name="ex">Exception object</param>
        protected IActionResult HandleException(int code, System.Exception ex)
        {
            //TODO: Catch MySql exceptions to display friendly message
            string msg = ex.GetBaseException().Message;
            string traceId = Request.HttpContext.TraceIdentifier;
            return StatusCode(code, new GerericExceptionResponse(code.ToString(), msg, traceId));
        }

        /// <summary>
        /// Performs the base operation and calls the callback function
        /// </summary>
        /// <param name="callback">Callback function operation</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> RunActionAsync(Task<IActionResult> callback, CancellationToken cancellationToken = default)
            => await RunActionAsync(callback, false, false, cancellationToken);

        /// <summary>
        /// Performs the base operation and calls the callback function
        /// </summary>
        /// <param name="callback">Callback function operation</param>
        /// /// <param name="validateRequest">Indicates whether a request validation should be performed</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> RunActionAsync(Task<IActionResult> callback, bool validateRequest, CancellationToken cancellationToken = default)
            => await RunActionAsync(callback, validateRequest, false, cancellationToken);

        /// <summary>
        /// Performs the base operation and calls the callback function
        /// </summary>
        /// <param name="callback">Callback function operation</param>
        /// <param name="validateRequest">Indicates whether a request validation should be performed</param>
        /// <param name="catchException">Indicates whether to catch exceptions</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        protected async Task<IActionResult> RunActionAsync(Task<IActionResult> callback, bool validateRequest, bool catchException, CancellationToken cancellationToken = default)
        {

            try
            {

                if (validateRequest)
                {
                    if (!ModelState.IsValid)
                    {
                        IEnumerable<GenericNotificationResponse> msg = GetModelErrors(ModelState);
                        return BadRequest(msg);
                    }
                }

                return await Task.Run(() =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return callback;
                }, cancellationToken);

            }
            catch (System.Exception ex)
            {

                if (catchException)
                    return HandleException(StatusCodes.Status500InternalServerError, ex);
                else
                    throw;

            }

        }

        /// <summary>
        /// Obtain requisition validation messages from model-request validation
        /// </summary>
        /// <param name="modelState">Dicionário de model-state</param>
        protected IEnumerable<GenericNotificationResponse> GetModelErrors(ModelStateDictionary modelState)
        {

            IList<GenericNotificationResponse> result = new List<GenericNotificationResponse>();

            foreach (var item in modelState)
            {

                string prop = string.IsNullOrWhiteSpace(item.Key) ? "body" : item.Key;
                string errors = string.Join(" | ", item.Value.Errors.ToList().Select(x => x.ErrorMessage).ToList()).Trim();
                if (!string.IsNullOrWhiteSpace(errors))
                    result.Add(new GenericNotificationResponse(prop, errors));
            }

            return result;

        }

        /// <summary>
        /// Obtain requisition validation messages from entity-notifications
        /// </summary>
        /// <param name="notifications">Notifications list generated by the domain layer</param>
        protected IEnumerable<GenericNotificationResponse> GetNotificationsErrors(IReadOnlyCollection<Notification> notifications)
        {

            IList<GenericNotificationResponse> result = new List<GenericNotificationResponse>();

            foreach (Notification n in notifications)
            {
                if (!string.IsNullOrWhiteSpace(n.Message))
                    result.Add(new GenericNotificationResponse(n.Property, n.Message));
            }

            return result;

        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false;

        /// <summary>
        /// Release resources
        /// </summary>
        /// <param name="disposing">Flag indicating whether to release a resource</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {

                if (disposing)
                {
                    //object?.Dispose();
                }

                //object = null;

                disposedValue = true;
            }
        }

        /// <summary>
        /// Destroys the controller by freeing up resources
        /// </summary>
        ~ApiBaseController()
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
