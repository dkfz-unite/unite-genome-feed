using Unite.Essentials.Tsv;
using Unite.Genome.Feed.Web.Models.Base.Binders;

namespace Unite.Genome.Feed.Web.Models.Variants.SV.Binders;

public class TsvModelBinder : SequencingDataTsvModelBinder<VariantModel>
{
    protected override ClassMap<VariantModel> CreateMap()
    {
        return new ClassMap<VariantModel>()
            .Map(entity => entity.Chromosome, "chromosome_1")
            .Map(entity => entity.Start, "start_1")
            .Map(entity => entity.End, "end_1")
            .Map(entity => entity.FlankingSequenceFrom, "flanking_sequence_1")
            .Map(entity => entity.OtherChromosome, "chromosome_2")
            .Map(entity => entity.OtherStart, "start_2")
            .Map(entity => entity.OtherEnd, "end_2")
            .Map(entity => entity.Type, "type")
            .Map(entity => entity.Inverted, "inverted");
    }
}
