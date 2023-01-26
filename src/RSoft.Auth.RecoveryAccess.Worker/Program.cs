using RSoft.Logs.Extensions;
using RSoft.Auth.RecoveryAccess.Worker.IoC;

IHost host = 
    Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsoleLogger();
        logging.AddSeqLogger();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddAuthWorkerRegister(hostContext.Configuration);
    })
    .Build();

#region Execution

await host.RunAsync();

#endregion
