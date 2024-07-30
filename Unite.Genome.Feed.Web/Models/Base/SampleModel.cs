﻿using System.Text.Json.Serialization;
using Unite.Data.Entities.Genome.Analysis.Enums;
using Unite.Data.Entities.Specimens.Enums;

namespace Unite.Genome.Feed.Web.Models.Base;

public record SampleModel
{
    public const double DefaultPloidy = 2.0;

    protected string _donorId;
    protected string _specimenId;
    protected SpecimenType? _specimenType;
    protected AnalysisType? _analysisType;
    protected DateOnly? _analysisDate;
    protected int? _analysisDay;
    protected double? _purity;
    protected double? _ploidy;
    protected int? _cellsNumber;
    protected string _genesModel;


    /// <summary>
    /// Sample donor identifier
    /// </summary>
    [JsonPropertyName("donor_id")]
    public virtual string DonorId { get => _donorId?.Trim(); set => _donorId = value; }

    /// <summary>
    /// Specimen identifier
    /// </summary>
    [JsonPropertyName("specimen_id")]
    public virtual string SpecimenId { get => _specimenId?.Trim(); set => _specimenId = value; }

    /// <summary>
    /// Specimen type
    /// </summary>
    [JsonPropertyName("specimen_type")]
    public virtual SpecimenType? SpecimenType { get => _specimenType; set => _specimenType = value; }

    /// <summary>
    /// Analysis type (WGS, WES, RNASeq, RNASeqSc)
    /// </summary>
    [JsonPropertyName("analysis_type")]
    public virtual AnalysisType? AnalysisType { get => _analysisType; set => _analysisType = value; }

    /// <summary>
    /// Analysis date
    /// </summary>
    [JsonPropertyName("analysis_date")]
    public virtual DateOnly? AnalysisDate { get => _analysisDate; set => _analysisDate = value; }

    /// <summary>
    /// Analysis day - relative day since the diagnosis statement when the analysis was performed
    /// </summary> 
    [JsonPropertyName("analysis_day")]
    public virtual int? AnalysisDay { get => _analysisDay; set => _analysisDay = value; } 

    /// <summary>
    /// Sample purity (TCC)
    /// </summary>
    [JsonPropertyName("purity")]
    public virtual double? Purity { get => GetPercent(_purity); set => _purity = value; }

    /// <summary>
    /// Sample ploidy
    /// </summary>
    [JsonPropertyName("ploidy")]
    public virtual double? Ploidy { get => _ploidy; set => _ploidy = value; }

    /// <summary>
    /// Sample cells number (if it's single cell sequencing)
    /// </summary>
    [JsonPropertyName("cells_number")]
    public virtual int? CellsNumber { get => _cellsNumber; set => _cellsNumber = value; }

    /// <summary>
    /// Sample genes model (if it's single cell sequencing)
    /// </summary>
    [JsonPropertyName("genes_model")]
    public virtual string GenesModel { get => _genesModel?.Trim(); set => _genesModel = value; }

    /// <summary>
    /// Sample resources
    /// </summary>
    [JsonPropertyName("resources")]
    public virtual ResourceModel[] Resources { get; set; }


    private static double? GetPercent(double? value)
    {
        if (!value.HasValue)
            return null;

        if (value.Value > 1)
            return value.Value / 100;
        else
            return value.Value;
    }
}
