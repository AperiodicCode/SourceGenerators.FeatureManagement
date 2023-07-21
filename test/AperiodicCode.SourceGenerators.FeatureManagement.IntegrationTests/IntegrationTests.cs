namespace AperiodicCode.SourceGenerators.FeatureManagement.IntegrationTests;

public class IntegrationTests
{
    [Fact]
    public void Generated_FeaturesExist()
    {
        Assert.Equal(FeatureConstants.TestFeature1, "TestFeature1");
        Assert.Equal(FeatureConstants.TestFeature2, "TestFeature2");
    }
}
