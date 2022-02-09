﻿using System.Linq;
using Microsoft.Extensions.Logging;
using Unite.Data.Entities.Tasks.Enums;
using Unite.Indices.Entities.Mutations;
using Unite.Indices.Services;
using Unite.Genome.Feed.Web.Services;

namespace Unite.Genome.Feed.Web.Handlers
{
    public class MutationsIndexingHandler
    {
        private readonly TasksProcessingService _taskProcessingService;
        private readonly IIndexCreationService<MutationIndex> _indexCreationService;
        private readonly IIndexingService<MutationIndex> _indexingService;
        private readonly ILogger _logger;

        public MutationsIndexingHandler(
            TasksProcessingService taskProcessingService,
            IIndexCreationService<MutationIndex> indexCreationService,
            IIndexingService<MutationIndex> indexingService,
            ILogger<MutationsIndexingHandler> logger)
        {
            _taskProcessingService = taskProcessingService;
            _indexCreationService = indexCreationService;
            _indexingService = indexingService;
            _logger = logger;
        }


        public void Handle(int bucketSize)
        {
            ProcessIndexingTasks(bucketSize);
        }


        private void ProcessIndexingTasks(int bucketSize)
        {
            _taskProcessingService.Process(TaskType.Indexing, TaskTargetType.Mutation, bucketSize, (tasks) =>
            {
                var indices = tasks.Select(task =>
                {
                    var id = long.Parse(task.Target);

                    var index = _indexCreationService.CreateIndex(id);

                    return index;

                }).ToArray();

                _indexingService.IndexMany(indices);

                _logger.LogInformation($"Finished indexing of {tasks.Length} mutations");
            });
        }
    }
}
