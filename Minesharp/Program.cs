using Minesharp;
using Serilog;
using Serilog.Filters;

var configuration = new ConfigurationBuilder()
    .AddYamlFile("config.yaml")
    .Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
    .MinimumLevel.Information()
    .CreateLogger();

var app = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(x =>
    {
        x.UseStartup<Startup>();
        x.UseConfiguration(configuration);
        x.UseKestrel(s => { s.ListenAnyIP(5000); });
    })
    .UseSerilog()
    .UseConsoleLifetime()
    .Build();

using (app)
{
    await app.StartAsync();
    await app.WaitForShutdownAsync();
}