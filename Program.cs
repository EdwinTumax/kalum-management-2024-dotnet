using System.Text;
using KalumManagement;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration()  
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("./Logs/kalumManagement.out",Serilog.Events.LogEventLevel.Information,"{Message:lj}{NewLine}",encoding: Encoding.UTF8)
    .CreateLogger();


var startUp = new Startup(builder.Configuration);

startUp.ConfigureServices(builder.Services);

var app = builder.Build();

startUp.Configure(app,app.Environment);


app.Run();
