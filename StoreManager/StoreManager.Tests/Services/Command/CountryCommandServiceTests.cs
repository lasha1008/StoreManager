using Microsoft.EntityFrameworkCore;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Services;

namespace StoreManager.Tests.Services.Command;

public class CountryCommandServiceTests : CommandUnitTestsBase
{
    public CountryCommandServiceTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [Theory]
    [InlineData("Country 1")]
    [InlineData("Country 2")]
    [InlineData("Country 3")]
    [InlineData("Country 4")]
    public void Insert(string name)
    {
        Country country = GetTestRecord(name);
        CountryCommandService countryCommandService = new(_unitOfWork);
        countryCommandService.Insert(country);

        Assert.NotNull(country.Name);
        Assert.True(country.Id > 0);
    }

    [Theory]
    [InlineData("Country 1")]
    [InlineData("Country 2")]
    [InlineData("Country 3")]
    [InlineData("Country 4")]
    public void NotInserted(string name)
    {
        Country country = GetTestRecord(name);
        country.Id = 1;

        Assert.Throws<ArgumentException>(() =>
        {
            CountryCommandService countryCommandService = new(_unitOfWork);
            countryCommandService.Insert(country);
        });
    }

    [Theory]
    [InlineData("Country 1")]
    [InlineData("Country 2")]
    [InlineData("Country 3")]
    [InlineData("Country 4")]
    public void Update(string name)
    {
        Country newCountry = GetTestRecord(name);

        CountryCommandService countryCommandService = new(_unitOfWork);
        countryCommandService.Insert(newCountry);

        newCountry.Name = $"New {name}";

        countryCommandService.Update(newCountry);

        Country updatedCountry = _unitOfWork.CountryRepository.Set(x => x.Id == newCountry.Id).Single();

        Assert.True(updatedCountry.Name == newCountry.Name);
    }

    [Theory]
    [InlineData("Country 1")]
    [InlineData("Country 2")]
    [InlineData("Country 3")]
    [InlineData("Country 4")]
    public void NotUpdated(string name)
    {
        Country newCountry = GetTestRecord(name);

        Assert.Throws<DbUpdateConcurrencyException>(() =>
        {
            CountryCommandService countryCommandService = new(_unitOfWork);
            countryCommandService.Update(newCountry);
        });
    }

    [Theory]
    [InlineData("Country 1")]
    [InlineData("Country 2")]
    [InlineData("Country 3")]
    [InlineData("Country 4")]
    public void Delete(string name)
    {
        Country country = GetTestRecord(name);

        CountryCommandService countryCommandService = new(_unitOfWork);
        countryCommandService.Insert(country);

        countryCommandService.Delete(country);

        Assert.True(_unitOfWork.CountryRepository.Set(x => x.Id == country.Id).Single().IsDeleted);
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

