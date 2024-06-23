using System.Diagnostics.Eventing.Reader;
using KalumManagement.DBContext;
using KalumManagement.Services;
using KalumManagement.Utils;
using Microsoft.EntityFrameworkCore;

namespace KalumManagement
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration _Configuration)
        {
            
            this.Configuration = _Configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IKalumQueueService,KalumQueueService>();
            services.AddTransient<IUtilities,Utilities>();
            services.AddControllers();
            services.AddDbContext<KalumDBContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("KalumConnection")));
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore);
            services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}