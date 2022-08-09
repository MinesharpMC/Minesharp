using Minesharp;
using Serilog;
using Serilog.Filters;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
    .MinimumLevel.Information()
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