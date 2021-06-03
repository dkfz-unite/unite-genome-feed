﻿using System;
using Unite.Data.Entities.Specimens.Tissues.Enums;

namespace Unite.Mutations.Feed.Web.Models.Mutations
{
    public class TissueModel : SpecimenModel
    {
        public TissueType? Type { get; set; }
        public TumourType? TumourType { get; set; }
        public DateTime? ExtractionDate { get; set; }
        public string Source { get; set; }


        public override void Sanitise()
        {
            base.Sanitise();

            Source = Source?.Trim();
        }
    }
}
