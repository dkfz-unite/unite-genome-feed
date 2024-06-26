﻿using Unite.Data.Context;
using Unite.Data.Entities.Genome.Analysis.Dna.Cnv;
using Unite.Genome.Feed.Data.Models.Dna.Cnv;

namespace Unite.Genome.Feed.Data.Repositories.Dna.Cnv;

public class VariantEntryRepository : VariantEntryRepository<VariantEntry, Variant, VariantModel>
{
    public VariantEntryRepository(DomainDbContext dbContext, VariantRepository<Variant, VariantModel> variantRepository) : base(dbContext, variantRepository)
    {
    }
}
