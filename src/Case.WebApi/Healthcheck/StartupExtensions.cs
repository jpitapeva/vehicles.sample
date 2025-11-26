using Case.Model;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Case.WebApi.Healthcheck
{
    /// <summary>
    /// Provides extension methods to register and enable health checks for the application.
    /// This class contains methods to configure the health check endpoint and to automatically
    /// register health checks based on configuration settings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        /// <summary>
        /// Registers the health check middleware on the application pipeline.
        /// Call this method from <c>Startup.Configure</c> to expose the /health endpoint.
        /// The endpoint uses a UI-friendly response writer and maps degraded checks to status code 200.
        /// </summary>
        /// <param name="applicationBuilder">The application builder used to configure middleware.</param>
        public static void UseHealthCheck(this IApplicationBuilder applicationBuilder)
        {
            var options = new HealthCheckOptions
            {
                ResultStatusCodes =
                {
                    [HealthStatus.Degraded] = 200
                },
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            };

            applicationBuilder.UseHealthChecks("/health", options);
        }

        /// <summary>
        /// Adds health checks to the service collection and registers them automatically based on configuration.
        /// Call this method from <c>Startup.ConfigureServices</c> after configuration (settings) have been bound.
        /// </summary>
        /// <param name="services">The service collection to which health checks will be added.</param>
        /// <param name="configuration">The application configuration used to resolve health check settings.</param>
        /// <returns>The original <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection ImplementHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddHealthCheckAutomatic(configuration);

            return services;
        }

        /// <summary>
        /// Reads health check configuration and registers the appropriate health checks.
        /// This method delegates registration to specialized private helpers for each supported type.
        /// </summary>
        /// <param name="healthChecksBuilder">Builder used to register health checks.</param>
        /// <param name="configuration">Configuration instance used to bind <c>Settings</c>.</param>
        private static void AddHealthCheckAutomatic(this IHealthChecksBuilder healthChecksBuilder, IConfiguration configuration)
        {
            var settings = configuration.Get<Settings>();

            ExternalServices(healthChecksBuilder, settings);

            ConnectionStringsSqlServer(healthChecksBuilder, settings);

            ConnectionStringsMongoDb(healthChecksBuilder, settings);

            ConnectionStringsAzureBlob(healthChecksBuilder, settings);

            ConnectionStringsRedis(healthChecksBuilder, settings);

            ServiceBus(healthChecksBuilder, settings);

            EventHub(healthChecksBuilder, settings);

            AuthorizationServer(healthChecksBuilder, settings);
        }

        /// <summary>
        /// Registers URL-based health checks for configured external services.
        /// For each enabled external service in settings, an URL group health check is added.
        /// </summary>
        /// <param name="healthChecksBuilder">Builder used to register health checks.</param>
        /// <param name="settings">Application settings containing the <c>ExternalServices</c> collection.</param>
        private static void ExternalServices(IHealthChecksBuilder healthChecksBuilder, Settings settings)
        {
            if (settings?.ExternalServices != null)
                foreach (var externalServices in settings.ExternalServices)
                {
                    if (externalServices.Health)
                    {
                        healthChecksBuilder.AddUrlGroup(name: $"WebApi | {externalServices.Name} ({externalServices.Url})",
                            failureStatus: HealthStatus.Degraded,
                            uri: new Uri(new Uri(externalServices.Url), externalServices.InfraPath),
                            tags: new[] { "EnternalServices", "WebApi" });
                    }
                }
        }

        /// <summary>
        /// Registers SQL Server health checks for each configured connection string.
        /// </summary>
        /// <param name="healthChecksBuilder">Builder used to register health checks.</param>
        /// <param name="settings">Application settings containing SQL Server connection definitions.</param>
        private static void ConnectionStringsSqlServer(IHealthChecksBuilder healthChecksBuilder, Settings settings)
        {
            if (settings?.ConnectionStrings?.SqlServer != null)
                foreach (var sqlServer in settings.ConnectionStrings.SqlServer)
                    healthChecksBuilder.AddSqlServer(name: $"SqlServer | {sqlServer.Name}",
                        failureStatus: HealthStatus.Unhealthy, connectionString: sqlServer.ConnectionString,
                        tags: new[] { "Repository", "SqlServer" });
        }

        /// <summary>
        /// Registers MongoDB health checks for each configured MongoDB connection.
        /// </summary>
        /// <param name="healthChecksBuilder">Builder used to register health checks.</param>
        /// <param name="settings">Application settings containing MongoDB connection definitions.</param>
        private static void ConnectionStringsMongoDb(IHealthChecksBuilder healthChecksBuilder, Settings settings)
        {
            if (settings?.ConnectionStrings?.MongoDb != null)
                foreach (var mongoDb in settings.ConnectionStrings.MongoDb)
                    healthChecksBuilder.AddMongoDb(name: $"MongoDb | {mongoDb.Name}", failureStatus: HealthStatus.Unhealthy,
                        mongodbConnectionString: mongoDb.ConnectionString, tags: new[] { "Repository", "MongoDb" });
        }

        /// <summary>
        /// Registers Azure Blob Storage health checks for each configured blob connection.
        /// </summary>
        /// <param name="healthChecksBuilder">Builder used to register health checks.</param>
        /// <param name="settings">Application settings containing Azure Blob Storage connection definitions.</param>
        private static void ConnectionStringsAzureBlob(IHealthChecksBuilder healthChecksBuilder, Settings settings)
        {
            if (settings?.ConnectionStrings?.AzureBlob != null)
                foreach (var azureBlob in settings.ConnectionStrings.AzureBlob)
                    healthChecksBuilder.AddAzureBlobStorage(name: $"Blob | {azureBlob.Name}",
                        failureStatus: HealthStatus.Unhealthy, connectionString: azureBlob.ConnectionString,
                        tags: new[] { "Repository", "BlobStorage" });
        }

        /// <summary>
        /// Registers Redis health checks for each configured Redis connection.
        /// </summary>
        /// <param name="healthChecksBuilder">Builder used to register health checks.</param>
        /// <param name="settings">Application settings containing Redis connection definitions.</param>
        private static void ConnectionStringsRedis(IHealthChecksBuilder healthChecksBuilder, Settings settings)
        {
            if (settings?.ConnectionStrings?.Redis != null)
                foreach (var redis in settings.ConnectionStrings.Redis)
                    healthChecksBuilder.AddRedis(name: $"Redis | {redis.Name}", failureStatus: HealthStatus.Unhealthy,
                        redisConnectionString: redis.ConnectionString, tags: new[] { "Cache", "Redis" });
        }

        /// <summary>
        /// Registers Azure Service Bus health checks for configured topics and queues.
        /// For each ServiceBus configuration, registered topics and queues will be added as health checks.
        /// </summary>
        /// <param name="healthChecksBuilder">Builder used to register health checks.</param>
        /// <param name="settings">Application settings containing Service Bus configurations.</param>
        private static void ServiceBus(IHealthChecksBuilder healthChecksBuilder, Settings settings)
        {
            if (settings?.ServiceBus != null)
                foreach (var serviceBus in settings.ServiceBus)
                {
                    if (serviceBus.Topics != null)
                        foreach (var topic in serviceBus.Topics)
                            healthChecksBuilder.AddAzureServiceBusTopic(name: $"ServiceBus-Topic | {topic.Name} ({topic.Path})",
                                failureStatus: HealthStatus.Degraded, connectionString: serviceBus.ConnectionString,
                                topicName: topic.Path, tags: new[] { "ServiceBus", "Topic" });

                    if (serviceBus.Queues != null)
                        foreach (var queue in serviceBus.Queues)
                            healthChecksBuilder.AddAzureServiceBusQueue(name: $"ServiceBus-Queue | {queue.Name} ({queue.Path})",
                                failureStatus: HealthStatus.Degraded, connectionString: serviceBus.ConnectionString,
                                queueName: queue.Path, tags: new[] { "ServiceBus", "Queue" });
                }
        }

        /// <summary>
        /// Registers Azure Event Hubs health checks for each configured event hub.
        /// </summary>
        /// <param name="healthChecksBuilder">Builder used to register health checks.</param>
        /// <param name="settings">Application settings containing Event Hub configurations.</param>
        private static void EventHub(IHealthChecksBuilder healthChecksBuilder, Settings settings)
        {
            if (settings?.EventHub != null)
                foreach (var eventHubConfig in settings.EventHub)
                {
                    foreach (var eventHub in eventHubConfig.EventHubs)
                        healthChecksBuilder.AddAzureEventHub(name: $"EventHub | {eventHub.Name} ({eventHub.Path})",
                            failureStatus: HealthStatus.Degraded, connectionString: eventHubConfig.ConnectionString,
                            eventHubName: eventHub.Path, tags: new[] { "EventHubs" });
                }
        }

        /// <summary>
        /// Registers URL-based health checks for configured authorization servers.
        /// For each enabled authorization server in settings, an URL group health check is added.
        /// </summary>
        /// <param name="healthChecksBuilder">Builder used to register health checks.</param>
        /// <param name="settings">Application settings containing authorization server definitions.</param>
        private static void AuthorizationServer(IHealthChecksBuilder healthChecksBuilder, Settings settings)
        {
            if (settings?.AuthorizationServer != null)
                foreach (var authorizationServer in settings.AuthorizationServer)
                {
                    if (authorizationServer.Health)
                    {
                        healthChecksBuilder.AddUrlGroup(name: $"AuthorizationServer | {authorizationServer.Name} ({authorizationServer.Url})",
                            failureStatus: HealthStatus.Degraded,
                            uri: new Uri(new Uri(authorizationServer.Url), authorizationServer.InfraPath),
                            tags: new[] { "AuthorizationServer" });
                    }
                }
        }
    }
}