using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Data.Context.Services.Tasks;
using Unite.Data.Entities.Tasks.Enums;
using Unite.Genome.Feed.Web.Configuration.Constants;
using Unite.Genome.Feed.Web.Models.Base;
using Unite.Genome.Feed.Web.Models.RnaSc;
using Unite.Genome.Feed.Web.Models.RnaSc.Binders;
using Unite.Genome.Feed.Web.Submissions;

namespace Unite.Genome.Feed.Web.Controllers.RnaSc;

[Route("api/rnasc/analysis/exp")]
[Authorize(Policy = Policies.Data.Writer)]
public class ExpressionsController : Controller
{
    private readonly RnaScSubmissionService _submissionService;
    private readonly SubmissionTaskService _submissionTaskService;

    public ExpressionsController(
        RnaScSubmissionService submissionService,
        SubmissionTaskService submissionTaskService)
    {
        _submissionService = submissionService;
        _submissionTaskService = submissionTaskService;
    }


    [HttpPost("")]
    [RequestSizeLimit(100_000_000)]
    public IActionResult Post([FromBody] AnalysisModel<ExpressionModel> model)
    {
        var submissionId = _submissionService.AddExpSubmission(model);

        _submissionTaskService.CreateTask(SubmissionTaskType.RNASC_EXP, submissionId);

        return Ok();
    }

    [HttpPost("tsv")]
    [RequestSizeLimit(100_000_000)]
    public IActionResult PostTsv([ModelBinder(typeof(AnalysisTsvModelsBinder))] AnalysisModel<ResourceModel> model)
    {
        var analysisModel = new AnalysisModel<ExpressionModel>
        {
            TargetSample = model.TargetSample,
            MatchedSample = model.MatchedSample,
            Resources = model.Entries
        };

        return Post(analysisModel);
    }
}
