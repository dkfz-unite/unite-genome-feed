﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Unite.Data.Entities.Donors;
using Unite.Data.Entities.Mutations;
using Unite.Data.Services;
using Unite.Indices.Entities.Mutations;
using Unite.Mutations.Feed.Indices.Services.Mapping.Extensions;

namespace Unite.Mutations.Feed.Indices.Services
{
    public class MutationIndexCreationService
    {
        private readonly UniteDbContext _dbContext;

        public MutationIndexCreationService(UniteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public MutationIndex CreateIndex(int mutationId)
        {
            return CreateMutationIndex(mutationId);
        }


        private Mutation LoadMutation(int mutationId)
        {
            var mutation = _dbContext.Mutations
                .Include(mutation => mutation.AffectedTranscripts)
                    .ThenInclude(affectedTranscript => affectedTranscript.Gene)
                        .ThenInclude(gene => gene.Info)
                .Include(mutation => mutation.AffectedTranscripts)
                    .ThenInclude(affectedTranscript => affectedTranscript.Gene)
                        .ThenInclude(gene => gene.Biotype)
                .Include(mutation => mutation.AffectedTranscripts)
                    .ThenInclude(affectedTranscript => affectedTranscript.Transcript)
                        .ThenInclude(transcript => transcript.Info)
                .Include(mutation => mutation.AffectedTranscripts)
                    .ThenInclude(affectedTranscript => affectedTranscript.Transcript)
                        .ThenInclude(transcript => transcript.Biotype)
                .Include(mutation => mutation.AffectedTranscripts)
                    .ThenInclude(affectedTranscript => affectedTranscript.Consequences)
                        .ThenInclude(affectedTranscriptConsequence => affectedTranscriptConsequence.Consequence)
                .FirstOrDefault(mutation => mutation.Id == mutationId);

            return mutation;
        }

        private MutationIndex CreateMutationIndex(int mutationId)
        {
            var mutation = LoadMutation(mutationId);

            var index = CreateMutationIndex(mutation);

            return index;
        }

        private MutationIndex CreateMutationIndex(Mutation mutation)
        {
            if (mutation == null)
            {
                return null;
            }

            var index = new MutationIndex();

            index.MapFrom(mutation);

            index.Donors = CreateDonorIndices(mutation.Id);

            index.NumberOfDonors = index.Donors
                .Select(donor => donor.Id)
                .Distinct()
                .Count();

            return index;
        }        


        private Donor[] LoadDonors(int mutationId)
        {
            var ids = _dbContext.MutationOccurrences
                .Where(mutationOccurrence => mutationOccurrence.MutationId == mutationId)
                .Select(mutationOccurrence => mutationOccurrence.AnalysedSample.Sample.DonorId)
                .Distinct()
                .ToArray();

            var donors = _dbContext.Donors
                .Include(donor => donor.PrimarySite)
                .Include(donor => donor.ClinicalData)
                    .ThenInclude(clinicalData => clinicalData.Localization)
                .Include(donor => donor.EpigeneticsData)
                .Include(donor => donor.Treatments)
                    .ThenInclude(treatment => treatment.Therapy)
                .Include(donor => donor.DonorWorkPackages)
                    .ThenInclude(workPackageDonor => workPackageDonor.WorkPackage)
                .Include(donor => donor.DonorStudies)
                    .ThenInclude(studyDonor => studyDonor.Study)
                .Where(donor => ids.Contains(donor.Id))
                .ToArray();

            return donors;
        }

        private DonorIndex[] CreateDonorIndices(int mutationId)
        {
            var donors = LoadDonors(mutationId);

            if (donors == null)
            {
                return null;
            }

            var indices = donors
                .Select(donor => CreateDonorIndex(donor, mutationId))
                .ToArray();

            return indices;
        }

        private DonorIndex CreateDonorIndex(Donor donor, int mutationId)
        {
            var index = new DonorIndex();

            index.MapFrom(donor);

            index.Samples = CreateSampleIndices(donor.Id, mutationId);

            return index;
        }


        private AnalysedSample[] LoadSamples(string donorId, int mutationId)
        {
            var samples = _dbContext.AnalysedSamples
                .Include(analysedSample => analysedSample.Sample)
                .Include(analysedSample => analysedSample.Analysis)
                    .ThenInclude(analysis => analysis.File)
                .Include(analysedSample => analysedSample.MatchedSamples)
                    .ThenInclude(matchedSample => matchedSample.Matched)
                        .ThenInclude(analysedSample => analysedSample.Sample)
                .Where(analysedSample =>
                    analysedSample.Sample.DonorId == donorId &&
                    analysedSample.MutationOccurrences.Any(mutationOccurrence => mutationOccurrence.MutationId == mutationId))
                .ToArray();

            return samples;
        }

        private AnalysedSampleIndex[] CreateSampleIndices(string donorId, int mutationId)
        {
            var samples = LoadSamples(donorId, mutationId);

            if (samples == null)
            {
                return null;
            }

            var indices = samples
                .Select(analysedSample => CreateSampleIndex(analysedSample))
                .ToArray();

            return indices;
        }

        private AnalysedSampleIndex CreateSampleIndex(AnalysedSample analysedSample)
        {
            if (analysedSample == null)
            {
                return null;
            }

            var index = new AnalysedSampleIndex();

            index.MapFrom(analysedSample);

            return index;
        }
    }
}