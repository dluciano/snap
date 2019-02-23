using System.Collections.Generic;
using Autofac;
using Dawlin.Util.Impl;
using GameSharp.DataAccess;
using GameSharp.Services.Abstract;
using GameSharp.Services.Impl;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.AspNetCore;
using NSwag.SwaggerGeneration.Processors.Security;
using Snap.DataAccess;
using Snap.Server.Provider;
using Snap.Services.Impl;

namespace Snap.Server
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
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<SnapDbContext>(options =>
            {
                //options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"),
                //    config => config.MigrationsAssembly(this.GetType().Assembly.FullName));
                options.UseSqlServer(Configuration.GetConnectionString("MSQLDefaultConnection"),
                    config => config.MigrationsAssembly(this.GetType().Assembly.FullName));
            });

            services
                .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = "https://localhost:52365";
                options.RequireHttpsMetadata = false;
                options.Validate();
            });

            services.AddOpenApiDocument(document =>
            {
                document.Title = "Snap Game API";
                document.DocumentProcessors.Add(
                    new SecurityDefinitionAppender("oauth", new SwaggerSecurityScheme
                    {
                        Type = SwaggerSecuritySchemeType.OAuth2,
                        //Description = "Foo",
                        Flow = SwaggerOAuth2Flow.Implicit,
                        AuthorizationUrl = "https://localhost:52365/connect/authorize",
                        TokenUrl = "https://localhost:52365/connect/token",
                        Scopes = new Dictionary<string, string>
                        {
                            { "snapgame ", "Read access to protected resources" },
                        }
                    }));
                document.OperationProcessors.Add(new OperationSecurityScopeProcessor("oauth"));
            });
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. If you
        // need a reference to the container, you need to use the
        // "Without ConfigureContainer" mechanism shown later.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder
                .Register<GameSharpContext>(t => t.Resolve<SnapDbContext>())
                .As<GameSharpContext>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .OnActivated(async args =>
                {
                    var db = args.Instance;
                    //await db.Database.MigrateAsync();
                    //await db.Database.EnsureCreatedAsync();
                });

            builder.RegisterType<ServerPlayerProvider>()
                .As<IPlayerProvider>()
                .InstancePerLifetimeScope();

            builder.RegisterModule<DawlinUtilModule>();
            builder.RegisterModule<GameSharpModule>();
            builder.RegisterModule<SnapGameModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwagger();
            app.UseSwaggerUi3(settings =>
            {
                settings.OAuth2Client = new OAuth2ClientSettings
                {
                    ClientId = "snapGameApiDevSwagger",
                    AdditionalQueryStringParameters =
                    {
                        { "nonce", "636864884858396406.MWI4ZjNkYWItOGU1My00YmFiLTg1MTAtMWQzOTY2OTM4YzRkOGFhOGI1OGItODc0YS00NGEyLWI3NzgtYzU0YmJiMzk5NWY0" }
                    }
                };
            });

            app.UseMvc();
        }
    }
}
