﻿using System.Diagnostics;
using Unite.Data.Entities.Genome.Variants.CNV;
using Unite.Data.Entities.Tasks.Enums;
using Unite.Data.Services.Tasks;
using Unite.Genome.Indices.Services;

namespace Unite.Genome.Feed.Web.Handlers;

public class CopyNumberVariantsIndexingHandler
{
    private readonly TasksProcessingService _taskProcessingService;
    private readonly VariantIndexCreationService<Variant, VariantOccurrence> _indexCreationService;
    private readonly VariantsIndexingService _indexingService;
    private readonly ILogger _logger;


    public CopyNumberVariantsIndexingHandler(
        TasksProcessingService taskProcessingService,
        VariantIndexCreationService<Variant, VariantOccurrence> indexCreationService,
        VariantsIndexingService indexingService,
        ILogger<MutationsIndexingHandler> logger)
    {
        _taskProcessingService = taskProcessingService;
        _indexCreationService = indexCreationService;
        _indexingService = indexingService;
        _logger = logger;
    }


    public void Prepare()
    {
        _indexingService.UpdateMapping().GetAwaiter().GetResult();
    }

    public void Handle(int bucketSize)
    {
        ProcessIndexingTasks(bucketSize);
    }


    private void ProcessIndexingTasks(int bucketSize)
    {
        var stopwatch = new Stopwatch();

        _taskProcessingService.Process(IndexingTaskType.CNV, bucketSize, (tasks) =>
        {
            _logger.LogInformation($"Indexing {tasks.Length} copy number variants");

            stopwatch.Restart();

            var indices = tasks.Select(task =>
            {
                var id = long.Parse(task.Target);

                var index = _indexCreationService.CreateIndex(id);

                return index;

            }).ToArray();

            _indexingService.IndexMany(indices);

            stopwatch.Stop();

            _logger.LogInformation($"Indexing of {tasks.Length} copy number variants completed in {Math.Round(stopwatch.Elapsed.TotalSeconds, 2)}s");
        });
    }
}