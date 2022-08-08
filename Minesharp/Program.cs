using Minesharp;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .CreateLogger();

var app = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(x =>
    {
        x.UseStartup<Startup>();
        x.UseKestrel(s =>
        {
            s.ListenAnyIP(5000);
        });
    })
    .UseSerilog()
    .UseConsoleLifetime()
    .Build();

using (app)
{
    await app.StartAsync();
    await app.WaitForShutdownAsync();
}