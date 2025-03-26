using IceCreamCompany.Application.Core.Interfaces;
using IceCreamCompany.Application.Core.Interfaces.Abstract;
using IceCreamCompany.Application.Core.Services;
using IceCreamCompany.Application.Core.Workflows.MapperProfiles;
using IceCreamCompany.Data;
using IceCreamCompany.Data.Repositories.Abstracts;
using IceCreamCompany.Domain.Constants;
using IceCreamCompany.Domain.Interfaces.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Quartz;
using System.Reflection;

namespace IceCreamCompany.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IceCreamCompanyContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(GlobalConstants.DefaultConnectionStringName));
            });

            return services;
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services.AddScopedServiceTypes(typeof(BaseRepository).Assembly, typeof(IRepository));
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services.AddScopedServiceTypes(typeof(WorkflowService).Assembly, typeof(IService));
        }

        public static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper(typeof(WorkflowProfile));
        }

        public static IServiceCollection AddCustomSwaggerExtension(this IServiceCollection services)
        {
            var xmlDocsPath = Path.Combine(AppContext.BaseDirectory, typeof(Program).Assembly.GetName().Name + ".xml");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ice Cream Company API", Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            return services;
        }

        public static IServiceCollection RegisterQuartzJobs(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey(GlobalConstants.SyncWorkflowsJobKey);

                q.AddJob<Jobs.SyncWorkflowsJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                     .ForJob(jobKey)
                     .WithIdentity(GlobalConstants.SyncWorkflowsJobTriggerKey)
                     .StartNow()
                     .WithSimpleSchedule(x => x
                         .WithIntervalInMinutes(30)
                         .RepeatForever()));
            });

            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

            return services;
        }

        private static IServiceCollection AddScopedServiceTypes(this IServiceCollection services, Assembly assembly, Type fromType)
        {
            var types = assembly.GetTypes()
                .Where(x => !string.IsNullOrEmpty(x.Namespace) && x.IsClass && !x.IsAbstract && fromType.IsAssignableFrom(x))
                .Select(x => new
                {
                    Interface = x.GetInterface($"I{x.Name}"),
                    Implementation = x
                });

            foreach (var type in types)
            {
                services.AddScoped(type.Interface, type.Implementation);
            }

            return services;
        }
    }
}
