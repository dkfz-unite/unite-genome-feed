﻿using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Essentials.Extensions;
using Unite.Genome.Feed.Data.Models;
using Unite.Genome.Feed.Data.Models.Rna;
using Unite.Genome.Feed.Data.Repositories;
using Unite.Genome.Feed.Data.Repositories.Rna;

namespace Unite.Genome.Feed.Data.Writers.Rna;

public class BulkExpDataWriter : DataWriter<SampleModel, BulkExpDataWriteAudit>
{
    private const int _batchSize = 1000;

    private SampleRepository _sampleRepository;
    private GeneRepository _geneRepository;
    private ExpressionRepository _expressionRepository;

    public BulkExpDataWriter(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    protected override void Initialize(DomainDbContext dbContext)
    {
        _sampleRepository = new SampleRepository(dbContext);
        _geneRepository = new GeneRepository(dbContext);
        _expressionRepository = new ExpressionRepository(dbContext);
        _resourceRepository = new ResourceRepository(dbContext);
    }

    protected override void ProcessModel(SampleModel model, ref BulkExpDataWriteAudit audit)
    {
        var sample = _sampleRepository.FindOrCreate(model);

        if (model.Exps != null)
        {
            WriteExpressions(sample.Id, model.Exps, ref audit);
        }

        if (model.Resources != null)
        {
            WriteResources(sample.Id, model.Resources, ref audit);
        }
    }


    private void WriteExpressions(int analysedSampleId, IEnumerable<GeneExpressionModel> models, ref BulkExpDataWriteAudit audit)
    {
        var queue = new Queue<GeneExpressionModel>(models);

        _expressionRepository.RemoveAll(analysedSampleId);

        while (queue.Any())
        {
            var chunk = queue.Dequeue(_batchSize).ToArray();

            var existingGenes = _geneRepository.Find(chunk.Select(model => model.Gene)).ToArray();

            var createdGenes = _geneRepository.CreateMissing(chunk.Select(model => model.Gene), existingGenes).ToArray();

            var genesCache = Enumerable.Concat(createdGenes, existingGenes).ToArray();


            var geneExpressions = _expressionRepository.CreateAll(analysedSampleId, chunk, genesCache).ToArray();


            audit.GenesCreated += createdGenes.Length;

            audit.ExpressionsCreated += geneExpressions.Length;

            audit.Genes.AddRange(geneExpressions.Select(entity => entity.EntityId));
        }
    }
}
