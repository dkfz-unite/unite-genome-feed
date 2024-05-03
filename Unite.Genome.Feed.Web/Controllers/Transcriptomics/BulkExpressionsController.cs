﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Data.Context.Services.Tasks;
using Unite.Data.Entities.Tasks.Enums;
using Unite.Genome.Feed.Web.Configuration.Constants;
using Unite.Genome.Feed.Web.Models.Base;
using Unite.Genome.Feed.Web.Models.Transcriptomics;
using Unite.Genome.Feed.Web.Models.Transcriptomics.Binders;
using Unite.Genome.Feed.Web.Submissions;

namespace Unite.Genome.Feed.Web.Controllers.Transcriptomics;

[Route("api/rna/exp/bulk")]
[Authorize(Policy = Policies.Data.Writer)]
public class BulkExpressionsController : Controller
{
    private readonly TranscriptomicsSubmissionService _submissionService;
    private readonly SubmissionTaskService _submissionTaskService;

	public BulkExpressionsController(
        TranscriptomicsSubmissionService submissionService,
        SubmissionTaskService submissionTaskService)
	{
		_submissionService = submissionService;
        _submissionTaskService = submissionTaskService;
    }


    [HttpPost("")]
    [RequestSizeLimit(100_000_000)]
    public IActionResult Post([FromBody] SequencingDataModel<BulkExpressionModel> model)
	{
        return PostData(model);
	}

    [HttpPost("tsv")]
    [RequestSizeLimit(100_000_000)]
    public IActionResult PostTsv([ModelBinder(typeof(BulkExpressionTsvModelsBinder))] SequencingDataModel<BulkExpressionModel> model)
    {
        return PostData(model);
    }


    private IActionResult PostData(SequencingDataModel<BulkExpressionModel> model)
    {
        var submissionId = _submissionService.AddBulkSubmission(model);

        _submissionTaskService.CreateTask(SubmissionTaskType.BGE, submissionId);

        return Ok();
    }
}
