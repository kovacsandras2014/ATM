using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ATM.Controllers;
using ATM.Model.DbModel;
using ATM.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ATM
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AtmDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("AtmDb")));
            services.AddScoped<ISerializer, DbSerializer>();
            services.AddScoped<IAtm, AtmMachine>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ATM", Version = "v1.0" });
            });

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<AtmController>>();
            services.AddSingleton(typeof(ILogger), logger);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UpdateDatabase(app, env);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ATM v1.0"));
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        static void UpdateDatabase(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment()) return;

            using IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using AtmDbContext context = serviceScope.ServiceProvider.GetService<AtmDbContext>();
            context?.Database.Migrate();
        }



    }
}
