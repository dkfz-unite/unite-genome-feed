# Copy Number Variants (CNV) Data Models

## Sequencing Data
Includes information about the analysis, samples and sequencing data.

**`Analysis`** - Sequencing analysis data.
- Type: _Object([Analysis](https://github.com/dkfz-unite/unite-genome-feed/blob/main/Docs/api-models-cnv.md#analysis))_
- Example: `{...}`

**`Samples`*** - Which samples were analysed.
- Type: _Array_
- Element type: _Object([Sample](https://github.com/dkfz-unite/unite-genome-feed/blob/main/Docs/api-models-cnv.md#sample))_
- Example: `[{...}, {...}]`

## Analysis
Sequencing analysis data.

**`Type`*** - Analysis type.
- Type: _String_
- Possible values: `"WGS"`, `"WES"`
- Example: `"WES"`

#### Analysis Type
Analysis can be of the following types:
- `"WGS"` - whole genome sequencing
- `"WES"` - whole exome sequencing

## Sample
Analysed sample data.

**`Id`*** - Sample identifier.
- Type: _String_
- Limitations: Maximum length 255
- Example: `"SA5"`

**`DonorId`*** - Sample donor identifier.
- Type: _String_
- Limitations: Maximum length 255
- Example: `"DO1"`

**`SpecimenId`*** - Identifier of the specimen the sample was created from.
- Type: _String_
- Limitations: Maximum length 255
- Example: `"TI1"`

**`SpecimenType`*** - Type of the specimen the sample was created from.
- Type: _String_
- Possible values: `"Tissue"`, `"CellLine"`, `"Organoid"`, `"Xenograft"`
- Example: `"Tissue"`

**`MatchedSampleId`** - Matched(control) sample identifier from samples array.
- Type: _String_
- Limitations: Should match single sample identifier from samples array
- Example: `"SA14"`

**`Ploidy`** - Sample ploidy.
- Note: If ploidy is not set, calculation of variant properties like `Type`, `Loh` or `HomoDel` (if they were not set explicitly) won't be possible.
- Type: _Double_
- Limitations: Should be greater than 0
- Example: `2`

**`Purity`** - Sample purity (TCC) percentage of tumor cells in the tissue.
- Type: _Double_
- Limitations: Should be greater than 0
- Example: `95`

**`Variants`** - Copy number variants found in the sample during the analysis.
- Type: _Array_
- Element type: _Object([CNV](https://github.com/dkfz-unite/unite-genome-feed/blob/main/Docs/api-models-cnv.md#cnv))_
- Limitations: If set, should contain at leas one element
- Example: `[{...}, {...}]`

## CNV
Copy number variant (CNV) data.

**`Chromosome`*** - Chromosome.
- Type: _String_
- Possible values: `"1"`, ..., `"22"`, `"X"`, `"Y"`
- Example: `"5"`

**`Start`*** - Start position.
- Type: _Integer_
- Limitations: Greater than 0
- Example: `65498712`

**`End`*** - End position.
- Type: _Integer_
- Limitations: Greater than `Start`
- Example: `65608792`

**`Type`** - Copy number alteration type.
- Note: If not set, the api will try to calculate the value from `Tcn` and sample `Ploidy`.
- Type: _String_
- Possible values: `"Gain"`, `"Loss"`, `"Neutral"`
- Example: `"Loss"`

**`Loh`** - Loss of heterozygosity.
- Note: If not set, the api will try to calculate the value from `C1`, `C2` and sample `Ploidy`.
- Type: _Boolean_
- Example: `true`

**`HomoDel`** - Homozygous deletion.
- Note: If not set, the api will try to calculate the value from `C1`, `C2` and sample `Ploidy`.
- Type: _Boolean_
- Example: `false`

**`C1Mean`** - Estimated mean number of copies at **major** allele (calculated from number of reads and sample ploidy). 
- Type: _Double_
- Limitations: Greater or equal to 0
- Example: `1.1265`

**`C2Mean`** - Estimated mean number of copies at **minor** allele (calculated from number of reads and sample ploidy). 
- Type: _Double_
- Limitations: Greater or equal to 0
- Example: `0.0378`

**`TcnMean`** - Estimated mean total number of copies (`C1Mean` + `C2Mean`). 
- Type: _Double_
- Limitations: Greater or equal to 0
- Example: `1.1643`

**`C1`** - Rounded number of copies at **major** allele (Rounded `C1Mean`).
- Note: If not set, the api will try to calculate the value from `C1Mean` with [threshold rule](https://github.com/dkfz-unite/unite-genome-feed/blob/main/Docs/api-models-cnv.md#threshold-rule).
- Limitations: Greater or equal to `0` or `-1` if not precise enough ([threshold rule](https://github.com/dkfz-unite/unite-genome-feed/blob/main/Docs/api-models-cnv.md#threshold-rule))
- Type: _Integer_
- Example: `1`

**`C2`** - Rounded number of copies at **minor** allele (Rounded `C2Mean`).
- Note: If not set, the api will try to calculate the value from `C2Mean` with [threshold rule](https://github.com/dkfz-unite/unite-genome-feed/blob/main/Docs/api-models-cnv.md#threshold-rule).
- Limitations: Greater or equal to `0` or `-1` if not precise enough ([threshold rule](https://github.com/dkfz-unite/unite-genome-feed/blob/main/Docs/api-models-cnv.md#threshold-rule))
- Type: _Integer_
- Example: `0`

**`TCN`** - Rounded total number of copies (`C1` + `C2`).
- Note: If not set, the api will try to calculate the value from `C1` and `C2` or `TcnMean` with [threshold rule](https://github.com/dkfz-unite/unite-genome-feed/blob/main/Docs/api-models-cnv.md#threshold-rule).
- Limitations: Greater or equal to `0` or `-1` if not precise enough ([threshold rule](https://github.com/dkfz-unite/unite-genome-feed/blob/main/Docs/api-models-cnv.md#threshold-rule))
- Type: _Integer_
- Example: `1`

**`DhMax`** - Esstimated decrease of heterozygosity (calculated from number of reads and sample ploidy).
- Limitations: Greater or equal to 0
- Type: _Integer_
- Example: `1`

#### Type
Type values are:
- `"Gain"` - total number of copies is certainly higher than sample ploidy
- `"Loss"` - total number of copies is certainly lower than sample ploidy
- `"Neutral"` - total number of copies is certainly similar to sample ploidy

#### Threshold Rule
To round double value to integer there is `0.3` certancy threshold applied:
if the value is more than `0.3` far from the nearest integer, it is considered as not precise enought and rounding operation results to `-1`, 
this makes further processing of the value easier for the API.

##
**`*`** - Required fields