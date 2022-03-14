﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Unite.Genome.Feed.Web.Configuration.Options;
using Unite.Genome.Feed.Web.Handlers;

namespace Unite.Genome.Feed.Web.HostedServices
{
    public class GenesIndexingHostedService : BackgroundService
    {
        private readonly GenesIndexingOptions _options;
        private readonly GenesIndexingHandler _handler;
        private readonly ILogger<GenesIndexingHostedService> _logger;


        public GenesIndexingHostedService(
            GenesIndexingOptions options,
            GenesIndexingHandler handler,
            ILogger<GenesIndexingHostedService> logger)
        {
            _options = options;
            _handler = handler;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Genes indexing service started");

            stoppingToken.Register(() => _logger.LogInformation("Genes indexing service stopped"));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _handler.Handle(_options.BucketSize);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception.Message);

                    if (exception.InnerException != null)
                    {
                        _logger.LogError(exception.InnerException.Message);
                    }
                }
                finally
                {
                    await Task.Delay(_options.Interval, stoppingToken);
                }
            }
        }
    }
}