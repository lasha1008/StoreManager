using Microsoft.EntityFrameworkCore;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Tests.Repositories;

public class CountryRepositoryTests : RepositoryUnitTestBase
{
    public CountryRepositoryTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [Theory]
    [InlineData("Country 1")]
    [InlineData("Country 2")]
    [InlineData("Country 3")]
    [InlineData("Country 4")]
    public void Insert(string name)
    {
        Country country = GetTestRecord(name);
        _unitOfWork.CountryRepository.Insert(country);
        _unitOfWork.SaveChanges();

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
            _unitOfWork.CountryRepository.Insert(country);
            _unitOfWork.SaveChanges();
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
        _unitOfWork.CountryRepository.Insert(newCountry);
        _unitOfWork.SaveChanges();

        newCountry.Name = $"New {name}";
        _unitOfWork.CountryRepository.Update(newCountry);
        _unitOfWork.SaveChanges();

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
            _unitOfWork.CountryRepository.Update(newCountry);
            _unitOfWork.SaveChanges();
        });
    }

    [Theory]
    [InlineData("Country 1")]
    [InlineData("Country 2")]
    [InlineData("Country 3")]
    [InlineData("Country 4")]
    public void DeleteByObject(string name)
    {
        Country country = GetTestRecord(name);
        _unitOfWork.CountryRepository.Insert(country);
        _unitOfWork.SaveChanges();

        _unitOfWork.CountryRepository.Delete(country);
        _unitOfWork.SaveChanges();

        Assert.Null(_unitOfWork.CountryRepository.Set(x => x.Id == country.Id).SingleOrDefault());
    }

    [Theory]
    [InlineData("Country 1")]
    [InlineData("Country 2")]
    [InlineData("Country 3")]
    [InlineData("Country 4")]
    public void DeleteById(string name)
    {
        Country country = GetTestRecord(name);
        _unitOfWork.CountryRepository.Insert(country);
        _unitOfWork.SaveChanges();

        _unitOfWork.CountryRepository.Delete(country.Id);
        _unitOfWork.SaveChanges();

        Assert.Null(_unitOfWork.CountryRepository.Set(x => x.Id == country.Id).SingleOrDefault());
    }

    [Theory]
    [InlineData("Country 1")]
    [InlineData("Country 2")]
    [InlineData("Country 3")]
    [InlineData("Country 4")]
    public void GetById(string name)
    {
        Country newCountry = GetTestRecord(name);
        _unitOfWork.CountryRepository.Insert(newCountry);
        _unitOfWork.SaveChanges();

        Country retrievedCountry = _unitOfWork.CountryRepository.Get(newCountry.Id);

        Assert.True(retrievedCountry.Id == newCountry.Id);
    }

    [Theory]
    [InlineData("Country 1")]
    [InlineData("Country 2")]
    [InlineData("Country 3")]
    [InlineData("Country 4")]
    public void Set(string name)
    {
        Country newCountry = GetTestRecord(name);
        List<Country> expectedSet = new();
        expectedSet.Add(newCountry);

        _unitOfWork.CountryRepository.Insert(newCountry);
        _unitOfWork.SaveChanges();

        var retrievedCountries = _unitOfWork.CountryRepository.Set();

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