using FluentValidation;

namespace Unite.Genome.Feed.Web.Models.Base.Validators;

public class ResourceModelValidator : AbstractValidator<ResourceModel>
{
    private static readonly string[] _allowedTypes =
    {
        "dna", // dna allignment
        "dna-ssm", // simple somatic mutations
        "dna-cnv", // copy number variants
        "dna-sv", // structural variants
        "rna", // rna allignment
        "rna-exp", // gene expressions
        "rnasc", // single cell rna allignment
        "rnasc-exp", // single cell gene expressions
    };

    private static readonly string[] _allowedFormats = 
    {
        "txt",
        "tsv",
        "csv",
        "vcf", // variant call format
        "bam", // binary alignment map
        "mtx", // 10x genomics single cell gene expression matrix
    };

    private static readonly string[] _allowedArchives = 
    {
        "zip",
        "gz"
    };

    public ResourceModelValidator()
    {
        RuleFor(model => model.Type)
            .NotEmpty()
            .WithMessage("Should not be empty");

        RuleFor(model => model.Type)
            .Must(type => _allowedTypes.Contains(type))
            .WithMessage("Type is not allowed");

        RuleFor(model => model.Format)
            .NotEmpty()
            .WithMessage("Should not be empty");

        RuleFor(model => model.Format)
            .Must(format => _allowedFormats.Contains(format))
            .WithMessage("Format is not allowed");

        RuleFor(model => model.Archive)
            .Must(format => _allowedArchives.Contains(format))
            .When(model => model.Archive != null)
            .WithMessage("Archive is not allowed");

        RuleFor(model => model.Url)
            .NotEmpty()
            .WithMessage("Should not be empty");
    }
}
