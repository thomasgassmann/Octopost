namespace Octopost.WebApi
{
    using AutoMapper;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Octopost.DataAccess.Context;
    using Octopost.Model.Settings;
    using Octopost.Services;
    using Octopost.Services.ApiResult;
    using Octopost.Services.Assembly;
    using Octopost.Services.BusinessRules.Interfaces;
    using Octopost.Services.BusinessRules.Registry;
    using Octopost.Services.BusinessRules.Registry.Interfaces;
    using Octopost.Services.Comments;
    using Octopost.Services.Common;
    using Octopost.Services.Files;
    using Octopost.Services.Location;
    using Octopost.Services.Posts;
    using Octopost.Services.Tagging;
    using Octopost.Services.UoW;
    using Octopost.Services.Validation;
    using Octopost.Services.Votes;
    using Octopost.Validation.Dto.Newsletter;
    using Octopost.WebApi.Infrastructure;
    using Octopost.WebApi.Infrastructure.Filters;
    using Octopost.WebApi.Infrastructure.Middleware;
    using Octopost.WebApi.Sockets;
    using Octopost.WebApi.Sockets.Posts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this.Configuration = configuration;
            this.HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
            services.AddOptions();
            services.Configure<OctopostSettings>(this.Configuration.GetSection("OctopostSettings"));
            services.AddSingleton<IConfiguration>(this.Configuration);
            services.AddSingleton<OctopostSettings>(x => x.GetService<IOptions<OctopostSettings>>().Value);
            var settings = services.BuildServiceProvider().GetService<OctopostSettings>();
            this.ConfigureAutomapper();
            services.AddCors();
            var mvc = services.AddMvc(config =>
            {
                config.Filters.Add(typeof(ValidateActionFilter));
            });
            mvc.AddFluentValidation(fv =>
            {
                fv.ValidatorFactoryType = typeof(ValidationService);
                foreach (var assembly in AssemblyUtilities.GetOctopostAssemblies())
                {
                    fv.RegisterValidatorsFromAssembly(assembly);
                }
            });
            services.AddTransient<IValidatorFactory, ValidationService>();
            mvc.AddMvcOptions(o => o.Filters.Add(typeof(GlobalExceptionFilter)));
            services.AddSwaggerGen(x => x.OperationFilter<SwaggerFilter>());
            services.AddWebSocketManager();
            var connectionString = this.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<OctopostDbContext>(options =>
            {
                options.UseMySql(connectionString);
            });

            services.AddSingleton<IUnitOfWorkFactory>(x =>
            {
                var businessRuleRegistry = x.GetService<IBusinessRuleRegistry>();
                var result = new UnitOfWorkFactory(connectionString, businessRuleRegistry);
                return result;
            });
            this.ConfigureBusinessRules(services);
            services.AddScoped<IPostCreationService, PostCreationService>();
            services.AddSingleton<IPostTaggingService, PostTaggingService>();
            services.AddScoped<IPostFilterService, PostFilterService>();
            services.AddScoped<IVoteCountService, VoteCountService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<ILocationNameService, LocationNameService>();
            services.AddScoped<ICommentCreationService, CommentCreationService>();
            services.AddScoped<ICommentFilterService, CommentFilterService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IQueryService, QueryService>();
            services.AddSingleton<IPagingValidator, PagingValidator>();
            services.AddSingleton<IBusinessRuleRegistry, BaseBusinessRuleRegistry>();
            services.AddSingleton<IAssemblyContainer>(x => new AssemblyContainer(this.GetAssemblies()));
            services.AddSingleton<IApiResultService, ApiResultService>();
            ServiceLocator.SetServiceLocator(() => services.BuildServiceProvider());

            var context = services.BuildServiceProvider().GetService<OctopostDbContext>();
            context.Database.EnsureCreated();

            // This instance needs to be created for the compiler to reference the Octopost.Validation assembly
            var instance = new PostDtoValidator();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.Map("/octopost", x => this.ConfigureCore(x, env, serviceProvider));
        }

        private void ConfigureCore(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseMiddleware<OptionsMiddleware>();
            app.UseMiddleware<CorsPolicy>();
            app.UseMvc();
            app.UseMiddleware<NotFoundMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUi();
            }
        }

        private void ConfigureBusinessRules(IServiceCollection services)
        {
            var octopostAssemblies = this.GetAssemblies();
            foreach (var octopostAssembly in octopostAssemblies)
            {
                var types = octopostAssembly.GetTypes();
                var businessRules = types.Where(x => x.GetInterfaces().Contains(typeof(IBusinessRuleBase)) && !x.IsInterface);
                foreach (var businessRule in businessRules)
                {
                    if (!businessRule.IsAbstract && !businessRule.IsGenericType)
                    {
                        services.AddTransient(businessRule);
                    }
                }
            }
        }

        private void ConfigureAutomapper() =>
            Mapper.Initialize(cfg => cfg.AddProfiles(this.GetAssemblies().ToList()));

        private IEnumerable<Assembly> GetAssemblies() =>
            AssemblyUtilities.GetOctopostAssemblies();
    }
}
