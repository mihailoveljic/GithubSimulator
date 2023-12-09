﻿using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Services;
using GitHubSimulator.Factories;
using GitHubSimulator.Infrastructure.Cache;
using GitHubSimulator.Infrastructure.Configuration;
using GitHubSimulator.Infrastructure.Repositories;

namespace GitHubSimulator.Configuration;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager builderConfiguration)
    {
        services.Configure<DatabaseSettings>(builderConfiguration.GetSection(DatabaseSettings.SectionName));
        services.Configure<RedisSettings>(builderConfiguration.GetSection(RedisSettings.SectionName));

        return services;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMilestoneRepository, MilestoneRepository>()
                .AddScoped<IIssueRepository, IssueRepository>()
                .AddScoped<ILabelRepository, LabelRepository>()
                .AddScoped<IPullRequestRepository, PullRequestRepository>();

        return services;
    }
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IMilestoneService, MilestoneService>()
            .AddScoped<IIssueService, IssueService>()
            .AddScoped<ICacheService, CacheService>()
            .AddScoped<ILabelService, LabelService>()
            .AddScoped<IPullRequestService, PullRequestService>();

        return services;
    }
    public static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services
            .AddScoped<MilestoneFactory>()
            .AddScoped<IssueFactory>()
            .AddScoped<LabelFactory>()
            .AddScoped<PullRequestFactory>();

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
