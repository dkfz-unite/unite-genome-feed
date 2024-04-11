﻿using System.Diagnostics;
using Unite.Data.Context.Services.Tasks;
using Unite.Data.Entities.Genome.Variants.SV;
using Unite.Data.Entities.Tasks.Enums;
using Unite.Essentials.Extensions;
using Unite.Genome.Indices.Services;
using Unite.Indices.Context;
using Unite.Indices.Entities.Variants;

namespace Unite.Genome.Feed.Web.Handlers.Indexing;

public class SvsIndexingHandler
{
    private readonly TasksProcessingService _taskProcessingService;
    private readonly VariantIndexCreationService<Variant, VariantEntry> _indexCreationService;
    private readonly IIndexService<VariantIndex> _indexingService;
    private readonly ILogger _logger;


    public SvsIndexingHandler(
        TasksProcessingService taskProcessingService,
        VariantIndexCreationService<Variant, VariantEntry> indexCreationService,
        IIndexService<VariantIndex> indexingService,
        ILogger<SvsIndexingHandler> logger)
    {
        _taskProcessingService = taskProcessingService;
        _indexCreationService = indexCreationService;
        _indexingService = indexingService;
        _logger = logger;
    }


    public async Task Prepare()
    {
        await _indexingService.UpdateIndex();
    }

    public async Task Handle(int bucketSize)
    {
        await ProcessIndexingTasks(bucketSize);
    }


    private async Task ProcessIndexingTasks(int bucketSize)
    {
        var stopwatch = new Stopwatch();

        await _taskProcessingService.Process(IndexingTaskType.SV, bucketSize, async (tasks) =>
        {
            if (_taskProcessingService.HasTasks(WorkerType.Submission) || _taskProcessingService.HasTasks(WorkerType.Annotation))
                return false;

            stopwatch.Restart();

            var indicesToDelete = new List<string>();
            var indicesToCreate = new List<VariantIndex>();

            tasks.ForEach(task =>
            {
                var id = long.Parse(task.Target);

                var index = _indexCreationService.CreateIndex(id);

                if (index == null)
                    indicesToDelete.Add($"SV{id}");
                else
                    indicesToCreate.Add(index);
            });

            if (indicesToDelete.Any())
                await _indexingService.DeleteRange(indicesToDelete);
            
            if (indicesToCreate.Any())
                await _indexingService.AddRange(indicesToCreate);

            stopwatch.Stop();

            _logger.LogInformation("Indexed {number} SVs in {time}s", tasks.Length, Math.Round(stopwatch.Elapsed.TotalSeconds, 2));

            return true;
        });
    }
}
