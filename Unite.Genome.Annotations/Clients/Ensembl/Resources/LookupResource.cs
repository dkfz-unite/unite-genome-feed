﻿using System.Text.Json.Serialization;

namespace Unite.Genome.Annotations.Clients.Ensembl.Resources;

public abstract class LookupResource
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}