using System.Text;
using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Services;
using GitHubSimulator.Factories;
using GitHubSimulator.Infrastructure.Authentication;
using GitHubSimulator.Infrastructure.Cache;
using GitHubSimulator.Infrastructure.Configuration;
using GitHubSimulator.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GitHubSimulator.Configuration;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager builderConfiguration)
    {
        services.Configure<DatabaseSettings>(builderConfiguration.GetSection(DatabaseSettings.SectionName));
        services.Configure<RedisSettings>(builderConfiguration.GetSection(RedisSettings.SectionName));
        services.Configure<JwtSettings>(builderConfiguration.GetSection(JwtSettings.SectionName));
        services.AddScoped<IJwtProvider, JwtProvider>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMilestoneRepository, MilestoneRepository>()
                .AddScoped<IIssueRepository, IssueRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IPullRequestRepository, PullRequestRepository>()
                .AddScoped<IRepositoryRepository, RepositoryRepository>()
                .AddScoped<ILabelRepository, LabelRepository>()
                .AddScoped<IBranchRepository, BranchRepository>()
                .AddScoped<ICommentRepository, CommentRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IMilestoneService, MilestoneService>()
            .AddScoped<IIssueService, IssueService>()
            .AddScoped<ICacheService, CacheService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IRepositoryService, RepositoryService>()
            .AddScoped<ICacheService, CacheService>()
            .AddScoped<ILabelService, LabelService>()
            .AddScoped<IBranchService, BranchService>()
            .AddScoped<IPullRequestService, PullRequestService>()
            .AddScoped<ICommentService, CommentService>();

        return services;
    }

    public static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services
            .AddScoped<MilestoneFactory>()
            .AddScoped<IssueFactory>()
            .AddScoped<UserFactory>()
            .AddScoped<PullRequestFactory>()
            .AddScoped<RepositoryFactory>()
            .AddScoped<LabelFactory>()
            .AddScoped<BranchFactory>()
            .AddScoped<CommentFactory>();

        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new()
                {
                    ValidIssuer = config["JwtSettings:Issuer"],
                    ValidAudience = config["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });

        return services;
    }

    //public static IServiceCollection AddLogging(this IServiceCollection services)
    //{
    //    services.AddLogging(builder =>
    //    {
    //        builder.AddConsole();
    //    });

    //    return services;
    //}
}
