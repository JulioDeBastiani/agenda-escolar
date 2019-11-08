using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arq.Data;
using Diary.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Diary.WebApi
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(op => op
                .UseMySql(_config.GetConnectionString("somedb")));

            services.AddScoped<GenericRepository<Appointment>>();
            services.AddScoped<GenericRepository<Assignment>>();
            services.AddScoped<GenericRepository<Attendance>>();
            services.AddScoped<GenericRepository<Class>>();
            services.AddScoped<GenericRepository<Event>>();
            services.AddScoped<GenericRepository<Grade>>();
            services.AddScoped<GenericRepository<SchoolYear>>();
            services.AddScoped<GenericRepository<StudentClass>>();
            services.AddScoped<GenericRepository<Subject>>();
            services.AddScoped<GenericRepository<User>>();
            services.AddScoped<GenericRepository<UserEvent>>();
            services.AddScoped<GenericRepository<UserGuardian>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
