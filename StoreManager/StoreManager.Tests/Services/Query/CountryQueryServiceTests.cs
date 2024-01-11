using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Services;

namespace StoreManager.Tests.Services.Query;

public class CountryQueryServiceTests : QueryUnitTestsBase
{
    public CountryQueryServiceTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [Theory]
    [InlineData("Country 1")]
    [InlineData("Country 2")]
    [InlineData("Country 3")]
    [InlineData("Country 4")]
    public void Get(string name)
    {
        Country newCountry = GetTestRecord(name);
        _unitOfWork.CountryRepository.Insert(newCountry);
        _unitOfWork.SaveChanges();

        CountryQueryService countryQueryService = new(_unitOfWork);

        Country retrievedCountry = countryQueryService.Get(newCountry.Id);

        Assert.True(retrievedCountry.Id == newCountry.Id);
    }

    [Theory]
    [InlineData("Country 1")]
    [InlineData("Country 2")]
    [InlineData("Country 3")]
    [InlineData("Country 4")]
    public void Set(string name)
    {
        CountryQueryService countryQueryService = new(_unitOfWork);

        Country newCountry = GetTestRecord(name);
        List<Country> expectedSet = new();
        expectedSet.Add(newCountry);

        _unitOfWork.CountryRepository.Insert(newCountry);
        _unitOfWork.SaveChanges();

        var retrievedCountries = countryQueryService.Set();

        Assert.True(expectedSet.Last().Name == retrievedCountries.Last().Name);
    }

    [Theory]
    [InlineData("Country 1")]
    [InlineData("Country 2")]
    [InlineData("Country 3")]
    [InlineData("Country 4")]
    public void ExpressionSet(string name)
    {
        CountryQueryService countryQueryService = new(_unitOfWork);

        Country newCountry = GetTestRecord(name);
        List<Country> expectedSet = new();
        expectedSet.Add(newCountry);

        _unitOfWork.CountryRepository.Insert(newCountry);
        _unitOfWork.SaveChanges();

        var retrievedCountries = countryQueryService.Set(x => x.Name == name);

        Assert.True(expectedSet.Last().Name == retrievedCountries.Last().Name);
    }
    private static Country GetTestRecord(string name)
    {
        Country country = new()
        {
            Name = name,
        };
        return country;
    }

}
