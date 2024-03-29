﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unite.Genome.Annotations.Clients.Ensembl.Configuration.Options;

namespace Unite.Genome.Annotations.Tests.Integration.Services;

[TestClass]
public class AnnotationsDataLoaderTests
{
    private AnnotationsDataLoader _annotationDataLoader;

    [TestInitialize]
    public void InitializeTest()
    {
        var vepOptions = new VepOptions();
        var ensemblOptions = new EnsemblOptions();

        _annotationDataLoader = new AnnotationsDataLoader(vepOptions, ensemblOptions);
    }


    [TestMethod]
    public void LoadData_ShouldLoadAnnotationsDataForMutations()
    {
        var vepCodes = new string[]
        {
            "1 14142850 14142850 G/A",
            "22 40139912 40139911 -/GA",
            "3 65369232 65369233 AG/-"
        };

        var mutationModels = _annotationDataLoader.LoadData(vepCodes).Result;

        Assert.IsNotNull(mutationModels);
        Assert.AreEqual(mutationModels.Count(), 3);
    }


    private class VepOptions : IEnsemblVepOptions
    {
        public string Host => @"http://localhost:5110";
    }

    private class EnsemblOptions : IEnsemblOptions
    {
        public string Host => @"https://grch37.rest.ensembl.org";
    }
}
