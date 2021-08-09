using Service.Interfaces;
using Database;
using Database.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model.Enum;
using Service;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Reflection;
using WebApi.Filters;
using Database.Repositories;

namespace WebApi
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
            services.AddDbContext<DatabaseContext>(item => item
            .EnableSensitiveDataLogging(), ServiceLifetime.Transient
            );
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddScoped<IExchangeService, ExchangeService>();

            services.AddScoped<IExchangeTransactionService, ExchangeTransactionService>();
            services.AddScoped<IExchangeTransactionRepository, ExchangeTransactionRepository>();

            services.AddTransient<ExchangeRateSourceUSD>();
            services.AddTransient<ExchangeRateSourceBRL>();
            services.AddTransient<ExchangeRateSourceNotImpl>();

            services.AddTransient(serviceProvider =>
            {
                Func<CurrencyCodeEnum, IExchangeRateSource> func = key =>
                                {
                                    switch (key)
                                    {
                                        case CurrencyCodeEnum.BRL:
                                            return serviceProvider.GetService<ExchangeRateSourceBRL>();
                                        case CurrencyCodeEnum.USD:
                                            return serviceProvider.GetService<ExchangeRateSourceUSD>();
                                        default:
                                            return serviceProvider.GetService<ExchangeRateSourceNotImpl>();
                                    }
                                };
                return func;
            });


            services.AddControllers(options =>
                options.Filters.Add(new HttpResponseExceptionFilter()));

            // Register the Swagger generator
            services.AddSwaggerGen(c =>
            {
                // Use method name as operationId
                c.CustomOperationIds(e => e.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
