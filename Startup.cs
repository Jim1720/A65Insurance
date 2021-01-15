using A65Insurance.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A65Insurance
{
    public class Startup
    {

        string a60Origin = "";
        string a70Origin = "";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var noneFound = "";

            a60Origin = Configuration.GetValue<string>("A60Origin", noneFound);
            a70Origin = Configuration.GetValue<string>("A70Origin", noneFound);

            string[] origions = { a60Origin, a70Origin }; 

            services.AddControllers();


            var connectionString = Configuration.GetValue<string>("Connect", noneFound);

            services.AddDbContext<A45InsuranceContext>(options =>
                options.UseSqlServer(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(policy =>
             policy.WithOrigins(a60Origin, a70Origin)
             .AllowAnyMethod()
             .WithHeaders(HeaderNames.ContentType));

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
