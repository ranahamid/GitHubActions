using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using WebApplication5.Configurations;
using WebApplication5.Data;
using WebApplication5.IRepository;
using WebApplication5.Repository;
using WebApplication5.Services;

namespace WebApplication5
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
            services.AddDbContext<DatabaseContext>(x => x.UseSqlServer(Configuration.GetConnectionString("sqlConnection")));
            //services.AddResponseCaching();
            //services.AddHttpCacheHeaders();

            services.AddMemoryCache();
            services.AddInMemoryRateLimiting();

            services.ConfigureRateLimiting();
            services.AddHttpContextAccessor();

            services.ConfigureHttpCacheHeaders();
            services.AddAuthentication();
            services.ConfigureIdentity();
     
            services.ConfigureJwt(configuration:Configuration);
            services.AddCors(x =>
            {
                x.AddPolicy("AllowAll", 
                    builder => builder.AllowAnyOrigin().
                        AllowAnyMethod().
                        AllowAnyHeader());
            });
            services.AddAutoMapper(typeof(MapperInitilizer));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthManager, AuthManager>();
            services.ConfigureSwaggerDoc();

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web Application", Version = "v1" });
            //});
            services.AddControllers(
                //x =>
                //        {
                //            x.CacheProfiles.Add("120SecondsCacheDuration", new CacheProfile
                //            {
                //                Duration = 120,
                //                Location = ResponseCacheLocation.Any,
                //            });
                //        }
                ).AddNewtonsoftJson(x=>x.SerializerSettings.ReferenceLoopHandling= Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.ConfigureVersion();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            app.UseSwagger();
            //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web Application v1"));
            app.UseSwaggerUI(c =>
            {
                var swaggerBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerBasePath}/swagger/v1/swagger.json", "Hotel Listing API");
            });
            app.ConfigureException();
     
            app.UseHttpsRedirection();
            // UseCors must be called before UseResponseCaching
            app.UseCors("AllowAll");

            app.UseResponseCaching();
            app.UseHttpCacheHeaders();
           
            app.UseIpRateLimiting();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name:"default",
                //    pattern:"{controller=Home}/{Action=Index}/{id?}"
                //    );
                endpoints.MapControllers();
            });
        }
    }
}
