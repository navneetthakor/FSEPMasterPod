

using Hangfire;

namespace wps_master_pod_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            Startup startup = new Startup(builder.Configuration);

            startup.ConfigureServices(builder.Services);

            WebApplication app = builder.Build();
            IRecurringJobManager recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();
            startup.Configure(app, builder.Environment, recurringJobManager);
            //startup.Configure(app, builder.Environment);
            //startup.Configure(app, builder.Environment);


            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}

// accept request from api gateway
// accept heartbeats from worker pods
// communicate with the database
// create worker pods (if they fails) (recurring job)