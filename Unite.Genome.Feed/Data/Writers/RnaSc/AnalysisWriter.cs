using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Essentials.Extensions;
using Unite.Genome.Feed.Data.Models;
using Unite.Genome.Feed.Data.Repositories;

namespace Unite.Genome.Feed.Data.Writers.RnaSc;

public class AnalysisWriter : DataWriter<SampleModel, AnalysisWriteAudit>
{
    public AnalysisWriter(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    protected override void Initialize(DomainDbContext dbContext)
    {
        _sampleRepository = new SampleRepository(dbContext);
        _resourceRepository = new ResourceRepository(dbContext);
    }

    protected override void ProcessModel(SampleModel model, ref AnalysisWriteAudit audit)
    {
        var sampleId = WriteSample(model, ref audit);

        if (model.Resources.IsNotEmpty())
            WriteResources(sampleId, model.Resources, ref audit);
    }
}
