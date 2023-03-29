﻿using System.Diagnostics;
using Unite.Cache.Configuration.Options;
using Unite.Data.Entities.Tasks.Enums;
using Unite.Data.Services;
using Unite.Data.Services.Configuration.Options;
using Unite.Data.Services.Tasks;
using Unite.Genome.Feed.Data;
using Unite.Genome.Feed.Web.Services.Annotation;
using Unite.Genome.Feed.Web.Submissions;

namespace Unite.Genome.Feed.Web.Handlers.Submission;

public class StructuralVariantsSubmissionHandler
{
    private readonly ISqlOptions _sqlOptions;
    private readonly IMongoOptions _mongoOptions;
    private readonly TasksProcessingService _taskProcessingService;
    private readonly ILogger _logger;

    private readonly Models.Variants.SV.Converters.SequencingDataModelConverter _dataConverter;


    public StructuralVariantsSubmissionHandler(
        ISqlOptions sqlOptions,
        IMongoOptions mongoOptions,
        TasksProcessingService taskProcessingService,
        ILogger<StructuralVariantsSubmissionHandler> logger)
    {
        _sqlOptions = sqlOptions;
        _mongoOptions = mongoOptions;
        _taskProcessingService = taskProcessingService;
        _logger = logger;

        _dataConverter = new Models.Variants.SV.Converters.SequencingDataModelConverter();
    }


    public void Handle()
    {
        ProcessSubmissionTasks();
    }


    private void ProcessSubmissionTasks()
    {
        var stopwatch = new Stopwatch();

        _taskProcessingService.Process(SubmissionTaskType.SV, 1, (tasks) =>
        {
            _logger.LogInformation($"Processing SV data submission");

            stopwatch.Restart();

            ProcessDefaultModelSubmission(tasks[0].Target);

            stopwatch.Stop();

            _logger.LogInformation($"Processing of SV data submission completed in {Math.Round(stopwatch.Elapsed.TotalSeconds, 2)}s");

            return true;
        });
    }

    private void ProcessDefaultModelSubmission(string submissionId)
    {
        using var dbContext = new DomainDbContext(_sqlOptions);

        var dataWriter = new SequencingDataWriter(dbContext);
        var annotationTaskService = new StructuralVariantAnnotationTaskService(dbContext);
        var submissionService = new VariantsSubmissionService(_mongoOptions);
        
        var sequencingData = submissionService.FindSvSubmission(submissionId);
        var analysisData = _dataConverter.Convert(sequencingData);

        dataWriter.SaveData(analysisData, out var audit);
        annotationTaskService.PopulateTasks(audit.StructuralVariants);
        submissionService.DeleteSvSubmission(submissionId);

        _logger.LogInformation(audit.ToString());
    }
}
