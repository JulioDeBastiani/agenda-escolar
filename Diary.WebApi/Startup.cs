using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Diary.Data;
using Diary.Domain;
using Diary.WebApi.Security;
using Diary.WebApi.Services;
using Hangfire;
using Hangfire.MySql.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

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

            services.AddScoped<TokenService>();
            services.AddScoped<NotificationService>();
            services.Configure<EmailSettings>(_config.GetSection("EmailSettings"));
            services.AddScoped<EmailService>();
            
            var tokenSettings = new TokenSettings();
            var tokenConfigurator = new ConfigureFromConfigurationOptions<TokenSettings>(_config.GetSection("TokenSettings"));
            tokenConfigurator.Configure(tokenSettings);
            services.AddSingleton<TokenSettings>(tokenSettings);

            var key = Convert.FromBase64String(tokenSettings.SecretKey);

            services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = tokenSettings.Issuer,
                        ValidateIssuer = true,
                        ValidAudience = tokenSettings.Audience,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            services.AddAuthorization(o =>
                {
                    o.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim(DefaultClaims.UserId)
                        .Build();
                });

            services.AddCors(o => o.AddPolicy("AnyOrigin", b =>
            {
                b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Diary-Backend", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please enter into field the word 'Bearer' following by space and JWT", 
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", Enumerable.Empty<string>() },
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(new MySqlStorage(_config.GetConnectionString("hangfiredb"))));

            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/v1/swagger.json", "Soapstone Backend v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthentication();
            app.UseCors("AnyOrigin");
            app.UseMvc();

            app.UseHangfireDashboard();
        }
    }
}
