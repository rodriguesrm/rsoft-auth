using MassTransit;
using RSoft.Auth.Application.Services;
using RSoft.Auth.Cross.Common.Model.Args;
using RSoft.Lib.Common.Abstractions;
using RSoft.Lib.Common.Models;
using RSoft.Lib.Contracts.Events;
using RSoft.Lib.Messaging.Contracts;
using System.Text.Json;
using RSoft.Lib.Messaging.Extensions;

namespace RSoft.Auth.RecoveryAccess.Worker.Consumers
{
    public class UserRequestAccessEventConsumer : IConsumerEvent<UserRequestAccessEvent>
    {

        #region Local objects/variables

        private readonly ILogger<UserRequestAccessEventConsumer>? _logger;
        private readonly IUserRequestAccessAppService? _userRequestAccessAppService;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new consumer event instance
        /// </summary>
        public UserRequestAccessEventConsumer()
        {
            _logger = ServiceActivator.GetScope().ServiceProvider.GetService<ILogger<UserRequestAccessEventConsumer>>();
            _userRequestAccessAppService = ServiceActivator.GetScope().ServiceProvider.GetService<IUserRequestAccessAppService>();
        }

        #endregion

        #region Consumer methods

        /// <summary>
        /// Consume UserCreatedEvent handler
        /// </summary>
        /// <param name="context">Consumer context</param>
        public async Task Consume(ConsumeContext<UserRequestAccessEvent> context)
        {

            _logger?.LogInformation(message: $"Process {nameof(UserRequestAccessEvent)} START", context.MessageId, JsonSerializer.Serialize(context.Message));

            if (_userRequestAccessAppService == null)
                throw new InvalidProgramException($"Cannot get {nameof(IUserRequestAccessAppService)} service instante from container");

            UserRequestAccessArgs args = new
            (
                context.Message.FirstAccess,
                context.Message.Name,
                context.Message.Email,
                context.Message.Token,
                context.Message.ExpireOn,
                context.Message.UrlCredential,
                context.MessageId
            );

            SimpleOperationResult result = await _userRequestAccessAppService.SendMail(args);
            if (!result.Success)
            {
                _logger?.LogInformation($"Process {nameof(UserRequestAccessEvent)} FAIL", context.MessageId);
                throw new Exception(result.ErrorsMessage);
            }

            _logger?.LogInformation($"Process {nameof(UserRequestAccessEvent)} END", context.MessageId);

        }

        #endregion

    }
}
