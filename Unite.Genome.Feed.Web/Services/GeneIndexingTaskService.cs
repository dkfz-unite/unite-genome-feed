﻿using System;
using System.Collections.Generic;
using System.Linq;
using Unite.Data.Entities.Genome;
using Unite.Data.Entities.Tasks.Enums;
using Unite.Data.Services;

namespace Unite.Genome.Feed.Web.Services
{
    public class GeneIndexingTaskService : IndexingTaskService<Gene, int>
    {
        protected override int BucketSize => 1000;


        public GeneIndexingTaskService(DomainDbContext dbContext) : base(dbContext)
        {
        }


        public override void CreateTasks()
        {
            IterateEntities<Gene, int>(gene => true, gene => gene.Id, genes =>
            {
                CreateTasks(TaskType.Indexing, TaskTargetType.Gene, genes);
            });
        }

        public override void CreateTasks(IEnumerable<int> keys)
        {
            IterateEntities<Gene, int>(gene => keys.Contains(gene.Id), gene => gene.Id, genes =>
            {
                CreateTasks(TaskType.Indexing, TaskTargetType.Gene, genes);
            });
        }

        public override void PopulateTasks(IEnumerable<int> keys)
        {
            throw new NotImplementedException();
        }


        protected override IEnumerable<int> LoadRelatedDonors(IEnumerable<int> keys)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<int> LoadRelatedGenes(IEnumerable<int> keys)
        {
            return keys;
        }

        protected override IEnumerable<int> LoadRelatedImages(IEnumerable<int> keys)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<long> LoadRelatedMutations(IEnumerable<int> keys)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<int> LoadRelatedSpecimens(IEnumerable<int> keys)
        {
            throw new NotImplementedException();
        }
    }
}