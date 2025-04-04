# Sample Data Model
Includes the information about analysed sample.

>[!NOTE]
> All exact dates are hiddent and protected. Relative dates are shown instead, if calculation was possible.

**`donor_id`*** - Sample donor identifier.
- Type: _String_
- Limitations: Maximum length 255
- Example: `"DO1"`

**`specimen_id`*** - Identifier of the specimen the sample was created from.
- Type: _String_
- Limitations: Maximum length 255
- Example: `"TI1"`

**`specimen_type`*** - Type of the specimen the sample was created from.
- Type: _String_
- Possible values: `"Material"`, `"Line"`, `"Organoid"`, `"Xenograft"`
- Example: `"Material"`

**`analysis_type`*** - Type of the analysis performed on the sample.
- Type: _String_
- Possible values: `"WGS"`, `"WES"`, `"RNASeq"`, `"scRNASeq"`, `"snRNASeq"`, `"ATACSeq"`, `"scATACSeq"`, `"snATACSeq"`, `"MethArray"`, `"WGBS"`, `"RRBS"`
- Example: `"WES"`

**`analysis_date`** - Date when the analysis was performed.
- Type: _Date_
- Limitations: Either 'analysis_date' or 'analysis_day' should be set.
- Format: "YYYY-MM-DD"
- Example: `2023-12-01`

**`analysis_day`** - Relative number of days since diagnosis statement when the analysis was performed.
- Type: _Integer_
- Limitations: Integet, greater than or equal to 1, either 'date' or 'day' should be set.
- Example: `22`

**`genome`** - Reference genome.
- Notes: for DNA samples only `grch37` is supported for now.
- Type: _String_
- Limitations: Maximum length 100
- Example: `"grch37"`

**`purity`** - Sample purity (TCC) percentage of tumor cells in the tissue.
- Notes: Available for CNVs data only.
- Type: _Double_
- Limitations: Should be in range [0, 1].
- Example: `0.95`

**`ploidy`** - Sample ploidy.
- Notes: Available for CNVs data only.
- Type: _Double_
- Limitations: Should be greater than 0
- Example: `2`

**`cells`** - Number of cells in the sample.
- Notes: Available for single cell RNA sequenicng analysis only.
- Type: _Integer_
- Limitations: Should be greater than 1
- Example: `1000`

**`resources`** - Resources associated with the sample.
- Type: _Array_
- Element type: _Object([Resource](api-models-resource.md))_
- Limitations: Should contain at leas one element
- Example: `[{...}, {...}]`


#### Specimen Type
Specimen can be of the following types:
- `"Material"` - all donor derived materials
- `"Line"` - cell lines
- `"Organoid"` - organoids
- `"Xenograft"` - xenografts

#### Analysis Type
Analysis can be of the following types:
- `"WGS"` - whole genome DNA sequencing
- `"WES"` - whole exome DNA sequencing
- `"RNASeq"` - bulk RNA sequencing
- `"scRNASeq"` - single cell RNA sequencing
- `"snRNASeq"` - single nucleus RNA sequencing
- `"ATACSeq"` - ATAC sequencing
- `"scATACSeq"` - single cell ATAC sequencing
- `"snATACSeq"` - single nucleus ATAC sequencing
- `"MethArray"` - Illumina Methylation Array
- `"WGBS"` - whole genome bisulfite sequencing
- `"RRBS"` - reduced representation bisulfite sequencing

##
**`*`** - Required fields
