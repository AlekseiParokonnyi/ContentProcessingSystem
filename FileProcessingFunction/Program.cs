using FileProcessingFunction.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
      services.AddScoped<IFileProcessingService, FileProcessingService>();
    })
    .Build();

host.Run();