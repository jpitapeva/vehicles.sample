using System;
using System.Collections.Generic;

namespace Case.Model
{
    /// <summary>
    /// Standard settings for applications.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Serilog configuration.
        /// </summary>
        public SerilogSettings Serilog { get; set; }

        /// <summary>
        /// Connection strings configuration.
        /// </summary>
        public ConnectionStringSettings ConnectionStrings { get; set; }

        /// <summary>
        /// List of external services configurations.
        /// </summary>
        public IList<ExternalServicesSettings> ExternalServices { get; set; }

        /// <summary>
        /// List of Service Bus configurations.
        /// </summary>
        public IList<ServiceBusSettings> ServiceBus { get; set; }

        /// <summary>
        /// List of Event Hub configurations.
        /// </summary>
        public IList<EventHubSettings> EventHub { get; set; }

        /// <summary>
        /// List of authorization server configurations.
        /// </summary>
        public IList<AuthorizationServerSettings> AuthorizationServer { get; set; }

        /// <summary>
        /// OpenTelemetry configuration.
        /// </summary>
        public OpenTelemetrySettings OpenTelemetry { get; set; }

        /// <summary>
        /// Cache configuration.
        /// </summary>
        public CacheSettings Cache { get; set; }

        /// <summary>
        /// Application-specific configuration.
        /// </summary>
        public InternalSettings Internal { get; set; }
    }

    public class ConnectionStringSettings
    {
        /// <summary>
        /// List of SqlServer connection string configurations.
        /// </summary>
        public IList<ConnectionStringDefaultSettings> SqlServer { get; set; }

        /// <summary>
        /// List of MongoDb connection string configurations.
        /// </summary>
        public IList<ConnectionStringDefaultSettings> MongoDb { get; set; }

        /// <summary>
        /// List of Redis connection string configurations.
        /// </summary>
        public IList<ConnectionStringDefaultSettings> Redis { get; set; }

        /// <summary>
        /// List of Azure Blob connection string configurations.
        /// </summary>
        public IList<ConnectionStringDefaultSettings> AzureBlob { get; set; }
    }

    /// <summary>
    /// Serilog configuration.
    /// </summary>
    public class SerilogSettings
    {
        /// <summary>
        /// List of Serilog using assemblies, e.g. ["Serilog.Sinks.Console", "Serilog.Sinks.Dynatrace"].
        /// </summary>
        public IList<string> Using { get; set; }

        /// <summary>
        /// Defines the MinimumLevel settings for logs.
        /// </summary>
        public SerilogMinimumLevelSettings MinimumLevel { get; set; }

        /// <summary>
        /// List of Serilog enrichers, e.g. ["WithProcessId"].
        /// </summary>
        public IList<string> Enrich { get; set; }

        /// <summary>
        /// Individual WriteTo configurations, e.g. { "Name": "Console" }, { "Name": "Dynatrace" }.
        /// </summary>
        public IList<SerilogWriteToSettings> WriteTo { get; set; }
    }

    /// <summary>
    /// Properties for Serilog minimum log level.
    /// </summary>
    public class SerilogMinimumLevelSettings
    {
        /// <summary>
        /// Default and override values for MinimumLevel.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// Override properties for MinimumLevel.
        /// </summary>
        public SerilogOverrideSerilogSettings Override { get; set; }
    }

    /// <summary>
    /// Properties for Serilog override settings.
    /// </summary>
    public class SerilogOverrideSerilogSettings
    {
        /// <summary>
        /// Override level for Microsoft logs.
        /// </summary>
        public string Microsoft { get; set; }

        /// <summary>
        /// Override level for System logs.
        /// </summary>
        public string System { get; set; }
    }

    /// <summary>
    /// Configuration for Serilog WriteTo targets.
    /// </summary>
    public class SerilogWriteToSettings
    {
        /// <summary>
        /// Name of the WriteTo target.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Arguments configuration for the WriteTo target.
        /// </summary>
        public SerilogArgsSettings Args { get; set; }
    }

    /// <summary>
    /// Configuration for Serilog WriteTo arguments.
    /// </summary>
    public class SerilogArgsSettings
    {
        /// <summary>
        /// Output template argument for Serilog.
        /// </summary>
        public string OutputTemplate { get; set; }

        /// <summary>
        /// Theme argument for Serilog.
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// Access token argument for Serilog sinks that require authentication.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Ingest URL argument for remote Serilog sinks.
        /// </summary>
        public string IngestUrl { get; set; }

        /// <summary>
        /// Application ID argument for Serilog sinks.
        /// </summary>
        public string ApplicationID { get; set; }

        /// <summary>
        /// RestrictedToMinimumLevel argument for Serilog sinks.
        /// </summary>
        public string RestrictedToMinimumLevel { get; set; }
    }

    public class ExternalServicesSettings : UrlsBaseSettings
    {
        /// <summary>
        /// API endpoints map.
        /// </summary>
        public Dictionary<string, string> EndPoints { get; set; }

        /// <summary>
        /// Authorization client configuration for the external service.
        /// </summary>
        public AuthorizationClientSettings AuthorizationClient { get; set; }
    }

    public class EventHubsSettings
    {
        /// <summary>
        /// Name of the Event Hub topic.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path of the Event Hub topic.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Container name used for message consumption.
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// Consumer group name used for message consumption.
        /// </summary>
        public string ConsumerGroup { get; set; }

        /// <summary>
        /// Parameters for redelivery/resilience.
        /// </summary>
        public ResilienceSettings Redelivery { get; set; }
    }

    public class ServiceBusSettings : BaseSettings
    {
        /// <summary>
        /// Operation timeout in seconds that specifies how long message operations should complete before timing out.
        /// </summary>
        public int OperationTimeout { get; set; }

        /// <summary>
        /// Number of concurrent messages.
        /// </summary>
        public int ConcurrencyLimit { get; set; }

        /// <summary>
        /// List of Service Bus topic configurations.
        /// </summary>
        public IList<ServiceBusTopicSettings> Topics { get; set; }

        /// <summary>
        /// List of Service Bus queue configurations.
        /// </summary>
        public IList<ServiceBusQueueSettings> Queues { get; set; }
    }

    public class EventHubSettings : BaseSettings
    {
        /// <summary>
        /// List of Event Hub topic configurations.
        /// </summary>
        public IList<EventHubsSettings> EventHubs { get; set; }

        /// <summary>
        /// Azure Blob connection string configuration for Event Hub storage.
        /// </summary>
        public string AzureBlob { get; set; }
    }

    /// <summary>
    /// Polly and redelivery configuration.
    /// </summary>
    public class ResilienceSettings
    {
        /// <summary>
        /// Number of retries.
        /// </summary>
        public int Retries { get; set; }

        /// <summary>
        /// Timeout in seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; }

        /// <summary>
        /// Interval in seconds between retries.
        /// </summary>
        public int RetryIntervalSeconds { get; set; }

        /// <summary>
        /// List of intervals in seconds between retries.
        /// </summary>
        public IList<int> RetryIntervalSecondsList { get; set; }

        /// <summary>
        /// Minimum interval in seconds between retries.
        /// </summary>
        public int MinimumIntervalSeconds { get; set; }

        /// <summary>
        /// Maximum interval in seconds between retries.
        /// </summary>
        public int MaximumIntervalSeconds { get; set; }

        /// <summary>
        /// Delta interval in seconds between retries.
        /// </summary>
        public int IntervalDeltaSeconds { get; set; }
    }

    public class ServiceBusBaseSettings
    {
        /// <summary>
        /// Name of the Service Bus queue or topic.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path of the Service Bus queue or topic.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Subscription name for the Service Bus subscription.
        /// </summary>
        public string SubscriptionName { get; set; }

        /// <summary>
        /// Number of messages delivered to the consumer at once (prefetch count).
        /// </summary>
        public int PrefetchCount { get; set; }

        /// <summary>
        /// Default message time-to-live in minutes.
        /// </summary>
        public int MinutesToDefaultMessageTimeToLive { get; set; }

        /// <summary>
        /// Auto-delete on idle duration in minutes; minimum is 5 minutes.
        /// </summary>
        public int MinutesToAutoDeleteOnIdle { get; set; }

        /// <summary>
        /// Number of threads running in parallel in the consumer.
        /// </summary>
        public int ConcurrentMessageLimit { get; set; }

        /// <summary>
        /// Allows disabling default topology creation for MassTransit consumers.
        /// </summary>
        public bool ConfigureConsumeTopology { get; set; }

        /// <summary>
        /// Enables partitioning.
        /// </summary>
        public bool EnablePartitioning { get; set; }

        /// <summary>
        /// Redelivery/resilience configuration.
        /// </summary>
        public ResilienceSettings Redelivery { get; set; }
    }

    public class ServiceBusTopicSettings : ServiceBusBaseSettings
    { }

    public class ServiceBusQueueSettings : ServiceBusBaseSettings
    {
        /// <summary>
        /// Default lock duration in minutes; default is 1 minute.
        /// </summary>
        public int MinutesToLockDuration { get; set; }

        /// <summary>
        /// Duplicate detection window in minutes used to help control MessageId for all messages sent to a queue or topic.
        /// </summary>
        public int MinutesToDuplicateDetection { get; set; }
    }

    /// <summary>
    /// Health check UI / authorization server configuration.
    /// </summary>
    public class AuthorizationServerSettings : UrlsBaseSettings
    {
        /// <summary>
        /// List of issuers.
        /// </summary>
        public IList<string> Issuers { get; set; }

        /// <summary>
        /// URL of the authorization server.
        /// </summary>
        public string AuthorityUrl { get; set; }

        /// <summary>
        /// Secret key for the authorization server.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Client identifier for the authorization server.
        /// </summary>
        public string ClientId { get; set; }
    }

    /// <summary>
    /// Authorization client configuration.
    /// </summary>
    public class AuthorizationClientSettings
    {
        /// <summary>
        /// Username for authorization.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Password for authorization.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Secret key for authorization.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Client identifier for authorization.
        /// </summary>
        public string ClientId { get; set; }
    }

    public class UrlsBaseSettings
    {
        /// <summary>
        /// Application name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Application URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Path to the infra API.
        /// </summary>
        public string InfraPath { get; set; }

        /// <summary>
        /// Enables health check for the API.
        /// </summary>
        public bool Health { get; set; }

        /// <summary>
        /// HTTP connection timeout in seconds; default is 100 seconds.
        /// </summary>
        public int HttpConnectionTimeoutSeconds { get; set; }

        /// <summary>
        /// Polly resilience configuration.
        /// </summary>
        public ResilienceSettings Polly { get; set; }
    }

    public class ConnectionStringDefaultSettings : BaseSettings
    { }

    public class BaseSettings
    {
        /// <summary>
        /// Service name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Connection string for the service.
        /// </summary>
        public string ConnectionString { get; set; }
    }

    /// <summary>
    /// OpenTelemetry configuration.
    /// </summary>
    public class OpenTelemetrySettings
    {
        /// <summary>
        /// Service name for OpenTelemetry.
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Minimum log level for OpenTelemetry.
        /// </summary>
        public string MinimumLogLevel { get; set; }

        /// <summary>
        /// Traces endpoint for OpenTelemetry.
        /// </summary>
        public string TracesEndpoint { get; set; }

        /// <summary>
        /// Metrics endpoint for OpenTelemetry.
        /// </summary>
        public string MetricsEndpoint { get; set; }

        /// <summary>
        /// Logs endpoint for OpenTelemetry.
        /// </summary>
        public string LogsEndpoint { get; set; }

        /// <summary>
        /// Exporter token for OpenTelemetry.
        /// </summary>
        public string ExporterToken { get; set; }

        /// <summary>
        /// Indicates whether metrics are enabled.
        /// </summary>
        public bool EnableMetrics { get; set; }
    }

    /// <summary>
    /// Application cache configuration.
    /// </summary>
    public class CacheSettings
    {
        /// <summary>
        /// Enables application cache (true/false or configuration string).
        /// </summary>
        public string Enable { get; set; }

        /// <summary>
        /// Default TTL in seconds for cache entries.
        /// </summary>
        public int TtlDefaultSeconds { get; set; }

        /// <summary>
        /// Cache type.
        /// </summary>
        public String CacheType { get; set; }
    }

    /// <summary>
    /// Application-specific internal settings.
    /// </summary>
    public class InternalSettings
    {
        /// <summary>
        /// Application name.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// SID GUID for the application.
        /// </summary>
        public Guid SidGuid { get; set; }

        /// <summary>
        /// SID identifier for the application.
        /// </summary>
        public int SidId { get; set; }

        /// <summary>
        /// Dynamic application settings.
        /// </summary>
        public dynamic Dynamic { get; set; }
    }
}